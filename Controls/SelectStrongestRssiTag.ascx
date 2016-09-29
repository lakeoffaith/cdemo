<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SelectStrongestRssiTag.ascx.cs"
    Inherits="NetRadio.LocatingMonitor.Controls.__SelectStrongestRssiTag" %>
<% if (EnableBorderCSS)
   { %><div id="divSelectTag" class="floatLayer" style="display: none;">
       <% }%>
       <span id="labTitle" style="color: Red;"></span>
       <span id="labTag" style="color: Red;"></span>
       <%--  <asp:HiddenField ID="hidTagID" runat="server" Value="" />
    <asp:HiddenField ID="hidTagMAC" runat="server" Value="" />--%>
       <input id="hidTagID" name="hidTagID" type="hidden" value="" />
       <input id="hidTagMAC" name="hidTagMAC" type="hidden" value="" />
       <span id="spanClear" style="display: none; cursor: pointer; color: Black;" onclick="clearInfo()">
           &nbsp;清除</span> &nbsp;
       <% if (EnableOK)
          { %>
       <span id="spanOK" style="display: none; cursor: pointer; color: Black;" onclick="ChangeTag()">
           &nbsp;绑定</span> &nbsp;
       <% }%>
       <span id="spanUnbind" style="cursor: pointer; color: Black; <% if(!EnableUnbind){ %> display: none;
           <% }%>" onclick="UnbindTag()">&nbsp;解除</span> &nbsp; <span id="spanClose" style="cursor: pointer;
               color: Black; <% if(!EnableClose){ %> display: none; <% }%>" onclick="SelectStrongestRssiTag.Close()">
               &nbsp;关闭</span> &nbsp;
       <br />
       <span id="labLoading"></span>
       <% if (EnableBorderCSS)
          { %></div>
<% }%>
