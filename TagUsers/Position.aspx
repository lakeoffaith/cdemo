<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Position.aspx.cs" Inherits="NetRadio.LocatingMonitor.TagUsers.__Position" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
<%@ Register TagPrefix="NetRadio" TagName="TagSelector" Src="~/Controls/TagSelector.ascx" %>
	<Include type="javascript" src="../Scripts/UI/TagUser.aspx.js" />

			<style type="text/css">
			
			.propName {
				width: 135px;
				background: #fefefe;
			}
			.propValue {
				color: #aa0000;
			}
			
			</style>	
			
			<asp:HiddenField id="userId" runat="server" />
			
			<NetRadio:CollapseView id="summary" runat="server">
				<Header>
					<Caption>摘要信息</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td class="propName">名称</td>
							<td class="propValue"><NetRadio:SmartLabel id="name" signclientid="true" class="bold" runat="server"/>
								&nbsp; &nbsp; 
								<asp:LinkButton id="deleteButton" text="[从本系统中删除该定点报警标签]" class="t3" onclick="deleteButton_Click" onclientclick="javascript:return confirm('删除后有关历史数据不可恢复, 确定要从本系统中删除该警员信息吗?');" runat="server" />
								
								<div id="changeNameDialog" style="display:none">
									<div class="floatLayer" style="position:fixed; width:200px">
										<span class="bold">更改姓名</span>
										<div class="vspace"></div>
										姓名: <asp:TextBox id="newName" maxlength="50" width="135" runat="server" />
										<div class="vspace"></div>
										<div id="changeNameButtons" class="center">
											<input type="button" class="button" value="确认" onclick="javascript:submitChangingNameOnly();" />
											<input type="button" class="button" value="取消" onclick="javascript:closeNameChangingDialog();" />
										</div>
										<div id="changeNameState"></div>
									</div>
								</div></td>
						</tr>
						<tr>
							<td class="propName">绑定标签</td>
							<td><NetRadio:SmartLabel id="tagMac" signclientid="true" class="propValue" runat="server" />
								&nbsp; &nbsp; 
								
								<div id="changeTagDialog" style="display:none">
									<div class="floatLayer" style="position:static; width:200px">
										<span class="bold">选择他携带的新标签:</span>
										<div class="vspace"></div>
										<asp:HiddenField id="currentTagId" runat="server" />
										<NetRadio:TagSelector id="tagSelector" AllowedSelectCount="1" SelectedTagsVisible="true" runat="server" />
										<div class="vspace"></div>
										<div id="changeTagButtons">
											<input type="button" class="button" value="确认" onclick="javascript:submitNewTag();" />
											<input type="button" class="button" value="取消" onclick="javascript:closeTagChangingDialog();" />
										</div>
										<div id="changeTagState"></div>
									</div>
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
						</tr>
					</table>
				</Content>
				<Footer />
			</NetRadio:CollapseView>
			
			<NetRadio:CollapseView id="warnings" runat="server">
				<Header>
					<Caption>告警状态</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
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
							<td></td>
							<td></td>
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
			<div class="center"><input type="button" class="button" value="关闭窗口" onclick="top.close();" /></div>
			<br />

</asp:Content>
