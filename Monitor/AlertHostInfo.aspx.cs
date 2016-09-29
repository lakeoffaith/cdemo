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
//using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.Monitor
{

    public partial class __AlertHostInfo : BasePage
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
        public __AlertHostInfo()
        {
            //_tagId = Fetch.QueryUrlAsInteger("id");
            _tagId = RequestEx.QueryStringAsInt("id");
        }

        int _tagId;
        int _id;
        TagAlert _tagAlert;
        protected void Page_Load(object sender, EventArgs e)
        {
            aaaaaaaaaa.Visible = false;
            ProcessAlert1.Visible = false;


            Ajax.AjaxManager.RegisterClass(typeof(__AlertHostInfo));
            this.LoadTagStatus();

            NetRadio.Data.TagAlert a;

            NetRadio.Data.AppDataContext db = new AppDataContext();
            TagAlert ta = db.TagAlerts.Where(_d => _d.TagId == _tagId && _d.AlertStatus == 1).FirstOrDefault();
            if (ta != null)
            {
                aaaaaaaaaa.Visible = true;
                ProcessAlert1.Visible = true;
                ProcessAlert1.AlertID = ta.AlertId;
                //_id = ta.AlertId;// Fetch.QueryUrlAsInteger("id");

                //LoadTagAlert();

                //if (!IsPostBack)
                //    LoadCopList();
            }




            AlertCoordinates.Value = "";
        }

        protected void LoadTagStatus()
        {
            //if (!LocatingServiceUtil.IsAvailable())
            //{
            //    ShowMessagePage("LocatingService未启动，无法获得即时状态。");
            //    return;
            //}

            switch (BusSystemConfig.GetVedioType())
            {
                case 0:
                default:
                    video.Visible = false;
                    CollapseView_Video.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView",
                       "AlertView();window.setInterval(function(){AlertView(); },2000);",
                       true);
                    break;
                case 1:
                    video.Visible = true;
                    CollapseView_Video.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView",
                        "AlertView();window.setInterval(function(){AlertView(); },2000);",
                        true);
                    break;
                case 2:
                    video.Visible = false;
                    CollapseView_Video.Visible = true;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "AlertView2",
                       "AlertView2();window.setInterval(function(){AlertView2(); },2000);",
                       true);
                    break;
            }
        }

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
