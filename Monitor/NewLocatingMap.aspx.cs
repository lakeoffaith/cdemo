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
using NetRadio.Business;


namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __NewLocatingMap : BasePage
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
            Ajax.AjaxManager.RegisterClass(typeof(__NewLocatingMap));
            //LocatingServiceUtil.DemandLocatingService();
            if (!LocatingServiceUtil.IsAvailable())
            {
                ShowMessagePage("LocatingService未启动，无法获得即时状态。");
            }

            video.Visible = true;
        }

        //[Ajax.AjaxMethod]
        //public bool LoadTagStatus(int tagId)
        //{
        //    if (!LocatingServiceUtil.IsAvailable())
        //    {
        //        //ShowMessagePage("LocatingService未启动，无法获得即时状态。");
        //        return false;
        //    }
        //    switch (BusSystemConfig.GetVedioType())
        //    {
        //        case 0:
        //        default:
        //            video.Visible = false;
        //            CollapseView_Video.Visible = false;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView",
        //               "AlertView();window.setInterval(function(){AlertView(); },2000);",
        //               true);
        //            break;
        //        case 1:
        //            video.Visible = true;
        //            CollapseView_Video.Visible = false;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView",
        //                "AlertView();window.setInterval(function(){AlertView(); },2000);",
        //                true);
        //            break;
        //        case 2:
        //            video.Visible = false;
        //            CollapseView_Video.Visible = true;
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView2",
        //               "AlertView2();window.setInterval(function(){AlertView2(); },2000);",
        //               true);
        //            break;
        //    }

        //    return true;
        //}

        /// <summary>
        /// 通过标签ID获取定位点摄像头和当前标签当前状态的相关资料，add by lyz ( 扬州 )
        /// </summary>
        /// <param name="tagID">标签ID</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public object GetCoordinatesAndTagInfo(int tagID)
        {
            if (!LocatingServiceUtil.IsAvailable())
            {
                return null;
            }

            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
            TagStatusView tagStatusView = serviceApi.SelectTagStatus(tagID);

            int coordinatesID = 0;
            string hostName = "";
            string coordinates = "";
            string tagStatus = "";
            bool SystemHasVedio = BusSystemConfig.GetVedioType() == 1;
            bool CoordinatesHasVedio = false;
            string url = "";
            int iChannel = 0;
            string urlref = "";
            int iChannelref = 0;


            if (tagStatusView != null)
            {
                hostName = string.Format("<a href=\"{0}\">{1}</a>", Web.WebPath.GetFullPath("TagUsers/TagUser.aspx?id=" + tagStatusView.HostTag.HostId), tagStatusView.HostTag.HostName);
                coordinates = tagStatusView.CoordinatesName;
                coordinatesID = tagStatusView.CoordinatesId;
                //tagStatus.Text = LocatingMonitorUtils.GetAllTagEventsDescription(tagStatusView);
                tagStatus = LocatingMonitorUtils.GetAllTagEventsDescription(tagStatusView, tagStatusView.HostTag.HostGroupId.Min(), 10, "Master/WebItem.Master");

                if (tagStatus.Length == 0)
                {
                    tagStatus = "正常";
                }


                if (SystemHasVedio)
                {
                    using (AppExtensionDataContext db = new AppExtensionDataContext())
                    {
                        CoordinatesCamera coordinate = db.CoordinatesCameras.SingleOrDefault(c => c.CoordinatesId == coordinatesID);
                        if (coordinate != null)
                        {
                            CoordinatesHasVedio = true;
                            url = coordinate.CameraIP;
                            iChannel = coordinate.iChannel;
                            urlref = coordinate.CameraIPlref;
                            iChannelref = coordinate.iChannelref;
                        }
                    }
                }
                return new
                {
                    //------位置、状态-----
                    hostName,
                    coordinates,
                    tagStatus,
                    coordinatesID,
                    //------是否有视频-----
                    SystemHasVedio,
                    CoordinatesHasVedio,
                    //------摄像头信息-----                    
                    url,
                    iChannel,
                    urlref,
                    iChannelref,
                    VedioType = NetRadio.Business.BusSystemConfig.GetVedioType()

                };
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 通过标签ID获取定位点摄像头和当前标签当前状态的相关资料，add by lyz ( 福建 )
        /// </summary>
        /// <param name="tagID">标签ID</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public object GetAnchorCameraAndTagInfo(int tagID)
        {
            if (!LocatingServiceUtil.IsAvailable())
            {
                return null;
            }

            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
            TagStatusView tagStatusView = serviceApi.SelectTagStatus(tagID);

            int coordinatesID = 0;
            string hostName = "";
            string coordinates = "";
            string tagStatus = "";
            bool SystemHasVedio = BusSystemConfig.GetVedioType() == 2;
            bool CoordinatesHasVedio = false;
            string url = "";
            int iChannel = 0;
            string urlref = "";
            int iChannelref = 0;
            AnchorCamera[] coordinate = null;

            if (tagStatusView != null)
            {
                hostName = string.Format("<a href=\"{0}\">{1}</a>", Web.WebPath.GetFullPath("TagUsers/TagUser.aspx?id=" + tagStatusView.HostTag.HostId), tagStatusView.HostTag.HostName);
                coordinates = tagStatusView.CoordinatesName;
                coordinatesID = tagStatusView.CoordinatesId;
                //tagStatus.Text = LocatingMonitorUtils.GetAllTagEventsDescription(tagStatusView);
                tagStatus = LocatingMonitorUtils.GetAllTagEventsDescription(tagStatusView, tagStatusView.HostTag.HostGroupId.Min(), 10, "Master/WebItem.Master");

                if (tagStatus.Length == 0)
                {
                    tagStatus = "正常";
                }


                if (SystemHasVedio)
                {
                    using (AppDataContext db = new AppDataContext())
                    {
                        coordinate = db.AnchorCameras.Where(c => c.CoordinateID == coordinatesID).ToArray();
                        if (coordinate != null && coordinate.Length > 0)
                        {
                            CoordinatesHasVedio = true;
                            //coordinate = null;
                            //url = coordinate.CameraIP;
                            //iChannel = coordinate.iChannel;
                            //urlref = coordinate.CameraIPlref;
                            //iChannelref = coordinate.iChannelref;
                        }
                        else
                        {
                            CoordinatesHasVedio = false;
                            coordinate = null;
                        }
                    }
                }
                return new
                {
                    //------位置、状态-----
                    hostName,
                    coordinates,
                    tagStatus,
                    coordinatesID,
                    //------是否有视频-----
                    SystemHasVedio,
                    CoordinatesHasVedio,
                    //------摄像头信息-----
                    coordinate,
                    VedioType = NetRadio.Business.BusSystemConfig.GetVedioType()
                    //url,
                    //iChannel,
                    //urlref,
                    //iChannelref
                };
            }
            else
            {
                return null;
            }
        }

    }
}
