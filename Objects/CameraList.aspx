<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="CameraList.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Objects.__CameraList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:TabView ID="tabView" SelectedIndex="0" runat="server">
			    <Tab label="AP定位器状态"  href="/Monitor/APList.aspx" />
				<Tab label="定位器记录" href="/History/APLocatorLog.aspx" />
				<Tab label="受虐标签设置" href="/Objects/AlertTag.aspx" />
				<Tab label="定位设置" href="/Settings/LocatingManager.aspx" />
			    <Tab label="AP定位器状态" >
					<NetRadio:CollapseView id="searchResult" runat="server">
						<Header skinId="header_empty"></Header>
						<Content>
							<table class="grid alternate fixed">
								<thead>
									<th><NetRadio:SortButton id="apNameSorter" text="AP名称" sortKey="APName" onclick="sorter_Click" runat="server" /></th>
									<th>MAC地址</th>
									<th>SSID</th>
									<th>局域网IP</th>
									<th>MAC&amp;IP匹配</th>
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
											<td><NetRadio:SmartLabel id="ipMacMatched" runat="server" /></td>
											<td><NetRadio:SmartLabel id="apLocateEnabled" runat="server" /></td>
											<td><NetRadio:DateTimeLabel id="updateTime" showAsTimeSpan="true" format="yyyy-M-d H:mm:ss" runat="server" /></td>
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
				</Tab>
    </NetRadio:TabView>
</asp:Content>
