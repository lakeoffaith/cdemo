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
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __ReplayRoute : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");

            scriptFiles.Add("5", "App_Script/UI/ReplayRoute.aspx.js");
            scriptFiles.Add("6", "App_Script/UI/SelectTagUser.ascx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }

        private static string _zSortKey = " ID";
        private static SortDirection _sortDir = SortDirection.Descending;

        public __ReplayRoute()
        {
            //int n = Fetch.QueryUrlAsIntegerOrDefault("type", -1);
            //switch (n) {
            //    case 1:
            //    case 2:
            //        _userType = (TagUserType)n;
            //        break;

            //    default:
            //        ShowMessagePage("访问无效.");
            //        break;
            //}
        }

        //TagUserType _userType;
        protected Model.TagUser[] GetTagUser()
        {
            return tagSelector.SelectedTagUsers;
        }

        //[Ajax.AjaxMethod]
        //public bool IsFutureTime(string shortDate, int hour, int minute, int second, int addMinute)
        //{
        //    try
        //    {
        //        DateTime dt = Convert.ToDateTime(shortDate);
        //        dt = dt.AddHours(hour);
        //        dt = dt.AddMinutes(minute);
        //        dt = dt.AddSeconds(second);
        //        dt = dt.AddMinutes(addMinute);
        //        if (dt.Ticks > DateTime.Now.Ticks)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [Ajax.AjaxMethod]
        public int IsFutureTime(string beginTime, string endTime)
        {

            DateTime dt0 = DateTime.MinValue;
            DateTime dt1 = DateTime.MinValue;
            try
            {
                dt0 = Convert.ToDateTime(beginTime);
            }
            catch
            {
                throw new Exception("开始时间格式错误");
            }
            try
            {
                dt1 = Convert.ToDateTime(endTime);
            }
            catch
            {
                throw new Exception("结束时间格式错误");
            }

            if (dt0.Ticks > dt1.Ticks)
            {
                throw new Exception("开始时间不能在结束时间的后面");
            }
            return (int)dt1.Subtract(dt0).TotalMinutes;

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__ReplayRoute));
            this.SetSortButtonPresentation();
            if (!IsPostBack)
            {

                fromHour.Items.Clear();
                for (int i = 0; i <= 23; i++)
                    fromHour.Items.Add(new ListItem(i.ToString("00") + "时", i.ToString()));
                fromMinute.Items.Clear();
                for (int i = 0; i <= 59; i++)
                    fromMinute.Items.Add(new ListItem(i.ToString("00") + "分", i.ToString()));

                toHour.Items.Clear();
                for (int i = 0; i <= 23; i++)
                    toHour.Items.Add(new ListItem(i.ToString("00") + "时", i.ToString()));
                toMinute.Items.Clear();
                for (int i = 0; i <= 59; i++)
                    toMinute.Items.Add(new ListItem(i.ToString("00") + "分", i.ToString()));

                DateTime t0 = DateTime.Now;
                DateTime t1 = new DateTime(t0.Year, t0.Month, t0.Day, t0.Hour, t0.Minute, 0);
                DateTime t2 = t1.AddMinutes(3);

                fromDate.Text = t1.ToString("yyyy-M-d");
                fromHour.SelectedIndex = t1.Hour;
                fromMinute.SelectedIndex = t1.Minute;

                toDate.Text = t2.ToString("yyyy-M-d");
                toHour.SelectedIndex = t2.Hour;
                toMinute.SelectedIndex = t2.Minute;


                //SetPlayTimeDefaultScope();
                LoadDefaultView();
                tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers1);
                //tagSelector.SetDataSourceLeft(NetRadio.Business.BusSelectTagUser.SelectTagUsers1);
            }
        }

        private void LoadDefaultView()
        {

            //tagSelector.SelectedGroupId = -1;

            string zSortDir = "desc";
            if (_sortDir == SortDirection.Ascending)
            {
                zSortDir = "";
            }
            string strWhere = "";

            if (tagSelector.SelectedUserIds != null && tagSelector.SelectedUserIds.Length > 0)
            {
                string hostIds = "(";
                List<int> lGroupIds = new List<int>();
                for (int i = 0; i < tagSelector.SelectedUserIds.Length; i++)
                {
                    hostIds += tagSelector.SelectedUserIds[i].ToString();
                    HostTagGroupStatus hostGroup = HostTagGroupStatus.SelectByHostId(tagSelector.SelectedUserIds[i]);
                    if (hostGroup != null && !lGroupIds.Contains(hostGroup.HostGroupId))
                    {
                        lGroupIds.Add(hostGroup.HostGroupId);
                    }
                    if (i != (tagSelector.SelectedUserIds.Length - 1)) hostIds += ", ";
                }
                hostIds += ")";
                strWhere = "A.HostId in " + hostIds;

                if (lGroupIds.Count > 0)
                {
                    string zGroupIds = "(";
                    int j = 0;
                    foreach (int groupId in lGroupIds)
                    {
                        if (j == 0)
                            zGroupIds += groupId.ToString();
                        else
                            zGroupIds += ", " + groupId.ToString();
                        j++;
                    }
                    zGroupIds += ")";
                    strWhere += " AND HostGroupId in " + zGroupIds;
                }
                DateTime dtFrom = DateTime.Parse(fromDate.Text);
                dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, fromHour.SelectedIndex, fromMinute.SelectedIndex, 0);
                strWhere += " AND WriteTime >= '" + dtFrom.ToString("M/d/yyyy H:m:s") + "'";

                DateTime dtTo = DateTime.Parse(toDate.Text);
                dtTo = new DateTime(dtTo.Year, dtTo.Month, dtTo.Day, toHour.SelectedIndex, toMinute.SelectedIndex, 59);
                strWhere += " AND WriteTime <= '" + dtTo.ToString("M/d/yyyy H:m:s") + "'";

                int totalCount = 0;
                DataSet ds = DBPositionLog.GetPositionLog(strWhere, _zSortKey, zSortDir, p.PageSize, p.PageIndex, out totalCount);
                p.RecordCount = totalCount;
                if (ds.Tables.Count != 0)
                {
                    list.DataSource = ds.Tables[0].DefaultView;
                    list.DataBind();
                }
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadDefaultView();
        }

        #region SetPlayTimeDefaultScope

        //public void SetPlayTimeDefaultScope()
        //{
        //    if (Matching.IsInteger(continueTime.Text) == false)
        //    {
        //        continueTime.Text = "3";
        //    }
        //    var setTime = DateTime.Now.AddMinutes(-int.Parse(continueTime.Text));
        //    fromDate.Text = setTime.ToString("yyyy-M-d");
        //    fromHour.SelectedIndex = setTime.Hour;
        //    fromMinute.Text = setTime.Minute.ToString().PadLeft(2, '0');

        //    //facilityMap.SelectedMapId = Facility.All.Where(f => f.MapId != 0).Select(f => f.MapId).FirstOrDefault();
        //}

        #endregion

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            this.LoadDefaultView();
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

            DataRowView log = (DataRowView)e.Item.DataItem;
            if (log != null)
            {
                //Img icon = e.Item.FindControl("icon") as Img;
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                SmartLabel isDisappeared = e.Item.FindControl("isDisappeared") as SmartLabel;
                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;

                //icon.Src = CommonExtension.IdentityIcon(Convert.ToInt32(log["TagId"]));
                tagName.Text = Convert.ToString(log["HostName"]);
                //tagName.Href = PathUtil.ResolveUrl("TagUsers/TagUser.aspx?id=" + Convert.ToString(log["hostid"]) + "&type=" + (int)_userType);
                facilityName.Text = Convert.ToString(log["FacilityName"]);
                coordinatesName.Text = Convert.ToString(log["CoordinatesName"]);

                if (coordinatesName.Text.Length == 0)
                    coordinatesName.Text = "离开";

                if (Convert.ToInt32(log["IsDisappeared"]) >= 0)
                    isDisappeared.Text = "";
                else
                    isDisappeared.Text = "消失";
                writeTime.DisplayValue = Convert.ToDateTime(log["WriteTime"]);
            }
        }

        #region SetSortButtonPresentation

        private void SetSortButtonPresentation()
        {
            SortButton[] sortButtons = { updateTimeSorter };
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
            LoadDefaultView();
            //Terminator.Redirect(Request.Path);
        }

        #endregion
    }
}
