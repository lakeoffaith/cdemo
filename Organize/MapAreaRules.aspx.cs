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
    public partial class __MapAreaRules : BasePage
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
        public __MapAreaRules()
        {
            _id = Fetch.QueryUrlAsInteger("areaid");
        }

        int _id;
        string sTagID = "";
        string errMsg = "";
        IList<_temp> _list;
        private class _temp
        {
            public int RuleId;
            public int TagId;
            public string HostName;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers2);
            }
            ruleList.ItemDataBound += new RepeaterItemEventHandler(ruleList_ItemDataBound);
            //var list=(select new { a=1,b=2}).
            /////////////////////tagSelector.SelectedGroupId = -1;

            int deleteRuleId = Fetch.QueryUrlAsIntegerOrDefault("deleteRuleId", -1);
            if (deleteRuleId != -1)
            {
                if (!LocatingServiceUtil.IsAvailable())
                {
                    AreaWarningRule.Delete(deleteRuleId);
                    AreaWarningRuleCoverage.DeleteByRuleId(deleteRuleId);
                }
                else
                {
                    IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                    serviceApi.DeleteWarningRule(deleteRuleId);
                    serviceApi.DeleteWarningRuleCoverage(deleteRuleId);
                }
                Terminator.Redirect(Fetch.Referrer);
            }

            MapArea area = MapArea.Select(_id);
            areaName.Text = area.AreaName;
            facilityName.Text = Facility.GetNameByMapId(area.MapId);

            IList<AreaWarningRule> _p = AreaWarningRule.SelectRuleByAreaId(area.Id);
            IList<AreaWarningRuleCoverage> _c = AreaWarningRuleCoverage.GetAreaWarningRuleCoverages();
            _list = (from _p1 in _p
                     join _c1 in _c
                     on _p1.Id equals _c1.RuleId
                     join _h1 in HostTag.All
                     on _c1.TagId equals _h1.TagId
                     select new _temp
                     {
                         RuleId = _c1.RuleId,
                         TagId = _c1.TagId,
                         HostName = _h1.HostName
                     }).ToList();

            ruleList.DataSource = _p;
            ruleList.DataBind();
            ruleCount.Value = ruleList.Items.Count;

            if (forAllTags.SelectedValue == "1")
            {
                tagSelectorContainer.Style.Add("display", "none");
            }
        }

        void ruleList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label _labPeople = e.Item.FindControl("labPeople") as Label;
            if (_labPeople != null)
            {
                AreaWarningRule _awr = e.Item.DataItem as AreaWarningRule;
                IEnumerable<_temp> awrs = _list.Where(_d => _d.RuleId == _awr.Id);
                _labPeople.Text = "";
                for (int i = 0; i < awrs.Count(); i++)
                {
                    if (i == 0)
                    { _labPeople.Text += awrs.ElementAt(i).HostName; }
                    else
                    { _labPeople.Text += "，" + awrs.ElementAt(i).HostName; }
                }
                if (_labPeople.Text.Length != 0)
                {
                    _labPeople.Text = "{&nbsp;" + _labPeople.Text + "&nbsp;}";
                }

            }
        }

        protected void ruleList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            AreaWarningRule rule = e.Item.DataItem as AreaWarningRule;
            if (rule != null)
            {
                SmartLabel ruleDescription = (SmartLabel)e.Item.FindControl("ruleDescription");
                var str = string.Empty;
                if (rule.EnableToAllTags)
                {
                    str = "所有标签";
                }
                else
                {
                    str = "指定的标签";

                }

                if (rule.AreaEventType == (byte)AreaEventType.StayInside)
                {
                    str += "进入停留在区域内时";
                }
                else
                {
                    str += "离开区域或停留在区域外时";
                }
                ruleDescription.Text = str + "触发告警";

                Anchor deleteRule = (Anchor)e.Item.FindControl("deleteRule");
                deleteRule.Href = "?areaid=" + _id + "&deleteRuleId=" + rule.Id;
            }
        }

        protected void saveNewRule_Click(object sender, EventArgs e)
        {
            AreaWarningRule rule = new AreaWarningRule();
            rule.EnableToAllTags = forAllTags.SelectedValue == "1";
            rule.AreaEventType = byte.Parse(areaEventType.SelectedValue);
            rule.AreaId = _id;
            rule.IsDisabled = false;

            int[] tagidarray = new int[tagSelector.SelectedUserIds.Count()];
            ArrayList alHostId = new ArrayList();
            int[] hostIdArray = null;
            sTagID = "";
            for (int i = 0; i < tagSelector.SelectedUserIds.Count(); i++)
            {
                //根据hostid取得tagid
                HostTag ht = new HostTag();
                ht = HostTag.GetById(tagSelector.SelectedUserIds[i]);
                if (ht != null)
                {
                    alHostId.Add(ht.HostId);
                    sTagID += ht.TagId + ",";
                    tagidarray[i] = ht.TagId;
                }
            }
            hostIdArray = (int[])alHostId.ToArray(typeof(int));
            AreaWarningRule.sTagID = sTagID;

            if (!LocatingServiceUtil.IsAvailable())
            {
                AreaWarningRule.InsertAndReturnId(rule);
                if (rule.EnableToAllTags == false)
                {
                    //AreaWarningRuleCoverage.SetCoverage(rule.Id, hostIdArray);
                    AreaWarningRuleCoverage.SetCoverage(rule.Id, tagidarray);
                    //AreaWarningRuleCoverage.SetCoverage(rule.Id, tagSelector.SelectedTagIdArray);
                }
            }
            else
            {

                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                EditResult result = serviceApi.InsertWarningRule(rule);
                rule.Id = result.Id;
                if (rule.EnableToAllTags == false)
                {
                    serviceApi.SetWarningRuleCoverage(rule.Id, tagidarray);
                    //serviceApi.SetWarningRuleCoverage(rule.Id, tagSelector.SelectedTagIdArray);
                }
            }

            Terminator.Redirect(Fetch.CurrentUrl);
        }
    }
}
