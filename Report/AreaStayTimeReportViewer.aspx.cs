using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using System.IO;
using NetRadio.LocatingService.RemotingEntry;
using System.Data;
using System.Data.SqlClient;
using NetRadio.Assistant.Web.Util; 
//using CrystalDecisions.CrystalReports.Engine;
using System.Globalization;
//using NetRadio.LocatingMonitor.Report.CrystalReports;

namespace NetRadio.LocatingMonitor.Report
{
	public partial class __AreaStayTimeReportViewer : Page
	{
        int areaId;
        DateTime fromTime;
        DateTime toTime;
        string tagIdArray;
      
      	protected void Page_Load(object sender, EventArgs e) {
			 areaId = Fetch.QueryUrlAsInteger("areaId");
			 fromTime = DateTime.Parse(Fetch.QueryUrl("fromTime"));
			 toTime = DateTime.Parse(Fetch.QueryUrl("toTime"));
			 tagIdArray = Fetch.QueryUrl("tagIdArray");
            
            if (!IsPostBack)
            {
                if (fromTime > toTime)
                {
                    this.lbltitle.Text = "开始时间大于结束时间，不能进行统计！";
                    this.btnExport.Visible = false;
                }
                else
                {
                    this.btnExport.Visible = true;
                    this.GridView1.Attributes.Add("SortExpression", "TagName");
                    this.GridView1.Attributes.Add("SortDirection", "ASC");
                    LoadRepeater(areaId, fromTime, toTime, tagIdArray);
                }
            }

			using (AppDataContext db = new AppDataContext()) {
                //SqlDataAdapter adapter = new SqlDataAdapter("GetInAreaAndDisappearTime", (SqlConnection)db.Connection);
                //adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                //adapter.SelectCommand.Parameters.AddWithValue("@areaId", areaId);
                //adapter.SelectCommand.Parameters.AddWithValue("@fromTime", fromTime);
                //adapter.SelectCommand.Parameters.AddWithValue("@toTime", toTime);
                //adapter.SelectCommand.Parameters.AddWithValue("@tagIdArray", tagIdArray);
                //DataSet ds = new DataSet();
                //adapter.Fill(ds); 
                //ReportDocument doc = new AreaStayTimeReport();
                //doc.SetDataSource(ds.Tables[0]);
                //reportViewer.ReportSource = doc;
			}
		}

        private void LoadRepeater(int tagid,DateTime fromtime,DateTime totime,string tagidarray)
        {
            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;

                myCommand = new SqlCommand("pNetRadio_CountInAreaAndDisppearTime", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600; 

                myCommand.Parameters.Add("@areaId", tagid);
                myCommand.Parameters.Add("@fromTime", fromtime);
                myCommand.Parameters.Add("@toTime", totime);
                myCommand.Parameters.Add("@tagIdArray", tagidarray);

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlParameter Par = myCommand.Parameters.AddWithValue("@rowscount", SqlDbType.Int);
                Par.Direction = ParameterDirection.Output;

              
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                adapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "A");

                int count = 0;
                if(ds.Tables.Count>0)
                {
                  count=ds.Tables.Count-1;
                }

                string sortExpression = this.GridView1.Attributes["SortExpression"];
                string sortDirection = this.GridView1.Attributes["SortDirection"]; 
             
                if ((!string.IsNullOrEmpty(sortExpression)) && (!string.IsNullOrEmpty(sortDirection)))   
                {
                  ds.Tables[count].DefaultView.Sort = string.Format("{0} {1}", sortExpression, sortDirection);   
                }   

                this.GridView1.DataSource = ds.Tables[count];
                this.GridView1.DataBind();

                lbltime.Text = "     从[" + fromtime.ToString("yyyy年MM月dd日HH时mm分", DateTimeFormatInfo.InvariantInfo) +
                    "] 到 [" + totime.ToString("yyyy年MM月dd日HH时mm分", DateTimeFormatInfo.InvariantInfo) + "]";

                lbltitle.Text = "区域停留和消失时间统计";
                //list.DataSource = ds.Tables[count];
                //list.DataBind(); 
                ds = null;
                myConnection.Close();
                //p.RecordCount = Convert.ToInt32(Par.Value.ToString());
                //p.PageSize = p.RecordCount;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            LoadRepeater(areaId, fromTime, toTime, tagIdArray);
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        { 
            string sortExpression = e.SortExpression.ToString(); 
            string sortDirection = "ASC";
 
            if (sortExpression == this.GridView1.Attributes["SortExpression"])
            { 
                sortDirection = (this.GridView1.Attributes["SortDirection"].ToString() == sortDirection ? "DESC" : "ASC");
            }
 
            this.GridView1.Attributes["SortExpression"] = sortExpression;
            this.GridView1.Attributes["SortDirection"] = sortDirection;

            LoadRepeater(areaId, fromTime, toTime, tagIdArray);
        }

        protected void btnExport_Click(object sender, System.EventArgs e)
        {
            this.GridView1.AllowPaging = false;
            this.GridView1.AllowSorting = false;
            LoadRepeater(areaId, fromTime, toTime, tagIdArray);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=NetRadioCountTime.xls"); 
            //Response.ContentEncoding = System.Text.Encoding.UTF7;
            Response.ContentType = "application/ms-excel"; 
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
            this.GridView1.Caption = this.lbltitle.Text + "              " + this.lbltime.Text;
            this.GridView1.RenderControl(oHtmlTextWriter);
            Response.Output.Write(oStringWriter.ToString());
            Response.Flush();
            Response.End();
            this.GridView1.AllowPaging = true;
            this.GridView1.AllowSorting = true;
            this.GridView1.Caption = "";
            LoadRepeater(areaId, fromTime, toTime, tagIdArray);

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes.Add("style", "vnd.ms-excel.numberformat:@;");
            }
        }

        protected void list_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

            //DataRowView log = (DataRowView)e.Item.DataItem;
          
            //    if (log != null)
            //    {
            //        SmartLabel TagName = e.Item.FindControl("TagName") as SmartLabel;
            //        TagName.Text = Convert.ToString(log["TagName"]);
            //        SmartLabel InArea = e.Item.FindControl("InArea") as SmartLabel;
            //        InArea.Text = Convert.ToString(log["InArea"]);
            //        SmartLabel Disappear = e.Item.FindControl("Disappear") as SmartLabel;
            //        Disappear.Text = Convert.ToString(log["Disappear"]); 

            //    }
            
        }

	}
}
