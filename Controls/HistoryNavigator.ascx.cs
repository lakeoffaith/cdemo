using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Data;
using NetRadio.DataExtension;
 
namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __HistoryNavigator : UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string pid = Request.QueryString["pid"];
            string userType = Request.QueryString["userType"];
            if (!string.IsNullOrEmpty(pid))
            {
                NetRadio.Assistant.Web.Controls.TabViewItem item;
                System.Collections.Generic.IEnumerable<NetRadio.Model.Menu> menus = Business.BusSystemMenu.GetSystemMenuFromCache(NetRadio.Common.LocatingMonitor.ContextUser.Current, pid, true);
                tabView.Items.Clear();
                string _url = "";
                for (int i = 0; i < menus.Count(); i++)
                {
                    item = new NetRadio.Assistant.Web.Controls.TabViewItem();
                    _url = menus.ElementAt(i).MenuUrl;
                    if (_url.IndexOf("pid=") == -1)
                    {
                        _url = _url.IndexOf("?") == -1 ? (_url + "?pid=" + pid) : (_url + "&pid=" + pid);
                    }
                    if (_url.IndexOf("userType=") == -1)
                    {
                        _url = _url.IndexOf("?") == -1 ? (_url + "?userType=" + userType) : (_url + "&userType=" + userType);
                    }
                    item.Href = "/" + _url;
                    item.Label = menus.ElementAt(i).MenuText;
                    tabView.Items.Add(item);
                }
            }
            string masterFile = Request.QueryString["masterFile"];
            if (!string.IsNullOrEmpty(masterFile))
            {
                NetRadio.Assistant.Web.Controls.TabViewItem _ctl = null;
                foreach (NetRadio.Assistant.Web.Controls.TabViewItem ctl in tabView.Items)
                {
                    if (ctl.GetType() == typeof(NetRadio.Assistant.Web.Controls.TabViewItem))
                    {
                        _ctl = (NetRadio.Assistant.Web.Controls.TabViewItem)ctl;
                        _ctl.Href = _ctl.Href + (_ctl.Href.IndexOf("?") == -1 ? "?masterFile=" + masterFile : "&masterFile=" + masterFile);
                    }
                }
            }
        }
         
       
        protected void Page_Load(object sender, EventArgs e)
        { 
           
            TagUserType userType = (TagUserType)Fetch.QueryUrlAsIntegerOrDefault("userType", 0);

            /*if (userType == TagUserType.Cop)
            {
                var item = tabView.Items[2];
                    item.Visible = true;
                    item = tabView.Items[3];
                    item.Visible = false;
            }

            else if (userType == TagUserType.Culprit)
            {
                var item = tabView.Items[2];
                item.Visible = false;
                item = tabView.Items[3];
                item.Visible = true;
            }
            else
            {
                var item = tabView.Items[2];
                item.Visible = true;
                item = tabView.Items[3];
                item.Visible = false;
            }*/

           
            IdentifyTabViewSelectedIndex();
        }

        private void IdentifyTabViewSelectedIndex()
        {
            //string fileName = Request.Path.Substring(Request.Path.LastIndexOf('/') + 1).ToLower();
            string fileName = Request.Path.ToLower();

            for (int i = 0; i < tabView.Items.Count; i++)
            {
                var item = tabView.Items[i];
                //if (item.Href.ToLower().StartsWith(fileName))
                //if (item.Href.Substring(item.Href.LastIndexOf('/') + 1).ToLower().StartsWith(fileName))
                if (item.Href.ToLower().StartsWith(fileName))
                {
                    tabView.SelectedIndex = i;
                    break;
                }
            }

            //if (Config.Settings.ProjectType == ProjectTypeEnum.NMPrison)
            //{
            //    var item = tabView.Items[2];
            //    item.Visible = false;
            //}
            //else if (Config.Settings.ProjectType == ProjectTypeEnum.YZPrison)
            //{
            //    foreach (var item in tabView.Items)
            //    {
            //        List<string> YZLabels = new List<string>();
            //        YZLabels.Add("告警记录");
            //        YZLabels.Add("位置记录");


            //        if (!YZLabels.Contains(item.Label.Trim()))
            //        {
            //            item.Visible = false;
            //        }
            //    }
            //}
        }

        public void AppendSearchConditionQueryString(string conditionQueryString)
        {
            //if (conditionQueryString.Length != 0)
            //{
            //    foreach (var item in tabView.Items)
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