using System.Web;
using System.Linq;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using System.Text;
using Summer;
using System.Data;
namespace NetRadio.LocatingMonitor.Flash.Xml
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>    
    public class Camera : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/xml";
            context.Response.Write(CreateContent(int.Parse(context.Request.QueryString["mapid"])));
        }
        private string CreateContent(int mapid)
        {
            StringBuilder sb = new StringBuilder();
            string parameter = "";
            sb.Append("<?xml version=\"1.0\" standalone=\"yes\" ?>");
            sb.Append("<Coordinates>");

            DataTable dt = null;
            switch (NetRadio.Business.BusSystemConfig.GetVedioType())
            {
                case 0:
                default:
                    break;
                case 1:
                    dt = Summer.Query.RunQuerySQLString(
                       string.Format(@"
                        select a.id, a.mapid,a.CoordinatesName, a.x,a.y,
                        case  when b.CameraIP is NULL then 0 
                        else 1 end 
                        as HasCamera ,b.CameraIP,b.iChannel,b.CameraIPlref,b.iChannelref,
                        NULL as CameraIDs , NULL as IP,NULL as Port,NULL as [User],NULL as Pwd
                         from (select * from map_Coordinates  where CoordinatesUsage=1 and mapid={0})a
                        left join
                        plugin_CoordinatesCamera b on(a.id=b.CoordinatesId)
                        ", mapid)
               , "LocatingMonitor");
                    break;
                case 2:
                    dt = Summer.Query.RunQuerySQLString(
                           string.Format(@"
                            select a.id, a.mapid,a.CoordinatesName, a.x,a.y,
                            case  when b.CoordinateId is NULL then 0 
                            else 1 end 
                            as HasCamera,NULL as CameraIP,NULL as iChannel,NULL as CameraIPlref,NULL as iChannelref 
                             ,b.CameraID as CameraIDs ,b.IP,b.Port,b.[User],b.Pwd
                            from (select * from map_Coordinates  where CoordinatesUsage=1 and mapid={0})a
                            left join
                            plugin_AnchorCamera b on(a.id=b.CoordinateId)
                            ", mapid)
               , "LocatingMonitor");
                    break;
            }

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    parameter = dr["CameraIP"] + "," + dr["iChannel"] + "," + dr["CameraIPlref"] + "," + dr["iChannelref"] + "," + dr["CameraIDs"] + "," + dr["IP"] + "," + dr["User"] + "," + dr["Pwd"] + "," + dr["Port"];
                    sb.AppendFormat("<coordinate id=\"{0}\" mapId=\"{1}\" coordinatesName=\"{2}\" x=\"{3}\" y=\"{4}\" HasCamera=\"{5}\" CameraIP=\"{6}\" iChannel=\"{7}\" CameraIPlref=\"{8}\" iChannelref=\"{9}\"  CameraIDs=\"{10}\" IP=\"{11}\"  User=\"{12}\"  Pwd=\"{13}\" Port=\"{14}\" Parameter=\"{15}\">", dr["id"], dr["mapid"], dr["CoordinatesName"], dr["x"], dr["y"], dr["HasCamera"], dr["CameraIP"], dr["iChannel"], dr["CameraIPlref"], dr["iChannelref"], dr["CameraIDs"], dr["IP"], dr["User"], dr["Pwd"], dr["Port"], parameter);
                    sb.Append("</coordinate>");
                }
            }
            sb.Append("</Coordinates>");
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
