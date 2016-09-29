using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __TagFilter : UserControl
	{
		#region Properties

		public string TagNameKeyword {
			get {
				return tagName.Text.Trim();
			}
			set {
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

		public int MapId {
			get {
				return facilityMap.SelectedMapId;
			}
			set {
				facilityMap.SelectedMapId = value;
			}
		}

		public bool LocatingOnly {
			get {
				return locatingOnly.Checked;
			}
			set {
				locatingOnly.Checked = value;
			}
		}

		public bool AbsentOnly {
			get {
				return absentOnly.Checked;
			}
			set {
				absentOnly.Checked = value;
			}
		}

		public bool LowerBatteryOnly {
			get {
				return lowerBatteryOnly.Checked;
			}
			set {
				lowerBatteryOnly.Checked = value;
			}
		}

		public bool AreaEventOnly {
			get {
				return areaEventOnly.Checked;
			}
			set {
				areaEventOnly.Checked = value;
			}
		}

		public bool ButtonPressedOnly {
			get {
				return buttonPressedOnly.Checked;
			}
			set {
				buttonPressedOnly.Checked = value;
			}
		}

		public bool WristletBrokenOnly {
			get {
				return wristletBrokenOnly.Checked;
			}
			set {
				wristletBrokenOnly.Checked = value;
			}
		}


		public string SortField {
			get {
				return sortField.SelectedItem.Value;
			}
			set {
				ListItem finding = sortField.Items.FindByValue(value.ToString());
				if (finding != null) {
					sortField.ClearSelection();
					finding.Selected = true;
				}
			}
		}

		public SortDirection SortDirection {
			get {
				return (SortDirection)int.Parse(sortDirection.SelectedItem.Value);
			}
			set {
				ListItem finding = sortDirection.Items.FindByValue(((int)value).ToString());
				if (finding != null) {
					sortDirection.ClearSelection();
					finding.Selected = true;
				}
			}
		}


		private string _conditionQueryString;
		public string ConditionQueryString {
			get {
				EnsureConditions();
				return _conditionQueryString;
			}
		}

		private string _conditionDescription;
		public string ConditionDescription {
			get {
				EnsureConditions();
				return _conditionDescription;
			}
		}

		#endregion

		protected override void OnInit(EventArgs e) {
			Page.PreLoad += new EventHandler(Page_PreLoad);
			base.OnInit(e);
		}

		private void Page_PreLoad(object sender, EventArgs e) {
			if (!Page.IsPostBack) {
				this.SetFilterElementsByQueryString();
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			condtionDescription.Text = this.ConditionDescription;			
            this.Page.ClientScript.RegisterClientScriptInclude("xx_" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/TagFilter.ascx.js"));
		}

		protected void searchButton_click(object sender, EventArgs e) {
			bool submitEventAttached = OnSubmit(e);

			// 如果 submit 事件无实现，改用 URL 转向来传递
			if (!submitEventAttached) {
				if (ConditionQueryString.Length == 0) {
					Response.Redirect(Request.Path);
				}
				else {
                    
                    Response.Redirect( Request.Path +"?" + ConditionQueryString);
				}
			}
		}

		#region Event: OnSubmit

		static readonly object EventSubmit = new object();

		public event EventHandler<EventArgs> Submit {
			add {
				base.Events.AddHandler(EventSubmit, value);
			}
			remove {
				base.Events.RemoveHandler(EventSubmit, value);
			}
		}

		bool OnSubmit(EventArgs e) {
			EventHandler<EventArgs> handler = (EventHandler<EventArgs>)base.Events[EventSubmit];
			if (handler == null) {
				return false;
			}
			handler(this, e);
			return true;
		}

		#endregion

		#region SetFilterElementsByQueryString

		private void SetFilterElementsByQueryString() {
			this.TagNameKeyword = Fetch.QueryUrl("tagNameKeyword");
			string groupIds = Fetch.QueryUrl("groups");
			if (groupIds.Length != 0)
			{
				this.SelectedGroupIdArray = Strings.ParseToArray<int>(groupIds);
			}
			this.MapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);

			this.LocatingOnly = Fetch.QueryUrl("locating") == "1";

			this.AbsentOnly = Fetch.QueryUrl("absent") == "1";
			this.LowerBatteryOnly = Fetch.QueryUrl("lowerBattery") == "1";
			this.AreaEventOnly = Fetch.QueryUrl("areaEvent") == "1";
			this.ButtonPressedOnly = Fetch.QueryUrl("buttonPressed") == "1";
			this.WristletBrokenOnly = Fetch.QueryUrl("wristletBroken") == "1";

			this.SortField = Fetch.QueryUrl("sortField");
			this.SortDirection = (SortDirection)Fetch.QueryUrlAsIntegerOrDefault("sortDirection", 0);
		}
		#endregion

		#region EnsureConditions

		private void EnsureConditions() {
			IList<string> queryCollection = new List<string>();
			IList<string> descriptionCollection = new List<string>();
			_conditionDescription = "";

			if (this.TagNameKeyword.Length != 0) {
				queryCollection.Add("tagNameKeyword=" + this.TagNameKeyword);
                descriptionCollection.Add("名称关键字: <span class='t1'>" + this.TagNameKeyword + "</span>");
			}

			if (this.SelectedGroupIdArray != null) {
				queryCollection.Add("groups=" + Misc.JoinToString(",", groupSelector.SelectedGroupIdArray));
				descriptionCollection.Add("属于分组: <span class='t1'>" + string.Join(", ", groupSelector.SelectedGroupNames) + "</span>");
			}

			if (this.MapId > 0) {
				queryCollection.Add("mapId=" + this.MapId);
				if (facilityFilterRow.Visible) {
					descriptionCollection.Add("当前位于: <span class='t1'>" + facilityMap.SelectedText + "</span>");
				}
			}

			if (this.LocatingOnly) {
				queryCollection.Add("locating=1");
				descriptionCollection.Add("<span class='t1'>仅正在定位</span>");
			}

			IList<string> events = new List<string>();
			if (this.AbsentOnly) {
				queryCollection.Add("absent=1");
				events.Add("消失");
			}
			if (this.LowerBatteryOnly) {
				queryCollection.Add("lowerBattery=1");
				events.Add("低电");
			}
			if (this.AreaEventOnly) {
				queryCollection.Add("areaEvent=1");
				events.Add("区域告警");
			}
			if (this.ButtonPressedOnly) {
				queryCollection.Add("buttonPressed=1");
				events.Add("触发按钮");
			}
			if (this.WristletBrokenOnly) {
				queryCollection.Add("wristletBroken=1");
				events.Add("腕带断开");
			}
			if (events.Count != 0) {
				descriptionCollection.Add("处于告警: <span class='t1'>" + string.Join("<span class='separator'>,</span>", events.ToArray()) + "</span>");
			}

			if (sortField.SelectedIndex > 0) {
				queryCollection.Add("sortField=" + sortField.SelectedItem.Value);
			}
			if (sortDirection.SelectedIndex > 0) {
				queryCollection.Add("sortDirection=" + sortDirection.SelectedItem.Value);
			}

			_conditionQueryString = string.Join("&", queryCollection.ToArray());

			string description = descriptionCollection.Count == 0 ? "无" : string.Join("<span class='separator'> | </span>", descriptionCollection.ToArray());
			_conditionDescription = "<span class='t3'>[过滤条件]</span> &nbsp; " + description;
		}

		#endregion
	}
}