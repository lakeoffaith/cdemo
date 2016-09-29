<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="SystemSecurityLog.aspx.cs" Inherits="NetRadio.LocatingMonitor.History.__SystemSecurityLog"%>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">	
				<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator"  Src="~/Controls/ObjectNavigator.ascx" %>
<NetRadio:CollapseView id="collapseView" runat="server">
				<Content>
				    <NetRadio:ObjectNavigator id="ObjectNavigator" runat="server" />	
					<table class="grid alternate fixed">
						<thead>
							<th width="35"></th>
							<th width="140">操作者</th>
							<th>描述</th>
<%--							<th width="80">对象</th>
--%>							<th width="130">标签ID</th>
							<th width="130">时间</th>
						</thead>
						<asp:Repeater id="repeater" onitemcreated="repeater_ItemCreated" runat="server">
							<ItemTemplate>
								<tr id="tr" runat="server">
									<td align="center"><NetRadio:SmartLabel id="selection" runat="server" /></td>
									<td><NetRadio:SmartLabel id="userName" target="_blank" runat="server" /></td>
									<td><NetRadio:SmartLabel id="description" runat="server" /></td>
<%--									<td><NetRadio:SmartLabel id="hostName" runat="server" /></td>
--%>									<td><NetRadio:SmartLabel id="tagMac" runat="server" /></td>
									<td><NetRadio:DateTimeLabel id="writeTime" format="yyyy-M-d H:mm:ss" showastimespan="false" runat="server" /></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						<!--
						<tr id="deleteButtonRow" runat="server">
							<td align="center"><input type="checkbox" onclick="javascript:CheckboxUtil.checkAll(this, 'selection');" title="选中所有列出的项" runat="server" /></td>
							<td colspan="5"><asp:Button id="submitDelete" text="删除" onclick="submitDelete_Click" runat="server" /> &nbsp; <span class="t3" style="position:relative; top:-2px">删除选中的行</span></td>
						</tr>
						-->
						<tr>
							<td colspan="5"><NetRadio:Pagination id="p" mode="PostBack" pagesize="50" class="p_left" onpageindexchanged="p_PageIndexChanged" runat="server" /></td>
						</tr>
					</table>
				</Content>
				<Footer />
			</NetRadio:CollapseView>
			
</asp:Content>
