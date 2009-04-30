// http://www.learningjquery.com/2007/10/a-plugin-development-pattern

// create closure
(function($) {
	// plugin definition
	$.fn.reactiveTextBox = function(options) {
		// Extend our default options with those provided.
		// Note that the first arg to extend is an empty object -
		// this is to keep from overriding our "defaults" object.
		var opts = $.extend({}, $.fn.reactiveTextBox.defaults, options);

		var chkKeepUpdated = document.getElementById(opts.keepUpdatedClientID);

		// Plugin implementation.
		var t = this;
		$(document).bind("ReactiveTextBox_OtherEditorChanged", function(event) {
			t.each(function() {
				if (!chkKeepUpdated.checked)
					return;
				$(this).val($.fn.reactiveTextBox.formattedValue(opts));
			});
		});
		for (var index in opts.otherEditors) {
			$("#" + opts.otherEditors[index]).change(function() {
				$(document).trigger("ReactiveTextBox_OtherEditorChanged");
			});
			$("#" + opts.otherEditors[index]).keyup(function() {
				$(document).trigger("ReactiveTextBox_OtherEditorChanged");
			});
		}
	};

	$.fn.reactiveTextBox.formattedValue = function(options) {
		// Format string will be something like:
		// {Title} - something else
		// So {Title} needs to be replaced by the value of the editor associated with the "Title" name.
		var result = options.formatString;
		for (var index in options.otherEditors) {
			var otherEditorValue = $("#" + options.otherEditors[index]).val();

			var re = new RegExp("{" + index + "}", "g");
			result = result.replace(re, otherEditorValue);
		}
		return result;
	};

	// end of closure
})(jQuery);

// plugin defaults - added as a property on our plugin function
$.fn.reactiveTextBox.defaults = {
	formatString : ""
};