using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using NetRadio.LocatingService.RemotingEntry;
using System.Xml.Linq;
using NetRadio.Web;
using Summer;
using NetRadio.Model;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.TagUsers
{

    public partial class __TagUser_Add : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            if (BusSystemConfig.IsAutoSelectStrongestRssiTag() == false)
            {
                scriptFiles.Add("5", "App_Script/UI/SelectTag.ascx.js");
            }
            scriptFiles.Add("6", "App_Script/Control.js");
            scriptFiles.Add("7", "App_Script/UI/SelectStrongestRssiTag.ascx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        public __TagUser_Add()
        {
            int n = Fetch.QueryUrlAsIntegerOrDefault("type", -1);
            switch (n)
            {
                case 1:
                case 2:
                case 3:
                    _userType = (TagUserType)n;
                    break;

                default:
                    ShowMessagePage("访问无效。");
                    break;
            }
        }

        TagUserType _userType;

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                LoadGroupList();
                if (BusSystemConfig.IsAutoSelectStrongestRssiTag())
                {
                    tagSelector.Visible = false;
                    tagSelectorAuto.Visible = true;                   
                }
                else
                {
                    tagSelector.Visible = true;
                    tagSelectorAuto.Visible = false;
                }

                if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                {
                    objectNavigator.Visible = true;
                }
                else
                {
                    objectNavigator.Visible = false;
                }
            }
            switch (_userType)
            {
                default:
                    break;

                case TagUserType.Cop:
                    nameCalling.Text = "名称";
                    numberCalling.Text = "编号";
                    culpritRoomRow.Visible = false;
                    //number.MaxLength = 6;
                    break;

                case TagUserType.Culprit:
                    nameCalling.Text = "名称";
                    numberCalling.Text = "编号";
                    culpritRoomRow.Visible = true;
                    //number.MaxLength = 5;

                    if (!Page.IsPostBack)
                    {
                        LoadJailRoomDropList();
                    }
                    break;

                case TagUserType.Position:
                    nameCalling.Text = "定点标签名称";
                    numberCalling.Text = "标签编号";
                    culpritRoomRow.Visible = false;
                    numberCallingRow.Visible = false;
                    
                    photoRow.Visible = false;
                    grouplistRow.Visible = false;
                    break;
            }
            //tagSelector.AllowedSelectCount = 1;
        }

        private void LoadGroupList()
        {
            var hostGroupName = HostGroupInfo.GetByParentId((int)_userType);
            if (hostGroupName == null || hostGroupName.Count == 0)
            {
                grouplist.Items.Insert(0, new ListItem("无", "0"));
            }
            else
            {
                foreach (var name in hostGroupName)
                {
                    grouplist.Items.Add(new ListItem(name.HostGroupName, name.HostGroupId.ToString()));
                }
                grouplist.Items.Insert(0, new ListItem("无", "0"));
            }
        }

        private void LoadJailRoomDropList()
        {
            culpritRoom.Items.Clear();

            culpritRoom.DataSource = MapArea.All;
            culpritRoom.DataTextField = "AreaName";
            culpritRoom.DataValueField = "Id";
            culpritRoom.DataBind();

            culpritRoom.Items.Insert(0, new ListItem("选择所在监舍", "-1"));
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (name.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("没有填写" + nameCalling.Text + "。");
            }
            //if (_userType == TagUserType.Cop && number.Text.Trim().Length != 6)
            //{
            //    feedbacks.Items.AddError(numberCalling.Text + "必须是6位数字。");
            //}
            //if (_userType == TagUserType.Culprit && number.Text.Trim().Length != 5)
            //{
            //    feedbacks.Items.AddError(numberCalling.Text + "必须是5位数字。");
            //}
            if (_userType != TagUserType.Position && number.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("请输入编号");
            }

            if (_userType == TagUserType.Culprit && culpritRoom.Items.Count > 0 && culpritRoom.SelectedIndex < 1)
            {
                feedbacks.Items.AddError("没有选择该犯人所在的监舍。");
            }
            if (feedbacks.HasItems)
            {
                return;
            }

            //yyang 090916
            //if (TagUser.All.Any(x => string.Compare(x.Number, number.Text.Trim(), true) == 0))
            //{
            //    feedbacks.Items.AddError(numberCalling.Text + " " + number.Text.Trim() + " 已经存在在系统中, 请勿重复。");
            //    return;
            //}

            if (_userType != TagUserType.Position && HostTag.All.Any(x => string.Compare(x.HostExternalId, number.Text.Trim(), true) == 0))
            {
                feedbacks.Items.AddError(numberCalling.Text + " " + number.Text.Trim() + " 已经存在在系统中, 请勿重复。");
                return;
            }
            if (BusSystemConfig.IsAutoSelectStrongestRssiTag() == false)
            {
                if (tagSelector.SelectedTagIdArray.Length > 0)
                {
                    if (HostTag.All.Any(x => x.TagId == tagSelector.SelectedTagIdArray[0]))
                    {
                        feedbacks.Items.AddError("您选择的标签已经被别人领用，请核实。");
                        return;
                    }
                }
            }

            string photoPath = "";
            if (uploadPhoto.HasFile)
            {
                photoPath = CreatePhotoUploadPath() + "/" + Misc.CreateUniqueFileName() + Path.GetExtension(uploadPhoto.FileName);

                try
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(uploadPhoto.FileContent))
                    {
                        if (image != null)
                        {
                            using (Bitmap bitmap = new Bitmap(image, 100, 120))
                            {
                                bitmap.Save(Fetch.MapPath(PathUtil.ResolveUrl(photoPath)), ImageFormat.Jpeg);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    feedbacks.Items.AddError("上传照片出错，可能是图片体积过大，或者不是图片格式文件。");
                    return;
                }
            }

            HostTag hostTag = new HostTag();
            //TagUser user = new TagUser();
            hostTag.HostName = name.Text.Trim();
            hostTag.HostExternalId = number.Text.Trim();
            hostTag.ImagePath = photoPath;
            hostTag.HostType = 1;
            hostTag.Description = Strings.Left(memo.Text, 500);

            if (BusSystemConfig.IsAutoSelectStrongestRssiTag())
            {
                hostTag.TagId = tagSelectorAuto.GetStrongestRssiTagID();
            }
            else
            {
                if (tagSelector.SelectedTagIdArray.Length > 0)
                {
                    hostTag.TagId = tagSelector.SelectedTagIdArray[0];
                }
            }

            //TagUser.Insert(user);
            //yzhu 090913
            HostTagView oHostTag = new HostTagView();
            try
            {
                int hostId = HostTag.AddOrUpdateHostTag(0, hostTag.TagId, hostTag.HostExternalId, hostTag.HostName, (int)HostTypeType.Unknown, hostTag.Description, hostTag.ImagePath);
                hostTag.HostId = hostId;
                //lyz 设置用户绑定的标签的名称
                device_Tag tag = new device_Tag();
                tag.Id = hostTag.TagId;
                if (tag.Select())
                {
                    tag.TagName = hostTag.HostName;
                    tag.Save();
                }

                HostTag.SetHostGroup(hostId, (int)_userType);
                if (grouplist.SelectedIndex > 0)
                {
                    HostTag.SetHostGroup(hostId, int.Parse(grouplist.SelectedItem.Value));
                }

                HostTag.SetHostStatus(hostId, (int)HostTagStatusType.Normal);

                Caching.Remove(AppKeys.Cache_TagStatusDictionary);

            }
            catch { }


            //记录日志
            Diary.Insert(ContextUser.Current.Id, hostTag.TagId, oHostTag.HostId, "新增标签使用者, " + nameCalling.Text + ": " + hostTag.HostName + (hostTag.TagId == 0 ? "。" : "，并已为其绑定标签。"));

            //清除缓存
            //TagUser.ClearCache();


            bool isServiceAvailable = LocatingServiceUtil.IsAvailable();
            //更新标签信息
            if (hostTag.TagId != 0)
            {
                //Tag.UpdateTagNameAndSerialNo(hostTag.TagId, hostTag.HostName, hostTag.HostExternalId);
                //LocatingServiceUtil.Instance<IServiceApi>().UpdateTagNameAndSerialNo(hostTag.TagId, hostTag.HostName, hostTag.HostExternalId);
                if (Config.Settings.ProjectType != ProjectTypeEnum.WXFactory)//无锡项目需要给标签设定参数，所以需要手动启动定位
                {
                    //自定将标签启动定位
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
                    if (Config.Settings.ProjectType == ProjectTypeEnum.WXFactory)
                    {
                        useSettingModel.ScanMode = 1;
                    }
                    TagLocateSetting.StartLocating(new int[] { hostTag.TagId }, useSettingModel);


                    if (isServiceAvailable)
                    {
                        // Send a command to LocatingService.
                        LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();
                        LocatingServiceUtil.Instance<IServiceApi>().ReloadTagHost(hostTag.TagId);
                    }
                    TagStatusView tagView = TagStatusView.SelectTagStatus(hostTag.TagId);
                    tagView.HostTag = HostTagView.GetHostView(hostTag.TagId);
                }

            }

            int groupId = 1;
            //设置犯人所在监舍
            if (_userType == TagUserType.Culprit)
            {
                groupId = 2;
                int areaId = int.Parse(culpritRoom.SelectedValue);

                CulpritRoomReference.ArrangeNewReference(hostTag.HostId, areaId);

                //查询警告条件
                int ruleId = AreaWarningRule.SelectRuleByAreaId(areaId).Select(x => x.Id).FirstOrDefault();

                //如果警告条件不存在，则自动建立一条
                if (ruleId == 0)
                {
                    var rule = new AreaWarningRule
                    {
                        AreaEventType = (byte)AreaEventType.StayOutside,
                        AreaId = areaId,
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
                var culpritIdArray = CulpritRoomReference.All.Where(x => x.JailRoomId == areaId).Select(x => x.CulpritId).ToArray();
                var tagIdArray = HostTag.All.Where(x => culpritIdArray.Contains(x.HostId) && x.TagId > 0).Select(x => x.TagId).ToArray();
                if (isServiceAvailable)
                {
                    LocatingServiceUtil.Instance<IServiceApi>().SetWarningRuleCoverage(ruleId, tagIdArray);
                }
                else
                {
                    AreaWarningRuleCoverage.SetCoverage(ruleId, tagIdArray);
                }
            }
            else if (_userType == TagUserType.Cop)
            {
                groupId = 1;
            }

            /*if (hostTag.TagId != 0)
            {
                TagGroup oGroup = TagGroup.Select(groupId);
                if (oGroup == null)
                {

                }
                else
                {
                    int[] tagIdArray = new int[1];
                    int[] selectedTagIdArray = TagGroupCoverage.GetCoveredTagIdArray(groupId);
                    bool bIncluded = false;
                    if (selectedTagIdArray != null && selectedTagIdArray.Length > 0)
                    {
                        for (int i = 0; i < selectedTagIdArray.Length; i++)
                        {
                            if (selectedTagIdArray[i] == hostTag.TagId)
                            {
                                bIncluded = true;
                                break;
                            }
                        }
                        if (!bIncluded)
                        {
                            tagIdArray = new int[selectedTagIdArray.Length + 1];
                            tagIdArray[selectedTagIdArray.Length] = hostTag.TagId;
                        }
                    }
                    else
                    {

                        tagIdArray[0] = hostTag.TagId;
                    }
                    if (!bIncluded)
                        TagGroup.UpdateById(groupId, oGroup.GroupName, oGroup.GroupDescription, tagIdArray);
                }
            }*/

            //结束
            ShowMessagePage(string.Format(
                    "{0}: <span class='bold'>{1}</span>, {2}: <span class='bold'>{3}</span> 的信息已成功添加到系统中。",
                    nameCalling.Text, name.Text.Trim(), numberCalling.Text, number.Text.Trim()
                ),
                new Link("继续"// + Wrap.Title
                    , Fetch.CurrentUrl),
                    Config.Settings.ProjectType==ProjectTypeEnum.NMPrison?new Link():new Link("返回信息列表", WebPath.GetFullPath("TagUsers/TagUserList.aspx?type=" + (byte)_userType))
                );

        }




        #region Create Folder


        string CreatePhotoUploadPath()
        {
            string str = null;

            switch (_userType)
            {
                default:
                    break;

                case TagUserType.Cop:
                    str = "/police";
                    break;

                case TagUserType.Culprit:
                    str = "/culprit";
                    break;
            }
            string path = "app_data/photo" + str;
            EnsureFolder(path);

            return path;
        }

        void EnsureFolder(string relativePath)
        {
            string folder = Fetch.MapPath(PathUtil.ResolveUrl(relativePath));

            if (!Directory.Exists(folder))
            {
                try
                {
                    Directory.CreateDirectory(folder);
                }
                catch
                {
                    //try {
                    //        Scripting.FileSystemObject fso = new Scripting.FileSystemObject();
                    //        fso.CreateFolder(folder);
                    //        fso = null;
                    //} catch {
                    throw new IOException("Failed on creating folder '" + folder + "'");
                    //}
                }
            }
        }

        #endregion
    }
}
