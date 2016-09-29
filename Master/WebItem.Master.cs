using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NetRadio.Web;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor
{
    public partial class WebItem :BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPageData();
        }
        /// <summary>
        /// 系统名称、标题栏、页面地图、用户名
        /// </summary>
        private void InitPageData()
        {           
            HtmlTitle hTitle = new HtmlTitle();
            if (hidTitle.Attributes["sign"] == "true")
            {
                hTitle.Text = hidTitle.Value;
            }
            else
            {
                hTitle.Text = Request.GetTitle();
            }
            if (labWebSite.Attributes["Sign"] == "false")
            {
                labWebSite.Text = Request.GetSiteMap();
            }
            Head1.Controls.Add(hTitle);           
        }
    }
}
