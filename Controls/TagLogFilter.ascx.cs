using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __TagLogFilter : UserControl
    {
        static readonly DateTime _SqlServerMinDateTime = new DateTime(1753, 1, 1);

        #region Properties

        public string TagNameKeyword
        {
            get
            {
                return tagName.Text.Trim();
            }
            set
            {
                tagName.Text = value;
            }
        }

        public int[] SelectedGroupIdArray
        {
            get
            {
                return groupSelector.SelectedGroupIdArray;
            }
            set
            {
                groupSelector.SelectedGroupIdArray = value;
            }
        }

        public bool FacilityFilterRowVisible
        {
            get
            {
                return facilityFilterRow.Visible;
            }
            set
            {
                facilityFilterRow.Visible = value;
            }
        }

        public DateTime FromTime
        {
            get
            {
                return this.FetchFromTime();
            }
            set
            {
                fromDate.Text = value.ToString("yyyy-M-d");
                fromHour.SelectedIndex = value.Hour;
                fromMinute.Text = value.Minute.ToString().PadLeft(2, '0');
            }
        }

        public DateTime ToTime
        {
            get
            {
                DateTime value = this.FetchToTime();
                if (value == _SqlServerMinDateTime)
                {
                    return DateTime.Now;
                }
                return value;
            }
            set
            {
                toDate.Text = value.ToString("yyyy-M-d");
                toHour.SelectedIndex = value.Hour;
                toMinute.Text = value.Minute.ToString().PadLeft(2, '0');
            }
        }

        public int MapId
        {
            get
            {
                return facilityMap.SelectedMapId;
            }
            set
            {
                facilityMap.SelectedMapId = value;
            }
        }

        private string _conditionQueryString;
        public string ConditionQueryString
        {
            get
            {
                EnsureConditions();
                return _conditionQueryString;
            }
        }

        private string _conditionDescription;
        public string ConditionDescription
        {
            get
            {
                EnsureConditions();
                return _conditionDescription;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            Page.PreLoad += new EventHandler(Page_PreLoad);
            base.OnInit(e);
        }

        private void Page_PreLoad(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SetFilterElementsByQueryString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["TagLogFilter"] != null)
            {
                __TagLogFilter tagLogFilter1 = (__TagLogFilter)Session["TagLogFilter"];
                condtionDescription.Text = tagLogFilter1.ConditionDescription;
            }
            else
                condtionDescription.Text = this.ConditionDescription;


            this.Page.ClientScript.RegisterClientScriptInclude("xx_" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/TagLogFilter.ascx.js"));
        }

        protected void searchButton_click(object sender, EventArgs e)
        {
            bool submitEventAttached = OnSubmit(e);
            Session["TagLogFilter"] = this;

            // 如果 submit 事件无实现，改用 URL 转向来传递
            if (!submitEventAttached)
            {
                if (ConditionQueryString.Length == 0)
                {
                    Response.Redirect(Request.Path);
                }
                else
                {
                    Response.Redirect(Request.Path + "?" + ConditionQueryString);
                }
            }
        }

        #region Event: OnSubmit

        static readonly object EventSubmit = new object();

        public event EventHandler<EventArgs> Submit
        {
            add
            {
                base.Events.AddHandler(EventSubmit, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventSubmit, value);
            }
        }

        bool OnSubmit(EventArgs e)
        {
            EventHandler<EventArgs> handler = (EventHandler<EventArgs>)base.Events[EventSubmit];
            if (handler == null)
            {
                return false;
            }
            handler(this, e);
            return true;
        }

        #endregion

        #region SetFilterElementsByQueryString

        private void SetFilterElementsByQueryString()
        {
            DateTime setTime;
            if (DateTime.TryParse(Fetch.QueryUrl("fromTime"), out setTime))
            {
                this.FromTime = setTime;
            }
            if (DateTime.TryParse(Fetch.QueryUrl("toTime"), out setTime))
            {
                ToTime = setTime;
            }

            string groupIds = Fetch.QueryUrl("groups");
            if (groupIds.Length != 0)
            {
                this.SelectedGroupIdArray = Strings.ParseToArray<int>(groupIds);
            }

            this.TagNameKeyword = Fetch.QueryUrl("tagNameKeyword");
            this.MapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);
        }
        #endregion

        #region EnsureConditions

        private void EnsureConditions()
        {
            IList<string> queryCollection = new List<string>();
            IList<string> descriptionCollection = new List<string>();
            _conditionDescription = "";

            //queryCollection.Add("userType=" + Fetch.QueryUrlAsIntegerOrDefault("userType",1));

            if (this.TagNameKeyword.Length != 0)
            {
                queryCollection.Add("tagNameKeyword=" + Server.UrlEncode(this.TagNameKeyword));
                descriptionCollection.Add("名称关键字: <span class='t1'>" + this.TagNameKeyword + "</span>");
            }

            if (this.SelectedGroupIdArray != null)
            {
                queryCollection.Add("groups=" + Misc.JoinToString(",", groupSelector.SelectedGroupIdArray));
                descriptionCollection.Add("属于分组: <span class='t1'>" + string.Join(", ", groupSelector.SelectedGroupNames) + "</span>");
            }

            if (this.MapId > 0)
            {
                queryCollection.Add("mapId=" + this.MapId);
                if (facilityFilterRow.Visible)
                {
                    descriptionCollection.Add("地图: <span class='t1'>" + facilityMap.SelectedText + "</span>");
                }
            }

            DateTime fromTime = this.FromTime;
            if (fromTime != _SqlServerMinDateTime)
            {
                queryCollection.Add("fromTime=" + fromTime.ToString("G"));
                descriptionCollection.Add("从 <span class='t1'>" + fromTime.ToString("g") + "</span>");
            }

            DateTime toTime = this.ToTime;
            if (ToTime != DateTime.Now)
            {
                queryCollection.Add("toTime=" + toTime.ToString("G"));
                descriptionCollection.Add("  至 <span class='t1'>" + toTime.ToString("g") + "</span>");
            }
            string _url = System.Web.HttpContext.Current.Request.RawUrl;
            int index = _url.IndexOf("?");
            string _urlQuery = "";
            if (index != -1)
            {
                _urlQuery = _url.Substring(index + 1);
            }
            _conditionQueryString = string.Join("&", queryCollection.ToArray());
            if (!string.IsNullOrEmpty(_conditionQueryString))
            {
                _conditionQueryString += "&" + _urlQuery;
            }
            else
            {
                _conditionQueryString = _urlQuery;
            }
            string description = descriptionCollection.Count == 0 ? "无" : string.Join("<span class='separator'> | </span>", descriptionCollection.ToArray());
            _conditionDescription = "<span class='t3'>[过滤条件]</span> &nbsp; " + description;
        }

        #endregion

        #region Time Format Method Fragments

        private DateTime FetchFromTime()
        {
            return this.MergeTime(fromDate.Text, fromHour.SelectedItem.Value, fromMinute.Text);
        }

        private DateTime FetchToTime()
        {
            return this.MergeTime(toDate.Text, toHour.SelectedItem.Value, toMinute.Text);
        }

        private DateTime MergeTime(string datePart, string hourPart, string minutePart)
        {
            string merge = datePart + " " + hourPart + ":" + minutePart.PadLeft(2, '0') + ":00";

            DateTime dt;
            if (datePart.Length > 0 && DateTime.TryParse(merge, out dt))
            {
                return dt;
            }
            return _SqlServerMinDateTime;
        }

        #endregion
    }
}