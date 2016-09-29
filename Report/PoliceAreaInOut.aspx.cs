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
namespace NetRadio.LocatingMonitor.Report
{
    #region mark
    //public class AreaInOutTime
    //{
    //    public int hostId;
    //    public string hostName;
    //    public DateTime inTime;
    //    public DateTime outTime;
    //    public TimeSpan stayTime;

    //    public AreaInOutTime()
    //    {
    //        hostId = 0;
    //        hostName = "";
    //        inTime = DateTime.MinValue;
    //        outTime = DateTime.MinValue;
    //    }
    //}
    //public partial class __PoliceAreaInOut : BasePage
    //{
    //    protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
    //    {
    //        scriptFiles.Clear();
    //        scriptFiles.Add("0", "App_Script/Global.js");
    //        scriptFiles.Add("1", "App_Script/func.js");
    //        scriptFiles.Add("2", "App_Script/master.js");
    //        scriptFiles.Add("3", "App_Script/Common.js");
    //        scriptFiles.Add("4", "App_Script/Project.js");
    //        scriptFiles.Add("5", "App_Script/UI/ReportIndex.aspx.js");

    //        //base.RegisterScriptFileInHead(scriptFiles);
    //    }
    //    private TagUserType _userType;
    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        //this.Title = this.WebSiteLabel = "报表统计";
    //        _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 1);
    //        //this.Wrap.Title = CommonExtension.GetUserTypeDescription(_userType) + this.Wrap.Title;

    //        if (!Page.IsPostBack)
    //        {
    //            this.LoadDefaultView();
    //        }
    //    }

    //    private void LoadDefaultView()
    //    {
    //        fromDate.Text = DateTime.Today.ToString("yyyy-M-d");
    //        fromHour.SelectedValue = "0";
    //        fromMinute.Text = "00";
    //        toDate.Text = DateTime.Today.ToString("yyyy-M-d");
    //        toHour.SelectedValue = "23";
    //        toMinute.Text = "59";

    //        tagUserSelector.SelectedGroupId = (int)_userType;
    //    }


    //    protected void BtnStat_Click(Object sender, CommandEventArgs e)
    //    {
    //        try
    //        {
    //            lblMessage.Text = "";//GTang20100111警告消息清除
    //            if (tagUserSelector.SelectedTagIdArray == null || tagUserSelector.SelectedTagIdArray.Length == 0)
    //            {
    //                lblMessage.Text = "没有选择标签";
    //                return;
    //            }

    //            if (fromDate.Text == "")
    //            {
    //                lblMessage.Text = "请选择要统计的开始日期。";
    //                return;
    //            }

    //            if (toDate.Text == "")
    //            {
    //                lblMessage.Text = "请选择要统计的结束日期。";
    //                return;
    //            }
    //            //----start- GTang 20100111检查分钟设置
    //            if (fromMinute.Text.Trim() == "")
    //            {
    //                fromMinute.Text = "00";
    //            }
    //            if (toMinute.Text.Trim() == "")
    //            {
    //                toMinute.Text = "59";
    //            }
    //            //----end---

    //            var h1 = Convert.ToInt32(fromHour.SelectedValue);
    //            var h2 = Convert.ToInt32(toHour.SelectedValue);


    //            var m1 = Convert.ToInt32(fromMinute.Text);
    //            if (m1 < 0 || m1 > 59)
    //            {
    //                lblMessage.Text = "开始时间中分钟的数字范围只能在0-59之间。";
    //                return;
    //            }
    //            var m2 = Convert.ToInt32(toMinute.Text);
    //            if (m2 < 0 || m2 > 59)
    //            {
    //                lblMessage.Text = "结束时间中分钟的数字范围只能在0-59之间。";
    //                return;
    //            }

    //            string zFromDate = fromDate.Text + " " + fromHour.SelectedValue + ":" + fromMinute.Text + ":00";
    //            string zToDate = toDate.Text + " " + toHour.SelectedValue + ":" + toMinute.Text + ":00";

    //            DateTime dtFrom = Convert.ToDateTime(zFromDate);
    //            DateTime dtTo = Convert.ToDateTime(zToDate);

    //            if (dtTo < dtFrom)
    //            {
    //                lblMessage.Text = "结束时间不可小于开始时间!";
    //                return;
    //            }

    //            if (DateTime.Now < dtFrom)//GTang200100111
    //            {
    //                lblMessage.Text = "开始时间不能大于当前时间。";
    //                return;
    //            }

    //            IList<AreaInOutTime> lTime = new List<AreaInOutTime>();
    //            int totalCount = 0;
    //            using (AppDataContext db = new AppDataContext())
    //            {

    //                for (int i = 0; i < tagUserSelector.SelectedTagIdArray.Length; i++)
    //                {
    //                    long totalTicks = 0;
    //                    int entryCount = 0;
    //                    DateTime firstInTime = DateTime.MinValue;
    //                    DateTime lastOutTime = DateTime.MinValue;

    //                    DateTime endTime = DateTime.Now;
    //                    if (dtTo < endTime) endTime = dtTo;

    //                    int hostId = tagUserSelector.SelectedTagIdArray[i];
    //                    HostTag oHost = HostTag.GetById(hostId);
    //                    if (oHost == null)
    //                    {
    //                        lblMessage.Text += String.Format("ID={0}不存在", hostId);
    //                        continue;
    //                    }
    //                    int tagId = oHost.TagId;
    //                    string hostName = oHost.HostName;
    //                    IList<TagPositionLog> lLog = db.TagPositionLogs
    //                        .Where(x => x.HostId == hostId && x.WriteTime >= dtFrom && x.WriteTime <= endTime)
    //                        .OrderBy(x => x.WriteTime).ToList();

    //                    var firstQuery = db.TagPositionLogs.Where(x => x.WriteTime < dtFrom && x.HostId == hostId)
    //                        .OrderByDescending(x => x.WriteTime).Take(1);
    //                    TagPositionLog firstLog = null;
    //                    try
    //                    {
    //                        if (firstQuery != null) firstLog = firstQuery.First();
    //                    }
    //                    catch { }

    //                    //TagPositionLog lastLog = null;

    //                    //if (dtTo > DateTime.Now)
    //                    //{

    //                    //}
    //                    //else
    //                    //{
    //                    //    lastLog = db.TagPositionLogs.Where(x => x.WriteTime > dtTo)
    //                    //    .OrderBy(x => x.WriteTime).Take(1);
    //                    //}

    //                    AreaInOutTime areaTime = new AreaInOutTime();
    //                    areaTime.hostName = hostName;
    //                    areaTime.hostId = hostId;

    //                    bool bFirst = true;
    //                    if (lLog != null && lLog.Count > 0)
    //                    {
    //                        foreach (TagPositionLog log in lLog)
    //                        {
    //                            if (bFirst)
    //                            {
    //                                bFirst = false;
    //                                if (firstLog != null)
    //                                {
    //                                    if (firstLog.X > 0)
    //                                    {
    //                                        areaTime.inTime = dtFrom;
    //                                        firstInTime = areaTime.inTime;
    //                                    }
    //                                    else
    //                                    {
    //                                        areaTime.outTime = dtFrom;
    //                                        lastOutTime = areaTime.outTime;
    //                                    }
    //                                }
    //                                //else
    //                                //{
    //                                if (log.X > 0)
    //                                {
    //                                    areaTime.inTime = log.WriteTime;
    //                                    firstInTime = areaTime.inTime;
    //                                }
    //                                else
    //                                {
    //                                    areaTime.outTime = log.WriteTime;
    //                                    lastOutTime = areaTime.outTime;
    //                                }
    //                                //}
    //                            }
    //                            else
    //                            {
    //                                if (log.X > 0)
    //                                {
    //                                    if (areaTime.inTime == DateTime.MinValue && areaTime.outTime == DateTime.MinValue)
    //                                    {
    //                                        areaTime.inTime = log.WriteTime;
    //                                        firstInTime = areaTime.inTime;
    //                                    }
    //                                    else if (areaTime.outTime != DateTime.MinValue)
    //                                    {
    //                                        if (areaTime.inTime != DateTime.MinValue)
    //                                        {
    //                                            double disappearedTime = (log.WriteTime - areaTime.outTime).Duration().TotalSeconds;
    //                                            if (disappearedTime > 60)
    //                                            {
    //                                                areaTime.stayTime = (areaTime.outTime - areaTime.inTime).Duration();
    //                                                totalTicks += areaTime.stayTime.Ticks;
    //                                                if (radioButtonMode.SelectedValue == "2")
    //                                                    lTime.Add(areaTime);
    //                                                entryCount++;
    //                                                areaTime = new AreaInOutTime();
    //                                                areaTime.inTime = log.WriteTime;
    //                                            }
    //                                            else
    //                                                areaTime.outTime = DateTime.MinValue;
    //                                        }
    //                                        else
    //                                        {
    //                                            areaTime.inTime = log.WriteTime;
    //                                            firstInTime = areaTime.inTime;
    //                                        }
    //                                    }
    //                                    else if (areaTime.inTime != DateTime.MinValue && areaTime.outTime == DateTime.MinValue)
    //                                    {

    //                                    }
    //                                }
    //                                else
    //                                {
    //                                    areaTime.outTime = log.WriteTime;
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (firstLog != null)
    //                        {
    //                            if (firstLog.X > 0)
    //                            {
    //                                areaTime.inTime = dtFrom;
    //                                firstInTime = areaTime.inTime;
    //                            }
    //                        }
    //                    }


    //                    if (areaTime.inTime != DateTime.MinValue)
    //                    {
    //                        if (areaTime.outTime != DateTime.MinValue)
    //                        {
    //                            double disappearedTime = (endTime - areaTime.outTime).Duration().TotalSeconds;
    //                            if (disappearedTime > 60)
    //                            {
    //                                areaTime.stayTime = (areaTime.outTime - areaTime.inTime).Duration();
    //                                totalTicks += areaTime.stayTime.Ticks;
    //                                if (radioButtonMode.SelectedValue == "2")
    //                                    lTime.Add(areaTime);
    //                                entryCount++;
    //                            }
    //                            else
    //                            {
    //                                areaTime.stayTime = (endTime - areaTime.inTime).Duration();
    //                                totalTicks += areaTime.stayTime.Ticks;
    //                                if (radioButtonMode.SelectedValue == "2")
    //                                    lTime.Add(areaTime);
    //                                entryCount++;
    //                            }
    //                            lastOutTime = areaTime.outTime;
    //                        }
    //                        else
    //                        {
    //                            areaTime.stayTime = (endTime - areaTime.inTime).Duration();
    //                            totalTicks += areaTime.stayTime.Ticks;
    //                            if (radioButtonMode.SelectedValue == "2")
    //                                lTime.Add(areaTime);
    //                            entryCount++;
    //                        }
    //                    }

    //                    if (totalTicks > 0)
    //                    {
    //                        areaTime = new AreaInOutTime();
    //                        areaTime.hostId = -1;
    //                        if (radioButtonMode.SelectedValue == "1")
    //                        {
    //                            areaTime.hostName = hostName;
    //                            areaTime.inTime = firstInTime;
    //                            areaTime.outTime = lastOutTime;
    //                        }
    //                        else if (radioButtonMode.SelectedValue == "2")
    //                        {
    //                            areaTime.hostName = "--- ---";
    //                            areaTime.inTime = DateTime.MinValue;
    //                            areaTime.outTime = DateTime.MinValue;
    //                        }
    //                        areaTime.stayTime = new TimeSpan(totalTicks);
    //                        lTime.Add(areaTime);
    //                        totalCount++;
    //                    }
    //                    else
    //                    {
    //                        areaTime = new AreaInOutTime();
    //                        areaTime.hostName = hostName;
    //                        areaTime.inTime = DateTime.MinValue;
    //                        areaTime.outTime = DateTime.MinValue;
    //                        areaTime.stayTime = new TimeSpan(0);
    //                        lTime.Add(areaTime);
    //                        totalCount++;
    //                    }

    //                }
    //            }

    //            p.RecordCount = totalCount;
    //            list.DataSource = lTime;
    //            list.DataBind();

    //        }
    //        catch (Exception err)
    //        {
    //            lblMessage.Text += err.ToString();
    //        }
    //    }

    //    protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
    //    {

    //        AreaInOutTime areaTime = (AreaInOutTime)e.Item.DataItem;
    //        if (areaTime != null)
    //        {
    //            Img icon = e.Item.FindControl("icon") as Img;
    //            Anchor tagName = e.Item.FindControl("tagName") as Anchor;

    //            DateTimeLabel inTime = e.Item.FindControl("inTime") as DateTimeLabel;
    //            DateTimeLabel outTime = e.Item.FindControl("outTime") as DateTimeLabel;
    //            SmartLabel duration = e.Item.FindControl("duration") as SmartLabel;

    //            tagName.Text = Convert.ToString(areaTime.hostName);
    //            if (areaTime.hostId > 0)
    //                tagName.Href = PathUtil.ResolveUrl("TagUsers/TagUser.aspx?id=" + areaTime.hostId.ToString() + "&type=" + (int)_userType);

    //            if (areaTime.inTime != DateTime.MinValue)
    //                inTime.DisplayValue = areaTime.inTime;
    //            if (areaTime.outTime != DateTime.MinValue)
    //                outTime.DisplayValue = areaTime.outTime;

    //            duration.Text = "";
    //            if (tagName.Text == "--- ---")
    //            {
    //                duration.Text = "(";
    //            }
    //            if (areaTime.stayTime.Ticks == 0)
    //            {
    //                duration.Text += "0";
    //            }
    //            else
    //            {
    //                if (areaTime.stayTime.Days > 0)
    //                    duration.Text += areaTime.stayTime.Days.ToString() + "天";
    //                if (areaTime.stayTime.Hours > 0)
    //                    duration.Text += areaTime.stayTime.Hours.ToString() + "时";
    //                if (areaTime.stayTime.Minutes > 0)
    //                    duration.Text += areaTime.stayTime.Minutes.ToString() + "分";
    //                if (areaTime.stayTime.Seconds > 0)
    //                    duration.Text += areaTime.stayTime.Seconds.ToString() + "秒";
    //            }
    //            if (tagName.Text == "--- ---")
    //            {
    //                duration.Text += ")";
    //            }

    //        }

    //        //----------------------------------//


    //    }
    //}
    #endregion

    public class AreaInOutTime
    {
        public int hostId;
        public string hostName;
        public DateTime inTime;
        public DateTime outTime;
        public TimeSpan stayTime;

        public AreaInOutTime()
        {
            hostId = 0;
            hostName = "";
            inTime = DateTime.MinValue;
            outTime = DateTime.MinValue;
        }
    }

    public partial class __PoliceAreaInOut : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/ReportIndex.aspx.js");
            scriptFiles.Add("6", "App_Script/UI/SelectTagUser.ascx.js");
        }

        private TagUserType _userType;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sitemap.Text2 = "统计报表";
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 1);
           // this.Wrap.Title = CommonExtension.GetUserTypeDescription(_userType) + this.Wrap.Title;

            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
                tagUserSelector.SetDataSourceLeft(NetRadio.LocatingMonitor.Controls.__SelectTagUser.SelectTagUsers3);
            }
        }

        private void LoadDefaultView()
        {
            fromDate.Text = DateTime.Today.ToString("yyyy-M-d");
            fromHour.SelectedValue = "0";
            fromMinute.Text = "00";
            toDate.Text = DateTime.Today.ToString("yyyy-M-d");
            toHour.SelectedValue = "23";
            toMinute.Text = "59";
            GetChklArea();
            btnCheckState.Text = "全选";
            //////////tagUserSelector.SelectedGroupId = (int)_userType;
        }

        private void GetChklArea() //绑定区域信息 add GTang 2010/01/18
        {
            lblMessage.Text = "";
            var mapAreaInfor = MapArea.All;
            if (mapAreaInfor == null || mapAreaInfor.Count == 0)
            {
                lblMessage.Text = "区域表为空，请联系管理员维护！";
                return;
            }
            else
            {
                foreach (var name in mapAreaInfor)
                {
                    if (name.LinkedMapId.ToString() == "0")
                    {
                        ListItem item = new ListItem(name.AreaName.ToString(), name.Id.ToString());
                        chklArea.Items.Add(item);
                    }
                }
            }
        }

        private List<int> GetAreaCoordinates()  //根据区域坐标点
        {
            List<int> AreaCoordinates = new List<int>();
            List<int> AreaIds = new List<int>();
            foreach (ListItem Itemvalue in chklArea.Items) //获取区域ID
            {
                if (Itemvalue.Selected)
                {
                    AreaIds.Add(int.Parse(Itemvalue.Value));
                }
            }

            //根据区域ID获取坐标点
            foreach (int listvule in AreaIds)
            {
                IList<MapAreaCoverage> tempCoordinates = MapAreaCoverage.SelectByAreaId(listvule);
                foreach (var Coordinates in tempCoordinates)
                {
                    if (!AreaCoordinates.Contains(Coordinates.CoordinatesId))//避免重复
                    {
                        AreaCoordinates.Add(Coordinates.CoordinatesId);
                    }
                }
            }
            return AreaCoordinates;
        }

        protected void btnCheckState_Click(object sender, System.EventArgs e)
        {
            if (btnCheckState.Text == "全选")
            {
                foreach (ListItem Itemvalue in chklArea.Items)
                {
                    Itemvalue.Selected = true;
                }
                btnCheckState.Text = "全不选";
            }
            else
            {
                foreach (ListItem Itemvalue in chklArea.Items)
                {
                    Itemvalue.Selected = false;
                }
                btnCheckState.Text = "全选";
            }
        }


        protected void BtnStat_Click(Object sender, CommandEventArgs e)
        {
            try
            {
                #region Check Input
                int Itemchecked = 0;
                foreach (ListItem chkItem in chklArea.Items)
                {
                    if (chkItem.Selected)
                    {
                        Itemchecked++;
                    }
                }
                if (Itemchecked == 0)
                {
                    lblMessage.Text = "没有选择区域";
                    return;
                }

                lblMessage.Text = "";//GTang20100111警告消息清除
                if (tagUserSelector.SelectedUserIds == null || tagUserSelector.SelectedUserIds.Length == 0)
                {
                    lblMessage.Text = "没有选择标签";
                    return;
                }

                if (fromDate.Text == "")
                {
                    lblMessage.Text = "请选择要统计的开始日期。";
                    return;
                }

                if (toDate.Text == "")
                {
                    lblMessage.Text = "请选择要统计的结束日期。";
                    return;
                }
                //----start- GTang 20100111检查分钟设置
                if (fromMinute.Text.Trim() == "")
                {
                    fromMinute.Text = "00";
                }
                if (toMinute.Text.Trim() == "")
                {
                    toMinute.Text = "59";
                }
                //----end---

                var h1 = Convert.ToInt32(fromHour.SelectedValue);
                var h2 = Convert.ToInt32(toHour.SelectedValue);


                var m1 = Convert.ToInt32(fromMinute.Text);
                if (m1 < 0 || m1 > 59)
                {
                    lblMessage.Text = "开始时间中分钟的数字范围只能在0-59之间。";
                    return;
                }
                var m2 = Convert.ToInt32(toMinute.Text);
                if (m2 < 0 || m2 > 59)
                {
                    lblMessage.Text = "结束时间中分钟的数字范围只能在0-59之间。";
                    return;
                }

                string zFromDate = fromDate.Text + " " + fromHour.SelectedValue + ":" + fromMinute.Text + ":00";
                string zToDate = toDate.Text + " " + toHour.SelectedValue + ":" + toMinute.Text + ":00";

                DateTime dtFrom = Convert.ToDateTime(zFromDate);
                DateTime dtTo = Convert.ToDateTime(zToDate);

                if (dtTo < dtFrom)
                {
                    lblMessage.Text = "结束时间不可小于开始时间!";
                    return;
                }

                if (DateTime.Now < dtFrom)
                {
                    lblMessage.Text = "开始时间不能大于当前时间。";
                    return;
                }
                #endregion

                List<int> listAreaCoordinates = GetAreaCoordinates();

                Page.Session["SelectedTagIdArray"] = tagUserSelector.SelectedUserIds;
                Page.Session["dtFrom"] = dtFrom;
                Page.Session["dtTo"] = dtTo;
                Page.Session["SearchType"] = radioButtonMode.SelectedValue;
                Page.Session["listAreaCoordinates"] = listAreaCoordinates;

                ScriptManager.RegisterStartupScript(this, this.GetType(), "ReportAreaInOut", "window.open('ReportAreaInOut.aspx');", true);

            }
            catch (Exception err)
            {
                lblMessage.Text += err.ToString();
            }
        }
    }
}
