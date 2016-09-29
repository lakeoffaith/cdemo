<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagLogFilter.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__TagLogFilter" %>

<div style="margin:10px auto">
	<div class="fl"><input type="button" value="设置查询条件" class="button" style="width:120px" onclick="javascript:callFilter();" /></div>
	<div class="fr"><NetRadio:SmartLabel id="condtionDescription" runat="server" /></div>
	<div class="clear"></div>						
</div>

<div id="filterForm" class="floatLayer" style="cursor: pointer; display: none">
	<table width="425" cellpadding="0" cellspacing="0">
		<tr>
			<td class="t1 bold bigger">设置查询条件</td>
			<td align="right"><a href="javascript:cancelFilter();" class="underline">取消</a></td>
		</tr>
		<tr>
			<td colspan="2" style="padding: 15px 0">
				<table class="grid fixed">
					<tr>
						<td width="135">名称关键字:</td>
						<td><asp:TextBox id="tagName" width="145" runat="server" /> &nbsp; </td>
					</tr>
					<!--
					<tr>
						<td>选择组范围:</td>
						<td><NetRadio:TagGroupSelector id="groupSelector" width="145" runat="server" /></td>
					</tr>
					<tr id="facilityFilterRow" runat="server">
						<td>所属地图:</td>
						<td><NetRadio:FacilityMapDropList id="facilityMap" runat="server" /></td>
					</tr>
					-->
					<tr>
						<td>时间范围从:</td>
						<td>
							<NetRadio:CalendarBox id="fromDate" width="90" runat="server" />
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
							<asp:TextBox id="fromMinute" width="25" text="00" runat="server" /> 分
						</td>
					</tr>
					<tr>
						<td>时间范围至:</td>
						<td>
							<NetRadio:CalendarBox id="toDate" width="90" runat="server" EnableViewState="true"  />
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
								<asp:ListItem value="23" selected="true">23 时</asp:ListItem>
							</asp:DropDownList>
							<asp:TextBox id="toMinute" width="25" text="59" runat="server" /> 分</td>
						</td>
					</tr>
					<tr>
						<td></td>
						<td><asp:Button id="searchButton" text="提交查询" width="120" onclick="searchButton_click" runat="server" /></td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>