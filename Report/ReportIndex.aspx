<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ReportIndex.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Report.__ReportIndex" %>

<%@ Register TagPrefix="NetRadio" TagName="ObjecetNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjecetNavigator ID="objectNavigator" runat="server" />
    <NetRadio:CollapseView ID="collapseView1" runat="server">
				
				<Content>
					<blockquote>
					<span class="bigger bold"><NetRadio:SmartLabel id="StatTime" runat="server" /> &nbsp;&nbsp;&nbsp; 总人数：
						<NetRadio:SmartLabel id="TotalCount" runat="server" />
						&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp&nbsp;&nbsp;&nbsp;
				    </span>
				    <a href="/Report/ReportIndex.aspx?pid=14"> 刷新</a>
						<div class="lineBreak"></div>
					
						<table class="grid alternate fixed">
							<thead>
								<th width="100">地图</th>
								<th width="80">人数</th>
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:SmartLabel id="facilityName" runat="server" /></td>
										<td><NetRadio:SmartLabel id="headCount" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
						</table>
					</blockquote>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
