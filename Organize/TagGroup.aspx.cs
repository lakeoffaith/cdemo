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
    public partial class __TagGroup : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/SelectTagUser.ascx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        public __TagGroup()
        {
            _id = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
            _editMode = Fetch.QueryUrl("action") == "addnew" ? EditMode.AddNew : EditMode.Modify;
        }

        int _id;
        EditMode _editMode;

        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Title = this.WebSiteLabel = "设置标签分组";
            //Sitemap.Text2 = "系统设置";
            //Sitemap.Text3 = "管理对象分组";
            //Sitemap.Url3 = "TagGroupList.aspx";
            //Sitemap.Text4 = this.Wrap.Title = _editMode == EditMode.AddNew ? "新增分组" : "编辑已有分组";

            if (!IsPostBack)
            {
                LoadDefaullView();
                if (_editMode == EditMode.AddNew)
                {
                    tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.GetAllTagUsers);
                }
                else if (_editMode == EditMode.Modify)
                {
                    tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.GetAllTagUsers);
                    tagSelector.SetDataSourceRight(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers0, _id.ToString());
                }
            }

        }

        private void LoadDefaullView()
        {

            try
            {
                grouplist.Items.Clear();
                //var hostGroupName = HostGroupInfo.All.OrderBy(x => x.ParentGroupId);

                //bool bParentGroupID = false;
                //foreach (var name in hostGroupName)
                //{
                //    grouplist.Items.Add(new ListItem(name.HostGroupName, name.HostGroupId.ToString()));
                //    if (name.ParentGroupId == 0)
                //    {
                //        bParentGroupID = true;
                //    }
                //}
                //if (!bParentGroupID)
                //{
                //    grouplist.Items.Insert(0, new ListItem("无", "0"));
                //}

                if (_editMode == EditMode.AddNew)
                {
                    // tagSelector.SelectedGroupId = 0; //////////////
                    save.Text = "新增";
                    try
                    {
                        //GTang2010/01/27 根据项目绑定显示内容
                        IList<HostGroupInfo> hostGroupName;
                        if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                        {
                            hostGroupName = HostGroupInfo.GetGroupNameByProjectType((int)ProjectTypeEnum.NMPrison);//HostGroupInfo.GetByParentId(0);
                        }
                        else if (Config.Settings.ProjectType == ProjectTypeEnum.YZPrison)
                        {
                            hostGroupName = HostGroupInfo.GetGroupNameByProjectType((int)ProjectTypeEnum.YZPrison);
                        }
                        else
                        {
                            hostGroupName = HostGroupInfo.GetByParentId(0);
                        }
                        //  var hostGroupName = HostGroupInfo.GetByParentId(0);
                        if (hostGroupName == null || hostGroupName.Count == 0)
                        {
                            grouplist.Items.Insert(0, new ListItem("无", "0"));
                        }
                        else
                        {
                            foreach (var name in hostGroupName)
                            {
                                grouplist.Items.Add(new ListItem(name.HostGroupName, name.HostGroupId.ToString()));
                            }
                        }
                        //grouplist.Items.Insert(0, new ListItem("无", "0"));
                    }
                    catch
                    {

                    }
                }
                else
                {
                    //tagSelector.SelectedGroupId = _id;/////////////////
                    HostGroupInfo groupInfo = HostGroupInfo.GetById(_id);

                    if (groupInfo == null) return;
                    if (groupInfo.ParentGroupId == 0)
                    {
                        grouplist.Items.Insert(0, new ListItem("无", "0"));
                        tagSelector.Visible = false;
                    }
                    else
                    {
                        tagSelector.Visible = true;
                        HostGroupInfo parentGroupInfo = HostGroupInfo.GetById(groupInfo.ParentGroupId);
                        if (parentGroupInfo == null)
                            return;
                        grouplist.Items.Add(new ListItem(parentGroupInfo.HostGroupName, parentGroupInfo.HostGroupId.ToString()));
                    }

                    save.Text = "保存";
                    /*TagGroup group = TagGroup.Select(_id);
                    groupName.Text = group.GroupName;
                    groupDescription.Text = group.GroupDescription;
                    tagSelector.SelectedTagIdArray = TagGroupCoverage.GetCoveredTagIdArray(_id);
                     * */

                    HostGroupInfo group = HostGroupInfo.GetById(_id);
                    groupName.Text = group.HostGroupName.Trim();
                    groupDescription.Text = group.Description;
                    //tagSelector.SelectedTagIdArray = HostTagGroupStatus.GetCoveredHostIdArray(_id); //HostTagGroupStatus.GetCoveredTagIdArray(_id);/////////////////
                    grouplist.SelectedValue = Convert.ToString(group.ParentGroupId);

                    if (group.ParentGroupId == 0)
                    {
                        if (grouplist.Items.Count > 1)
                        {
                            for (int i = grouplist.Items.Count - 1; i > 0; i--)
                            {
                                grouplist.Items.RemoveAt(i);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < grouplist.Items.Count; i++)
                        {
                            if (grouplist.Items[i].Value == Convert.ToString(group.HostGroupId))
                            {
                                grouplist.Items.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                feedbacks.Items.AddError("Error:" + ex.Message);
            }
        }

        protected void save_click(object sender, EventArgs e)
        {
            if (groupName.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入对象组名称。");
            }
            //if (_editMode == EditMode.AddNew && TagGroup.All.Any(g => g.GroupName == groupName.Text.Trim())) {
            //if (HostGroupInfo.All.Any(g => g.HostGroupName.Trim() == groupName.Text.Trim() && g.HostGroupId != tagSelector.SelectedGroupId))
            if (HostGroupInfo.All.Any(g => g.HostGroupName.Trim() == groupName.Text.Trim() && g.HostGroupId != (_id == -1 ? 0 : _id)))
            {
                feedbacks.Items.AddError("对象组名称“" + groupName.Text.Trim() + "”已存在。");
            }
            if (feedbacks.Items.Count > 0)
            {
                return;
            }

            if (_editMode == EditMode.AddNew)
            {
                //_id = TagGroup.Insert(groupName.Text.Trim(), Strings.Left(groupDescription.Text.Trim(), 200), tagSelector.SelectedTagIdArray);
                //yzhu 20091002, add parent Group
                _id = HostGroupInfo.AddHostGroupInfo(0, groupName.Text.Trim(), Strings.Left(groupDescription.Text.Trim(), 200), Convert.ToInt32(this.grouplist.SelectedValue));
                HostTag.RemoveHostGroupByGroupId(_id);
                //如果是根组，把tagid=0的用户都插入该组,下面的插入数据代码后期需要写在数据层HostGroup里
                /*if (Convert.ToInt32(this.grouplist.SelectedValue) == 0)   
                {
                    var query = HostTag.All.Where(t =>t.TagId == 0).Select(x => x.HostId).ToList();

                    using (AppDataContext db = new AppDataContext())
                    {
                        for (int i = 0; i < query.Count; i++)
                        { 
                            HostGroup hg = new HostGroup();
                            hg.HostId = Convert.ToInt32(query[i].ToString());
                            hg.HostGroupId = _id;
                            db.HostGroups.InsertOnSubmit(hg);
                            db.SubmitChanges();
                        } 
                    }
                }
                */

                if (tagSelector.SelectedUserIds != null)
                {
                    AddHostTagGroup(tagSelector.SelectedUserIds, _id);

                }
                Terminator.Redirect("TagGroupList.aspx");
            }
            else
            {
                //TagGroup.UpdateById(_id, groupName.Text.Trim(), Strings.Left(groupDescription.Text.Trim(), 200), tagSelector.SelectedTagIdArray);
                HostGroupInfo.UpdateHostGroupInfo(_id, groupName.Text.Trim(), Strings.Left(groupDescription.Text.Trim(), 200), Convert.ToInt32(this.grouplist.SelectedValue));
                HostTag.RemoveHostGroupByGroupId(_id);
                if (tagSelector.SelectedUserIds != null)
                {
                    AddHostTagGroup(tagSelector.SelectedUserIds, _id);

                }
                //add 2009-10-21
                //////////////////tagSelector.SelectedTagIdArray = HostTagGroupStatus.GetCoveredHostIdArray(_id);

                feedbacks.Items.AddPrompt("保存成功。");
            }
        }

        private void AddHostTagGroup(int[] TagIdArray, int groupId)
        {
            int MapId = 0;
            List<int> lHostIds = new List<int>();
            for (int i = 0; i < TagIdArray.Length; i++)
            {

                HostTag host = HostTag.GetById(TagIdArray[i]);

                if (host != null && host.HostId > 0)
                {
                    HostTag.SetHostGroup(host.HostId, groupId);
                }
                else
                {
                    host = new HostTag();
                    Tag tag = Tag.Select(TagIdArray[i]);
                    if (tag != null)
                    {
                        host.HostExternalId = tag.SerialNo;
                        //TagHost tagHost = Tag.SelectTagHost(TagIdArray[i]);
                        host.HostName = groupName.Text.Trim() + "_" + ((tag.TagMac.Length > 9) ? tag.TagMac.Substring(9) : tag.TagMac);
                        host.Description = tag.TagName;
                        host.ImagePath = "";
                        host.HostType = 0;
                        host.TagId = TagIdArray[i];
                        host.HostId = HostTag.AddOrUpdateHostTag(0, host.TagId, host.HostExternalId, host.HostName, host.HostType, host.Description, host.ImagePath);
                        HostTag.SetHostGroup(host.HostId, groupId);
                        if (i == 0)
                        {
                            TagStatusView tagView1 = TagStatusView.SelectTagStatusByHostId(host.HostId);
                            MapId = tagView1.MapId;
                        }
                    }

                }
                lHostIds.Add(host.HostId);
            }
            int[] hostIds = lHostIds.ToArray();
            if (LocatingServiceUtil.IsAvailable())
            {
                //内蒙不需要这样删除
                try
                {
                    int[] delhostIds;
                    GetDelIDs(out delhostIds, groupId, MapId, hostIds);
                    if (delhostIds.Count() > 1)
                    {
                        //合并两个数组，传值，以-1为分界值
                        int[] myhostIds = new int[delhostIds.Length + hostIds.Length];
                        delhostIds.CopyTo(myhostIds, 0);
                        hostIds.CopyTo(myhostIds, delhostIds.Length);
                        LocatingServiceUtil.Instance<IServiceApi>().UpdateHostGroup(myhostIds, groupId);
                    }
                    else
                    {
                        LocatingServiceUtil.Instance<IServiceApi>().UpdateHostGroup(hostIds, groupId);
                    }
                }
                catch (Exception err)
                {

                }

            }
        }

        /// <summary>
        /// 取出需要删除掉的hostid
        /// </summary>
        /// <param name="delhostIds"></param>
        /// <param name="groupid"></param>
        /// <param name="MapId"></param>
        /// <param name="hostIds"></param>
        /// 

        private void GetDelIDs(out int[] delhostIds, int groupid, int MapId, int[] hostIds)
        {
            LocatingServiceUtil.DemandLocatingService();
            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
            List<int> ihostid = new List<int>();
            bool bExist = false;
            int itotalcount = 0;
            int imapid = MapId;
            int[] FindHostIDByGroup = new int[] { groupid };
            TagStatusView[] tgv = serviceApi.SelectTagStatusListByKeywords(
                                 "",
                                 FindHostIDByGroup,
                                 MapId,
                                 false,
                                 false,
                                 false,
                                 false,
                                 false,
                                 false,
                                 "TagName",
                                SortDirection.Ascending,
                                 50,
                                 0,
                                 out itotalcount
                             );
            for (int k = 0; k < tgv.Count(); k++)
            {
                for (int m = 0; m < hostIds.Count(); m++)
                {
                    if (tgv[k].HostTag.HostId == hostIds[m])
                    {
                        bExist = true;
                        break;
                    }
                }
                if (bExist)
                {
                    bExist = false;
                    continue;
                }
                else
                {
                    ihostid.Add(tgv[k].HostTag.HostId);
                }
            }
            if (ihostid.Count() > 0)
            {
                ihostid.Add(-1);
            }

            delhostIds = ihostid.ToArray();
        }

    }
}
