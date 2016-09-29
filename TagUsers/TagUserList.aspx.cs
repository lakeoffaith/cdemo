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
using System.Net;
using System.IO;
using NetRadio.LocatingMonitor.Controls;
namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __TagUserList : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {

            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/SelectTag.ascx.js");
            scriptFiles.Add("6", "App_Script/Control.js");
            scriptFiles.Add("7", "App_Script/UI/SelectStrongestRssiTag.ascx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Ajax.AjaxManager.RegisterClass(typeof(__TagUserList));
                Ajax.AjaxManager.RegisterClass(typeof(__SelectTag));
                Ajax.AjaxManager.RegisterClass(typeof(__SelectStrongestRssiTag));
            }
        }

        protected TagUserType GetTagUserType(int userType)
        {
            switch (userType)
            {
                case 1:
                case 2:
                case 3:
                    return (TagUserType)userType;
                default:
                    throw new Exception("无此用户类型");
            }
        }

        [Ajax.AjaxMethod]
        public string get_TagUserList0(int actionNum, int userType)
        {
            return BaseUserControl.GetControlHTML<__TagUserList0>(Web.WebPath.GetFullPath("TagUsers/TagUserList0.ascx"), actionNum, userType);
        }

        [Ajax.AjaxMethod]
        public string get_TagUserList1(int actionNum, int userType, string _keyword, string _extandId, int _jailRoomSelectedIndex, string _jailRoomSelectedValue, string tagBindingSelectedValue, string tagOnlineSelectedValue, int pageIndex)
        {
            return BaseUserControl.GetControlHTML<__TagUserList1>(Web.WebPath.GetFullPath("TagUsers/TagUserList1.ascx"), actionNum, userType, _keyword, _extandId, _jailRoomSelectedIndex, _jailRoomSelectedValue, tagBindingSelectedValue, tagOnlineSelectedValue, pageIndex);
        }


        [Ajax.AjaxMethod]
        public bool ImportUsers()
        {
            int totalCount = 0;
            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                GeneralResult gr = serviceApi.LoadHostInfo();
                CulpritRoomReference.RemoveCaching();
                if (!gr.Suceess)
                    throw new Exception(gr.ErrText);
            }
            return true;
        }
    }
}