using System.Web;
using System.Linq;
using System.Collections.Generic;
using NetRadio.Assistant.Web.Util;
using NetRadio.Data;
using System.Text;
using NetRadio.Common.LocatingMonitor;
using NetRadio.LocatingService.RemotingEntry;
using System.Web.UI.WebControls;
using NetRadio.DataExtension;

namespace NetRadio.LocatingMonitor.Flash.Xml
{
	public class __TagCoordinates : IHttpHandler
	{
		public __TagCoordinates() {
			_mapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);
			_keyword = Fetch.QueryUrl("keyword");
            _hostGroupArray = Strings.ParseToArray<int>(Fetch.QueryUrl("taggroups"));
			_eventTypes = Strings.ParseToArray<int>(Fetch.QueryUrl("eventtypes"));
			_pageSize = Fetch.QueryUrlAsIntegerOrDefault("pagesize", 10);
			_currentPage = Fetch.QueryUrlAsIntegerOrDefault("currentpage", 1);
            _tagId = Fetch.QueryUrlAsIntegerOrDefault("tagid", 0);
		}

		int _mapId;
		string _keyword;
        int[] _hostGroupArray;
		int[] _eventTypes;
		int _pageSize;
		int _currentPage;
        int _tagId;

		public void ProcessRequest(HttpContext context) {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            context.Response.Expires = 0;
            context.Response.CacheControl = "no-cache"; 
			context.Response.ContentType = "text/xml";
			context.Response.Write(this.CreateOutputContent());
		}

		int _coordinatesId = -1;
        private string CreateOutputContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" standalone=\"yes\" ?>");
            sb.AppendLine("<tagCoordinates>");

            if (!LocatingServiceUtil.IsAvailable())
            {
                sb.AppendLine("</tagCoordinates>");
                return sb.ToString();
            }

            IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
            TagStatusView tagStatusView = new TagStatusView();

            if (_tagId > 0)
            {
                tagStatusView = serviceApi.SelectTagStatus(_tagId);

                if (tagStatusView != null && _mapId > 0 && tagStatusView.TagId > 0)
                {
                    _mapId = tagStatusView.MapId;
                }

                if (tagStatusView.WorkingStatus != TagWorkingStatus.Locating)
                {
                    _tagId=0;
                    _mapId = Fetch.QueryUrlAsIntegerOrDefault("mapId", -1);
                }
            }
            int totalCount = 0;
            IList<TagStatusView> tagList = new List<TagStatusView>();

            if (_hostGroupArray ==null || _hostGroupArray.Length == 0)
            {
                _hostGroupArray = new int[] { 1, 2 };
            }

            if (_keyword.Length > 0 && !_keyword.Contains(','))
            {
                _mapId = 0;
                tagList = serviceApi.SelectTagStatusListByKeywords(
                    _keyword,
                        _hostGroupArray,
                        _mapId,
                        true,
                        _eventTypes.Contains((int)SupportEvent.Absent),
                        _eventTypes.Contains((int)SupportEvent.BatteryInsufficient),
                        _eventTypes.Contains((int)SupportEvent.AreaEvent),
                        _eventTypes.Contains((int)SupportEvent.ButtonPressed),
                        _eventTypes.Contains((int)SupportEvent.WristletBroken),
                        "Position",
                        SortDirection.Ascending,
                        _pageSize,
                        _pageSize * (_currentPage - 1),
                        out totalCount
                    ).ToList();
                tagList = tagList.Where(x => x.X > 0 && !x.HostTag.HostGroupId.Contains((int)TagUserType.Position)).OrderBy(x => x.X).ToList();
                //totalCount = tagList.Count;
            }
            else
            {
                tagList = serviceApi.SelectTagStatusList(
                        _keyword,
                        _hostGroupArray,
                        _mapId,
                        true,
                        _eventTypes.Contains((int)SupportEvent.Absent),
                        _eventTypes.Contains((int)SupportEvent.BatteryInsufficient),
                        _eventTypes.Contains((int)SupportEvent.AreaEvent),
                        _eventTypes.Contains((int)SupportEvent.ButtonPressed),
                        _eventTypes.Contains((int)SupportEvent.WristletBroken),
                        "Position",
                        SortDirection.Ascending,
                        _pageSize,
                        _pageSize * (_currentPage - 1),
                        out totalCount
                    ).ToList();
                tagList = tagList.Where(x => x.X > 0).OrderBy(x => x.X).ToList();
                //totalCount = tagList.Count;
            }

            int tracingItemIndex = -1;

            if (tagStatusView != null && _tagId > 0)
            {
                for (int i = 0; i < tagList.Count(); i++)
                {
                    if (tagList[i].Mac == tagStatusView.Mac)
                    {
                        tracingItemIndex = i;
                    }
                }
            }

            //IList<TagStatusView> tagStatusList = tagList.ToList();
            if (tracingItemIndex != 0 && _tagId > 0)
            {
                if (tracingItemIndex > 0)
                {
                    tagList.RemoveAt(tracingItemIndex);
                }
                tagList.Insert(0, tagStatusView);
            }

            sb.AppendLine("<pagination>");
            sb.AppendFormat("<recordCount>{0}</recordCount>\r\n", totalCount);
            sb.AppendFormat("<pageSize>{0}</pageSize>\r\n", _pageSize);
            sb.AppendFormat("<currentPage>{0}</currentPage>\r\n", _currentPage);
            sb.AppendLine("</pagination>");

            var coordinatesFormat = "<coordinates id=\"{0}\" mapId=\"{1}\" mapName=\"{2}\" coordinatesName=\"{3}\" x=\"{4}\" y=\"{5}\">";
            var tagFormat = "<tag id=\"{0}\" tagName=\"{1}\" warningTypes=\"{2}\" warningLevel=\"{3}\" moveSpeed=\"{4}\" icon=\"{5}\" updateTime=\"{6}\"  groupIds=\"{7}\" />";
            foreach (var item in tagList)
            {
                if (_coordinatesId != item.CoordinatesId)
                {
                    if (_coordinatesId >= 0)
                    {
                        sb.AppendLine("</coordinates>");
                    }
                    sb.AppendFormat(coordinatesFormat, item.CoordinatesId, item.MapId, Facility.GetNameByMapId(item.MapId), item.CoordinatesName, item.X, item.Y);
                    _coordinatesId = item.CoordinatesId;
                }

                List<string> warningTypes = new List<string>();
                if (item.AreaEventStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.AreaEvent).ToString());
                }
                if (item.AbsenceStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.Absent).ToString());
                }
                if (item.BatteryInsufficientStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.BatteryInsufficient).ToString());
                }
                if (item.BatteryResetStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.BatteryReset).ToString());
                }
                if (item.ButtonPressedStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.ButtonPressed).ToString());
                }
                if (item.WristletBrokenStatus == EventStatus.Occurring)
                {
                    warningTypes.Add(((int)SupportEvent.WristletBroken).ToString());
                }

                string hostGroupIds = "";
                if (item.HostTag.HostGroupId.Length > 0)
                {
                    foreach (int gId in item.HostTag.HostGroupId)
                    {
                        if (gId > 0)
                        {
                            if (hostGroupIds == "")
                                hostGroupIds += gId.ToString();
                            else
                                hostGroupIds += "," + gId.ToString();
                        }
                    }
                }
                string hostName = item.HostTag.HostName;
                if (hostName == "") hostName = item.TagName;
                sb.AppendFormat(tagFormat, item.TagId, hostName, string.Join(",", warningTypes.ToArray()), warningTypes.ToArray().Length > 0 ? "3" : "1", "2", CommonExtension.IdentityIconByGroupId(item.HostTag.HostGroupId), item.PositionUpdateTime.ToString("yyyy/M/d H:mm:ss").Replace('-', '/'),
                    hostGroupIds/*item.HostTag.HostGroupId.Contains(1) ? 1 : 2*/);
            }

            if (_coordinatesId >= 0)
            {
                sb.AppendLine("</coordinates>");
            }

            sb.AppendLine("</tagCoordinates>");
            return sb.ToString();
        }

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
