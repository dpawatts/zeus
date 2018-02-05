//delete button functions
function delButtons() {
	$(".deleteWrap").children().not(".delete").hide();

	$(".delete").click(function() {
		$(this).siblings().show();
	});

	$(".cancel").click(function() {
		$(".deleteWrap").children().not(".delete").hide();
	});
}

function prepareDocument() {
	delButtons();
}

$(document).ready(function() {
	prepareDocument();
});