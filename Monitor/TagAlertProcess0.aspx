<%@ Page Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="TagAlertProcess0.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__TagAlertProcess0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <div>
        <NetRadio:CollapseView ID="summary" runat="server">
				<Header>
					<Caption>摘要信息</Caption>
				</Header>
				<Content>
			<table class="fixed grid">
				<tr>
				    <td class="propName">报警者名称</td>
					<td class="propValue"><NetRadio:SmartLabel id="tagName" target="_blank" runat="server" /></td>
					<td class="propName">报警位置</td>
					<td class="propValue"><NetRadio:SmartLabel id="coordinatesName" runat="server" /></td>
				</tr>
				<tr>
				    <td class="propName">报警类型</td>
				    <td class="propValue"><NetRadio:SmartLabel id="description" runat="server" /></td>
				    <td class="propName">报警时间</td>
				    <td class="propValue"><NetRadio:DateTimeLabel id="time" format="H:mm:ss" class="t3" runat="server" /></td>
				</tr>
				<tr>
				    <td class="propName">处理人</td>
				    <td class="propValue"><NetRadio:SmartLabel id="alertMaster" runat="server" /></td>
				    <td class="propName">当前状态</td>
				    <td class="propValue"><NetRadio:SmartLabel id="alertStatus" format="H:mm:ss" class="t3" runat="server" /></td>
				</tr>
				</table>
				</Content>
				<Footer />
        </NetRadio:CollapseView>
        <NetRadio:CollapseView ID="processAction" runat="server">
				<Header>
					<Caption>事件处理</Caption>
				</Header>
				<Content>
			<table class="fixed grid">
				<tr>
				    <td class="propName">处理结果</td>
				    <td colspan="3">
		                <asp:RadioButtonList runat="server" ID="alertResultList" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        </asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox runat="server" ID="otherReason" Width="100px" MaxLength="20"></asp:TextBox>
                        <NetRadio:SmartLabel id="alertResult" style="font-weight:bold; color:Red;" runat="server" />&nbsp;&nbsp;&nbsp;&nbsp;
<%--                        <NetRadio:Anchor ID="Anchor2" text="[确定]" class="t3" href="javascript:Set();" runat="server" />--%>
                        <asp:Button ID="alertProcess" OnClick="alertProcess_Click" text="确定" style="width:40px; height:19px" href="javascript:Set();" runat="server" />
				    </td>
				</tr>
				<tr>
				    <td colspan="4" align="center">
					<NetRadio:Remind id="feedbacks" autohideduration="5" style="width:350px; color:Red;" runat="server" />					
				    </td>
				</tr>
				<!--<tr>
				    <td class="propName">事件移交</td>
				    <td class="propValue" colspan="3">-->
				       <!-- 将报警处理移交给：<asp:DropDownList runat="server" ID="copDropDownList"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
				       -->
<%--				        <NetRadio:Anchor ID="Anchor1" text="[移交]" class="t3" href="javascript:Set();" runat="server" />
--%>				         <!-- <asp:Button ID="handover" OnClick="handover_Click" text="移交" style="width:40px; height:19px" runat="server" />
				    </td>
				</tr>-->
<%--				<tr>
				    <td colspan="4">
				        
				    </td>
				</tr>--%>
				<tr>
				    <td colspan="4" align="center">

				    </td>
				</tr>
			</table>
							
				</Content>
				<Footer />
        </NetRadio:CollapseView>
        <NetRadio:CollapseView ID="processLog" runat="server">
				<Header>
					<Caption>处理历史</Caption>
				</Header>
				<Content>
	        <table class="fixed grid" width="80%">
	            <tr>
                    <td colspan="4" align="center">报警处理进度表</td>	        
	            </tr>
		        <tr>
		            <td>处理人</td>
		            <td>报警状态</td>
		            <td>详细信息</td>
		            <td>处理时间</td>
		        </tr>
				<asp:Repeater id="list" runat="server">
					<ItemTemplate>
						<NetRadio:Div id="div" runat="server">
						    <tr>
						        <td><NetRadio:SmartLabel id="processPerson" runat="server" /></td>
							    <td><NetRadio:SmartLabel id="preocessStatus" runat="server" /></td>
							    <td><NetRadio:SmartLabel id="changeDes" runat="server" /></td>
							    <td><NetRadio:DateTimeLabel id="changetime" format="H:mm:ss" class="t3" runat="server" /></td>
							</tr>
						</NetRadio:Div>
					</ItemTemplate>
				</asp:Repeater>
			</table>
							
				</Content>
				<Footer />
        </NetRadio:CollapseView>
    </div>
</asp:Content>
