function viewAreaStayTimeReport() {
	var f = document.forms[0];
	var areaId = f.areaList.value;
	if (areaId == 0) {
		alert("请选择一个地图区域。");
		return;
	}
	
	var d1 = f.fromDate.value;
	if (d1.value == "") {
		alert("请选择要统计的开始日期。");
		return;
	}
	var d2 = f.toDate.value;
	if (d2.value == "") {
		alert("请选择要统计的结束日期。");
		return;
	}
	
	var h1 = f.fromHour.value;
	var h2 = f.toHour.value;
	
	var m1 = f.fromMinute.value;
	if (parseInt(m1.value) < 0 || parseInt(m1.value) > 59) {
		alert("开始时间中分钟的数字范围只能在0-59之间。");
		return;
	}
	var m2 = f.toMinute.value;
	if (parseInt(m2.value) < 0 || parseInt(m2.value) > 59) {
		alert("结束时间中分钟的数字范围只能在0-59之间。");
		return;
	}
	
	var tags = f.tagSelector_selectedTagIds.value;
	if (tags == "") {
		alert("没有选择标签。");
		return;
	}
	
	
	
//	var   newwin=window.open("AreaStayTimeReportViewer.aspx?areaId=" + areaId + "&fromTime=" + 
//	             d1 + " " + h1 + ":" + m1 + ":00&toTime=" + d2 + " " + h2 + ":" +
//	             m2 + ":00&tagIdArray=" + tags+"&target='pop'"); 
	window.open ("AreaStayTimeReportViewer.aspx?areaId=" + areaId + "&fromTime=" + 
	             d1 + " " + h1 + ":" + m1 + ":00&toTime=" + d2 + " " + h2 + ":" +
	             m2 + ":00&tagIdArray=" + tags, "newwindow", 
	             "height=600, width=1000, toolbar=yes, menubar=yes, scrollbars=yes, resizable=yes, location=no, status=yes") 
}


function FindUserInArea() {
 
  	var f = document.forms[0];
	var areaId = f.areaList.value;
	if (areaId == 0) {
		alert("请选择一个地图区域。");
		return;
	}
	var areaname=f.areaList.Text;
	window.open ("ReportFindUser.aspx?areaId=" + areaId+"&areaname="+areaname, "newwindow", 
	             "height=100, width=1000, toolbar=no, menubar=no, scrollbars=yes, resizable=no, location=no, status=no") 
	
 }