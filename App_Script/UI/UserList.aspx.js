function deleteUser(id) {
	var caution = "确定要删除该用户吗 ?";
	if (confirm(caution)) {
		var el = document.getElementById("user_" + id);
		if (el) {
			var tr = el.parentNode.parentNode;	//a << td << tr
			tr.parentNode.removeChild(tr);
		}
		MarshalAjax.DeleteUser(id, _empty);
	}
}