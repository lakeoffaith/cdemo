var Locating = {};


//开关: 是否跳过定位条件设置步骤
Locating.skipSetting = true;  // true | false










// ---------------------------------------------------------------



setSurveyGroupDisabled= function(value) {
	$("SurveyGroup").disabled = value;
}

setScanTargetSsidDisabled = function(value) {
	$("scanTarget").disabled = value;
	$("scanSsid").disabled = value;
}








Locating.startLocatingClicked = function() {
	if (Locating.skipSetting) {
		$("startLocating").click();
	}
	else {
		Locating.openPanel();
	}
}

Locating.start = function() {
	    //var f = document.forms[0];
	    if ($(idPreString+"scanMode")!=null && $(idPreString+"scanMode").checked)
	    {	
		    if (($(idPreString+"scanTarget").value == "1" || $(idPreString+"scanTarget").value == "4") && $(idPreString+"scanSsid").value == "") {
			    alert("请输入scanSsid");
			    return false;
		    }
		    if ($(idPreString+"surveyGroup").value == "") {
			    alert("请选择采样祖");
			    return false;
		    }
	    }
    
	return true;
}

Locating.stop = function() {
	return true;
}

Locating.quickStart = function(tagId) {
	var loading = document.createElement("span");
	var a = $("op_" + tagId);
	a.parentNode.insertBefore(loading, a);
	a.style.display = "none";
	$$(loading).showLoading("连接..");

	MarshalAjax.QuickStartLocating(tagId, function(succeed) {
		loading.parentNode.removeChild(loading);
		if (succeed) {
			a.innerHTML = "停止";
			a.href = "javascript:Locating.quickStop(" + tagId + ");";
			a.style.display = "";
		}
		else {
			a.style.display = "";	
			alert("启动请求失败，请检查 LocatingService 是否可被连接。");
		}
	});
}

Locating.quickStop = function(tagId) {
	var loading = document.createElement("span");
	var a = $("op_" + tagId);
	a.parentNode.insertBefore(loading, a);
	a.style.display = "none";
	$$(loading).showLoading("连接..");

	MarshalAjax.QuickStopLocating(tagId, function(succeed) {
		loading.parentNode.removeChild(loading);
		if (succeed) {
			a.innerHTML = "快速启动";
			a.href = "javascript:Locating.quickStart(" + tagId + ");";
			a.style.display = "";
		}
		else {
			a.style.display = "";	
			alert("启动请求失败，请检查 LocatingService 是否可被连接。");
		}
	});
}


//
// ui controllers below .......
//

Locating.selectTag = function(el) {
	var boxes = $("tagList").getElementsByTagName("input");
	var n = 0;
	var values = [];
	for (var i = 0; i < boxes.length; i++) {
		if (boxes[i].name == "selection" && boxes[i].checked) {
			n++;
			values.push(boxes[i].value);
		}
	}
	$("selectedTagIds").value = values.join(",");

	var entry = $$("floatEntry");
	var panel = $$("floatPanel");
	if (n == 0) {
		entry.hide();
		panel.hide();
	}
	else {
		for (var i = 1;  i <= 2; i++) {
			var str = "已选中<span style='font-family:Tahoma; font-size:20px' class='t2'>&nbsp;" + n + "&nbsp;</span>个标签";
			$("selectionTitle" + i).innerHTML = str;
		}
		var pos = getPos(el);
		entry.locate(pos.x + el.offsetWidth + 3, pos.y + 3);
		entry.display();
	}
}

Locating.closeEntry = function() {
	var boxes = $("tagList").getElementsByTagName("input");
	for (var i = 0; i < boxes.length; i++) {
		boxes[i].checked = false;
	}
	$$("floatEntry").hide();
	$$("floatPanel").hide();
}

Locating.openPanel = function() {
	var panel = $$("floatPanel");
	panel.display();
	panel.locateCenter();
}

Locating.closePanel = function() {
	$$("floatPanel").hide();
}