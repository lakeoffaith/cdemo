<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Member.__EditUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Header>
					<Caption>按照提示输入表单内容</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160">登录名</td>
							<td><asp:TextBox id="userName" width="165" runat="server" />
								&nbsp; <span class="t2" name="tishi" style="display:none;">*</span>
								<NetRadio:SmartLabel id="readonlyMark" class="t3" runat="server">不允许修改</NetRadio:SmartLabel></td>
						</tr>
						<tr>
							<td>密码</td>
							<td><asp:TextBox textmode="Password" id="password" width="165" maxlength="16" runat="server" />
								&nbsp; <span class="t2" name="tishi" style="display:none;">*</span>
								<NetRadio:SmartLabel id="passwordHint" class="t3" runat="server">不修改请保留空白</NetRadio:SmartLabel>
							</td>
						</tr>
						<tr>
							<td>密码确认</td>
							<td><asp:TextBox textmode="Password" id="confirmationPassword" width="165" maxlength="16" runat="server" />
							&nbsp; <span class="t2" name="tishi" style="display:none;">*</span></td>
						</tr>
						<tr id="Tr1" visible="true" runat="server">
							<td>角色</td>
							<td><asp:RadioButtonList id="userRole" repeatdirection="horizontal" repeatlayout="flow" runat="server">
									<asp:ListItem value="1" style="margin-right:24px">普通用户</asp:ListItem>
									<asp:ListItem value="2" visible="false">管理员</asp:ListItem>
								</asp:RadioButtonList></td>
						</tr>
						<tr>
							<td></td>
							<td><NetRadio:Remind id="feedbacks" autohideduration="10" style="width:240px" runat="server" /></td>
						</tr>
						<tr>
							<td></td>
							<td>
								<asp:Button id="submit" text="修改" onclick="submit_Click" runat="server" />
								<input type="button" value="返回" class="button" onclick="javascript:location.href='UserList.aspx';" />
							</td>
						</tr>
					</table>
					<script>
					    if(request("action")=="addnew")
					    {
					         var ts=$n("span","tishi");
					         for( var i=0;i< ts.length;i++)
			                 {
			                   ts[i].style.display="inline";
			                 }
					    }
					</script>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
