// event Remind.
var _soundRemark = null;
var _showPopWin = true;
var _alertWindowHandle = null;
var _alerts = 0;
var _LatestTime = null; //lyz;最新报警时间

var winPopObject = null; //lyz;弹出窗口

function popWarning() {
    if (winPopObject && winPopObject.open && !winPopObject.closed) {
        winPopObject.focus();
    }
    else {
        winPopObject = window.open(baseURL + "/" + PopupEventWindowUrl + "?masterFile=Master/WebItem.Master", "PopupMonitorWindow", PopupEventWindowPara); //内蒙
        //"width=1100,height=500,top=120,left=300,toolbar=0,menubar=0,location=0,status=0");
    }
}

function showEventRemind() {

    if (typeof (ThreadingAjax) == "undefined") {
        return;
    }

    var el = document.getElementById("eventRemind");
    if (el) {
        ThreadingAjax.GetNewEventCounts(function(result) {
            //			var str = "";
            if (result && result.TotalCount > 0) {

                if (_LatestTime != result.LatestTime) {
                    _showPopWin = true;
                }

                //                if (_alerts != result.TotalCount) {
                //                    _showPopWin = true;
                //                }



                //				if (result.LatestCount > 0) {
                //					str += "<img src='" + baseURL + "/images/text_warningMessage.gif' width='65' height='14' alt='' align='absmiddle' /> &nbsp;";
                //				}
                //				str += "<a href=\"" + baseURL + "/Monitor/LatestEvents.aspx\" " +
                //					"onmouseover='javascript:WarningTagLister.display(this);' " +
                //					"onmouseout='javascript:WarningTagLister.hide();' " +
                //					"style='text-decoration:none'>";
                //				if (result.LatestCount > 0) {
                //					str += "在 <span class='t2'>" + result.TimePoint + "</span> 之后有 <span class='t2 bold'>" + result.LatestCount + "</span> 次报警";
                //				} else {
                //					str += "有 <span class='t2 bold' style='font-size:14px'>" + result.TotalCount + "</span> 个未处理报警";
                //				}
                //				str += "</a>";
                //				var now = new Date();
                //				if (result.MakeSound) {
                //					if (_soundRemark == null || now - _soundRemark > 10000) {
                //						//_soundRemark = now;
                //						str += "<bgsound loop='10' src='" + baseURL + "/sounds/beep.wav' />";
                //					}
                //				}

                if (_showPopWin && window.opener == null) {
                    _showPopWin = false;
                    //_alerts = result.TotalCount;
                    _LatestTime = result.LatestTime;
                    //alert('00000000000000000');
                    //文字
                    //window.open(baseURL + "/Monitor/PopupEventWindow.aspx", "PopupEventWindow",
                    //"width=1000,height=440,top=120,left=130,toolbar=0,menubar=0,location=0,status=0,resizable=yes");
                    //视频
                    popWarning();

                }
            }
            //			el.innerHTML = str;
        });
    }
    setTimeout(showEventRemind, 2500);
};





setTimeout(showEventRemind, 1000);

document.write("<div id='js_warningTags' class='shadow' style='position:absolute; display:none; z-index:9999' onmouseover='javascript:clearTimeout(WarningTagLister.timer);' onmouseout='javascript:WarningTagLister.hide();'></div>");
var WarningTagLister = function() {
};
WarningTagLister.timer = null;
WarningTagLister.resetTimePoint = function() {
    var el = document.getElementById("timePointReset");
    el.disabled = "disabled";
    el.className = "t3";
    ThreadingAjax.ResetTimePoint(_emptyFunc);
};
WarningTagLister.display = function(handler) {
    var container = document.getElementById("js_warningTags");
    if (container.style.display != "none") {
        clearTimeout(WarningTagLister.timer);
        return;
    }

    container.innerHTML = "";

    var offset = getPos(handler);
    container.style.left = offset.x + handler.offsetWidth - 325;
    container.style.top = offset.y + 20 + "px";
    container.style.display = "";

    var table = $create("table", container);
    table.className = "grid";
    table.style.width = "320px";

    var headtbody = $create("tbody", table);
    var tr = $create("tr", headtbody);
    var td1 = $create("td", tr);
    td1.innerHTML = "<span class='bold'>最新未处理事件</span>";
    var td2 = $create("td", tr);
    td2.align = "right";
    //if ($("eventRemind").innerHTML.indexOf("之后有") != -1) {
    //	td2.innerHTML = "<a id='timePointReset' href='javascript:WarningTagLister.resetTimePoint();'>重设时间点</a> &nbsp;";
    //}
    td2.innerHTML += "<a href=\"" + baseURL + "/Monitor/LatestEvents.aspx\">更多...</a>";

    var loadingtbody = $create("tbody", table);
    var loadingtr = $create("tr", loadingtbody);
    var loadingtd = $create("td", loadingtr, "loadingtd");
    loadingtd.colspan = "2";
    $$("loadingtd").showLoading();

    ThreadingAjax.GetEventApprizings(function(events) {
        loadingtbody.style.display = "none";
        var listtbody = $create("tbody", table);
        for (var i = 0; i < events.length; i++) {
            tr = $create("tr", listtbody);
            td1 = $create("td", tr);
            td2 = $create("td", tr);
            td2.align = "right";
            td2.className = "t3";
            td1.innerHTML = "<a href='" + baseURL + "/Objects/Tag.aspx?id=" + events[i].TagId + "' target='_blank'>" + events[i].TagName + "</a> : " + events[i].Description;
            if (events[i].IsNew) {
                td2.className = "t1";
            }
            td2.innerHTML = events[i].Time;
        }
    });
};
WarningTagLister.hide = function() {
    WarningTagLister.timer = setTimeout(function() {
        var container = $("js_warningTags");
        container.style.display = "none";
    }, 200);
};



function setSomeHeight() {
    document.getElementById("flashContainer").style.height = document.documentElement.scrollHeight - 180 + "px";
}

function setFlastHeight() {
    setInterval("setSomeHeight()", 50);
}