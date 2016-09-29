using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
namespace NetRadio.LocatingMonitor.History
{
    public partial class __SystemSecurityLog : BasePage
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
            if (!Page.IsPostBack)
            {
                this.LoadRepeater();

                if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                {
                    ObjectNavigator.Visible = true;
                }
                else
                {
                    ObjectNavigator.Visible = false;
                }
            }
            NetRadio.Data.User oUser = NetRadio.Data.User.SelectByUserName(me.Name);
            if (oUser.Role < (int)UserRole.Admin)
                deleteButtonRow.Visible = false;
            else
                deleteButtonRow.Visible = true;

        }

        private void LoadRepeater()
        {
            using (AppDataContext db = new AppDataContext())
            {
                IList<DiaryView> listViews = db.DiaryViews
                    .Where(x => x.HostId > 0)
                    .OrderByDescending(x => x.Id).ToList();

                p.RecordCount = listViews.Count;
                repeater.DataSource = listViews.Skip(p.RecordOffset).Take(p.PageSize)
                    .ToList();
                repeater.DataBind();
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadRepeater();
        }

        protected void submitDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> idList = Strings.ParseToArray<int>(Request.Form["selection"]);
            if (idList.Count() > 0)
            {
                SecurityLog.DeleteMany(idList.ToArray());
            }
            LoadRepeater();
        }

        protected void repeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DiaryView log = e.Item.DataItem as DiaryView;
            if (log != null)
            {
                SmartLabel selection = (SmartLabel)e.Item.FindControl("selection");
                NetRadio.Data.User oUser = NetRadio.Data.User.SelectByUserName(me.Name);
                if (oUser.Role >= (int)UserRole.Admin)
                {
                    //selection.Text = "<input type='checkbox' name='selection' value='" + log.Id + "' />";
                }
                else
                {
                    //selection.Text = "-";
                }

                if (log.UserId != 0)
                {
                    SmartLabel userName = (SmartLabel)e.Item.FindControl("userName");
                    userName.Text = log.UserName;
                }

                SmartLabel description = (SmartLabel)e.Item.FindControl("description");
                description.Text = log.Action;

                //SmartLabel hostName = (SmartLabel)e.Item.FindControl("hostName");
                //hostName.Text = log.HostName;

                SmartLabel tagMac = (SmartLabel)e.Item.FindControl("tagMac");
                tagMac.Text = log.TagMac;

                DateTimeLabel writeTime = (DateTimeLabel)e.Item.FindControl("writeTime");
                writeTime.DisplayValue = log.WriteTime;
            }
        }
    }
}
