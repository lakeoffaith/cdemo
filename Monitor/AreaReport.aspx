<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="AreaReport.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Monitor.__AreaReport" %>

<%@ Register TagPrefix="NetRadio" TagName="AreaNavigator" Src="~/Controls/AreaNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <NetRadio:CollapseView ID="collapseView" runat="server">
			<Content>
			<NetRadio:AreaNavigator id="AreaNavigator" runat="server" /><br />
				<table class="grid alternate fixed">
					<thead>
						<th width="140">监房</th>
						<th width="90">在押人数</th>
						<th width="90">监控人数</th>
						<th width="90">在监人数</th>
						<th>外出人员</th>
						<th width="140">病犯</th>
					</thead>
					<asp:Repeater id="list" onitemcreated="list_ItemCreated" runat="server" 
        onitemdatabound="list_ItemDataBound">
						<ItemTemplate>
							<tr>
								<td><NetRadio:SmartLabel id="areaName" class="bold" runat="server" /></td>
								<td><NetRadio:NumericLabel id="quota" runat="server" /> 人</td>
								<td><NetRadio:NumericLabel id="bindingCount" runat="server" /> 人</td>
								<td><NetRadio:NumericLabel id="currentCount" runat="server" /> 人</td>
								<td><NetRadio:SmartLabel id="expectedNames" runat="server" /></td>
							    <td><NetRadio:SmartLabel id="illedNames" runat="server" /></td>
							</tr>
						</ItemTemplate>
						<FooterTemplate>
						<tr style=" color:Red;">
								<td>合计</td>
								<td><NetRadio:NumericLabel id="quota0" runat="server" /> 人</td>
								<td><NetRadio:NumericLabel id="bindingCount0" runat="server" /> 人</td>
								<td><NetRadio:NumericLabel id="currentCount0" runat="server" /> 人</td>
								<td>&nbsp;</td>
							    <td>&nbsp;</td>
							</tr>
						</FooterTemplate>
					</asp:Repeater>
				</table>
			</Content>
    </NetRadio:CollapseView>
</asp:Content>
