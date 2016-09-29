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
    public partial class __ChangePassword : BasePage
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
           // this.Title = this.WebSiteLabel = "修改用户密码";
            //base.Sitemap.Text2 = "系统设置";
            //base.Sitemap.Text3 = this.Wrap.Title;

            if (!IsPostBack)
            {
                ClearMenuSelectedStatus();
                loginName.Text = me.Name;
                messageLabel.Text = "";
            }
        }

        protected void submitPassword_Click(object sender, EventArgs e)
        {
            if (password.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入原密码。");
                return;
            }
            if (newPassword.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入新密码。");
                return;
            }
            if (newPassword.Text != newPassword.Text.Trim())
            {
                feedbacks.Items.AddError("密码首尾不能输入空格。");
                return;
            }
            if (newPassword.Text != confirmPassword.Text)
            {
                feedbacks.Items.AddError("新密码两次输入的密码不一致。");
                return;
            }
            if (me.Password != Strings.MD5(password.Text))
            {
                feedbacks.Items.AddError("原密码错误。");
                return;
            }

            Data.User.ChangePassword(me.Id, Strings.MD5(newPassword.Text.Trim()));
            me.Logout();

            //记录日志
            Diary.Insert(me.Id, 0, 0, "设置了新密码。 ");

            string message = "密码修改成功，请重新登录。 "
                + "<a href=\"../Member/Action.aspx?behavior=logout\" target=\"_top\"><u>退出登录</u></a>";

            messageLabel.Text = message;
            

        }

        protected void cancelChange_Click(object sender, EventArgs e)
        {
            Terminator.Redirect(PathUtil.ResolveUrl("Home.aspx"));
        }
    }
}
