using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using System.ComponentModel;
using NetRadio.DataExtension;
using NetRadio.Common;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __TagSelector : UserControl
	{
		#region Properties

		public int  AllowedSelectCount {
			get {
				return (int)allowedCount.Value;
			}
			set {
				allowedCount.Value = value;
			}
		}

		public bool SelectedTagsVisible {
			get {
				return selectedList.Visible;
			}
			set {
				selectedList.Visible = value;
			}
		}

		public int[] SelectedTagIdArray {
			get {
				return Strings.ParseToArray<int>(selectedTagIds.Value);
			}
			set {
				if (value == null) {
					selectedTagIds.Value = "";
					selectedCount.Value = 0;
				}
				else {
					selectedTagIds.Value = string.Join(",", value.Select(i => i.ToString()).ToArray());
					selectedCount.Value = value.Length;
				}
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{ 
			allowedCountLabel.Visible = (allowedCount.Value > 0);
			selectAllLabel.Visible = !allowedCountLabel.Visible;

			//屏蔽分组功能
			//groupSelector.Style.Add("display", "none");

			AjaxUtil.RegisterClientScript(typeof(__TagSelector), this.Page);
            this.Page.ClientScript.RegisterClientScriptInclude("xx_" +DateTime.Now.ToString("yyyyMMddhhmmss")+ new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/TagSelector.ascx.js"));
           
            
			if (this.SelectedTagsVisible && selectedCount.Value > 0) {
				ScriptManager.RegisterStartupScript(this, this.GetType(), "OnTagSelectorLoad", "OnTagSelectorLoad();", true);
			}
		}

		#region Ajax: SelectTags

		[AjaxMethod(RequireSessionState.True)]
		public static object[] SelectTags(string keyword, string groupIdSerial, int pageSize, int skipOffset) {
			using (AppDataContext db = new AppDataContext()) {
                //20091231: need to set filter 
                if (Config.Settings.ModelHFilter != "")
                {
                    TagStatusView.aLicenseTagHFilter = Config.Settings.ModelHFilter.Split(';');
                }
                List<int> lHostTagIds = db.HostTags.Select(h => h.TagId).ToList();
                var query = Tag.AllValidTags
                    //.Where(t => Config.Settings.ModelHFilter.Split(';').Contains(t.TagMac.Substring(0,5)))
                    .Where(t => !lHostTagIds.Contains(t.Id))
					.Select(t => new {
						Id = t.Id,
						TagName = t.TagMac.Substring(9,8)
					});

				if (!string.IsNullOrEmpty(keyword)) {
					query = query.Where(t => t.TagName.Contains(keyword.Trim()));
				}
				if (!string.IsNullOrEmpty(groupIdSerial)) {
					var range1 = TagGroupCoverage.GetCoveredTagIdArray(Strings.ParseToArray<int>(groupIdSerial));
                    var range2 = HostTagGroupStatus.GetCoveredTagIdArray(Strings.ParseToArray<int>(groupIdSerial));
                    var range = MergeTwoArrayWithoutDuplicates(range1, range2);
                    query = query.Where(t => range.Contains(t.Id));
				}

                query = query.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize);
                
                return query.ToArray();
                 
			}
		}

        private static int[] MergeTwoArrayWithoutDuplicates(int[] arr1, int[] arr2)
        {
            if (arr1 == null || arr1.Length == 0) return arr2;
            if (arr2 == null || arr2.Length == 0) return arr1;
            List<int> lArr1 = arr1.ToList();
            for (int i=0; i<arr2.Length;i++)
            {
                if (lArr1.Contains(arr2[i])) continue;
                lArr1.Add(arr2[i]);
            }
            return lArr1.ToArray();

        }
		#endregion

		#region Ajax: GetSelectedTags

		[AjaxMethod]
		public static string[] GetSelectedTags(string tagIdArraySerial) {
			using (AppDataContext db = new AppDataContext()) {
				var tagIdArray = Strings.ParseToArray<int>(tagIdArraySerial);
				var names = db.Tags
					.Where(t => tagIdArray.Contains(t.Id))
					.Select(t => t.TagMac)
					.ToArray();
                //for (int i = 0; i < names.Length; i++) {
                //    names[i] = "<a href='" + PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + tagIdArray[i]) + "'>" + names[i] + "</a>";
                //}
				return names;
			}
		}

		#endregion
	}
}