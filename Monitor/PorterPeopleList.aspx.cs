using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.Text;
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __PorterPeopleList : BasePage
    {
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
            Ajax.AjaxManager.RegisterClass(typeof(__PorterPeopleList));
        }

        public override void DemandLogin()
        {
        }

        protected override bool CheckRight()
        {
            //return base.CheckRight();
            return true;
        }


        private string GetList(int groupID)
        {
            if (groupID == 1 || groupID == 2)
            {
                if (LocatingServiceUtil.IsAvailable())
                {
                    IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                    TagStatusView[] tagsView = serviceApi.GetTagOnSentry();

                    if (tagsView == null)
                    {
                        return "";
                    }
                    var q = tagsView.Where(_d => _d != null && _d.HostTag.HostGroupId.Contains(groupID)).Select(_d => new { HostId = _d.HostTag.HostId, HostName = _d.HostTag.HostName, Mac = _d.Mac, HostPhotoPath = "../TagUsers/UserPhoto.ashx?id=" + _d.HostTag.HostId });

                    StringBuilder sb = new StringBuilder("");
                    foreach (var log in q)
                    {
                        sb.AppendFormat(@" 
                                    <td width=4></td>
                                    <td width=100 align=center valign=top><img src={0} width=100 height=120 /><br/><br/> 姓名：{1}<br/> 标签：{2}</td>
                                    ", log.HostPhotoPath, log.HostName, log.Mac);
                    }
                    if (sb.Length > 0)
                    {
                        sb.Insert(0, "<table border=0 cellspacing=0 cellpadding=0><tr>");
                        sb.Append("</tr></table>");
                    }
                    return sb.ToString();
                }
            }
            return "";
        }

        [Ajax.AjaxMethod]
        public string GetPoliceList()
        {
            return GetList(1);
        }

        [Ajax.AjaxMethod]
        public string GetCulpritList()
        {
            return GetList(2);
        }
    }
}








//TagStatusView tsv0 = serviceApi.SelectTagStatus(926);
//                    TagStatusView tsv1 = serviceApi.SelectTagStatus(769);
//                    TagStatusView tsv2 = serviceApi.SelectTagStatus(734);
//                    TagStatusView tsv3 = serviceApi.SelectTagStatus(735);
//                    TagStatusView tsv4 = serviceApi.SelectTagStatus(731);
//                    TagStatusView tsv5 = serviceApi.SelectTagStatus(730);
//                    TagStatusView[] tagsView = new TagStatusView[] { tsv0, tsv1, tsv2, tsv3, tsv4, tsv5 };


//                    var q = tagsView.Where(_d => _d.HostTag.HostGroupId.Contains(1)).Select(_d => new { HostId = _d.HostTag.HostId, HostName = _d.HostTag.HostName, Mac = _d.Mac });
//                    int[] hostIDs = q.Select(_d => _d.HostId).ToArray();
//                    using (AppDataContext db = new AppDataContext())
//                    {
//                        var qHost = db.HostTags.Where(hostIDs.Contains(_d => _d.HostId));
//                        var order = from _d in q
//                                    join _d2 in qHost
//                                    on _d.HostId equals _d2.HostId
//                                    select new 
//                                    {
//                                        HostId = _d.HostId,
//                                        HostName = _d.HostName,
//                                        Mac = _d.Mac,
//                                        HostPhotoPath = _d2.ImagePath
//                                    };
//                        return order.ToArray();

//                        StringBuilder sb = new StringBuilder();
//                        foreach (var log in order)
//                        {
//                            sb.AppendFormat(@"                                
//                                  <tr>
//                                    <td width=100 height=120 rowspan=3 align=center valign=middle><img src={0} /></td>
//                                    <td>姓名：{1}</td>
//                                  </tr>
//                                  <tr>
//                                    <td>MAC：{2}</td>
//                                  </tr>
//                                  <tr>
//                                    <td>&nbsp;</td>
//                                  </tr>
//                                  <tr>
//                                    <td colspan=4 height=6></td>
//                                  </tr>
//                                ", log.HostPhotoPath,log.HostName,log.Mac);                        
//                        }
//                        if (sb.Length > 0)

//                        {
//                            sb.Insert(0, "<table width=100% border=0 cellspacing=0 cellpadding=0>");
//                            sb.Append("</table>");
//                        }
//                        return sb.ToString();
//                    }