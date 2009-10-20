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
	refreshNavigation: function(nodePath) {
		stpNavigation.getLoader().baseParams.overrideNode = nodePath;
		stpNavigation.getLoader().baseParams.fromRoot = true;
		stpNavigation.getRootNode().reload(function() {
			stpNavigation.selectPath(stpNavigation.getNodeById(decodeURIComponent(nodePath)).getPath());
		});
		stpNavigation.getLoader().baseParams.overrideNode = "";
		stpNavigation.getLoader().baseParams.fromRoot = false;
	},
	refreshPreview: function(previewUrl) {
		this.reloadContentPanel('Preview', previewUrl);
	},
	refresh: function(nodePath, previewUrl) {
		this.refreshNavigation(nodePath);
		this.refreshPreview(previewUrl);
	},
	reloadContentPanel: function(title, url) {
		var memory, action;
		if (window.top.zeus) {
			memory = window.top.zeus.getMemory();
			action = window.top.zeus.getAction();
		}
		url = url
			.replace("{memory}", memory)
			.replace("{action}", action);

		Ext.getCmp("pnlContent").setTitle(title);
		document.getElementById("pnlContent_IFrame").src = url;
		//Coolite.AjaxMethods.ReloadContentPanel(title, url);
	}
};