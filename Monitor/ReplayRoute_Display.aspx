<%@ Page Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="ReplayRoute_Display.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__ReplayRoute_Display" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script language="javascript">



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

        function formatDateTime(t) {
            var r = "";
            r += t.getFullYear() + "-";
            r += t.getMonth() + "-";
            r += t.getDate() + " ";
            r += t.getHours() + ":";
            r += t.getMinutes() + ":";
            r += t.getSeconds();
            return r;
        }

        function StartPlayBackByTime(ichanel, ichanelref, beginTime, endTime) {


            var obj = document.getElementById("HCNetVideo");
            obj.StopPlayBack();
            obj.PlayBackByTime(ichanel, formatDateTime(beginTime), formatDateTime(endTime));

            var obj1 = document.getElementById("HCNetVideo1");
            obj1.StopPlayBack();
            obj1.PlayBackByTime(ichanelref, formatDateTime(beginTime), formatDateTime(endTime));
        }

        function StopPlayBack() {
            var obj = document.getElementById("HCNetVideo");
            var obj1 = document.getElementById("HCNetVideo1");
            obj.StopPlayBack();
            obj1.StopPlayBack();
        }


        function Vedio() {
            var userId = request("userIds");
            if (userId.indexOf(",", 0) > 0) {
                $("ddVedio").style.display = "none";
                $("ddVedioTip").style.display = "none";
                return; //只能查看一个人的视频
            }
            userId = parseInt(userId);


            var dateTime = request("startTime");
            var addMinutes = parseInt(request("minutes"));
            if (userId == NaN || addMinutes == NaN)
                return;

            NetRadio.LocatingMonitor.Monitor.__ReplayRoute_Display.GetCoordinatesVedio(userId, dateTime, addMinutes, function(r) {
                if (r.error == 0) {
                    var __value = r.value;
                    var beg = 0;

                    for (var i = 0; i < __value.length; i++)
                    //for(var i=3;i<4;i++)
                    {
                        var _v = __value[i];

                        var __CoordinatesId = _v.CoordinatesId;
                        var __CoordinatesHasVedio = _v.CoordinatesHasVedio;
                        var __CameraIP = _v.CameraIP;
                        var __CameraIPlref = _v.CameraIPlref;
                        var __iChannel = _v.iChannel;
                        var __iChannelref = _v.iChannelref;
                        var __BeginPlayTime = _v.BeginPlayTime;
                        var __EndPlayTime = _v.EndPlayTime;

                        window.setTimeout(
		                 "test_vedio("
		                 + __CoordinatesId + ","
		                 + __CoordinatesHasVedio + ",'"
		                 + __CameraIP + "','"
		                 + __CameraIPlref + "',"
		                 + __iChannel + ","
		                 + __iChannelref + ",'"
		                 + __BeginPlayTime + "','"
		                 + __EndPlayTime + "');"
                         , beg * 1000);
                        beg += _v.PlaySecondLength;
                    }
                }
                else {
                    alert(r.errorText);
                }
            });

        }


        function test_vedio(__CoordinatesId, __CoordinatesHasVedio, __CameraIP, __CameraIPlref, __iChannel, __iChannelref, __BeginPlayTime, __EndPlayTime) {
            __BeginPlayTime = new Date(__BeginPlayTime);
            __EndPlayTime = new Date(__EndPlayTime);
            //alert(__BeginPlayTime);
            if (__CoordinatesId != -1) {
                if (__CoordinatesHasVedio) {
                    // $("ddVedio").style.display="block";
                    loginHCNet(__CameraIP, __CameraIPlref);
                    StartPlayBackByTime(__iChannel, __iChannelref, __BeginPlayTime, __EndPlayTime);
                }
                else {
                    loginHCNet(__CameraIP, __CameraIPlref);
                    StopPlayBack();
                    //$("ddVedio").style.display="none";
                }
            }
        }
    </script>

    <script>
        function sh() {
            if ($("ddVedio").style.display == "block") {
                $("img1").src = "/images/z1.gif";
                $("ddVedio").style.display = "none";
                return;
            }

            if ($("ddVedio").style.display == "none") {
                $("img1").src = "/images/z2.jpg";
                $("ddVedio").style.display = "block";
                return;
            }
        }
    </script>

    <input type="hidden" id="activeUrl" />
    <input type="hidden" id="activeUrlref" />
    <div id="videoYangZhou" runat="server">
        <NetRadio:CollapseView ID="CollapseView1" runat="server">
			    <Header>
				    <Caption>视频录像</Caption>
			    </Header>
			    <Content>
				    <table class="grid fixed">
				        <tr>
				            <td align=center>
				               
				                <object id="HCNetVideo" height="350" width="480" name="ocx" codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1"
        classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924" standby="Waiting...">
    </object>   
                            </td>
                           <td align=center width="20">
                           </td>
				            <td align=center>
				               <object id="HCNetVideo1" height="350" width="480" name="ocx" codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924" standby="Waiting...">
                                </object>
				            </td>
				        </tr>
                      </table>
                    </Content>
        </NetRadio:CollapseView>
    </div>
    <div id="videoDefault" runat="server">
        <dl>
            <dd runat="server" id="ddVedioTip" style="margin: 0px; padding: 0px; width: 30px;
                overflow: hidden; float: left;">
                <img id="img1" src="/images/z2.jpg" onclick="sh()" style="cursor: pointer;" />
            </dd>
            <dd id="ddVedio" runat="server" style="margin: 0px; padding: 0px; width: 360px; overflow: hidden;
                float: left; display: block;">
                <NetRadio:CollapseView ID="video" runat="server">
			    <Header>
				    <Caption>视频录像</Caption>
			    </Header>
			    <Content>
				    <table class="grid fixed">
				        <tr>
				            <td align=center>
				               
				                <object id="HCNetVideo" height="270" width="360" name="ocx" codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1"
        classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924" standby="Waiting...">
    </object>   
                            </td>
                             </tr>
                              <tr>
				            <td align=center>
				               <object id="HCNetVideo1" height="270" width="360" name="ocx" codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924" standby="Waiting...">
                                </object>
				            </td>
				        </tr>
                      </table>
                    </Content>
                </NetRadio:CollapseView>
            </dd>
            <dd id="flashContainer" style="margin: 0px; padding: 0px; <% if(NetRadio.Business.BusSystemConfig.GetVedioType()==1) { %> float: right;
                <% } %>">

                <script>
                    document.write(
    "<object classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' id='flashmap'  codebase='http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0' align='top' style='width:100%; height:" + (screen.availHeight - 170) + "px;'>" +
            "<param name='allowScriptAccess' value='sameDomain' />" +
            "<param name='movie' value='../Flash/ReplayRoute.swf' />" +
			"<param name='quality' value='high' />" +
			"<param name='bgcolor' value='#ffffff' />" +
            "<param name='allowFullScreen' value='true' />" +
            "<embed src='../Flash/ReplayRoute.swf' quality='high' bgcolor='#ffffff'   name='flashmap' align='top' allowScriptAccess='sameDomain' type='application/x-shockwave-flash' pluginspage='http://www.macromedia.com/go/getflashplayer' />" +
			"</object>"
    );
    
     var temp= window.onload;//function(){ window.onload() ;}
			window.onload = function() {
			if(temp!=null)
			{
			   temp();
			}
				var div = document.getElementById("flashContainer");
				$("flashmap").changeScale(div.clientWidth, div.clientHeight);
				$("flashmap").loadXml("<NetRadio:SmartLabel id='xmlPath' runat='server' />");
				
				<NetRadio:SmartLabel id='showPlayerMarker' text='$("flashmap").showPlayer(true);' runat='server' />
			}
			window.onscroll = function() {
				return false;
			}
                </script>

            </dd>
        </dl>
    </div>
</asp:Content>
