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
    public partial class __RoutePolice : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__RoutePolice));
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

            string positionList = "";
            StringBuilder sb = new StringBuilder();
            using (AppDataContext db = new AppDataContext())
            {

                var qPolice = from _d in db.HostTags
                        join _d2 in db.HostGroups on _d.HostId equals _d2.HostId
                        where _d2.HostGroupId == 1
                        select _d;
                police = qPolice.Select(_p => new NameID { Name = _p.HostName, ID = _p.HostId }).ToArray();

                Data.Coordinates[] cs = db.Coordinates.Where(_d => _d.CoordinatesUsage == 1).ToArray();
                foreach (Coordinates c in cs)
                {
                    sb.AppendFormat(@"<option value=""{0}"">{1}</option>", c.Id, c.CoordinatesName);
                }
                positionList = sb.ToString();
            }

            return new { police = police, positionList = positionList };
        }

        [Ajax.AjaxMethod]
        public string UpdateData(string policeIDs, string CIDs)
        {
            int[] pids = Strings.ParseToArray<int>(policeIDs, new char[] { ',' });
            for (int i = 0; i < pids.Length; i++)
            {
                GoupCoordinate a = new GoupCoordinate();
                a.ArraginID = pids[i];
                a.PointArray = CIDs;
                a.GoupUseful = 1;
                a.Interval = "";
                a.IsDisabled = true;
                GoupCoordinate.Insert(a);
            }
            return "policeIDs=" + policeIDs + " CIDs=" + CIDs;
        }

    }
}