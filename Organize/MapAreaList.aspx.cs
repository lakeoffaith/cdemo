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
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
namespace NetRadio.LocatingMonitor.Organize
{
    public partial class __MapAreaList : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!Page.IsPostBack)
            {
                LoadRepeater();

                if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                {
                    objectNavigator.Visible = true;
                }
                else
                {
                    
                    objectNavigator.Visible = false;
                }
            }
        }

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadRepeater();
        }

        protected void filterButton_Click(object sender, EventArgs e)
        {
            LoadRepeater();
        }

        protected void removeCache_Click(object sender, EventArgs e)
        {
            Caching.Remove(AppKeys.Cache_AllFacilities);
            Caching.Remove(AppKeys.Cache_AllMapAreas);
            Caching.Remove(AppKeys.Cache_AllMapAreaCoverages);
            Caching.Remove(AppKeys.Cache_AllSurveyGroups);
            LoadRepeater();
        }

        private void LoadRepeater()
        {
            IEnumerable<MapArea> source = MapArea.All;
            if (maps.SelectedMapId > 0)
            {
                source = source.Where(x => x.MapId == maps.SelectedMapId);
            }

            var query = source.OrderBy(x => x.MapId).OrderBy(x => x.AreaName);

            p.RecordCount = query.Count();
            areaList.DataSource = query.Skip(p.RecordOffset).Take(p.PageSize);
            areaList.DataBind();
        }

        protected void areaList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            MapArea area = e.Item.DataItem as MapArea;
            if (area != null)
            {
                Anchor areaName = (Anchor)e.Item.FindControl("areaName");
                SmartLabel facilityName = (SmartLabel)e.Item.FindControl("facilityName");
                //NumericLabel ruleCount = (NumericLabel)e.Item.FindControl("ruleCount");
                Anchor setRule = (Anchor)e.Item.FindControl("setRule");

                areaName.Text = area.AreaName;
                facilityName.Text = Facility.GetNameByMapId(area.MapId);
                areaName.Href = setRule.Href = "MapAreaRules.aspx?AreaId=" + area.Id;
            }
        }
    }
}
