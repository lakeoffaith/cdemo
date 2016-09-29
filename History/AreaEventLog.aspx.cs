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
    public partial class __AreaEventLog : BasePage
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
            //this.Title = this.WebSiteLabel = "区域告警";
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

                string zSortDir = "desc";
                if (_sortDir == SortDirection.Ascending)
                {
                    zSortDir = "";
                }
                int totalCount = 0;
                DataSet ds = NetRadio.Data.AreaEventLog.GetAreaEventLog(strWhere, _zSortKey, zSortDir, p.PageSize, p.PageIndex, out totalCount);
                p.RecordCount = totalCount;

                if (ds.Tables.Count != 0)
                {
                    list.DataSource = ds.Tables[0].DefaultView;
                    list.DataBind();
                }
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //AreaEventLogView log = e.Item.DataItem as AreaEventLogView;
            //---add by tan  2009-10-6---//
            DataRowView log = (DataRowView)e.Item.DataItem;
            if (log != null)
            {
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                SmartLabel areaName = e.Item.FindControl("areaName") as SmartLabel;
                SmartLabel areaEventType = e.Item.FindControl("areaEventType") as SmartLabel;
                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;

                tagName.Text = LocatingMonitorUtils.GetHostName(log["TagName"].ToString(), log["TagMac"].ToString(), log["HostName"].ToString());
                HostTagGroupStatus hostTagGroupStatus = HostTagGroupStatus.SelectByHostId((int)log["HostId"]);
                if (hostTagGroupStatus != null)
                {
                    int groupId = hostTagGroupStatus.HostGroupId;

                    tagName.Href = PathUtil.ResolveUrl(string.Format("/TagUsers/TagUser.aspx?id={0}&type={1}", log["HostId"], groupId));
                }
                else
                {

                }

                facilityName.Text = Convert.ToString(log["FacilityName"]);
                areaName.Text = Convert.ToString(log["AreaName"]);
                if (Convert.ToInt32(log["AreaEventType"]) == 0)
                {
                    areaEventType.Text = "进入区域";
                }
                else
                {
                    areaEventType.Text = "离开区域";
                }


                writeTime.DisplayValue = Convert.ToDateTime(log["WriteTime"]);
            }

            //----------------------------------//

            /*
            Anchor tagName = e.Item.FindControl("tagName") as Anchor;
            SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
            SmartLabel areaName = e.Item.FindControl("areaName") as SmartLabel;
            SmartLabel areaEventType = e.Item.FindControl("areaEventType") as SmartLabel;
            DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;

            tagName.Text = log.TagName;
            tagName.Href = PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + log.TagId);
            facilityName.Text = log.FacilityName;
            areaName.Text = log.AreaName;
            areaEventType.Text = (AreaEventType)log.AreaEventType == AreaEventType.StayInside ? "进入区域" : "离开区域";
            writeTime.DisplayValue = log.WriteTime;
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
