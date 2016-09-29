using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NetRadio.Web;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.TagUsers
{
    public partial class __Police : BasePage
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
            scriptFiles.Add("6", "App_Script/UI/SelectTag.ascx.js");
            scriptFiles.Add("7", "App_Script/Control.js");
            scriptFiles.Add("8", "App_Script/UI/SelectStrongestRssiTag.ascx.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        public __Police()
        {
            _id = Fetch.QueryUrlAsInteger("id");
        }

        int _id;
        bool isAdmin = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            NetRadio.Data.User oUser = NetRadio.Data.User.SelectByUserName(me.Name);
            if (oUser.Role >= (int)UserRole.Admin)
                isAdmin = true;



            if (BusSystemConfig.IsAutoSelectStrongestRssiTag() == false)
            {
                tagSelector.Visible = true;
                selectStrongestRssiTag.Visible = false;
                changeTag.Href = "javascript:tt();";
            }
            else
            {
                tagSelector.Visible = false;
                selectStrongestRssiTag.Visible = true;
                changeTag.Href = "javascript:tt2();";
            }

            if (!Page.IsPostBack)
            {
                Ajax.AjaxManager.RegisterClass(typeof(__TagUser));
                AjaxUtil.RegisterClientScript(typeof(__TagUser), this.Page);

                int _tagId;
                string _tagMac = null;

                using (AppDataContext db = new AppDataContext())
                {
                    var cop = db.HostTagGroupStatus.SingleOrDefault(h => h.HostId == _id && h.HostGroupId == (int)TagUserType.Cop);
                    if (cop == null)
                    {
                        ShowMessagePage("记录不存在。");
                    }

                    _tagId = cop.TagId;
                    var tag = Tag.Select(_tagId);
                    if (tag != null)
                    {
                        _tagMac = tag.TagMac;
                    }

                    currentTagId.Value = _tagId.ToString();
                    tagMac.Text = _tagMac;
                    userId.Value = cop.HostId.ToString();

                    name.Text = newName.Text = cop.HostName;
                    number.Text = newNumber.Text = cop.HostExternalId;
                    string[] aGroupIds = HostTagGroupStatus.GetBelongsGroupNameArrayByHostId(_id);
                    if (aGroupIds != null && aGroupIds.Length > 0)
                    {
                        LabelGroup.Text = String.Join(", ", aGroupIds);

                    }
                    memo.Text = newMemo.Text = Strings.TextEncode(cop.Description);

                    groups.Text = string.Join(",", HostTagGroupStatus.GetBelongsGroupNameArrayByHostId(_id));

                    tagBound.Text = cop.TagId == 0 ? "未携带标签" : "已领用标签";
                    photo.Src = "UserPhoto.ashx?id=" + _id;

                    if (!isAdmin)
                    {
                        changeName.Visible = false;
                        changeNumber.Visible = false;
                        changeMemo.Visible = false;
                        deleteButton.Visible = false;
                        uploadButton.Visible = false;
                    }
                }

                if (Config.Settings.ProjectType == ProjectTypeEnum.YZPrison)
                {
                    absencediv.Visible = false;
                }
                else
                {
                    absencediv.Visible = true;
                }

                locatingServiceDownMarker.Visible = !LocatingServiceUtil.IsAvailable();

                if (!LocatingServiceUtil.IsAvailable() || !isAdmin)
                {
                    changeTag.Visible = false;
                    clearAllEvents.Visible = false;
                    //locatingServiceDownMarker.Visible = true;
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(_tagMac))
                        {
                            TagStatusView tagStatus = LocatingServiceUtil.Instance<IServiceApi>().SelectTagStatus(_tagMac);
                            if (tagStatus != null)
                            {
                                coordinatesName.Text = Coordinates.GetName(tagStatus.CoordinatesId);
                                positionUpdateTime.Value = tagStatus.PositionUpdateTime;

                                absence.Text = Misc.GetEventStatusDescription(tagStatus.AbsenceStatus);
                                areaEvent.Text = Misc.GetEventStatusDescription(tagStatus.AreaEventStatus);
                                batteryInsufficient.Text = Misc.GetEventStatusDescription(tagStatus.BatteryInsufficientStatus);
                                batteryReset.Text = Misc.GetEventStatusDescription(tagStatus.BatteryResetStatus);
                                buttonPressed.Text = Misc.GetEventStatusDescription(tagStatus.ButtonPressedStatus);
                                //wristletBroken.Text = Misc.GetEventStatusDescription(tagStatus.WristletBrokenStatus);
                                eventUpdateTime.Value = tagStatus.EventUpdateTime;

                                clearAbsence.Visible = tagStatus.AbsenceStatus != (byte)EventStatus.Cleared;
                                clearAreaEvent.Visible = tagStatus.AreaEventStatus != (byte)EventStatus.Cleared;
                                clearBatteryInsufficient.Visible = tagStatus.BatteryInsufficientStatus != (byte)EventStatus.Cleared;
                                clearBatteryReset.Visible = tagStatus.BatteryResetStatus != (byte)EventStatus.Cleared;
                                clearButtonPressed.Visible = tagStatus.ButtonPressedStatus != (byte)EventStatus.Cleared;
                                //clearWristletBroken.Visible = tagStatus.WristletBrokenStatus != (byte)EventStatus.Cleared;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        protected void uploadButton_Click(object sender, EventArgs e)
        {
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
                    Terminator.End("上传照片出错，可能是图片体积过大，或者不是图片格式文件。");
                    return;
                }
            }

            HostTag hostTag = HostTag.GetById(_id);
            //删除旧文件
            try
            {
                string oldPath = hostTag.ImagePath;
                if (oldPath != null)
                {
                    File.Delete(Fetch.MapPath(PathUtil.ResolveUrl(hostTag.ImagePath)));
                }
            }
            catch (Exception)
            {
            }
            hostTag.ImagePath = photoPath;
            //更新数据库
            HostTag.UpdateHostTag(hostTag);
            //TagUser.UpdatePhotoUrl(_id, photoPath);

            Terminator.Redirect(Fetch.CurrentUrl);
        }

        protected void deleteButton_Click(object sender, EventArgs e)
        {
            //TagUser user = TagUser.SelectById(_id);

            HostTag host = HostTag.GetById(_id);
            if (host != null)
            {
                bool serviceAvailable = LocatingServiceUtil.IsAvailable();
                var tagId = host.TagId;
                if (tagId > 0)
                {
                    using (AppDataContext db = new AppDataContext())
                    {

                        //更新标签名称
                        Tag tag = Tag.Select(tagId);
                        if (tag != null)
                        {
                            string mac = tag.TagMac;
                            if (serviceAvailable)
                            {
                                LocatingServiceUtil.Instance<IServiceApi>().UpdateTagNameAndSerialNo(tagId, "NewTag_" + mac.Substring(12), "");
                            }
                            else
                            {
                                Tag.UpdateTagNameAndSerialNo(tagId, "NewTag_" + mac.Substring(12), "");
                            }
                        }

                        //删除标签历史记录和相关信息
                        AreaEventLog.DeleteByTagId(tagId);
                        GenericEventLog.DeleteByTagId(tagId);
                        TagPositionLog.DeleteByTagId(tagId);
                        //LocationChangeLog.DeleteMany(tagId);
                        //LatestEvent.DeleteByTagId(tagId);
                        TagEventStatus.DeleteMany(tagId);
                        TagPositionStatus.DeleteMany(tagId);
                        AreaWarningRuleCoverage.DeleteMany(tagId);
                        TagAlert.DeleteTagAlerts(tagId);


                        //保留低电记录 
                        //db.ExecuteCommand("delete from history_BatteryResetLog where TagId=", tagId);
                    }
                    TagLocateSetting.StopLocating(tagId);
                    if (serviceAvailable)
                    {
                        LocatingServiceUtil.Instance<IServiceApi>().StartStopLocating();
                    }

                    //TagUser.DeleteById(_id);

                    TagStatusView.SelectTagStatus(tagId).HostTag = null;
                }

                TagAlert.DeleteTagAlerts(_id);
                //删除人员信息
                HostTag.DeleteHostTag(_id);
                //记录日志

                if (tagId > 0 && serviceAvailable)
                {
                    LocatingServiceUtil.Instance<IServiceApi>().ReloadTagHost(tagId);
                }
                Diary.Insert(ContextUser.Current.Id, tagId, _id, "删除使用者" + host.HostName + "的信息" + (host.TagId == 0 ? "" : "并解除标签绑定。") + "。");
            }
            ShowMessagePage("删除成功。");
        }



        #region Create Folder


        string CreatePhotoUploadPath()
        {
            string path = "app_data/photo" + "/police";
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
