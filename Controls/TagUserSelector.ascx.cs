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

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __TagUserSelector : UserControl
    {
        #region Properties

        public TagUserType UserType
        {
            get
            {
                int i;
                int.TryParse(tagUserType.Value, out i);
                return (TagUserType)i;
            }
            set
            {
                tagUserType.Value = ((int)value).ToString();
            }
        }

        public int SelectedGroupId
        {
            get
            {
                int i;
                int.TryParse(selectedGroupId.Value, out i);
                return i;
            }
            set
            {
                selectedGroupId.Value = value.ToString();
            }
        }

        public int AllowedSelectCount
        {
            get
            {
                return (int)allowedCount.Value;
            }
            set
            {
                allowedCount.Value = value;
            }
        }

        public bool SelectedTagsVisible
        {
            get
            {
                return selectedList.Visible;
            }
            set
            {
                selectedList.Visible = value;
            }
        }

        public int[] SelectedTagIdArray
        {
            get
            {
                return Strings.ParseToArray<int>(selectedTagIds.Value);
            }
            set
            {
                if (value == null)
                {
                    selectedTagIds.Value = "";
                    selectedCount.Text = "0";
                }
                else
                {
                    selectedTagIds.Value = string.Join(",", value.Select(i => i.ToString()).ToArray());
                    selectedCount.Text = value.Length.ToString();
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            selectedCount.ID = "selectedCount";
            allowedCountLabel.Visible = (allowedCount.Value > 0);
            selectAllLabel.Visible = !allowedCountLabel.Visible;

            AjaxUtil.RegisterClientScript(typeof(__TagUserSelector), this.Page);
            this.Page.ClientScript.RegisterClientScriptInclude("xx_" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/TagUserSelector.ascx.js"));
            selectedCount.Attributes["readonly"] = "readonly";
            int __count;
            int.TryParse(selectedCount.Text, out __count);
            if (this.SelectedTagsVisible && __count > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "OnTagUserSelectorLoad", "OnTagUserSelectorLoad();", true);
            }

        }

        #region Ajax: SelectTags

        //select Tags that parent group ID =0
        [AjaxMethod(RequireSessionState.True)]
        public static object[] SelectTags(int userTypeEnumValue, string keyword, int pageSize, int skipOffset)
        {
            //var query=HostTagGroupStatus.All()
            //    .Where(u => u.TagId != 0 && u.HostGroupId == userTypeEnumValue)
            //    .Select(u => new {
            //        Id = u.TagId,
            //        TagName = u.HostName
            //    });

            //yyang,090927，轨迹用户分类暂时取消
            //var query = (HostTagGroupStatus.All
            //    .Where(u => u.ParentGroupId == 0 && !u.HostName.Contains("受虐"))
            //    .Select(u => new
            //    {
            //        Id = u.HostId,
            //        TagName = u.HostName
            //    })).Distinct(); //modified by Tan

            //lyz,tagid大于0，即用户必须已经绑定标签
            var query = (HostTagGroupStatus.All
                .Where(u => u.TagId > 0 && u.ParentGroupId == 0 && !u.HostName.Contains("受虐") && u.HostGroupId != 3)
                .Select(u => new
                {
                    Id = u.HostId,
                    TagName = u.HostName
                })).Distinct();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.TagName.Contains(keyword.Trim()));
            }

            return query.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize).ToArray();
        }

        //select one group
        [AjaxMethod(RequireSessionState.True)]
        public static object[] SelectTagsForGroup(int userTypeEnumValue, string keyword, int pageSize, int skipOffset)
        {
            //yzhu 20091228, select by group
            //var query = HostTagGroupStatus.All
            //    .Where(u => u.HostGroupId == userTypeEnumValue)
            //    .Select(u => new
            //    {
            //        Id = u.HostId,//yyang 20100105, id is hostId not tagId
            //        TagName = u.HostName
            //    });
            //lyz,tagid大于0，即用户必须已经绑定标签
            var query = HostTagGroupStatus.All
                .Where(u => u.TagId > 0 && u.HostGroupId == userTypeEnumValue)
                .Select(u => new
                {
                    Id = u.HostId,//yyang 20100105, id is hostId not tagId
                    TagName = u.HostName
                });

            //yyang,090927，轨迹用户分类暂时取消
            //var query = (HostTagGroupStatus.All
            //    .Where(u => u.ParentGroupId == 0) //remove TagId != 0
            //    .Select(u => new
            //    {
            //        Id = u.HostId,
            //        TagName = u.HostName
            //    })).Distinct(); //modified by Tan

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.TagName.Contains(keyword.Trim()));
            }

            return query.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize).ToArray();
        }

        [AjaxMethod(RequireSessionState.True)]
        public static object[] SelectTagsBySelectedGroupId(int groupId, int userTypeEnumValue, string keyword, int pageSize, int skipOffset)
        {
            if (groupId == -1)
                return SelectTags(userTypeEnumValue, keyword, pageSize, skipOffset);

            if (groupId == 0)
                return SelectTagsForGroup(userTypeEnumValue, keyword, pageSize, skipOffset);
            //var query=HostTagGroupStatus.All()
            //    .Where(u => u.TagId != 0 && u.HostGroupId == userTypeEnumValue)
            //    .Select(u => new {
            //        Id = u.TagId,
            //        TagName = u.HostName
            //    });

            //yzhu
            HostGroupInfo groupInfo = HostGroupInfo.GetById(groupId);
            if (groupInfo == null)
                return SelectTagsForGroup(userTypeEnumValue, keyword, pageSize, skipOffset);

            if (groupInfo.ParentGroupId == 0)
                return SelectTagsForGroup(userTypeEnumValue, keyword, pageSize, skipOffset);

            //yyang,090927，轨迹用户分类暂时取消
            //var query = (HostTagGroupStatus.All
            //    .Where(u => u.HostGroupId == groupInfo.ParentGroupId)
            //    .Select(u => new
            //    {
            //        Id = u.HostId,
            //        TagName = u.HostName
            //    })).Distinct(); //modified by Tan
            //lyz,tagid大于0，即用户必须已经绑定标签
            var query = (HostTagGroupStatus.All
               .Where(u => u.TagId > 0 && u.HostGroupId == groupInfo.ParentGroupId)
               .Select(u => new
               {
                   Id = u.HostId,
                   TagName = u.HostName
               })).Distinct(); //modified by Tan

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(t => t.TagName.Contains(keyword.Trim()));
            }

            return query.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize).ToArray();
        }

        [AjaxMethod(RequireSessionState.True)]
        public static object[] SelectTagsBySelectedGroupName(string groupName, int groupId, int userTypeEnumValue, string keyword, int pageSize, int skipOffset)
        {

            if (groupName == null || groupName == "" || groupName == "未指定组")
            {

            }
            else
            {

                //int pos = groupName.IndexOf("已选组: ");

                if (groupName.Length >= 5) //"已选组: " ==> 5 char
                {
                    groupName = groupName.Substring(5);
                    groupName = groupName.Replace(", ", ",");
                    string[] aGroupIds = groupName.Split(',');
                    if (aGroupIds != null && aGroupIds.Length > 0)
                    {
                        if (aGroupIds.Length == 1) groupId = Convert.ToInt32(aGroupIds[0]);
                        else
                        {
                            //var query2 = from q in HostTagGroupStatus.All
                            //             where (aGroupIds.Contains(q.HostGroupId.ToString()))
                            //             select new
                            //             {
                            //                 Id = q.HostId,
                            //                 TagName = q.HostName
                            //             };


                            //var query2 = (HostTagGroupStatus.All
                            //.Where(u => aGroupIds.Contains(u.HostGroupId.ToString()))
                            //.Select(u => new
                            //    {
                            //        Id = u.HostId,
                            //        TagName = u.HostName
                            //    })).Distinct(); //modified by Tan


                            //lyz,tagid大于0，即用户必须已经绑定标签
                            var query2 = (HostTagGroupStatus.All
                           .Where(u => u.TagId > 0 && aGroupIds.Contains(u.HostGroupId.ToString()))
                           .Select(u => new
                           {
                               Id = u.HostId,
                               TagName = u.HostName
                           })).Distinct();


                            if (!string.IsNullOrEmpty(keyword))
                            {
                                query2 = query2.Where(t => t.TagName.Contains(keyword.Trim()));
                            }

                            return query2.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize).ToArray();

                        }
                    }
                }
            }

            if (groupId == -1)
                return SelectTags(userTypeEnumValue, keyword, pageSize, skipOffset);

            if (groupId == 0)
                return SelectTagsForGroup(userTypeEnumValue, keyword, pageSize, skipOffset);
            //var query=HostTagGroupStatus.All()
            //    .Where(u => u.TagId != 0 && u.HostGroupId == userTypeEnumValue)
            //    .Select(u => new {
            //        Id = u.TagId,
            //        TagName = u.HostName
            //    });

            //yzhu
            HostGroupInfo groupInfo = HostGroupInfo.GetById(groupId);
            if (groupInfo == null)
                return SelectTagsForGroup(userTypeEnumValue, keyword, pageSize, skipOffset);

            //if (groupInfo.ParentGroupId == 0)
            return SelectTagsForGroup(groupId, keyword, pageSize, skipOffset);

            //yyang,090927，轨迹用户分类暂时取消
            //var query = (HostTagGroupStatus.All
            //    .Where(u => u.HostGroupId == groupInfo.ParentGroupId)
            //    .Select(u => new
            //    {
            //        Id = u.HostId,
            //        TagName = u.HostName
            //    })).Distinct(); //modified by Tan

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(t => t.TagName.Contains(keyword.Trim()));
            //}

            //return query.OrderBy(t => t.TagName).Skip(skipOffset).Take(pageSize).ToArray();



        }
        #endregion

        #region Ajax: GetSelectedTags

        [AjaxMethod]
        public static string[] GetSelectedTags(string tagIdArraySerial)
        {
            if (tagIdArraySerial == null || tagIdArraySerial == "") return null;

            var tagIdArray = Strings.ParseToArray<int>(tagIdArraySerial);
            var names = HostTag.All
                .Where(u => tagIdArray.Contains(u.HostId)) //modified by Tan
                .Select(u => u.HostName)
                .ToArray();

            return names;
        }

        #endregion
    }
}