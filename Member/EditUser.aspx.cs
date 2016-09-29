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
    public partial class __EditUser : BasePage
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
        public __EditUser()
        {
            _userId = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
            _editMode = Fetch.QueryUrl("action") == "addnew" ? EditMode.AddNew : EditMode.Modify;
        }

        readonly int _userId;
        readonly EditMode _editMode;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (me.Id == _userId)
            {
                ShowMessagePage("没无法编辑自己, 请返回。");
            }
           

            //Sitemap.Text2 = "系统设置";
            //Sitemap.Text3 = "用户列表";
            //Sitemap.Url3 = "UserList.aspx";
            //Sitemap.Text4 = this.Wrap.Title = (_editMode == EditMode.AddNew ? "新增" : "编辑") + "用户";

            if (!Page.IsPostBack)
            {
                LoadDefaultView();
            }
        }

        void LoadDefaultView()
        {
            if (_editMode == EditMode.AddNew)
            {
                submit.Text = "新增";
                userName.ReadOnly = readonlyMark.Visible = false;
                passwordHint.Visible = false;
                userRole.SelectedIndex = 0;
            }

            else
            {
                var user = Data.User.Select(_userId);
                if (user == null)
                {
                    ShowMessagePage("用户不存在。");
                }

                submit.Text = "编辑";
                userName.Text = user.UserName;
                userName.ReadOnly = readonlyMark.Visible = true;
                passwordHint.Visible = true;
                ListItem listItem = userRole.Items.FindByValue(user.Role.ToString());
                if (listItem != null)
                {
                    userRole.ClearSelection();
                    listItem.Selected = true;
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            // Check
            if (userName.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入登录名。");
            }
            if (_editMode == EditMode.AddNew && password.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入密码。");
            }
            if (password.Text != password.Text.Trim())
            {
                feedbacks.Items.AddError("密码首尾不能输入空格。");
            }
            if (password.Text != confirmationPassword.Text)
            {
                feedbacks.Items.AddError("两次输入的密码不一致。");
            }
            if (feedbacks.HasItems)
            {
                return;
            }

            if (_editMode == EditMode.AddNew && Data.User.IsNameExsit(userName.Text.Trim()))
            {
                feedbacks.Items.AddError("用户名已经存在，请换一个。");
                return;
            }

            // Save
            var user = new User
            {
                UserName = userName.Text.Trim(),
                Password = password.Text.Trim().Length == 0 ? null : Strings.MD5(password.Text.Trim()),
                Role = int.Parse(userRole.SelectedValue)
            };

            if (_editMode == EditMode.AddNew)
            {
                Data.User.Insert(user);

                //记录日志
                Diary.Insert(me.Id, 0, 0, "新增系统用户: " + user.UserName);

                Terminator.Redirect(PathUtil.ResolveUrl("Member/UserList.aspx"));
            }
            else
            {
                Data.User.UpdateById(_userId, user.Password, user.Role);

                //记录日志
                Diary.Insert(me.Id, 0, 0, "修改系统用户" + user.UserName + "的信息。");

                if (me.Id == _userId && password.Text.Length > 0)
                {
                  
                }
                feedbacks.Items.AddPrompt("保存完毕。");
            }
        }
    }
}
