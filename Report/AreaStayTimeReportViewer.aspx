<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaStayTimeReportViewer.aspx.cs" Inherits="NetRadio.LocatingMonitor.Report.__AreaStayTimeReportViewer" %>
 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>对象区域停留和消失时间统计结果</title>
    <style type="text/css">
        .style1
        {
            width: 129px;
        }
        .style2
        {
            width: 129px;
            height: 23px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
     
     <table width=100%>
     <tr><td></td></tr>
     <tr><td align =center > <asp:Label ID="lbltitle" ForeColor =  "#003300"  
             Font-Bold=True Width ="100%" 
                runat="server" style="margin-right: 237px" Font-Size=Large ></asp:Label></td> </tr>
       <tr><td> <asp:Button ID="btnExport" runat="server" Text="导出" OnClick="btnExport_Click"/></td></tr> 
       <tr> 
       <td align =right > <asp:Label ID="lbltime" ForeColor = DarkBlue 
               Font-Bold  =False Width ="100%" 
                runat="server" style="margin-right: 237px" Font-Size=Smaller  ></asp:Label></td></tr>
        
   
        </table>
      
        <asp:GridView ID="GridView1" OnPageIndexChanging="GridView1_PageIndexChanging"  
            runat="server" style="width:100%" AllowPaging="True" OnSorting="GridView1_Sorting" 
            BorderColor="#333399" BorderStyle="Inset" BorderWidth="1px" 
               CaptionAlign="Top" Font-Names="Arial" 
            Font-Size=Medium   Height="234px" Width="640px" 
             FooterStyle-ForeColor=SkyBlue  AutoGenerateColumns="False" 
             CellPadding="3" CellSpacing="1" EnableModelValidation="True" 
             HorizontalAlign="Center" PageSize="50" AllowSorting="True">
            <RowStyle Font-Size="Smaller" HorizontalAlign="Center" VerticalAlign="Middle" />
            <EmptyDataRowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
       
            <Columns >
            <asp:BoundField HeaderText="对象名称" DataField="TagName" SortExpression="TagName"/>
          <asp:BoundField HeaderText="区域内时间" DataField="InArea" SortExpression="InArea"/>
          <asp:BoundField HeaderText="消失时间" DataField="Disappear" SortExpression="Disappear" />
          <asp:BoundField HeaderText="最早时间" DataField="FirstTime" SortExpression="FirstTime"/>
          <asp:BoundField HeaderText="最近时间" DataField="LastTime" SortExpression="LastTime" />
            </Columns>
            <FooterStyle BorderStyle="Double" />
            <SelectedRowStyle BorderStyle="Solid" />
            <HeaderStyle BackColor="#f1f1f1" Font-Size=Medium ForeColor="#78462e" />
            <EditRowStyle HorizontalAlign="Center" VerticalAlign="Middle" 
                Font-Size="Smaller" />
            <AlternatingRowStyle BackColor="#f7f7f7" />
        </asp:GridView>
       
        
    
    
    </form>
</body>
</html>















 