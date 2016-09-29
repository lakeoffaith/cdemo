<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapAreaGroup0.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__MapAreaGroup0" %>
<form id="form1" runat="server">
<NetRadio:CollapseView ID="collapseView" runat="server">
        <content>
				<input id="hidCurrentGroupID" type="hidden" />
				<div id="DivAreaGroup" class="floatLayer" style="width:465px; display:none">
				<table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td align="left">&nbsp;&nbsp;◆◆ 请选择该组包含的区域</td>
                    <td align="center">&nbsp;</td>
                    <td align="center">&nbsp;</td>
                  </tr>
                  <tr>
                    <td colspan="3" height="20"></td>
                  </tr>
                  <tr>
                    <td colspan="3" id="areas"></td>
                  </tr>
                   <tr>
                    <td colspan="3" height="20"></td>
                  </tr>
                  <tr>
                    <td id="loading">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td align="right">
                    <input id="butOK" type="button" value=" 确定 " onclick="return butOK_click()" />
                    <input id="butCancel" type="button" value=" 关闭 "  style="margin-left: 20px;" onclick="return butCancel_click()"/>
                    </td>
                  </tr>
                </table>  				
				</div>
					<table class="grid fixed">
						<thead>
							<th width="160"> ID编号</th>
							<th>区域分组名称</th>
							<th width="160">设置分组包含区域</th>
						</thead>
						 <asp:Repeater id="areaGroupList" runat="server" onitemdatabound="areaGroupList_ItemDataBound" >
							<ItemTemplate>
								<tr>
                                    <td>
                                        <%# Eval("GroupID")%>
                                    </td>
                                    <td>
                                        <%#  Eval("GroupName")%><asp:Label ID="labAreaNames" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <span style="cursor:pointer; color:Black;" onclick="return  __show('<%# Eval("GroupID")%>')">设置</span>
                                    </td>
                                </tr>
							</ItemTemplate>
						</asp:Repeater>						
					</table>									
				</content>
</NetRadio:CollapseView>
</form>
