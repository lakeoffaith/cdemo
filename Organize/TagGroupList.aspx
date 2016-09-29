<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="TagGroupList.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__TagGroupList" %>

<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjectNavigator runat="server" ID="objectNavigator" /><br />
    <NetRadio:collapseview id="collapseView" runat="server">
				<Header>
					<Caption>对象分组列表</Caption>
					<Subcaption><a href="TagGroup.aspx?action=addnew" class="bullet_add">新建对象分组</a></Subcaption>
				</Header>
				<Content>
					<table class="grid alternate fixed">
						<thead>
							<th width="120">分组名称</th>
							<th width="100">对象数量</th>
							<th>描述</th>
							<th width="100" style="text-align:center">操作</th>
						</thead>
						<asp:Repeater id="groupList"  onitemcreated="groupList_ItemCreated"  runat="server">
							<ItemTemplate>
								<tr>
									<td><NetRadio:Anchor id="groupName" runat="server" /></td>
									<td><NetRadio:NumericLabel id="tagCount" runat="server" /> <span class="t3">个</span></td>
									<td><NetRadio:SmartLabel id="groupDescription" runat="server" /></td>
									<td align="center">
										<NetRadio:Anchor id="detail" text="编辑" runat="server" /><span class="separator"> | </span>
										<NetRadio:Anchor id="delete" text="删除" runat="server" />
									</td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
					</table>
				</Content>
    </NetRadio:collapseview>
</asp:Content>
