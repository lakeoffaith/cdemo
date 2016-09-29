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
    [MarshalAjaxRegister()]
    public partial class __UserList : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");

            scriptFiles.Add("5", "App_Script/UI/UserList.aspx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }

        UserRole meRole = UserRole.User;
        protected void Page_Load(object sender, EventArgs e)
        {
            using (AppDataContext db = new AppDataContext())
            {
                User[] users = db.Users.ToArray();
                meRole = (UserRole)users.Where(_d => _d.Id == me.Id).First().Role;
                userRepeater.DataSource = users;
                userRepeater.DataBind();
            }
            if (meRole != UserRole.Admin)
            {
                addUser.Visible = false;
            }

            if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
            {
                objectNavigator.Visible = true;
            }
            else
            {
                objectNavigator.Visible = false;
            }
        }

        protected void userRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            User user = e.Item.DataItem as User;
            if (user != null)
            {
                SmartLabel userName = (SmartLabel)e.Item.FindControl("userName");
                userName.Text = user.UserName;
                userName.Attributes.Add("id", "user_" + user.Id);

                SmartLabel userRole = (SmartLabel)e.Item.FindControl("userRole");
                switch ((UserRole)user.Role)
                {
                    default:
                    case UserRole.User:
                        userRole.Text = "普通用户";
                        break;
                    case UserRole.Admin:
                        userRole.Text = "管理员";
                        break;
                }

                Anchor edit = (Anchor)e.Item.FindControl("edit");
                Anchor delete = (Anchor)e.Item.FindControl("delete");
                switch (meRole)
                {
                    default:
                    case UserRole.User:
                        //编辑
                        edit.CssClass = "t3";
                        //删除
                        delete.CssClass = "t3";
                        break;
                    case UserRole.Admin:
                        //编辑
                        if (user.Id == me.Id)
                            edit.CssClass = "t3";
                        else
                            edit.Href = "EditUser.aspx?id=" + user.Id;
                        //删除
                        if (user.Id == me.Id)
                            delete.CssClass = "t3";
                        else
                            delete.Href = "javascript:deleteUser(" + user.Id + ");";
                        break;
                }
            }
        }

    }
}
