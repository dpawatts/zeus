// Global functions - Begin

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

function onUploadStarted() {
	++_uploadsInProgress;
}

function onUploadCompleted() {
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
}

function checkFileUploads() {
	_formSubmitAttempted = true;
	return _uploadsInProgress == 0;
}

function onSilverlightError(sender, args) {
	var appSource = "";
	if (sender != null && sender != 0) {
		appSource = sender.getHost().Source;
	}
	var errorType = args.ErrorType;
	var iErrorCode = args.ErrorCode;

	var errMsg = "Unhandled Error in Silverlight 2 Application " + appSource + "\n";

	errMsg += "Code: " + iErrorCode + "    \n";
	errMsg += "Category: " + errorType + "       \n";
	errMsg += "Message: " + args.ErrorMessage + "     \n";

	if (errorType == "ParserError") {
		errMsg += "File: " + args.xamlFile + "     \n";
		errMsg += "Line: " + args.lineNumber + "     \n";
		errMsg += "Position: " + args.charPosition + "     \n";
	}
	else if (errorType == "RuntimeError") {
		if (args.lineNumber != 0) {
			errMsg += "Line: " + args.lineNumber + "     \n";
			errMsg += "Position: " + args.charPosition + "     \n";
		}
		errMsg += "MethodName: " + args.methodName + "     \n";
	}

	throw new Error(errMsg);
}

// Global functions - End