using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ajax;
using System.Text;

using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using Summer;
using NetRadio.Model;
using NetRadio.Business;
using System.Data;

namespace NetRadio.LocatingMonitor.Organize
{
    public partial class __Patrol_ShangHai : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {

            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/SelectTagUser.ascx.js");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__Patrol_ShangHai));

            if (!IsPostBack)
            {
                fromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                fromHour.SelectedIndex = 0;
                toDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                toHour.SelectedIndex = 23;

                tagSelector.selectedGroupName.SelectedGroupIdArray = new int[] { 1 };
                tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers6);
            }
        }
        [Ajax.AjaxMethod]
        public static string GetHTML(int pageIndex, string userids, string begintime, string endtime)
        {
            AppDataContext db = new AppDataContext();
            string sql = "select * from history_TagPositionLog";
            List<PatrolLog> patlog = Patrol.FigerOut();
            int recordcount = Convert.ToInt32(patlog.Count);
            int pageCount = 0;
            NetRadio.LocatingMonitor.PageData pd = NetRadio.LocatingMonitor.__Pager.GetPageData(pageIndex, recordcount);
            DataTable dt = Summer.QueryExtension.GetPageDataTableForMSSQL("LocatingMonitor", sql, pageIndex, pd.PageSize, out pageCount);
          
           
            var tt = (from _d in patlog
                      join _d1 in db.HostTags
                          on _d.HostId equals _d1.HostId
                      join _d2 in db.Coordinates
                          on _d.StartCoordinateId equals _d2.Id 
                      select new
                      {

                          _d1.HostName,
                         _d2.CoordinatesName,
                          _d.PatrolStartTime,
                          _d.PatrolEndTime,
                          _d.PatrolReturnTime,
                          _d.EndCoordinateId,
                          _d.TimeInteval,
                          _d.PointsCount
                        
                          
                      }

                        ).AsEnumerable();
            
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <table cellpadding=""0"" cellspacing=""0"" class=""grid alternate fixed"">
                <thead class=""category"">
                     <th width=""100"" style=""text-align: center"">
                       警察名称
                     </th>
                     <th  width=""200"" style=""text-align: center;"">
                        起始位置
                     </th>
                        <th  width=""150""  style=""text-align: center;"">
                        起始时间
                     </th>
                     
                    <th  width=""100""  style=""text-align: center;"">
                        终点位置
                     </th>
                    <th  width=""150""  style=""text-align: center;"">
                      返回到起点时间
                     </th>
                 <th  width=""100""  style=""text-align: center;"">
                       巡逻时间
                     </th>  
                   
                  
                </thead>
            ");
            var q =
                   from _d in tt
                   select new
                   {
                     name=_d.HostName,
                     coorname=_d.CoordinatesName,
                    sttime=_d.PatrolStartTime,
                    endId=_d.EndCoordinateId,
                    endtime=_d.PatrolEndTime,
                    returntime=_d.PatrolReturnTime,
                    timeInteval=_d.TimeInteval,
                    pointCount=_d.PointsCount
                   
                    
                   };

            foreach (var item in q)
            {
               
                sb.AppendFormat(@"
                <tr>
                    <td width=""100""  style=""text-align: center"">
                         {0} 
                    </td>
                    <td   style=""text-align: center;"">  
                           {1}
                    </td>
                    <td   style=""text-align: center;"">  
                           {2}
                    </td>
                     <td   style=""text-align: center;"">  
                           {3}
                    </td>
                    <td   style=""text-align: center;"">  
                           {4}
                    </td>
                    <td   style=""text-align: center;"">  
                           {5}
                    </td>
               
                    
                    
                </tr>
            ", item.name, item.coorname, item.sttime,Coordinates.GetName(item.endId),item.returntime,item.timeInteval);

            }

            if (sb.Length == 0)
            {
                sb.AppendFormat(@"
                <tr>
                    <td colspan=""4"">                
                       无数据记录
                    </td>
                </tr>");
            }
            sb.Append(@"</table>");
            sb.Append("<br />" + pd.HtmlCode);
            return sb.ToString();
        }
    }
}
