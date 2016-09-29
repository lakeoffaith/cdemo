using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using NetRadio.Data;
using System.Text;
using NetRadio.DataExtension;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
    public class __LatestEvents : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/xml";
            context.Response.Write(this.CreateOutputContent());
        }

        private string CreateOutputContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" standalone=\"yes\" ?>");
            sb.AppendLine("<LatestEvents>");

            using (AppDataContext db = new AppDataContext())
            {
                //var list = db.LatestEvents
                //            .Where(x => x.ResolveFlag != (byte)ResolveFlag.Processed)
                //            .Where(x => HostTag.All.Where(u => u.TagId != 0).Select(u => u.TagId).ToArray().Contains(x.TagId))
                //            .OrderByDescending(x => x.LastHappenTime)
                //            .ToList();

                var list = db.TagAlerts.Where(l => l.AlertStatus == (byte)AlertStatusType.New)
                    .Where(l => l.HostId > 0)
                    .OrderByDescending(l => l.WriteTime)
                    .ThenByDescending(l => l.AlertLevel)
                    //.OrderByDescending(l => l.AlertLevel)
                    .ToList();

                //lyz 获取用户的类型
                System.Collections.Generic.IEnumerable<int> hostidds = list.Select(u => u.HostId);
                System.Collections.Generic.IEnumerable<Data.HostGroup> hgs = db.HostGroups.Where(_d => hostidds.Contains(_d.HostId) && (_d.HostGroupId == 1 || _d.HostGroupId == 2)).AsEnumerable();

                var str = "	<item id='{0}' tagId='{1}' tagMac='{2}' tagName='{3}' coordinatesId='{4}' coodinatesName='{5}' eventType='{6}' eventDescription='{7}' lastHappenTime='{8}' hostGroupID='{9}' />\r\n";
                foreach (var i in list)
                {
                    //lyz 获取用户的类型
                    Data.HostGroup hg = hgs.Where(_d => _d.HostId == i.HostId).FirstOrDefault();

                    //20100106: select by host
                    //TagStatusView tagStatusView = TagStatusView.SelectTagStatus(i.TagId);
                    TagStatusView tagStatusView = TagStatusView.SelectTagStatusByHostId(i.HostId);
                    if (hg != null && tagStatusView != null)//判断对象是否存在 lyz
                    {
                        sb.AppendFormat(str, i.HostId, i.TagId, tagStatusView.Mac, tagStatusView.HostTag.HostName, i.CoordinatesId, Coordinates.GetName(i.CoordinatesId), i.AlertType, CommonExtension.GetEventDescription((SupportEvent)i.AlertType, i.HostId), i.WriteTime, hg.HostGroupId);
                    }
                }
            }

            sb.AppendLine("</LatestEvents>");
            return sb.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
