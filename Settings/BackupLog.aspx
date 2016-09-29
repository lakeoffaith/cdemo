<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="BackupLog.aspx.cs" Inherits="NetRadio.LocatingMonitor.Settings.__BackupLog" %>

<%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator"  Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
   <style type="text/css">
        .style2
        {
            color: #FF0000;
        }
    </style>
    <NetRadio:ObjectNavigator id="ObjectNavigator" runat="server" />	
<NetRadio:CollapseView id="collapseView" runat="server">
				<Header>
					<Caption>按照提示输入表单内容</Caption>
				</Header>
				<Content>
			 <script type="text/jscript">
				function CheckedState()
				{
				 if(document.getElementById(idPreString+'backupdays').value.length==0 ||document.getElementById(idPreString+'backupdays').value=="0")
				 {
				   alert('请输入有效天数！');
				   return false;
				 }
				 
				 if(confirm('您确定要进行备份操作？！'))
				 {
                   document.getElementById(idPreString+"lblMessage").innerHTML="<font color='red'>备份可能需要您一点时间，请耐心等待...</font>";
				   return true;
				 }
				 else{
				 return false;
				 }
				}
				</script>
					<table class="grid fixed">
						<tr>
							<td width="160">备份天数</td>
							<td><asp:TextBox id="backupdays" width="75"  runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
								&nbsp;
								<NetRadio:SmartLabel id="readonlyMark" class="t3" runat="server"></NetRadio:SmartLabel></td>
						</tr>
						<tr>
							<td>是否清除</td>
							<td class="style2"><asp:RadioButtonList id="Rdolist" repeatlayout="flow" ForeColor="Black" repeatdirection="horizontal" runat="server">
									<asp:ListItem value="1">是</asp:ListItem>
									<asp:ListItem value="0" selected="true">否</asp:ListItem>
								</asp:RadioButtonList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(如果选"是"，将会清除数据库中已被备份的数据)</td>
						</tr>
						<tr>
							<td>备份路径</td>
							<td><NetRadio:SmartLabel id="lblBackupPath"  class="t3" runat="server"></NetRadio:SmartLabel>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(此路径为数据库服务器的路径)</td>
						</tr>
						<tr>
							<td>备份的数据表</td>
							<td><NetRadio:SmartLabel id="lblBackupTableList" class="t3" runat="server"></NetRadio:SmartLabel></td>
						</tr>
						<tr>
						<td></td>
							<td><asp:Label  id="lblMessage" class="t3"  runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td></td>
							<td><NetRadio:Remind id="feedbacks" autohideduration="10" style="width:240px" runat="server" /></td>
						</tr>
						<tr>
							<td></td>
							<td>
								<asp:Button id="submit" text="备份数据" onclick="submit_Click" onclientclick="javascript:return CheckedState();"  runat="server" />&nbsp;
								<input type="button" value="返 回" class="button" onclick="javascript:window.history.go(-1);" />
							</td>
						</tr>
					</table>
				</Content>
			</NetRadio:CollapseView>
			</asp:Content>
