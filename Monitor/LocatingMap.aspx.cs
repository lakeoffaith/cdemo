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
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __LocatingMap : BasePage
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
        int _facilityMapId;

        protected int FacilityMapId
        {
            get
            {
                return _facilityMapId;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            _facilityMapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);
            if (_facilityMapId == -1)
            {
                var facility = Facility.All.Where(f => f.MapId > 0).OrderBy(f => f.Id).FirstOrDefault();
                if (facility == null)
                {
                    ShowMessagePage("目前还没有地图，请先用 Site Survey 工具建立并上传地图。");
                }
                _facilityMapId = facility.MapId;
            }
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Sitemap.Text2 = "实时监控";
            //Sitemap.Text3 = "地图定位监控";

            //LocatingServiceUtil.DemandLocatingService();
            if (!LocatingServiceUtil.IsAvailable())
            {
                ShowMessagePage("LocatingService未启动，无法获得即时状态。");
            }
            switch (Business.BusSystemConfig.GetVedioType())
            {
                case 1:
                    vedioYangZhou.Visible = true;
                    vedioDemo.Visible = false;
                    break;
                case 2:
                    vedioYangZhou.Visible = false;
                    vedioDemo.Visible = true;
                    break;
                default:
                    divVedio.Visible = false;
                    break;
            }
        }
    }
}
