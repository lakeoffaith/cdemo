using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetRadio.LocatingMonitor
{
    /// <summary>
    /// __Pager类，lyz
    /// </summary>
    public partial class __Pager : NetRadio.Web.BaseUserControl
    {
      
        protected void Page_Load(object sender, EventArgs e)
        {
            int pageIndex = (int)GetDateItem(0);
            int recordCount = (int)GetDateItem(1);
            p.PageIndex = pageIndex;
            p.RecordCount = recordCount;
            pageResult.InnerHtml = "$" + p.RecordOffset + "," + p.PageSize + "$";
        }

        static public PageData GetPageData(int pageIndex, int recordeCount)
        {
            string r = NetRadio.Web.BaseUserControl.GetControlHTML<NetRadio.LocatingMonitor.__Pager>(Web.WebPath.GetFullPath("Controls/Pager.ascx"), pageIndex, recordeCount);
            int i1 = r.IndexOf('$');
            int i2 = r.IndexOf('$', i1 + 1);
            string data = r.Substring(i1 + 1, i2 - i1 - 1);
            string[] datas = data.Split(new char[] { ',' });
            int _recordOffset = Convert.ToInt32(datas[0]);
            int _pageSize = Convert.ToInt32(datas[1]);
            return new PageData { PageSize = _pageSize, RecordOffset = _recordOffset, HtmlCode = r };
        }
    }
    public struct PageData
    {
        public int RecordOffset;
        public int PageSize;
        public string HtmlCode;
    }

}