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
using NetRadio.Data;
using NetRadio.DataExtension;
using System.Text;
namespace NetRadio.LocatingMonitor.Monitor
{
    [AjaxRegister]
    public partial class __PopupEventWindowWithVedio : BasePage
    {
        private int _VedioType = -10000;
        public int VedioType
        {
            get
            {
                if (_VedioType == -10000)
                {
                    _VedioType = Business.BusSystemConfig.GetVedioType();
                }
                return _VedioType;
            }
        }

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
            Ajax.AjaxManager.RegisterClass(typeof(__PopupEventWindowWithVedio));
            vedioShangHai.Visible = false;
            vedioYangZhou.Visible = false;
            vedioFuJian.Visible = false;
            switch (VedioType)
            {
                case 1:
                    vedioYangZhou.Visible = true;
                    tdContent.Style["height"] = "300px";
                    break;
                case 2:
                    vedioFuJian.Visible = true;
                    tdContent.Style["height"] = "300px";
                    break;
                case 3:
                    vedioShangHai.Visible = true;
                    tdContent.Style["height"] = "300px";
                    break;
                default:

                    tdContent.Style["height"] = "400px";
                    break;
            }
            hidUserID.Value = me.Id.ToString();
        }

        [Ajax.AjaxMethod]
        public object GetResult()
        {
            StringBuilder sb = new StringBuilder();

            TagAlert[] tas = null;
            using (AppDataContext db = new AppDataContext())
            {
                tas = db.TagAlerts.Where(t => t.AlertStatus == (byte)AlertStatusType.New)
                    .Where(t => t.HostId > 0)
                    .OrderByDescending(x => x.WriteTime).ToArray();
            }
            string div = "";
            string tagName = "";
            string tagNameHref = "";
            string coordinatesName = "";
            string coordinatesNameHref = "";
            string description = "";
            string time = "";
            string tagIcon = "";
            int firstCoordinateID = 0;
            foreach (TagAlert ev in tas)
            {
                if (firstCoordinateID == 0)
                {
                    firstCoordinateID = ev.CoordinatesId;
                }

                if (ev != null)
                {

                    div = "id_" + ev.AlertId;

                    HostTagGroupStatus hostTag = HostTagGroupStatus.SelectByHostId(ev.HostId);
                    tagName = hostTag.HostName;
                    tagIcon = CommonExtension.IdentityIconByGroupId(hostTag.HostGroupId);





                    //need to open video based on the _alertId
                    if (CommonExtension.IsIlltreatTag(ev.HostId))
                    {
                        if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                        {
                            coordinatesName = hostTag.HostName;
                        }
                        else
                        {

                            int coorid = CommonExtension.GetCoordinatesId(hostTag.Description.Substring(0, hostTag.Description.Length - 2));
                            coordinatesName = Coordinates.GetName(coorid);

                            switch (VedioType)
                            {
                                case 1:
                                    coordinatesNameHref = "javascript:AlertView(" + ev.CoordinatesId + ");";
                                    break;
                                case 2:
                                    coordinatesNameHref = "javascript:AlertView2(" + ev.CoordinatesId + ");";
                                    break;
                                case 3:
                                    coordinatesNameHref = "javascript:AlertView3(" + ev.CoordinatesId + ");";
                                    break;
                                default: break;
                            }
                        }
                    }
                    else
                    {
                        int hosttype = TagStatusView.SelectTagStatus(ev.TagId).HostTag.HostGroupId.Contains(1) ? 1 : 2;
                        tagNameHref = "../TagUsers/TagUser.aspx?id=" + ev.HostId + "&type=" + hosttype;

                        coordinatesName = Coordinates.GetName(ev.CoordinatesId);
                        switch (VedioType)
                        {
                            case 1:
                                coordinatesNameHref = "javascript:AlertView(" + ev.CoordinatesId + ");";
                                break;
                            case 2:
                                coordinatesNameHref = "javascript:AlertView2(" + ev.CoordinatesId + ");";
                                //coordinatesName.Attributes["onclick"] = "AlertView2(" + ev.CoordinatesId + ")";
                                break;
                            case 3:
                                coordinatesNameHref = "javascript:AlertView3(" + ev.CoordinatesId + ");";
                                break;
                            default: break;
                        }

                    }

                    description = CommonExtension.GetEventDescription((SupportEvent)ev.AlertType, ev.HostId);

                    time = ev.WriteTime.ToString();


                    /**********************/
                    sb.AppendFormat(@"
        <tr>
            <td>
                <img src='{0}' Width='13' Height='13'/>
                <a href='{1}' style='font-weight: bold;' target='_blank'>{2}</a> 
            </td>
            <td>
                {3}
            </td>
            <td>
                <span>{4}</span>
            </td>
            <td>
                <span>{5}</span>
            </td>
            <td>  
                <div id='{6}'>                          
		    <a onclick='javascript:setResolved(this);' href='#' style='width:35px; height:19px;' title='点此按钮后该事件将标记为已确认,页面将跳转到事件处理页面'>[确认]</a>
                </div>
            </td>
        </tr>
               ", tagIcon, tagNameHref, tagName, coordinatesNameHref.Length > 0 ? "<a href='" + coordinatesNameHref + "' style='font-weight: bold;'>" + coordinatesName + "</a>" : coordinatesName, description, time, div);
                    /**********************/
                }
            }//foreach


            if (sb.Length > 0)
            {
                sb.Insert(0, "<table cellpadding=0 cellspacing=0 border=0>");
                sb.Append("</table>");
            }
            else
            {
                sb.Append("<br/>暂时没有发现报警。。。&nbsp;&nbsp;【<span onclick=\" window.opener=null;window.open('','_self');window.close();\" style=\"cursor:pointer;\">关闭窗口</span>】<br/><br/>");
            }

            switch (VedioType)
            {
                case 1:
                    return new { html = sb.ToString(), callBackFunction = "loadfirst(" + firstCoordinateID + ")" };

                case 2:
                    return new { html = sb.ToString(), callBackFunction = "loadfirst2(" + firstCoordinateID + ")" };

                default:
                    return new { html = sb.ToString(), callBackFunction = "" };
            }

        }

        [AjaxMethod(RequireSessionState.True)]
        public static object GetHCUrl(int coordinatesId)
        {
            string url = "";
            int iChannel = 0;
            string urlref = "";
            int iChannelref = 0;
            using (AppExtensionDataContext db = new AppExtensionDataContext())
            {
                CoordinatesCamera coordinate = db.CoordinatesCameras.SingleOrDefault(c => c.CoordinatesId == coordinatesId);
                if (coordinate != null)
                {
                    url = coordinate.CameraIP;
                    iChannel = coordinate.iChannel;
                    urlref = coordinate.CameraIPlref;
                    iChannelref = coordinate.iChannelref;






                }
            }

            return new
            {
                url,
                iChannel,
                urlref,
                iChannelref
            };
        }


        /// <summary>
        /// 通过标签ID获取定位点摄像头和当前标签当前状态的相关资料，add by lyz ( 福建 )
        /// </summary>
        /// <param name="tagID">标签ID</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public object GetAnchorCamera(int coordinatesID)
        {
            bool CoordinatesHasVedio = false;
            AnchorCamera[] coordinate = null;

            using (AppDataContext db = new AppDataContext())
            {
                coordinate = db.AnchorCameras.Where(c => c.CoordinateID == coordinatesID).ToArray();
                if (coordinate != null && coordinate.Length > 0)
                {
                    CoordinatesHasVedio = true;
                }
                else
                {
                    CoordinatesHasVedio = false;
                    coordinate = null;
                }
            }

            return new
            {
                CoordinatesHasVedio,
                coordinate
            };

        }


        /// <summary>
        /// 通过标签ID获取定位点摄像头和当前标签当前状态的相关资料，add by lyz ( 上海 )
        /// </summary>
        /// <param name="tagID">标签ID</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public object GetAnchorCameraShangHai(int coordinatesID)
        {
            bool CoordinatesHasVedio = false;
            NetRadio.Data.AnchorCameraShangHai[] coordinate = null;

            using (AppDataContext db = new AppDataContext())
            {
                coordinate = db.AnchorCameraShangHais.Where(c => c.CoordinateID == coordinatesID).ToArray();
                if (coordinate != null && coordinate.Length > 0)
                {
                    CoordinatesHasVedio = true;
                }
                else
                {
                    
                    CoordinatesHasVedio = false;
                    coordinate = null;
                }
            }

            return new
            {
                CoordinatesHasVedio,
                coordinate
            };

        }


    }
}
