using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __ReportNavigator : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string pid = Request.QueryString["pid"];
            if (!string.IsNullOrEmpty(pid))
            {
                NetRadio.Assistant.Web.Controls.TabViewItem item;
                System.Collections.Generic.IEnumerable<NetRadio.Model.Menu> menus = Business.BusSystemMenu.GetSystemMenuFromCache(NetRadio.Common.LocatingMonitor.ContextUser.Current, pid, true);
                ReportView.Items.Clear();
                string _url = "";
                for (int i = 0; i < menus.Count(); i++)
                {
                    item = new NetRadio.Assistant.Web.Controls.TabViewItem();
                    _url = menus.ElementAt(i).MenuUrl;
                    if (_url.IndexOf("pid=") == -1)
                    {
                        _url = _url.IndexOf("?") == -1 ? (_url + "?pid=" + pid) : (_url + "&pid=" + pid);
                    }
                    item.Href = "/" + _url;
                    item.Label = menus.ElementAt(i).MenuText;
                    ReportView.Items.Add(item);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            IdentifyTabViewSelectedIndex();
        }

        private void IdentifyTabViewSelectedIndex()
        {
            string fileName = Request.Path.Substring(Request.Path.LastIndexOf('/') + 1).ToLower();

            for (int i = 0; i < ReportView.Items.Count; i++)
            {
                var item = ReportView.Items[i];
                if (item.Href.Substring(item.Href.LastIndexOf('/') + 1).ToLower().StartsWith(fileName))
                {
                    ReportView.SelectedIndex = i;
                    break;
                }
            }


        }

        public void AppendSearchConditionQueryString(string conditionQueryString)
        {
            //if (conditionQueryString.Length != 0)
            //{
            //    foreach (var item in ReportView.Items)
            //    {
            //        if (item.Href.Length > 0)
            //        {
            //            if (item.Href.Contains("?"))
            //            {
            //                item.Href = item.Href.Substring(0, item.Href.IndexOf('?') + 1) + conditionQueryString;
            //            }
            //            else
            //            {
            //                item.Href = item.Href + "?" + conditionQueryString;
            //            }
            //        }
            //    }
            //}
        }

    }
}