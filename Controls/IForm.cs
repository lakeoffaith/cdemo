using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

namespace NetRadio.LocatingMonitor.Controls
{
    public class IForm : System.Web.UI.HtmlControls.HtmlForm
    {
        public virtual string RenderAsString()
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            this.Render(writer);
            string str = sw.ToString();
            writer.Close();
            sw.Close();
            return str;
        }
        public virtual string RenderControlAsString()
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            this.RenderControl(writer);
            string str = sw.ToString();
            writer.Close();
            sw.Close();
            return str;
        }
        public virtual string RenderChildrenAsString()
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter writer = new HtmlTextWriter(sw);
            this.RenderChildren(writer);
            string str = sw.ToString();
            writer.Close();
            sw.Close();
            return str;
        }

    }

}
