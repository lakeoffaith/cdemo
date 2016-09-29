<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="AlertHostInfo.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Monitor.__AlertHostInfo" %>

<%@ Register src="../Controls/ProcessAlert.ascx" tagname="ProcessAlert" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script language="javascript">
         function loginHCNet(url, urlref) {
            var HCNetVideo = document.getElementById("HCNetVideo");
            var HCNetVideo1 = document.getElementById("HCNetVideo1");

            //if ($("activeUrl").value != url) 
            {
                var obj = new Object();
                obj = HCNetVideo;
                obj.Logout();
                var i = obj.Login(url, 8000, "admin", "12345");
                $("activeUrl").value = url;
            }

            //if ($("activeUrlref").value != urlref) 
            {
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

var lastIsComplete=true;   
 
function AlertView() {
 
       if(lastIsComplete)
       {
           lastIsComplete=false;
            var tagID=parseInt(request("id"));       
            if(tagID== NaN)
            {
               tagID=0;
            }
            NetRadio.LocatingMonitor.Monitor.__AlertHostInfo.GetCoordinatesAndTagInfo(tagID,
                function(result) {  
                        lastIsComplete=true;             
                        if(result.error!=0 || result.value == null)
                        {  
                            //alert(result.errorText);                                          
                            return;
                        }                       
                        var __value=result.value;
                        var cid= parseInt(__value.coordinatesID);
                        
                         $input_SetAttribute("hostName","innerHTML",__value.hostName);
                         $input_SetAttribute("coordinates","innerHTML",__value.coordinates);
                         $input_SetAttribute("tagStatus","innerHTML",__value.tagStatus);
                         $("divVedio").style.display="none";
                        if(__value.SystemHasVedio && __value.CoordinatesHasVedio && cid>0)
                        {                        
                            $("divVedio").style.display="inline";
                            if ($("AlertCoordinates").value != cid) {                               
                                     window.setTimeout(function(){ 
                                    loginHCNet(__value.url, __value.urlref);
                                    StartPlay(__value.iChannel, __value.iChannelref);
                                     },1);       
                            }
                            $("AlertCoordinates").value = cid;
                        }                        
                    }
            );
         } 
}  
 var g_hasVedio,g_cid,g_ip,g_port,g_user,g_pwd  ; 
  var flag_ip,flag_port;
 var isSet=false;
 var isPlaying=false; 
function AlertView2() { 
       if(lastIsComplete)
       {
           lastIsComplete=false;
            var tagID=parseInt(request("id"));       
            if(tagID== NaN)
            {
               tagID=0;
            }
            NetRadio.LocatingMonitor.Monitor.__AlertHostInfo.GetAnchorCameraAndTagInfo(tagID,
                function(result) {  
                        lastIsComplete=true;             
                        if(result.error!=0 || result.value == null)
                        {  
                            //alert(result.errorText);                                          
                            return;
                        }                       
                        var __value=result.value;
                        var cid= parseInt(__value.coordinatesID);
                        
                         $input_SetAttribute("hostName","innerHTML",__value.hostName);
                         $input_SetAttribute("coordinates","innerHTML",__value.coordinates);
                         $input_SetAttribute("tagStatus","innerHTML",__value.tagStatus);
                         $("divVedio").style.display="none";
                        
                        if(__value.SystemHasVedio && __value.CoordinatesHasVedio && cid>0)
                        {       
                            $("divVedio").style.display="inline";
                            if ($("AlertCoordinates").value != cid) { 
                            
                             if(__value.coordinate!=null && __value.coordinate.length>0)
                             {
                                 g_ip=__value.coordinate[0].IP;
                                 g_port=__value.coordinate[0].Port;
                                 g_user=__value.coordinate[0].User;
                                 g_pwd=__value.coordinate[0].Pwd;
                                 
                                 g_hasVedio =__value.CoordinatesHasVedio;
                             }
                             
                                if(isPlaying)
                                {
                                   // todo: stop vedio
                                   getVedio1().StopVideo();
                                }
                                else
                                {
                                   beforePlayVedio(g_hasVedio,g_cid,g_ip,g_port,g_user,g_pwd);
                                }                                     
                            }
                            $("AlertCoordinates").value = cid;
                        }
                    }
            );
         } 
} 


function beforePlayVedio(hasVedio,cid,ip,port,user,pwd)
{   
//alert('f'+hasVedio);
if (hasVedio) {      
 //alert('有视频');
 window.setTimeout(
                  "PlayVedio('"+ip+"',"
                  +port+",'"
                  +user+"','"
                  +(pwd==null ? "":pwd)
                   +"')"
                  ,1);
             }
}

function __OnMonitorConnectResult(Result)
{
     if(Result==0)
     {
       getVedio1().PlayVideo();        
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
function __OnVideoStopped(Result)
{
     if(Result==0)
     {
       // alert('已经停止播放了');
        isPlaying=false;
        getVedio1().MonitorDisconnect();
        //beforePlayVedio(g_hasVedio,g_cid,g_ip,g_port,g_user,g_pwd);
     }
}
 function __OnPlayVideoResult(Result)
{
     if(Result==0)
     {
       
        lastIsComplete=true;   
     }
}
function getVedio1()
{
 return document.getElementById("ocx_Vedio");;
}
        
function PlayVedio(ip,port,user,pwd)
{
    isPlaying=true; 
    var v1 =getVedio1();  
    if(isSet==false)
    {
      v1.attachEvent("OnMonitorConnectResult",__OnMonitorConnectResult);
      v1.attachEvent("OnMonitorDisconnected", __OnMonitorDisconnectResult);
      v1.attachEvent("OnVideoStopped",__OnVideoStopped );
      v1.attachEvent("OnPlayVideoResult",__OnPlayVideoResult  );
      isSet=true;   
    }   
    
    v1.MonitorConnect(ip, port, user, pwd);    
            return;
            
            
            
    if(flag_ip != ip || flag_port!= port)
    {
      v1.MonitorConnect(ip,port,user,pwd);
      flag_ip = ip ; flag_port= port;
    }
    else
    {
      //alert('f');
      __OnMonitorConnectResult(0);
    }
}
    
    </script>

    <asp:ScriptManager ID="clientScriptManager" runat="server" />
    <div id="divVedioLoading" style="display: none; height: 50; vertical-align: middle;
        text-align: left;">
        视频连接中。。。。</div>
    <div id="divVedio" style="display: none;">
        <NetRadio:CollapseView ID="video" runat="server">
			    <Header>
				    <Caption>视频监控</Caption>
			    </Header>
			    <Content>
				    <table class="grid fixed">
				        <tr>
				            <td align=center>
				                <object id="HCNetVideo" height="270" width="360" name="ocx"
            codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
            standby="Waiting...">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </object>
                            </td>
				            <td align=center>
				                <object id="HCNetVideo1" height="270" width="360" name="ocx"
            codebase="../Controls/NetVideoActiveX23.cab#version=2,3,6,1" classid="clsid:CAFCF48D-8E34-4490-8154-026191D73924"
            standby="Waiting...">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </object>
				            </td>
				        </tr>
				    </table>
			    </Content>
        </NetRadio:CollapseView>
        <NetRadio:CollapseView ID="CollapseView_Video" runat="server">
			    <Header>
				    <Caption>视频监控</Caption>
			    </Header>
			    <Content>
				    <table class="grid fixed">
				        <tr>
				            <td align=center>
				                <object id="ocx_Vedio" height="270" width="360" name="ocx"
            codebase="../Controls/DVM_IPCam2.ocx#version=0,0,0,1" classid="clsid:A4150320-98EC-4DB6-9BFB-EBF4B6FBEB16"
            standby="Waiting...">
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </object>
                            </td>
				        </tr>
				    </table>
			    </Content>
        </NetRadio:CollapseView>
    </div>
    <NetRadio:CollapseView ID="summry" runat="server">
		        <Header>
		            <Caption>摘要信息</Caption>
		        </Header>
		        <Content>
		            <asp:UpdatePanel id="updatePanel" runat="server">
                        <ContentTemplate>
		                    <table class="grid fixed">
				                <tr>
				                    <td class="propName">名称</td>
				                    <td class="propValue"><span id="hostName"></span> </td>
				                    <td class="propName">当前位置</td>
				                    <td class="propValue"><span id="coordinates"></span> </td>
				                </tr>
				                <tr>
				                    <td class="propName">标签状态</td>
				                    <td class="propValue"><span id="tagStatus"></span> </td>
				                </tr>
				            </table>
				        </ContentTemplate>
				    </asp:UpdatePanel>
		        </Content>
    </NetRadio:CollapseView>
    <asp:HiddenField runat="server" ID="AlertCoordinates"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="activeUrl"></asp:HiddenField>
    <asp:HiddenField runat="server" ID="activeUrlref"></asp:HiddenField>
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    <br />
    
    
     <div id="aaaaaaaaaa" runat="server" visible="false">
       
         <uc1:ProcessAlert ID="ProcessAlert1" runat="server" />
       
    </div>
</asp:Content>
