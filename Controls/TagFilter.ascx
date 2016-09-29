<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagFilter.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__TagFilter" %>

<div style="margin:10px auto">
	<div class="fl"><NetRadio:SmartLabel id="condtionDescription" runat="server" /></div>
	<div class="fr"><a href="javascript:callFilter();">设置查询条件</a></div>
	<div class="clear"></div>						
</div>

<div id="filterForm" class="floatLayer" style="display:none">
	<table width="435" cellpadding="0" cellspacing="0">
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
					<tr>
						<td>选择分组:</td>
						<td><NetRadio:TagGroupSelector id="groupSelector" width="145" runat="server" /></td>
					</tr>
					<tr id="facilityFilterRow" runat="server">
						<td>当前位于:</td>
						<td><NetRadio:FacilityMapDropList id="facilityMap" runat="server" /></td>
					</tr>
					<tr>
						<td>正在定位:</td>
						<td><asp:CheckBox id="locatingOnly" text="仅显示正在定位的使用者" runat="server" /></td>
					</tr>
					<tr>
						<td>处于告警状态:</td>
						<td nowrap="nowrap">
							<style type="text/css">
							<!--
							#eventOptions li {
								width: 33%;
								float: left;
								list-style: none;
							}
							// -->
							</style>
							<ul id="eventOptions">
								<li><asp:CheckBox id="absentOnly" text="消失" runat="server" /></li>
								<li><asp:CheckBox id="lowerBatteryOnly" text="低电量" runat="server" /></li>
								<li><asp:CheckBox id="areaEventOnly" text="区域告警" runat="server" /></li>
								<li><asp:CheckBox id="buttonPressedOnly" text="触发按钮" runat="server" /></li>
								<li><asp:CheckBox id="wristletBrokenOnly" text="腕带断开" runat="server" /></li>
							</ul>
						</td>
					</tr>
					<tr>
						<td>排序方式</td>
						<td><asp:DropDownList id="sortField" runat="server">
								<asp:ListItem value="TagName">名称</asp:ListItem>
								<asp:ListItem value="Position">当前位置</asp:ListItem>
								<asp:ListItem value="UpdateTime">状态更新时间</asp:ListItem>
							</asp:DropDownList>
							<asp:DropDownList id="sortDirection" width="70" runat="server">
								<asp:ListItem value="0">顺序</asp:ListItem>
								<asp:ListItem value="1">倒序</asp:ListItem>
							</asp:DropDownList>
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