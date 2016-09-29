using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Controls;
using System.Web.UI.HtmlControls;
using NetRadio.Data;
using NetRadio.DataExtension;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Web;

namespace NetRadio.LocatingMonitor.History
{
    public partial class __PositionAlert : BasePage
    {
        private static string _zSortKey = "";
        private static SortDirection _sortDir = SortDirection.Descending;
        private TagUserType _userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            _userType = TagUserType.Position;
            if (!Page.IsPostBack)
            {
                LoadDefaultView();
            }
        }

        protected void LoadDefaultView()
        {
            this.SetSortButtonPresentation();
            historyNavigator.AppendSearchConditionQueryString(tagLogFilter.ConditionQueryString);
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

                IList<DBViewTagAlert> lAlerts = db.DBViewTagAlerts.Where(t => t.AlertType==(byte)SupportEvent.ButtonPressed && (t.AlertStatus == (int)AlertStatusType.Ignored || t.AlertStatus == (int)AlertStatusType.Resolved || t.AlertStatus == (int)AlertStatusType.Closed))
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
                int[] groupIds = new int[1] { (int)_userType };
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

        protected void messageRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DBViewTagAlert ev = e.Item.DataItem as DBViewTagAlert;
            if (ev != null)
            {

                SmartLabel isResolved = (SmartLabel)e.Item.FindControl("isResolved");
                if (ev.AlertStatus != (byte)AlertStatusType.New)
                {
                    isResolved.Text = "√";
                }
                else
                {
                    isResolved.Text = "<input type='checkbox' name='selection' value='" + ev.AlertId + "' />";
                }
                Img icon = e.Item.FindControl("icon") as Img;
                HostTagGroupStatus hostTagGroupStatus = HostTagGroupStatus.SelectByHostId((int)ev.HostId);
                int groupId = hostTagGroupStatus.HostGroupId;
                icon.Src = CommonExtension.IdentityIconByGroupId(groupId);

                Anchor tagName = (Anchor)e.Item.FindControl("tagName");
                tagName.Text = ev.HostName;
                tagName.Href = PathUtil.ResolveUrl(string.Format("/TagUsers/TagUser.aspx?id={0}&type={1}", ev.HostId, groupId));

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

            Terminator.Redirect(Request.Path + "?userType=" + ((int)_userType).ToString());
        }

        #endregion

    }
}
