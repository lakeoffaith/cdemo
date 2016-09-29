using System;
using System.Linq;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using System.Collections.Generic;
using NetRadio.LocatingMonitor.Controls;

namespace NetRadio.LocatingMonitor
{
    public class LocatingMonitorUtils
    {
        public static string GetHostName(string tagName, string tagMac, HostTagView host)
        {
            string hostName = tagName;
            if (host != null && host.HostName != "")
                hostName = host.HostName;
            if (hostName == "") hostName = tagMac;
            return hostName;
        }

        public static string GetHostName(string tagName, string hostName)
        {
            if (hostName == "") hostName = tagName;
            return hostName;
        }

        public static string GetHostName(string tagName, string tagMac, string hostName)
        {
            string macStr = "";
            if (tagMac.Length > 9) macStr = tagMac.Substring(9);
            else if (tagMac.Length > 0) macStr = tagMac;
            else macStr = tagName;
            string hostStr = string.Format("{0}({1})", hostName, macStr);
            return hostStr;
        }


        public static string GetWorkingStatusString(TagWorkingStatus WorkingStatus)
        {
            string strStatus = "";
            switch (WorkingStatus)
            {
                default:
                case TagWorkingStatus.Normal:
                    strStatus = "常规";
                    break;
                case TagWorkingStatus.Locating:
                    strStatus = "定位";
                    break;
                case TagWorkingStatus.Survey:
                    strStatus = "采样";
                    break;
                case TagWorkingStatus.Stopped:
                    strStatus = "常规";
                    break;
            }
            return strStatus;
        }

        public static string GetAllTagEventsDescription(TagStatusView status, int userType, int pid, string masterFile)
        {
            IList<string> arr = new List<string>();

            if (status.AbsenceStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/AbsenceLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "消失"));
            }
            if (status.AreaEventStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/AreaEventLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "出入区域"));
            }
            if (status.BatteryInsufficientStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/LowBatteryLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "电量不足"));
            }
            if (status.BatteryResetStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/BatteryResetLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "换电池"));
            }
            if (status.ButtonPressedStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/ButtonPressedLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "触动按钮"));
            }
            if (status.WristletBrokenStatus == EventStatus.Occurring)
            {
                arr.Add(string.Format("<a href=\"" + Web.WebPath.GetFullPath("History/WristletBrokenLog.aspx?userType={0}&pid={1}&masterFile={2}") + "\">{3}</a>", userType, pid, masterFile, "断开腕带"));
            }
            return string.Join("<span class='separator'> | </span>", arr.ToArray());
        }

        public static string GetAllTagEventsDescription(TagStatusView status)
        {
            IList<string> arr = new List<string>();

            if (status.AbsenceStatus == EventStatus.Occurring)
            {
                arr.Add("消失");
            }
            if (status.AreaEventStatus == EventStatus.Occurring)
            {
                arr.Add("出入区域");
            }
            if (status.BatteryInsufficientStatus == EventStatus.Occurring)
            {
                arr.Add("电量不足");
            }
            if (status.BatteryResetStatus == EventStatus.Occurring)
            {
                arr.Add("换电池");
            }
            if (status.ButtonPressedStatus == EventStatus.Occurring)
            {
                arr.Add("触动按钮");
            }
            if (status.WristletBrokenStatus == EventStatus.Occurring)
            {
                arr.Add("断开腕带");
            }
            return string.Join("<span class='separator'> | </span>", arr.ToArray());
        }

        public static string GetEventDescription(SupportEvent eventType)
        {
            switch (eventType)
            {
                default:
                    return "未知事件类型";
                case SupportEvent.Absent:
                    return "消失";
                case SupportEvent.AreaEvent:
                    return "区域告警";
                case SupportEvent.BatteryInsufficient:
                    return "电量不足";
                case SupportEvent.BatteryReset:
                    return "电池重置";
                case SupportEvent.ButtonPressed:
                    return "触动按钮";
                case SupportEvent.PositionChanged:
                    return "位置改变";
                case SupportEvent.WristletBroken:
                    return "腕带断开";
            }
            return "";
        }

        public static string GetScanModeDescription(ScanMode ScanMode)
        {
            string modeStr = "";
            switch (ScanMode)
            {
                default:
                case ScanMode.TagScanAP:
                    modeStr = "标签扫描AP";
                    break;
                case ScanMode.APScanTag:
                    modeStr = "AP扫描标签";
                    break;
            }
            return modeStr;
        }

        public static string GetLocatingModeDescription(LocatingMode LocatingMode)
        {
            string modeStr = "";
            switch (LocatingMode)
            {
                default:
                case LocatingMode.BySurvey:
                    modeStr = "距离";
                    break;
                case LocatingMode.ByDelta:
                    modeStr = "三角定位";
                    break;
            }
            return modeStr;
        }

        public static string JoinIntArray(int[] aIds)
        {
            string zId = "";
            if (aIds != null && aIds.Length > 0)
            {
                for (int i = 0; i < aIds.Length; i++)
                {
                    if (i == 0)
                        zId = aIds[i].ToString();
                    else
                        zId += "," + aIds[i].ToString();
                }
            }
            return zId;
        }

        public static string ConstructSQLWhere(__TagLogFilter tagLogFilter)
        {
            string strWhere = "";
            if (tagLogFilter.TagNameKeyword.Length > 0)
            {
                string zLike = " (HostName like '%" + tagLogFilter.TagNameKeyword + "%' or " +
                                "TagName like '%" + tagLogFilter.TagNameKeyword + "%')";
                strWhere += (strWhere.Trim() == "") ? zLike : " and " + zLike;
            }

            //yyang,2010-01-29暂时取消按组及地图查询功能查询功能
            //所属组
            //if (tagLogFilter.SelectedGroupIdArray != null)
            //{
            //    var range = HostTagGroupStatus.GetCoveredTagIdArray(tagLogFilter.SelectedGroupIdArray);
            //    string zTagIds = LocatingMonitorUtils.JoinIntArray(range);
            //    if (zTagIds != null && zTagIds != "")
            //    {
            //        string Instr = "TagId in (" + zTagIds + ")";
            //        strWhere += (strWhere.Trim() == "") ? Instr : " and " + Instr;
            //    }
            //}
            //地图
            if (tagLogFilter.FacilityFilterRowVisible && tagLogFilter.MapId > 0)
            {
                string zCondition = "MapID='" + tagLogFilter.MapId + "'";
                strWhere += (strWhere.Trim() == "") ? zCondition : " and " + zCondition;
            }
            //时间段
            if (tagLogFilter.FromTime.ToString().Trim() != "")
            {
                string zTime = "writetime >='" + tagLogFilter.FromTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strWhere += (strWhere.Trim() == "") ? zTime : " and " + zTime;
            }

            if (tagLogFilter.ToTime.ToString().Trim() != "")
            {
                string zTime = "writetime<='" + tagLogFilter.ToTime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                strWhere += (strWhere.Trim() == "") ? zTime : " and " + zTime;
            }
            return strWhere;
        }
    }
}