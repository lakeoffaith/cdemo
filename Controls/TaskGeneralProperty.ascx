<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaskGeneralProperty.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__TaskGeneralProperty" %>

<tr class="bg3">
	<td></td>
	<td class="italic t3">任务命令参数</td>
</tr>
<tr>
	<td>首次执行时间</td>
	<td><NetRadio:CalendarBox id="executeDate" width="80" runat="server" />
		<asp:DropDownList id="executeHour" runat="server">
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
		<asp:TextBox id="executeMinute" value="00" width="25" maxlength="2" runat="server" />
		 分</td>
</tr>
<tr>
	<td>重复执行周期</td>
	<td><asp:DropDownList id="period" width="100" runat="server">
			<asp:ListItem value="0">只执行一次</asp:ListItem>
			<asp:ListItem value="1">每小时</asp:ListItem>
			<asp:ListItem value="2">每天</asp:ListItem>
			<asp:ListItem value="3">每星期</asp:ListItem>
			<asp:ListItem value="4">每月</asp:ListItem>
		</asp:DropDownList></td>
</tr>
<tr>
	<td>备注说明</td>
	<td><asp:TextBox id="memo" textmode="multiline" width="360" rows="5" runat="server" /></td>
</tr>