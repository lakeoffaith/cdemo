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
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __TagAlertProcess0 : BasePage
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
        public __TagAlertProcess0()
        {
            _id = Fetch.QueryUrlAsInteger("id");
        }

        int _id;
        TagAlert _tagAlert;

        protected void Page_Load(object sender, EventArgs e)
        {
           // this.Title = this.WebSiteLabel = "警告或事件处理";
            LoadTagAlert();

            if (!IsPostBack)
                LoadCopList();
        }

        protected void LoadCopList()
        {
            using (AppDataContext db = new AppDataContext())
            {
                Dictionary<int, string> dicUsers = db.Users.ToDictionary(t => t.Id, t => t.UserName);

                foreach (var item in dicUsers)
                {
                    ListItem listItem = new ListItem(item.Value, item.Key.ToString());
                    copDropDownList.Items.Add(listItem);
                }
            }
        }

        protected void LoadTagAlert()
        {
            using (AppDataContext db = new AppDataContext())
            {
                _tagAlert = db.TagAlerts.SingleOrDefault(t => t.AlertId == _id);

                if (_tagAlert == null)
                {
                    ShowMessagePage("报警事件不存在。");
                }
                else
                {
                                    HostTag thisHostTag = HostTag.GetById(_tagAlert.HostId);
                                    Tag thisTag = Tag.Select(thisHostTag.TagId);
                                    if (thisTag != null)
                                    {
                                        tagName.Text = thisHostTag.HostName;
                                        int coorid = _tagAlert.CoordinatesId;
                                        if (CommonExtension.IsIlltreatTag(_tagAlert.HostId) && Config.Settings.ProjectType!=ProjectTypeEnum.NMPrison)
                                        {
                                            coorid = CommonExtension.GetCoordinatesId(thisHostTag.Description.Substring(0, thisHostTag.Description.Length - 2));
                                        }

                                        if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                                        {
                                            coordinatesName.Text = thisHostTag.HostName;
                                        }
                                        else
                                        {
                                            coordinatesName.Text = Coordinates.GetName(coorid);
                                        }

                                        if (LocatingServiceUtil.IsAvailable())
                                        {
                                            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                                            bool boolean = serviceApi.ClearTagStatus(thisTag.TagMac, (SupportEvent)_tagAlert.AlertType);
                                        }

                                        description.Text = CommonExtension.GetEventDescription((SupportEvent)_tagAlert.AlertType, _tagAlert.HostId);
                                        time.DisplayValue = _tagAlert.WriteTime;
                                        if (_tagAlert.MasterUserId > 0)
                                            alertMaster.Text = Data.User.Select(_tagAlert.MasterUserId) == null ? "未知" : Data.User.Select(_tagAlert.MasterUserId).UserName;

                                        alertStatus.Text = NetRadio.Common.LocatingMonitor.Misc.GetAlertStatus((AlertStatusType)_tagAlert.AlertStatus);

                                        list.DataSource = db.AlertProcessLogs.Where(t => t.AlertId == _id).OrderBy(t => t.UpdateTime).ToList();
                                        list.ItemCreated += new RepeaterItemEventHandler(list_ItemCreated);
                                        list.DataBind();

                                        if (!IsPostBack)
                                        {
                                            Dictionary<string, int> alertResults = new Dictionary<string, int>();
                                            alertResults.Add(CommonExtension.GetEventDescription((SupportEvent)_tagAlert.AlertType, _tagAlert.HostId), 1);
                                            alertResults.Add("误报", 2);
                                            alertResults.Add("其它", 3);

                                            foreach (var item in alertResults)
                                            {
                                                ListItem listitem = new ListItem(item.Key, item.Value.ToString());

                                                if (item.Value == 1)
                                                    listitem.Selected = true;

                                                alertResultList.Items.Add(listitem);
                                            }
                                        }

                                        if (_tagAlert.AlertStatus == (byte)AlertStatusType.New || _tagAlert.AlertStatus == (byte)AlertStatusType.Processing)
                                        {
                                            alertResultList.Visible = true;
                                            alertResult.Visible = false;
                                            handover.Visible = true;
                                            alertProcess.Visible = true;
                                            otherReason.Visible = true;
                                        }
                                        else
                                        {
                                            alertResultList.Visible = false;
                                            alertResult.Visible = true;
                                            handover.Visible = false;
                                            alertProcess.Visible = false;
                                            otherReason.Visible = false;

                                            AlertProcessLog alertProcessLog = db.AlertProcessLogs.SingleOrDefault(t => t.AlertId == _id && t.AlertStatus == (byte)AlertStatusType.Resolved);

                                            if (alertProcessLog != null)
                                            {
                                                alertResult.Text = alertProcessLog.ChangeReason;
                                            }
                                        }
                                    }
                }
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            AlertProcessLog ev = e.Item.DataItem as AlertProcessLog;

            if (ev != null)
            {
                SmartLabel processPerson = e.Item.FindControl("processPerson") as SmartLabel;
                if (ev.UserId > 0)
                    processPerson.Text = Data.User.Select(ev.UserId) == null ? "未知" : Data.User.Select(ev.UserId).UserName;
                else
                    processPerson.Text = "未知";

                SmartLabel preocessStatus = e.Item.FindControl("preocessStatus") as SmartLabel;
                preocessStatus.Text = Misc.GetAlertStatus((AlertStatusType)ev.AlertStatus);

                SmartLabel changeDes = e.Item.FindControl("changeDes") as SmartLabel;
                changeDes.Text = ev.ChangeReason;

                DateTimeLabel changetime = e.Item.FindControl("changetime") as DateTimeLabel;
                changetime.DisplayValue = ev.UpdateTime;

            }
        }

        protected void alertProcess_Click(object sender, EventArgs e)
        {
            if (alertResultList.SelectedItem.Text == "其它" && otherReason.Text.Trim().Length == 0)
            {
                feedbacks.Items.AddError("报警原因选择其它时，需要输入具体信息。");
                return;
            }

            TagAlert.UpdateStatusByAlertId(_id, AlertStatusType.Resolved);

            string reason = me.Name + "将报警原因设置为：" + alertResultList.SelectedItem.Text;

            if (otherReason.Text != null && otherReason.Text.Trim().Length > 0)
            {
                reason += "," + otherReason.Text.Trim();
            }
            AlertProcessLog.Insert(_id, me.Id, AlertStatusType.Resolved, reason);
            LoadTagAlert();
        }

        protected void handover_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(copDropDownList.SelectedItem.Value) == me.Id)
            {
                ShowMessagePage("不能将事件处理移交给自己。");
                return;
            }

            TagAlert.UpdateMasterByAlertId(_id, Convert.ToInt32(copDropDownList.SelectedItem.Value));
            AlertProcessLog.Insert(_id, me.Id, AlertStatusType.Processing, "将事件处理移交给：" + copDropDownList.SelectedItem.Text);
            LoadTagAlert();
        }
    }
}
