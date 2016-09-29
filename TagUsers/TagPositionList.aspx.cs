using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Web;
using NetRadio.LocatingService.RemotingEntry;
using NetRadio.Common.LocatingMonitor;

namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __TagPositionList : BasePage
    {
        private TagUserType _userType;
        private static SortDirection _sortDir = SortDirection.Descending;
        private static string _zSortKey = "HostName";

        protected void Page_Load(object sender, EventArgs e)
        {
            _userType = TagUserType.Position;

            if (!IsPostBack)
            {
                this.LoadRepeater();
            }
            addNew.Text = "添加新定点报警标签到系统";
            addNew.Href = "TagUser_Add.aspx?type=" + (byte)TagUserType.Position;


        }

        private void LoadRepeater()
        {
            using (AppDataContext db = new AppDataContext())
            {
                var query = db.HostTagGroupStatus.Where(u => u.HostGroupId == (int)_userType && u.ParentGroupId == 0);

                if (!string.IsNullOrEmpty(keyword.Text.Trim()))
                {
                    query = query.Where(u => u.HostName.Contains(keyword.Text.Trim()));
                }

                p.RecordCount = query.Count();
                if (_sortDir == SortDirection.Ascending)
                {
                    query = query.OrderBy(u => u.HostName);
                }
                else
                {
                    query = query.OrderByDescending(u => u.HostName);
                }
                list.DataSource = query.Skip(p.RecordOffset).Take(p.PageSize).ToList();
                list.DataBind();

             
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            HostTagGroupStatus user = e.Item.DataItem as HostTagGroupStatus;

            if (user != null)
            {
                Anchor name = (Anchor)e.Item.FindControl("name");
                name.Text = user.HostName;
                name.ToolTip = user.HostName;
                
                name.Href = "TagUser.aspx?type=" + user.HostGroupId + "&id=" + user.HostId;

                if (user.TagId != 0)
                {
                    Tag tag = Tag.Select(user.TagId);
                    SmartLabel smac = (SmartLabel)e.Item.FindControl("mac");
                    smac.Text = tag.TagMac.Substring(9);
                }
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            this.LoadRepeater();
        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            this.LoadRepeater();
        }

        #region SetSortButtonPresentation

        private void SetSortButtonPresentation()
        {
            SortButton[] sortButtons = { hostNameSorter };
            foreach (var button in sortButtons)
            {
                if (button.SortKey == _zSortKey)
                {
                    button.Activated = true;
                    button.SortDirection = _sortDir;
                    continue;
                }
                button.Activated = false;
            }
        }

        #endregion

        #region sorter_Click

        protected void sorter_Click(object sender, EventArgs e)
        {
            var button = (SortButton)sender;
            if (button.Activated)
            {
                button.SwitchSortDirection();
            }
            _zSortKey = button.SortKey;
            _sortDir = button.SortDirection;

            Terminator.Redirect(Request.Path + "?type=" + ((int)_userType).ToString());
        }

        #endregion

    }
}
