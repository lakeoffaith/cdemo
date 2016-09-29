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
namespace NetRadio.LocatingMonitor.Settings
{
    [MarshalAjaxRegister]
    public partial class __LocatingManager : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/LocatingManager.aspx.js");
            scriptFiles.Add("6", "App_Script/UI/TagSelector.ascx.js");
            
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

             

            if (!Page.IsPostBack)
            {
                LoadSurveyGroup();
                SetDefaultParameters();
                LoadRepeater();
               
            }
            SetConditionControlAccessibilities();
        }


        #region Property: IsLocatingServiceAvailable

        bool? _isLocatingServiceAvailable;
        bool IsLocatingServiceAvailable
        {
            get
            {
                if (_isLocatingServiceAvailable == null)
                {
                    _isLocatingServiceAvailable = LocatingServiceUtil.IsAvailable();
                    if (_isLocatingServiceAvailable == false)
                    {
                        locatingServiceState.Text = "系统未检测到 LocatingService 运行。";
                    }
                }
                return (bool)_isLocatingServiceAvailable;
            }
        }

        #endregion

        #region SetDefaultParameters

        private void SetDefaultParameters()
        {
            XDocument xDoc = XDocument.Load(Server.MapPath(PathUtil.ResolveUrl("Settings/LocateParameters.xml")));
            XElement root = xDoc.Element("Parameters");

            string[] keys = "LocatingMode,SurveyGroup,ScanMode,ScanTarget,RssiBackCount,ScanInterval,ScanChannels".Split(',');
            ListItemCollection[] itemsCollections = new ListItemCollection[] {
				locatingMode.Items,
				surveyGroup.Items,
				scanMode.Items,
				scanTarget.Items,
				rssiBackCount.Items,
				scanInterval.Items,
				scanChannels.Items
			};
            for (int i = 0; i < keys.Length; i++)
            {
                var value = root.Element(keys[i]).Value;
                var li = itemsCollections[i].FindByValue(value);
                if (li != null)
                {
                    li.Selected = true;
                }
            }

            scanSsid.Text = root.Element("ScanSsid").Value;
        }

        #endregion

        #region LoadSurveyGroup

        private void LoadSurveyGroup()
        {
            using (AppDataContext db = new AppDataContext())
            {
                surveyGroup.DataSource = SurveyGroup.All;
                surveyGroup.DataTextField = "SurveyGroupName";
                surveyGroup.DataValueField = "Id";
                surveyGroup.DataBind();
            }
        }

        #endregion

        #region SetConditionControlAccessibilities

        private void SetConditionControlAccessibilities()
        {
            if (locatingMode.SelectedIndex != 0)
            {
                surveyGroup.Attributes.Add("disabled", "disabled");
            }
            else
            {
                surveyGroup.Attributes.Remove("disabled");
            }

            if (scanMode.SelectedIndex != 0)
            {
                scanTarget.Attributes.Add("disabled", "disabled");
                scanSsid.Attributes.Add("disabled", "disabled");
            }
            else
            {
                scanTarget.Attributes.Remove("disabled");
                scanSsid.Attributes.Remove("disabled");
            }
        }

        #endregion
        protected void startLocat_Click(object sender, EventArgs e)
        {
            this.scanTime.Text = "9999";
        }
       
        #region startLocating_Click

        protected void startLocating_Click(object sender, EventArgs e)
        {
            if (!this.IsLocatingServiceAvailable)
            {
                feedbacks.Items.AddError("请求失败，请检查 LocatingService 是否可被连接。");
                LoadRepeater();
                return;
            }

            int[] tagIdArray = Strings.ParseToArray<int>(selectedTagIds.Value);
            if (tagIdArray.Length == 0)
            {
                feedbacks.Items.AddError("操作失败，没有选中任何记录行。");
                LoadRepeater();
                return;
            }
            

            IList<string> selectedChannels = new List<string>();
            foreach (ListItem item in scanChannels.Items)
            {
                if (item.Selected)
                {
                    selectedChannels.Add(item.Value);
                }
            }

            //自定将标签启动定位,从配置文件中加载数据
            XDocument xDoc = XDocument.Load(Server.MapPath(PathUtil.ResolveUrl("Settings/LocateParameters.xml")));
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


            //TagLocateSetting useSettingModel = new TagLocateSetting {
            //    LocatingMode = byte.Parse(locatingMode.SelectedValue),
            //    RssiBackCount = int.Parse(rssiBackCount.SelectedValue),
            //    ScanInterval = int.Parse(scanInterval.SelectedValue),
            //    ScanMode = 2,
            //    ScanSsid = scanSsid.Text.Trim(),
            //    ScanChannels = string.Join(",", selectedChannels.ToArray()),
            //    ScanTarget = byte.Parse(scanTarget.SelectedValue),
            //    SurveyGroupId = int.Parse(surveyGroup.SelectedValue),
            //    UpdateTime = DateTime.Now,
            //    CommandState = (byte)LocatingCommandState.WaitToStart
            //};
            if (Config.Settings.ProjectType == ProjectTypeEnum.WXFactory)
            {
                useSettingModel.ScanMode = 1;

                
                TagSetting useSetting = new TagSetting
                {
                    ScanMarkEnable=true,
                    ScanMarkDetectInterval=Convert.ToInt32(this.detectInterval.Text.Trim()),
                    ScanMarkScanTime=Convert.ToInt32(this.scanTime.Text.Trim()),
                    ScanMarkScanInterval=Convert.ToInt32(this.wscanInterval.Text.Trim()),
                    ScanMarkScanCount=Convert.ToInt32(this.scanCount.Text.Trim()),
                    ScanMarkGoodSignal=Convert.ToInt32(this.goodSignal.Text.Trim()),
                    ScanMarkLocatingInterval=Convert.ToInt32(this.locatingInterval.Text.Trim())                  
                    
                };
                foreach (var id in tagIdArray)
                {
                    if (id > 0)
                    {
                        using (AppDataContext db = new AppDataContext()) 
                        {
                            var dbTagSet = db.TagSettings.SingleOrDefault(t => t.TagId == id);
                            if (dbTagSet!=null)
                            {
                                dbTagSet.ScanMarkEnable = true;
                                dbTagSet.ScanMarkDetectInterval = useSetting.ScanMarkDetectInterval;
                                dbTagSet.ScanMarkScanTime = useSetting.ScanMarkScanTime;
                                dbTagSet.ScanMarkScanInterval=useSetting.ScanMarkScanInterval;
                                dbTagSet.ScanMarkScanCount = useSetting.ScanMarkScanCount;
                                dbTagSet.ScanMarkGoodSignal = useSetting.ScanMarkGoodSignal;
                                dbTagSet.ScanMarkLocatingInterval = useSetting.ScanMarkLocatingInterval;
                                db.SubmitChanges();
                            }
                            //else if (dbTagSet.ScanMarkDetectInterval != 0 && dbTagSet.ScanMarkGoodSignal != 0)
                            //{
                            //    this.detectInterval.Text = dbTagSet.ScanMarkDetectInterval.ToString();
                            //    this.scanTime.Text = dbTagSet.ScanMarkScanTime.ToString();
                            //    this.wscanInterval.Text = dbTagSet.ScanMarkScanInterval.ToString();
                            //    this.scanCount.Text = dbTagSet.ScanMarkScanCount.ToString();
                            //    this.goodSignal.Text = dbTagSet.ScanMarkGoodSignal.ToString();
                            //    this.locatingInterval.Text = dbTagSet.ScanMarkLocatingInterval.ToString();

                            //}
                        }
                    }
                }
                
            }
            TagLocateSetting.StartLocating(tagIdArray, useSettingModel);

            // Send a command to LocatingService.
            LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();

            // Reload List
            LoadRepeater();
        }

        #endregion

        #region stopLocating_Click

        protected void stopLocating_Click(object sender, EventArgs e)
        {
            if (!this.IsLocatingServiceAvailable)
            {
                feedbacks.Items.AddError("请求失败，请检查 LocatingService 是否可被连接。");
                LoadRepeater();
                return;
            }

            int[] tagIdArray = Strings.ParseToArray<int>(selectedTagIds.Value);
            if (tagIdArray.Length == 0)
            {
                feedbacks.Items.AddError("操作失败，没有选中任何记录行。");
                LoadRepeater();
                return;
            }

            TagLocateSetting.StopLocating(tagIdArray);

            // Send a command to LocatingService.
            LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();

            // Reload List
            LoadRepeater();
        }

        #endregion

        #region refresh_Click

        protected void refresh_Click(object sender, EventArgs e)
        {
            LoadRepeater();
        }

        #endregion

        #region filterButton_Click

        protected void filterButton_Click(object sender, EventArgs e)
        {
            LoadRepeater();
        }

        #endregion

        #region p_PageIndexChanged

        protected void p_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            p.PageIndex = e.NewPageIndex;
            LoadRepeater();
        }

        #endregion

        #region stateFilter_SelectedIndexChanged

        protected void stateFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            p.PageIndex = 1;
            LoadRepeater();
        }

        #endregion


        #region LoadRepeater

        private void LoadRepeater()
        {
            using (AppDataContext db = new AppDataContext())
            {
                var query = db.DBViewTagSettings
                    .Where(x => HostTag.All
                                .Where(u => u.TagId != 0)
                                .Select(u => u.TagId).ToArray()
                                .Contains((int)x.Id));//x.TagId));
                switch (stateFilter.SelectedIndex)
                {
                    default:
                    case 0:
                        break;
                    case 1:
                        query = query.Where(x => x.WorkingStatus == (byte)TagWorkingStatus.Locating);
                        break;
                    case 2:
                        query = query.Where(x => x.WorkingStatus != (byte)TagWorkingStatus.Locating);
                        break;
                    case 3:
                        query = query.Where(x => x.CommandState == null);
                        break;
                }

                string keyword = tagNameKeyword.Text == "关键字" ? string.Empty : tagNameKeyword.Text.Trim();
                if (keyword.Length != 0)
                    query = query.Where(x => x.HostName.Contains(keyword));

                if (groupSelector.SelectedGroupIdArray != null && groupSelector.SelectedGroupIdArray.Length > 0)
                {
                    var range = HostTagGroupStatus.GetCoveredTagIdArray(groupSelector.SelectedGroupIdArray);
                    query = query.Where(x => range.Contains((int)x.Id));//x.TagId));
                }

                p.RecordCount = query.Count();
                settingList.DataSource = query.OrderByDescending(x => x.HostName).Skip(p.RecordOffset).Take(p.PageSize).ToList();
                settingList.DataBind();
                //settingList.DataSource = query.Skip(p.RecordOffset).Take(p.PageSize).OrderBy(x => x.TagName).ToList();                
            }
        }

        #endregion

        #region settingList_ItemCreated

        protected void settingList_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DBViewTagSetting setting = e.Item.DataItem as DBViewTagSetting;

            if (setting != null)
            {
                Img icon = e.Item.FindControl("icon") as Img;
                icon.Src = CommonExtension.IdentityIcon((int)setting.Id);//TagId);


                Anchor tagName = (Anchor)e.Item.FindControl("tagName");
                SmartLabel userType = (SmartLabel)e.Item.FindControl("userType");
                SmartLabel userMemo = (SmartLabel)e.Item.FindControl("memo");
                tagName.Text = setting.HostName;

                HostTagGroupStatus tagUser = HostTagGroupStatus.SelectByTagId((int)setting.Id);//.TagId);
                int groupId = 0;
                if (tagUser != null)
                {
                    groupId = tagUser.HostGroupId;
                    userType.Text = BusTagUserType.GetTagUserTypeName(tagUser.HostGroupId);
                    userMemo.Text = tagUser.Description;
                }
                tagName.Href = PathUtil.ResolveUrl(string.Format("/TagUsers/TagUser.aspx?id={0}&type={1}",
                    setting.HostId, groupId));

                SmartLabel idSelection = (SmartLabel)e.Item.FindControl("idSelection");
                string disableOrNot = this.IsLocatingServiceAvailable ? "" : " disabled=\"disabled\"";
                idSelection.Text = "<input type=\"checkbox\" name=\"selection\"" + disableOrNot + " onclick=\"javascript:Locating.selectTag(this);\" value=\"" + setting.Id + "\" />";//setting.TagId + "\" />";


                if (setting.CommandState == null)
                {
                    return;	// <-----------------If never been configured ----------------------------------------------
                }

                SmartLabel commandState = (SmartLabel)e.Item.FindControl("commandState");
                DateTimeLabel startTime = (DateTimeLabel)e.Item.FindControl("startTime");
                Anchor operate = (Anchor)e.Item.FindControl("operate");

                switch ((LocatingCommandState)setting.CommandState)
                {
                    case LocatingCommandState.WaitToStart:
                        operate.Text = "N/A";
                        operate.CssClass = "t3";
                        commandState.Text = "请求开始中";
                        commandState.Style.Add("color", "Green");
                        break;

                    case LocatingCommandState.WaitToStop:
                        operate.Text = "N/A";
                        operate.CssClass = "t3";
                        commandState.Text = "请求停止中";
                        commandState.Style.Add("color", "Red");
                        break;

                    default:
                    case LocatingCommandState.Executed:
                        if (setting.WorkingStatus == (byte)TagWorkingStatus.Locating)
                        {
                            operate.Text = "停止";
                            operate.Href = "javascript:Locating.quickStop(" + setting.Id + ");"; //setting.TagId + ");";
                            startTime.DisplayValue = (DateTime)setting.UpdateTime;
                        }
                        else
                        {
                            operate.Text = "快速启动";
                            operate.ToolTip = "按照已设定参数快速启动";
                            operate.Href = "javascript:Locating.quickStart(" + setting.Id + ");"; //setting.TagId + ");";

                        }
                        operate.Attributes["id"] = "op_" + setting.Id;//setting.TagId;
                        break;
                }

                if (!this.IsLocatingServiceAvailable)
                {
                    operate.Text = "N/A";
                    operate.Href = null;
                    operate.CssClass = "t3";
                    operate.ToolTip = "系统未检测到LocatingServer运行。";
                }
            }
        }

        #endregion
    }
}
