<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Home0.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.__Home0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
  
    <NetRadio:CollapseView ID="collapseView" runat="server">
			<Header>
				<Caption>系统信息</Caption>
			</Header>
			<Content>
				<table class="grid fixed" width="100%" style="text-align:left;">					
			         <asp:Repeater ID="RepGroup" runat="server">
                       <ItemTemplate>
                            <tr>
                                <td width="200" class="boldTD">
                                    <%# Eval("HostGroupName")%>
                                </td>
                                <td>
                                    总计 &nbsp;<span class="t2"><%# Eval("Count")%></span> &nbsp;； &nbsp; 在线 &nbsp;<span class="t2"><%# Eval("ActiveCount")%></span> &nbsp;;
                                    &nbsp; <a href="TagUsers/TagUserList.aspx?type=<%# Eval("HostGroupId")%>" id="tagListLink"
                                       >管理...</a>
                                </td>
                            </tr>
                       </ItemTemplate>
                    </asp:Repeater>
					<tr>
						<td colspan="2" style="height:8px; overflow:hidden"></td>
					</tr>
					<tr>
						<td class="boldTD">未处理报警</td>
						<td>有 <NetRadio:NumericLabel id="newEventCount" class="t2" runat="server" /> 个未处理事件或告警。
							&nbsp;
							<a href="/Monitor/LatestEvents.aspx" >详细...</a></td>
					</tr>
					<tr>
						<td class="boldTD">事件记录</td>
						<td><NetRadio:DateTimeLabel id="logFromDate" format="yyyy/M/d" class="t2" runat="server" /> 以来共有 <NetRadio:NumericLabel id="logCount" class="t2" runat="server" /> 条。
							&nbsp;
							<a href="/History/AlertProcessed.aspx?userType=1&pid=10">详细...</a>
							</td>
					</tr>
					<tr>
						<td class="boldTD">标签低电报警</td>
						<td><NetRadio:DateTimeLabel id="lowBatteryFromDate" format="yyyy/M/d" class="t2" runat="server" /> 以来共有 <NetRadio:NumericLabel id="lowBatteryCount" class="t2" runat="server" /> 次。
							&nbsp;
							<a href="/History/LowBatteryLog.aspx?userType=1&pid=10" >详细...</a>
							
					</tr>
					<tr>
						<td colspan="2" style="height:8px; overflow:hidden"></td>
					</tr>
					<tr>
						<td class="boldTD">AP定位器</td>
						<td>共<NetRadio:NumericLabel id="apCount" class="t2" runat="server" /> 个；
							&nbsp;
							有<NetRadio:NumericLabel id="apFailedCount" class="t2" runat="server" /> 个在工作。
							&nbsp;
							<a href="/Monitor/APList.aspx?pid=11" id="apListLink" >详细...</a></td>
					    
					</tr>
					<tr>
						<td class="boldTD">定位服务器运行状态</td>
						<td><span id="homePageLSstatus"></span></td>
						
					</tr>
				</table>
			</Content>
    </NetRadio:CollapseView>
</asp:Content>
