using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Web.UI;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Controls
{
	public class FacilityMapDropList : CustomControl, IPostBackDataHandler
	{
		public FacilityMapDropList() {
			base.TagName = "select";
		}

		[DefaultValue(0)]
		public int SelectedFacilityId {
			get {
				object value = ViewState["SelectedFacilityId"];
				if (value == null) {
					return 0;
				}
				return (int)value;
			}
			set {
				ViewState["SelectedFacilityId"] = value;
			}
		}

		[DefaultValue(0)]
		public int SelectedMapId {
			get {
				var fid = this.SelectedFacilityId;
				if (fid == 0) {
					return 0;
				}
				return Facility.All.Where(f => f.Id == fid).Select(f => f.MapId).SingleOrDefault();
			}
			set {
				if (value > 0) {
					Facility facility = Facility.All.SingleOrDefault(f => f.MapId == value);
					if (facility != null) {
						this.SelectedFacilityId = facility.Id;
						return;
					}
				}
				this.SelectedFacilityId = 0;
			}
		}

		[DefaultValue(null), ReadOnly(true)]
		public string SelectedText {
			get {
				var fid = this.SelectedFacilityId;
				if (fid == 0) {
					return this.DefaultText;
				}
				return Facility.All.Where(f => f.Id == fid).Select(f => f.FacilityName).SingleOrDefault();
			}
		}

		[DefaultValue("--- 选择地图 ---")]
		public string DefaultText {
			get {
				object value = ViewState["DefaultText"];
				if (value == null) {
					return "--- 选择地图 ---";
				}
				return (string)value;
			}
			set {
				ViewState["DefaultText"] = value;
			}
		}

		[DefaultValue(false)]
		public bool DisplayTreeStyle {
			get {
				object value = ViewState["DisplayTreeStyle"];
				if (value == null) {
					return false;
				}
				return (bool)value;
			}
			set {
				ViewState["DisplayTreeStyle"] = value;
			}
		}

		void RenderSingleOption(HtmlTextWriter writer, Facility facility, int indent) {
			writer.WriteBeginTag("option");

			// 注意: FacilityMapDropList将MapId作为Value，区别于FacilityDropList
			writer.WriteAttribute("value", facility.MapId.ToString());

			string append = null;

			if (facility.MapId <= 0) {
				writer.WriteAttribute("class", "t3");
				append = " (N/A)";
			}
			else {
				if (facility.Id == this.SelectedFacilityId) {
					writer.WriteAttribute("selected", "selected");
				}
			}
			writer.Write(">");
			writer.Write("└ ".PadLeft(indent + 1, '　') + facility.FacilityName + append);
			writer.WriteEndTag("option");
			writer.WriteLine();
		}

		void RecursionTree(HtmlTextWriter writer, int parentId) {
			IList<Facility> facilities = Facility.All.Where(f => f.ParentFacilityId == parentId).ToList();
			foreach (Facility f in facilities) {
				// 计算indent
				int indent = 0;
				int pid = f.ParentFacilityId;
				while (pid > 0) {
					indent += 2;
					Facility parentFacility = Facility.All.SingleOrDefault(x => x.Id == pid);
					if (parentFacility == null) {
						break;
					}
					pid = parentFacility.ParentFacilityId;
				}

				this.RenderSingleOption(writer, f, indent);
				this.RecursionTree(writer, f.Id);
			}
		}

		void RenderSmoothList(HtmlTextWriter writer) {
			IList<Facility> facilities = Facility.All;
			foreach (Facility f in facilities) {
				if (f.MapId > 0) {
					this.RenderSingleOption(writer, f, 0);
				}
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			Attributes.Add("name", (this.NamingContainer == Page ? "" : (this.NamingContainer.UniqueID + "$")) + this.ID);
			base.RenderBeginTag(writer);
			writer.Indent++;

			writer.WriteLine();
			writer.Write("<option value=\"0\">" + this.DefaultText + " &nbsp;  &nbsp; </option>");

			if (this.DisplayTreeStyle) {
				this.RecursionTree(writer, 0);
			}
			else {
				this.RenderSmoothList(writer);
			}

			writer.Indent--;
			base.RenderEndTag(writer);
		}

		#region IPostBackDataHandler Members

		public bool LoadPostData(string postDataKey, NameValueCollection postCollection) {
			if (postDataKey != null) {
				// 注意: FacilityMapDropList将MapId作为Value，区别于FacilityDropList
				this.SelectedMapId = Convert.ToInt32(postCollection[postDataKey]);
			}
			return false;
		}

		public void RaisePostDataChangedEvent() {
		}

		#endregion
	}
}
