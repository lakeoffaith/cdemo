<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="PoliceAreaInOut.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Report.__PoliceAreaInOut" %>
<%@ Register TagPrefix="NetRadio" TagName="SelectTagUser" Src="~/Controls/SelectTagUser.ascx" %>
<%@ Register TagPrefix="NetRadio" TagName="ReportNavigator" Src="~/Controls/ReportNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <style>
        .fontStyle
        {
            color: Red;
        }
    </style>
    <NetRadio:ReportNavigator id="reportNavigator" runat="server" />
    <blockquote>
        <table class="fixed grid">
        <tr >
						<td width="130">选择区域<br /><asp:Button ID="btnCheckState" OnClick="btnCheckState_Click"  runat="server"></asp:Button></td>
						<td ><asp:CheckBoxList id="chklArea" RepeatColumns="7"  RepeatDirection="Horizontal"  runat="server"></asp:CheckBoxList></td>
					</tr>
            <tr width="130">
                <td>
                    选择人员
                </td>
                <td>
                    <NetRadio:SelectTagUser id="tagUserSelector" runat="server"  TitleRight="◆◆ 已选对象"/>
                </td>
            </tr>
            <tr>
                <td>
                    选择时间范围
                </td>
                <td>
                    <NetRadio:CalendarBox ID="fromDate" Width="80" runat="server" />
                    <asp:DropDownList ID="fromHour" runat="server">
                        <asp:ListItem Value="0">0 时</asp:ListItem>
                        <asp:ListItem Value="1">1 时</asp:ListItem>
                        <asp:ListItem Value="2">2 时</asp:ListItem>
                        <asp:ListItem Value="3">3 时</asp:ListItem>
                        <asp:ListItem Value="4">4 时</asp:ListItem>
                        <asp:ListItem Value="5">5 时</asp:ListItem>
                        <asp:ListItem Value="6">6 时</asp:ListItem>
                        <asp:ListItem Value="7">7 时</asp:ListItem>
                        <asp:ListItem Value="8">8 时</asp:ListItem>
                        <asp:ListItem Value="9">9 时</asp:ListItem>
                        <asp:ListItem Value="10">10 时</asp:ListItem>
                        <asp:ListItem Value="11">11 时</asp:ListItem>
                        <asp:ListItem Value="12">12 时</asp:ListItem>
                        <asp:ListItem Value="13">13 时</asp:ListItem>
                        <asp:ListItem Value="14">14 时</asp:ListItem>
                        <asp:ListItem Value="15">15 时</asp:ListItem>
                        <asp:ListItem Value="16">16 时</asp:ListItem>
                        <asp:ListItem Value="17">17 时</asp:ListItem>
                        <asp:ListItem Value="18">18 时</asp:ListItem>
                        <asp:ListItem Value="19">19 时</asp:ListItem>
                        <asp:ListItem Value="20">20 时</asp:ListItem>
                        <asp:ListItem Value="21">21 时</asp:ListItem>
                        <asp:ListItem Value="22">22 时</asp:ListItem>
                        <asp:ListItem Value="23">23 时</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="fromMinute" value="00" Width="25" MaxLength="2" runat="server"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"/>
                    &nbsp; — &nbsp;
                    <NetRadio:CalendarBox ID="toDate" Width="80" runat="server" />
                    <asp:DropDownList ID="toHour" runat="server">
                        <asp:ListItem Value="0">0 时</asp:ListItem>
                        <asp:ListItem Value="1">1 时</asp:ListItem>
                        <asp:ListItem Value="2">2 时</asp:ListItem>
                        <asp:ListItem Value="3">3 时</asp:ListItem>
                        <asp:ListItem Value="4">4 时</asp:ListItem>
                        <asp:ListItem Value="5">5 时</asp:ListItem>
                        <asp:ListItem Value="6">6 时</asp:ListItem>
                        <asp:ListItem Value="7">7 时</asp:ListItem>
                        <asp:ListItem Value="8">8 时</asp:ListItem>
                        <asp:ListItem Value="9">9 时</asp:ListItem>
                        <asp:ListItem Value="10">10 时</asp:ListItem>
                        <asp:ListItem Value="11">11 时</asp:ListItem>
                        <asp:ListItem Value="12">12 时</asp:ListItem>
                        <asp:ListItem Value="13">13 时</asp:ListItem>
                        <asp:ListItem Value="14">14 时</asp:ListItem>
                        <asp:ListItem Value="15">15 时</asp:ListItem>
                        <asp:ListItem Value="16">16 时</asp:ListItem>
                        <asp:ListItem Value="17">17 时</asp:ListItem>
                        <asp:ListItem Value="18">18 时</asp:ListItem>
                        <asp:ListItem Value="19">19 时</asp:ListItem>
                        <asp:ListItem Value="20">20 时</asp:ListItem>
                        <asp:ListItem Value="21">21 时</asp:ListItem>
                        <asp:ListItem Value="22">22 时</asp:ListItem>
                        <asp:ListItem Value="23">23 时</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="toMinute" value="00" Width="25" MaxLength="2" runat="server"  onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:RadioButtonList ID="radioButtonMode" RepeatLayout="flow" RepeatDirection="horizontal"
                        runat="server">
                        <asp:ListItem Value="1" Selected="true">合计停留时间</asp:ListItem>
                        <asp:ListItem Value="2">详细进出时间</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Button ID="BtnStat" Text="查看" OnCommand="BtnStat_Click" runat="server" />
                </td>
                <!--<td><input type="button" value="查看统计" class="button" onclick="javascript:viewPoliceAreaInOutReport();" /></td>-->
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <NetRadio:SmartLabel ID="lblMessage" CssClass="fontStyle" Text="" runat="server" />
                </td>
            </tr>
        </table>
        <br />
       <%-- <NetRadio:CollapseView ID="listPanel" runat="server">
					<Header skinId="header_empty" />
					<Content>
						<table class="grid alternate fixed">
							<thead>
							    <th width="16"></th>
								<th width="120">名称</th>
								<th width="180">进入区域时间</th>
								<th width="180">消失或离开时间</th>
								<th width="180">停留时间</th>
							</thead>
							<asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Img id="icon" runat="server" /></td>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="inTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="outTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
									    <td><NetRadio:SmartLabel id="duration" runat="server"></NetRadio:SmartLabel></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="5"><NetRadio:Pagination id="p"  Visible=false pageSize="20" mode="Redirect" class="p_left" runat="server" /></td>
							</tr>
						</table>
					</Content>
        </NetRadio:CollapseView>--%>
    </blockquote>
</asp:Content>
