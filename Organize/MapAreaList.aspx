<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="MapAreaList.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__MapAreaList" %>
    
<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjectNavigator ID="objectNavigator" runat="server" />
    <NetRadio:CollapseView ID="filterView" runat="server">
				<Content>
					<div class="bg3" style="padding:10px">
						<div>
							<NetRadio:FacilityMapDropList id="maps" DisplayTreeStyle="false" DefaultText="所有地图" style="width:180px" runat="server" />
							<asp:Button id="filterButton" text="筛选" onclick="filterButton_Click" runat="server" /> 
							&nbsp; &nbsp; &nbsp; &nbsp;
							<asp:LinkButton id="removeCache" text="点此更新缓存" class="underline" onclick="removeCache_Click" runat="server" />
						</div>
					</div>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Header>
					<Caption>区域列表</Caption>
				</Header>
				<Content>
					<table class="grid fixed">
						<thead>
							<th width="160">区域名称</th>
							<th>所属地图</th>
							<th width="100"></th>
						</thead>
						<asp:Repeater id="areaList" onitemcreated="areaList_ItemCreated" runat="server">
							<ItemTemplate>
								<tr>
									<td><NetRadio:Anchor id="areaName" runat="server" /></td>
									<td><NetRadio:SmartLabel id="facilityName" runat="server" /></td>
									<td><NetRadio:Anchor id="setRule" text="设置告警条件" runat="server" /></td>
								</tr>
							</ItemTemplate>
						</asp:Repeater>
						<tr>
							<td colspan="3"><NetRadio:Pagination id="p" mode="Postback" pagesize="50" class="p_left" onpageindexchanged="p_PageIndexChanged" runat="server" /></td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
