<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="TagGroup.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__TagGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <%@ register tagprefix="NetRadio" tagname="SelectTagUser" src="~/Controls/SelectTagUser.ascx" %>
    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Header>
					<Caption>设置对象分组</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160">分组名称</td>
							<td><asp:TextBox id="groupName" width="160" maxlength="32" runat="server" />&nbsp; <span class="t2">*</span></td>
						</tr>
						<tr>
						<td>所属组</td>
						<td><asp:DropDownList ID="grouplist" width=80 runat="server" /></td>
						</tr>
						<tr>
							<td>描述</td>
							<td><asp:TextBox id="groupDescription" width="320" height="60" runat="server" /></td>
						</tr>
						<tr>
							<td>属于该组的对象</td>
							<td><NetRadio:SelectTagUser id="tagSelector" runat="server" /></td>
						</tr>
						
						<tr>
							<td></td>
							<td><NetRadio:Remind id="feedbacks" style="width:275px" autohideduration="10" runat="server" /></td>
						</tr>
						<tr>
							<td></td>
							<td><asp:Button id="save"  class="button" Text="确定" onclick="save_click" runat="server" />
								<input type="reset" value="返回" class="button" onclick="javascript:location.href='TagGroupList.aspx';" /></td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
