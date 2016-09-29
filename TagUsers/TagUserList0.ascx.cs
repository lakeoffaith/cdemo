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
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __TagUserList0 : NetRadio.Web.BaseUserControl
    {
        private static string _zSortKey = "HostName";
        private static SortDirection _sortDir = SortDirection.Descending;
        private TagUserType _userType;

        protected void Page_Load(object sender, EventArgs e)
        {
            int actionNum = (int)GetDateItem(0);
            _userType = (TagUserType)GetDateItem(1);

            //this.SetSortButtonPresentation();
            switch (_userType)
            {
                case TagUserType.Cop:
                    addNew.Text = "增加使用者";
                    addNew.Href = "TagUser_Add.aspx?type=" + (byte)TagUserType.Cop;
                    jailRoomCondition.Visible = false;

                   
                    break;

                case TagUserType.Culprit:
                    addNew.Text = "增加使用者";
                    addNew.Href = "TagUser_Add.aspx?type=" + (byte)TagUserType.Culprit;
                    jailRoomCondition.Visible = true;
                    if (!Page.IsPostBack)
                    {
                        LoadJailRoomDropList();

                    }
                    break;
                case TagUserType.Position:
                    jailRoomCondition.Visible = false;

                    break;
                    

                default:
                    break;
            }

            if (Business.BusSystemConfig.IsLoadHostInfo())
            {
                importUsers.Visible = true;
            }
            else
            {
                importUsers.Visible = false;
            }
        }

        private void LoadJailRoomDropList()
        {
            jailRoom.Items.Clear();

            jailRoom.DataSource = MapArea.All;
            jailRoom.DataTextField = "AreaName";
            jailRoom.DataValueField = "Id";
            jailRoom.DataBind();

            jailRoom.Items.Insert(0, new ListItem("不限", "-1"));
        }

      

        protected void searchButton_Click(object sender, EventArgs e)
        {
            /////////////////this.LoadRepeater();
        }



        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            // p.PageIndex = e.NewPageIndex;
            //this.LoadRepeater();
        }

        #region SetSortButtonPresentation

        private void SetSortButtonPresentation()
        {
            //SortButton[] sortButtons = { hostNameSorter };
            //foreach (var button in sortButtons)
            //{
            //    if (button.SortKey == _zSortKey)
            //    {
            //        button.Activated = true;
            //        button.SortDirection = _sortDir;
            //        continue;
            //    }
            //    button.Activated = false;
            //}
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
            Response.Redirect(Request.Path + "?type=" + ((int)_userType).ToString());
            //Terminator.Redirect(Request.Path + "?type=" + ((int)_userType).ToString());
        }

        #endregion
    }
}