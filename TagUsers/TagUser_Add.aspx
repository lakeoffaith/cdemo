<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="TagUser_Add.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.TagUsers.__TagUser_Add" %>

<%@ Register TagPrefix="NetRadio" TagName="TagSelector" Src="~/Controls/TagSelector.ascx" %>
<%@ Register Src="../Controls/SelectTag.ascx" TagName="SelectTag" TagPrefix="uc1" %>
<%@ Register Src="../Controls/SelectStrongestRssiTag.ascx" TagName="SelectStrongestRssiTag"
    TagPrefix="uc2" %>
    <%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">

    <script>
   function page_Load()
    {
    <% if (NetRadio.Business.BusSystemConfig.IsAutoSelectStrongestRssiTag()==false)
    {
    %>
            SelectTag.FunctionNo="01";
//		    SelectTag.SelectedTagIDs=tagId;
//            SelectTag.UserID=userID;
//            SelectTag.RegFunction=function(){searchButton_click(currentPageIndex);	};
		    SelectTag.SetTip();
	<% 
	}
    %>
    }
    </script>
    <NetRadio:ObjectNavigator ID="objectNavigator" runat="server" />
    <NetRadio:CollapseView ID="collapseView" runat="server">
				<Content>
					<table class="grid fixed">
						<tr>
							<td width="160"><NetRadio:SmartLabel id="nameCalling" runat="server" /></td>
							<td><asp:TextBox id="name" width="140" maxlength="50" runat="server" /> &nbsp; <span class="t2">*</span></td>
						</tr>
						<tr id="numberCallingRow" runat="server">
							<td><NetRadio:SmartLabel id="numberCalling" runat="server" /></td>
							<td><asp:TextBox id="number" width="140" maxlength="50" runat="server" /> &nbsp; <span class="t2">*</span></td>
						</tr>
						<tr id="culpritRoomRow" runat="server">
							<td>所在监舍</td>
							<td><asp:DropDownList width="146" id="culpritRoom" runat="server" /> &nbsp; <span class="t2">*</span></td>
						</tr>
						<tr id="grouplistRow" runat="server">
						<td>所属组</td>
						<td><asp:DropDownList ID="grouplist" width=80 runat="server" /></td>
						</tr>
						<tr id="photoRow" runat="server">
							<td>照片</td>
							<td>
								<div><NetRadio:Img id="photo" width="100" height="120" runat="server" /></div>
								<asp:FileUpload id="uploadPhoto" width="280" runat="server" />
								<div class="lineBreak"></div>
								<span class="t3">从您的计算机中选择对应的照片, 若无需上传照片请忽略此项</span></td>
						</tr>
						<tr>
							<td>领用标签</td>
							<td>
							 
							<uc1:SelectTag ID="tagSelector" VisibleUnBindButton="false" EnableOkFunction="false" runat="server" />
							 <uc2:SelectStrongestRssiTag ID="tagSelectorAuto" runat="server" EnableOK="false" EnableClose="false" EnableBorderCSS="false"/>
							 <script>
							    SelectStrongestRssiTag.Init();
							 </script>
						
								<div class="lineBreak"></div>
								<span class="t3">从本系统中选出他携带的标签, 若未领用标签请忽略此项</span>
								</td>
						</tr>
						<tr>
							<td>备注信息</td>
							<td><asp:TextBox id="memo" width="260" rows="4" textmode="multiline" runat="server" /></td>
						</tr>
						<tr>
							<td></td>
							<td><NetRadio:Remind id="feedbacks" autohideduration="5" style="width:240px" runat="server" /></td>						
						</tr>
						<tr>
							<td></td>
							<td><asp:Button id="saveButton" text="确认" onclick="saveButton_Click" runat="server" /></td>
						</tr>
					</table>
				</Content>
    </NetRadio:CollapseView>
</asp:Content>
