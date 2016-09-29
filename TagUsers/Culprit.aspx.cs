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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using NetRadio.Web;
using NetRadio.Business;
using System.Data.SqlClient;
using System.Data;
namespace NetRadio.LocatingMonitor.TagUsers
{
    [AjaxRegister]
    public partial class __Culprit : BasePage
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
        public __Culprit()
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
            Ajax.AjaxManager.RegisterClass(typeof(__TagUser));
            AjaxUtil.RegisterClientScript(typeof(__TagUser), this.Page);

            if (!Page.IsPostBack)
            {
                LoadJailRoomList();
            }

            int _tagId;
            string _tagMac = null;
            userId.Value = _id.ToString();
            using (AppDataContext db = new AppDataContext())
            {
                var culprit = db.HostTagGroupStatus.SingleOrDefault(u => u.HostGroupId == (int)TagUserType.Culprit && u.HostId == _id);
                if (culprit == null)
                {
                    ShowMessagePage("记录不存在。");
                }

                _tagId = culprit.TagId;
                var tag = Tag.Select(_tagId);
                if (tag != null)
                {
                    _tagMac = tag.TagMac;
                }

                currentTagId.Value = _tagId.ToString();
                tagMac.Text = _tagMac;
                userId.Value = culprit.HostId.ToString();

                name.Text = newName.Text = culprit.HostName;
                number.Text = newNumber.Text = culprit.HostExternalId;
                memo.Text = newMemo.Text = Strings.TextEncode(culprit.Description);

                if (Config.Settings.IsLoadHostInfo)
                {
                    CulMoreInfo.Visible = true;
                    string strConnect = System.Configuration.ConfigurationSettings.AppSettings["FXConnectionString"].ToString();
                    SqlConnection conn = new SqlConnection(strConnect);

                    string strSQL = "SELECT top 1 RYBH, ZJHM, JYAQ,V_SSJD.HZ as SSJDHZ, V_AJLB.HZ as AJLBHZ from V_RYXXALL join V_AJLB on V_RYXXALL.AJLB=V_AJLB.DM join V_SSJD on V_SSJD.DM=V_RYXXALL.SSJD where RYBH='" + culprit.HostExternalId + "'";

                    DataSet ds = new DataSet();//   创建一个   DataSet
                    conn.Open();
                    SqlDataAdapter command = new SqlDataAdapter(strSQL, conn);//   用   SqlDataAdapter   得到一个数据集   
                    command.Fill(ds, "CulInfo");//把Dataset绑定到数据表   
                    DataTable dt = ds.Tables["CulInfo"];

                    if (dt.Rows.Count > 0)
                    {
                        IDNO.Text = dt.Rows[0]["ZJHM"].ToString();
                        CulKind.Text = dt.Rows[0]["AJLBHZ"].ToString();
                        CulState.Text = dt.Rows[0]["SSJDHZ"].ToString();
                        CulDes.Text = dt.Rows[0]["JYAQ"].ToString();
                    }
                }
                else
                {
                    CulMoreInfo.Visible = false;
                }

                tagBound.Text = culprit.TagId == 0 ? "未携带标签" : "已领用标签";
                photo.Src = "UserPhoto.ashx?id=" + _id;
                jailRoom.Text = MapArea.All.Where(a => a.Id == CulpritRoomReference.All.Where(r => r.CulpritId == _id).Select(r => r.JailRoomId).SingleOrDefault()).Select(a => a.AreaName).SingleOrDefault();

                if (!isAdmin)
                {
                    changeName.Visible = false;
                    changeNumber.Visible = false;
                    changeMemo.Visible = false;
                    deleteButton.Visible = false;
                    uploadButton.Visible = false;
                }
            }


            if (!LocatingServiceUtil.IsAvailable() || !isAdmin)
            {
                changeTag.Visible = false;
                changeJailRoom.Visible = false;
                clearAllEvents.Visible = false;
                locatingServiceDownMarker.Visible = true;
                setStatus.Visible = false;
                locatingServiceDownMarker1.Visible = true;
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

                            if (tagStatus.HostTag.HostGroupId.Contains(4))
                                illness.Checked = true;

                            if (tagStatus.HostTag.HostGroupId.Contains(3))
                                serious.Checked = true;

                            if (tagStatus.HostTag.HostStatusId == (int)HostTagStatusType.Interrogation)
                            {
                                arraignment.Checked = true;

                                using (AppExtensionDataContext dbExtension = new AppExtensionDataContext())
                                {
                                    InterrogationLog interrogationLog = dbExtension.InterrogationLogs.SingleOrDefault(t => t.CulpritId == tagStatus.HostTag.HostId && t.StartTime == null);

                                    if (interrogationLog != null)
                                    {
                                        if (interrogationLog.PoliceId == me.Id)
                                        {
                                            arraignment.Enabled = true;
                                        }
                                        else
                                        {
                                            arraignment.Enabled = false;
                                        }
                                    }
                                    else
                                    {
                                        //状态与数据库不一致，需要同步
                                    }
                                }
                            }


                            absence.Text = Misc.GetEventStatusDescription(tagStatus.AbsenceStatus);
                            areaEvent.Text = Misc.GetEventStatusDescription(tagStatus.AreaEventStatus);
                            batteryInsufficient.Text = Misc.GetEventStatusDescription(tagStatus.BatteryInsufficientStatus);
                            batteryReset.Text = Misc.GetEventStatusDescription(tagStatus.BatteryResetStatus);
                            buttonPressed.Text = Misc.GetEventStatusDescription(tagStatus.ButtonPressedStatus);
                            wristletBroken.Text = Misc.GetEventStatusDescription(tagStatus.WristletBrokenStatus);
                            eventUpdateTime.Value = tagStatus.EventUpdateTime;

                            clearAbsence.Visible = tagStatus.AbsenceStatus != (byte)EventStatus.Cleared;
                            clearAreaEvent.Visible = tagStatus.AreaEventStatus != (byte)EventStatus.Cleared;
                            clearBatteryInsufficient.Visible = tagStatus.BatteryInsufficientStatus != (byte)EventStatus.Cleared;
                            clearBatteryReset.Visible = tagStatus.BatteryResetStatus != (byte)EventStatus.Cleared;
                            clearButtonPressed.Visible = tagStatus.ButtonPressedStatus != (byte)EventStatus.Cleared;
                            clearWristletBroken.Visible = tagStatus.WristletBrokenStatus != (byte)EventStatus.Cleared;
                        }

                    }
                }
                catch
                {
                }
            }

            using (AppDataContext db = new AppDataContext())
            {
                list.DataSource = db.DBViewTagAlerts.Where(x => x.TagId == _tagId).OrderByDescending(x => x.WriteTime).Take(10).ToList();
                list.ItemCreated += new RepeaterItemEventHandler(list_ItemCreated);
                list.DataBind();

                if (latestWarnings.Visible = list.Items.Count > 0)
                {
                    moreWarninngs.Href = "../History/MarshalEventLog.aspx?tagNameKeyword=" + Server.UrlEncode(name.Text);
                }
            }
        }

        private void LoadJailRoomList()
        {
            newJailRoom.Items.Clear();

            newJailRoom.DataSource = MapArea.All;
            newJailRoom.DataTextField = "AreaName";
            newJailRoom.DataValueField = "Id";
            newJailRoom.DataBind();

            int roomId = CulpritRoomReference.GetRoomIdByCulpritId(_id);
            ListItem listItem = newJailRoom.Items.FindByValue(roomId.ToString());
            if (listItem == null)
            {
                newJailRoom.Items.Insert(0, new ListItem("请选择 ..", "-1"));
            }
            else
            {
                newJailRoom.SelectedIndex = -1;
                listItem.Selected = true;
            }
        }

        void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            DBViewTagAlert log = e.Item.DataItem as DBViewTagAlert;
            if (log != null)
            {
                SmartLabel description = e.Item.FindControl("description") as SmartLabel;
                description.Text = CommonExtension.GetEventDescription((SupportEvent)log.AlertType, log.HostId.Value);

                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                facilityName.Text = log.FacilityName;

                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
                writeTime.DisplayValue = log.WriteTime;

                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                coordinatesName.Text = log.CoordinatesName;
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
                    ShowMessagePage("上传照片出错，可能是图片体积过大，或者不是图片格式文件。");
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
                    File.Delete(Fetch.MapPath(PathUtil.ResolveUrl(oldPath)));
                }
            }
            catch (Exception)
            {
            }

            //更新数据库
            //TagUser.UpdatePhotoUrl(_id, photoPath);
            hostTag.ImagePath = photoPath;
            HostTag.UpdateHostTag(hostTag);
            Terminator.Redirect(Fetch.CurrentUrl);
        }

        protected void deleteButton_Click(object sender, EventArgs e)
        {
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
                        InterrogationLog.DeleteMany(host.HostId);
                        TagAlert.DeleteTagAlerts(tagId);
                        //删除低电记录: 不执行删除
                        //db.ExecuteCommand("delete from history_BatteryResetLog where TagId=", tagId);
                    }
                    TagLocateSetting.StopLocating(tagId);

                    if (serviceAvailable)
                    {
                        var s = LocatingServiceUtil.Instance<IServiceApi>();
                        s.StartStopLocating();
                    }

                    int areaId = CulpritRoomReference.GetRoomIdByCulpritId(host.HostId);
                    if (areaId != 0)
                    {
                        //查询警告条件
                        var ruleId = AreaWarningRule.SelectRuleByAreaId(areaId).Select(x => x.Id).FirstOrDefault();
                        //如果警告条件不存在，则自动建立一条
                        if (ruleId != 0)
                        {
                            //为警告条件设置关联标签
                            var culpritIdArray = CulpritRoomReference.All.Where(x => x.JailRoomId == areaId).Select(x => x.CulpritId).ToArray();
                            var tagIdArray = HostTag.All.Where(x => culpritIdArray.Contains(x.HostId) && x.TagId > 0 && x.TagId != tagId).Select(x => x.TagId).ToArray();
                            if (serviceAvailable)
                            {
                                LocatingServiceUtil.Instance<IServiceApi>().SetWarningRuleCoverage(ruleId, tagIdArray);
                            }
                            else
                            {
                                AreaWarningRuleCoverage.SetCoverage(ruleId, tagIdArray);
                            }
                        }
                    }
                }

                //删除使用者信息
                CulpritRoomReference.DeleteByCulpritId(_id);
                //TagUser.DeleteById(_id);
                HostTag.DeleteHostTag(_id);
                TagStatusView.SelectTagStatus(tagId).HostTag = null;

                if (tagId > 0 && serviceAvailable)
                {
                    LocatingServiceUtil.Instance<IServiceApi>().ReloadTagHost(tagId);
                }

                //记录日志
                Diary.Insert(ContextUser.Current.Id, tagId, _id, "删除犯人" + host.HostName + "的信息" + (host.TagId == 0 ? "" : "并解除标签绑定。") + "。");

            }
            ShowMessagePage("删除成功。");
        }


        #region Create Folder


        string CreatePhotoUploadPath()
        {
            string path = "app_data/photo" + "/culprit";
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
