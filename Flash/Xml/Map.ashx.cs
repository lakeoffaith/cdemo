using System.Web;
using System.Linq;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using System.Text;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __Map : IHttpHandler
	{
		public __Map() {
			_id = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
		}

		int _id;

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
			sb.AppendLine("<map>");

			if (_id > 0) {
				using (AppDataContext db = new AppDataContext()) {
					var map = db.FacilityMaps.SingleOrDefault(m => m.Id == _id);
					if (map != null) {
						sb.AppendLine("	<id>" + map.Id + "</id>");
						sb.AppendLine("	<mapName>" + Facility.GetNameByMapId(map.Id) + "</mapName>");
						sb.AppendLine("	<facilityId>" + Facility.GetFacilityIdByMapId(map.Id) + "</facilityId>");
						sb.AppendLine("	<eastWest>" + map.FacilityEastWest + "</eastWest>");
						sb.AppendLine("	<southNorth>" + map.FacilitySouthNorth + "</southNorth>");
						sb.AppendLine("	<mapFile>" + PathUtil.ResolveUrl("Objects/MapFile.ashx?id=" + map.Id) + "</mapFile>");
						sb.AppendLine("	<mapAreaFile>" + PathUtil.ResolveUrl("Flash/xml/MapArea.ashx?mapId=" + map.Id) + "</mapAreaFile>");
						sb.AppendLine("	<mapAPFile>" + PathUtil.ResolveUrl("Flash/xml/AP.ashx?mapId=" + map.Id) + "</mapAPFile>");
						sb.AppendLine("	<mapRouteFile>" + PathUtil.ResolveUrl("Flash/xml/Route.ashx?mapId=" + map.Id) + "</mapRouteFile>");
					}
				}
			}

			sb.AppendLine("</map>");
			return sb.ToString();
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
