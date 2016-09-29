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
    public partial class __TagAlertUI : BasePage
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
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sitemap.Text2 = "统计报警";
            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
                InitListData();
            }

        }

        protected void InitListData()
        {
            this.AlertUserList.Items.Clear();

            AlertUserList.Items.Insert(0, "警察");
            AlertUserList.Items.Insert(1, "犯人");

            this.AlertStatusTypeList.Items.Clear();
            AlertStatusTypeList.Items.Insert(0, "报警状态");
            AlertStatusTypeList.Items.Insert(1, "报警类型");

        }

        protected void GetFatherData(string AlertUser, string AlertStatusType, string sStartTime, string sEndTime)
        {
            string sProcName = "";

            int iMasterUserID = 1;
            if (AlertUser.ToString().Trim() == "警察")
            {
                iMasterUserID = 0;
            }

            string strWhere = "";
            string sAlertStatusType = "";
            sAlertStatusType = AlertStatusType.ToString().Trim();

            //int itime = -1;//
            //if (AlertTime.ToString().Trim() == "天")
            //{
            //    itime = 0;
            //    strWhere = "datediff(day,Writetime,getdate())=0";
            //}
            //else if (AlertTime.ToString().Trim() == "周")
            //{
            //    itime = 1;
            //    strWhere = "datediff(week,Writetime,getdate())=0";
            //}
            //else
            //{
            //    itime = 2;  //月
            //    strWhere = "datediff(month,Writetime,getdate())=0";
            //}

            strWhere = "WriteTime>'" + sStartTime + "' and WriteTime<'" + sEndTime + "'";
            strWhere += " and MasterUserID=" + iMasterUserID;

            int iST = -1;//报警状态或者类型，状态为0，类型为1
            if (sAlertStatusType == "报警状态")
            {
                iST = 0;
                sProcName = "pNetRadio_AlertStatusCount";
            }
            else
            {
                iST = 1;
                sProcName = "pNetRadio_AlertTypeCount";
            }

            //总的数量
            //报警状态下
            int inew = 0;
            int iProcessing = 0;
            int iResolved = 0;
            int isCount = 0;//报警状态总数

            //报警类型下
            int iAreaEvent = 0;
            int iAbsent = 0;
            int iBatteryInsufficient = 0;
            int iBatteryReset = 0;
            int iButtonPressed = 0;
            int iWristletBroken = 0;
            int iAPLocatorDown = 0;
            int itCount = 0;//报警类型总数


            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;

                myCommand = new SqlCommand(sProcName, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600;
                myCommand.Parameters.Add("@strWhere", strWhere);

                SqlDataAdapter adapter = new SqlDataAdapter();

                if (iST == 0)//报警状态下
                {
                    SqlParameter ParNew = myCommand.Parameters.AddWithValue("@inew", SqlDbType.Int);
                    ParNew.Direction = ParameterDirection.Output;
                    SqlParameter ParProcessing = myCommand.Parameters.AddWithValue("@iProcessing", SqlDbType.Int);
                    ParProcessing.Direction = ParameterDirection.Output;
                    SqlParameter ParResolved = myCommand.Parameters.AddWithValue("@iResolved", SqlDbType.Int);
                    ParResolved.Direction = ParameterDirection.Output;

                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    adapter.SelectCommand = myCommand;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "monitor_TagAlert");

                    myConnection.Close();
                    inew = Int32.Parse(ParNew.Value.ToString());
                    iProcessing = Int32.Parse(ParProcessing.Value.ToString());
                    iResolved = Int32.Parse(ParResolved.Value.ToString());


                    //此时取得了表Report_TotalCount的报警状态统计
                    //下面取出表history_AlertProcessLog的报警状态统计
                    int i_new = 0;
                    int i_Processing = 0;
                    int i_Resolved = 0;

                    // GetDataFromhistory_AlertProcessLog(sProcName, out i_new, out i_Processing, out i_Resolved);
                    inew += i_new;
                    iProcessing += i_Processing;
                    iResolved += i_Resolved;
                    //总计
                    isCount = inew + iProcessing + iResolved;

                    //修改Report_TotalCount记录
                    var MySQL = "Update  Report_TotalCount Set sNew=" + inew + ",sProcessing=" + iProcessing +
                                ",sResolved=" + iResolved + ",sTotal=" + isCount;
                    var conn = (SqlConnection)db.Connection;
                    var cmd = new SqlCommand(MySQL, conn);
                    cmd.Connection.Open();
                    var irows = cmd.ExecuteNonQuery();
                    conn.Close();

                }
                else//报警类型下
                {
                    SqlParameter ParAreaEvent = myCommand.Parameters.AddWithValue("@AreaEvent", SqlDbType.Int);
                    ParAreaEvent.Direction = ParameterDirection.Output;
                    SqlParameter ParAbsent = myCommand.Parameters.AddWithValue("@Absent", SqlDbType.Int);
                    ParAbsent.Direction = ParameterDirection.Output;
                    SqlParameter ParBatteryInsufficient = myCommand.Parameters.AddWithValue("@BatteryInsufficient", SqlDbType.Int);
                    ParBatteryInsufficient.Direction = ParameterDirection.Output;
                    SqlParameter ParBatteryReset = myCommand.Parameters.AddWithValue("@BatteryReset", SqlDbType.Int);
                    ParBatteryReset.Direction = ParameterDirection.Output;
                    SqlParameter ParButtonPressed = myCommand.Parameters.AddWithValue("@ButtonPressed", SqlDbType.Int);
                    ParButtonPressed.Direction = ParameterDirection.Output;
                    SqlParameter ParWristletBroken = myCommand.Parameters.AddWithValue("@WristletBroken", SqlDbType.Int);
                    ParWristletBroken.Direction = ParameterDirection.Output;
                    SqlParameter ParAPLocatorDown = myCommand.Parameters.AddWithValue("@APLocatorDown", SqlDbType.Int);
                    ParAPLocatorDown.Direction = ParameterDirection.Output;

                    myConnection.Open();
                    myCommand.ExecuteNonQuery();
                    adapter.SelectCommand = myCommand;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "monitor_TagAlert");

                    myConnection.Close();
                    iAreaEvent = Int32.Parse(ParAreaEvent.Value.ToString());
                    iAbsent = Int32.Parse(ParAbsent.Value.ToString());
                    iBatteryInsufficient = Int32.Parse(ParBatteryInsufficient.Value.ToString());
                    iBatteryReset = Int32.Parse(ParBatteryReset.Value.ToString());
                    iButtonPressed = Int32.Parse(ParButtonPressed.Value.ToString());
                    iWristletBroken = Int32.Parse(ParWristletBroken.Value.ToString());
                    iAPLocatorDown = Int32.Parse(ParAPLocatorDown.Value.ToString());
                    itCount = iAreaEvent + iAbsent + iBatteryInsufficient + iBatteryReset +
                              iButtonPressed + iWristletBroken + iAPLocatorDown;

                    //修改Report_TotalCount记录
                    var MySQL = "Update  Report_TotalCount Set tAreaEvent=" + iAreaEvent + ",tAbsent=" + iAbsent +
                                ",tBatteryInsufficient=" + iBatteryInsufficient + ",tBatteryReset=" + iBatteryReset +
                                ",tButtonPressed=" + iButtonPressed + ",tWristletBroken=" + iWristletBroken +
                                ",tAPLocatorDown=" + iAPLocatorDown + ",tTotal=" + itCount;
                    var conn = (SqlConnection)db.Connection;
                    var cmd = new SqlCommand(MySQL, conn);
                    cmd.Connection.Open();
                    var irows = cmd.ExecuteNonQuery();
                    conn.Close();

                }


                ////////父级数据已经读出，下面读取子级数据//////////

                string strQuery = strWhere;
                //当报警状态分别为1,2,3时，取出与之对应的数据
                if (iST == 0)//报警状态下
                {

                    for (int i = 1; i < 4; i++)
                    {
                        strWhere = strQuery;
                        StatusSonData(i, out iAreaEvent, out iAbsent, out iBatteryInsufficient, out iBatteryReset,
                                      out iButtonPressed, out iWristletBroken, out iAPLocatorDown, ref strWhere);


                        //修改Report_AlertStatus_Type记录
                        var MySQL = "Update  Report_AlertStatus_Type Set tAreaEvent=" + iAreaEvent + ",tAbsent=" + iAbsent +
                                    ",tBatteryInsufficient=" + iBatteryInsufficient + ",tBatteryReset=" + iBatteryReset +
                                    ",tButtonPressed=" + iButtonPressed + ",tWristletBroken=" + iWristletBroken +
                                    ",tAPLocatorDown=" + iAPLocatorDown + " where Status=" + i;
                        var conn = (SqlConnection)db.Connection;
                        var cmd = new SqlCommand(MySQL, conn);
                        cmd.Connection.Open();
                        var irows = cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                }
                else
                {
                    for (int i = 1; i < 8; i++)
                    {
                        strWhere = strQuery;
                        TypeSonData(i, out inew, out iProcessing, out iResolved, ref strWhere);

                        //修改Report_AlertType_Status记录
                        var MySQL = "Update  Report_AlertType_Status Set sNew=" + inew + ",sProcessing=" + iProcessing +
                                    ",sResolved=" + iResolved + " where tType=" + i;
                        var conn = (SqlConnection)db.Connection;
                        var cmd = new SqlCommand(MySQL, conn);
                        cmd.Connection.Open();
                        var irows = cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                }
            }

        }


        protected void TypeSonData(int itype, out int inew, out int iProcessing, out int iResolved, ref string strWhere)
        {

            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;

                myCommand = new SqlCommand("pNetRadio_AlertStatusCount", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600;
                strWhere += " and AlertType=" + itype;
                myCommand.Parameters.Add("@strWhere", strWhere);

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlParameter ParNew = myCommand.Parameters.AddWithValue("@inew", SqlDbType.Int);
                ParNew.Direction = ParameterDirection.Output;
                SqlParameter ParProcessing = myCommand.Parameters.AddWithValue("@iProcessing", SqlDbType.Int);
                ParProcessing.Direction = ParameterDirection.Output;
                SqlParameter ParResolved = myCommand.Parameters.AddWithValue("@iResolved", SqlDbType.Int);
                ParResolved.Direction = ParameterDirection.Output;


                myConnection.Open();
                myCommand.ExecuteNonQuery();
                adapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "monitor_TagAlert");

                inew = Int32.Parse(ParNew.Value.ToString());
                iProcessing = Int32.Parse(ParProcessing.Value.ToString());
                iResolved = Int32.Parse(ParResolved.Value.ToString());
                myConnection.Close();
            }

        }

        protected void StatusSonData(int istatus, out int iAreaEvent, out int iAbsent, out int iBatteryInsufficient,
                                  out int iBatteryReset, out int iButtonPressed, out int iWristletBroken,
                                  out int iAPLocatorDown, ref string strWhere)
        {

            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;
                myCommand = new SqlCommand("pNetRadio_AlertTypeCount", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600;
                strWhere += " and AlertStatus=" + istatus;
                myCommand.Parameters.Add("@strWhere", strWhere);

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlParameter ParAreaEvent = myCommand.Parameters.AddWithValue("@AreaEvent", SqlDbType.Int);
                ParAreaEvent.Direction = ParameterDirection.Output;
                SqlParameter ParAbsent = myCommand.Parameters.AddWithValue("@Absent", SqlDbType.Int);
                ParAbsent.Direction = ParameterDirection.Output;
                SqlParameter ParBatteryInsufficient = myCommand.Parameters.AddWithValue("@BatteryInsufficient", SqlDbType.Int);
                ParBatteryInsufficient.Direction = ParameterDirection.Output;
                SqlParameter ParBatteryReset = myCommand.Parameters.AddWithValue("@BatteryReset", SqlDbType.Int);
                ParBatteryReset.Direction = ParameterDirection.Output;
                SqlParameter ParButtonPressed = myCommand.Parameters.AddWithValue("@ButtonPressed", SqlDbType.Int);
                ParButtonPressed.Direction = ParameterDirection.Output;
                SqlParameter ParWristletBroken = myCommand.Parameters.AddWithValue("@WristletBroken", SqlDbType.Int);
                ParWristletBroken.Direction = ParameterDirection.Output;
                SqlParameter ParAPLocatorDown = myCommand.Parameters.AddWithValue("@APLocatorDown", SqlDbType.Int);
                ParAPLocatorDown.Direction = ParameterDirection.Output;

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                adapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "monitor_TagAlert");

                iAreaEvent = Int32.Parse(ParAreaEvent.Value.ToString());
                iAbsent = Int32.Parse(ParAbsent.Value.ToString());
                iBatteryInsufficient = Int32.Parse(ParBatteryInsufficient.Value.ToString());
                iBatteryReset = Int32.Parse(ParBatteryReset.Value.ToString());
                iButtonPressed = Int32.Parse(ParButtonPressed.Value.ToString());
                iWristletBroken = Int32.Parse(ParWristletBroken.Value.ToString());
                iAPLocatorDown = Int32.Parse(ParAPLocatorDown.Value.ToString());
                myConnection.Close();
            }

        }

        //history_AlertProcessLog
        private void GetDataFromhistory_AlertProcessLog(string sProcName, out int inew, out int iProcessing, out int iResolved)
        {
            inew = 0;
            iProcessing = 0;
            iResolved = 0;
            string strWhere = "";
            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;

                myCommand = new SqlCommand(sProcName, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600;
                myCommand.Parameters.Add("@strWhere", strWhere);

                SqlDataAdapter adapter = new SqlDataAdapter();


                myCommand.Parameters.Add("@tblName", "history_AlertProcessLog");
                SqlParameter ParNew = myCommand.Parameters.AddWithValue("@inew", SqlDbType.Int);
                ParNew.Direction = ParameterDirection.Output;
                SqlParameter ParProcessing = myCommand.Parameters.AddWithValue("@iProcessing", SqlDbType.Int);
                ParProcessing.Direction = ParameterDirection.Output;
                SqlParameter ParResolved = myCommand.Parameters.AddWithValue("@iResolved", SqlDbType.Int);
                ParResolved.Direction = ParameterDirection.Output;

                myConnection.Open();
                myCommand.ExecuteNonQuery();
                adapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "monitor_TagAlert");

                myConnection.Close();
                inew = Int32.Parse(ParNew.Value.ToString());
                iProcessing = Int32.Parse(ParProcessing.Value.ToString());
                iResolved = Int32.Parse(ParResolved.Value.ToString());

            }
        }

        private void LoadDefaultView()
        {
            fromDate.Text = DateTime.Today.AddDays(-1).ToString("yyyy-M-d");
            toDate.Text = DateTime.Today.ToString("yyyy-M-d");

        }

        protected void FindCount_Click(object sender, EventArgs e)
        {
            string sAlertUser = "";
            string sAlertStatusType = "";

            sAlertUser = this.AlertUserList.SelectedValue;
            sAlertStatusType = this.AlertStatusTypeList.SelectedValue;

            //
            string d1 = "";
            string d2 = "";
            d1 = fromDate.Text.Trim();
            d2 = toDate.Text.Trim();
            if (d1 == "")
            {
                Response.Write("<script>alert('请选择要统计的开始日期!')</script>");
                return;
            }
            if (d2 == "")
            {
                Response.Write("<script>alert('请选择要统计的结束日期!')</script>");
                return;
            }

            string h1 = "";
            string h2 = "";
            string m1 = "";
            string m2 = "";
            h1 = this.fromHour.SelectedValue;
            h2 = toHour.SelectedValue;
            m1 = fromMinute.Text.Trim();
            if (Convert.ToInt32(m1) < 0 || Convert.ToInt32(m1) > 59)
            {
                Response.Write("<script>alert('开始时间中分钟的数字范围只能在0-59之间!')</script>");
                return;
            }
            m2 = toMinute.Text.Trim();
            if (Convert.ToInt32(m2) < 0 || Convert.ToInt32(m2) > 59)
            {
                Response.Write("<script>alert('结束时间中分钟的数字范围只能在0-59之间!')</script>");
                return;
            }

            string sStarttime = "";//开始时间
            string sEndTime = "";//结束时间
            sStarttime = d1 + " " + h1 + ":" + m1 + ":00";
            sEndTime = d2 + " " + h2 + ":" + m2 + ":00";

            GetFatherData(sAlertUser, sAlertStatusType, sStarttime, sEndTime);
            sAlertUser = System.Web.HttpUtility.UrlEncode(sAlertUser);
            //sAlertTime = System.Web.HttpUtility.UrlEncode(sAlertTime);
            sStarttime = System.Web.HttpUtility.UrlEncode(sStarttime);
            sEndTime = System.Web.HttpUtility.UrlEncode(sEndTime);
            if (sAlertStatusType == "报警状态")
            {

                Response.Write("<script>window.open('TagAlertStatus.aspx?AlertUser=" + sAlertUser
                                + "&starttime=" + sStarttime + "&endtime=" + sEndTime + "')</script>");

            }
            else
            {
                Response.Write("<script>window.open('TagAlertType.aspx?AlertUser=" + sAlertUser
                                 + "&starttime=" + sStarttime + "&endtime=" + sEndTime + "')</script>");
            }
        }

    }
}
