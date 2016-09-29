<%@ Page Title="" Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true" CodeBehind="TagAlertProcess.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__TagAlertProcess" %>
<%@ Register src="../Controls/ProcessAlert.ascx" tagname="ProcessAlert" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <uc1:ProcessAlert ID="ProcessAlert1" runat="server" />
</asp:Content>
