// NAVIGATION
var zeusnav = new Object();


// EDIT
var zeustoggle = {
	show: function(btn, bar) {
		$(btn).addClass("toggled").blur();
		$(bar).show();
		cookie.create(bar, "show");
	},
	hide: function(btn, bar) {
		$(btn).removeClass("toggled").blur();
		$(bar).hide();
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
	initFrames: function() {
		///	<summary>
		///		Initialises the navigation and preview frames, with a splitter in the middle
		///	</summary>
		$("#splitter").splitter({
			type: 'v',
			sizeLeft: 200
		});
		var t = this;
		$(document).ready(function() {
			$(window).bind("resize", function() {
				t.repaint();
			}).trigger("resize");
		});
	},
	repaint: function() {
		$("#splitter").trigger("resize");
		$("#splitter").height(this.contentHeight());
		$("#splitter *").height(this.contentHeight());
	},
	contentHeight: function() {
		return document.documentElement.clientHeight - (jQuery.browser.msie ? 60 : 60);
	},
	getMemory: function() {
		var m = document.getElementById("memory");
		return encodeURIComponent(m.value);
	},
	getAction: function() {
		var a = document.getElementById("action");
		return encodeURIComponent(a.value);
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