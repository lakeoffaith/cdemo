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
namespace NetRadio.LocatingMonitor.Organize
{
    [MarshalAjaxRegister]
    public partial class __TagGroupList : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");

            scriptFiles.Add("5", "App_Script/UI/TagGroupList.aspx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadRepeater();

            if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
            {
                objectNavigator.Visible = true;
            }
            else
            {

                objectNavigator.Visible = false;
            }
        }

        internal class HostCountByGroup
        {
            internal int Id;
            internal string GroupName;
            internal int TagCount;
            internal string GroupDescription;
        }

        private void LoadRepeater()
        {
            /*using (AppDataContext db = new AppDataContext())
            {
                groupList.DataSource = db.TagGroupCounterViews.OrderBy(g => g.GroupName).ToList();
                groupList.DataBind();
            }*/
            IList<HostGroupInfo> lGroup = HostGroupInfo.All;
            List<HostCountByGroup> lGroupCount = new List<HostCountByGroup>();
            if (lGroup != null)
            {
                IList<HostTagGroupStatus> allHosts = HostTagGroupStatus.All;
                foreach (HostGroupInfo info in lGroup)
                {
                    //GTang2010/01/27
                    if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                    {
                        if (info.HostGroupId == (int)HostGroupTypeEnum.Phenomenon || info.HostGroupId == (int)HostGroupTypeEnum.Location)
                        {
                            continue;
                        }
                    }
                    else if (Config.Settings.ProjectType == ProjectTypeEnum.YZPrison)
                    {
                        if (info.HostGroupId == (int)HostGroupTypeEnum.Location)
                        {
                            continue;
                        }
                    }
                    //-------------
                    HostCountByGroup hostCount = new HostCountByGroup();
                    hostCount.Id = info.HostGroupId;
                    hostCount.GroupName = info.HostGroupName;
                    hostCount.GroupDescription = info.Description;
                    hostCount.TagCount = allHosts.Where(x => x.HostGroupId == info.HostGroupId).Count(); ;
                    lGroupCount.Add(hostCount);
                }
                groupList.DataSource = lGroupCount;
                groupList.DataBind();
            }
        }

        protected void groupList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //TagGroupCounterView group = e.Item.DataItem as TagGroupCounterView;
            HostCountByGroup group = e.Item.DataItem as HostCountByGroup;
            if (group != null)
            {
                Anchor groupName = e.Item.FindControl("groupName") as Anchor;
                groupName.Text = group.GroupName;
                groupName.Href = PathUtil.ResolveUrl("Organize/TagGroup.aspx?id=" + group.Id);
                groupName.Attributes.Add("id", "g_" + group.Id);

                NumericLabel tagCount = e.Item.FindControl("tagCount") as NumericLabel;
                tagCount.Value = group.TagCount;

                SmartLabel groupDescription = e.Item.FindControl("groupDescription") as SmartLabel;
                groupDescription.Text = Strings.MonospacedLeft(group.GroupDescription, 50);

                Anchor detail = e.Item.FindControl("detail") as Anchor;
                detail.Href = groupName.Href;

                //不能删除根级组
                int idelgroupid = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["DelFootGroup"]);
                int parentid = HostGroupInfo.GetById(group.Id).ParentGroupId;
                Anchor deleteLink = e.Item.FindControl("delete") as Anchor;
                if (parentid > 0 || idelgroupid > 0)
                {

                    deleteLink.Href = "javascript:deleteTagGroup(" + group.Id + ");";
                }
                else
                {
                    groupName.Href = "";
                    deleteLink.Href = "";
                    deleteLink.ToolTip = "一级组不能删除";
                    detail.ToolTip = "一级组不能编辑";
                    groupName.ToolTip = "一级组不能编辑";
                    detail.Href = "";
                    //deleteLink.Text = "";
                }
            }
        }
    }
}
