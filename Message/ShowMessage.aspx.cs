using System;
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
using System.Text;
using NetRadio.Assistant.Web.Util;
namespace NetRadio.LocatingMonitor.Message
{
    public partial class _ShowMessage : BasePage
    {
        protected override bool CheckRight()
        {
            return true;
        }

        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cache[me.Id + "_mesCaption"] != null && Cache[me.Id + "_mesContent"] != null && Cache[me.Id + "_mesLinks"] != null)
            {
                tdCaption.InnerHtml = Cache[me.Id + "_mesCaption"].ToString();
                Link[] links = (Link[])Cache[me.Id + "_mesLinks"];

                StringBuilder sb = new StringBuilder();
                sb.Append("<br/>");
                sb.Append(Cache[me.Id + "_mesContent"]);
                sb.Append("<br/>");
                sb.Append("<br/>");
                sb.Append("<ul>");
                foreach (Link l in links)
                {
                    sb.Append("<li>");
                    sb.Append(l.ToHtmlA());
                    sb.Append("</li>");
                }
                switch (Request.QueryString["masterFile"])
                {
                    case null:
                    case "":
                    case MasterList.Default:
                        //sb.Append("<li>");
                        //sb.Append(Link.Fastback.ToHtmlA());
                        //sb.Append("</li>");
                        break;
                    case MasterList.WebItem:
                        sb.Append("<li>");
                        sb.Append(Link.CloseWindow.ToHtmlA());
                        sb.Append("</li>");
                        break;
                    default: break;
                }
                sb.Append("</ul>");

                tdLinks.InnerHtml = sb.ToString();
            }
        }
    }
}
