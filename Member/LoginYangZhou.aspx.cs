using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Data;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Web;
namespace NetRadio.Web.Member
{
    public partial class __LoginYangZhou : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearMenuCookies();
            //清除所有缓存
            ClearCache(me);
            ClearSession();
        
        }
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void submit_Click(object sender, EventArgs e)
        {
            if (userName.Text.Trim().Length == 0 || password.Text.Length == 0)
            {
                MessageBox.Show(this, "登录名和密码都必须输入。");
                //Terminator.Alert("登录名和密码都必须输入。");
                return;
            }

            string result = me.ApplyLogin(userName.Text.Trim(), Strings.MD5(password.Text), false);
            if (result.Length == 0)
            {
                //记录日志
                Diary.Insert(ContextUser.Current.Id, 0, 0, userName.Text.Trim() + "登录成功。");
                NetRadio.Data.User oUser = NetRadio.Data.User.SelectByUserName(userName.Text.Trim());
                Session["LoginUser"] = oUser;
                Response.Redirect("../Home.aspx");
                //Terminator.Redirect(PathUtil.ResolveUrl("Default.aspx"));
            }
            else
            {
                //记录日志
                Diary.Insert(ContextUser.Current.Id, 0, 0, userName.Text.Trim() + "登录失败。");
                MessageBox.Show(this, result);
                //Terminator.Alert(result);
            }
        }
        /// <summary>
        /// 覆盖登录状态的检查
        /// </summary>
        public override void DemandLogin()
        {
        }
        //取消页面权限验证
        protected override bool CheckRight()
        {
            return true;
        }
    }
}
