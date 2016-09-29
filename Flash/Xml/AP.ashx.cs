using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __AP : IHttpHandler
	{
		public __AP() {
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
			sb.AppendLine("<apList>");

			if (_mapId > 0) {
				var list = this.SelectItemList();
				var format = "	<ap id=\"{0}\" name=\"{1}\" x=\"{2}\" y=\"{3}\" />";
				foreach (var ap in list) {
					sb.AppendFormat(format, ap.Id, ap.Name, ap.X, ap.Y);
					sb.AppendLine();
				}
			}

			sb.AppendLine("</apList>");
			return sb.ToString();
		}

		private IList<APNode> SelectItemList() {
			using (AppDataContext db = new AppDataContext()) {
				var query = from c in db.Coordinates
							from a in db.APs
							where c.Id == a.CoordinatesId
							where c.MapId == _mapId
							where c.CoordinatesUsage == (byte)CoordinatesUsage.APPoint
							select new APNode {
								Id = a.Id,
								Name = a.APName,
								X = c.X,
								Y = c.Y
							};
				return query.ToList();
			}
		}

		struct APNode
		{
			internal int Id {
				get;
				set;
			}
			internal string Name {
				get;
				set;
			}
			internal double X {
				get;
				set;
			}
			internal double Y {
				get;
				set;
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
