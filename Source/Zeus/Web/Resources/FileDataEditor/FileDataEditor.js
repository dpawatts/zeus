// http://www.learningjquery.com/2007/10/a-plugin-development-pattern

// create closure
(function($) {
	// Define FileDataEditor "class".
	var FileDataEditor = function(element, options) {
		// Extend our default options with those provided.
		// Note that the first arg to extend is an empty object -
		// this is to keep from overriding our "defaults" object.
		var opts = $.extend({}, $.fn.fileDataEditor.defaults, options);

		var hiddenFilenameField = $(opts.hiddenFilenameField);
		var hiddenIdentifierField = $(opts.hiddenIdentifierField);
		var beforeUploadDiv = $(opts.beforeUploadDiv);
		var duringUploadDiv = $(opts.duringUploadDiv);
		var afterUploadDiv = $(opts.afterUploadDiv);

		this.onUploadStarted = function(filename, identifier) {
			$.fn.fileDataEditor.onUploadStarted();

			hiddenFilenameField.val(filename);
			hiddenIdentifierField.val(identifier);
			beforeUploadDiv.hide();
			duringUploadDiv.show();
		};

		this.onUploadCompleted = function(filename) {
			$.fn.fileDataEditor.onUploadCompleted();

			duringUploadDiv.hide();
			afterUploadDiv.text(filename).show();
		};

		this.onPercentageChanged = function(percentage) {
			duringUploadDiv.text('Uploading: ' + percentage + '%');
		};

		this.onMaximumFileSizeReached = function() {
			alert('Maximum file size exceeded');
		};
	};

	// plugin definition
	$.fn.fileDataEditor = function(options) {
		var plugin;

		this.each(function() {
			var element = $(this);

			// Return early if this element already has a plugin instance
			plugin = element.data('fileDataEditor');
			if (plugin)
				return false;

			var fileDataEditor = new FileDataEditor(this, options);

			// Store plugin object in this element's data
			element.data('fileDataEditor', fileDataEditor);
		});

		// return either the plugin object or the jQuery object reference
		return plugin || this;
	};

	var _uploadsInProgress = 0;
	var _submitHandler;
	var _formSubmitAttempted = false;

	function initializeRequestHandler(sender, args) {
		if (_uploadsInProgress > 0) {
			args.set_cancel(true);
			alert("File uploads are still in progress. Please wait for them to complete and then try again.");
		}
	}

	$(document).ready(function() {
		Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(initializeRequestHandler);
	});

	$.fn.fileDataEditor.onUploadStarted = function() {
		++_uploadsInProgress;
	};

	$.fn.fileDataEditor.onUploadCompleted = function() {
		--_uploadsInProgress;

		// Check if a form submit has been attempted.
		if (!_formSubmitAttempted)
			return;

		if (_uploadsInProgress == 0) {
			// Submit form
			_formSubmitAttempted = false;
			alert("TODO: Submit form");
			//submitForm();
		}
	};

	$.fn.fileDataEditor.checkFileUploads = function() {
		_formSubmitAttempted = true;
		return _uploadsInProgress == 0;
	}

	// end of closure
})(jQuery);

// plugin defaults - added as a property on our plugin function
$.fn.fileDataEditor.defaults = {
	
};