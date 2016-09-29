<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagUserList0.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.TagUsers.__TagUserList0" %>
<form id="form1" runat="server">

<script type="text/javascript" language="javascript" defer="defer">
 
</script>

<NetRadio:TabView ID="tabView" runat="server">
				<Tab id="tabItem" label="标签使用者列表">
					<div style="margin: 10px 15px 15px 15px">
						<b>符合条件:</b>
						&nbsp;
						名称:&nbsp; <asp:TextBox id="keyword" width="90" maxlength="8" runat="server" />
						&nbsp; &nbsp; &nbsp;
						编号:&nbsp; <asp:TextBox id="extandId" width="90" maxlength="8" runat="server" />
						&nbsp; &nbsp; &nbsp;
						
						<NetRadio:SmartLabel id="jailRoomCondition" runat="server">
							属于监房:&nbsp; <asp:DropDownList id="jailRoom" width="100" runat="server" />
							&nbsp; &nbsp; &nbsp;
						</NetRadio:SmartLabel>
						标签:&nbsp; <asp:RadioButtonList id="tagBinding" repeatlayout="flow" repeatdirection="horizontal" runat="server">
									<asp:ListItem value="0">不限</asp:ListItem>
									<asp:ListItem value="1" selected="true">已领用</asp:ListItem>
									<asp:ListItem value="2">未领用</asp:ListItem>
								</asp:RadioButtonList>
								&nbsp; &nbsp; &nbsp;
						在线:&nbsp;<asp:RadioButtonList id="tagOnline" repeatlayout="flow" repeatdirection="horizontal" runat="server">
									<asp:ListItem value="0">不限</asp:ListItem>
									<asp:ListItem value="1" selected="true">在线</asp:ListItem>
									<asp:ListItem value="2">不在线</asp:ListItem>
								</asp:RadioButtonList> 
						&nbsp; &nbsp; &nbsp;
						<input type="button" id="searchButton" value=" 查询 " onclick="return searchButton_click(1)" />
						<br />
						<NetRadio:Anchor id="addNew" class="bullet_add" runat="server" />
						<NetRadio:Anchor id="importUsers" class="bullet_add" runat="server" Href="javascript:importUsers_onclick()" Text="导入用户" />
					</div>
				</Tab>
</NetRadio:TabView>
</form>
