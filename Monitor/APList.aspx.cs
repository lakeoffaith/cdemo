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
using System.Net.Mail;
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __APList : BasePage
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
                this.LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            this.SetSortButtonPresentation();
            this.LoadRepeater();
        }

        private SortButton _activatedSorter;

        private void LoadRepeater()
        {
            //LocatingServiceUtil.DemandLocatingService();
            try
            {
                int totalCount;
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                APStatusView[] apStatus = serviceApi.SelectAPStatusList(
                                       null,
                                       null,
                                       _activatedSorter.SortKey,
                                       _activatedSorter.SortDirection,
                                       p.PageSize,
                                       p.RecordOffset,
                                       out totalCount
                                   );

                if (_activatedSorter.SortDirection == SortDirection.Ascending)
                {
                    if (_activatedSorter.SortKey == "APName")
                    {
                        apStatus = apStatus.OrderBy(x => x.APName).ToArray();
                    }
                    else
                    {
                        apStatus = apStatus.OrderBy(x => x.UpdateTime).ToArray();
                    }
                }

                else
                {
                    if (_activatedSorter.SortKey == "APName")
                    {
                        apStatus = apStatus.OrderByDescending(x => x.APName).ToArray();
                    }
                    else
                    {
                        apStatus = apStatus.OrderByDescending(x => x.UpdateTime).ToArray();
                    }
                }
                apStatus = apStatus.Where(a => !a.Mac.Substring(0, 11).Contains("00:55:52:48")).ToArray();
                p.RecordCount = totalCount;//2010-11-17bydyp
                apList.DataSource = apStatus.ToList();
                apList.DataBind();

            }
            catch
            {
                //new PrettyTerminator().End("Locating Service 远程支持服务程序未启动，无法打开该页面。");
            }
        }

        #region refresher_Refresh

        protected void refresher_Refresh(object sender, EventArgs e)
        {
            this.LoadDefaultView();
        }

        #endregion

        #region SetSortButtonPresentation

        private void SetSortButtonPresentation()
        {
            string sortField = Fetch.QueryUrl("sortField");
            int sortDirection = Fetch.QueryUrlAsIntegerOrDefault("sortDirection", 0);

            if (sortField.Length == 0)
            {
                apNameSorter.Activated = true;
                _activatedSorter = apNameSorter;
                return;
            }

            SortButton[] sortButtons = { apNameSorter, updateTimeSorter };
            foreach (var button in sortButtons)
            {
                if (button.SortKey == sortField)
                {
                    button.Activated = true;
                    button.SortDirection = (SortDirection)sortDirection;
                    _activatedSorter = button;
                    continue;
                }
                button.Activated = false;
            }
        }

        #endregion

        #region sorter_Click

        protected void sorter_Click(object sender, EventArgs e)
        {
            var button = (SortButton)sender;
            if (button.Activated)
            {
                button.SwitchSortDirection();
            }

            Terminator.Redirect(Request.Path + "?pid=" + Request.QueryString["pid"] + "&sortField=" + button.SortKey + "&sortDirection=" + (byte)button.SortDirection);
        }

        #endregion

        #region apList_ItemCreated

        protected void apList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            APStatusView ap = e.Item.DataItem as APStatusView;
            if (ap != null)
            {
                Anchor apName = (Anchor)e.Item.FindControl("apName");
                apName.Text = ap.APName;
                //apName.Href = "../Objects/AP.aspx?id=" + ap.APId;

                ((SmartLabel)e.Item.FindControl("apMac")).Text = ap.Mac;
                ((SmartLabel)e.Item.FindControl("apSsid")).Text = ap.Ssid;
                ((SmartLabel)e.Item.FindControl("apLanIP")).Text = ap.LanIP;

                ((SmartLabel)e.Item.FindControl("apLocateEnabled")).Text = ap.APLocatorStatus == 0 ? "失败" : (ap.APLocatorStatus == 1 ? "正常" : (ap.APLocatorStatus == 2 ? "运行" : "停止"));
                
                ((DateTimeLabel)e.Item.FindControl("updateTime")).DisplayValue = ap.UpdateTime;
            }
        }

        #endregion
    }
}
