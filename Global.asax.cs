using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml.Linq;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            // BusAppInfo.Instance();
            Summer.ConfigurationManager.Path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\DataBaseMapping.xml";
            Summer.ConfigurationManager.Loaded += new Summer.ConfigurationManager.LoadConfigHandler(ConfigurationManager_Loaded);
        }

        void ConfigurationManager_Loaded()
        {
            if (!Summer.ConfigurationManager.DataBaseMappingTable["LocatingMonitor"].Parameters.ContainsKey("ConnectionString"))
            {
                Summer.ConfigurationManager.DataBaseMappingTable["LocatingMonitor"].AddParameter("ConnectionString", ConfigurationManager.AppSettings["ConnectionString"]);
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}