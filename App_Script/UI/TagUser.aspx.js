function clearEvent(btn, eventName) {
	var mac = $("tagMac").innerHTML;
	var loading = document.createElement("span");
	btn.parentNode.insertBefore(loading, btn);
	btn.style.display = "none";
	$$(loading).showLoading();
	__TagUser.ClearEventStatus(mac, eventName, function(succeed) {
		loading.parentNode.removeChild(loading);
		if (succeed) {
			$(eventName).innerHTML = "正常";
		}
		else {
			btn.style.display = "";
			alert("状态清除请求失败，请检查 LocatingService 是否可被连接。");
		}
	});
}

function clearAllEvents(btn) {
	var mac = $("tagMac").innerHTML;
	var loading = document.createElement("span");
	btn.parentNode.insertBefore(loading, btn);
	btn.style.display = "none";
	$$(loading).showLoading();

	__TagUser.ClearAllEventStatus(mac, function(succeed) {
		loading.parentNode.removeChild(loading);
		if (!succeed) {
			btn.style.display = "";
			alert("状态清除请求失败，请检查 LocatingService 是否可被连接。");
		}
	});
}


function callTagChangingDialog() {
	$$("changeTagDialog").display();
	$$("changeTagButtons").display();
	$("changeTagState").innerHTML = "";
}

function closeTagChangingDialog() {
	$$("changeTagDialog").hide();
}

function submitChangingTag() {
	var uid = $("userId").value;
	var oldId =  $("currentTagId").value;
	var newId = $("tagSelector_selectedTagIds").value;
	
	if (newId == "" || newId == null || isNaN(newId)) {
	    newId = 0;
		//return false;
	}

	$$("changeTagButtons").hide();
	$$("changeTagState").showLoading("提交中 ...");
	
	if (oldId == 0) {
		__TagUser.BindTag(uid, newId, function(response) {
		 
			if (response == "FAIL") {
				$("changeTagState").innerHTML = "<span class='t2'>连线失败。</span>";
			}
			else if (response == "") {
				$("changeTagState").innerHTML = "<span class='t2'>操作成功。</span>";
				setTimeout(closeTagChangingDialog, 1000);
				setTimeout(function() { $("tagBound").innerHTML = "未携带标签"; $("tagMac").innerHTML = response; }, 1010);
			}
			else {
				$("changeTagState").innerHTML = "<span class='t2'>操作成功。</span>";
				$("currentTagId").value=newId;
				setTimeout(closeTagChangingDialog, 1000);
				setTimeout(function() { $("tagBound").innerHTML = "已领用标签"; $("tagMac").innerHTML = response; }, 1010);
			}
		});
	}
	else{
		__TagUser.ChangeTag(oldId, newId, function(response) {
		 
			if (response == "FAIL") {
				$("changeTagState").innerHTML = "<span class='t2'>连线失败。</span>";
			}
			else if (response == "") {
				$("changeTagState").innerHTML = "<span class='t2'>操作成功。</span>";
				setTimeout(closeTagChangingDialog, 1000);
				setTimeout(function() { $("tagMac").innerHTML = response; }, 1010);
			}
			else {
				$("changeTagState").innerHTML = "<span class='t2'>操作成功。</span>";
				$("currentTagId").value=newId;
				setTimeout(closeTagChangingDialog, 1000);
				setTimeout(function() { $("tagMac").innerHTML = response; }, 1010);
			}
		});
	}
}



//----------------



function callRoomChangingDialog() {
	$$("changeRoomDialog").display();
	$$("changeRoomButtons").display();
	$("changeRoomState").innerHTML = "";
}

function closeRoomChangingDialog() {
	$$("changeRoomDialog").hide();
}

function submitChangingRoom() {
	var uid = $("userId").value;
	var newRoomId = $("newJailRoom").value;
	
	if (newRoomId == "-1") {
		alert("请选择现在所在的监舍");
		return false;
	}
	$$("changeRoomButtons").hide();
	$$("changeRoomState").showLoading("提交中 ...");
	__TagUser.ChangeJailRoom(uid, newRoomId, function(response) {
		if (response == "") {
			$("changeRoomState").innerHTML = "<span class='t2'>连线失败。</span>";
		}
		else {
			$("changeRoomState").innerHTML = "<span class='t2'>操作成功。</span>";
			setTimeout(closeRoomChangingDialog, 1000);
			setTimeout(function() { $("jailRoom").innerHTML = response; }, 1010);
		}
	});
}



//----------------


function callNameChangingDialog() {
	$$("changeNameDialog").display();
	$$("changeNameButtons").display();
	$("changeNameState").innerHTML = "";
}

function closeNameChangingDialog() {
	$$("changeNameDialog").hide();
}

function submitChangingName() {
	var uid = $("userId").value;
	var newName = $("newName").value.trim();
	var newNumber = $("newNumber").value.trim();
	
	if (newName == "") {
		alert("请输入名称");
		return false;
	}
//	var maxlen = $("newNumber").maxLength;
//	if (newNumber.length != maxlen || isNaN(newNumber)) {
//		alert("请输入正确的" + maxlen + "位" + (maxlen == 5 ? "号码" : "警号"));
//		return false;
//	}
	
	$$("changeNameButtons").hide();
	$$("changeNameState").showLoading("提交中 ...");
	NetRadio.LocatingMonitor.TagUsers.__TagUser.ChangeName(uid, newName, newNumber, function(response) {
	        if(response.error==0)
	        {
	           if (response.value === false) {
			        $("changeNameState").innerHTML = "<span class='t2'>连线失败。</span>";
		        }
		        else {
			        $("changeNameState").innerHTML = "<span class='t2'>操作成功。</span>";
			        setTimeout(closeNameChangingDialog, 1000);
			        setTimeout(function() { $("name").innerHTML = newName; $("number").innerHTML = newNumber; }, 1010);
		        }
	        }
	        else
	        {
	            $("changeNameState").innerHTML = "<span class='t2'>"+response.errorText+"</span>";
	            setTimeout(closeNameChangingDialog, 1000);
	        }
		
	});
}



//----------------


function callMemoChangingDialog() {
	$$("changeMemoDialog").display();
	$$("changeMemoButtons").display();
	$("changeMemoState").innerHTML = "";
}

function closeMemoChangingDialog() {
	$$("changeMemoDialog").hide();
}

function submitChangingMemo() {
	var uid = $("userId").value;
	var newMemo = $("newMemo").value.trim();
	
	$$("changeMemoButtons").hide();
	$$("changeMemoState").showLoading("提交中 ...");
	__TagUser.ChangeMemo(uid, newMemo, function(response) {
		if (response === false) {
			$("changeMemoState").innerHTML = "<span class='t2'>连线失败。</span>";
		}
		else {
			$("changeMemoState").innerHTML = "<span class='t2'>操作成功。</span>";
			setTimeout(closeMemoChangingDialog, 1000);
			setTimeout(function() { $("memo").innerHTML = newMemo.textEncode(); }, 1010);
		}
	});
}