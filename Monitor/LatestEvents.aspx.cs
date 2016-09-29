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
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __LatestEvents : BasePage
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
        private static string _zSortKey = "WriteTime";
        private static SortDirection _sortDir = SortDirection.Descending;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
            }
            
            timePoint.Text = me.TagEventApprizingTimePoint.ToString("G");
        }

        private void LoadDefaultView()
        {
            this.SetSortButtonPresentation();
            this.LoadRepeater();
        }

        private void LoadRepeater()
        {
            using (AppDataContext db = new AppDataContext())
            {

                if (Session["TagLogFilter"] != null)
                {
                    tagLogFilter = (NetRadio.LocatingMonitor.Controls.__TagLogFilter)Session["TagLogFilter"];
                    string tagFilterCondition = tagLogFilter.ConditionDescription;
                }

                //CommonExtension.ClearPoliceInvalidAlert();

                //var query = db.DBViewTagAlerts.Where(t => t.AlertStatus == (int)AlertStatusType.New
                //    || t.AlertStatus == (int)AlertStatusType.Processing)
                //    .Where(t=>t.HostId>0)
                //    .OrderBy(t=>t.AlertStatus)
                //    .ThenByDescending(t=>t.WriteTime);


                IList<DBViewTagAlert> lAlerts = db.DBViewTagAlerts.Where(t => t.AlertStatus == (int)AlertStatusType.New || t.AlertStatus == (int)AlertStatusType.Processing)
                    .Where(t => t.HostId > 0).ToList();
                if (tagLogFilter.TagNameKeyword != null && tagLogFilter.TagNameKeyword.Trim().Length > 0)
                {
                    lAlerts = lAlerts.Where(t => t.HostName.Contains(tagLogFilter.TagNameKeyword.Trim())).ToList();
                }

                if (tagLogFilter.FromTime.ToString().Trim() != "")
                {
                    lAlerts = lAlerts.Where(t => t.WriteTime >= tagLogFilter.FromTime).ToList();
                }

                if (tagLogFilter.ToTime.ToString().Trim() != "")
                {
                    lAlerts = lAlerts.Where(t => t.WriteTime <= tagLogFilter.ToTime).ToList();
                }
                int[] groupIds = new int[2] { (int)TagUserType.Cop, (int)TagUserType.Culprit };
                var range = HostTagGroupStatus.GetCoveredHostIdArray(groupIds);
                var query = lAlerts.Where(x => range.Contains((int)x.HostId)).ToList();


                if (_zSortKey == "HostName")
                {
                    if (_sortDir == SortDirection.Ascending)
                        query = query.OrderBy(t => t.HostName).ThenByDescending(t => t.WriteTime).ToList();
                    else
                        query = query.OrderByDescending(t => t.HostName).ThenByDescending(t => t.WriteTime).ToList();
                }
                else if (_zSortKey == "WriteTime")
                {
                    if (_sortDir == SortDirection.Ascending)
                        query = query.OrderBy(t => t.WriteTime).ToList();
                    else
                        query = query.OrderByDescending(t => t.WriteTime).ToList();
                }
                else
                {
                    query = query.OrderByDescending(t => t.WriteTime).ToList();
                }

                if (query != null)
                {
                    p.RecordCount = query.Count();

                    messageRepeater.DataSource = query.Skip(p.RecordOffset).Take(p.PageSize).ToList();
                    messageRepeater.DataBind();
                }
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadRepeater();
        }

        //protected void setResolved_Click(object sender, EventArgs e) {
        //    IEnumerable<int> idList = Strings.ParseToArray<int>(Request.Form["selection"]);
        //    if (idList.Count() > 0) {
        //        using (AppDataContext db = new AppDataContext()) {
        //            var alerts = db.TagAlerts.Where(x => idList.Contains(x.AlertId)).ToList();
        //            foreach (var item in alerts)
        //            {
        //                //TagStatusView.ChangeStatus(TagStatusView.SelectTagStatus(item.TagId).Mac, (SupportEvent)item.AlertType, EventStatus.Cleared);
        //                //if need to notify server???
        //                TagAlert.UpdateStatusByAlertId(item.AlertId, AlertStatusType.Resolved);
        //                AlertProcessLog.Insert(item.AlertId, me.Id, AlertStatusType.Resolved, "已处理");

        //            }
        //        }
        //    }
        //    me.TagEventApprizingTimePoint = DateTime.Now;
        //    Response.Redirect(Fetch.CurrentUrl);
        //}

        protected void messageRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DBViewTagAlert ev = e.Item.DataItem as DBViewTagAlert;
            if (ev != null)
            {
                HostTagGroupStatus hostTagGroupStatus = HostTagGroupStatus.SelectByHostId((int)ev.HostId);


                //SmartLabel isResolved = (SmartLabel)e.Item.FindControl("isResolved");
                //if (ev.AlertStatus == (byte) AlertStatusType.Processing) {
                //    isResolved.Text = "√";
                //}
                //else {
                //    isResolved.Text = "<input type='checkbox' name='selection' value='" + ev.AlertId + "' />";
                //}
                Image icon = e.Item.FindControl("icon") as Image;

                int groupId = hostTagGroupStatus.HostGroupId;
                icon.ImageUrl = CommonExtension.IdentityIconByGroupId(groupId);

                Anchor tagName = (Anchor)e.Item.FindControl("tagName");
                tagName.Text = ev.HostName;
                tagName.Href = PathUtil.ResolveUrl(string.Format("/TagUsers/TagUser.aspx?id={0}&type={1}", ev.HostId, groupId));
                SmartLabel facilityName = (SmartLabel)e.Item.FindControl("facilityName");
                facilityName.Text = ev.FacilityName;

                SmartLabel coordinatesName = (SmartLabel)e.Item.FindControl("coordinatesName");
                coordinatesName.Text = ev.CoordinatesName;

                SmartLabel eventType = (SmartLabel)e.Item.FindControl("eventType");
                eventType.Text = CommonExtension.GetEventDescription((SupportEvent)ev.AlertType, ev.HostId.Value);

                SmartLabel alertStatus = (SmartLabel)e.Item.FindControl("alertStatus");
                alertStatus.Text = Misc.GetAlertStatus((AlertStatusType)ev.AlertStatus);

                DateTimeLabel lastHappenTime = (DateTimeLabel)e.Item.FindControl("lastHappenTime");
                lastHappenTime.DisplayValue = ev.WriteTime;

                Anchor alertDetail = (Anchor)e.Item.FindControl("alertDetail");
                alertDetail.Text = "详情";
                alertDetail.Href = PathUtil.ResolveUrl("/Monitor/TagAlertProcess.aspx?id=" + ev.AlertId);

                if (ev.AlertStatus == (byte)ResolveFlag.Processed)
                {
                    HtmlTableRow tr = (HtmlTableRow)e.Item.FindControl("tr");
                    tr.Attributes["class"] = "t3";
                    tagName.CssClass = "t3";
                }
            }
        }

        #region SetSortButtonPresentation

        private void SetSortButtonPresentation()
        {
            SortButton[] sortButtons = { hostNameSorter, updateTimeSorter };
            foreach (var button in sortButtons)
            {
                if (button.SortKey == _zSortKey)
                {
                    button.Activated = true;
                    button.SortDirection = _sortDir;
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
            _zSortKey = button.SortKey;
            _sortDir = button.SortDirection;

            Terminator.Redirect(Request.Path);
        }

        #endregion

    }
}
