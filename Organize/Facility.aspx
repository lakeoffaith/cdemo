<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Facility.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__Facility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:CollapseView ID="wrapper" runat="server">
				<Header>
					<Caption>场所/地图结构信息</Caption>
				</Header>
				<Content>
					<table cellpadding="8" cellspacing="0">
						<tr>
							<td width="235" class="bg3" valign="top">
								<NetRadio:Img id="companyIcon" align="absmiddle" class="nodeIcon" runat="server" />
								<span class="bigger bold t1">场所结构</span>
								&nbsp; &nbsp;
								<%--<NetRadio:Anchor runat="server" id="addNew" Text="新增" Href="Facility.aspx?action=addnew" class="bullet_add t3" />--%>
								<style type="text/css">
								    <!
								    -- .treeBlock
								    {
								        margin: 5px 0 5px 14px;
								    }
								    .nodeIcon
								    {
								        margin-right: 4px;
								    }
								     //
								    -- ></style>
								<div class="treeBlock">
									<asp:Repeater id="facilityTreeRepeater" runat="server" />
								</div>
							</td>
							<td style="padding:10px 20px">
								<table class="grid fixed">
									<tr>
										<td width="160">编辑对象:</td>
										<td><asp:TextBox id="facilityId" width="55" readonly="true" runat="server" /></td>
									</tr>
									<tr>
										<td style="height:15px"></td>
										<td></td>
									</tr>
									<tr>
										<td>场所名称: </td>
										<td><asp:TextBox id="facilityName" width="205" readonly="true" maxlength="50" runat="server" /> <span class="t2">*</span></td>
									</tr>
									<tr>
										<td>所属上层:</td>
										<td><NetRadio:FacilityDropList id="parentList" readonly="true" defaultText="无" style="width:211px" runat="server" /></td>
									</tr>
									<%--
									<tr>
										<td></td>
										<td><NetRadio:Remind id="feedbacks" style="width:275px" autohideduration="10" runat="server" /></td>
									</tr>
									<tr>
										<td></td>
										<td><asp:Button id="save" text="保存" onclick="save_Click" runat="server" /></td>
									</tr>
									--%>
								</table>
								<div class="vspace"></div>
								<div class="vspace"></div>
								<div class="vspace"></div>
								<%--<NetRadio:Anchor id="linkOfAppendChild" class="bullet_add" text="点击这里添加子节点" runat="server" />--%>
								<asp:LinkButton id="removeCache" text="点此更新缓存" class="underline" onclick="removeCache_Click" runat="server" />
							</td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
