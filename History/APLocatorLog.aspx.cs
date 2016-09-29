using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
namespace NetRadio.LocatingMonitor.History
{
    public partial class __APLocatorLog : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        private static string _zSortKey = "WriteTime";
        private static SortDirection _sortDir = SortDirection.Descending;

        protected void Page_Load(object sender, EventArgs e)
        {
           

            if (!IsPostBack)
            {
                LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            //Sitemap.Text2 = "历史事件";
            //Sitemap.Text3 = this.Wrap.Title;
            this.SetSortButtonPresentation();
            this.LoadRepeater();
        }

        private SortButton _activatedSorter;

        private void LoadRepeater()
        {
            using (AppDataContext db = new AppDataContext())
            {
                IList<APLocatorLogView> query = db.APLocatorLogViews
                    .OrderByDescending(x => x.WriteTime)
                    .Select(x => new APLocatorLogView
                    {
                        APName = x.APName,
                        APMac = x.APMac,
                        APLocatorStatus = x.APLocatorStatus,
                        CoordinatesName = x.CoordinatesName,
                        MapId = x.MapId,
                        FacilityName = x.FacilityName,
                        WriteTime = x.WriteTime
                    }).ToList();


                //query = query.Where(x => x.WriteTime > tagLogFilter.FromTime);
                //query = query.Where(x => x.WriteTime < tagLogFilter.ToTime);

                /*if (tagLogFilter.TagNameKeyword.Length > 0)
                {
                    query = query.Where(x => x.TagName.Contains(tagLogFilter.TagNameKeyword));
                }

                if (tagLogFilter.SelectedGroupIdArray != null)
                {
                    int[] coveredTagIdArray = TagGroupCoverage.GetCoveredTagIdArray(tagLogFilter.SelectedGroupIdArray);
                    query = query.Where(x => coveredTagIdArray.Contains(x.TagId));
                }*/

                //if (tagLogFilter.FacilityFilterRowVisible && tagLogFilter.MapId > 0)
                //{
                //    query = query.Where(x => x.MapId == tagLogFilter.MapId);
                //}

                //if (_activatedSorter.SortDirection == SortDirection.Ascending)
                if (_sortDir == SortDirection.Ascending)
                {
                    //if (_activatedSorter.SortKey == "APName")
                    if (_zSortKey == "APName")
                    {
                        query = query.OrderBy(x => x.APName).ToList();
                    }
                    else
                    {
                        query = query.OrderBy(x => x.WriteTime).ToList();
                    }
                }

                else
                {
                    //if (_activatedSorter.SortKey == "APName")
                    if (_zSortKey == "APName")
                    {
                        query = query.OrderByDescending(x => x.APName).ToList();
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.WriteTime).ToList();
                    }
                }

                p.RecordCount = query.Count();
                query = query.Skip(p.RecordOffset).Take(p.PageSize).ToList();

                list.DataSource = query.ToList();
                list.DataBind();
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            APLocatorLogView log = e.Item.DataItem as APLocatorLogView;
            if (log != null)
            {
                Anchor apName = e.Item.FindControl("apName") as Anchor;
                apName.Text = string.Format("{0}[{1}]", log.APName, log.APMac);
                //apName.Href = PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + log.TagId);

                Anchor apStatus = e.Item.FindControl("apStatus") as Anchor;
                if (log.APLocatorStatus == 1)
                    apStatus.Text = "重现";
                else if (log.APLocatorStatus == 0)
                    apStatus.Text = "消失";

                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
                writeTime.DisplayValue = log.WriteTime;

                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                facilityName.Text = log.FacilityName;

                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                coordinatesName.Text = log.CoordinatesName;
            }
        }

        #region SetSortButtonPresentation
        private void SetSortButtonPresentation()
        {
            SortButton[] sortButtons = { apNameSorter, updateTimeSorter };
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
            Response.Redirect(Request.RawUrl);
            //Terminator.Redirect(Request.Path);
        }
        #endregion

        //#region SetSortButtonPresentation

        //private void SetSortButtonPresentation()
        //{
        //    string sortField = Fetch.QueryUrl("sortField");
        //    int sortDirection = Fetch.QueryUrlAsIntegerOrDefault("sortDirection", 0);

        //    if (sortField.Length == 0)
        //    {
        //        apNameSorter.Activated = true;
        //        _activatedSorter = updateTimeSorter;
        //        return;
        //    }

        //    SortButton[] sortButtons = { apNameSorter, updateTimeSorter };
        //    foreach (var button in sortButtons)
        //    {
        //        if (button.SortKey == sortField)
        //        {
        //            button.Activated = true;
        //            button.SortDirection = (SortDirection)sortDirection;
        //            _activatedSorter = button;
        //            continue;
        //        }
        //        button.Activated = false;
        //    }
        //}

        //#endregion

        //#region sorter_Click

        //protected void sorter_Click(object sender, EventArgs e)
        //{
        //    var button = (SortButton)sender;
        //    if (button.Activated)
        //    {
        //        button.SwitchSortDirection();
        //    }
        //    Terminator.Redirect(Request.Path + "?sortField=" + button.SortKey + "&sortDirection=" + (byte)button.SortDirection);
        //}

        //#endregion
    }
}
