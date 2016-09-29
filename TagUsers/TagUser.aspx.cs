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
using NetRadio.Business;
using System.Net.Mail;
namespace NetRadio.LocatingMonitor.TagUsers
{

    public partial class __TagUser : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");

            scriptFiles.Add("5", "App_Script/UI/TagUser.aspx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           
            int id = Fetch.QueryUrlAsInteger("id");

            int n = Fetch.QueryUrlAsIntegerOrDefault("type", -1);
            if (n <= 0)
            {
                try
                {
                    HostTagGroupStatus host = HostTagGroupStatus.SelectByHostId(id);
                    if (host != null)
                    {
                        n = host.HostGroupId;
                    }
                }
                catch (Exception err)
                {
                    Diary.Debug("TagUser.aspx: " + err.ToString());
                }

            }

            switch (n)
            {
                case 1:
                    Response.Redirect("Police.aspx?id=" + id);
                    break;

                case 2:
                    Response.Redirect("Culprit.aspx?id=" + id);
                    break;

                case 3:
                    Response.Redirect("Position.aspx?id=" + id);
                    break;

                default:
                    ShowMessagePage("访问无效。");
                    break;
            }
        }

        private static string GetGroupName(int groupId)
        {
            return BusTagUserType.GetTagUserTypeName(groupId);
        }

        private static void UpdateHostTag(HostTag userHost)
        {
            if (userHost != null)
            {
                HostTag.UpdateHostTag(userHost);
                //HostTag.AddOrUpdateHostTag(userHost.HostId, userHost.TagId, userHost.HostExternalId, userHost.HostName, (int)userHost.HostType, userHost.Description, userHost.ImagePath);
            }
        }

        private static void UpdateHostTag(HostTagGroupStatus userHost)
        {
            if (userHost != null)
            {
                HostTag hostTag = new HostTag();
                hostTag.HostId = userHost.HostId;
                hostTag.TagId = userHost.TagId;
                hostTag.HostExternalId = userHost.HostExternalId;
                hostTag.HostName = userHost.HostName;
                hostTag.Description = userHost.Description;
                hostTag.HostType = userHost.HostType;
                hostTag.ImagePath = userHost.ImagePath;
                HostTag.UpdateHostTag(hostTag);
            }
        }

        private static void SetStartLocating(int tagId)
        {
            //自定将标签启动定位
            XDocument xDoc = XDocument.Load(HttpContext.Current.Server.MapPath(PathUtil.ResolveUrl("Settings/LocateParameters.xml")));
            XElement root = xDoc.Element("Parameters");

            int surveyGroupId = int.Parse(root.Element("SurveyGroup").Value);
            using (AppDataContext db = new AppDataContext())
            {
                SurveyGroup surveryGroupValue = db.SurveyGroups.FirstOrDefault();
                if (surveryGroupValue != null)
                {
                    surveyGroupId = surveryGroupValue.Id;
                }
            }

            TagLocateSetting useSettingModel = new TagLocateSetting
            {
                LocatingMode = byte.Parse(root.Element("LocatingMode").Value),
                RssiBackCount = int.Parse(root.Element("RssiBackCount").Value),
                ScanInterval = int.Parse(root.Element("ScanInterval").Value),
                ScanMode = byte.Parse(root.Element("ScanMode").Value),
                ScanSsid = root.Element("ScanSsid").Value,
                ScanChannels = root.Element("ScanChannels").Value,
                ScanTarget = byte.Parse(root.Element("ScanTarget").Value),
                SurveyGroupId = surveyGroupId,
                UpdateTime = DateTime.Now,
                CommandState = (byte)LocatingCommandState.WaitToStart
            };

            if (Config.Settings.ProjectType == ProjectTypeEnum.WXFactory)
            {
                useSettingModel.ScanMode = 1;
            }
            TagLocateSetting.StartLocating(new int[] { tagId }, useSettingModel);


        }

        #region [AjaxMethod] ClearEventStatus

        [AjaxMethod]
        public static bool ClearEventStatus(string tagMac, string eventKeyword)
        {
            SupportEvent clearEvent;
            string str;

            switch (eventKeyword)
            {
                case "absence":
                    clearEvent = SupportEvent.Absent;
                    str = "消失";
                    break;
                case "areaEvent":
                    clearEvent = SupportEvent.AreaEvent;
                    str = "进出区域";
                    break;
                case "batteryInsufficient":
                    clearEvent = SupportEvent.BatteryInsufficient;
                    str = "低电量";
                    break;
                case "batteryReset":
                    clearEvent = SupportEvent.BatteryReset;
                    str = "电池重置";
                    break;
                case "buttonPressed":
                    clearEvent = SupportEvent.ButtonPressed;
                    str = "按钮";
                    break;
                case "wristletBroken":
                    clearEvent = SupportEvent.WristletBroken;
                    str = "腕带";
                    break;
                default:
                    return false;
            }

            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                bool boolean = serviceApi.ClearTagStatus(tagMac, 0);

                //记录日志
                using (AppDataContext db = new AppDataContext())
                {
                    Tag tag = db.Tags.SingleOrDefault(t => t.TagMac == tagMac);
                    if (tag != null)
                    {
                        Diary.Insert(ContextUser.Current.Id, tag.Id, TagStatusView.SelectTagStatus(tag.Id).HostTag.HostId, "清除" + tag.TagName + "的" + str + "报警状态。");
                    }
                }

                return boolean;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region [AjaxMethod] ClearAllEventStatus

        [AjaxMethod]
        public static bool ClearAllEventStatus(string tagMac)
        {
            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                bool boolean = serviceApi.ClearTagStatus(tagMac);

                //记录日志
                using (AppDataContext db = new AppDataContext())
                {
                    Tag tag = db.Tags.SingleOrDefault(t => t.TagMac == tagMac);
                    if (tag != null)
                    {
                        Diary.Insert(ContextUser.Current.Id, tag.Id, TagStatusView.SelectTagStatus(tag.Id).HostTag.HostId, "清除" + tag.TagName + "的所有报警状态。");//SecurityLog.Insert(tag.Id, TagUser.GetUserIdByTagId(tag.Id), "清除" + tag.TagName + "的所有报警状态。", Priority.High);
                    }
                }
                return boolean;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region [AjaxMethod] BindTag

        [AjaxMethod]
        public static string BindTag(int userId, int tagId)
        {
            //TagUser user = TagUser.SelectById(userId);
            string newMac = "FAIL";
            HostTagGroupStatus hostTag = HostTagGroupStatus.SelectByHostId(userId);

            if (hostTag != null)
            {
                //HostTag.ChangeHostTagBinding(userId, tagId);

                if (LocatingServiceUtil.IsAvailable())
                {
                    //LocatingServiceUtil.Instance<IServiceApi>().UpdateTagNameAndSerialNo(tagId, hostTag.HostName, hostTag.HostExternalId);
                    IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                    newMac = serviceApi.ExchangeTagMac(hostTag.HostId, 0, tagId);
                    SetStartLocating(tagId);

                    // Send a command to LocatingService.
                    LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();

                    string calling = GetGroupName(hostTag.HostGroupId);
                    //SecurityLog.Insert(tagId, userId, "为" + calling + hostTag.HostName + "绑定标签，标签MAC: " + newMac + "。", Priority.High);
                    Diary.Insert(ContextUser.Current.Id, tagId, hostTag.HostId, "为" + calling + hostTag.HostName + "绑定标签，标签MAC: " + newMac + "。");
                }
                //else
                //{
                //    SetStartLocating(tagId);
                //Tag.UpdateTagNameAndSerialNo(tagId, hostTag.HostName, hostTag.HostExternalId);
                //}
                //TagUser.BindTagToUser(userId, tagId);

            }
            return newMac;
        }


        #endregion

        #region [AjaxMethod] ChangeTag

        [AjaxMethod]
        public static string ChangeTag(int oldTagId, int newTagId)
        {
            string newMac = "FAIL";
            if (oldTagId == newTagId) return "";
            HostTagGroupStatus hostTag = HostTagGroupStatus.SelectByTagId(oldTagId);

            if (LocatingServiceUtil.IsAvailable())
            {
                //TagUser user = TagUser.SelectByTagId(oldTagId);
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                newMac = serviceApi.ExchangeTagMac(hostTag.HostId, oldTagId, newTagId);
                //无锡换地标
                if (Config.Settings.ProjectType == ProjectTypeEnum.WXFactory)
                {
                    Tag oldTag = Tag.Select(oldTagId);
                    Tag newTag = Tag.Select(newTagId);
                  
                    //更新缓存  
                    
                   IDictionary<string, APStatusView> dicAPStatus = APStatusView.SelectFullAPStatusView();
                   IServiceApi serApi = LocatingServiceUtil.Instance<IServiceApi>();
                   serApi.ExchangePositionTag(oldTag.TagMac,newTag.TagMac);
                   //更新数据库                  
                   AP.ChangeMac(oldTag.TagMac, newTag.TagMac);

                }

                
                    //自定将标签启动和停止定位
                    TagLocateSetting.StopLocating(new int[] { oldTagId });
                if (Config.Settings.ProjectType != ProjectTypeEnum.WXFactory)
                {
                    if (newTagId > 0)
                        SetStartLocating(newTagId);
                    else
                        newMac = "未携带标签";
                    // Send a command to LocatingService.
                    LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();
                }
                //记录日志
                string calling = GetGroupName(hostTag.HostGroupId);
                if (newTagId > 0)
                    Diary.Insert(ContextUser.Current.Id, newTagId, hostTag.HostId, "为" + calling + hostTag.HostName + "调换标签，新标签MAC: " + newMac + "。");
                else
                    Diary.Insert(ContextUser.Current.Id, newTagId, hostTag.HostId, "为" + calling + hostTag.HostName + "解除绑定标签，原标签MAC: " + hostTag.TagMac + "。");
                //SecurityLog.Insert(newTagId, hostTag.HostId, "为" + calling + hostTag.HostName + "调换标签，新标签MAC: " + newMac + "。", Priority.High);
            }
            /*else
            {
                HostTag.ChangeHostTagBinding(hostTag.HostId, newTagId);
                Tag oTag = Tag.Select(newTagId);
                newMac = oTag.TagMac;
                //自定将标签启动和停止定位
                TagLocateSetting.StopLocating(new int[] { oldTagId });
                SetStartLocating(newTagId);

                //记录日志
                string calling = GetGroupName(hostTag.HostGroupId);
                SecurityLog.Insert(hostTag.TagId, hostTag.HostId, "为" + calling + hostTag.HostName + "调换标签，新标签MAC: " + newMac + "。", Priority.High);
            }*/

            //AlertProcessLog.DeleteAlertProcessLog(hostTag.HostId);
            //TagAlert.DeleteTagAlerts(hostTag.HostId);

            return newMac;
        }
        #endregion

        #region [AjaxMethod] ChangeJailRoom

        [AjaxMethod]
        public static string ChangeJailRoom(int culpritId, int newJailRoomId)
        {

            //get Host
            HostTag hostTag = HostTag.GetById(culpritId);
            if (hostTag == null || hostTag.HostName == "")
            {
                return "";
            }

            //记录旧ID
            var oldJailRoomId = CulpritRoomReference.GetRoomIdByCulpritId(culpritId);

            //取得新监舍名称
            string newName = MapArea.All.Where(x => x.Id == newJailRoomId).Select(x => x.AreaName).SingleOrDefault();

            //判断是否相同
            if (oldJailRoomId == newJailRoomId)
            {
                return newName;
            }

            //执行更换
            CulpritRoomReference.ChangeRoom(culpritId, newJailRoomId);

            //警告条件绑定
            var isServiceAvailable = LocatingServiceUtil.IsAvailable();


            int ruleId = 0;
            int[] culpritIdArray = null;
            int[] tagIdArray = null;

            //旧监舍 -------------------------------------
            if (oldJailRoomId != 0)
            {
                //查询警告条件
                ruleId = AreaWarningRule.SelectRuleByAreaId(oldJailRoomId).Select(x => x.Id).FirstOrDefault();
                //如果警告条件不存在，则自动建立一条
                if (ruleId == 0)
                {
                    var rule = new AreaWarningRule
                    {
                        AreaEventType = (byte)AreaEventType.StayOutside,
                        AreaId = oldJailRoomId,
                        EnableToAllTags = false,
                        IsDisabled = false
                    };
                    if (isServiceAvailable)
                    {
                        ruleId = LocatingServiceUtil.Instance<IServiceApi>().InsertWarningRule(rule).Id;
                    }
                    else
                    {
                        ruleId = AreaWarningRule.InsertAndReturnId(rule);
                    }
                }
                //为警告条件设置关联标签
                culpritIdArray = CulpritRoomReference.All.Where(x => x.JailRoomId == oldJailRoomId).Select(x => x.CulpritId).ToArray();
                tagIdArray = HostTag.All.Where(x => culpritIdArray.Contains(x.HostId) && x.TagId > 0).Select(x => x.TagId).ToArray();
                if (isServiceAvailable)
                {
                    LocatingServiceUtil.Instance<IServiceApi>().SetWarningRuleCoverage(ruleId, tagIdArray);
                }
                else
                {
                    AreaWarningRuleCoverage.SetCoverage(ruleId, tagIdArray);
                }
            }

            //新监舍 -------------------------------------
            //查询警告条件
            ruleId = AreaWarningRule.SelectRuleByAreaId(newJailRoomId).Select(x => x.Id).FirstOrDefault();
            //如果警告条件不存在，则自动建立一条
            if (ruleId == 0)
            {
                var rule = new AreaWarningRule
                {
                    AreaEventType = (byte)AreaEventType.StayOutside,
                    AreaId = newJailRoomId,
                    EnableToAllTags = false,
                    IsDisabled = false
                };
                if (isServiceAvailable)
                {
                    ruleId = LocatingServiceUtil.Instance<IServiceApi>().InsertWarningRule(rule).Id;
                }
                else
                {
                    ruleId = AreaWarningRule.InsertAndReturnId(rule);
                }
            }
            //为警告条件设置关联标签
            culpritIdArray = CulpritRoomReference.All.Where(x => x.JailRoomId == newJailRoomId).Select(x => x.CulpritId).ToArray();
            tagIdArray = HostTag.All.Where(x => culpritIdArray.Contains(x.HostId) && x.TagId > 0).Select(x => x.TagId).ToArray();
            if (isServiceAvailable)
            {
                LocatingServiceUtil.Instance<IServiceApi>().SetWarningRuleCoverage(ruleId, tagIdArray);
            }
            else
            {
                AreaWarningRuleCoverage.SetCoverage(ruleId, tagIdArray);
            }


            //记录日志，返回
            Diary.Insert(ContextUser.Current.Id, 0, culpritId, "将犯人" + hostTag.HostName + "更换监舍到" + newName);
            return newName;
        }

        #endregion

        #region [AjaxMethod] ChangeName

        [Ajax.AjaxMethod]
        [NetRadio.Assistant.Web.Ajax.AjaxMethod]        
        public static bool ChangeName(int userId, string newName, string newNumber)
        {
            
            //var user = TagUser.All.SingleOrDefault(u => u.Id == userId);
            var userHost = HostTagGroupStatus.All.SingleOrDefault(h => h.HostId == userId && h.ParentGroupId == 0);
            if (userHost != null)
            {
                string oldName = userHost.HostName;
                string oldNumber = userHost.HostExternalId;

                userHost.HostName = newName;
                userHost.HostExternalId = newNumber;
                if (HostTag.All.Where(_d => _d.HostId != userId && _d.HostExternalId.Trim() == newNumber.Trim()).Count() > 0)
                {
                    throw new Exception("编号已经被使用！");
                }
                else
                {
                    UpdateHostTag(userHost);
                }
                if (userHost.TagId != 0)
                {
                    if (LocatingServiceUtil.IsAvailable())
                    {
                        IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                        serviceApi.UpdateTagNameAndSerialNo(userHost.TagId, newName, newNumber);
                    }
                    else
                    {
                        Tag.UpdateTagNameAndSerialNo(userHost.TagId, newName, newNumber);
                    }
                }

                //包含更新缓存
                //TagUser.UpdateNameAndNumber(userHost.Id, newName, newNumber);

                //var host = HostTag.AllActive.SingleOrDefault(h => h.HostId == userId);
                //host.HostName = newName;
                //host.HostExternalId = newNumber;
                if (Config.Settings.ProjectType == ProjectTypeEnum.WXFactory)
                {

                }


                //记录日志
                string str = "";
                if (oldName != newName)
                {
                    str += "名称由" + oldName + "改为" + newName + " ";
                }
                if (oldNumber != newNumber)
                {
                    str += "号码由" + oldNumber + "改为" + newNumber;
                }
                if (str != "")
                {
                    string calling = GetGroupName(userHost.HostGroupId);
                    Diary.Insert(ContextUser.Current.Id, 0, userHost.HostId, "修改" + calling + "信息: " + str);
                }

                return true;
            }
            return false;
        }

        #endregion

        #region [AjaxMethod] ChangeMemo

        [AjaxMethod]
        public static bool ChangeMemo(int userId, string newMemo)
        {
            //var host = HostTag.GetById(userId);
            //var user = TagUser.All.SingleOrDefault(u => u.Id == userId);
            var userHost = HostTagGroupStatus.All.SingleOrDefault(h => h.HostId == userId && h.ParentGroupId == 0);

            if (userHost != null)
            {
                //包含更新缓存
                //TagUser.UpdateMemo(user.Id, newMemo);
                userHost.Description = newMemo;
                //HostTag.UpdateHostTag(host);
                UpdateHostTag(userHost);

                //记录日志
                string calling = GetGroupName(userHost.HostGroupId);
                Diary.Insert(ContextUser.Current.Id, 0, userHost.HostId, "修改" + calling + userHost.HostName + "的备注信息。");

                return true;
            }
            return false;
        }



        #endregion

        [AjaxMethod]
        public static bool SetCulpritstatus(string tagMac, bool isIll, bool isSerious, bool isArraignment)
        {
            if (LocatingServiceUtil.IsAvailable())
            {
                TagStatusView tagView = TagStatusView.SelectTagStatus(tagMac);

                if (isIll)
                {
                    tagView.HostTag.AddHostGroup(4);
                    Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "将犯人" + tagView.HostTag.HostName + "设置为病犯。");
                }
                else
                {
                    if (tagView.HostTag.HostGroupId.Contains(4))
                    {
                        tagView.HostTag.RemoveHostGroup(4);
                        Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "取消犯人" + tagView.HostTag.HostName + "的病犯设置。");
                    }
                }

                if (isSerious)
                {
                    tagView.HostTag.AddHostGroup(3);
                    Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "将犯人" + tagView.HostTag.HostName + "设置为重刑犯。");
                }
                else
                {
                    if (tagView.HostTag.HostGroupId.Contains(3))
                    {
                        tagView.HostTag.RemoveHostGroup(3);
                        Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "取消犯人" + tagView.HostTag.HostName + "的病犯设置。");
                    }
                }

                try
                {
                    if (isArraignment)
                    {
                        if (tagView.HostTag.HostStatusId == (int)HostTagStatusType.Normal)
                        {
                            tagView.HostTag.HostStatusId = (int)HostTagStatusType.Interrogation;
                            HostTag.SetHostStatus(tagView.HostTag.HostId, (int)HostTagStatusType.Interrogation);
                            using (AppExtensionDataContext dbExtension = new AppExtensionDataContext())
                            {
                                InterrogationLog interrogationLog = new InterrogationLog();
                                interrogationLog.PoliceId = ContextUser.Current.Id;
                                interrogationLog.CulpritId = tagView.HostTag.HostId;
                                interrogationLog.StartTime = DateTime.Now;
                                dbExtension.InterrogationLogs.InsertOnSubmit(interrogationLog);
                                dbExtension.SubmitChanges();
                            }
                            Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "提审犯人" + tagView.HostTag.HostName + "。");
                        }
                    }
                    else
                    {
                        if (tagView.HostTag.HostStatusId == (int)HostTagStatusType.Interrogation)
                        {
                            tagView.HostTag.HostStatusId = (int)HostTagStatusType.Normal;
                            HostTag.SetHostStatus(tagView.HostTag.HostId, (int)HostTagStatusType.Normal);
                            using (AppExtensionDataContext dbExtension = new AppExtensionDataContext())
                            {
                                InterrogationLog interrogationLog = dbExtension.InterrogationLogs.SingleOrDefault(t => t.PoliceId == ContextUser.Current.Id && t.CulpritId == tagView.HostTag.HostId && t.EndTime == null);
                                if (interrogationLog != null)
                                {
                                    interrogationLog.EndTime = DateTime.Now;
                                    dbExtension.SubmitChanges();
                                }

                            }
                            Diary.Insert(ContextUser.Current.Id, tagView.TagId, tagView.HostTag.HostId, "犯人" + tagView.HostTag.HostName + "结束提审。");
                        }
                    }
                }
                catch (Exception exp)
                {
                    string err = exp.Source + ":" + exp.Message;
                }

                LocatingServiceUtil.Instance<IServiceApi>().ReloadTagHost(tagView.TagId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
