using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ajax;
using System.Text;

using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using Summer;
using NetRadio.Model;
using NetRadio.Business;

namespace NetRadio.LocatingMonitor.Organize
{
    public partial class __MapAreaRule : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__MapAreaRule));

            if (!IsPostBack)
            {
                tagSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers5);
            }
        }
        [Ajax.AjaxMethod]
        public static string GetHTML(int mapID)
        {
            AppDataContext db = new AppDataContext();
            MapArea[] mas = mapID == 0 ? db.MapAreas.ToArray() : db.MapAreas.Where(_d => _d.MapId == mapID).ToArray();
            AreaWarningRule[] aws = db.AreaWarningRules.Where(_d => mas.Select(_e => _e.Id).Contains(_d.AreaId)).ToArray();
            IEnumerable<AreaWarningRuleCoverage> awrcs = db.AreaWarningRuleCoverages.Where(_d => aws.Select(_e => _e.Id).Contains(_d.RuleId));

            var q = (from _d1 in db.HostTags
                     join _d2 in awrcs
                     on _d1.TagId equals _d2.TagId
                     select new
                     {
                         ruleId = _d2.RuleId,
                         hostName = _d1.HostName,
                         tagId = _d1.TagId
                     }
                   ).ToArray();

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
            <table cellpadding=""0"" cellspacing=""0"" class=""grid alternate fixed"">
                <thead class=""category"">
                     <th width=""200"" style=""text-align: center"">
                       区域名称
                    </th>
                         <th width=""200"" style=""text-align: center"">
                       进入区域告警
                    </th>
                       <th style=""text-align: center"">
                       离开区域告警
                    </th>
                </thead>
            ");
            int i = 0;
            foreach (MapArea ma in mas)
            {
                i++;
                //进
                AreaWarningRule idAWRsIN = aws.Where(_d => _d.AreaId == ma.Id && _d.AreaEventType == 0).FirstOrDefault();
                //出
                AreaWarningRule idAWRsOUT = aws.Where(_d => _d.AreaId == ma.Id && _d.AreaEventType == 1).FirstOrDefault();

                string inContext = "";
                string outContext = "";
                int idRuleIN = 0;
                int idRuleOUT = 0;
                string inTagIds = "";
                string outTagIds = "";
                int flag = 0;
                //进
                if (idAWRsIN != null)
                {
                    idRuleIN = idAWRsIN.Id;
                    if (idAWRsIN.EnableToAllTags)
                    {
                        inContext = "全部";
                        inTagIds = "";
                    }
                    else
                    {
                        flag = 0;
                        foreach (var order in q.Where(_d => _d.ruleId == idAWRsIN.Id))
                        {
                            inContext += order.hostName + ";";
                            if (flag == 0)
                            {
                                inTagIds += order.tagId;
                            }
                            else
                            {
                                inTagIds += "," + order.tagId;
                            }
                            flag++;
                        }
                    }
                }

                //出
                if (idAWRsOUT != null)
                {
                    idRuleOUT = idAWRsOUT.Id;
                    if (idAWRsOUT.EnableToAllTags)
                    {
                        outContext = "全部";
                        outTagIds = "";
                    }
                    else
                    {
                        flag = 0;
                        foreach (var order in q.Where(_d => _d.ruleId == idAWRsOUT.Id))
                        {
                            outContext += order.hostName + ";";
                            if (flag == 0)
                            {
                                outTagIds += order.tagId;
                            }
                            else
                            {
                                outTagIds += "," + order.tagId;
                            }
                            flag++;
                        }
                    }
                }

                inTagIds = "'" + inTagIds + "'";
                outTagIds = "'" + outTagIds + "'";
                // areaid, ruleid ,inOut, tagids
                sb.AppendFormat(@"
                <tr>
                    <td width=""200""  style=""text-align: center"">
                         {0} 
                    </td>
                    <td width=""200""  style=""text-align: center"">  
                            <table width=200 align=center class=inTable>
                            <tr>
                            <td valign=top width=150>{1}</td>
                            <td valign=top width=50> <input  type=button value=""设置"" onclick=""return btnSet_onclick({3},{4},0,{6})"" /></td>
                            </tr>
                            </table>
                    </td>
                    <td style=""text-align: center"">                
                            <table width=200 align=center class=inTable>
                            <tr>
                            <td valign=top width=150>{2}</td>
                            <td valign=top width=50><input  type=button value=""设置"" onclick=""return btnSet_onclick({3},{5},1,{7})"" /></td>
                            </tr>
                            </table>
                    </td>
                </tr>
            ", ma.AreaName, inContext, outContext, ma.Id, idRuleIN, idRuleOUT, inTagIds, outTagIds);
            }


            if (i == 0)
            {
                sb.AppendFormat(@"
                <tr>
                    <td colspan=""3"">                
                       无数据记录
                    </td>
                </tr>");
            }
            sb.Append(@"</table>");
            return sb.ToString();
        }


        [Ajax.AjaxMethod]
        public static object UpdateData(int areaid, int ruleid, int areaInOut, string tagIDs, bool forAll)
        {

            int[] _tagIDs = new int[0];
            if (tagIDs.Trim().Length > 0)
            {
                _tagIDs = Array.ConvertAll<string, int>(tagIDs.Split(new char[] { ',' }), delegate(string x) { return Convert.ToInt32(x); });
            }

            if (ruleid == 0)
            {
                Summer.Transaction t1 = new Transaction();
                try
                {
                    Model.plugin_AreaWarningRule rule1 = new plugin_AreaWarningRule();
                    rule1.AreaEventType = (byte)areaInOut;
                    rule1.AreaId = areaid;
                    rule1.EnableToAllTags = forAll;
                    t1.SaveEntity(rule1);
                    ruleid = rule1.Id;
                    if (!forAll)
                    {
                        foreach (int tagid in _tagIDs)
                        {
                            plugin_AreaWarningRuleCoverage rc1 = new plugin_AreaWarningRuleCoverage();
                            rc1.RuleId = rule1.Id;
                            rc1.TagId = tagid;
                            t1.InsertEntity(rc1);
                        }
                    }
                }
                catch (Exception e1)
                {
                    t1.Rollback();
                    throw e1;
                }
                t1.Commit();
            }
            else
            {
                Summer.Transaction t2 = new Transaction();
                try
                {
                    Model.plugin_AreaWarningRule rule2 = new plugin_AreaWarningRule();
                    rule2.Id = ruleid;
                    rule2.AreaEventType = (byte)areaInOut;
                    rule2.AreaId = areaid;
                    rule2.EnableToAllTags = forAll;
                    t2.SaveEntity(rule2);


                    DeleteBatch<plugin_AreaWarningRuleCoverage> delBat = new DeleteBatch<plugin_AreaWarningRuleCoverage>();
                    delBat.Where = (new Condition<plugin_AreaWarningRuleCoverage>()).EqualTo(plugin_AreaWarningRuleCoverage.__RuleId, ruleid);
                    t2.ExecuteDeleteBatch(delBat);

                    if (!forAll)
                    {
                        foreach (int tagid in _tagIDs)
                        {
                            plugin_AreaWarningRuleCoverage rc1 = new plugin_AreaWarningRuleCoverage();
                            rc1.RuleId = rule2.Id;
                            rc1.TagId = tagid;
                            t2.InsertEntity(rc1);
                        }
                    }
                }
                catch (Exception e2)
                {
                    t2.Rollback();
                    throw e2;
                }
                t2.Commit();

            }
            return new { RuleID = ruleid };
        }

    }
}
