using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Drawing;
using System.Drawing.Imaging;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using NetRadio.DataExtension;
using System.IO;
using System.Data.SqlClient;

namespace NetRadio.YangzhouJail.Web.TagUsers
{
	public class __UserPhoto : IHttpHandler
	{
		public void ProcessRequest(HttpContext context) {
			using (var image = CreateImage()) {
				image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
				context.Response.ContentType = "Image/Jpeg";
				context.Response.End();
			}
		}

		public Image CreateImage() {
			string filePath = null;

			int id = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
			if (id > 0) {
                string HostExternalId = "";
                bool isPolice = false;
                using (AppDataContext db = new AppDataContext())
                {
                    try
                    {
                        var user = db.HostTagGroupStatus.Where(u => u.ParentGroupId == 0).SingleOrDefault(u => u.HostId == id);
                        if (user != null && !string.IsNullOrEmpty(user.ImagePath))
                        {
                            filePath = Fetch.MapPath(PathUtil.ResolveUrl(user.ImagePath));
                        }

                        if (user.HostGroupId == 1)
                        {
                            isPolice = true;
                        }
                        else
                        {
                            isPolice = false;
                        }
                        HostExternalId = user.HostExternalId;

                    }
                    catch
                    {

                    }
                }

				if (!string.IsNullOrEmpty(filePath)) {
					if (File.Exists(filePath)) {
						return Image.FromFile(filePath);
					}
				}

                if (Config.Settings.IsLoadHostInfo)
                {
                    string strConnect = System.Configuration.ConfigurationSettings.AppSettings["FXConnectionString"].ToString();
                    SqlConnection conn = new SqlConnection(strConnect);

                    string strSQL = "";
                    if (isPolice)
                    {
                        strSQL = "SELECT top 1 A.ZP AS PIC from MJZPB AS A JOIN MJJBXXB AS B ON A.MJBH=B.MJBH where B.JH='" + HostExternalId + "' order by ZYBH desc";
                    }
                    else
                    {
                        strSQL = "SELECT top 1 PIC from ZPB where RYBH='" + HostExternalId + "' order by ZYBH";
                    }


                    DataSet ds = new DataSet();//   创建一个   DataSet
                    conn.Open();
                    SqlDataAdapter command = new SqlDataAdapter(strSQL, conn);//   用   SqlDataAdapter   得到一个数据集   
                    command.Fill(ds, "InterrogationInfo");//把Dataset绑定到数据表   
                    DataTable dt = ds.Tables["InterrogationInfo"];

                    if (dt.Rows.Count>0 && dt.Rows[0]["PIC"] != null && dt.Rows[0]["PIC"].ToString().Length > 0)
                    {
                        MemoryStream ms = new MemoryStream((byte[])dt.Rows[0]["PIC"]);
                        return Image.FromStream(ms);
                    }
                }

			}

			// Default Image.
			int w = Fetch.QueryUrlAsIntegerOrDefault("w", 100);
			int h = Fetch.QueryUrlAsIntegerOrDefault("h", 120);

			Bitmap image = new Bitmap(w, h);
			using (Graphics g = Graphics.FromImage(image)) {
				g.Clear(Color.Snow);
				using (Brush brush = new SolidBrush(Color.FromArgb(150, Color.LightGray))) {
					using (Pen pen = new Pen(brush)) {
						g.DrawRectangle(pen, 0, 0, image.Width - 1, image.Height - 1);
					}
					g.DrawString("无", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold), brush, new PointF(15, 8));
					g.DrawString("照", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold), brush, new PointF(15, 23));
					g.DrawString("片", new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold), brush, new PointF(15, 38));
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
