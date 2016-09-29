using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;

namespace NetRadio.LocatingMonitor.Objects
{
	public class MapFile : IHttpHandler
	{
		public void ProcessRequest(HttpContext context) {
			using (var image = CreateImage()) {
				image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				context.Response.ContentType = "Image/Jpeg";
				context.Response.End();
			}
		}

		public Image CreateImage() {
			string fileName = null;
			double eastWest = 0;
			double southNorth = 0;

			int id = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
			if (id > 0) {

				using (AppDataContext db = new AppDataContext()) {
					var map = db.FacilityMaps.Where(m => m.Id == id).SingleOrDefault();
					if (map != null) {
						fileName = map.MapFileName;
						eastWest = map.FacilityEastWest;
						southNorth = map.FacilitySouthNorth;
					}
				}
				if (!string.IsNullOrEmpty(fileName)) {
					string filePath = fileName;
					if (File.Exists(filePath)) {
						return Image.FromFile(filePath);
					}
				}
			}

			// Default Image.
			Bitmap image = new Bitmap(800, Convert.ToInt32(800 * (southNorth / eastWest)));
			using (Graphics g = Graphics.FromImage(image)) {
				g.Clear(Color.Snow);
				using (Brush brush = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
					using (Pen pen = new Pen(brush)) {
						g.DrawRectangle(pen, 0, 0, image.Width - 1, image.Height - 1);
					}
					g.DrawString("MAP NOT FOUND", new Font(FontFamily.GenericSansSerif, 50, FontStyle.Bold), brush, new PointF(100, 155));
					g.DrawString("地图文件未找到", new Font(FontFamily.GenericSansSerif, 40, FontStyle.Bold), brush, new PointF(165, 265));
				}
				return image;
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
