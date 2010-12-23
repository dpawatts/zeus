$(document).ready(function(){
    // loads rollover script
    SwapImage.rollover.init();

	/**************** DOM ELEMENTS *************/

	
	/**************** HANDLERS *************/
	
	if($('#rightNav').length) {
		_rightNav.init();
	}
	
	if($('#contactLeft').length) {
		iHelper($('#fYourName'));
		iHelper($('#fYourContactNumber'));
		iHelper($('#fEnquiry'));
		
	}

});

// RIGHT NAV DROPDOWNS

_rightNav = function() {
	var $rightNav;
	var $rightNavSpans;
	
	
	
	function init() {
		// Bind DOM Elements
		$rightNav = $('#rightNav');
		$rightNavSpans = $('#rightNav li span');
		
		// Init 
		$rightNav.children('li').not('.onParent').children('ul').hide();
		$rightNav.children('li.onParent').children('span').addClass('open').data('open', true);
		
		$rightNavSpans.addClass('JS').click(function() {
			
			if($(this).data('open')) {
				$(this).data('open', false).removeClass('open').next().slideUp('fast');
			} else {
				$(this).data('open', true).addClass('open').next().slideDown('fast');
			}
			
		});
	}
	
	return {
		init: init
	}
}();

// 	GENERIC INPUT HELPER	

function iHelper(theInput) {
	var orig = theInput.attr('value');			
     theInput.focus(function() {
        if($(this).val() == orig) {
            $(this).val('');
        } 
     }).blur(function() {
         if($(this).val() == "") {
             $(this).val(orig);
         }
     });
}


// image rollovers

SwapImage = {};

SwapImage.rollover =
{
   init: function()
   {
      this.preload();

      $('.hover').css('cursor', 'pointer').hover(function () { 
          $(this).attr( 'src', SwapImage.rollover.newimage($(this).attr('src')) ); 
      }, function () { 
          $(this).attr( 'src', SwapImage.rollover.oldimage($(this).attr('src')) ); 
      });
   },

   preload: function()
   {
      $(window).bind('load', function() {
         $('.hover').each( function( key, elm ) { $('<img>').attr( 'src', SwapImage.rollover.newimage( $(this).attr('src') ) ); });
      });
   },

   newimage: function( src ) { return src.replace(/\.(\w{3})/g, "Hover.$1"); },
   oldimage: function( src ){ return src.replace(/Hover\.(\w{3})/g, ".$1"); }
};
 




/*FANCY BOX EXAMPLE

if ($('.imgRight').length > 0) {
    $("a.fancy").fancybox({
        'zoomSpeedIn': 200,
        'zoomSpeedOut': 200,
        'overlayShow': true,
        'overlayOpacity': 0
    });
}*/