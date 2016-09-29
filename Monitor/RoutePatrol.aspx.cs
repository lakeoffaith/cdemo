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
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __RoutePatrol : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {

            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__RoutePatrol));
        }
        [Ajax.AjaxMethod]
        public static string GetHTML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <table cellpadding=""0"" cellspacing=""0"" class=""grid alternate fixed"">
                <thead class=""category"">
                     <th width=""200"" style=""text-align: center"">
                        ab
                    </th>
                         <th width=""200"" style=""text-align: center"">
                        cd
                    </th>
                         <th style=""text-align: center"">
                        ef
                    </th>
                </thead>
            ");
            int i = 0;
            for (i = 0; i < 10; i++)
            {
                sb.AppendFormat(@"
                <tr>
                    <td style=""text-align: center"">
                       {0}
                    </td>
                    <td style=""text-align: center"">               
                       {1}
                    </td>
                    <td style=""text-align: center"">                
                       {2}
                    </td>
                </tr>
            ", 1, 2, 3);
            }


            if (i == 0)
            {
                sb.AppendFormat(@"
                <tr>
                    <td colspan=""3"">                
                       无数据记录
                    </td>
                </tr>");
            }
            sb.Append(@"</table>");
            return sb.ToString();
        }
    }
}
