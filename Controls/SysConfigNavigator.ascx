<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SysConfigNavigator.ascx.cs" Inherits="NetRadio.YangzhouJail.Web.Controls.SysConfigNavigator" %>

<NetRadio:TabView id="tabView" runat="server">
	<Tab label="添加警员" href="../TagUsers/TagUser_Add.aspx?type=1" />
	<Tab label="定点报警" href="../TagUsers/TagPositionList.aspx" />
	<Tab label="分组管理" href="../Organize/TagGroupList.aspx"/>
	<Tab label="区域设置" href="../Organize/MapAreaList.aspx" />
	<Tab label="用户管理" href="../Member/UserList.aspx" />
</NetRadio:TabView>
