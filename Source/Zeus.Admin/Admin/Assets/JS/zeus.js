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
		this.reloadContentPanel('Preview', previewUrl);
	},
	refresh: function(navigationUrl, previewUrl) {
		this.refreshNavigation(navigationUrl);
		this.refreshPreview(previewUrl);
	},
	reloadContentPanel: function(title, url) {
		var contentIframe = document.getElementById('content');
		contentIframe.src = url;
	}
};