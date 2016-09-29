<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="PopupEventWindowWithVedio.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__PopupEventWindowWithVedio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <div id="tempDIV" style="height: 0px; width: 0px;">
    </div>
    <table cellpadding="0" cellspacing="0" border="0" class="grid fixed">
        <tr id="vedioYangZhou" runat="server">
            <td width="820" height="240" align="center" valign="top" bgcolor="#f9f9f9" style="font-size: 20px;
                font-weight: bold;">
                <object id="HCNetVideo" style="left: 0px; width: 320px; top: 0px; height: 240px"
                    codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
                    standby="Waiting...">
                </object>
                &nbsp; &nbsp;
                <object id="HCNetVideo1" style="left: 0px; width: 320px; top: 0px; height: 240px"
                    codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
                    standby="Waiting...">
                </object>
            </td>
        </tr>
        <tr id="vedioFuJian" runat="server">
            <td width="420" height="270" align="center" valign="top" bgcolor="#f9f9f9" style="font-size: 20px;
                font-weight: bold">
                <object id="ocx_vedio" height="270" width="360" name="ocx" codebase="../Controls/DVM_IPCam2.ocx#version=0,0,0,1"
                    classid="clsid:A4150320-98EC-4DB6-9BFB-EBF4B6FBEB16" standby="Waiting...">
                </object>
            </td>
        </tr>
        <tr id="vedioShangHai" runat="server">
            <td width="420" height="270" align="center" valign="top" bgcolor="#f9f9f9" style="font-size: 20px;
                font-weight: bold">
                <object id="vedioOcxShangHai" height="270" width="360" codebase="../Controls/Monitor.cab#version=3,2,3,0"
                    classid="clsid:B0E85D65-7449-4491-9844-8FA996AEB92F" viewastext>
                    <param name="_Version" value="65536">
                    <param name="_ExtentX" value="15875">
                    <param name="_ExtentY" value="11906">
                    <param name="_StockProps" value="0">
                    <param name="MaxReConnectTimes" value="10">
                    <param name="ReConnectTimer" value="60000">
                    <param name="ShowMenu" value="0">
                </object>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top" bgcolor="#f9f9f9">
                <div id="tdContent" runat="server" style="overflow-y: scroll;">
                    数据加载中，请稍候。。。
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField runat="server" ID="AlertCoordinates"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="activeUrl"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="activeUrlref"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="hidUserID"></asp:HiddenField>

    <script language="javascript">
        var sdiv = document.createElement("div");
        document.getElementById("tempDIV").appendChild(sdiv);
        function setBorder() {
            sdiv.className = "popWinBody";
            sdiv.style.height = document.body.offsetHeight - 8;
            sdiv.style.width = document.body.offsetWidth - 8;
        }


        window.onresize = setBorder;
        setBorder();


        //document.getElementById("eventRemind").innerHTML = "<img src='../Images/text_warningMessage.gif' alt='' />";

        window.onblur = function() {
            //window.focus();
        }

        window.onunload = function() {
            //opener._showPopWin = true;
        }

        function SetFocus() {
            window.focus();
        }

        function SetUnFocus() {
            window.blur();
        }

        function ShowPopWin() {
            opener._showPonWin = true;
        }

        function HidePopWin() {
            opener._showPonWin = false;
        }

        function setResolved(btn) {
            var el = btn.parentNode;
            while (el) {
                if (el.id.substring(0, 3) == "id_") {
                    break;
                }
                el = el.parentNode;
            }

            el.style.display = "none";

            var id = el.id.substring(3);
            var mid = $("hidUserID").value;
                 
                 //GTang20101123在弹出警告框点击确认不改变警告状态
            //ThreadingAjax.SetResolved(id, mid, function(r) { });

            window.open(baseURL + "/Monitor/TagAlertProcess.aspx?id=" + id, "_detail");
        }

        function mute() {
            var obj = $$("beep");
            if (obj && obj.element) obj.remove();
        }

        function sound() {
            var el = $("beep");
            if (el == null || typeof el == "undefined") {
                document.write("<bgsound id='beep' src='../Sounds/beep.wav' loop='-1' />");
            }
        }

        function loadfirst(cid) {
            AlertView(cid);
        }

        function loadfirst2(cid) {
            AlertView2(cid);
        }
        function loginHCNet(url, urlref) {
            var HCNetVideo = document.getElementById("HCNetVideo");
            var HCNetVideo1 = document.getElementById("HCNetVideo1");

            if ($("activeUrl").value != url) {
                var obj = new Object();
                obj = HCNetVideo;
                obj.Logout();
                var i = obj.Login(url, 8000, "admin", "12345");
                $("activeUrl").value = url;
            }

            if ($("activeUrlref").value != urlref) {
                var obj1 = new Object();
                obj1 = HCNetVideo1;
                obj1.Logout();
                var j = obj1.Login(urlref, 8000, "admin", "12345");
                $("activeUrlref").value = urlref;
            }
        }

        function StartPlay(ichanel, ichanelref) {
            var HCNetVideo = document.getElementById("HCNetVideo");
            var HCNetVideo1 = document.getElementById("HCNetVideo1");

            var obj = new Object();
            obj = HCNetVideo;
            obj.StopRealPlay();
            obj.StartRealPlay(ichanel, 0, 0);

            var obj1 = new Object();
            obj1 = HCNetVideo1;
            obj1.StopRealPlay();
            obj1.StartRealPlay(ichanelref, 0, 0);
        }

        function AlertView(cid) {
            __PopupEventWindowWithVedio.GetHCUrl(cid, function(result) {
                if ($("AlertCoordinates").value != cid) {
                    loginHCNet(result.url, result.urlref);
                    StartPlay(result.iChannel, result.iChannelref);
                }
                $("AlertCoordinates").value = cid;
            });
        }



        var g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd;
        var flag_ip, flag_port;
        var isPlaying = false;
        var isSet = false;
        var isConnect = false;
        var isComplete = true;

        function AlertView2(cid) {
            // return;
            //alert('AlertView2');
            if (isComplete == false) {
                return;
            }
            isComplete = false;
            NetRadio.LocatingMonitor.Monitor.__PopupEventWindowWithVedio.GetAnchorCamera(cid, function(result) {
                if (result.error != 0 || result.value == null) {
                    isComplete = true;
                    return;
                }
                var __value = result.value;
                if ($("AlertCoordinates").value != cid) {
                    $("AlertCoordinates").value = cid;
                    g_hasVedio = __value.CoordinatesHasVedio;
                    g_cid = cid;
                    if (__value.coordinate != null && __value.coordinate.length > 0) {
                        g_ip = __value.coordinate[0].IP;
                        g_port = __value.coordinate[0].Port;
                        g_user = __value.coordinate[0].User;
                        g_pwd = __value.coordinate[0].Pwd;
                    }
                    if (isPlaying) {
                        // todo: stop vedio
                        getVedio1().StopVideo();
                    }
                    else {
                        beforePlayVedio(g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd);
                    }
                }
                else {
                    isComplete = true;
                }
            });
        }

        var g_Index = -1;
        function AlertView3(cid) {
            // return;
            //alert('AlertView2');
            if (isComplete == false) {
                return;
            }
            isComplete = false;
            NetRadio.LocatingMonitor.Monitor.__PopupEventWindowWithVedio.GetAnchorCameraShangHai(cid, function(result) {
                if (result.error != 0 || result.value == null) {
                    isComplete = true;
                    return;
                }
                var __value = result.value;
                if ($("AlertCoordinates").value != cid) {
                    $("AlertCoordinates").value = cid;
                    if (__value.CoordinatesHasVedio && __value.coordinate != null && __value.coordinate.length > 0) {
                        if (g_Index != -1) {
                            $("vedioOcxShangHai").CloseMonitor(g_Index);
                        }
                        g_Index = coordinate[0].Index;
                        $("vedioOcxShangHai").OpenMonitor(coordinate[0].Index, coordinate[0].IP, coordinate[0].Port, coordinate[0].Channel, coordinate[0].Password, coordinate[0].OSDHostName)
                    }

                }
                isComplete = true;
            });
        }


        function beforePlayVedio(hasVedio, cid, ip, port, user, pwd) {
            if (hasVedio) {
                //alert('有视频');
                window.setTimeout(
                                                  "PlayVedio('" + ip + "',"
                                                  + port + ",'"
                                                  + user + "','"
                                                  + (pwd == null ? "" : pwd)
                                                   + "')"
                                                  , 1);

            }
            else {
                //alert('无视频');
                isComplete = true;
            }
        }

        function __OnMonitorConnectResult(Result) {
            if (Result == 0) {
                //alert('已经成功连接了');
                getVedio1().PlayVideo();
                isConnect = true;
            }
        }
        function __OnMonitorDisconnectResult(Result) {
            if (Result == 0) {
                //alert('已经dis连接了');
                //getVedio1().PlayVideo();
                isConnect = false;
                beforePlayVedio(g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd);
            }
        }

        function __OnVideoStopped(Result) {
            if (Result == 0) {
                // alert('已经停止播放了');
                isPlaying = false;
                getVedio1().MonitorDisconnect();
                //beforePlayVedio(g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd);
            }
        }
        function __OnPlayVideoResult(Result) {
            if (Result == 0) {
                // alert('已经开始播放了');
                isComplete = true;
            }
        }

        function getVedio1() {
            return document.getElementById("ocx_vedio");
        }

        function PlayVedio(ip, port, user, pwd) {
            isPlaying = true;
            var v1 = getVedio1();
            if (isSet == false) {
                v1.attachEvent("OnMonitorConnectResult", __OnMonitorConnectResult);
                v1.attachEvent("OnMonitorDisconnected", __OnMonitorDisconnectResult);
                v1.attachEvent("OnVideoStopped", __OnVideoStopped);
                v1.attachEvent("OnPlayVideoResult", __OnPlayVideoResult);
                isSet = true;
            }

            v1.MonitorConnect(ip, port, user, pwd);
            return;


            //alert('开始连接');
            //if(isConnect==false)
            //alert(flag_ip +"_"+ ip +"_"+  flag_port  +"_"+ port);
            if (flag_ip != ip || flag_port != port) {
                v1.MonitorConnect(ip, port, user, pwd);
                flag_ip = ip; flag_port = port;
            }
            else {
                //alert('f');
                __OnMonitorConnectResult(0);
            }

        }
        sound();
    </script>

    <script>

        var isCompleteGetResult = true;
        function execGetResult() {

            if (isCompleteGetResult == false) {
                return;
            }
            isCompleteGetResult = false;
            NetRadio.LocatingMonitor.Monitor.__PopupEventWindowWithVedio.GetResult(function(result) {

                var __value = result.value;

                if (result.error == 0) {
                    $("tdContent").innerHTML = __value.html;
                    if (__value.callBackFunction != "") {
                        eval(__value.callBackFunction);
                    }
                }
                else {
                    $("tdContent").innerHTML = result.errorText;
                }
                isCompleteGetResult = true;

            });
        }
        execGetResult();
        window.setInterval("execGetResult()", 2000);
    </script>

</asp:Content>
