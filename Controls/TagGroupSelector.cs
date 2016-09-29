using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI;
using NetRadio.Common.LocatingMonitor;
using System.Collections.Generic;
using NetRadio.Data;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Util;

namespace NetRadio.LocatingMonitor.Controls
{
    public class TagGroupSelector : TextBox
    {
        public int[] SelectedGroupIdArray
        {
            get
            {
                if (Text.Length == 0 || Text == "未指定组")
                {
                    return null;
                }
                var serial = Text.Substring(Text.IndexOf(':') + 1);
                return Strings.ParseToArray<int>(serial);
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    Text = "未指定组";
                }
                Text = "已选组: " + Misc.JoinToString(", ", value);
            }
        }

        public string[] SelectedGroupNames
        {
            get
            {
                if (this.SelectedGroupIdArray == null)
                {
                    return null;
                }
                var groupNames = new List<string>();
                foreach (var id in this.SelectedGroupIdArray)
                {
                    //groupNames.Add(TagGroup.GetGroupName(id));
                    HostGroupInfo groupInfo = HostGroupInfo.GetById(id);
                    if (groupInfo != null)
                        groupNames.Add(groupInfo.HostGroupName);
                }
                return groupNames.ToArray();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptInclude("TagGroupSelector" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/TagGroupSelector.ctl.js"));
            AjaxUtil.RegisterClientScript(typeof(TagGroupSelector), Page);
        }

        [AjaxMethod]
        public static object[] SelectAllGroups()
        {
                return HostGroupInfo.All.Select(x => new
                {
                    Id = x.HostGroupId,
                    GroupName = x.HostGroupName
                }).ToArray();
           
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.ReadOnly = true;
            this.Attributes.Add("onmouseover", "javascript:Benz.show(this);");

            if (Text.Length == 0 || Text == "未指定组")
            {
                Text = "未指定组";
                this.Attributes.CssStyle.Add("color", "Gray");
            }

            base.Render(writer);
        }
    }
}