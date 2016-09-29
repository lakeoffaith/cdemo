<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="TagAlertUI.aspx.cs" Inherits="NetRadio.LocatingMonitor.Report.__TagAlertUI"%><%@ Register TagPrefix="NetRadio" TagName="TagUserSelector" Src="~/Controls/TagUserSelector.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
<blockquote>
				<table class="fixed grid">
     
					<tr id="AlertUserSelecting"  >
						<td width="130">选择对象</td>
						<td><asp:DropDownList id="AlertUserList" width="140" runat="server" /></td>
					</tr>
					<tr id="AlertStatusTypeSelecting"  >
						<td width="130">选择报警</td>
						<td><asp:DropDownList id="AlertStatusTypeList" width="140" runat="server" /></td>
					</tr>
					
					
					<tr>
						<td>选择时间范围</td>
						<td><NetRadio:CalendarBox id="fromDate" width="80" runat="server" />
						<asp:DropDownList id="fromHour" runat="server">
							<asp:ListItem value="0">0 时</asp:ListItem>
							<asp:ListItem value="1">1 时</asp:ListItem>
							<asp:ListItem value="2">2 时</asp:ListItem>
							<asp:ListItem value="3">3 时</asp:ListItem>
							<asp:ListItem value="4">4 时</asp:ListItem>
							<asp:ListItem value="5">5 时</asp:ListItem>
							<asp:ListItem value="6">6 时</asp:ListItem>
							<asp:ListItem value="7">7 时</asp:ListItem>
							<asp:ListItem value="8">8 时</asp:ListItem>
							<asp:ListItem value="9">9 时</asp:ListItem>
							<asp:ListItem value="10">10 时</asp:ListItem>
							<asp:ListItem value="11">11 时</asp:ListItem>
							<asp:ListItem value="12">12 时</asp:ListItem>
							<asp:ListItem value="13">13 时</asp:ListItem>
							<asp:ListItem value="14">14 时</asp:ListItem>
							<asp:ListItem value="15">15 时</asp:ListItem>
							<asp:ListItem value="16">16 时</asp:ListItem>
							<asp:ListItem value="17">17 时</asp:ListItem>
							<asp:ListItem value="18">18 时</asp:ListItem>
							<asp:ListItem value="19">19 时</asp:ListItem>
							<asp:ListItem value="20">20 时</asp:ListItem>
							<asp:ListItem value="21">21 时</asp:ListItem>
							<asp:ListItem value="22">22 时</asp:ListItem>
							<asp:ListItem value="23">23 时</asp:ListItem>
						</asp:DropDownList>
						<asp:TextBox id="fromMinute" value="00" width="25" maxlength="2" runat="server" />
						&nbsp; — &nbsp; 
						<NetRadio:CalendarBox id="toDate" width="80" runat="server" />
						<asp:DropDownList id="toHour" runat="server">
							<asp:ListItem value="0">0 时</asp:ListItem>
							<asp:ListItem value="1">1 时</asp:ListItem>
							<asp:ListItem value="2">2 时</asp:ListItem>
							<asp:ListItem value="3">3 时</asp:ListItem>
							<asp:ListItem value="4">4 时</asp:ListItem>
							<asp:ListItem value="5">5 时</asp:ListItem>
							<asp:ListItem value="6">6 时</asp:ListItem>
							<asp:ListItem value="7">7 时</asp:ListItem>
							<asp:ListItem value="8">8 时</asp:ListItem>
							<asp:ListItem value="9">9 时</asp:ListItem>
							<asp:ListItem value="10">10 时</asp:ListItem>
							<asp:ListItem value="11">11 时</asp:ListItem>
							<asp:ListItem value="12">12 时</asp:ListItem>
							<asp:ListItem value="13">13 时</asp:ListItem>
							<asp:ListItem value="14">14 时</asp:ListItem>
							<asp:ListItem value="15">15 时</asp:ListItem>
							<asp:ListItem value="16">16 时</asp:ListItem>
							<asp:ListItem value="17">17 时</asp:ListItem>
							<asp:ListItem value="18">18 时</asp:ListItem>
							<asp:ListItem value="19">19 时</asp:ListItem>
							<asp:ListItem value="20">20 时</asp:ListItem>
							<asp:ListItem value="21">21 时</asp:ListItem>
							<asp:ListItem value="22">22 时</asp:ListItem>
							<asp:ListItem value="23">23 时</asp:ListItem>
						</asp:DropDownList>
						<asp:TextBox id="toMinute" value="00" width="25" maxlength="2" runat="server" /></td>
					</tr>
					<tr>
					<td></td>
					<td  >
                        <asp:Button ID="FindCount" runat="server" Text="查看统计" Width="87px" onclick="FindCount_Click" 
                             />
        		</table>
			</blockquote>
</asp:Content>
