using System.Web;
using System.Linq;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using System.Text;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    public class HostGroup : IHttpHandler
    {

        public HostGroup()
        {
		}

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
			sb.AppendLine("<hostGroup>");

			var hostGroup = HostGroupInfo.All;
			var format = "	<group groupId=\"{0}\" parentGroupId=\"{1}\" groupName=\"{2}\" />";
			foreach (var f in hostGroup) {
				sb.AppendFormat(format, f.HostGroupId, f.ParentGroupId, f.HostGroupName);
				sb.AppendLine();
			}

            sb.AppendLine("</hostGroup>");
			return sb.ToString();
		}


		public bool IsReusable {
			get {
				return false;
			}
		}
    }
}
