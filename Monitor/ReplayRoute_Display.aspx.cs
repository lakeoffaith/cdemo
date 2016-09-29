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
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
using Summer;
using NetRadio.Model;
using NetRadio.Business;
namespace NetRadio.LocatingMonitor.Monitor
{
    public partial class __ReplayRoute_Display : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__ReplayRoute_Display));
            //object pp = GetCoordinatesVedio(2, "2010-03-23 15:11:14.903", 999);

            xmlPath.Text = "../Flash/Xml/ReplayRoute.ashx?" + Request.QueryString.ToString();
            showPlayerMarker.Visible = Fetch.QueryUrlAsIntegerOrDefault("minutes", -1) > 0;
            videoDefault.Visible = false;
            videoYangZhou.Visible = false;

            switch (BusSystemConfig.GetVedioType())
            {
                case 0:
                default:
                    ddVedioTip.Visible = false;
                    ddVedio.Visible = false;
                    videoDefault.Visible = true;
                    break;
                case 1:
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Vedio", "Vedio();", true);
                    ddVedioTip.Visible = true;
                    ddVedio.Visible = true;
                    if (Business.BusSystemConfig.GetOnlyShowVideo())
                    {
                        videoYangZhou.Visible = true;
                    }
                    else
                    {
                        videoDefault.Visible = true;
                    }
                    break;
                case 2:
                    ddVedioTip.Visible = false;
                    ddVedio.Visible = false;
                    videoDefault.Visible = true;
                    break;
            }


        }
        [Ajax.AjaxMethod]
        public object GetCoordinatesVedio(int userId, string dateTime, int addMinutes)
        {

            DateTime dt_begin = Convert.ToDateTime(dateTime);
            DateTime dt_end = dt_begin.AddMinutes(addMinutes);
            int addSeconds = addMinutes * 60;
            SelectBatch<history_TagPositionLog> spl = new SelectBatch<history_TagPositionLog>();
            spl.Where.EqualTo(history_TagPositionLog.__HostId, userId).Between(history_TagPositionLog.__WriteTime, dt_begin, dt_end);
            IOrderedEnumerable<history_TagPositionLog> epl = spl.AsEntityCollection().OrderBy(_d => _d.WriteTime);



            ///////////2010-5-26
            SelectBatch<history_TagPositionLog> spl0 = new SelectBatch<history_TagPositionLog>();
            spl0.Where.EqualTo(history_TagPositionLog.__HostId, userId).LessThan(history_TagPositionLog.__WriteTime, dt_begin);
            spl0.Top = 1;
            history_TagPositionLog ht = spl0.AsEntity();
            history_TagPositionLog h1 = null;
            history_TagPositionLog h2 = null;
            if (ht != null)
            {
                h1 = new history_TagPositionLog();
                h1.CoordinatesId = ht.CoordinatesId;
                h1.HostId = ht.HostId;
                h1.Id = ht.Id;
                h1.MapId = ht.MapId;
                h1.TagId = ht.TagId;
                h1.WriteTime = dt_begin;///////////
                h1.X = ht.X;
                h1.Y = ht.Y;
                h1.Z = ht.Z;
                h2 = new history_TagPositionLog();
                h2.CoordinatesId = -1;////表示最终点
                h2.HostId = ht.HostId;
                h2.Id = ht.Id;
                h2.MapId = ht.MapId;
                h2.TagId = ht.TagId;
                h2.WriteTime = dt_end;////////
                h2.X = ht.X;
                h2.Y = ht.Y;
                h2.Z = ht.Z;
            }
            else
            {
                h1 = null;
                h2 = null;
                throw new Exception("未能发现历史轨迹");
            }

            if (epl.Count() == 0)
            {
                epl = new EntityCollection<history_TagPositionLog>(new history_TagPositionLog[2] { h1, h2 }).OrderBy(_d => _d.WriteTime);
            }
            else
            {
                history_TagPositionLog fLog = epl.First();
                history_TagPositionLog lLog = epl.Last();
                EntityCollection<history_TagPositionLog> aa = new EntityCollection<history_TagPositionLog>(epl.ToArray());
                if (h1.WriteTime < fLog.WriteTime)
                {
                    aa.Add(h1);
                }
                if (h2.WriteTime > lLog.WriteTime)
                {
                    aa.Add(h2);
                }
                epl = aa.OrderBy(_d => _d.WriteTime);
            }

            ///////////end-------2010-5-26

            EntityCollection<history_TagPositionLog> epl0 = new EntityCollection<history_TagPositionLog>();
            int cid = 0;
            history_TagPositionLog tpl = null;
            //int __minuteLength = 0;
            int __secondLength = 0;
            foreach (history_TagPositionLog order in epl)
            {
                if (cid != order.CoordinatesId)
                {
                    epl0.Add(order);
                    if (tpl != null)
                    {
                        //tpl.PropertyEX = order.WriteTime.Subtract(tpl.WriteTime).TotalMinutes;
                        //__minuteLength += Convert.ToInt32(tpl.PropertyEX);
                        tpl.PropertyEX = order.WriteTime.Subtract(tpl.WriteTime).TotalSeconds;
                        __secondLength += Convert.ToInt32(tpl.PropertyEX);
                    }
                    tpl = order;
                    cid = order.CoordinatesId;
                }

            }
            if (tpl != null)
            {
                //tpl.PropertyEX = addMinutes - __minuteLength;
                tpl.PropertyEX = addSeconds - __secondLength;

            }
            object[] CoordinatesIDs = epl0.Select(_d => (object)_d.CoordinatesId).Distinct().ToArray();
            SelectBatch<plugin_CoordinatesCamera> cc = new SelectBatch<plugin_CoordinatesCamera>();
            cc.Where.EqualTo(plugin_CoordinatesCamera.__CoordinatesId, CoordinatesIDs);
            EntityCollection<plugin_CoordinatesCamera> ecc = cc.AsEntityCollection();
            var q = from _d in epl0
                    join _d2 in ecc on _d.CoordinatesId equals _d2.CoordinatesId into g
                    from g0 in g.DefaultIfEmpty()
                    select new
                    {
                        CoordinatesId = _d.CoordinatesId,
                        CoordinatesHasVedio = g0 != null,
                        BeginPlayTime = _d.WriteTime,
                        //EndPlayTime = _d.WriteTime.AddMinutes(Convert.ToDouble(_d.PropertyEX)),
                        EndPlayTime = _d.WriteTime.AddSeconds(Convert.ToDouble(_d.PropertyEX)),
                        //PlayMinuteLength = Convert.ToInt32(_d.PropertyEX),
                        PlaySecondLength = Convert.ToInt32(_d.PropertyEX),
                        CameraIP = g0 != null ? g0.CameraIP : "",
                        iChannel = g0 != null ? g0.iChannel : 0,
                        CameraIPlref = g0 != null ? g0.CameraIPlref : "",
                        iChannelref = g0 != null ? g0.iChannelref : 0
                    };
            return q.ToArray();
        }
    }
}
