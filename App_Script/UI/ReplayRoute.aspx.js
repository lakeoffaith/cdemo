function View() {
    var f = document.forms[0];
    if (!/^\d{4}\-\d{1,2}\-\d{1,2}$/.test($("fromDate").value)) {
        alert("请输入或选择开始时间点的“日期”部分");
        return false;
    }
    if ($("fromMinute").value.trim().length == 0 || !/^\d{1,2}$/.test($("fromMinute").value) && parseInt($("fromMinute").value) > 59) {
        alert("请输入开始时间点的“分钟”部分");
        return false;
    }

    if (!/^\d{4}\-\d{1,2}\-\d{1,2}$/.test($("toDate").value)) {
        alert("请输入或选择结束时间点的“日期”部分");
        return false;
    }
    if ($("toMinute").value.trim().length == 0 || !/^\d{1,2}$/.test($("toMinute").value) && parseInt($("toMinute").value) > 59) {
        alert("请输入结束时间点的“分钟”部分");
        return false;
    }
//    if (isNaN($("continueTime").value)) {
//        alert("请输入要播放多少分钟");
//        return false;
//    }
//    if ($("continueTime").value <= 0) {
//        alert("持续播放时间(分钟)不能小于或等于0");
//        return false;
//    }
    //	if (f.facilityMap.value == "0") {
    //		alert("请选择一个有地图的公司/场地/楼层");
    //		return false;
    //	}
    if ($(ID_selectedUserIds).value == "") {
        alert("至少选定一个使用者");
        return false;
    }

    if (document.getElementsByName(namePreString + "facilityMap").item(0).value == "0" && $(ID_selectedUserIds).value.indexOf(",") > -1) {
        alert("选择多个使用者时，必须选择地图");
        return false;
    }




    var startTime = $("fromDate").value + " " + parseInt($("fromHour").value) + ":" + parseInt($("fromMinute").value) + ":00";
    var endTime = $("toDate").value + " " + parseInt($("toHour").value) + ":" + parseInt($("toMinute").value) + ":00";

    NetRadio.LocatingMonitor.Monitor.__ReplayRoute.IsFutureTime(startTime, endTime,
	                                                 function(r) {
	                                                     if (r.error == 0) {
	                                                         var startTime = $("fromDate").value + " " + $("fromHour").value + ":" + $("fromMinute").value + ":" + "00";
	                                                         var continueTime = r.value;
	                                                         viewTagPositionTrack(document.getElementsByName(namePreString + "facilityMap").item(0).value, $(ID_selectedUserIds).value, startTime, continueTime);
	                                                     }
	                                                     else {
	                                                         alert(r.errorText);
	                                                     }
	                                                 }
	);

    //    NetRadio.LocatingMonitor.Monitor.__ReplayRoute.IsFutureTime($("fromDate").value.toString(),
    //                                                     parseInt($("fromHour").value),
    //	                                                 parseInt($("fromMinute").value),
    //	                                                 0,
    //	                                                 parseInt($("continueTime").value),
    //	                                                 function(r) {
    //	                                                     if (r.error == 0) {
    //	                                                         if (r.value) {
    //	                                                             alert("您选择的时间范围包含了将来时间，请重新选择");
    //	                                                         }
    //	                                                         else {
    //	                                                             var startTime = $("fromDate").value + " " + $("fromHour").value + ":" + $("fromMinute").value + ":" + "00";
    //	                                                             viewTagPositionTrack(document.getElementsByName(namePreString + "facilityMap").item(0).value, $(ID_selectedUserIds).value, startTime, $("continueTime").value);
    //	                                                         }
    //	                                                     }
    //	                                                     else {
    //	                                                         alert(r.errorText);
    //	                                                     }
    //	                                                 }
    //	);

    return true;

}

function viewTagPositionTrack(mapId, userIds, startTime, minutes) {
    //debugger;
    //var userId=0;
    //var findex=userIds.toString().indexOf(",",0);
    //if(findex==-1)
    //{
    //  userId=userIds;
    //}
    //else
    //{
    // userId=userIds.toString().substring(0,findex);
    //}
    //window.open(baseURL + "/Monitor/ReplayRoute_Display.aspx?mapId=" + mapId + "&tagId=" + tagIds + "&startTime=" + startTime + "&minutes=" + minutes);
    //	alert(screen.width);
    window.open(baseURL + "/Monitor/ReplayRoute_Display.aspx?mapId=" + mapId + "&userIds=" + userIds + "&startTime=" + startTime + "&minutes=" + minutes + "&tagId=" + userIds, null,
	"height=" + (screen.height - 60) + ",width=" + (screen.width - 20) + ",top=0,left=0,location=no,resizable=yes,scrollbars=yes");
}
