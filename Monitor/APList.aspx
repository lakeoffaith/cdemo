<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="APList.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__APList"%><%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server"> <NetRadio:ObjectNavigator id="objectNavigator" runat="server" />
                <div class="tabitem">
					<asp:ScriptManager id="clientScriptManager" runat="server" />
					<asp:UpdatePanel id="updatePanel" runat="server">
						<ContentTemplate>
						
							<NetRadio:CollapseView id="searchResult" runat="server">
								<Header skinId="header_empty"></Header>
								<Content>
									<table class="grid alternate fixed">
										<thead>
											<th><NetRadio:SortButton id="apNameSorter" text="AP名称" sortKey="APName" onclick="sorter_Click" runat="server" /></th>
											<th>MAC地址</th>
											<th>SSID</th>
											<th>局域网IP</th>
											<th>AP定位</th>
											<th><NetRadio:SortButton id="updateTimeSorter" text="更新时间" sortKey="UpdateTime" onclick="sorter_Click" runat="server" /></th>
										</thead>
										<asp:Repeater id="apList" onitemcreated="apList_ItemCreated" runat="server">
											<ItemTemplate>
												<tr>
													<td><NetRadio:Anchor id="apName" target="_blank" runat="server" /></td>
													<td><NetRadio:SmartLabel id="apMac" runat="server" /></td>
													<td><NetRadio:SmartLabel id="apSsid" runat="server" /></td>
													<td><NetRadio:SmartLabel id="apLanIP" runat="server" /></td>
													<td><NetRadio:SmartLabel id="apLocateEnabled" runat="server" /></td>
													<td><NetRadio:DateTimeLabel id="updateTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
												</tr>
											</ItemTemplate>
										</asp:Repeater>
										<tr>
											<td colspan="7"><NetRadio:Pagination id="p" mode="Redirect" pagesize="20" class="p_left" runat="server" /></td>
										</tr>
									</table>
								</Content>
								<Footer />
							</NetRadio:CollapseView>
							
						</ContentTemplate>
					</asp:UpdatePanel>
					</div>
</asp:Content>
