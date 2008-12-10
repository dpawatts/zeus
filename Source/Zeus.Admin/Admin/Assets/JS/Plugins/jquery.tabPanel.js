// http://www.learningjquery.com/2007/10/a-plugin-development-pattern

// plugin definition
$.fn.tabPanel = function(options) {
	// Extend our default options with those provided.
	// Note that the first arg to extend is an empty object -
	// this is to keep from overriding our "defaults" object.
	var opts = $.extend({}, $.fn.tabPanel.defaults, options);

	// Plugin implementation.
	var tabContainer = $($(this.get(0)).before("<ul class='tabs'></ul>").prev().get(0));
	$(this).each(function() {
		tabContainer.append('<li><a href="#' + this.id + '"><span>' + this.title + '</span></a></li>');
	});

	tabContainer.tabs();
};

// plugin defaults - added as a property on our plugin function
$.fn.tabPanel.defaults = {
	selector: ".tabPanel"
};