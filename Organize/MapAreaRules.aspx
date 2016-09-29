<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="MapAreaRules.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__MapAreaRules" %>

<%@ Register TagPrefix="NetRadio" TagName="SelectTagUser" Src="~/Controls/SelectTagUser.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Header>
					<Caption>区域信息及警告条件设定</Caption>
				</Header>
				<Content>
					<table class="fixed grid">
						<tr>
							<td width="160">区域名称</td>
							<td class="bold"><NetRadio:SmartLabel id="areaName" runat="server" /></td>
						</tr>
						<tr>
							<td>区域所在地图</td>
							<td><NetRadio:SmartLabel id="facilityName" runat="server" /></td>
						</tr>
						<tr class="bg3">
							<td>已设置的警告条件 (<NetRadio:NumericLabel id="ruleCount" class="t2" runat="server" />)</td>
							<td></td>
						</tr>
						<asp:Repeater id="ruleList" onitemcreated="ruleList_ItemCreated" runat="server">
							<ItemTemplate>
								<tr>
									<td valign="baseline" align="right" style="padding-right:20px"><NetRadio:Anchor id="deleteRule" text="删除" class="underline" runat="server" /></td>
									<td><NetRadio:SmartLabel id="ruleDescription" runat="server" /><br /><asp:Label ID="labPeople" style="color:#666666;" runat="server"></asp:Label></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <div class="vspace">
    </div>
    <NetRadio:CollapseView ID="collapseView2" runat="server">
				<Header>
					<Caption>设置新警告条件</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160">选择使用者</td>
							<td>
								<asp:RadioButtonList id="forAllTags" repeatlayout="flow" repeatdirection="horizontal" style="position:relative; left:-5px;" runat="server">
									<asp:ListItem value="1" onclick="javascirpt:$$('tagSelectorContainer').hide();" selected="true">所有</asp:ListItem>
									<asp:ListItem value="0" onclick="javascirpt:$$('tagSelectorContainer').display();" style="margin-left:20px">指定</asp:ListItem>
								</asp:RadioButtonList>
								<NetRadio:Div id="tagSelectorContainer" signclientid="true" style="margin:6px;" runat="server">
									<div class="lineBreak"></div>
									<NetRadio:SelectTagUser id="tagSelector" SelectedTagsVisible="true" runat="server"  TitleRight="◆◆ 已选对象"/>
								</NetRadio:Div>
							</td>
						</tr>
						<tr>
							<td>选择触发条件</td>
							<td>
								<asp:DropDownList id="areaEventType" width="180" runat="server">
									<asp:ListItem value="0">标签进入或位于区域内</asp:ListItem>
									<asp:ListItem value="1">标签离开区域或位于区域外</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
					 
						<tr>
							<td></td>
							<td><asp:Button id="saveNewRule" text="确认" onclick="saveNewRule_Click" runat="server" />
								<input type="reset" value="返回" class="button" onclick="javascript:location.href='MapAreaList.aspx';" /></td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
