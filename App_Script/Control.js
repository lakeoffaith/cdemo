var __userControl=new Object();
__userControl.keywordFocus = function(el) {
	if (el.value == "关键字") {
		el.value = "";
		el.style.color = "";
	}
}
__userControl.keywordBlur = function(el) {
	if (el.value == "") {
		el.value = "关键字";
		el.style.color = "gray";
	}
}
__userControl.show=function(id)
{
    $(id).style.display="inline"; 
}

__userControl.close=function(id)
{
    $(id).style.display="none";
}