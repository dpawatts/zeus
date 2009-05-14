// NAVIGATION
var zeusnav = new Object();


// EDIT
var zeustoggle = {
	show: function(btn, bar) {
		jQuery(btn).addClass("toggled").blur();
		jQuery(bar).show();
		cookie.create(bar, "show");
	},
	hide: function(btn, bar) {
		jQuery(btn).removeClass("toggled").blur();
		jQuery(bar).hide();
		cookie.erase(bar)
	}
};


// DEFAULT
var frameManager = function() { }
frameManager.prototype = {
	memorize: function(selected, action) {
		document.getElementById("memory").value = selected;
		document.getElementById("action").value = action;
	},
	/*initFrames: function() {
	///	<summary>
	///		Initialises the navigation and preview frames, with a splitter in the middle
	///	</summary>
	jQuery("#splitter").splitter({
	type: 'v',
	sizeLeft: 200
	});
	var t = this;
	jQuery(document).ready(function() {
	jQuery(window).bind("resize", function() {
	t.repaint();
	}).trigger("resize");
	});
	},
	repaint: function() {
	jQuery("#splitter").trigger("resize");
	jQuery("#splitter").height(this.contentHeight());
	jQuery("#splitter *").height(this.contentHeight());
	},
	contentHeight: function() {
	return document.documentElement.clientHeight - (jQuery.browser.msie ? 60 : 60);
	},*/
	getMemory: function() {
		var m = document.getElementById("memory");
		return encodeURIComponent(m.value);
	},
	getAction: function() {
		var a = document.getElementById("action");
		return encodeURIComponent(a.value);
	},
	refreshNavigation: function(navigationUrl) {
		top.MUI.updateContent({
			element: top.$('sidePanel1'),
			loadMethod: 'iframe',
			url: navigationUrl,
			title: "Content Tree",
			padding: { top: 0, right: 0, bottom: 0, left: 0 }
		});
	},
	refreshPreview: function(previewUrl) {
		top.MUI.updateContent({
			element: top.$('mainPanel'),
			loadMethod: 'iframe',
			url: previewUrl,
			title: "Preview",
			padding: { top: 0, right: 0, bottom: 0, left: 0 }
		});
	},
	refresh: function(navigationUrl, previewUrl) {
		this.refreshNavigation(navigationUrl);
		this.refreshPreview(previewUrl);
	}
};