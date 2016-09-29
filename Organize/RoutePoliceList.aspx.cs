using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ajax;
using System.Text;
using System.Data;
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

namespace NetRadio.LocatingMonitor.Organize
{
    public partial class __RoutePoliceList : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {

            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__RoutePoliceList));

            if (!IsPostBack)
            {

            }
        }
        [Ajax.AjaxMethod]
        public static string GetHTML(int pageIndex)
        {
            NetRadio.Data.AppDataContext db0 = new AppDataContext();
            //NetRadio.DataExtension.AppExtensionDataContext db = new AppExtensionDataContext();
            var cs = db0.Coordinates.ToArray();

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <table cellpadding=""0"" cellspacing=""0"" class=""grid alternate fixed"">
                <thead class=""category"">
                     <th width=""100"" style=""text-align: center"">
                       警察名称
                     </th>
                     <th  style=""text-align: left;"">
                        轨迹的定位点
                     </th>
                     <th width=""100"" style=""text-align: center"">
                      操作
                     </th>
                </thead>
            ");


            string sql = @"
                    select pg.*,p.HostName as policeName  from 
                    (select * from Plugin_GoupCoordinates where GoupUseful=1) pg 
                    left join
                    (select h.* from object_HostTag h,object_HostGroup g where g.hostid=h.hostid and g.hostgroupid = 1)p
                    on(pg.ArraginID=p.HostId) 
                  ";
            int recordCount = Convert.ToInt32(Summer.Query.RunQuerySQLString(String.Format("select count(*) from ({0})g", sql), "LocatingMonitor").Rows[0][0]);
            int pageCount = 0;
            NetRadio.LocatingMonitor.PageData pd = NetRadio.LocatingMonitor.__Pager.GetPageData(pageIndex, recordCount);
            DataTable dt = Summer.QueryExtension.GetPageDataTableForMSSQL("LocatingMonitor", sql, pageIndex, pd.PageSize, out pageCount);

            var q =
                    from _d in dt.AsEnumerable()
                    select new
                    {
                        id = Convert.ToInt32(_d["id"]),
                        policeName = _d["policeName"].ToString(),
                        pointArray = Strings.ParseToArray<int>(_d["PointArray"].ToString(), new char[] { ',' })
                    };


            foreach (var order in q)
            {
                sb.AppendFormat(@"
                <tr>
                    <td width=""100""  style=""text-align: center"">
                         {0} 
                    </td>
                    <td   style=""text-align: left"">  
                           {1}
                    </td>
                    <td width=""100"" style=""text-align: center"">                
                           {2}
                    </td>
                </tr>
            ", order.policeName, cs.Where(_d => order.pointArray.Contains(_d.Id)).Select(_d1 => _d1.CoordinatesName).ToArray().ConnectAsString("，"), "<a href='javascript:__delete(" + order.id + ");'>删除</a>");

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


        [Ajax.AjaxMethod]
        public static bool DeleteData(int id)
        {
            GoupCoordinate.DeleteById(id);
            return true;
        }
    }
}
