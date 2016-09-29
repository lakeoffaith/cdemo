<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoRefresher.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__AutoRefresher" %>

<div style="display:none">
	<table cellpadding="0" cellspacing="0" id="refreshDuration">
		<tr><td><a href="javascript:setRefreshDuration(2);">2秒</a></td></tr>
		<tr><td><a href="javascript:setRefreshDuration(3);">3秒</a></td></tr>
		<tr><td><a href="javascript:setRefreshDuration(5);">5秒</a></td></tr>
		<tr><td><a href="javascript:setRefreshDuration(10);">10秒</a></td></tr>
	</table>
</div>

<asp:HiddenField id="autoRefreshDuration" value="3" runat="server" />
<input type="hidden" id="autoRefreshFlag" name="autoRefreshFlag" />
<input type="submit" id="autoRefreshSender" name="autoRefreshSender" style="display:none" />

本页面每
<NetRadio:SmartLabel id="autoRefreshDurationText" signclientid="true" class="t1" onmouseover="javascript:Dropmenu.show(this, 'refreshDuration')" style="cursor:pointer" runat="server" />
自动刷新一次

<span id="refreshProgress"></span>