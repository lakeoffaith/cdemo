using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using NetRadio.Common.LocatingMonitor;
using NetRadio.LocatingService.RemotingEntry;
using System.Web.UI.WebControls;
using NetRadio.DataExtension;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __MapArea : IHttpHandler
	{
		public __MapArea() {
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
			sb.AppendLine("<areaList>");

			if (_mapId > 0) {
				var list = this.SelectItemList();
                var format = "	<area id=\"{0}\" name=\"{1}\" coordinatesArray=\"{2}\" quota=\"{3}\" currentCount=\"{4}\" mapId=\"{5}\" />";
				foreach (var a in list) {
                    try
                    {
                        if (a.LinkedMapId > 0)
                        {
                            IList<TagStatusView> tagList = new List<TagStatusView>();
                            string _keyword = "";
                            int[] _hostGroupArray = new int[2] { 1, 2 };//2010-11-bydyp
                            int totalCount = 0;

                            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                            if (serviceApi != null)
                            {
                                TagStatusView tagStatusView = new TagStatusView();
                                tagList = serviceApi.SelectTagStatusList(
                                _keyword,
                                _hostGroupArray,
                                a.LinkedMapId,
                                true,
                                false, //SupportEvent.Absent),
                                false, //SupportEvent.BatteryInsufficient),
                                false, //SupportEvent.AreaEvent),
                                false, //SupportEvent.ButtonPressed),
                                false, //SupportEvent.WristletBroken),
                                "",
                                SortDirection.Ascending,
                                0,//only get total count
                                0,
                                out totalCount);

                                sb.AppendFormat(format, a.Id, a.AreaName, a.CoordinatesArray, 0, totalCount, a.LinkedMapId);
                                sb.AppendLine();
                            }
                        }
                        else
                        {
                            int quota = 0;

                            try
                            {
                                quota = CulpritRoomReference.All.Count(x => x.JailRoomId == a.Id);
                            }
                            catch { }

                            int currentCount = 0;
                            var coordinates = MapAreaCoverage.All.Where(x => x.AreaId == a.Id).Select(x => x.CoordinatesId).ToArray();
                           
                            foreach (var item in FullTagStatusView)
                            {
                                if (coordinates.Contains(item.CoordinatesId) &&
                                   
                                    item.AbsenceStatus != EventStatus.Occurring &&
                                    item.X>0)
                                {
                                    currentCount++;
                                }
                            }

                            sb.AppendFormat(format, a.Id, a.AreaName, a.CoordinatesArray, quota, currentCount, -1);
                            sb.AppendLine();
                        }
                    }
                    catch
                    {

                    }
				}
			}

			sb.AppendLine("</areaList>");
			return sb.ToString();
		}

		private IList<MapArea> SelectItemList() {
			return MapArea.All.Where(x => x.MapId == _mapId).ToList();
		}

		public bool IsReusable {
			get {
				return false;
			}
		}

		TagStatusView[] _fullTagStatusView;
		TagStatusView[] FullTagStatusView {
			get {
                try
                {
                    if (_fullTagStatusView == null)
                    {
                        int totalCount;
                        _fullTagStatusView = LocatingServiceUtil.Instance<IServiceApi>().SelectTagStatusList(
                            "",
                            new int[0],
                            0,
                            true,
                            false,
                            false,
                            false,
                            false,
                            false,
                            "TagName",
                            SortDirection.Ascending,
                            9999999,
                            0,
                            out totalCount
                        ).Where(t=>t.HostTag.HostGroupId.Contains(1)).ToArray();//2010-11-25by dyp
                        
                    }
				}
                catch
                {

                }
				return _fullTagStatusView;
			}
		}
	}
}
