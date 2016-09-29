using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __Route : IHttpHandler
	{
		public __Route() {
			_mapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);
		}

		int _mapId;

		public void ProcessRequest(HttpContext context) {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache"; 
			context.Response.ContentType = "text/xml";
			context.Response.Write(this.CreateOutputContent());
		}

		private string CreateOutputContent() {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("<?xml version=\"1.0\" standalone=\"yes\" ?>");
			sb.AppendLine("<routeList>");

			if (_mapId > 0) {
				var list = this.SelectItemList();
				var format = "<route name=\"{0}\" endpoint1=\"{1}\" endpoint2=\"{2}\" totalDistance=\"{3}\" coordinatesArray=\"{4}\" />";
				foreach (var r in list) {
					sb.AppendFormat(format, " ", r.EndPoint1, r.EndPoint2, r.TotalDistance, r.CoordinatesArray);
					sb.AppendLine();
				}
			}

			sb.AppendLine("</routeList>");
			return sb.ToString();
		}

        private IList<NeighborRoute> SelectItemList()
        {
			using (AppDataContext db = new AppDataContext()) {
				var query = db.NeighborRoutes.Where(r => r.MapId == _mapId);
				return query.ToList();
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
