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
namespace NetRadio.LocatingMonitor.Report
{
    public partial class __PoliceStayTimeUI : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/ReportIndex.aspx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sitemap.Text2 = "统计报表";

            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
            }

            //this.DecorateAreaDropList();
        }

        //private void DecorateAreaDropList() {
        //    foreach (ListItem item in areaList.Items) {
        //        if (item.Value == "0") {
        //            item.Attributes.CssStyle.Add("color", "gray");
        //        }
        //    }
        //}

        private void LoadDefaultView()
        {
            fromDate.Text = DateTime.Today.AddDays(-1).ToString("yyyy-M-d");
            toDate.Text = DateTime.Today.ToString("yyyy-M-d");

            areaList.Items.Clear();
            var allArea = MapArea.All.OrderBy(x => x.MapId).OrderBy(x => x.AreaName);
            //var mapId = -1;

            foreach (var area in allArea)
            {
                //if (mapId != area.MapId) {
                //	mapId = area.Id;
                //	areaList.Items.Add(new ListItem(Facility.GetNameByMapId(mapId), "0"));
                //}
                areaList.Items.Add(new ListItem(area.AreaName, area.Id.ToString()));	//"　└ " + 
            }
            areaList.Items.Insert(0, new ListItem("所有累计", "0"));
        }
    }
}
