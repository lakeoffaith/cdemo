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
    public partial class __ButtonPressedLog : BasePage
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
        private static string _zSortKey = " ID";
        private static SortDirection _sortDir = SortDirection.Descending;

        private TagUserType _userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Title = this.WebSiteLabel = "按钮触发";
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 0);
            if (!IsPostBack)
            {
                //清理缓存   
                Response.Cache.SetNoStore();
                LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            //Sitemap.Text2 = "历史事件";
            //Sitemap.Text3 = this.Wrap.Title;
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

                //过滤条件
                //tagLogFilter.SelectedGroupIdArray = new int[1] { (int)_userType };  //20091228 add
                string strWhere = LocatingMonitorUtils.ConstructSQLWhere(tagLogFilter);
                string zCondition = "GenericEventType= " + (int)SupportEvent.ButtonPressed;
                strWhere += (strWhere.Trim() == "") ? zCondition : " AND " + zCondition;
                strWhere += " AND hostgroupid= " + ((int)_userType).ToString();

                string zSortDir = "desc";
                if (_sortDir == SortDirection.Ascending)
                {
                    zSortDir = "";
                }

                int totalCount = 0;
                DataSet ds = GenericEventLog.GetGenericEventLog(strWhere, _zSortKey, zSortDir, p.PageSize, p.PageIndex, out totalCount);
                p.RecordCount = totalCount;
                if (ds.Tables.Count != 0)
                {
                    list.DataSource = ds.Tables[0].DefaultView;
                    list.DataBind();
                }
            }
            ////////////////////////////////////////////////////////////////////////////////
            /*using (AppDataContext db = new AppDataContext()) {
                var query = db.GenericEventLogViews
                    .Where(x => x.GenericEventType == (int)SupportEvent.ButtonPressed)
                    .OrderByDescending(x => x.Id)
                    .Select(x => new GenericEventLogView {
                        TagId = x.TagId,
                        TagName = x.TagName,
                        MapId = x.MapId,
                        CoordinatesName = x.CoordinatesName,
                        FacilityName = x.FacilityName,
                        WriteTime = x.WriteTime
                    });
                if (Session["TagLogFilter"] != null)
                {
                    tagLogFilter = (NetRadio.LocatingMonitor.Controls.__TagLogFilter)Session["TagLogFilter"];
                    string tagFilterCondition = tagLogFilter.ConditionDescription;
                }

                query = query.Where(x => x.WriteTime > tagLogFilter.FromTime);
                query = query.Where(x => x.WriteTime < tagLogFilter.ToTime);

                if (tagLogFilter.TagNameKeyword.Length > 0) {
                    query = query.Where(x => x.TagName.Contains(tagLogFilter.TagNameKeyword));
                }

                if (tagLogFilter.SelectedGroupIdArray != null)
                {
                    //int[] coveredTagIdArray = TagGroupCoverage.GetCoveredTagIdArray(tagLogFilter.SelectedGroupIdArray);
                    int[] coveredTagIdArray = HostTagGroupStatus.GetCoveredTagIdArray(tagLogFilter.SelectedGroupIdArray);
                    query = query.Where(x => coveredTagIdArray.Contains(x.TagId));
                }

                if (tagLogFilter.FacilityFilterRowVisible && tagLogFilter.MapId > 0) {
                    query = query.Where(x => x.MapId == tagLogFilter.MapId);
                }

                p.RecordCount = query.Count();
                query = query.Skip(p.RecordOffset).Take(p.PageSize);

                list.DataSource = query.ToList();
                list.DataBind();
            }
            */
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DataRowView log = (DataRowView)e.Item.DataItem;
            if (log != null)
            {
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                //tagName.Text = LocatingMonitorUtils.GetHostName(log["TagName"].ToString(), log["TagMac"].ToString(), log["HostName"].ToString());
                tagName.Text = Convert.ToString(log["HostName"]);
                HostTagGroupStatus hostTagGroupStatus = HostTagGroupStatus.SelectByHostId((int)log["HostId"]);
                if (hostTagGroupStatus != null)
                {
                    int groupId = hostTagGroupStatus.HostGroupId;

                    tagName.Href = PathUtil.ResolveUrl(string.Format("/TagUsers/TagUser.aspx?id={0}&type={1}", log["HostId"], groupId));
                }
                else
                {

                }

                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
                writeTime.DisplayValue = Convert.ToDateTime(log["WriteTime"]);
                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                facilityName.Text = Convert.ToString(log["FacilityName"]);
                coordinatesName.Text = Convert.ToString(log["CoordinatesName"]);
            }

            ///////////////////////////////////////////////////////////////////////
            /*
			GenericEventLogView log = e.Item.DataItem as GenericEventLogView;
			if (log != null) {
				Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                tagName.Text = LocatingMonitorUtils.GetHostName(log.TagName, log.HostName);
				tagName.Href = PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + log.TagId);

				DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
				writeTime.DisplayValue = log.WriteTime;

				SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
				facilityName.Text = log.FacilityName;

				SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
				coordinatesName.Text = log.CoordinatesName;
			}
            */
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
            Response.Redirect(Request.RawUrl);
            //Terminator.Redirect(Request.Path + "?userType=" + ((int)_userType).ToString() );
        }

        #endregion
    }
}
