<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="LocatingMap.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Monitor.__LocatingMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script type="text/javascript">
        function initMap() {
            var div = document.getElementById("flashContainer");

            var obj = new Object();
            obj.width = div.clientWidth; //宽度
            obj.height = div.clientHeight; //高度
            obj.maplistxml = "../Flash/xml/MapList.ashx"; 		//地图列表XML
            obj.mapxml = "../Flash/xml/Map.ashx?id=<%# FacilityMapId%>"; 		//地图XML
            obj.tagxml = "../Flash/xml/TagCoordinates.ashx?mapId=<%# FacilityMapId%>"; 	//标签XML
            obj.cameraxml = "../Flash/xml/camera.ashx?mapId=<%# FacilityMapId%>"; //摄像头url
            obj.hostgroupxml = "../Flash/xml/HostGroup.ashx";
            obj.latesteventsxml = "../Flash/xml/LatestEvents.ashx";
            obj.jumpurl = "?facilityId=@fid&mapId=@mid"; //下拉列表选择时跳转到的网址，@开头的字符串将会被替换为列表值
            obj.searchurl = "?keyword=@keyword&eventtypes=@eventtypes&pagesize=@pagesize&currentpage=@currentpage&taggroups=@taggroups&tagid=@tagid"; //标签搜索网址，@开头的字符串将会被替换为列表值
            obj.refersh = 2000; 				//刷新时间（毫秒），标签XML数据的间隔时间
            obj.delay = 500; 				//现实中的1000毫秒相当于地图上的数值，500表示移动速度为现实的2倍
            obj.speed = 4;
            return obj;
        }
        function viewTag(tid) {
            window.open("../Monitor/AlertHostInfo.aspx?masterFile=Master/WebItem.Master&id=" + tid);
        }

        //	function viewEvent(eid) {
        //	    alert(eid);
        //	            }

        function viewAllEvents() {
            window.open("../Monitor/LatestEvents.aspx", "_self");
        }

        function viewLastEventName(EventType, hostGroupID) {
            var _href = "";
            var parasHref = "?userType=" + hostGroupID + "&pid=10&masterFile=Master/WebItem.Master";
            switch (parseInt(EventType)) {
                case 1:
                    _href = "History/PositionLog.aspx";
                    break;
                case 2:
                    _href = "History/AreaEventLog.aspx";
                    break;
                case 3:
                    _href = "History/AbsenceLog.aspx";
                    break;
                case 4:
                    _href = "History/LowBatteryLog.aspx";
                    break;
                case 5:
                    _href = "History/BatteryResetLog.aspx";
                    break;
                case 6:
                    _href = "History/ButtonPressedLog.aspx";
                    break;
                case 7:
                    _href = "";
                    break;
                case 8:
                    _href = "";
                    break;
                case 9:
                    _href = "";
                    break;
                case 10:
                    _href = "";
                    break;
                case 11:
                    _href = "";
                    break;
                default: break;
            }
            if (_href.length > 0) {
                window.open("../" + _href + parasHref, "blank");
            }
        }

        function viewUserName(id, hostGroupID) {
            if (hostGroupID == 1) {
                window.open("../TagUsers/Police.aspx?id=" + id, "blank");
            }
            else if (hostGroupID == 2) {
                window.open("../TagUsers/Culprit.aspx?id=" + id, "blank");
            }
        }

        function callVideo(cid, name, parameter) {
            if (parameter.length == 0)
                return;
            var yz = parameter.split(",");
            switch (gVideoType) {
                case 1:

                    callCamera(yz[0], yz[1], yz[2], yz[3]);
                    break;
                case 2:
                    callCamera(0, 0, 0, 0, 0, yz[5], yz[8], yz[6], yz[7]);
                    break;
                default:

                    break;
            }
        }





        function loginHCNet(url, urlref) {
            var HCNetVideo = document.getElementById("HCNetVideo");
            var HCNetVideo1 = document.getElementById("HCNetVideo1");

            //if ($("activeUrl").value != url) {
            var obj = new Object();
            obj = HCNetVideo;
            obj.Logout();
            var i = obj.Login(url, 8000, "admin", "12345");
            //$("activeUrl").value = url;
            // }

            // if ($("activeUrlref").value != urlref) {
            var obj1 = new Object();
            obj1 = HCNetVideo1;
            obj1.Logout();
            var j = obj1.Login(urlref, 8000, "admin", "12345");
            // $("activeUrlref").value = urlref;
            //}
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



        var g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd;
        var flag_ip, flag_port;
        var isPlaying = false;
        var isSet = false;
        var isConnect = false;
        var isComplete = true;



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
            //alert('已经开始播放了'+Result);
            if (Result == 0) {
                //alert('已经成功连接了');
                getVedio1().PlayVideo();
                isConnect = true;
            }
            //             else if (Result == -1) {//防止被拿掉
            //              isPlaying = false;
            //              isConnect =false;
            //             }
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
                //alert('已经停止播放了');
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
            //alert('开始连接');
            //if(isConnect==false)
            //alert(flag_ip +"_"+ ip +"_"+  flag_port  +"_"+ port);

            v1.MonitorConnect(ip, port, user, pwd);
            return;


            if (flag_ip != ip || flag_port != port) {
                //alert(ip+"-"+port+"-"+user+"-"+"-"+pwd);
                alert('0');
                v1.MonitorConnect(ip, port, user, pwd);
                flag_ip = ip; flag_port = port;
            }
            else {
                alert('f');
                __OnMonitorConnectResult(0);
            }

        }


        function callCamera(
                CameraIP,
				iChannel,
				CameraIPlref,
				iChannelref,
				CameraIDs,
				IP,
				Port,
				User,
				Pwd
                ) {
            //debugger;
            if (gVideoType == 1) {
                loginHCNet(CameraIP, CameraIPlref);
                StartPlay(iChannel, iChannelref);
            } else if (gVideoType == 2) {

                g_hasVedio = true;
                g_ip = IP;
                g_port = Port;
                g_user = User;
                g_pwd = Pwd;
                //alert(g_ip+"-"+g_port+"-"+g_user+"-"+"-"+g_user);
                if (isPlaying) {
                    //alert("stop");  
                    // todo: stop vedio
                    getVedio1().StopVideo();
                }
                else {
                    //alert("beforePlayVedio");  
                    beforePlayVedio(g_hasVedio, g_cid, g_ip, g_port, g_user, g_pwd);
                }
            }
        }   
    </script>

    <div id="divVedio" runat="server" style="width: 240px; vertical-align: top; text-align: left;
        float: left">
        <NetRadio:CollapseView ID="video" runat="server">
			    <Header>
				    <Caption></Caption>
			    </Header>
			    <Content>
				    <table id="vedioYangZhou" runat="server" class="grid fixed" style="margin-top:100px;">
				        <tr>
				            <td align=center>
				                <object id="HCNetVideo" height="180px" width="240px" name="ocx"
            codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
            standby="Waiting...">
        </object>
                            </td>
                            </tr>
                            <tr>
				            <td align=center>
				                <object id="HCNetVideo1" height="180px" width="240px" name="ocx"
            codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
            standby="Waiting...">
        </object>
				            </td>
				        </tr>
				    </table>
				    
				     <table id="vedioDemo" runat="server" class="grid fixed">
				        <tr>
				            <td align=center>
				               <object id="ocx_vedio" height="180" width="240" name="ocx" codebase="../Controls/DVM_IPCam2.ocx#version=0,0,0,1"
                    classid="clsid:A4150320-98EC-4DB6-9BFB-EBF4B6FBEB16" standby="Waiting...">
                </object>
                            </td>
                            </tr>
                            
				    </table>
			    </Content>
        </NetRadio:CollapseView>
    </div>
    <div id="flashContainer" style="margin: auto; overflow: hidden;">
        <%--   <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"
            width="100%" height="92%" id="map" align="top">
            <param name="allowScriptAccess" value="sameDomain" />
            <param name="allowFullScreen" value="true" />
            <param name="wmode" value="Opaque">
            <param name="movie" value="../Flash/MapRealTime.swf" />
            <param name="quality" value="high" />
            <param name="bgcolor" value="#ffffff" />
            <embed src="MapRealTime.swf" quality="high" bgcolor="#ffffff" width="100%" height="100%"
                name="map" align="top" allowscriptaccess="sameDomain" type="application/x-shockwave-flash"
                pluginspage="http://www.macromedia.com/go/getflashplayer" />
        </object>
        --%>
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" id="Main" width="100%"
            height="92%" codebase="http://fpdownload.macromedia.com/get/flashplayer/current/swflash.cab">
            <param name="movie" value="../Flash/MapRealTime.swf" />
            <param name="quality" value="high" />
            <param name="bgcolor" value="#ffffff" />
            <param name="allowScriptAccess" value="sameDomain" />
            <param name="allowFullScreen" value="true" />
            <embed src="MapRealTime.swf" quality="high" bgcolor="#ffffff" width="100%" height="100%"
                name="Main" align="middle" play="true" loop="false" quality="high" allowscriptaccess="sameDomain"
                type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/go/getflashplayer">
			</embed>
        </object>
    </div>

    <script>

        function setSomeHeight() {
            document.getElementById("flashContainer").style.height = document.documentElement.scrollHeight - 180 + "px";
        }
        function page_Load() { setFlastHeight(); } 
    </script>

</asp:Content>
