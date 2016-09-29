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
namespace NetRadio.LocatingMonitor.Member
{
    public partial class __Action : BasePage
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
            string behavior = Fetch.QueryUrl("behavior").ToLower();
            switch (behavior)
            {
                default:
                    {                       
                        ShowMessagePage("未知操作，请返回。");
                        break;
                    }

                case "logoff":
                case "logout":
                    {
                        //清除所有缓存
                        ClearCache(me);
                        ClearSession();
                        Session["LoginUser"] = null;
                        if (me.IsGuest)
                        {
                            Terminator.Redirect(PathUtil.ResolveUrl("Member/Login.aspx"));
                        }
                        else
                        {
                            me.Logout();
                            Terminator.Redirect(PathUtil.ResolveUrl("Member/Login.aspx"));
                           
                        }
                        break;
                    }
            }
        }

        /// <summary>
        /// 覆盖登录状态的检查
        /// </summary>
        public override void DemandLogin()
        {
        }
    }
}