function deleteTagGroup(id) {
	var a = $("g_" + id);
	var caution = "您确定要删除名称为“" + a.innerHTML + "”的对象分组吗 ?";
	if (confirm(caution)) {
		var tr = a.parentNode.parentNode;		// a << td << tr
		tr.parentNode.removeChild(tr);
		MarshalAjax.DeleteTagGroup(id, _empty);
	}
}