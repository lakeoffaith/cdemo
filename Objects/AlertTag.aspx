<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="AlertTag.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Objects.__AlertTag" %>

<%@ Register TagPrefix="NetRadio" TagName="TagSelector" Src="~/Controls/TagSelector.ascx" %>
<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjectNavigator ID="objectNavigator" runat="server" />
    <div class="tabitem">
        <NetRadio:CollapseView ID="CollapseView1" runat="server">
				<Header>
					<Caption>添加受虐报警标签</Caption>
				</Header>
				<Content>
				    <table class="grid fixed">
						<tr>
							<td width="160">所在监房</td>
							<td><asp:DropDownList id="newJailRoom" width="135" runat="server" /></td>
						</tr>
						<tr>
						    <td>标签名称</td>
						    <td><asp:TextBox id="tagName" width="135" runat="server"></asp:TextBox> &nbsp; <span class="t2">定义标签的名称，便于区分</span></td>
						</tr>
						<tr>
							<td>选择标签</td>
							<td><NetRadio:TagSelector id="tagSelector" AllowedSelectCount="1" runat="server" />
								<div class="lineBreak"></div>
								<span class="t3">从本系统中选出放置在该监舍的受虐报警标签</span>
							</td>
						</tr>
						<tr>
						    <td colspan="2" align="center"><asp:Button id="submit" text="确定" runat="server" onClick="submit_Click" /></td>
						</tr>
					</table>
				</Content>
                <Footer />
        </NetRadio:CollapseView>
        <NetRadio:CollapseView ID="edit" runat="server">
				<Header>
					<Caption>已添加标签</Caption>
				</Header>
				<Content>
				    <table class="grid fixed">
				        <tr>
				            <td>所处监舍</td>
				            <td>标签名称</td>
				            <td>标签Mac</td>
				            <td>添加时间</td>
				        </tr>
				        <asp:Repeater id="list" runat="server" OnItemDataBound="list_ItemDataBound">
					        <ItemTemplate>
					        <tr>
					            <td><NetRadio:Anchor id="isChecked" runat="server" /> <NetRadio:SmartLabel id="jailRoom" runat="server" /></td>
					            <td><NetRadio:SmartLabel id="tagHostName" runat="server" /></td>
					            <td><NetRadio:SmartLabel id="tagMac" runat="server" /></td>
					            <td><NetRadio:DateTimeLabel id="writeTime" runat="server" /></td>
					        </tr>
					        </ItemTemplate>
					    </asp:Repeater>
					    <tr><td> 
					    <asp:Button id="setDelete" text="删除" width="90" onclick="setDelete_Click" runat="server" />
					    </td><td colspan="3"></td>
					    </tr>
					</table>
				</Content>
                <Footer />
        </NetRadio:CollapseView>
    </div>
</asp:Content>
