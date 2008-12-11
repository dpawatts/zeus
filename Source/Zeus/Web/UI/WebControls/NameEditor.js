// http://www.learningjquery.com/2007/10/a-plugin-development-pattern

// create closure
(function($) {
	// plugin definition
	$.fn.nameEditor = function(options) {
		// Extend our default options with those provided.
		// Note that the first arg to extend is an empty object -
		// this is to keep from overriding our "defaults" object.
		var opts = $.extend({}, $.fn.nameEditor.defaults, options);

		// Plugin implementation.
		var t = this;
		$(document).bind("NameEditor_NameChanged", function(event, titleTextBox) {
			var newTitle = getNameFromTitle($(titleTextBox).val());
			t.each(function() {
				$(this).val(newTitle);
			});
		});
		$("#" + options.titleEditorID).change(function() {
			$(document).trigger("NameEditor_NameChanged", this);
		});
		$("#" + options.titleEditorID).keyup(function() {
			$(document).trigger("NameEditor_NameChanged", this);
		});
	};

	// private function
	function getNameFromTitle(title) {
		return title.toLowerCase().replace(/[ ]+/g, "-").replace(/[^a-zA-Z0-9_-]/g, "");
	};
	// end of closure
})(jQuery);

// plugin defaults - added as a property on our plugin function
$.fn.nameEditor.defaults = {
	titleEditorID: 'txtTitle'
};