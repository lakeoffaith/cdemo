<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagUserSelector.ascx.cs" Inherits="NetRadio.LocatingMonitor.Controls.__TagUserSelector" %>

<style type="text/css">
<!--
	#selectedList {
		margin: 6px 0;
		padding: 10px 15px;
		border: 1px solid #dddddd;
		float:left;
	}
	#selectorList {
		margin: 10px 0 10px 0;
		list-style: none;
		overflow:auto;
	}
	#selectorList li {
		width: 110px;
		margin-top: 3px;
		float: left;
		overflow: hidden;
	}
//-->
</style>

<asp:HiddenField id="tagUserType" runat="server" />
<asp:HiddenField id="selectedGroupId" runat="server" />


<a id="selectorLauncher" href="javascript:void(0);" onclick="javascript:Bmw.show(this);" class="underline">选择...</a><asp:HiddenField id="selectedTagIds" runat="server" />
<!-- 上面2个标记必须紧靠并且不能对调位置 -->

 &nbsp;
<span class="t3">(已选择 <asp:TextBox ID="selectedCount"  runat="server" Text="0" style="width:50px; color:Red; border:solid 0px black; text-align:center;"></asp:TextBox>个)</span>
<NetRadio:Div id="selectedList" signclientid="true" class="bg3" style="display:none" runat="server" />
<div class="clear"></div>
<script>
var selectedCountID='<%= selectedCount.ClientID%>';
</script>
<div id="tagSelector" class="floatLayer" style="width:465px; display:none">
	<div>
		<div class="fl">
			<asp:TextBox id="tagNameKeyword" width="90" text="关键字" style="color:gray" onfocus="javascript:Bmw.keywordFocus(this);" onblur="javascript:Bmw.keywordBlur(this);" runat="server" />
            <NetRadio:TagGroupSelector id="selectedGroupName" width="100" runat="server" />
			<input type="button" class="button" value="搜索" onclick="javascript:Bmw.listTags();" />
		</div>
		<div class="fr">
		<NetRadio:SmartLabel id="totalCountLabel" runat="server">
				总计 <NetRadio:NumericLabel id="totalCount" signclientid="true" class="t2" runat="server" /> 个
			</NetRadio:SmartLabel>
			<a href="javascript:Bmw.hide();" class="underline">确定</a>
		</div>
		<div class="clear"></div>
	</div>
	<div id="selectorListContainer">
		<ul id="selectorList" style="min-height: 120px; max-height:300px"></ul>
		<div class="clear"></div>
		<div class="fl" style="padding-top:5px">
			<NetRadio:SmartLabel id="allowedCountLabel" runat="server">
				最多允许选择 <NetRadio:NumericLabel id="allowedCount" signclientid="true" class="t2" runat="server" /> 个
			</NetRadio:SmartLabel>
			<NetRadio:SmartLabel id="selectAllLabel" runat="server">
				<input type="checkbox" id="selectAll" onclick="javascript:CheckboxUtil.checkAll(this, 'tagItem'); Bmw.onSelect();" /><label for="selectAll">全选</label>
			</NetRadio:SmartLabel>
			&nbsp; &nbsp;
			<span id="loading"></span>
		</div>
		<div class="fr">
			<input type="button" value="确定" class="button" onclick="javascript:Bmw.hide();" />
		</div>
		<div class="clear"></div>
	</div>
</div>