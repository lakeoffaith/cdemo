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
    public partial class __ReportIndex : BasePage
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
        private static int _totalCount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Title = this.WebSiteLabel = "报表统计";
            //Sitemap.Text2 = "统计报表";

            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
            }

            this.DecorateAreaDropList();
        }

        private void DecorateAreaDropList()
        {
            //foreach (ListItem item in areaList.Items) {
            //    if (item.Value == "0") {
            //        item.Attributes.CssStyle.Add("color", "gray");
            //    }
            //}
        }

        private void LoadDefaultView()
        {
            //fromDate.Text = DateTime.Today.AddDays(-1).ToString("yyyy-M-d");
            //toDate.Text = DateTime.Today.ToString("yyyy-M-d");

            //areaList.Items.Clear();
            //var allArea = MapArea.All.OrderBy(x => x.AreaName).OrderBy(x => x.MapId);
            //var mapId = -1;

            //foreach (var area in allArea) {
            //    if (mapId != area.MapId) {
            //        mapId = area.MapId;
            //        areaList.Items.Add(new ListItem(Facility.GetNameByMapId(mapId), area.Id.ToString()));
            //    }
            //    areaList.Items.Add(new ListItem("　└ " + area.AreaName, area.Id.ToString()));
            //}
            //areaList.Items.Insert(0, new ListItem("-- 选择地图区域 --", "0"));
            _totalCount = 0;

            list.DataSource = Facility.All;
            list.DataBind();


            StatTime.Text = DateTime.Now.ToString();
            TotalCount.Text = _totalCount.ToString();
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Facility f = (Facility)e.Item.DataItem;
            SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
            SmartLabel headCount = e.Item.FindControl("headCount") as SmartLabel;

            facilityName.Text = f.FacilityName;

            IList<TagStatusView> tagList = new List<TagStatusView>();
            string _keyword = "";
            //GTang 20101122 修改为统计hostGroupid=1,2
            //int[] _hostGroupArray = new int[1] { 1 };
            int[] _hostGroupArray = new int[2] { 1, 2 };
            int totalCount = 0;

            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
            if (serviceApi != null && LocatingServiceUtil.IsAvailable())
            {
                TagStatusView tagStatusView = new TagStatusView();
                tagList = serviceApi.SelectTagStatusList(
                _keyword,
                _hostGroupArray,
                f.MapId,
                true,
                false, //SupportEvent.Absent),
                false, //SupportEvent.BatteryInsufficient),
                false, //SupportEvent.AreaEvent),
                false, //SupportEvent.ButtonPressed),
                false, //SupportEvent.WristletBroken),
                "",
                SortDirection.Ascending,
                0,//only get total count
                0,
                out totalCount);

                headCount.Text = totalCount.ToString();
                _totalCount += totalCount;
            }
        }
    }
}
