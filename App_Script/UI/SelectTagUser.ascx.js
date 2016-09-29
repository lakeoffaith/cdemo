function showCount() {
    $('lCount').innerHTML = $('listLeft').options.length;
    $('rCount').innerHTML = $('listRight').options.length;
}
function isExists(id, value) {
    for (var i = $(id).options.length - 1; i >= 0; i--) {
        if ($(id).options[i].value == value) {
            return true;
        }
    }
    return false;
}
function select_ClearOptions(id) {
    $select_ClearOptions(id);
    showCount();
}
function select_AddOption(id, option) {
    $select_AddOption(id, option);
    showCount();
}
function select_DeleteOption(id, index) {
    $select_DeleteOption(id, index);
    showCount();
}
function multi_onclick(from, to) {
    for (var i = $(from).options.length - 1; i >= 0; i--) {
        if ($(from).options[i].selected) {
            var item = document.createElement("option");
            item.text = $(from).options[i].text;
            item.value = $(from).options[i].value;
            item.TagID = $(from).options[i].TagID;
            if (!isExists(to, item.value)) {
                select_AddOption(to, item);
            }
            select_DeleteOption(from, $(from).options[i].index);
        }
    }
}
function total_onclick(from, to) {
    for (var i = $(from).options.length - 1; i >= 0; i--) {
        var item = document.createElement("option");
        item.text = $(from).options[i].text;
        item.value = $(from).options[i].value;
        item.TagID = $(from).options[i].TagID;
        if (!isExists(to, item.value)) {
            select_AddOption(to, item);
        }
        select_DeleteOption(from, $(from).options[i].index);
    }
}
function searchLeft_onclick() {
    $$("loading").showLoading("载入中 ...");
    //    var userType = 1;
    //	var selectedGroupId=0;
    var keyword = "";
    var groupName = "";
    //    userType = parseInt($(ID_tagUserType).value);		 
    //	selectedGroupId =  parseInt($(ID_selectedGroupId).value);		 
    keyword = $("tagNameKeyword").value;
    groupName = $(ID_selectedGroupName).value;
    //	if (isNaN(userType)) userType = 1;
    if (keyword == "关键字") keyword = "";
    if (groupName == "未指定组") groupName = "";
    select_ClearOptions('listLeft');
    NetRadio.LocatingMonitor.Controls.__SelectTagUser.SearchTagUserSource_Left_Distinct($(ID_methodGetUserLeft).value, groupName, keyword, callBack_Left);
}
function searchRight_onclick() {
    var txt = $("keyWord").value.replace(/\s/g, "");
    if (txt.length > 0 && txt != "关键字") {
        $$("loading").showLoading("载入中 ...");
        select_ClearOptions('listRight');
        for (var i = 0; i < userSourceForRight.length; i++) {
            if (userSourceForRight[i].text.indexOf(txt, 0) >= 0) {
                select_AddOption('listRight', userSourceForRight[i]);
            }
        }
        $("loading").innerHTML = "";
    }
    else {
        select_ClearOptions('listRight');
        for (var i = 0; i < userSourceForRight.length; i++) {
            select_AddOption('listRight', userSourceForRight[i]);
        }
    }
}


function __ok_onclick() {
    setDivAndHiddenField();
    setSelectedUserIds();
    __userControl.close('divSelectTagUser');
}

function __cancel_onclick() {
    __userControl.close('divSelectTagUser');
}

function ok_onclick() {
    __ok_onclick();
}

function cancel_onclick() {
    __cancel_onclick();
}

function callBack_Left(r) {
    if (r.error == 0) {
        select_ClearOptions('listLeft');
        for (var i = 0; i < r.value.length; i++) {
            var item = document.createElement("option");
            item.text = r.value[i].UserName;
            item.value = r.value[i].UserID;
            item.TagID = r.value[i].TagID;
            select_AddOption('listLeft', item);
        }
        $("loading").innerHTML = "";
    }
    else {
        $("loading").innerHTML = "错误信息：" + r.errorText;
    }
}
function setDivAndHiddenField() {
    var c = "";
    var count = $("listRight").options.length;
    var ids = "";
    for (var i = 0; i < count; i++) {
        if (i == 0) { ids += $("listRight").options[i].value; c += $("listRight").options[i].text }
        else { ids += "," + $("listRight").options[i].value; c += "，" + $("listRight").options[i].text }
    }
    if (c.length > 0) { c = "{&nbsp;" + c + "&nbsp;}"; }
    if ($(ID_selectedList) != null) {
        $(ID_selectedList).innerHTML = "<font color=gray>已选择使用者：共计&nbsp;<font color=red>" + count + "</font>&nbsp;；</font><br/>" + c;
        $(ID_selectedList).style.display = "block";
    }
    $(ID_selectedUserIds).value = ids;

}

function setRightSource() {
    var userSourceRight = new Array();
    var count = $("listRight").options.length;
    for (var i = 0; i < count; i++) {
        var a = new NetRadio.Model.TagUser2_class();
        a.UserID = $("listRight").options[i].value;
        a.UserName = $("listRight").options[i].text;
        a.TagID = $("listRight").options[i].TagID;
        userSourceRight[i] = a;
    }
    $(ID_userSource_Right).value = convertObjectToJsonString(userSourceRight);
}

function setSelectedUserIds() {
    var count = $("listRight").options.length;
    var a = "";
    for (var i = 0; i < count; i++) {
        a += (i == 0 ? "" : ",") + $("listRight").options[i].value;
    }
    $(ID_selectedUserIds).value = a;
}

function checkNumForSelect() {
    var maxValue = $(ID_maxLength).value.length == 0 ? 999999 : parseInt($(ID_maxLength).value);
    var leftSelectCount = 0;
    for (var i = 0; i < $("listLeft").options.length; i++) {
        if ($("listLeft").options[i].selected) {
            leftSelectCount++;
        }
    }
    if (leftSelectCount + $("listRight").options.length > maxValue) {
        alert("最多可以选择" + maxValue + "个对象");
        return false;
    }
    return true;
}
function checkNumForAll() {

    var maxValue = $(ID_maxLength).value.length == 0 ? 999999 : parseInt($(ID_maxLength).value);
    if ($("listLeft").options.length + $("listRight").options.length > maxValue) {
        alert("最多可以选择" + maxValue + "个对象");
        return false;
    }
    return true;
}
function __some() { if (!checkNumForSelect()) return; multi_onclick('listLeft', 'listRight'); }
function __all() { if (!checkNumForAll()) return; total_onclick('listLeft', 'listRight'); }

var userSourceForRight = new Array();
var funCount = 0;

function SelectTagUser_ClearData() {
    select_ClearOptions('listLeft');
    select_ClearOptions('listRight');

    if ($("tagNameKeyword") != null) {
        $("tagNameKeyword").value = "关键字";
        $("tagNameKeyword").style.color = "gray";
    }
    if ($("keyWord") != null) {
        $("keyWord").value = "关键字";
        $("keyWord").style.color = "gray";
    }
    if (document.getElementsByName("ctl00$CphRight$tagSelector$selectedGroupName").length == 1 ) {
        document.getElementsByName("ctl00$CphRight$tagSelector$selectedGroupName").item(0).value = "未指定组";
        document.getElementsByName("ctl00$CphRight$tagSelector$selectedGroupName").item(0).style.color = "gray";
    }
    for (var i = 0; i < document.getElementsByName("groupItem").length; i++) {
        document.getElementsByName("groupItem").item(i).checked = false;    
    } 

}

function SelectTagUser_InitData() {

    if ($(ID_selectedList) != null) {
        $$(ID_selectedList).showLoading("载入中 ...");
        $(ID_selectedList).style.display = "block";
    }
    select_ClearOptions('listLeft');
    select_ClearOptions('listRight');
    if ($(ID_selectorLauncher) != null)
        $(ID_selectorLauncher).style.display = "none";

    NetRadio.LocatingMonitor.Controls.__SelectTagUser.GetTagUserSource_Left_Distinct($(ID_methodGetUserLeft).value, function(r) {
        if (r.error == 0) {
            select_ClearOptions('listLeft');
            for (var i = 0; i < r.value.length; i++) {
                var item = document.createElement("option");
                item.text = r.value[i].UserName;
                item.value = r.value[i].UserID;
                item.TagID = r.value[i].TagID;
                select_AddOption('listLeft', item);
            }
            funCount++;
            if (funCount == 2) {
                if ($(ID_selectorLauncher) != null)
                    $(ID_selectorLauncher).style.display = "block";
            }
        }
        else {
            alert(r.errorText);
        }
    });
    NetRadio.LocatingMonitor.Controls.__SelectTagUser.GetTagUserSource_Right_Distinct_ForLoad($(ID_methodGetUserLeft).value, $(ID_methodGetUserRight).value, $(ID_selectedUserIds).value, function(r) {
        if (r.error == 0) {
            select_ClearOptions('listRight');
            for (var i = 0; i < r.value.length; i++) {
                var item = document.createElement("option");
                item.text = r.value[i].UserName;
                item.value = r.value[i].UserID;
                item.TagID = r.value[i].TagID;
                userSourceForRight[i] = item;
                select_AddOption('listRight', item);
            }
            setDivAndHiddenField();
            funCount++;
            if (funCount == 2) {
                if ($(ID_selectorLauncher) != null)
                    $(ID_selectorLauncher).style.display = "block";
            }
        }
        else {
            alert(r.errorText);
        }
    });
}
function page_Load() {
    if (Para_AutoLoadData) {
        SelectTagUser_InitData();
    }
}
 