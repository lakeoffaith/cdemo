using System;
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
    public partial class __Home0 : BasePage
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
            if (!Page.IsPostBack)
            {
                this.LoadDefaultView();
            }
        }
        /// <summary>
        /// 得到在线部分
        /// </summary>
        /// <param name="HostGroupId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        private int GetActiveCount(int HostGroupId, AppDataContext db)
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
        private void LoadDefaultView()
        {
            using (AppDataContext db = new AppDataContext())
            { 
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
                RepGroup.DataSource = _data;
                RepGroup.DataBind();

                apCount.Value = db.APs.Count();
                //apFailedCount.Value = db.APStatus.Count(x => x.APLocatorStatus == 1);
                apFailedCount.Value = GetActiveAPTotalCount();

                newEventCount.Value = db.DBViewTagAlerts.Count(x => x.AlertStatus == (byte)AlertStatusType.New);
                //logCount.Value = db.MarshalTagLogs.Count();
                DateTime weekAgo = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
                logCount.Value = db.DBViewTagAlerts.Count(x => x.WriteTime > weekAgo);
                logFromDate.DisplayValue = weekAgo;

                DateTime TwoDays = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0));
                lowBatteryCount.Value = db.GenericEventLogs.Count(x => x.WriteTime > TwoDays && x.GenericEventType == (int)SupportEvent.BatteryInsufficient && x.HostId > 0);
                lowBatteryFromDate.DisplayValue = TwoDays;

               
            }
        }

        [AjaxMethod]
        public static bool DetectLocatingService()
        {
            return LocatingServiceUtil.IsAvailable();
        }

        private int GetActiveTagTotalCount(int groupId)
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

        private int GetActiveAPTotalCount()
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
