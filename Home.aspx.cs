using System;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.Common;
using NetRadio.LocatingService.RemotingEntry;
using NetRadio.Web;
namespace NetRadio.LocatingMonitor
{
    [AjaxRegister]
    public partial class __Home : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearMenuSelectedStatus();

            //tabForPrison.Visible = false;
            //tabForDefault.Visible = false;
            //switch (NetRadio.Business.BusAppInfo.Theme)
            //{
            //    case "Prison":
            //        tabForPrison.Visible = true;
            //        break;
            //    case "Default":
            //    default:
            //        tabForDefault.Visible = true;
            //        break;
            //}
            if (!Page.IsPostBack)
            {
                Ajax.AjaxManager.RegisterClass(typeof(__Home));
            }
        }
        [Ajax.AjaxMethod]
        static public object GetHTML()
        {
            string id_people = "";
            string id_btn = "";
            string id_lowpower = "";
            string id_reset = "";
            string id_area = "";
            string id_ap = "";
            string id_goodap = "";

            DateTime dt = DateTime.Now;
            DateTime dt0 = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            DateTime dt1 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            using (AppDataContext db = new AppDataContext())
            {
                StringBuilder sb = new StringBuilder();
                TagAlert[] tas = db.TagAlerts.Where(_d => _d.WriteTime >= dt0 && _d.WriteTime <= dt1
                    && (_d.AlertType == (byte)SupportEvent.ButtonPressed || _d.AlertType == (byte)SupportEvent.BatteryReset || _d.AlertType == (byte)SupportEvent.BatteryInsufficient)).ToArray();

                id_btn = tas.Where(_d => _d.AlertType == (byte)SupportEvent.ButtonPressed).Count().ToString();
                id_lowpower = tas.Where(_d => _d.AlertType == (byte)SupportEvent.BatteryInsufficient).Count().ToString();
                id_reset = tas.Where(_d => _d.AlertType == (byte)SupportEvent.BatteryReset).Count().ToString();
                id_area = db.AreaEventLogs.Where(_d => _d.WriteTime >= dt0 && _d.WriteTime <= dt1).Count().ToString();

                id_ap = db.APs.Count().ToString();
                id_goodap = GetActiveAPTotalCount().ToString();


                var _data =
                     from _d in db.HostGroupInfos.AsEnumerable()
                     where _d.ParentGroupId == 0
                     select new
                     {
                         HostGroupId = _d.HostGroupId,
                         HostGroupName = _d.HostGroupName,
                         Count = db.HostTagGroupStatus.Count(x => x.HostGroupId == _d.HostGroupId),
                         ActiveCount = GetActiveCount(_d.HostGroupId, db)
                     };
                sb.Append(@"<table border=""0"" cellspacing=""1"" cellpadding=""0"" class=""tableBg0"">");
                int flag = 0;
                foreach (var order in _data)
                {
                    sb.AppendFormat(@"<tr class=""{3}"">
          <td width=""150"" height=""25"" align=""right"" >{0}&nbsp;&nbsp;总数：</td>
          <td width=""100"" class=""tdRed"">{1}</td>
          <td width=""200"" align=""right"">在线：</td>
          <td width=""100"" class=""tdRed"" >{2}</td>
          </tr>", order.HostGroupName, order.Count, order.ActiveCount, flag++ % 2 == 0 ? "tdBg1" : "tdBg2");
                }
                sb.Append(@" </table>");
                id_people = sb.ToString();
            }


            return new
            {
                id_people,
                id_btn,
                id_lowpower,
                id_reset,
                id_area,
                id_ap,
                id_goodap
            };
        }
        /// <summary>
        /// 得到在线部分
        /// </summary>
        /// <param name="HostGroupId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private static int GetActiveCount(int HostGroupId, AppDataContext db)
        {
            int count = 0;
            switch (HostGroupId)
            {
                case (int)TagUserType.Cop:
                    count = GetActiveTagTotalCount(HostGroupId);
                    break;
                case (int)TagUserType.Culprit:
                    count = db.HostPositionStatusViews.Count(x => x.HostGroupId == (int)TagUserType.Culprit
                        && x.CoordinatesId > 0);
                    break;
                default:
                    //TODO:疑问
                    count = GetActiveTagTotalCount(HostGroupId);
                    break;
            }
            return count;
        }


        [AjaxMethod]
        public static bool DetectLocatingService()
        {
            return LocatingServiceUtil.IsAvailable();
        }

        private static int GetActiveTagTotalCount(int groupId)
        {
            IList<TagStatusView> tagList = new List<TagStatusView>();
            string _keyword = "";
            int[] _hostGroupArray = new int[1];
            _hostGroupArray[0] = groupId;
            int totalCount = 0;
            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                if (serviceApi != null)
                {
                    TagStatusView tagStatusView = new TagStatusView();
                    tagList = serviceApi.SelectTagStatusList(
                    _keyword,
                    _hostGroupArray,
                    0,
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
                }
            }
            return totalCount;
        }

        private static int GetActiveAPTotalCount()
        {
            int totalCount = 0;
            if (LocatingServiceUtil.IsAvailable())
            {
                IServiceApi serviceApi = LocatingServiceUtil.Instance<IServiceApi>();
                APStatusView[] apStatus = serviceApi.SelectAPStatusList(
                                       null,
                                       null,
                                       "",
                                       SortDirection.Ascending,
                                       9999,
                                       0,
                                       out totalCount
                                   );
                totalCount = 0;
                if (apStatus != null && apStatus.Length > 0)
                {
                    for (int i = 0; i < apStatus.Length; i++)
                    {
                        if (apStatus[i].APLocatorStatus == (byte)APLocatorStatus.Success || apStatus[i].APLocatorStatus == (byte)APLocatorStatus.Running)
                            totalCount++;
                    }
                }
            }
            return totalCount;
        }
    }
}
