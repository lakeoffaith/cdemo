using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.Text;
namespace NetRadio.LocatingMonitor.Report
{
    public partial class __PoliceJobSize : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js"); ;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__PoliceJobSize));
            fromDate.Text = DateTime.Today.ToString("yyyy-M-d");
            fromHour.SelectedValue = "0";
            fromMinute.Text = "00";
            toDate.Text = DateTime.Today.ToString("yyyy-M-d");
            toHour.SelectedValue = "23";
            toMinute.Text = "59";
        }
        [Ajax.AjaxMethod]
        public string GetJobSize(string beginTime, string endTime)
        {
            #region 新算法
            string _res = "";
            try
            {
                DateTime _beginTime = DateTime.Parse(beginTime);
                DateTime _endTime = DateTime.Parse(endTime);

                IList<InterrogationLog> interrogationLogs = new List<InterrogationLog>();
                IList<TagPositionLog> positionReconds = new List<TagPositionLog>();
                Dictionary<int, int> policeJobAmount = new Dictionary<int, int>();
                List<int> interrogationRooms = new List<int>();
                Dictionary<InterrogationLog, int> interrogationItems = new Dictionary<InterrogationLog, int>();
                using (AppExtensionDataContext dbExtension = new AppExtensionDataContext())
                {
                    interrogationLogs = dbExtension.InterrogationLogs.Where(t => t.StartTime > _beginTime && t.EndTime < _endTime).ToList();
                }

                using(AppDataContext db=new AppDataContext())
                {
                    interrogationRooms = db.LocationGroups.Where(t => t.GroupId == (int)PrisonCoordinatesGroup.InterrogateRoom).Select(t => t.LocationId).ToList();
                    policeJobAmount = db.HostPositionStatusViews.ToList().Where(u => u.TagId>0 && u.HostGroupId == (byte)TagUserType.Cop).ToDictionary(u => u.HostId, u => u.HostGroupId - 1);
                    positionReconds = db.TagPositionLogs.Where(t => t.WriteTime > _beginTime && t.WriteTime < _endTime && interrogationRooms.Contains(t.CoordinatesId)).ToList();

                }

                foreach (var item in interrogationLogs)
                {
                    int interrogationRoomId = 0;

                    TagPositionLog culpritOutItem = positionReconds.Where(t => t.WriteTime > item.StartTime && t.HostId == item.CulpritId).OrderBy(t => t.WriteTime).FirstOrDefault();//culpritOutItem表示犯人从监区带出，而非从提审室外出
                    interrogationRoomId = culpritOutItem.CoordinatesId;
                    
                    if (culpritOutItem != null && culpritOutItem.HostId > 0)
                    {
                        TagPositionLog outItem = positionReconds.Where(t =>t.CoordinatesId==interrogationRoomId && t.WriteTime > item.StartTime).OrderBy(t => t.WriteTime).FirstOrDefault();
                        if (outItem != null && policeJobAmount.Keys.Contains(outItem.HostId.Value))
                        {
                            policeJobAmount[outItem.HostId.Value]++;
                        }

                        TagPositionLog inItem = positionReconds.Where(t => t.CoordinatesId == interrogationRoomId && t.WriteTime > item.EndTime).OrderBy(t => t.WriteTime).FirstOrDefault();

                        if (inItem != null && policeJobAmount.Keys.Contains(inItem.HostId.Value))
                        {
                            policeJobAmount[inItem.HostId.Value]++;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                Dictionary<HostTag, int> JobAmountresult = new Dictionary<HostTag, int>();

                using (AppDataContext db = new AppDataContext())
                {
                    var query = from a in policeJobAmount
                                from b in db.HostTags
                                where a.Key == b.HostId
                                select new
                                {
                                    b,
                                    a.Value
                                };

                    JobAmountresult = query.OrderByDescending(t => t.Value).ToDictionary(t => t.b, t => t.Value);
                }

                StringBuilder sb = new StringBuilder("");
                sb.AppendFormat(@" 
                       <table style='width: 100%;' cellpadding=0 cellspacing=0 border=0 class='grid alternate fixed'>
                        <thead class=category>
                            <th style='text-align: center;'>
                                警员名称
                            </th>
                            <th style='text-align: center;'>
                                警员编号
                            </th>
                            <th style='text-align: center;'>
                                警员工作量
                            </th>
                        </thead>
                        ");

                if (JobAmountresult.Count > 0)
                {
                    foreach (var item in JobAmountresult)
                    {
                        sb.AppendFormat(@"                                
                                 <tr>
                                    <td style='text-align: center;'>
                                        {0}
                                    </td>
                                    <td style='border-left: solid 1px #e9e9e9;text-align: center;'>
                                        {1}
                                    </td>
                                    <td style='border-left: solid 1px #e9e9e9;text-align: center;'>
                                        {2}
                                    </td>
                                 </tr>
                                ", item.Key.HostName, item.Key.HostExternalId, item.Value);
                    }
                }
                else
                {
                    sb.AppendFormat(@"                                
                                 <tr>
                                    <td colspan=3>
                                       无数据！！！
                                    </td>
                                 </tr>"
                               );
                }
                sb.AppendFormat(@"</table>");
                _res = sb.ToString();
            }
            catch (Exception e1)
            {
                _res = e1.Message;
            }
            return _res;

            #endregion

            #region 原算法
//            string _res = "";
//            try
//            {
//                DateTime _beginTime = DateTime.Parse(beginTime);
//                DateTime _endTime = DateTime.Parse(endTime);
//                SqlParameter p1 = new SqlParameter("@BeginTime", _beginTime);
//                SqlParameter p2 = new SqlParameter("@EndTime", _endTime);
//                DataSet ds = Summer.Query.RunProcedure("GetPoliceJobSize", new SqlParameter[2] { p1, p2 }, "LocatingMonitor");
//                StringBuilder sb = new StringBuilder("");
//                sb.AppendFormat(@" 
//                       <table style='width: 100%;' cellpadding=0 cellspacing=0 border=0 class='grid alternate fixed'>
//                        <thead class=category>
//                            <th style='text-align: center;'>
//                                警员名称
//                            </th>
//                            <th style='text-align: center;'>
//                                警员编号
//                            </th>
//                            <th style='text-align: center;'>
//                                警员工作量
//                            </th>
//                        </thead>
//                        ");
//                bool hasData = false;
//                if (ds != null && ds.Tables.Count == 1)
//                {
//                    foreach (DataRow log in ds.Tables[0].Rows)
//                    {
//                        sb.AppendFormat(@"                                
//                                 <tr>
//                                    <td style='text-align: center;'>
//                                        {0}
//                                    </td>
//                                    <td style='border-left: solid 1px #e9e9e9;text-align: center;'>
//                                        {1}
//                                    </td>
//                                    <td style='border-left: solid 1px #e9e9e9;text-align: center;'>
//                                        {2}
//                                    </td>
//                                 </tr>
//                                ", log["HostName"], log["HostExternalid"], log["JobSize"]);
//                        hasData = true;
//                    }
//                }
//                if (!hasData)
//                {
//                    sb.AppendFormat(@"                                
//                                 <tr>
//                                    <td colspan=3>
//                                       无数据！！！
//                                    </td>
//                                 </tr>"
//                               );
//                }

//                sb.AppendFormat(@"</table>");
//                _res = sb.ToString();
//            }
//            catch (Exception e1)
//            {
//                _res = e1.Message;
//            }
//            return _res;
            #endregion

        }
    }
}
