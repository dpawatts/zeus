		jQuery(function($){

      // Create variables (in this scope) to hold the API and image size
      var jcrop_api, boundx, boundy;
      
      $('#target').Jcrop({
        onChange: updatePreview,
        onSelect: updatePreview,
        onRelease:  clearCoords,
        aspectRatio: 1.3333
        //${(double)Model.ImageToEdit.FixedWidthValue / (double)Model.ImageToEdit.FixedHeightValue}
      },function(){
        // Use the API to get the real image size
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        // Store the API in the jcrop_api variable
        jcrop_api = this;
      });

      function updatePreview(c)
      {
        if (parseInt(c.w) > 0)
        {
          var rx = 100 / c.w;
          var ry = 100 / c.h;

          $('#preview').css({
            width: Math.round(rx * boundx) + 'px',
            height: Math.round(ry * boundy) + 'px',
            marginLeft: '-' + Math.round(rx * c.x) + 'px',
            marginTop: '-' + Math.round(ry * c.y) + 'px'
          });
        }
	    
		  $('#x1').val(c.x);
		  $('#y1').val(c.y);
		  $('#w').val(c.w);
		  $('#h').val(c.h);
        
      };

    });


		function clearCoords()
		{
		  $('#coords input').val('');
		  $('#h').css({color:'red'});
		  window.setTimeout(function(){
			$('#h').css({color:'inherit'});
		  },500);
		};
		