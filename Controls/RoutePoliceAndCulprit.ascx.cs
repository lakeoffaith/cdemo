using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using System.Text;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __RoutePoliceAndCulprit : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__RoutePoliceAndCulprit));
        }

        struct NameID
        {
            public string Name;
            public int ID;
        }


        [Ajax.AjaxMethod]
        public static object GetData()
        {
            NameID[] police = null;
            NameID[] culprit = null;
            string positionList = "";
            StringBuilder sb = new StringBuilder();
            using (AppDataContext db = new AppDataContext())
            {
                var qPolice = from _d in db.HostTags
                              join _d2 in db.HostGroups on _d.HostId equals _d2.HostId
                              where _d2.HostGroupId == 1
                              select _d;
                var qCulprit = from _d in db.HostTags
                               join _d2 in db.HostGroups on _d.HostId equals _d2.HostId
                               where _d2.HostGroupId == 2
                               select _d;
                police = qPolice.Select(_p => new NameID { Name = _p.HostName, ID = _p.HostId }).ToArray();
                culprit = qCulprit.Select(_p => new NameID { Name = _p.HostName, ID = _p.HostId }).ToArray();
                Data.Coordinates[] cs = db.Coordinates.Where(_d => _d.CoordinatesUsage == 1).ToArray();
                foreach (Coordinates c in cs)
                {
                    sb.AppendFormat(@"<option value=""{0}"">{1}</option>", c.Id, c.CoordinatesName);
                }
                positionList = sb.ToString();
            }
           // positionList = NetRadio.DataExtension.GoupCoordinate.GetArrayPointsByIdandPos(culprit[0].ID,positionList);

            return new { police = police, culprit = culprit, positionList = positionList };
        }

        [Ajax.AjaxMethod]
        public string UpdateData(int policeID, int culpritID, string CIDs)
        {
            GoupCoordinate a = new GoupCoordinate();
            a.ArraginID = policeID;
            a.PrisonerID = culpritID;
            a.PointArray = GoupCoordinate.GetArrayPointsByIdandPos(culpritID, Convert.ToInt32(CIDs));
            /////////
            //HostTag host = HostTag.GetById(culpritID);
            //int roomId = CulpritRoomReference.GetRoomIdByCulpritId(culpritID);//根据hostID获取犯人所在的监舍的ID
            //IList<MapAreaCoverage> tempCoordinates = MapAreaCoverage.SelectByAreaId(roomId);
            //int stPoint = Coordinates.GetParentIdById(tempCoordinates[0].CoordinatesId);
            //int endPoint = Convert.ToInt32(CIDs);
            //IList<NeighborRoute> all = NeighborRoute.All;
            //NeighborRoute neiroute = new NeighborRoute();
            //List<int> listPoint = new List<int>();
            //List<int> listNeighPoint = new List<int>();
            //neiroute = all.SingleOrDefault(b => b.EndPoint1 == stPoint && b.EndPoint2 == endPoint);
            //if (neiroute != null)
            //{
            //    listPoint.Add(stPoint);

            //    string[] arr = neiroute.PointArray.ToString().Split(',');
            //    foreach (string item in arr)
            //    {
            //        listPoint.Add(Convert.ToInt32(item));
            //    }

            //}

            //for (int i = 0; i < listPoint.Count; i++)
            //{
            //    foreach (MapRoute t in FindoutNeighPoint(listPoint[i]))
            //    {
            //        if (!listNeighPoint.Contains(t.Endpoint1) && !listPoint.Contains(t.Endpoint1))
            //        {
            //            listNeighPoint.Add(t.Endpoint1);
            //        }
            //        if (!listNeighPoint.Contains(t.Endpoint2) && !listPoint.Contains(t.Endpoint2))
            //        {
            //            listNeighPoint.Add(t.Endpoint2);
            //        }
            //    }
            //}
            //string arr1 = "";
            //string arr2 = "";
            //int x = 0;
            //foreach (int p in listPoint)
            //{
            //    if (x == 0) arr1 = p.ToString();
            //    else
            //        arr1 += "," + p.ToString();
            //    x++;
            //}
            //int j = 0;
            //foreach (int p in listNeighPoint)
            //{
            //    if (j == 0) arr2 = p.ToString();
            //    else
            //        arr2 += "," + p.ToString();
            //    j++;
            //}
            ///////////////////
            //a.PointArray = arr1 + ',' + arr2;
            a.GoupUseful = 2;
            a.Interval = "";
            a.IsDisabled = true;
            GoupCoordinate.Insert(a);

            return "policeID=" + policeID + " culpritID=" + culpritID + " CIDs=" + CIDs;
        }
        //private static List<MapRoute> FindoutNeighPoint(int x)
        //{
        //    IList<Coordinates> listCoordinates = Coordinates.All;
        //    IList<MapRoute> listMapRoute = MapRoute.All;
            
        //    List<MapRoute> NearRoute = new List<MapRoute>();
        //    NearRoute = listMapRoute.Where(a => a.Endpoint1 == x|| a.Endpoint2 == x).ToList();
        //    return NearRoute;


        //}

    }
}