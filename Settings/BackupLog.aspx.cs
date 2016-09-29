using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Web;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using NetRadio.DataExtension;
using NetRadio.Common;

namespace NetRadio.LocatingMonitor.Settings
{
    public partial class __BackupLog : BasePage
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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
          

            if (!Page.IsPostBack)
            {
                LoadDefaultView();

                if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
                {
                    ObjectNavigator.Visible = true;
                }
                else
                {
                    ObjectNavigator.Visible = false;
                }

            }
        }

        private void LoadDefaultView()
        {
            backupdays.Text = "30";
            readonlyMark.Text = "<font color='red'>(从现在起几天前的数据进行备份)</font>";

            var backupInfor = BackupLogConfig.All;
            if (backupInfor == null || backupInfor.Count == 0)
            {
                feedbacks.Items.AddError("备份配置表未设置！");
                return;
            }
            else
            {
                foreach (var name in backupInfor)
                {
                    if (name.FieldType == "backuppath")
                    {
                        lblBackupPath.Text = name.FieldValue;
                    }
                    else
                    {
                        lblBackupTableList.Text += name.FieldValue + "(" + name.Description + ")" + "<br>";
                    }
                }
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            int Days = int.Parse(backupdays.Text);           //最近几天前的进行备份
            string FPath = lblBackupPath.Text.Trim();        //导出数据所在文件路径 
            int ITest = 0;                                   //为0时为测试，即只创建文件，不删除原始数据
            ITest = int.Parse(Rdolist.SelectedValue);
            string sProcName = "pNetRadio_historyDataExport";

            try
            {
                var backupInfor = BackupLogConfig.All;
                foreach (var name in backupInfor)
                {
                    if (name.FieldType == "backuptable" && name.DateFieldName != "")
                    {
                        using (AppDataContext db = new AppDataContext())
                        {
                            SqlCommand myCommand;
                            SqlConnection myConnection = (SqlConnection)db.Connection;

                            myCommand = new SqlCommand(sProcName, myConnection);
                            myCommand.CommandType = CommandType.StoredProcedure;
                            myCommand.CommandTimeout = 600;

                            myCommand.Parameters.AddWithValue("@Days", Days);
                            myCommand.Parameters.AddWithValue("@TblName", name.FieldValue);         //表名
                            myCommand.Parameters.AddWithValue("@FieldName", name.DateFieldName);    //表对应的时间字段
                            myCommand.Parameters.AddWithValue("@FPath", FPath);
                            myCommand.Parameters.AddWithValue("@iTest ", ITest);

                            myConnection.Open();
                            myCommand.ExecuteNonQuery();
                            myConnection.Close();
                        }
                    }
                }
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message",
                    @"
                    var l=window.onload;
                    window.onload=function()
                    {   
                        if(l!=null)                     
                        {l();}
                         window.setTimeout('window.alert(\'备份完成！\');',2)
                        window.onload=l;
                    }
                    "
                    , true);
            }
            catch (Exception ex)
            {
                string exMessage;
                exMessage = "备份失败!原因：" + ex.Message;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "window.alert(" + exMessage + ");", true);
                throw;
            }

        }
    }
}
