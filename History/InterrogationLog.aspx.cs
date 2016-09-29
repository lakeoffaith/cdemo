using System;
using System.Collections;
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
    public partial class __InterrogationLog : BasePage
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
        private TagUserType _userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Title = this.WebSiteLabel = "提审记录";
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 0);
            if (!IsPostBack)
            {
                LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            //Sitemap.Text2 = "历史事件";
            //Sitemap.Text3 = this.Wrap.Title;

            historyNavigator.AppendSearchConditionQueryString(string.Format("userType={0}", (int)_userType));
            this.LoadRepeater();

            //historyNavigator.AppendSearchConditionQueryString(tagLogFilter.ConditionQueryString);
            // this.LoadRepeater();
        }

        private void LoadRepeater()
        {
            using (AppExtensionDataContext db = new AppExtensionDataContext())
            {
                repeater.DataSource = db.InterrogationLogs.Where(t => t.PoliceId > 0)
                    .OrderByDescending(x => x.StartTime)
                    .Skip(p.RecordOffset).Take(p.PageSize)
                    .ToList();
                repeater.DataBind();
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadRepeater();
        }

        protected void repeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            InterrogationLog log = e.Item.DataItem as InterrogationLog;

            SmartLabel policeName = e.Item.FindControl("policeName") as SmartLabel;
            SmartLabel culpritName = e.Item.FindControl("culpritName") as SmartLabel;
            SmartLabel status = e.Item.FindControl("status") as SmartLabel;
            DateTimeLabel StartTime = e.Item.FindControl("StartTime") as DateTimeLabel;
            DateTimeLabel EndTime = e.Item.FindControl("EndTime") as DateTimeLabel;
            User user = Data.User.Select(log.PoliceId);
            if (user != null)
                policeName.Text = Data.User.Select(log.PoliceId).UserName;
            culpritName.Text = HostTag.GetById(log.CulpritId).HostName;
            StartTime.DisplayValue = log.StartTime;
            if (log.EndTime != null)
            {
                EndTime.DisplayValue = log.EndTime.Value;
                status.Text = "提审完成";
            }
            else
            {
                status.Text = "提审中";
            }
        }
    }
}
