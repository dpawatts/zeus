<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Imagecrop.aspx.cs" Inherits="Zeus.Admin.Imagecrop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head runat="server">
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
	<title>Login</title>
	
	

</head>

<body>

    <script>
		jQuery(function($){

      // Create variables (in this scope) to hold the API and image size
      var jcrop_api, boundx, boundy;
      
      $('#target').Jcrop({
        onChange: updatePreview,
        onSelect: updatePreview,
        onRelease:  clearCoords,
        aspectRatio: <%= aspectRatio %>
      },function(){
        // Use the API to get the real image size
        var bounds = this.getBounds();
        boundx = bounds[0];
        boundy = bounds[1];
        // Store the API in the jcrop_api variable
        jcrop_api = this;
        jcrop_api.setOptions( {
			minSize: [ <%= minWidth %>, <%= minHeight %> ],
			maxSize: [ 0, 0 ]
		} );
		//alert('hey');
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
		
	</script>

	<noscript><div id="js">
		<p><span class="bold">NOTE: </span>Javascript is turned off. You must have javascript turned on to use this interface. For instructions, please contact us by clicking <a href="http://www.sitdap.com">here</a></p>
	</div></noscript>
	
		<!-- This is the image we're attaching Jcrop to -->
		<img src="<%= ImageToEdit.GetUrlForAdmin(800, 600, true, SoundInTheory.DynamicImage.DynamicImageFormat.Jpeg, true) %>?rand=<%= new System.Random().Next(1000) %>" id="target" alt="Image to Edit" />

		<!-- This is the form that our event handler fills -->
		<form id="coords" class="coords" action="/admin/Imagecrop.aspx" method="post">
			<input type="hidden" name="id" value="<%= ImageToEdit.ID %>" />		  
			<input type="hidden" name="selected" value="<%= selectedForForm %>" />		  
			<div>
				<label>X1 <input type="text" size="4" id="x1" name="x1" /></label>
				<label>Y1 <input type="text" size="4" id="y1" name="y1" /></label>
				<label>W <input type="text" size="4" id="w" name="w" /></label>
				<label>H <input type="text" size="4" id="h" name="h" /></label>
				<br /><br />
				<input type="submit" value="Save Changes" id="btnSaveCrop" />
			</div>
			
		</form>

</body>
</html>