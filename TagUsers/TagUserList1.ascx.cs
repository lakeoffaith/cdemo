using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.IO;
using System.Text.RegularExpressions;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __TagUserList1 : NetRadio.Web.BaseUserControl
    {
        private TagUserType _userType;

        protected void Page_Load(object sender, EventArgs e)
        {
            int actionNum = (int)GetDateItem(0);
            TagUserType userType = (TagUserType)GetDateItem(1);
            string p_keyword = GetDateItem(2).ToString();
            string p_extandId = GetDateItem(3).ToString();
            int p_jailRoomSelectedIndex = (int)GetDateItem(4);
            string p_jailRoomSelectedValue = GetDateItem(5).ToString();
            string tagBindingSelectedValue = GetDateItem(6).ToString();
            string tagOnlineSelectedValue = GetDateItem(7).ToString();
            int pageIndex = (int)GetDateItem(8);
            
            switch (actionNum)
            {
                case 2:
                    _userType = userType;
                    p.PageIndex = pageIndex;
                    this.LoadRepeater(p_keyword, p_extandId, p_jailRoomSelectedIndex, p_jailRoomSelectedValue, tagBindingSelectedValue, tagOnlineSelectedValue);
                    break;
                default:
                    break;
            }
            if (BusSystemConfig.IsAutoSelectStrongestRssiTag() == false)
            {
                tagSelector.Visible = true;
                selectStrongestRssiTag.Visible = false;                
            }
            else
            {
                tagSelector.Visible = false;
                selectStrongestRssiTag.Visible = true;
                
            }
        }

        private void LoadRepeater(string p_keyword, string p_extandId, int p_jailRoomSelectedIndex, string p_jailRoomSelectedValue, string tagBindingSelectedValue, string tagOnlineSelectedValue)
        {

           // var query = HostTagGroupStatus.All().Where(u => u.HostGroupId == (byte)_userType);
           
            using (AppDataContext db = new AppDataContext())
            {
               var query = db.HostPositionStatusViews.ToList().Where(u => u.HostGroupId == (byte)_userType);
               
                if (!string.IsNullOrEmpty(p_keyword.Trim()))
                {
                    query = query.Where(u => u.HostName.ToUpper().Contains(p_keyword.Trim().ToUpper()));
                }

                if (!string.IsNullOrEmpty(p_extandId.Trim()))
                {
                    query = query.Where(u => u.HostExternalId.ToUpper().Contains(p_extandId.Trim().ToUpper()));
                }

                if (tagBindingSelectedValue == "1")
                {
                    query = query.Where(u => u.TagId != 0);
                }
                if (tagBindingSelectedValue == "2")
                {
                    query = query.Where(u => u.TagId == 0);
                }

                if (tagOnlineSelectedValue == "1")
                {
                    IList<TagStatusView> tagList = new List<TagStatusView>();
                    string _keyword = "";
                    int[] _hostGroupArray = null;
                    int totalCount = 0;
                    if (LocatingServiceUtil.IsAvailable())
                    {
                        IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                        if (serviceApi != null)
                        {
                            tagList = serviceApi.SelectTagStatusList(
                            _keyword,
                            _hostGroupArray,
                            0,
                            true,
                            false, //SupportEvent.Absent),
                            false, //SupportEvent.BatteryInsufficient),
                            false, //SupportEvent.AreaEvent),
                            false, //SupportEvent.ButtonPressed),
                            false, //SupportEvent.WristletBroken),
                            "",
                            SortDirection.Ascending,
                            9999,
                            0,
                            out totalCount);
                        }
                    }

                    query = query.Where(u => tagList.Where(t => t.X > 0).Select(t => t.TagId).ToList().Contains(u.TagId));
                }

                if (tagOnlineSelectedValue == "2")
                {
                    IList<TagStatusView> tagList = new List<TagStatusView>();
                    string _keyword = "";
                    int[] _hostGroupArray = null;
                    int totalCount = 0;
                    if (LocatingServiceUtil.IsAvailable())
                    {
                        IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                        if (serviceApi != null)
                        {
                            tagList = serviceApi.SelectTagStatusList(
                            _keyword,
                            _hostGroupArray,
                            0,
                            true,
                            false, //SupportEvent.Absent),
                            false, //SupportEvent.BatteryInsufficient),
                            false, //SupportEvent.AreaEvent),
                            false, //SupportEvent.ButtonPressed),
                            false, //SupportEvent.WristletBroken),
                            "",
                            SortDirection.Ascending,
                            9999,
                            0,
                            out totalCount);
                        }
                    }

                    query = query.Where(u => !tagList.Where(t => t.X > 0).Select(t => t.TagId).ToList().Contains(u.TagId));
                }

                if (_userType == TagUserType.Culprit && p_jailRoomSelectedIndex > 0)
                {
                    int[] range = CulpritRoomReference.GetCulpritIdByRoomId(int.Parse(p_jailRoomSelectedValue));
                    query = query.Where(u => range.Contains(u.HostId));
                }

                p.RecordCount = query.Count();

                //if (_sortDir == SortDirection.Ascending)
                //{
                //    query = query.OrderBy(u => u.HostName);
                //}
                //else
                //{
                //    query = query.OrderByDescending(u => u.HostName);
                //}
                list.DataSource = query.Skip(p.RecordOffset).Take(p.PageSize).ToList();
                list.DataBind();

            }
        }

        protected void list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                HostPositionStatusView user = e.Item.DataItem as HostPositionStatusView;
                Img img = e.Item.FindControl("bindTag") as Img;
                if (img != null)
                {
                    img.Attributes["onclick"] = "return bindTag_click(" + user.TagId + "," + user.HostId + ",'" + user.HostExternalId + "')";
                    img.Src = "../Images/bindTag.gif";
                    img.ToolTip = "绑定标签";
                }
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //HostTagGroupStatus user = e.Item.DataItem as HostTagGroupStatus;
            HostPositionStatusView user = e.Item.DataItem as HostPositionStatusView;

            if (user != null)
            {
                //Img photo = (Img)e.Item.FindControl("photo");
                //photo.Src = "UserPhoto.ashx?id=" + user.HostId + "&w=50&h=60";
                //photo.Href = "TagUser.aspx?type=" + user.HostGroupId + "&id=" + user.HostId;

                Anchor name = (Anchor)e.Item.FindControl("name");
                //name.Text = Strings.MonospacedLeft(user.HostName, 10);
                name.Text = user.HostName;
                name.ToolTip = user.HostName;
                name.Href = "TagUser.aspx?type=" + user.HostGroupId + "&id=" + user.HostId;

                SmartLabel number = (SmartLabel)e.Item.FindControl("number");
                number.Text = user.HostExternalId;

                if (user.TagId != 0)
                {
                    try
                    {
                        Tag tag = Tag.Select(user.TagId);
                        SmartLabel smac = (SmartLabel)e.Item.FindControl("mac");
                        smac.Text = tag.TagMac.Substring(9);
                    }
                    catch { }
                }



                Anchor currentLocation = (Anchor)e.Item.FindControl("currentLocation");
                currentLocation.Text = user.CoordinatesName;
                currentLocation.Href = string.Format("/History/PositionLog.aspx?userType={0}&hostid={1}&masterFile=Master/WebItem.Master",
                    user.HostGroupId, user.HostId);

                if (user.TagId != 0)
                {
                    Img tagIcon = (Img)e.Item.FindControl("tagIcon");
                    tagIcon.Src = "../Images/tag_small.gif";
                    tagIcon.ToolTip = "已领用标签";
                }

                if (user.AlertCount > 0)
                {
                    Img tagIcon = (Img)e.Item.FindControl("tagAlertIcon");
                    tagIcon.Src = "../Images/alert1.jpg";
                    tagIcon.ToolTip = string.Format("有{0}个报警", user.AlertCount);
                    tagIcon.Href = string.Format("/TagUsers/TagUser.aspx?type={0}&id={1}",
                    user.HostGroupId, user.HostId);
                }

                if (user.IsDisappeared < 0)
                {
                    Img tagIcon = (Img)e.Item.FindControl("tagDisappeared");
                    tagIcon.Src = "../Images/disappeared.jpg";
                    tagIcon.ToolTip = string.Format("消失");
                    tagIcon.Href = string.Format("/TagUsers/TagUser.aspx?type={0}&id={1}",
                    user.HostGroupId, user.HostId);
                }
            }
        }

    }
}