<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Tag.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Objects.__Tag" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <style type="text/css">
        <!
        -- .propName
        {
            width: 135px;
            background: #fefefe;
        }
        .propValue
        {
            color: #aa0000;
        }
         //
        -- ></style>
    <NetRadio:CollapseView ID="summary" runat="server">
				<Header>
					<Caption>标签摘要信息</Caption>
					<Switch />
				</Header>
				<Content>
					<table class="grid fixed">
						<tr>
							<td class="propName">名称</td>
							<td class="propValue"><NetRadio:SmartLabel id="tagName" class="bold" runat="server"/></td>
							<td class="propName">标签Mac</td>
							<td class="propValue"><NetRadio:SmartLabel id="macAddress" signclientid="true" runat="server"/></td>
						</tr>
						<tr>
							<td class="propName">产品类型</td>
							<td class="propValue"><NetRadio:SmartLabel id="productType"  runat="server"/></td>
							<td class="propName">标签状态</td>
							<td class="propValue">空闲, 无人佩戴</td>
						</tr>
					</table>
				</Content>
				<Footer />
    </NetRadio:CollapseView>
    <div class="center">
        <input type="button" class="button" value="关闭窗口" onclick="closeWindow();" /></div>
</asp:Content>
