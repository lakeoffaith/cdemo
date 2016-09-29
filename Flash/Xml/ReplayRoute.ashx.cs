using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using System.Collections.Generic;
using System.Text;
using NetRadio.DataExtension;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __ReplayRoute : IHttpHandler
	{
		public void ProcessRequest(HttpContext context) {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache"; 
            int mapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", 0);
			int[] hostIdArray = Strings.ParseToArray<int>(Fetch.QueryUrl("tagId"));
			int requestSeconds = Fetch.QueryUrlAsInteger("minutes");

			int seconds = requestSeconds * 60;
			DateTime startTime;
			if (requestSeconds < 0) {
				seconds = Math.Abs(requestSeconds);
				startTime = DateTime.Now.AddSeconds(-seconds);
			}
			else {
				startTime = DateTime.Parse(Fetch.QueryUrl("startTime"));
			}


			Facility facility;
			FacilityMap facilityMap;
			HostTag[] tags;

            using (AppDataContext db = new AppDataContext())
            {
                tags = db.HostTags.Where(h => hostIdArray.Contains(h.HostId)).ToArray();
                if (tags.Length == 0)
                {
                    return;
                }
            }


			StringBuilder sb = new StringBuilder();

			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
			sb.AppendLine("<routeDisplay>");
            sb.AppendLine("	<title>" + (requestSeconds < 0 ? "标签实时定位" : "标签历史轨迹") + "</title>");
            sb.AppendLine("	<loop>" + (requestSeconds < 0 ? "True" : "False") + "</loop>");
            sb.AppendLine("	<timeInfo>");
            sb.AppendLine("		<fromTime>" + startTime.ToString("yyyy/M/d H:mm:ss").Replace("-", "/") + "</fromTime>");
            sb.AppendLine("		<toTime>" + startTime.AddSeconds(seconds+1).ToString("yyyy/M/d H:mm:ss").Replace("-", "/") + "</toTime>");
            sb.AppendLine("		<timeZone>GMT" + (Fetch.ServerUtcOffset > 0 ? "+" : "-") + Math.Abs(Fetch.ServerUtcOffset) + "</timeZone>");
            sb.AppendLine("	</timeInfo>");
            DateTime endTime;

            if (mapId > 0 && hostIdArray.Length >= 1)
            {
                facility = Facility.All.SingleOrDefault(f => f.MapId == mapId);
                if (facility == null)
                {
                    return;
                }
                facilityMap = FacilityMap.SelectFacilityMapById(mapId);
                if (facilityMap == null)
                {
                    return;
                }

                endTime = startTime.AddSeconds(seconds+1);
                sb.Append(this.GetFacilityInnerXml(facility, facilityMap, endTime));

                foreach (var tag in tags)
                {
                    IList<TagPositionLog> tracks = this.GetPositionList(tag.HostId, startTime, seconds).Where(t => t.MapId == mapId).ToList();
                    if (tracks.Count == 0)
                    {
                        continue;
                    }
                    sb.AppendLine("	<tag id=\"" + tag.HostId + "\" name=\"" + tag.HostName + "\" moveSpeed=\"0.5\" icon=\"" + CommonExtension.IdentityIcon(tag.TagId) + "\">");
                    sb.Append(this.GetRouteInnerXml(tracks));
                    sb.AppendLine("	</tag>");
                }

                sb.AppendLine("	</routeDisplaymap>");

            }
            else
            {
                IList<TagPositionLog> tracksall = this.GetPositionList(tags[0].HostId, startTime, seconds);
                IList<TagPositionLog> tracks = new List<TagPositionLog>();
                string iconFile = CommonExtension.IdentityIcon(tags[0].TagId);

                for (int i = 0; i < tracksall.Count; i++)
                {
                    if (tracksall[i].MapId == 0)
                        continue;

                    if (mapId != tracksall[i].MapId)
                    {
                        if (tracks.Count > 0)
                        {
                            if (i + 1 == tracksall.Count)
                            {
                                endTime = startTime.AddSeconds(seconds);
                            }
                            else
                            {
                                endTime = tracksall[i].WriteTime;
                            }
                            sb.AppendLine(SetRouteDisplayMap(mapId, startTime, endTime, 
                                tags[0].HostId, tags[0].HostName, 0.5, iconFile, tracks));
                        }
                        mapId = tracksall[i].MapId;

                        tracks = new List<TagPositionLog>();
                        tracks.Add(tracksall[i]);
                    }
                    else
                    {
                        tracks.Add(tracksall[i]);

                        if (i + 1 == tracksall.Count)
                        {
                            endTime = startTime.AddSeconds(seconds+1);
                            sb.AppendLine(SetRouteDisplayMap(mapId, startTime, endTime, 
                                tags[0].HostId, tags[0].HostName, 0.5, iconFile, tracks));
                        }
                    }
                }

                if (tracks.Count == 1)
                {
                    endTime = startTime.AddSeconds(seconds+1);
                    sb.AppendLine(SetRouteDisplayMap(mapId, startTime, endTime, 
                        tags[0].HostId, tags[0].HostName, 0.5, iconFile, tracks));
                }
            }
            
			sb.AppendLine("</routeDisplay>");
            
			context.Response.ContentType = "text/xml";
			context.Response.Write(sb.ToString());

		}

        private string SetRouteDisplayMap(int mapId, DateTime startTime, DateTime endTime, int hostId, string hostName, double moveSpeed, string iconFile, IList<TagPositionLog> tracks)
        {
            StringBuilder sb = new StringBuilder();
            Facility facility = Facility.All.SingleOrDefault(f => f.MapId == mapId);
            FacilityMap facilityMap = FacilityMap.SelectFacilityMapById(mapId);
            if (facility != null && facilityMap != null)
            {
                sb.Append(this.GetFacilityInnerXml(facility, facilityMap, endTime));
                sb.AppendLine(string.Format("	<tag id=\"{0}\" name=\"{1}\" moveSpeed=\"{2}\" icon=\"{3}\">", hostId, hostName, moveSpeed.ToString("#.#"), iconFile));
                
                //sb.AppendLine("	<tag id=\"" + tags[0].HostId + "\" name=\"" + tags[0].HostName + "\" moveSpeed=\"0.5\" icon=\"../images/person.gif\">");
                sb.Append(this.GetRouteInnerXml(tracks));
                sb.AppendLine("	</tag>");
                sb.AppendLine("	</routeDisplaymap>");
            }
            return sb.ToString();
        }

		private IList<MapArea> GetAreaList(int mapId) {
			return MapArea.All.Where(a => a.MapId == mapId).ToList();
		}

		private IList<TagPositionLog> GetPositionList(int hostId, DateTime startTime, int seconds) {
			using (AppDataContext db = new AppDataContext()) {
				var tracks = db.TagPositionLogs
					.Where(t =>
                        t.HostId == hostId &&
						t.WriteTime >= startTime &&
						t.WriteTime <= startTime.AddSeconds(seconds+1) &&
						t.X > 0 && t.Y > 0)
					.OrderBy(t => t.Id)
					.ToList();

				var single = db.TagPositionLogs
					.OrderByDescending(t => t.Id)
                    .FirstOrDefault(t => t.HostId == hostId && t.Id < (tracks.Count == 0 ? int.MaxValue : tracks[0].Id));

                if (single != null)
                {
                    tracks.Insert(0, single);
                }
                else
                {
                    TagPositionLog aLog = new TagPositionLog();
                    aLog.HostId = hostId;
                    aLog.X = -1;
                    aLog.Y = -1;
                    aLog.Z = -1;
                    aLog.WriteTime = startTime;
                    if (tracks != null && tracks.Count > 0)
                    {
                        aLog.MapId = tracks.First().MapId;
                        aLog.TagId = tracks.First().TagId;
                    }
                    tracks.Insert(0, aLog);
                }
				return tracks;
			}
		}

		private string GetAreaInnerXml(IList<MapArea> areas) {
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < areas.Count; i++) {
                sb.AppendFormat("			<area name=\"{0}\" coordsArray=\"{1}\" />\r\n", areas[i].AreaName, areas[i].CoordinatesArray);
			}
			return sb.ToString();
		}

        private string GetFacilityInnerXml(Facility facility, FacilityMap facilityMap, DateTime endTime)
        {
            StringBuilder sb = new StringBuilder();
            IList<MapArea> areas;
            
            areas = this.GetAreaList(facility.MapId);
            sb.AppendLine("	<routeDisplaymap>");
            sb.AppendLine("	<facilityInfo>");
            sb.AppendLine("		<facilityName>" + facility.FacilityName + "</facilityName>");
            sb.AppendLine("		<facilityEastWest>" + facilityMap.FacilityEastWest + "</facilityEastWest>");
            sb.AppendLine("		<facilityNorthSouth>" + facilityMap.FacilitySouthNorth + "</facilityNorthSouth>");
            sb.AppendLine("		<facilityMap>" + WebConfig.BasePath + "/Objects/MapFile.ashx?id=" + facilityMap.Id + "</facilityMap>");
            sb.AppendLine("		<facilityEndTime>" + endTime.ToString("yyyy/M/d H:mm:ss").Replace("-", "/") + "</facilityEndTime>");
            sb.AppendLine("		<facilityArea>");
            sb.Append(this.GetAreaInnerXml(areas));
            sb.AppendLine("		</facilityArea>");
            sb.AppendLine("	</facilityInfo>");

            return sb.ToString();
        }

		private string GetRouteInnerXml(IList<TagPositionLog> tracks) {
			StringBuilder sb = new StringBuilder();

			if (tracks.Count == 1) {
				sb.AppendLine("		<route endTime=\"" + tracks[0].WriteTime.ToString("yyyy/M/d H:mm:ss").Replace("-", "/") + "\">");
				sb.AppendFormat("			<point name=\"{0}\" x=\"{1}\" y=\"{2}\" />\r\n", "", tracks[0].X, tracks[0].Y);
				sb.AppendFormat("			<point name=\"{0}\" x=\"{1}\" y=\"{2}\" />\r\n", "", tracks[0].X, tracks[0].Y);
				sb.AppendLine("		</route>");
				return sb.ToString();
			}

			using (AppDataContext db = new AppDataContext()) {
				for (int i = 0; i < tracks.Count - 1; i++) {

                    //2010-01-11: if there are two tracks, it should have one route. so comment out temporally
                    //if (tracks[i].X == tracks[i + 1].X && tracks[i].Y == tracks[i + 1].Y) {
                    //        continue;
                    //}

					sb.AppendLine("		<route endTime=\"" + tracks[i + 1].WriteTime.ToString("yyyy/M/d H:mm:ss").Replace("-", "/") + "\">");

					int location1 = Math.Min(tracks[i].CoordinatesId, tracks[i + 1].CoordinatesId);
					int location2 = Math.Max(tracks[i].CoordinatesId, tracks[i + 1].CoordinatesId);

					//MapRoute path = db.MapRoutes.SingleOrDefault(p => p.Endpoint1 == location1 && p.Endpoint2 == location2);
                    MapRoute path = RouteDistance.GetRoutePoints(location1, location2);
					if (path == null) {
						sb.AppendFormat("			<point name=\"{0}\" x=\"{1}\" y=\"{2}\" />\r\n", "", tracks[i].X, tracks[i].Y);
						sb.AppendFormat("			<point name=\"{0}\" x=\"{1}\" y=\"{2}\" />\r\n", "", tracks[i + 1].X, tracks[i + 1].Y);
					}
					else {
						string[] arr = path.CoordinatesArray.Split('|');
						if (location1 == tracks[i + 1].CoordinatesId) {
							Array.Reverse(arr);
						}
						for (int j = 0; j < arr.Length; j++) {
							var xy = arr[j].Split(',');
							sb.AppendFormat("			<point name=\"{0}\" x=\"{1}\" y=\"{2}\" />\r\n", "", xy[0], xy[1]);
						}
					}
					sb.AppendLine("		</route>");
				}
				return sb.ToString();
			}
		}

		struct Point
		{
			internal Point(double x, double y) {
				_x = x;
				_y = y;
			}

			double _x, _y;
			internal double X {
				get {
					return _x;
				}
				set {
					_x = value;
				}
			}
			internal double Y {
				get {
					return _y;
				}
				set {
					_y = value;
				}
			}

			public override int GetHashCode() {
				return base.GetHashCode();
			}

			public override bool Equals(object obj) {
				Point p = (Point)obj;
				return p.X == this.X && p.Y == this.Y;
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
