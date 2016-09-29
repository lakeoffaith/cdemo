<%@ Page Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="ReportAreaInOut.aspx.cs" Inherits="NetRadio.LocatingMonitor.Report.__ReportAreaInOut" %>
<%@ Register TagPrefix="NetRadio" TagName="ObjecetNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ObjecetNavigator ID="objectNavigator" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <asp:Button ID="btnExport" runat="server" Text="导出" OnClick="btnExport_Click" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lbltime" ForeColor="DarkBlue" Font-Bold="False" Width="100%" runat="server"
                    Style="margin-right: 237px" Font-Size="Smaller"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <NetRadio:SmartLabel ID="lblMessage" CssClass="fontStyle" Text="" runat="server" />
            </td>
        </tr>
    </table>
    
    <NetRadio:CollapseView ID="listPanel" runat="server"><Content>
    
    <table id="tableExcel" class="grid alternate fixed" >
    
    <thead><th width="16"></th><th width="120">人员姓名</th><th width="180">进入区域时间</th><th width="180">消失或离开时间</th><th width="180">停留时间</th></thead>
    
    <asp:Repeater id="list" onItemCreated="list_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:Img id="icon" runat="server" /></td>
										<td><NetRadio:Anchor id="tagName" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="inTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
										<td><NetRadio:DateTimeLabel id="outTime" showAsTimeSpan="false" format="yyyy-M-d H:mm:ss" runat="server" /></td>
									    <td><NetRadio:SmartLabel id="duration" runat="server"></NetRadio:SmartLabel></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
						</table>
					</Content>
    </NetRadio:CollapseView>    
</asp:Content>
