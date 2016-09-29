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
	public class FacilityDropList : CustomControl, IPostBackDataHandler
	{
		public FacilityDropList() {
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
		public int HideFacilityId {
			get {
				object value = ViewState["HideFacilityId"];
				if (value == null) {
					return 0;
				}
				return (int)value;
			}
			set {
				ViewState["HideFacilityId"] = value;
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

		[DefaultValue("--- 选择场所 ---")]
		public string DefaultText {
			get {
				object value = ViewState["DefaultText"];
				if (value == null) {
					return "--- 选择场所 ---";
				}
				return (string)value;
			}
			set {
				ViewState["DefaultText"] = value;
			}
		}

		void RenderSingleOption(HtmlTextWriter writer, Facility facility, int indent) {
			writer.WriteBeginTag("option");

			// 注意: FacilityDropList将FacilityId作为Value，区别于FacilityMapDropList
			writer.WriteAttribute("value", facility.Id.ToString());

			string append = null;

			if (facility.Id == this.SelectedFacilityId) {
				writer.WriteAttribute("selected", "selected");
			}
			writer.Write(">");
			writer.Write("└ ".PadLeft(indent + 2, '　') + facility.FacilityName + append);
			writer.WriteEndTag("option");
			writer.WriteLine();
		}

		void RecursionTree(HtmlTextWriter writer, int parentId) {
			IList<Facility> facilities = Facility.All.Where(f => f.ParentFacilityId == parentId).ToList();

			foreach (Facility f in facilities) {
				int indent = 0;
				int pid = f.ParentFacilityId;
				bool output = (f.Id != this.HideFacilityId);

				while (pid > 0) {
					if (pid == this.HideFacilityId) {
						output = false;
					}
					indent += 2;
					Facility parentFacility = Facility.All.SingleOrDefault(x => x.Id == pid);
					if (parentFacility == null) {
						break;
					}
					pid = parentFacility.ParentFacilityId;
				}

				if (output) {
					this.RenderSingleOption(writer, f, indent);
				}
				this.RecursionTree(writer, f.Id);
			}
		}

		protected override void Render(HtmlTextWriter writer) {
			Attributes.Add("name", (this.NamingContainer == Page ? "" : (this.NamingContainer.UniqueID + "$")) + this.ID);
			base.RenderBeginTag(writer);
			writer.Indent++;

			writer.WriteLine();
			writer.Write("<option value=\"0\">" + this.DefaultText + " &nbsp;  &nbsp; </option>");

			this.RecursionTree(writer, 0);

			writer.Indent--;
			base.RenderEndTag(writer);
		}

		#region IPostBackDataHandler Members

		public bool LoadPostData(string postDataKey, NameValueCollection postCollection) {
			if (postDataKey != null) {
				// 注意: FacilityDropList将FacilityId作为Value，区别于FacilityMapDropList
				this.SelectedFacilityId = Convert.ToInt32(postCollection[postDataKey]);
			}
			return false;
		}

		public void RaisePostDataChangedEvent() {
		}

		#endregion
	}
}
