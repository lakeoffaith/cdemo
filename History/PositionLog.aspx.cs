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
    public partial class __PositionLog : BasePage
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
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        private static string _zSortKey = " ID";
        private static SortDirection _sortDir = SortDirection.Descending;

        private TagUserType _userType;
        
        protected void Page_Load(object sender, EventArgs e)
        {
           // this.Title = this.WebSiteLabel = "位置记录";
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 0);
            //this.Wrap.Title = CommonExtension.GetUserTypeDescription(_userType) + this.Wrap.Title;

            if (!IsPostBack)
            {
                LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            this.SetSortButtonPresentation();
            historyNavigator.AppendSearchConditionQueryString(tagLogFilter.ConditionQueryString);
            this.LoadRepeater();
        }

        private void LoadRepeater()
        {
            //---add by tan 2009-10-5---//
            using (AppDataContext db = new AppDataContext())
            {


                if (Session["TagLogFilter"] != null)
                {
                    tagLogFilter = (NetRadio.LocatingMonitor.Controls.__TagLogFilter)Session["TagLogFilter"];
                    string tagFilterCondition = tagLogFilter.ConditionDescription;
                }


                string strWhere = LocatingMonitorUtils.ConstructSQLWhere(tagLogFilter);

                strWhere += (strWhere.Trim() == "") ? "hostgroupid= " + ((int)_userType).ToString() : " AND hostgroupid= " + ((int)_userType).ToString();
                //if (strWhere.Trim().Length <= 0)
                //{
                //    itotalrecords = db.pNetRadio_AllRecords("view_TagPositionLogView", ref itotalrecords);
                //    p.RecordCount = Convert.ToInt32(itotalrecords);
                //}

                string zSortDir = "desc";
                if (_sortDir == SortDirection.Ascending)
                {
                    zSortDir = "";
                }


                //SqlCommand myCommand;
                //SqlConnection myConnection = (SqlConnection)db.Connection;

                //myCommand = new SqlCommand("pNetRadio_PageRecords", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //myCommand.CommandTimeout = 600;

                //int? itotalrecords = 0;
                //myCommand.Parameters.Add("@tblName", "DBViewTagPositionLog as A JOIN  object_HostGroup as B ON A.hostid=B.hostid and B.hostgroupid= " + (int)_userType + " and " + strWhere);
                //myCommand.Parameters.Add("@strGetFields", "A.TagId,A.hostid,A.HostName,A.FacilityName,A.CoordinatesName,A.IsDisappeared,A.WriteTime,A.MapId");
                //myCommand.Parameters.Add("@fldName", " "+_zSortKey);
                //myCommand.Parameters.Add("@strWhere", "");
                //myCommand.Parameters.Add("@PageSize", p.PageSize);
                //myCommand.Parameters.Add("@PageIndex", p.PageIndex);
                //myCommand.Parameters.Add("@Sort", zSortDir);
                //myCommand.Parameters.Add("@totalRecords", itotalrecords);

                //SqlDataAdapter adapter = new SqlDataAdapter();
                //SqlParameter Par = myCommand.Parameters.AddWithValue("@rowcount", SqlDbType.Int);
                //Par.Direction = ParameterDirection.Output;


                //myConnection.Open();
                //myCommand.ExecuteNonQuery();
                //adapter.SelectCommand = myCommand;
                //DataSet ds = new DataSet();
                //adapter.Fill(ds, "view_TagPositionLogView");
                //myConnection.Close();

                ////取得过滤后的总记录数
                //if (strWhere != "")
                //    p.RecordCount = Int32.Parse(Par.Value.ToString());
                int totalCount = 0;
                DataSet ds = DBPositionLog.GetPositionLog(strWhere, _zSortKey, zSortDir, p.PageSize, p.PageIndex, out totalCount);
                p.RecordCount = totalCount;
                if (ds.Tables.Count != 0)
                {
                    list.DataSource = ds.Tables[0].DefaultView;
                    list.DataBind();
                }
            }
            /*using (AppDataContext db = new AppDataContext()) {

                var query = db.TagPositionLogViews
                    .OrderByDescending(x => x.Id)
                    .Select(x => new TagPositionLogView {
                        TagId = x.TagId,
                        TagName = x.TagName,
                        FacilityId = x.FacilityId,
                        FacilityName = x.FacilityName,
                        MapId = x.MapId,
                        CoordinatesId = x.CoordinatesId,
                        CoordinatesName = x.CoordinatesName,
                        WriteTime = x.WriteTime
                    });

                query = query.Where(x => x.WriteTime >= tagLogFilter.FromTime);
                query = query.Where(x => x.WriteTime < tagLogFilter.ToTime);

                if (tagLogFilter.TagNameKeyword.Length > 0) {
                    query = query.Where(x => x.TagName.Contains(tagLogFilter.TagNameKeyword));
                }

                if (tagLogFilter.SelectedGroupIdArray != null) {
                    int[] coveredTagIdArray = TagGroupCoverage.GetCoveredTagIdArray(tagLogFilter.SelectedGroupIdArray);
                    query = query.Where(x => coveredTagIdArray.Contains(x.TagId));
                }

                if (tagLogFilter.FacilityFilterRowVisible && tagLogFilter.MapId > 0) {
                    query = query.Where(x => x.MapId == tagLogFilter.MapId);
                }

                switch ((TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 1)) {
                    default:
                        break;
                    case TagUserType.Cop:
                        var arr1 = HostTagGroupStatus.All().Where(u => u.TagId != 0 && u.HostGroupId == (byte)TagUserType.Cop).Select(u => u.TagId).ToArray();
                        query = query.Where(x => arr1.Contains(x.TagId));
                        break;
                    case TagUserType.Culprit:
                        var arr2 = HostTagGroupStatus.All().Where(u => u.TagId != 0 && u.HostGroupId == (byte)TagUserType.Culprit).Select(u => u.TagId).ToArray();
                        query = query.Where(x => arr2.Contains(x.TagId));
                        break;
                }

                p.RecordCount = query.Count();
                query = query.Skip(p.RecordOffset).Take(p.PageSize);

                list.DataSource = query.ToList();
                list.DataBind();
            }*/
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //---add by tan  2009-10-56---//
            DataRowView log = (DataRowView)e.Item.DataItem;
            if (log != null)
            {
                Img icon = e.Item.FindControl("icon") as Img;
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                SmartLabel isDisappeared = e.Item.FindControl("isDisappeared") as SmartLabel;
                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
                Anchor replayRoute = e.Item.FindControl("replayRoute") as Anchor;

                icon.Src = CommonExtension.IdentityIcon(Convert.ToInt32(log["TagId"]));
                tagName.Text = Convert.ToString(log["HostName"]);
                tagName.Href = PathUtil.ResolveUrl("TagUsers/TagUser.aspx?id=" + Convert.ToString(log["hostid"]) + "&type=" + (int)_userType);
                facilityName.Text = Convert.ToString(log["FacilityName"]);
                coordinatesName.Text = Convert.ToString(log["CoordinatesName"]);

                if (coordinatesName.Text.Length == 0)
                    coordinatesName.Text = "离开监控区域";

                if (Convert.ToInt32(log["IsDisappeared"]) >= 0)
                    isDisappeared.Text = "";
                else
                    isDisappeared.Text = "消失";
                writeTime.DisplayValue = Convert.ToDateTime(log["WriteTime"]);
                //replayRoute.Href = "javascript:viewTagPositionTrack(" + Convert.ToString(log["MapId"]) + ", '" + Convert.ToString(log["hostid"]) + "', '" + Convert.ToDateTime(log["WriteTime"]).AddMinutes(-10) + "', 10);";
                replayRoute.Href = "javascript:viewTagPositionTrack(0, '" + Convert.ToString(log["hostid"]) + "', '" + Convert.ToDateTime(log["WriteTime"]).AddMinutes(-10) + "', 10);";

            }

            //----------------------------------//

            //TagPositionLogView log = e.Item.DataItem as TagPositionLogView;
            /*
			if (log != null) {
				Img icon = e.Item.FindControl("icon") as Img;
				Anchor tagName = e.Item.FindControl("tagName") as Anchor;
				SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
				SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
				DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
				Anchor replayRoute = e.Item.FindControl("replayRoute") as Anchor;

                icon.Src = CommonExtension.IdentityIcon(log.TagId);
				tagName.Text = log.TagName;
				tagName.Href = PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + log.TagId);
				facilityName.Text = log.FacilityName;
				coordinatesName.Text = log.CoordinatesName;
				writeTime.DisplayValue = log.WriteTime;
				replayRoute.Href = "javascript:viewTagPositionTrack(" + log.MapId + ", '" + log.TagId + "', '" + log.WriteTime.AddMinutes(-10) + "', 10);";
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
