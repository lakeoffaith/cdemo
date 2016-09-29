function callFilter() {
	var layer = $$("filterForm");
	layer.display();
	layer.locateCenter();
}

function cancelFilter() {
	$$("filterForm").hide();
}