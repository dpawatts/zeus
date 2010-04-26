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
				if (this.tagName == "SPAN")
					$(this).text(newTitle);
				else
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
		if (!title)
			return "";

		var temp = title.toLowerCase().replace(/^\s*/, "").replace(/\s*$/, "");
		temp = temp.replace(/[ ]+/g, "-");

		var decimalRegExp = XRegExp("^\\p{Nd}$");
		var letterRegExp = XRegExp("^\\p{Ll}$");

		var result = "";
		for (var i = 0; i < temp.length; i++) {
			var c = temp.charAt(i);
			if (decimalRegExp.test(c) || letterRegExp.test(c))
				result += encodeURI(c);
			else if (c == '-' || c == '_')
				result += c;
		}
		return result;
	};

	// plugin defaults - added as a property on our plugin function
	$.fn.nameEditor.defaults = {
		titleEditorID: 'txtTitle'
	};
	
	// end of closure
})(jQuery);