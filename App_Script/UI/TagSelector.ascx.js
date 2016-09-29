var Bmw = {};
Bmw.receiver = null;
Bmw.layer = null;
Bmw.maxAllowed = null;

Bmw.keywordFocus = function(el) {
	if (el.value == "关键字") {
		el.value = "";
		el.style.color = "";
	}
}

Bmw.keywordBlur = function(el) {
	if (el.value == "") {
		el.value = "关键字";
		el.style.color = "gray";
	}
}

Bmw.show = function(el) {
	if (Bmw.receiver == null) {
		Bmw.receiver = el.nextSibling;
	}
	if (Bmw.layer == null) {
		Bmw.layer = document.getElementById("tagSelector");
	}
	if (Bmw.maxAllowed == null) {
		var el = $("allowedCount");
		Bmw.maxAllowed = el ? parseInt(el.innerHTML) : 0;
	}

	var ul = $("selectorList");
	if (ul.innerHTML == "") {
		Bmw.listTags(); //20091231, do not load first time
	}

	var layer = $$(Bmw.layer);
	layer.display();
	layer.locateCenter();
}

Bmw.hide = function() {
	$$(Bmw.layer).hide();
}

Bmw.setDefault = function() {
	if (Bmw.receiver.value != "") {
		var arr = Bmw.receiver.value.split(",");
		var inputs = Bmw.layer.getElementsByTagName("input");
		var reach = (Bmw.maxAllowed > 0 && arr.length >= Bmw.maxAllowed);

		for (var i = 0; i < inputs.length; i++) {
			var el = inputs[i];
			if (el.type == "checkbox") {
				el.checked = el.value.inArray(arr);
				if (reach && !el.checked) {
					el.disabled = true;
				}
			}
		}
	}
}

Bmw.listTags = function() {
	$$("loading").showLoading("载入中 ...");
    
	var keyword = "";
	var groups = "";
	var f = document.forms[0];
	for (var i = 0; i < f.elements.length; i++) {
		if (f.elements[i].id.endsWith("tagNameKeyword")) {
			keyword = f.elements[i].value;
		}
//		else if (f.elements[i].id.endsWith("groupSelector")) {
//			groups = f.elements[i].value.replace(/(未指定组|已选组:|\s)/gi, "");
//		}
	}
	if (keyword == "关键字") keyword = "";
	__TagSelector.SelectTags(keyword, groups, 999999, 0, Bmw.renderTagList);
}

Bmw.renderTagList = function(tags) {
	var ul = $("selectorList");
	var selectedIds = Bmw.receiver.value.split(",");
	$("totalCount").innerHTML = tags.length;

	if (tags.length == 0) {
		ul.innerHTML = "<li class='t3'>无可选标签</span>";
	}
	else {
		ul.innerHTML = "";
		for (var i = 0; i < tags.length; i++) {
			var input = document.createElement("input");
			input.type = "checkbox";
			input.style.cursor = "pointer";
			input.name = "tagItem";
			input.id = "tag_" + tags[i].Id.toString();
			input.value = tags[i].Id;
			input.onclick = Bmw.onSelect;

			var label = document.createElement("label");
			label.htmlFor = input.id;
			label.nowrap = "nowrap";
			label.innerHTML = tags[i].TagName;

			var li = document.createElement("li");
			li.appendChild(input);
			li.appendChild(label);
			ul.appendChild(li);
		}
		Bmw.setDefault();
	}
	$("loading").innerHTML = "";
}

Bmw.showSelectedResult = function(tagNames) {
	var selectedList = $("selectedList");
	if (selectedList) {
		if (tagNames.length == 0) {
			selectedList.style.display = "none";
		} else {
			selectedList.style.display = "";
			selectedList.innerHTML = "<span class='t3'>已选择的标签:</span> &nbsp; " + tagNames.join(" ,&nbsp; ");
		}
	}
}

Bmw.onSelect = function(e) {
	var arr = [];
	var nameArray = [];
	var tags = Bmw.layer.getElementsByTagName("input");
	var srcCount = 0;
	
	for (var i = 0; i < tags.length; i++) {
		var el = tags[i];
		if (el.type == "checkbox" && el.name == "tagItem") {
			if (el.checked) {
				arr.push(el.value);
				nameArray.push("<a href='" + baseURL + "/Objects/Tag.aspx?id=" + el.value + "' target='_blank'>" + el.nextSibling.innerHTML + "</a>");
			}
			srcCount++;
			tags[i].disabled = false;
		}
	}
	
	var selectAllElement = $("selectAll");
	if (selectAllElement && srcCount != arr.length) {
		selectAllElement.checked = false;
	}
	if (Bmw.maxAllowed > 0 && arr.length >= Bmw.maxAllowed) {
		for (var i = 0; i < tags.length; i++) {
			var el = tags[i];
			if (el.type == "checkbox" && !el.checked) {
				el.disabled = true;
			}
		}
	}
	
	$("selectedCount").innerHTML = arr.length.toString();
	Bmw.receiver.value = arr.join(",");
	Bmw.showSelectedResult(nameArray);
}

function OnTagSelectorLoad() {
	var str = $("selectorLauncher").nextSibling.value;
	if (str != "") {
		__TagSelector.GetSelectedTags(str, Bmw.showSelectedResult);
	}
}