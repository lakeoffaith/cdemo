<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="TagPositionList.aspx.cs" Inherits="NetRadio.LocatingMonitor.TagUsers.__TagPositionList" %>
<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator"  Src="~/Controls/ObjectNavigator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

			<style type="text/css">
			.cell {
				float: left;
				margin:5px 15px 15px 15px;
			}
			.cell .photo {
				margin: 6px 0;
			}
			</style>
			<!-- Tab 导航 -->
			<NetRadio:ObjectNavigator id="ObjectNavigator" runat="server" />		
			<%--<NetRadio:TabView id="tabView" runat="server">
				<Tab id="tabItem" label="定点报警标签列表">--%>
					<div style="margin: 10px 15px 15px 15px">
						<b>符合条件:</b>
						&nbsp;
						名称:&nbsp; <asp:TextBox id="keyword" width="90" maxlength="8" runat="server" />
						&nbsp; &nbsp; &nbsp;
						<asp:Button id="searchButton" text="查询" onclick="searchButton_Click" runat="server" />
						<br />
						<NetRadio:Anchor id="addNew" class="bullet_add" runat="server" />
					</div>
				<table cellpadding="0" cellspacing="0" class="grid alternate fixed">
						<thead class="category">
							<th width="250"><NetRadio:SortButton id="hostNameSorter" text="名称" sortKey="HostName" onclick="sorter_Click" runat="server" />
											</th>
							<th style="text-align:center">绑定标签</th>
						</thead>
					<asp:Repeater id="list" onitemcreated="list_ItemCreated" runat="server">
						<ItemTemplate>
									<tr>
										<td><NetRadio:Anchor id="name" class="bold" target="_detail" runat="server" />
										</td>
										<td style="text-align:center">
											<NetRadio:SmartLabel id="mac" style="color:#990000" runat="server" />
										</td>
									</tr>
						</ItemTemplate>
					</asp:Repeater>
					</table>
					<div class="clear"></div>
					<div style="margin: 0 15px 0 15px">
						<NetRadio:Pagination id="p" mode="Postback" class="p_left" pagesize="30" onpageindexchanged="p_PageIndexChanged" runat="server" />
					</div>
<%--				</Tab>
			</NetRadio:TabView>--%>

</asp:Content>
