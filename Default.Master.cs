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

using System.Web.Services;
using System.IO;
using System.Text;
using NetRadio.Data;
using NetRadio.Web;
using NetRadio.Business;
using Ajax;
using NetRadio.Common.LocatingMonitor;
namespace NetRadio.LocatingMonitor
{
    /// <summary>
    /// 母版页
    /// </summary>
    public partial class __Default : BaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPageData();
            AjaxManager.RegisterClass(typeof(__Default), Page);
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(__Default), Page);
            Page.ClientScript.RegisterClientScriptInclude("cookieFun", WebPath.GetFullPath("Root.js"));


        }

        [AjaxMethod]
        public static bool LocatingServerIsOnline()
        {
            return LocatingServiceUtil.IsAvailable();
        }


        [AjaxMethod]
        //[AjaxPro.AjaxMethod]
        public static object GetTreeNodes()
        {
            NetRadio.Model.Menu[] ms = BusSystemMenu.GetSystemMenuFromCache(new __Default().me, true).ToArray();
            NetRadio.Model.TreeNode[] tns = new NetRadio.Model.TreeNode[ms.Count()];
            for (int i = 0; i < tns.Length; i++)
            {
                NetRadio.Model.Menu m = ms.ElementAt(i);
                tns[i] = new NetRadio.Model.TreeNode();
                tns[i].ID = m.ID;
                tns[i].PID = m.PID;
                tns[i].MenuText = "<b>" + m.MenuText + "</b>";
                tns[i].MenuUrl = getUrl(m.MenuUrl);//"#" == m.MenuUrl ? "#" : WebPath.GetFullPath(m.MenuUrl);
                tns[i].MenuImgSrc = WebPath.GetFullPath(m.MenuImgSrc);
                tns[i].SortNum = m.SortNum;
                tns[i].Target = m.Target;
            }
            NetRadio.Model.Menu[] msHotKeys = ms.Where(_d => _d.IsHotKey == true).Select(
                _d => new NetRadio.Model.Menu { MenuText = _d.MenuText, MenuUrl = getUrl(_d.MenuUrl), Target = _d.Target, ID = _d.ID, HotKeySortNum=_d.HotKeySortNum }).OrderBy(_d => _d.HotKeySortNum).ToArray();
            return new { TreeNodes = tns, HotKeys = msHotKeys };// tns;
        }
        static string getUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && url.Length >= 10 && url.ToLower().Substring(0, 10) == "javascript")
            {
                return url;
            }
            return "#" == url ? "#" : WebPath.GetFullPath(url);
        }
        /// <summary>
        /// 系统名称、标题栏、页面地图、用户名
        /// </summary>
        private void InitPageData()
        {
            sysName.InnerText = BusAppInfo.Name;
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
            labUserName.Text = me.Name;
        }
    }
}

