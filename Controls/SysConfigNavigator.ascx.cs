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
using NetRadio.Assistant.Web.Util;
using NetRadio.DataExtension;
using NetRadio.Common;

namespace NetRadio.YangzhouJail.Web.Controls
{
    public partial class SysConfigNavigator : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IdentifyTabViewSelectedIndex();
        }

        private void IdentifyTabViewSelectedIndex()
        {
            string fileName = Request.Path.Substring(Request.Path.LastIndexOf('/') + 1).ToLower();

            for (int i = 0; i < tabView.Items.Count; i++)
            {
                var item = tabView.Items[i];
                if (item.Href.ToLower().Contains(fileName))
                {
                    tabView.SelectedIndex = i;
                    break;
                }
            }
        }

        public void AppendSearchConditionQueryString(string conditionQueryString)
        {
            if (conditionQueryString.Length != 0)
            {
                foreach (var item in tabView.Items)
                {
                    if (item.Href.Length > 0)
                    {
                        if (item.Href.Contains("?"))
                        {
                            item.Href = item.Href.Substring(0, item.Href.IndexOf('?') + 1) + conditionQueryString;
                        }
                        else
                        {
                            item.Href = item.Href + "?" + conditionQueryString;
                        }
                    }
                }
            }
        }
    }
}