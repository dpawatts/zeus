var frameManager = function() { }
frameManager.prototype = {
	initFrames: function() {
		///	<summary>
		///		Initialises the navigation and preview frames, with a splitter in the middle
		///	</summary>
		$("#outerSplitter").splitter({
			type: "v",
			initA: true
		});

		$("#splitter").splitter({
			type: 'v',
			initA: true // use width of A (#leftPane) from styles
		});
		var t = this;
		$(document).ready(function() {
			$(window).bind("resize", function() {
				t.repaint();
			}).trigger("resize");
		});
	},
	repaint: function() {
	$("#outerSplitter").trigger("resize");
	$("#outerSplitter").height(this.contentHeight());
		$("#outerSplitter *").height(this.contentHeight());
	},
	contentHeight: function() {
		return document.documentElement.clientHeight - (jQuery.browser.msie ? 88 : 88);
	},
	refreshNavigation: function(navigationUrl) {
		var nav = document.getElementById('navigation');
		nav.src = navigationUrl;
	},
	refreshPreview: function(previewUrl) {
		var prev = document.getElementById('preview');
		prev.src = previewUrl;
	},
	refresh: function(navigationUrl, previewUrl) {
		this.refreshNavigation(navigationUrl);
		this.refreshPreview(previewUrl);
	}
};