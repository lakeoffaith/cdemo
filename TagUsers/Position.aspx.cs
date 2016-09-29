using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using NetRadio.Common.LocatingMonitor;
using NetRadio.LocatingService.RemotingEntry;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.DataExtension;

namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __Position : BasePage
    {
        public __Position()
        {
            _id = Fetch.QueryUrlAsInteger("id");
        }

        int _id;
        bool isAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            NetRadio.Data.User oUser = NetRadio.Data.User.SelectByUserName(me.Name);
            if (oUser.Role >= (int)UserRole.Admin)
                isAdmin = true;

            if (!Page.IsPostBack)
            {
                AjaxUtil.RegisterClientScript(typeof(__TagUser), this.Page);

                int _tagId;
                string _tagMac = null;

                using (AppDataContext db = new AppDataContext())
                {
                    var position = db.HostTagGroupStatus.SingleOrDefault(h => h.HostId == _id && h.HostGroupId == (int)TagUserType.Position);
                    if (position == null)
                    {
                        Terminator.End("记录不存在。");
                    }

                    _tagId = position.TagId;
                    var tag = Tag.Select(_tagId);
                    if (tag != null)
                    {
                        _tagMac = tag.TagMac;
                    }

                    currentTagId.Value = _tagId.ToString();
                    tagMac.Text = _tagMac;
                    userId.Value = position.HostId.ToString();

                    name.Text = newName.Text = position.HostName;

                    memo.Text = newMemo.Text = Strings.TextEncode(position.Description);



                    if (!isAdmin)
                    {
                        changeMemo.Visible = false;
                        deleteButton.Visible = false;
                    }
                }


                if (!LocatingServiceUtil.IsAvailable() || !isAdmin)
                {
                    //changeTag.Visible = false;
                    clearAllEvents.Visible = false;
                    locatingServiceDownMarker.Visible = true;
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(_tagMac))
                        {
                            TagStatusView tagStatus = LocatingServiceUtil.Instance<IServiceApi>().SelectTagStatus(_tagMac);
                            if (tagStatus != null)
                            {

                                batteryInsufficient.Text = Misc.GetEventStatusDescription(tagStatus.BatteryInsufficientStatus);
                                batteryReset.Text = Misc.GetEventStatusDescription(tagStatus.BatteryResetStatus);
                                buttonPressed.Text = Misc.GetEventStatusDescription(tagStatus.ButtonPressedStatus);
                                eventUpdateTime.Value = tagStatus.EventUpdateTime;

                                clearBatteryInsufficient.Visible = tagStatus.BatteryInsufficientStatus != (byte)EventStatus.Cleared;
                                clearBatteryReset.Visible = tagStatus.BatteryResetStatus != (byte)EventStatus.Cleared;
                                clearButtonPressed.Visible = tagStatus.ButtonPressedStatus != (byte)EventStatus.Cleared;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected void deleteButton_Click(object sender, EventArgs e)
        {

            HostTag host = HostTag.GetById(_id);
            if (host != null)
            {
                bool serviceAvailable = LocatingServiceUtil.IsAvailable();
                var tagId = host.TagId;
                if (tagId > 0)
                {
                    using (AppDataContext db = new AppDataContext())
                    {

                        //更新标签名称
                        Tag tag = Tag.Select(tagId);
                        if (tag != null)
                        {
                            string mac = tag.TagMac;
                            if (serviceAvailable)
                            {
                                LocatingServiceUtil.Instance<IServiceApi>().UpdateTagNameAndSerialNo(tagId, "NewTag_" + mac.Substring(12), "");
                            }
                            else
                            {
                                Tag.UpdateTagNameAndSerialNo(tagId, "NewTag_" + mac.Substring(12), "");
                            }
                        }

                        //删除标签历史记录和相关信息
                        GenericEventLog.DeleteByTagId(tagId);
                        TagEventStatus.DeleteMany(tagId);
                        TagPositionStatus.DeleteMany(tagId);
                        TagAlert.DeleteTagAlerts(tagId);


                    }

                    TagStatusView.SelectTagStatus(tagId).HostTag = null;
                }

                TagAlert.DeleteTagAlerts(_id);
                //删除host信息
                HostTag.DeleteHostTag(_id);
                //记录日志

                if (tagId > 0 && serviceAvailable)
                {
                    LocatingServiceUtil.Instance<IServiceApi>().ReloadTagHost(tagId);
                }
                Diary.Insert(ContextUser.Current.Id, tagId, _id, "删除定点报警标签" + host.HostName + "的信息" + (host.TagId == 0 ? "" : "并解除标签绑定。") + "。");
            }
            new SuccessTerminator().End("删除成功。", -1, Link.CloseWindow);
        }

    }
}
