<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="PoliceJobSize.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Report.__PoliceJobSize" %>

<%@ Register TagPrefix="NetRadio" TagName="ReportNavigator" Src="~/Controls/ReportNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <NetRadio:ReportNavigator ID="reportNavigator" runat="server" />
    <blockquote>
        <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0">
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
                    <asp:TextBox ID="fromMinute" value="00" Width="25" MaxLength="2" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
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
                    <asp:TextBox ID="toMinute" value="00" Width="25" MaxLength="2" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')"
                        onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                    <input id="btnSearch" type="button" value="查看" onclick="return getJobSize();" />
                </td>
            </tr>
        </table>
        <br />
        <div id="htmlContent">
        </div>

        <script>
    function getJobSize()
    {
         $("htmlContent").innerHTML="";
         $$("htmlContent").showLoading();
         $("btnSearch").disabled=true;
         window.setTimeout(function(){
          var beginTime=$('<%= fromDate.ClientID%>').value+" "+$('<%= fromHour.ClientID%>').value+":"+$('<%= fromMinute.ClientID%>').value+":00";
          var endTime=$('<%= toDate.ClientID%>').value+" "+$('<%= toHour.ClientID%>').value+":"+$('<%= toMinute.ClientID%>').value+":00";
          NetRadio.LocatingMonitor.Report.__PoliceJobSize.GetJobSize(beginTime,endTime,function(r){
           if(r.error==0)
           { 
                 $("htmlContent").innerHTML=r.value;
              
           }
           else
           {             
                 $("htmlContent").innerHTML=r.errorText;
           }  
           $("btnSearch").disabled=false;           
          }); 
          },2);     
    }
   
    getJobSize();
     
        </script>
</asp:Content>
