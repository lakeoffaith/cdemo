
var _result;
var _tree;
var divArray = new Array(); //菜单
var divCount = 0; //菜单的个数
var pChildWidth = 15;
var menuBgColor = "#ffffff"; //菜单项的背景颜色
var menuBorder = "solid 1px #5194D2"; //菜单项的边框#D8F0FA
var menuBgColor_No = "#F2FBFE"; //菜单项的背景颜色（没有）
var menuBorder_No = "solid 1px #F2FBFE"; //菜单项的边框（没有）  
var menuTextColorOut = "#ffffff"; //菜单字体颜色 鼠标离开时
var menuTextColorOn = "#000000"; //菜单字体颜色 鼠标对准时


function cssTD(pid) {
    if (pid == null) {
        return 'class="tdMenu"';

    }
}

function onmouseAction(id, pid, MenuUrl, Target, MenuText) {

    return "onmouseover=\"menuMouseOver('dm_" + id + "')\" onmouseout=\"menuMouseOut('dm_" + id + "')\"" +
        "onclick=\"  fold('tr" + id + "','" + id + "','" + pid + "'); changeSelectColor('dm_" + id + "'); setDataInCookie('" + id + "'); goUrl('" + id + "','" + MenuUrl + "','" + Target + "','" + MenuText + "');\"";

}
function MC(objectArray, pid) {
    var _objectArray = Sort(Search(objectArray, "PID", pid), "SortNum", 1, 0);
    var tempArray = new Array();
    var r = new Array();
    r.push("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
    for (var i = 0; i < _objectArray.length; i++) {
        r.push("<tr>");
        r.push("<td height='30' style=\"padding:0px 2px 0px 2px;\"" + cssTD(_objectArray[i].PID) + ">");
        r.push("<div id=\"dm_" + _objectArray[i].ID + "\" style=\"height:22px; filter:alpha(opacity=0); border:solid 1px #FFFFFF; color:" + menuTextColorOut + ";\" selected=\"false\">");
        r.push("<span id=\"sp_" + _objectArray[i].ID + "\" style=\" position:relative; cursor:pointer;\" " + onmouseAction(_objectArray[i].ID, _objectArray[i].PID, _objectArray[i].MenuUrl, _objectArray[i].Target, _objectArray[i].MenuText) + "  >");
        r.push("<img src=\"" + _objectArray[i].MenuImgSrc + "\" align=\"absmiddle\" width=\"20px\" height= \"20px\"  />");
        r.push(_objectArray[i].MenuText);
        r.push("</span>");
        r.push("</div>");
        r.push("</td>");
        r.push("</tr>");
        divArray[divCount++] = "dm_" + _objectArray[i].ID;
        if (Search(objectArray, "PID", _objectArray[i].ID).length > 0) {
            r.push("<tr id=\"tr" + _objectArray[i].ID + "\" style=\"display:none;\">");
            r.push("<td style=\"padding-left:" + pChildWidth + "px;\">" + MC(objectArray, _objectArray[i].ID) + "</td>");
            r.push("</tr>");
        }
        else {
            r.push("<tr id=\"tr" + _objectArray[i].ID + "\" style=\"display:none;\" sign=\"\"><td>eeee</td></tr>");
        }
    }
    r.push("</table>");
    return r.join("");
}

function foldLoadSet(_r, id) {
    var nodes = Search(_r, "PID", id);
    for (var i = 0; i < nodes.length; i++) {
        fold('tr' + nodes[i].ID, "" + nodes[i].ID + "", "" + nodes[i].PID + "");
        foldLoadSet(_r, nodes[i].ID);
    }
}
function callBack_Master(r) {
    if (r.error != 0) {
        alert(r.errorText);
    }
    else {
        _result = r.value.TreeNodes;
        _result = Sort(_result, "SortNum", 1, 0);
        _tree = new tree();
        var _treeNodes = new Array();
        for (var i = 0; i < _result.length; i++) {
            _treeNodes[i] = new treeNode();
            _treeNodes[i].key = _result[i].ID;
            _treeNodes[i].parentKey = _result[i].PID;
            _treeNodes[i].value = _result[i];
        }
        _tree.treeNodes = _treeNodes;
        document.getElementById("DivInLeftMenu").innerHTML = MC(_result, null);
        var ids = getCookie("ids");
        //var _currentID= request("menuid");//getCookie("currentID"); 
        var _currentID = getCookie("currentID");

        if (ids != null && ids.length > 0) {
            var idsArray = new Array();
            if (ids != null && ids.length > 0) {
                idsArray = ids.split("$");
            }
            var nodes = Query(_result, "ID", idsArray);
            var nodesPre = Search(nodes, "PID", null);
            foldLoadSet(nodes, null);
        }
        if (gTheme != "Prison") {
            if (_currentID != null && _currentID.length > 0) {
                changeSelectColor('dm_' + _currentID);
            }
        }
        //debugger;
        //打开一级菜单
        //var _nodes=Search(_result,"PID",null);
        //for(var i=0;i<_nodes.length;i++)
        //{
        //fold("tr"+_nodes[i].ID,""+_nodes[i].ID+"",null);
        //}


        //热键
        var hotKeys = r.value.HotKeys;
        var hotKeysHTML = new Array();
        //hotKeysHTML.push("<input id='inputMasterMenu' class=hotkeysinput  type=button onclick='showMasterMenu()' />");     
        for (var i = 0; i < hotKeys.length; i++) {
            //hotKeysHTML.push("<input class=hotkeysinput  type=button value="+hotKeys[i].MenuText+" onclick=window.location.href='"+hotKeys[i].MenuUrl+"' />");
            hotKeysHTML.push("<div class=hotkeysinput onmousemove=\"this.className='hotkeysinput1';\" onmouseout=\"this.className='hotkeysinput';\" onclick=\"goUrl('" + hotKeys[i].ID + "','" + hotKeys[i].MenuUrl + "','" + hotKeys[i].Target + "','" + hotKeys[i].MenuText + "')\">" + hotKeys[i].MenuText + "</div>");
        }
        hotKeysHTML.push("<div class=hotkeysinput onmousemove=\"this.className='hotkeysinput1';\" onmouseout=\"this.className='hotkeysinput';\" id='inputMasterMenu' onclick='showMasterMenu()'></div>");
        if (gTheme != "Prison") {
            document.getElementById("hotkeys").innerHTML = hotKeysHTML.join("");
        } else {
            document.getElementById("hotkeys").innerHTML = "<div style=' width:25px; height:31px; float:left;background-image:url(/Images/left1.jpg);'></div><div style=' height:31px; float:left;  background-image:url(/Images/center1bg.jpg);'>" + hotKeysHTML.join("") + "</div><div style=' width:25px; height:31px; float:right;background-image:url(/Images/right1.jpg);'></div>";
            document.getElementById("hotkeys").style.width = ((hotKeys.length + 1) * 89 + 50) + "px";

        }

        if (getCookie("isShowMasterMenu") == null) {
            setCookie("isShowMasterMenu", 0);
        }

        if (getCookie("isShowMasterMenu") == 0) {
            document.getElementById("leftTD").style.display = "none";
            //document.getElementById("imgShowMasterMenu").src="/Images/right.jpg";
            document.getElementById("inputMasterMenu").innerText = "》系统设置"; //"显示主菜单";
        }
        else if (getCookie("isShowMasterMenu") == 1) {
            document.getElementById("leftTD").style.display = "block";
            //document.getElementById("imgShowMasterMenu").src="/Images/left.jpg";
            document.getElementById("inputMasterMenu").innerText = "《系统设置"; //"隐藏主菜单";
        }
    }
}

function showMasterMenu() {
    if (getCookie("isShowMasterMenu") == 1) {
        document.getElementById("leftTD").style.display = "none";
        //document.getElementById("imgShowMasterMenu").src="/Images/right.jpg";
        document.getElementById("inputMasterMenu").innerText = "》系统设置"; //"显示主菜单";
        setCookie("isShowMasterMenu", 0);
    }
    else if (getCookie("isShowMasterMenu") == 0) {
        document.getElementById("leftTD").style.display = "block";
        //document.getElementById("imgShowMasterMenu").src="/Images/left.jpg";
        document.getElementById("inputMasterMenu").innerText = "《系统设置"; //"隐藏主菜单";
        setCookie("isShowMasterMenu", 1);
    }
}




function goUrl(id, url, target, text) {
    //encodeURIComponent(fullText);
    if (url != "null" && url != null && url.toString().trim() != "#") {
        //alert(text);
        if (text.toString().indexOf("实时告警") != -1) {
            popWarning();
            return;
        }
        switch (target.toString().toLowerCase()) {
            case "self":
            default:
                window.location.href = url;
                break;

            case "blank":
                window.open(url);
                break;
        }
    }
    //   if(url!="null" && url!=null && url.toString().trim()!="#")
    //   {
    ////       var _url;
    ////       if(url.toString().indexOf("?",0)==-1)
    ////       {
    ////         _url=url+"?menuid="+id;
    ////       }
    ////       else
    ////       {
    ////         _url=url+"&menuid="+id;
    ////       }      
    ////       window.location.href=_url;
    //         window.location.href=url;
    //   }
}

function menuMouseOver(id) {
    if (document.getElementById(id).selected == "false") {
        document.getElementById(id).style.color = menuTextColorOn;
        if (gTheme == "Prison") return;
        document.getElementById(id).style.border = menuBorder;
        document.getElementById(id).style.backgroundColor = menuBgColor;
        document.getElementById(id).style.filter = "alpha(opacity=100);"
    }
}
function menuMouseOut(id) {
    if (document.getElementById(id).selected == "false") {

        document.getElementById(id).style.color = menuTextColorOut;
        if (gTheme == "Prison") return;
        document.getElementById(id).style.border = menuBorder_No;
        document.getElementById(id).style.backgroundColor = menuBgColor_No;
        document.getElementById(id).style.filter = "alpha(opacity=0);";
    }
}
function changeSelectColor(id) {
    if (gTheme == "Prison") return;
    for (var i = 0; i < divCount; i++) {
        document.getElementById(divArray[i]).style.border = menuBorder_No;
        document.getElementById(divArray[i]).style.backgroundColor = menuBgColor_No;
        document.getElementById(divArray[i]).style.filter = "alpha(opacity=0);"
        document.getElementById(divArray[i]).selected = "false";
        document.getElementById(divArray[i]).style.color = menuTextColorOut;
    }
    document.getElementById(id).style.border = menuBorder;
    document.getElementById(id).style.backgroundColor = menuBgColor;
    document.getElementById(id).style.filter = "alpha(opacity=100);"
    document.getElementById(id).selected = "true";
    document.getElementById(id).style.color = menuTextColorOn;
}
function fold(childsTrID, id, pid) {
    if (document.getElementById(childsTrID) != null && document.getElementById(childsTrID).sign == null) {
        if (document.getElementById(childsTrID).style.display == "none") {
            var brothers = Search(_result, "PID", pid);
            //                 for(var i=0;i<brothers.length;i++)
            //                 {
            //                   document.getElementById("tr"+brothers[i].ID).style.display="none"; 
            //                 }
            document.getElementById(childsTrID).style.display = "block";
            if (id != null) {
                _tree.getNode(id).value.Fold = true;
            }

        }
        else if (document.getElementById(childsTrID).style.display == "block") {
            document.getElementById(childsTrID).style.display = "none";
            if (id != null) {
                _tree.getNode(id).value.Fold = false;
                var nodes = _tree.getChildNodes(id);
                for (var i = 0; i < nodes.length; i++) {
                    nodes[i].value.Fold = false;
                }
            }
        }
        return;
    }
}
function setDataInCookie(ID) {

    setCookie("currentID", ID);
    var foldNodes = Search(_result, "Fold", true);
    var ids = "";
    for (var i = 0; i < foldNodes.length; i++) {
        if (i == 0) { ids = foldNodes[i].ID; }
        else { ids += "$" + foldNodes[i].ID; }
    }
    setCookie("ids", ids);
}
function setleftTDHEIGHT() {

    //var h1=document.getElementById("rightTD").clientHeight-
    //document.getElementById("rightTD").currentStyle.paddingTop.replace(/(px)/g,"");
    //  var h1=document.getElementById("rightTD").scrollHeight;
    //  var h2=document.documentElement.scrollHeight-100;//document.getElementById("topHeader").offsetHeight; 
    //  var h3=document.getElementById("pageheader").scrollHeight; 
    //  document.getElementById("leftTD").style.height=(h1>=h2?h1:h2)+"px";
    document.getElementById("rightTD").style.height = document.documentElement.scrollHeight - 120 + "px";
    document.getElementById("leftTD").style.height = document.documentElement.scrollHeight - 120 + "px";
    //document.getElementById("pagetemplate").style.height=document.getElementById("rightTD").style.height.replace(/(px)/g,"")-52+"px";

    //  alert(
    //  document.getElementById("rightTD").style.height+"\r\n"+
    //   document.getElementById("pagetemplate").style.height
    //  );
    //document.getElementById("rightTD").clientHeight+"_"+
    //document.getElementById("rightTD").offsetHeight+"_"+
    //document.getElementById("rightTD").scrollHeight)+"_"+.replace(/(px)/g,"")
    //debugger;
    //alert(

    //document.getElementById("rightTD").currentStyle.paddingTop.replace(/(px)/g,"")
    //);

    //alert(document.getElementById("rightTD").clientHeight+"_"+document.getElementById("rightTD").offsetHeight+"_"+document.getElementById("rightTD").scrollHeight)
    //rightTD.style.paddingTop
    //alert(h1+"_"+tt+"_"+document.getElementById("rightTD").offsetHeight)

    //document.getElementById("pagetemplate").style.height=(h1-h3)+"px";

    //  alert(
    //  document.getElementById("rightTD").offsetHeight
    //  );
}