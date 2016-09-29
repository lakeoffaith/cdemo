using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using NetRadio.DataExtension;
using System.Text;
using System.Reflection;
using System.Collections;
using NetRadio.Web;

namespace NetRadio.LocatingMonitor.Report
{
    public partial class __ReportAreaInOut : BasePage
    {
        private TagUserType _userType;
        List<int> listAreaCoordinates;
        int[] SelectedTagIdArray;
        DateTime dtFrom;
        DateTime dtTo;
        string SearchType;
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/ReportIndex.aspx.js");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 1);

            listAreaCoordinates = (List<int>)Session["listAreaCoordinates"];
            SelectedTagIdArray = (int[])Session["SelectedTagIdArray"];
            dtFrom = DateTime.Parse(Session["dtFrom"].ToString());
            dtTo = DateTime.Parse(Session["dtTo"].ToString());
            SearchType = Session["SearchType"].ToString();

            if (!IsPostBack)
            {
                LoadDefaultView();
            }
        }

        private void LoadDefaultView()
        {
            this.btnExport.Visible = true;
            LoadRepeater(listAreaCoordinates, SelectedTagIdArray, dtFrom, dtTo, SearchType);
        }

        private void LoadRepeater(List<int> listAreaCoordinates, int[] TagIdArray, DateTime dtFrom, DateTime dtTo, string SearchType)
        {
            try
            {
                IList<AreaInOutTime> lTime = new List<AreaInOutTime>();
                int totalCount = 0;
                using (AppDataContext db = new AppDataContext())
                {

                    for (int i = 0; i < TagIdArray.Length; i++)
                    {
                        long totalTicks = 0;
                        int entryCount = 0;
                        DateTime firstInTime = DateTime.MinValue;
                        DateTime lastOutTime = DateTime.MinValue;

                        DateTime endTime = DateTime.Now;
                        if (dtTo < endTime) endTime = dtTo;

                        int hostId = TagIdArray[i];
                        HostTag oHost = HostTag.GetById(hostId);
                        if (oHost == null)
                        {
                            lblMessage.Text += String.Format("ID={0}不存在", hostId);
                            continue;
                        }
                        int tagId = oHost.TagId;
                        string hostName = oHost.HostName;
                        IList<TagPositionLog> lLog = db.TagPositionLogs
                            .Where(x => x.HostId == hostId && x.WriteTime >= dtFrom && x.WriteTime <= endTime)
                            .OrderBy(x => x.WriteTime).ToList();

                        var firstQuery = db.TagPositionLogs.Where(x => x.WriteTime < dtFrom && x.HostId == hostId)
                            .OrderByDescending(x => x.WriteTime).Take(1);
                        TagPositionLog prevLog = null;
                        try
                        {
                            if (firstQuery != null) prevLog = firstQuery.First();
                        }
                        catch { }

                        AreaInOutTime areaTime = new AreaInOutTime();
                        areaTime.hostName = hostName;
                        areaTime.hostId = hostId;

                        //previous log
                        if (prevLog != null)
                        {
                            if (prevLog.X > 0)
                            {
                                if (listAreaCoordinates.Contains(prevLog.CoordinatesId))
                                {
                                    areaTime.inTime = dtFrom;
                                    firstInTime = areaTime.inTime;
                                }
                            }
                        }

                        bool bFirst = true;
                        if (lLog != null && lLog.Count > 0)
                        {
                            foreach (TagPositionLog log in lLog)
                            {
                                DateTime dtWriteTime = new DateTime(log.WriteTime.Year, log.WriteTime.Month, log.WriteTime.Day,
                                    log.WriteTime.Hour, log.WriteTime.Minute, log.WriteTime.Second);
                                if (bFirst)
                                {
                                    bFirst = false;

                                    //first log
                                    if (log.X > 0 && listAreaCoordinates.Contains(log.CoordinatesId))
                                    {
                                        if (areaTime.inTime == DateTime.MinValue)
                                        {
                                            areaTime.inTime = dtWriteTime;
                                            firstInTime = areaTime.inTime;
                                        }
                                    }
                                    else
                                    {
                                        areaTime.outTime = dtWriteTime;
                                        if (areaTime.inTime < dtWriteTime)
                                            lastOutTime = areaTime.outTime;
                                    }
                                }
                                else
                                {
                                    if (log.X > 0 && listAreaCoordinates.Contains(log.CoordinatesId))
                                    {
                                        if (areaTime.inTime == DateTime.MinValue)
                                        {
                                            areaTime.inTime = dtWriteTime;
                                            firstInTime = areaTime.inTime;
                                            if (areaTime.outTime != DateTime.MinValue)
                                            {
                                                areaTime.outTime = DateTime.MinValue;
                                            }
                                        }
                                        else
                                        {
                                            if (areaTime.outTime == DateTime.MinValue)
                                            {

                                            }
                                            else
                                            {
                                                double disappearedTime = (dtWriteTime - areaTime.outTime).Duration().TotalSeconds;
                                                if (disappearedTime > 60)
                                                {
                                                    areaTime.stayTime = (areaTime.outTime - areaTime.inTime).Duration();
                                                    totalTicks += areaTime.stayTime.Ticks;
                                                    if (SearchType == "2")
                                                        lTime.Add(areaTime);
                                                    entryCount++;
                                                    areaTime = new AreaInOutTime();
                                                    areaTime.inTime = dtWriteTime;
                                                }
                                                else
                                                    areaTime.outTime = DateTime.MinValue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (areaTime.outTime == DateTime.MinValue)
                                        {
                                            areaTime.outTime = dtWriteTime;
                                            lastOutTime = areaTime.outTime;
                                        }
                                    }
                                }
                            }
                        }

                        if (areaTime.inTime != DateTime.MinValue)
                        {
                            if (areaTime.outTime != DateTime.MinValue)
                            {
                                double disappearedTime = (endTime - areaTime.outTime).Duration().TotalSeconds;
                                if (disappearedTime > 60)
                                {
                                    areaTime.stayTime = (areaTime.outTime - areaTime.inTime).Duration();
                                    totalTicks += areaTime.stayTime.Ticks;
                                    if (SearchType == "2")
                                        lTime.Add(areaTime);
                                    entryCount++;
                                }
                                else
                                {
                                    areaTime.stayTime = (endTime - areaTime.inTime).Duration();
                                    totalTicks += areaTime.stayTime.Ticks;
                                    if (SearchType == "2")
                                        lTime.Add(areaTime);
                                    entryCount++;
                                }
                                lastOutTime = areaTime.outTime;
                            }
                            else
                            {
                                areaTime.stayTime = (endTime - areaTime.inTime).Duration();
                                totalTicks += areaTime.stayTime.Ticks;
                                if (SearchType == "2")
                                    lTime.Add(areaTime);
                                entryCount++;
                            }
                        }

                        if (totalTicks > 0)
                        {
                            lastOutTime = areaTime.outTime; //record the last out time
                            areaTime = new AreaInOutTime();
                            areaTime.hostId = -1;
                            if (SearchType == "1")
                            {
                                areaTime.hostName = hostName;
                                areaTime.inTime = firstInTime;
                                areaTime.outTime = lastOutTime; //assign last out time
                                //if (areaTime.inTime < lastOutTime)
                                //    areaTime.outTime = lastOutTime;
                            }
                            else if (SearchType == "2")
                            {
                                areaTime.hostName = "--- ---";
                                areaTime.inTime = DateTime.MinValue;
                                areaTime.outTime = DateTime.MinValue;
                            }
                            areaTime.stayTime = new TimeSpan(totalTicks);
                            lTime.Add(areaTime);
                            totalCount++;
                        }
                        else
                        {
                            areaTime = new AreaInOutTime();
                            areaTime.hostName = hostName;
                            areaTime.inTime = DateTime.MinValue;
                            areaTime.outTime = DateTime.MinValue;
                            areaTime.stayTime = new TimeSpan(0);
                            lTime.Add(areaTime);
                            totalCount++;
                        }

                    }
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("hostName", typeof(string));
                dt.Columns.Add("inTime", typeof(string));
                dt.Columns.Add("outTime", typeof(string));
                dt.Columns.Add("stayTime", typeof(string));
                for (int i = 0; i < lTime.Count(); i++)
                {
                    string inTimeValue = (lTime.ElementAt(i).inTime == DateTime.MinValue) ? "" : lTime.ElementAt(i).inTime.ToString();
                    string outTimeValue = (lTime.ElementAt(i).outTime == DateTime.MinValue) ? "" : lTime.ElementAt(i).outTime.ToString();
                    dt.Rows.Add(
                     lTime.ElementAt(i).hostName,
                     inTimeValue,
                     outTimeValue,
                     lTime.ElementAt(i).stayTime.ToString()
                     );
                }
                ViewState["dt"] = dt;

                list.DataSource = lTime;
                list.DataBind();
            }
            catch (Exception err)
            {
                lblMessage.Text += err.ToString();
            }
            lbltime.Text = "     从[" + dtFrom.ToString("yyyy年MM月dd日HH时mm分", DateTimeFormatInfo.InvariantInfo) + "] 到 [" + dtTo.ToString("yyyy年MM月dd日HH时mm分", DateTimeFormatInfo.InvariantInfo) + "]";
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["dt"];
            // AreaInOutTime areaTime = (AreaInOutTime)e.Item.DataItem;
            StringWriter sw = new StringWriter();
            sw.WriteLine(lbltime.Text);
            sw.WriteLine("人员姓名\t进入区域时间\t消失或离开时间\t停留时间");
            foreach (DataRow dr in dt.Rows)
            {
                TimeSpan areaTime = TimeSpan.Parse(dr["stayTime"].ToString());
                string stayTime = "";
                string inTime = string.Format("{0}", dr["inTime"]);
                string outTime = string.Format("{0}", dr["outTime"]);
                if (dr["hostName"].ToString() == "--- ---")
                {
                    stayTime = "(";
                }
                if (areaTime.Ticks == 0)
                {
                    stayTime += "0";
                }
                else
                {
                    if (areaTime.Days > 0)
                        stayTime += areaTime.Days.ToString() + "天";
                    if (areaTime.Hours > 0)
                        stayTime += areaTime.Hours.ToString() + "时";
                    if (areaTime.Minutes > 0)
                        stayTime += areaTime.Minutes.ToString() + "分";
                    if (areaTime.Seconds > 0)
                        stayTime += areaTime.Seconds.ToString() + "秒";
                }
                if (dr["hostName"].ToString() == "--- ---")
                {
                    stayTime += ")";
                    outTime = "";
                    inTime = "";
                }

                sw.WriteLine(dr["hostName"].ToString() + "\t" + inTime + "\t" + outTime + "\t" + stayTime);
            }
            sw.Close();

            Response.Charset = "GB2312";
            //Response.ContentEncoding = System.Text.Encoding.UTF8;
            //Response.Write("<meta http-equiv=Content-Type content=text/html;charset=UTF-8>");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode("AreaInOutTimeCount.xls", Encoding.UTF8).ToString());

            Response.ContentType = "application/ms-excel";
            Response.Write(sw);
            Response.End();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            AreaInOutTime areaTime = (AreaInOutTime)e.Item.DataItem;
            if (areaTime != null)
            {
                Img icon = e.Item.FindControl("icon") as Img;
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;

                DateTimeLabel inTime = e.Item.FindControl("inTime") as DateTimeLabel;
                DateTimeLabel outTime = e.Item.FindControl("outTime") as DateTimeLabel;
                SmartLabel duration = e.Item.FindControl("duration") as SmartLabel;

                tagName.Text = Convert.ToString(areaTime.hostName);
                if (areaTime.inTime != DateTime.MinValue)
                    inTime.DisplayValue = areaTime.inTime;
                if (areaTime.outTime != DateTime.MinValue)
                    outTime.DisplayValue = areaTime.outTime;

                duration.Text = "";
                if (tagName.Text == "--- ---")
                {
                    duration.Text = "(";
                }
                if (areaTime.stayTime.Ticks == 0)
                {
                    duration.Text += "0";
                }
                else
                {
                    if (areaTime.stayTime.Days > 0)
                        duration.Text += areaTime.stayTime.Days.ToString() + "天";
                    if (areaTime.stayTime.Hours > 0)
                        duration.Text += areaTime.stayTime.Hours.ToString() + "时";
                    if (areaTime.stayTime.Minutes > 0)
                        duration.Text += areaTime.stayTime.Minutes.ToString() + "分";
                    if (areaTime.stayTime.Seconds > 0)
                        duration.Text += areaTime.stayTime.Seconds.ToString() + "秒";
                }
                if (tagName.Text == "--- ---")
                {
                    duration.Text += ")";
                }
            }
        }

    }
}
