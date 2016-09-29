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
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using Summer;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.Organize
{
    /// <summary>
    /// 区域分组设置，画面。lyz
    /// </summary>
    public partial class __MapAreaGroup : BasePage
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
            if (!IsPostBack)
            {
                Ajax.AjaxManager.RegisterClass(typeof(__MapAreaGroup));
            }
        }

        [Ajax.AjaxMethod]
        public string get_collapseView()
        {
            return BaseUserControl.GetControlHTML<__MapAreaGroup0>(Web.WebPath.GetFullPath("Organize/MapAreaGroup0.ascx"));
        }

        [Ajax.AjaxMethod]
        public object get_MapAreas(string groupID)
        {
            return BusAreaGroup.GetMapAreas(groupID);
        }

        [Ajax.AjaxMethod]
        public void set_AreaGroup(string groupID, string newAreaIDs)
        {
            BusAreaGroup.SetAreaGroup(groupID, newAreaIDs);
        }
        
    }
}