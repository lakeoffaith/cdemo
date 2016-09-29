<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="NetRadio.LocatingMonitor.Member.__ChangePassword"%>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server"><NetRadio:CollapseView id="collapseView" runat="server">
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160">我登录的用户</td>
							<td><NetRadio:SmartLabel id="loginName" class="bold" runat="server" /></td>
						</tr>
						<tr>
							<td>原密码</td>
							<td><asp:TextBox id="password" textmode="Password" width="165" runat="server" />
							&nbsp;
							<span class="t3">请输入原密码</span></td>
						</tr>
						<tr>
							<td>新密码</td>
							<td><asp:TextBox textmode="Password" id="newPassword" width="165" maxlength="16" runat="server" />
							&nbsp;
							<span class="t3">请输入您需要修改的新密码</span></td>
						</tr>
						<tr>
							<td >密码确认</td>
							<td><asp:TextBox textmode="Password" id="confirmPassword" width="165" maxlength="16" runat="server" />
							&nbsp;
							<span class="t3">请重复输入一遍新密码进行确认</span></td>
						</tr>
						<tr>
							<td></td>
							<td><NetRadio:Remind id="feedbacks" autohideduration="5" style="width:240px" runat="server" /></td>						
						</tr>
						<tr>
							<td></td>
							<td>
							    <asp:Button id="cancelChange" text="取消" onclick="cancelChange_Click" runat="server" />
							    <asp:Button id="submitPassword" text="确认" onclick="submitPassword_Click" runat="server" />
							</td>
						</tr>
						<tr>
						<td></td>
						<td><asp:Label id="messageLabel" text="" runat="server" ></asp:Label></td>
						</tr>
					</table>
				</Content>
			</NetRadio:CollapseView>
</asp:Content>
