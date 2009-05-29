/* 

	In this file we setup our Windows, Columns and Panels,
	and then inititialize MochaUI.
	
	At the bottom of Core.js you can setup lazy loading for your
	own plugins.

*/

/*
  
INITIALIZE WINDOWS

	1. Define windows
	
		var myWindow = function(){ 
			new MUI.Window({
				id: 'mywindow',
				title: 'My Window',
				contentURL: 'pages/lipsum.html',
				width: 340,
				height: 150
			});
		}

	2. Build windows on onDomReady
	
		myWindow();
	
	3. Add link events to build future windows
	
		if ($('myWindowLink')){
			$('myWindowLink').addEvent('click', function(e) {
				new Event(e).stop();
				jsonWindows();
			});
		}

		Note: If your link is in the top menu, it opens only a single window, and you would
		like a check mark next to it when it's window is open, format the link name as follows:

		window.id + LinkCheck, e.g., mywindowLinkCheck

		Otherwise it is suggested you just use mywindowLink

	Associated HTML for link event above:

		<a id="myWindowLink" href="pages/lipsum.html">My Window</a>	


	Notes:
		If you need to add link events to links within windows you are creating, do
		it in the onContentLoaded function of the new window. 
 
-------------------------------------------------------------------- */


/*
  
INITIALIZE COLUMNS AND PANELS  

	Creating a Column and Panel Layout:
	 
	 - If you are not using panels then these columns are not required.
	 - If you do use panels, the main column is required. The side columns are optional.
	 
	 Columns
	 - Create your columns from left to right.
	 - One column should not have it's width set. This column will have a fluid width.
	 
	 Panels
	 - After creating Columns, create your panels from top to bottom, left to right.
	 - One panel in each column should not have it's height set. This panel will have a fluid height.	 
	 - New Panels are inserted at the bottom of their column. 
 
-------------------------------------------------------------------- */


initializeColumns = function() {

	new MUI.Column({
		id: 'sideColumn1',
		placement: 'left',
		width: 200,
		resizeLimit: [100, 400]
	});
	
	new MUI.Column({
		id: 'mainColumn',
		placement: 'main',
		resizeLimit: [100, 300]
	});
	
	// Add panels to first side column
	new MUI.Panel({
		id: 'treePanel',
		title: 'Content Tree',
		column: 'sideColumn1',
		loadMethod: 'iframe',
		contentURL: 'navigation/tree.aspx'
	});
	
	// Add panels to main column	
	new MUI.Panel({
		id: 'mainPanel',
		title: 'Preview',
		column: 'mainColumn',
		loadMethod: 'iframe',
		contentURL: '/default.aspx'
	});

	MUI.myChain.callChain();
	
}

// Initialize MochaUI when the DOM is ready
window.addEvent('load', function(){ //using load instead of domready for IE8

	MUI.myChain = new Chain();
	MUI.myChain.chain(
		function(){MUI.Desktop.initialize();},
		function(){MUI.Dock.initialize();},
		function(){initializeColumns();}	
	).callChain();	

});
