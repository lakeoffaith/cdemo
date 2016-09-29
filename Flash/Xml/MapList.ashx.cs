using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using System.Text;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __MapList : IHttpHandler
	{

		public __MapList() {
		}

		public void ProcessRequest(HttpContext context) {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache"; 
            context.Response.ContentType = "text/plain";
			context.Response.Write(this.CreateOutputContent());
		}

		private string CreateOutputContent() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" standalone=\"yes\" ?>");
			sb.AppendLine("<mapList>");

			var facilities = Facility.All.OrderBy(x=>x.ParentFacilityId);
			var format = "	<map facilityId=\"{0}\" parentFacilityId=\"{1}\" mapId=\"{2}\" name=\"{3}\" />";
			foreach (var f in facilities) {
				sb.AppendFormat(format, f.Id, f.ParentFacilityId, f.MapId, f.FacilityName);
				sb.AppendLine();
			}

			sb.AppendLine("</mapList>");
			return sb.ToString();
		}


		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
