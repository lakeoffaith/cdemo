function setRefreshDuration(duration) {
	var f = document.forms[0];
	for (var i = 0; i < f.elements.length; i++) {
		var el = f.elements[i];
		if (el.id.endsWith("autoRefreshDuration")) {
			el.value = duration.toString();
			break;
		}
	}
	$("autoRefreshDurationText").innerHTML = duration + "秒";
	Dropmenu.hide();
}

function doRefresh() {
	var exsitedFilterPopup = $('filterForm');
	if (exsitedFilterPopup && exsitedFilterPopup.style.display != 'none') {
		setTimeout(doRefresh, 2000);
	} else {
		document.forms[0].autoRefreshFlag.value = "1";
		document.forms[0].autoRefreshSender.click();
	}
}

function showProgress() {
	setTimeout(function() {
		var el = $("refreshProgress");
		if (el) {
			el.innerHTML += ".";
		}
		showProgress();
	}, 333);
}

window.onload = showProgress;