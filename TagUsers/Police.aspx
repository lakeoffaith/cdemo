<%@ Page Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="Police.aspx.cs" Inherits="NetRadio.LocatingMonitor.TagUsers.__Police" %>

<%@ Register TagPrefix="NetRadio" TagName="TagSelector" Src="~/Controls/TagSelector.ascx" %>
<%@ Register Src="../Controls/SelectTag.ascx" TagName="SelectTag" TagPrefix="uc1" %>
<%@ Register Src="../Controls/SelectStrongestRssiTag.ascx" TagName="SelectStrongestRssiTag"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <style type="text/css">
        .propName
        {
            width: 135px;
            background: #fefefe;
        }
        .propValue
        {
            color: #aa0000;
        }
    </style>
    <asp:HiddenField ID="userId" runat="server" />
    <NetRadio:CollapseView ID="summary" runat="server">
				<Header>
					<Caption>摘要信息</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td class="propName">名称</td>
							<td class="propValue"><NetRadio:SmartLabel id="name" signclientid="true" class="bold" runat="server"/>
								&nbsp; &nbsp; 
								<NetRadio:Anchor ID="changeName" text="[更改]" class="t3" href="javascript:callNameChangingDialog();" runat="server" />
								&nbsp; &nbsp; 
								<asp:LinkButton id="deleteButton" text="[将其从本系统中删除]" class="t3" onclick="deleteButton_Click" onclientclick="javascript:return confirm('删除后有关历史数据不可恢复, 确定要将其删除吗?');" runat="server" />
								
								<div id="changeNameDialog" style="display:none">
									<div class="floatLayer" style="position:fixed; width:200px">
										<span class="bold">更改名称或编号:</span>
										<div class="vspace"></div>
										名称: <asp:TextBox id="newName" maxlength="50" width="135" runat="server" />
										<div class="lineBreak"></div>
										编号: <asp:TextBox id="newNumber" maxlength="50" width="135" runat="server" />
										<div class="vspace"></div>
										<div id="changeNameButtons" class="center">
											<input type="button" class="button" value="确认" onclick="javascript:submitChangingName();" />
											<input type="button" class="button" value="取消" onclick="javascript:closeNameChangingDialog();" />
										</div>
										<div id="changeNameState"></div>
									</div>
								</div></td>
							<td width="50%" style="width:50%" rowspan="3"><NetRadio:Img id="photo" runat="server" /></td>
						</tr>
						<tr>
							<td class="propName">编号</td>
							<td class="propValue"><NetRadio:SmartLabel id="number" signclientid="true" runat="server"/>
								&nbsp; &nbsp; 
								<NetRadio:Anchor ID="changeNumber" text="[更改]" class="t3" href="javascript:callNameChangingDialog();" runat="server" /></td>
						</tr>
						<tr>
							<td class="propName">分组</td>
							<td class="propValue"><NetRadio:SmartLabel id="LabelGroup" signclientid="true" runat="server"/>
								</td>
						</tr>
						<tr>
							<td class="propName">是否携带标签</td>
							<td><NetRadio:SmartLabel id="tagBound" signclientid="true" class="propValue" runat="server"/>
								<NetRadio:SmartLabel id="tagMac" signclientid="true" class="propValue" runat="server" />
								&nbsp; &nbsp; 
								<NetRadio:Anchor id="changeTag" text="更换标签" class="t3" runat="server" />
								<script>
								function tt()
								{
								    //callTagChangingDialog();
								    $("changeTagDialog").style.display="block";
								    SelectTag.FunctionNo="01";
		                            SelectTag.SelectedTagIDs=$("<%= currentTagId.ClientID%>").value;
                                    SelectTag.UserID=$("<%= userId.ClientID%>").value;
                                    SelectTag.RegFunction=function(){
                                     var _tagid=SelectTag.GetSelectedIDs(); 
                                     if(_tagid=="")_tagid="0"; 
                                     $("<%= currentTagId.ClientID%>").value=_tagid;
                                     var _mac=SelectTag.GetSelectedMACs();
                                     $("<%= tagBound.ClientID%>").innerHTML= _mac=="" ?"未携带标签":"已领用标签";
                                     $("<%= tagMac.ClientID%>").innerHTML=SelectTag.GetSelectedMACs();
                                     SelectTag.Close();                                     
                                     };
		                            SelectTag.Show();	
								}
								function tt2()
								{ 
								     $("changeTagDialog").style.display="block";
								     SelectStrongestRssiTag.Init();
								     SelectStrongestRssiTag.UserID=$("<%= userId.ClientID%>").value;
								     SelectStrongestRssiTag.Show();
								     SelectStrongestRssiTag.RegFunction=function(){
                                     var _tagid=SelectStrongestRssiTag.GetSelectedID(); 
                                     if(_tagid=="")_tagid="0"; 
                                     $("<%= currentTagId.ClientID%>").value=_tagid;
                                     var _mac=SelectStrongestRssiTag.GetSelectedMAC();
                                     $("<%= tagBound.ClientID%>").innerHTML= _mac=="" ?"未携带标签":"已领用标签";
                                     $("<%= tagMac.ClientID%>").innerHTML=SelectStrongestRssiTag.GetSelectedMAC();                                                                        
                                     };
								}
								
								</script> <div id="changeTagDialog" style="display:none">
								<asp:HiddenField id="currentTagId" runat="server" />
								<uc1:SelectTag ID="tagSelector" runat="server" />
								 
								<uc2:SelectStrongestRssiTag ID="selectStrongestRssiTag" runat="server"  EnableOK="true" EnableClose="true" EnableUnbind="true" EnableBorderCSS="true"/>
									<%--<div class="floatLayer" style="position:static; width:200px">
										<span class="bold">选择他携带的新标签:</span>
										<div class="vspace"></div>
										<asp:HiddenField id="currentTagId" runat="server" />
										<uc1:SelectTag ID="tagSelector" runat="server" />
										 
										<div class="vspace"></div>
										<div id="changeTagButtons">
											<input type="button" class="button" value="确认" onclick="javascript:submitChangingTag();" />
											<input type="button" class="button" value="取消" onclick="javascript:closeTagChangingDialog();" />
										</div>
										<div id="changeTagState"></div>
									</div>--%>
								</div></td>
						</tr>
						<tr>
							<td class="propName">备注</td>
							<td class="propValue" colspan=""><NetRadio:SmartLabel id="memo" signclientid="true" runat="server"/>
								&nbsp; &nbsp; 
								<NetRadio:Anchor ID="changeMemo" text="[更改]" class="t3" href="javascript:callMemoChangingDialog();" runat="server" />
								
								<div id="changeMemoDialog" style="display:none">
									<div class="floatLayer" style="position:fixed; width:300px">
										<span class="bold">更改备注:</span>
										<div class="vspace"></div>
										<asp:TextBox id="newMemo" width="290" rows="4" textmode="multiline" runat="server" />
										<div class="vspace"></div>
										<div id="changeMemoButtons">
											<input type="button" class="button" value="确认" onclick="javascript:submitChangingMemo();" />
											<input type="button" class="button" value="取消" onclick="javascript:closeMemoChangingDialog();" />
										</div>
										<div id="changeMemoState"></div>
									</div>
								</div>
							</td>
								
							<td><asp:FileUpload id="uploadPhoto"  width="240" runat="server" />
								<asp:Button id="uploadButton" text="更新照片" onclick="uploadButton_Click" runat="server" /></td>
						</tr>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <NetRadio:CollapseView ID="status" runat="server">
			    <Header>
			        <Caption>组别信息</Caption>
			    </Header>
			    <Content>
					<table class="grid fixed">
						<tr>
							<td class="propName">所属组</td>
							<td class="propValue" colspan="3">
							    <NetRadio:SmartLabel id="groups" runat="server"></NetRadio:SmartLabel>
                            </td>
						</tr>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <NetRadio:CollapseView ID="position" runat="server">
				<Header>
					<Caption>位置信息</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td class="propName">位置</td>
							<td class="propValue"><NetRadio:SmartLabel id="coordinatesName" text="未知" runat="server" /></td>
							<td class="propName">更新时间</td>
							<td class="propValue"><NetRadio:DateTimeLabel id="positionUpdateTime" format="yyyy-M-d H:mm:ss" showastimespan="true" runat="server" /></td>
						</tr>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <NetRadio:CollapseView ID="warnings" runat="server">
				<Header>
					<Caption>告警状态</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<div id="absencediv" runat="server">
						    <tr>
							    <td class="propName">消失告警</td>
							    <td class="propValue">
								    <NetRadio:SmartLabel id="absence" signclientid="true" text="未知" runat="server" />
								    <NetRadio:Anchor id="clearAbsence" text="清除" href="javascript:void(0)" onclick="javascript:clearEvent(this, 'absence');" class="t3" style="margin-left:20px" visible="false" runat="server" />
							    </td>
							    <td class="propName">区域告警</td>
							    <td class="propValue">
								    <NetRadio:SmartLabel id="areaEvent" signclientid="true" text="未知" runat="server" />
								    <NetRadio:Anchor id="clearAreaEvent" text="清除"
									     href="javascript:void(0)" onclick="javascript:clearEvent(this, 'areaEvent');"
									     class="t3" style="margin-left:20px" visible="false" runat="server" />
							    </td>
						    </tr>
						</div>
						<tr>
							<td class="propName">电池低电压告警</td>
							<td class="propValue">
								<NetRadio:SmartLabel id="batteryInsufficient" signclientid="true" text="未知" runat="server" />
								<NetRadio:Anchor id="clearBatteryInsufficient" text="清除" href="javascript:void(0)" onclick="javascript:clearEvent(this, 'batteryInsufficient');" 
									class="t3" style="margin-left:20px" visible="false" runat="server" />
							</td>
							<td class="propName">电池重置告警</td>
							<td class="propValue">
								<NetRadio:SmartLabel id="batteryReset" signclientid="true" text="未知" runat="server" />
								<NetRadio:Anchor id="clearBatteryReset" text="清除" href="javascript:void(0)" onclick="javascript:clearEvent(this, 'batteryReset');" 
									class="t3" style="margin-left:20px" visible="false" runat="server" />
							</td>
						</tr>
						<tr>
							<td class="propName">按钮告警</td>
							<td class="propValue">
								<NetRadio:SmartLabel id="buttonPressed" signclientid="true" text="未知" runat="server" />
								<NetRadio:Anchor id="clearButtonPressed" text="清除" href="javascript:void(0)" onclick="javascript:clearEvent(this, 'buttonPressed');" 
									class="t3" style="margin-left:20px" visible="false" runat="server" />
							</td>
							<%--<td class="propName">腕带断开告警</td>
							<td class="propValue">
								<NetRadio:SmartLabel id="wristletBroken" signclientid="true" text="未知" runat="server" />
								<NetRadio:Anchor id="clearWristletBroken" text="清除" href="javascript:void(0)" onclick="javascript:clearEvent(this, 'wristletBroken');" 
									class="t3" style="margin-left:20px" visible="false" runat="server" />
							</td>--%>
						</tr>
						<tr>
							<td class="propName">更新时间</td>
							<td class="propValue"><NetRadio:DateTimeLabel id="eventUpdateTime" format="yyyy-M-d H:mm:ss" showastimespan="true" runat="server" /></td>
							<td colspan="2">
								<NetRadio:Anchor id="clearAllEvents" text="清除所有告警" href="javascript:void(0)" onclick="javascript:clearAllEvents(this);" class="t3" style="margin-left:20px" visible="false" runat="server" />
								<NetRadio:SmartLabel id="locatingServiceDownMarker" class="t2" visible="false" runat="server">Locating Service 远程支持服务程序未启动，无法获得实时告警状态。</NetRadio:SmartLabel>
							</td>
						</tr>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <div class="center">
        <input type="button" class="button" value="关闭窗口" onclick="closeWindow();" /></div>
    <br />
</asp:Content>
