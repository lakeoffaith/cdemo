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
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __AreaReport : BasePage
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
            if (!LocatingServiceUtil.IsAvailable())
            {
                ShowMessagePage("服务器程序未启动，无法获得实时监房统计。");
            }

            LocatingServiceUtil.DemandLocatingService();
            string areaname = Request.QueryString["areaname"].Trim();
            if (!string.IsNullOrEmpty(areaname))
            {
                list.DataSource = MapArea.All.Where(_d => _d.AreaName.Trim().Substring(0, 1) == areaname);
                list.DataBind();
            }
        }


        TagStatusView[] _fullTagStatusView;
        TagStatusView[] FullTagStatusView
        {
            get
            {
                if (_fullTagStatusView == null)
                {
                    int totalCount;
                    _fullTagStatusView = LocatingServiceUtil.Instance<IServiceApi>().SelectTagStatusList(
                        "",
                        new int[0],
                        0,
                        true,
                        false,
                        false,
                        false,
                        false,
                        false,
                        "TagName",
                        SortDirection.Ascending,
                        9999999,
                        0,
                        out totalCount
                    );
                }
                return _fullTagStatusView;
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            MapArea area = e.Item.DataItem as MapArea;
            if (area != null)
            {
                SmartLabel areaName = (SmartLabel)e.Item.FindControl("areaName");
                areaName.Text = area.AreaName;

                NumericLabel quota = (NumericLabel)e.Item.FindControl("quota");
                quota.Value = CulpritRoomReference.All.Count(x => x.JailRoomId == area.Id);

                NumericLabel bindingCount = (NumericLabel)e.Item.FindControl("bindingCount");

                NumericLabel currentCount = (NumericLabel)e.Item.FindControl("currentCount");
                SmartLabel expectedNames = (SmartLabel)e.Item.FindControl("expectedNames");
                SmartLabel illedNames = (SmartLabel)e.Item.FindControl("illedNames");

                var coordinates = MapAreaCoverage.All.Where(x => x.AreaId == area.Id).Select(x => x.CoordinatesId).ToArray();
                IList<TagStatusView> nowUsers = new List<TagStatusView>();
                foreach (var item in FullTagStatusView)
                {
                    if (coordinates.Contains(item.CoordinatesId) && HostTag.GetHostGroup(item.HostTag.HostId).Contains((int)TagUserType.Culprit) && item.AbsenceStatus != EventStatus.Occurring)
                    {
                        currentCount.Value++;
                        nowUsers.Add(item);
                    }
                }
                if (currentCount.Value != quota.Value)
                {
                    currentCount.CssClass = "t2";
                }
                IList<HostTag> shouldUsers = HostTag.AllActive
                    .Where(x => CulpritRoomReference.All.Where(r => r.JailRoomId == area.Id).Select(r => r.CulpritId).Contains(x.HostId))
                    .ToList();

                bindingCount.Value = shouldUsers.Count;

                IList<string> arr1 = new List<string>();
                //foreach (var item in nowUsers)
                //{
                //    if (shouldUsers.Any(x => x.TagId == item.TagId) == false)
                //    {
                //        arr1.Add("<a href='../Objects/Tag.aspx?id=" + item.TagId + "' target='_blank'>" + item.TagName + "</a>");
                //    }
                //}
                //extraNames.Text = string.Join(", ", arr1.ToArray());

                IList<string> arr2 = new List<string>();
                foreach (var item in shouldUsers)
                {
                    TagStatusView tagStatusView = LocatingServiceUtil.Instance<IServiceApi>().SelectTagStatus(item.TagId);

                    if (nowUsers.Any(x => x.TagId == item.TagId) == false)
                    {
                        int hostType = HostTagView.GetHostView(item.TagId).HostGroupId.Contains(1) ? 1 : 2;
                        arr2.Add("<a href='../TagUsers/TagUser.aspx?type=" + hostType + "&id=" + item.HostId + "' target='_blank'>" + item.HostName + "</a> : " + tagStatusView.CoordinatesName);
                    }

                    if (tagStatusView.HostTag.HostGroupId.Contains((int)TagUserType.IlledPrisoner))
                    {
                        arr1.Add("<a href='../TagUsers/TagUser.aspx?type=" + (int)TagUserType.Culprit + "&id=" + item.HostId + "' target='_blank'>" + item.HostName + "</a>");
                    }
                }
                expectedNames.Text = string.Join("<br>", arr2.ToArray());
                illedNames.Text = string.Join(", ", arr1.ToArray());
            }
        }

        protected void list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                int c0 = 0, c1 = 0, c2 = 0;
                for (int i = 0; i < list.Items.Count; i++)
                {
                    NumericLabel n0 = list.Items[i].FindControl("quota") as NumericLabel;
                    NumericLabel n1 = list.Items[i].FindControl("bindingCount") as NumericLabel;
                    NumericLabel n2 = list.Items[i].FindControl("currentCount") as NumericLabel;
                    c0 += Convert.ToInt32(n0.Value);
                    c1 += Convert.ToInt32(n1.Value);
                    c2 += Convert.ToInt32(n2.Value);

                }
                NumericLabel nn0 = e.Item.FindControl("quota0") as NumericLabel;
                NumericLabel nn1 = e.Item.FindControl("bindingCount0") as NumericLabel;
                NumericLabel nn2 = e.Item.FindControl("currentCount0") as NumericLabel;
                nn0.Value = c0;
                nn1.Value = c1;
                nn2.Value = c2;
            }
        }
    }
}
