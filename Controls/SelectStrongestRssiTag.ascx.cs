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
using NetRadio.Data;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.LocatingService.RemotingEntry;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __SelectStrongestRssiTag : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__SelectStrongestRssiTag), Page);
            Ajax.AjaxManager.RegisterClass(typeof(__SelectTag), Page);
        }

        public int GetStrongestRssiTagID()
        {
            int tid = 0;
            if (!int.TryParse(Request.Form["hidTagID"].ToString(), out tid))
            {
                tid = 0;
            }
            return tid;
        }
        /// <summary>
        /// 是否启用绑定按钮
        /// </summary>
        public bool EnableOK
        {
            set;
            get;
        }
        /// <summary>
        /// 是否启用关闭按钮
        /// </summary>
        public bool EnableClose
        {
            set;
            get;
        }
        /// <summary>
        /// 是否启用解除按钮
        /// </summary>
        public bool EnableUnbind
        {
            set;
            get;
        }


        /// <summary>
        /// 是否启用边框的样式
        /// </summary>
        public bool EnableBorderCSS
        {
            set;
            get;
        }

        [Ajax.AjaxMethod]
        public object GetStrongestRssiTag()
        {
            string Mes = "";
            int TagID = 0;
            string TagMAC = "";
            
            int Error = 0;// 标签可用 = 0 ; 标签已经被绑定 =1 ; LocatingService不可用时 = 2  ; 没有扫描到可用的标签 = 3


            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                string mac = serviceApi.GetTagOnReception();
                if (mac == null)
                {
                    mac = "";
                }
                AppDataContext db = new AppDataContext();
                IQueryable<Tag> tags = db.Tags.Where(_d => _d.TagMac.ToLower().Trim() == mac.ToLower().Trim());
                if (tags.Count() == 1)
                {
                    Tag tag = tags.First();
                    IQueryable<HostTag> hostTags = db.HostTags.Where(_d => _d.TagId == tag.Id);
                    if (hostTags.Count() > 0)
                    {
                        Mes = string.Format("×当前信号最强的标签MAC地址是{0}，但已经被绑定，标签不可用。", tag.TagMac);
                        Error = 1;
                    }
                    else
                    {
                        Mes = string.Format("√当前信号最强的标签MAC地址是{0}，标签可用。", tag.TagMac);
                        TagID = tag.Id;
                        TagMAC = tag.TagMac;
                        Error = 0;
                    }
                }
                else
                {
                    Mes = "暂时没有扫描到可用的标签！";
                    Error = 3;
                }
            }
            else
            {
                Mes = "LocatingService不可用";
                Error = 2;
            }
            return new { Mes, TagID, TagMAC, Error};
        }

    }
}