using System;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Data;
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

namespace NetRadio.LocatingMonitor.History
{
    public partial class __WristletBrokenLog : BasePage
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

        private TagUserType _userType;
		protected void Page_Load(object sender, EventArgs e) {
            _userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 0);
            if (!IsPostBack)
            {
                LoadDefaultView();
            }
		}


		private void LoadDefaultView() {
            //Sitemap.Text2 = "历史事件";
            //Sitemap.Text3 = this.Wrap.Title;

            historyNavigator1.AppendSearchConditionQueryString(string.Format("userType={0}", (int)_userType));
            this.LoadRepeater();

            //historyNavigator.AppendSearchConditionQueryString(tagLogFilter.ConditionQueryString);
            //this.LoadRepeater();
		}

		private void LoadRepeater() {
            using (AppDataContext db = new AppDataContext())
            {
                SqlCommand myCommand;
                SqlConnection myConnection = (SqlConnection)db.Connection;

                myCommand = new SqlCommand("pNetRadio_PageRecords", myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandTimeout = 600;

                if (Session["TagLogFilter"] != null)
                {
                    TagLogFilter1 = (NetRadio.LocatingMonitor.Controls.__TagLogFilter)Session["TagLogFilter"];
                    string tagFilterCondition = TagLogFilter1.ConditionDescription;
                }

                //过滤条件
                string strWhere = LocatingMonitorUtils.ConstructSQLWhere(TagLogFilter1);
                string zCondition = "GenericEventType= " + (int)SupportEvent.WristletBroken;
                strWhere += (strWhere.Trim() == "") ? zCondition : " AND " + zCondition;

                int? itotalrecords = 0;

                myCommand.Parameters.AddWithValue("@tblName", "DBViewGenericEventLog");
                myCommand.Parameters.AddWithValue("@strGetFields", "*");
                myCommand.Parameters.AddWithValue("@fldName", "ID");
                myCommand.Parameters.AddWithValue("@strWhere", strWhere);
                myCommand.Parameters.AddWithValue("@PageSize", p.PageSize);
                myCommand.Parameters.AddWithValue("@PageIndex", p.PageIndex);
                myCommand.Parameters.AddWithValue("@Sort", "desc");
                myCommand.Parameters.AddWithValue("@totalRecords", itotalrecords);

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlParameter Par = myCommand.Parameters.AddWithValue("@rowcount", SqlDbType.Int);
                Par.Direction = ParameterDirection.Output;


                myConnection.Open();
                myCommand.ExecuteNonQuery();
                adapter.SelectCommand = myCommand;
                DataSet ds = new DataSet();
                adapter.Fill(ds, "DBViewGenericEventLog");

                //取得过滤后的总记录数
                p.RecordCount = Int32.Parse(Par.Value.ToString());

                list.DataSource = ds.Tables[0].DefaultView;
                list.DataBind();

                myConnection.Close();
            }
		}

		protected void list_ItemCreated(object sender, RepeaterItemEventArgs e) {
            DataRowView log = (DataRowView)e.Item.DataItem;
            if (log != null)
            {
                Anchor tagName = e.Item.FindControl("tagName") as Anchor;
                tagName.Text = LocatingMonitorUtils.GetHostName(log["TagName"].ToString(), log["TagMac"].ToString(), log["HostName"].ToString());
                tagName.Href = PathUtil.ResolveUrl("Objects/Tag.aspx?id=" + Convert.ToString(log["TagID"]));
                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;
                writeTime.DisplayValue = Convert.ToDateTime(log["WriteTime"]);
                SmartLabel facilityName = e.Item.FindControl("facilityName") as SmartLabel;
                SmartLabel coordinatesName = e.Item.FindControl("coordinatesName") as SmartLabel;
                facilityName.Text = Convert.ToString(log["FacilityName"]);
                coordinatesName.Text = Convert.ToString(log["CoordinatesName"]);
            }
           
		}
	}
}
