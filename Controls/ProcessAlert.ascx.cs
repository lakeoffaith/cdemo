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
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.Text;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __ProcessAlert : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__ProcessAlert));
        }

        struct NameID
        {
            public string Name;
            public int ID;
        }
        public int AlertID
        {
            set;
            get;
        }
        [Ajax.AjaxMethod]
        public static string GetProcessTable(int alertID)
        {

            string tab1 = "";
            string tab2 = "";
            string tab3 = "";
            string tab4 = "";

            StringBuilder sb = new StringBuilder();


            using (AppDataContext db = new AppDataContext())
            {
                foreach (AlertProcessLog ev in db.AlertProcessLogs.Where(t => t.AlertId == alertID).OrderBy(t => t.UpdateTime).ToList())
                {
                    if (ev != null)
                    {
                        if (ev.UserId > 0)
                            tab1 = Data.User.Select(ev.UserId) == null ? "未知" : Data.User.Select(ev.UserId).UserName;
                        else
                            tab1 = "未知";

                        tab2 = Misc.GetAlertStatus((AlertStatusType)ev.AlertStatus);
                        tab3 = ev.ChangeReason;
                        tab4 = ev.UpdateTime.ToString("yyyy/MM/dd HH:mm:ss");


                        sb.AppendFormat(@"
                                        <tr>
                                        <td>
                                            {0}
                                        </td>
                                        <td>
                                           {1}
                                        </td>
                                        <td>
                                          {2}
                                        </td>                                        
                                        <td>
                                           {3}
                                        </td>
                                    </tr>", tab1, tab2, tab3, tab4);
                    }


                }
                if (sb.Length > 0)
                {
                    sb.Insert(0, @"<table class=""grid alternate fixed"" border=""0"" cellspacing=""0"" cellpadding=""0"">
                                <thead>    
                                <th width=200>处理人</th>
                                <th width=200>报警状态</th>
                                <th width=200>处理结果</th>
                                <th width=200>处理时间</th>
                                </thead>
                            ");
                    sb.Append(" </table>");
                }
            }
            return sb.ToString();
        }
        [Ajax.AjaxMethod]
        public static object GetData(int alertID)
        {
            string id_name = "";
            string id_position = "";
            string id_type = "";
            string id_time = "";
            string id_table = "";
            NameID[] id_selectResult = null;

            using (AppDataContext db = new AppDataContext())
            {
                TagAlert _tagAlert = db.TagAlerts.SingleOrDefault(t => t.AlertId == alertID);

                if (_tagAlert == null)
                {
                    throw new Exception("报警事件不存在!");
                }
                else
                {
                    HostTag thisHostTag = HostTag.GetById(_tagAlert.HostId);
                    Tag thisTag = Tag.Select(thisHostTag.TagId);
                    if (thisTag != null)
                    {

                        if (CommonExtension.IsIlltreatTag(_tagAlert.HostId))
                        {
                            if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                            {
                                id_position = thisHostTag.HostName;
                            }
                            else
                            {

                                int coorid = CommonExtension.GetCoordinatesId(thisHostTag.Description.Substring(0, thisHostTag.Description.Length - 2));
                                id_position = Coordinates.GetName(coorid);

                            }
                        }
                        else
                        {
                            id_position = Coordinates.GetName(_tagAlert.CoordinatesId);
                        }


                        //if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                        //{
                        //    id_position = thisHostTag.HostName;
                        //}
                        //else
                        //{

                        //    int coorid = CommonExtension.GetCoordinatesId(thisHostTag.Description.Substring(0, thisHostTag.Description.Length - 2));
                        //    id_position = Coordinates.GetName(coorid);

                        //}

                        if (LocatingServiceUtil.IsAvailable())
                        {
                            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                            bool boolean = serviceApi.ClearTagStatus(thisTag.TagMac, (SupportEvent)_tagAlert.AlertType);
                        }

                        id_name = thisHostTag.HostName;
                        //id_position = Coordinates.GetName(coorid);
                        id_type = CommonExtension.GetEventDescription((SupportEvent)_tagAlert.AlertType, _tagAlert.HostId);
                        id_time = _tagAlert.WriteTime.ToString("yyyy/MM/dd HH:mm:ss");

                        id_selectResult = db.ProcessResults.Select(_d => new NameID { ID = _d.ID, Name = _d.Text }).ToArray();

                        id_table = GetProcessTable(alertID);


                        return new
                        {
                            id_name,
                            id_position,
                            id_type,
                            id_time,
                            id_table,
                            id_selectResult
                        };

                    }
                }
            }




            return "";
        }


        [Ajax.AjaxMethod]
        public static bool ProcessAlertFun(int alertID, int _value, int processID, string processName)
        {
            TagAlert.UpdateStatusByAlertId(alertID, AlertStatusType.Resolved);
            string reason = "";
            if (_value == 1)
            {
                reason = "确认报警，并 " + processName;
            }
            else if (_value == 0)
            {
                reason = "误报";
            }
            AlertProcessLog.Insert(alertID, ContextUser.Current.Id, AlertStatusType.Resolved, reason);
            return true;
        }
    }
}