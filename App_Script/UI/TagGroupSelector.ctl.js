var Benz = {};
Benz.receiver = null;
Benz.layer = null;

Benz.onSelect = function(box) {
	var inputs = Benz.layer.getElementsByTagName("input");
	var arr = [];
	for (var i = 0; i < inputs.length; i++) {
		var el = inputs[i];
		if (el.checked) {
			arr.push(el.value);
		}
	}
	switch (arr.length) {
		case 0:
			Benz.receiver.value = "未指定组";
			Benz.receiver.style.color = "Gray";
			break;
		default:
			Benz.receiver.value = "已选组: " + arr.join(", ");
			Benz.receiver.style.color = "";
			break;
	}
}

Benz.show = function(el) {
	Benz.receiver = el;

	if (Benz.layer == null) {
		Benz.createLayer();
	}
		
	var layer = $$(Benz.layer);
	var pos = getPos(el);
	layer.display();
	layer.locate(pos.x, pos.y);

	addEvent(document, "mousemove", Benz.hide);
}

Benz.hide = function(e) {
	e || (e = window.event);
	var el = e.target ? e.target : e.srcElement;
	while (el) {
		if (el == Benz.layer) {
			return;
		}
		el = el.parentNode;
	}
	$$(Benz.layer).hide();
	removeEvent(document, "mousemove", Benz.hide);
}

Benz.createLayer = function() {
    var div = document.createElement("div");
    div.id = "divGroupName";
    div.className = "floatList";
    div.style.width = Math.max(parseInt(Benz.receiver.clientWidth), 145) + "px";
    div.style.cursor = "pointer";
    div.style.display = "none";
    document.body.appendChild(div);
    Benz.layer = div;

    var loading = document.createElement("span");
    div.appendChild(loading);
    try {
        $$(loading).showLoading();
    }
    catch (e) {
        loading.innerHTML = "Loading ...";
    }

    TagGroupSelector.SelectAllGroups(function(groups) {
        div.removeChild(loading);

        var ul = document.createElement("ul");
        ul.style.listStyle = "none";
        div.appendChild(ul);

        for (var i = 0; i < groups.length; i++) {
            var li = document.createElement("ul");
            li.style.marginTop = "3px";
            ul.appendChild(li);

            var input = document.createElement("input");
            input.type = "checkbox";
            input.style.cursor = "pointer";
            input.name = "groupItem";
            input.id = "g_" + groups[i].Id.toString();
            input.value = groups[i].Id;
            input.onclick = Benz.onSelect;
            li.appendChild(input);

            var label = document.createElement("label");
            label.htmlFor = input.id;
            label.nowrap = "nowrap";
            label.innerHTML = groups[i].GroupName;
            li.appendChild(label);
        }

        var li = document.createElement("ul");
        li.innerHTML = "<div class='t3' style='margin:10px 3px 3px 4px'>多选表示或关系(并集)<br />不选择任何组表示不限制</div>";
        ul.appendChild(li);

        Benz.setDefault();
    });
}

Benz.setDefault = function() {
	var textboxValue = Benz.receiver.value.replace(/(未指定组|已选组:|\s)/gi, "");
	if (textboxValue != "") {
		var arr = textboxValue.split(",");
		var inputs = Benz.layer.getElementsByTagName("input");
		for (var i = 0; i < inputs.length; i++) {
			var el = inputs[i];
			el.checked = el.value.inArray(arr);
		}
	}
}