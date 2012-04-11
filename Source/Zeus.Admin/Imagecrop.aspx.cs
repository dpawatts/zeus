using System;
using System.Configuration;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Configuration;
using Zeus.Web.Security;
using Zeus.Web.UI;
using Zeus.FileSystem.Images;
using SoundInTheory.DynamicImage;
using System.IO;

namespace Zeus.Admin
{
    public partial class Imagecrop : System.Web.UI.Page
	{
        public CroppedImage ImageToEdit;
        public string selectedForForm;
        public double aspectRatio;
        public int minWidth;
        public int minHeight;        

		protected void Page_Load(object sender, EventArgs e)
		{
            selectedForForm = Request.QueryString["selected"];

			//ltlAdminName.Text = ((AdminSection) ConfigurationManager.GetSection("zeus/admin")).Name;
            if (Request.Form["id"] == null)
            {
                //this is the display page
                int id = Convert.ToInt32(Request.QueryString["id"]);
                ImageToEdit = Zeus.Context.Persister.Get<CroppedImage>(id);
                bFixedAspectRatio = ImageToEdit.FixedWidthValue > 0 && ImageToEdit.FixedHeightValue > 0;
                if (bFixedAspectRatio)
                    aspectRatio = (double)ImageToEdit.FixedWidthValue / (double)ImageToEdit.FixedHeightValue;
                
                //need to set the min and max sizes...this will stop people upscaling their images
                CroppedImage imageToEdit = Zeus.Context.Persister.Get<CroppedImage>(id);

                System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageToEdit.Data));
                int ActualWidth = image.Width;
                int ActualHeight = image.Height;
                image.Dispose();

                int widthAfterResize = 0;
                int heightAfterResize = 0;

                if ((Convert.ToDouble(ActualWidth) / Convert.ToDouble(800)) >= (Convert.ToDouble(ActualHeight) / Convert.ToDouble(600)))
                {
                    double percChange = (double)800 / (double)ActualWidth;
                    widthAfterResize = Convert.ToInt32(Convert.ToDouble(ActualWidth) * percChange);
                    heightAfterResize = Convert.ToInt32(Convert.ToDouble(ActualHeight) * percChange);

                    //resized, leaving width @ 800
                    if (percChange < 1)
                    {
                        minWidth = Convert.ToInt32(Math.Round(percChange * ImageToEdit.FixedWidthValue, 0));
                        minHeight = Convert.ToInt32(Math.Round(percChange * ImageToEdit.FixedHeightValue, 0));
                    }
                    else
                    {
                        minWidth = ImageToEdit.FixedWidthValue;
                        minHeight = ImageToEdit.FixedHeightValue;
                    }
                }
                else
                {                    
                    //resized, leaving height @ 600
                    double percChange = (double)600 / (double)ActualHeight;
                    if (percChange < 1)
                    {
                        minWidth = Convert.ToInt32(Math.Round(percChange * ImageToEdit.FixedWidthValue, 0));
                        minHeight = Convert.ToInt32(Math.Round(percChange * ImageToEdit.FixedHeightValue, 0));
                    }
                    else
                    {
                        minWidth = ImageToEdit.FixedWidthValue;
                        minHeight = ImageToEdit.FixedHeightValue;
                    }
                }


                //check to see if now outside of the boundaries!
                if (minWidth > ActualWidth)
                {
                    /*
                    double percChange2 = ActualWidth / minWidth;
                    minWidth = ActualWidth;
                    minHeight = minHeight * percChange2;
                     */
                }

                //ImageToEdit.GetUrl(800, 600, true, DynamicImageFormat.Jpeg, true);
            }
            else
            {
                //this is the form post - so save the data
                int id = Convert.ToInt32(Request.Form["id"]);
                int x1 = Convert.ToInt32(Request.Form["x1"]);
                int y1 = Convert.ToInt32(Request.Form["y1"]);
                int w = Convert.ToInt32(Request.Form["w"]);
                int h = Convert.ToInt32(Request.Form["h"]);
                string selected = Request.Form["selected"];

                CroppedImage imageToEdit = Zeus.Context.Persister.Get<CroppedImage>(id);

                System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(imageToEdit.Data));
                int ActualWidth = image.Width;
                int ActualHeight = image.Height;
                image.Dispose();

                //we know that for display purposes before cropping the image was resized to 800 x 600, so do some calcs...

                if (ActualWidth <= 800 && ActualHeight <= 600)
                {
                    //no resizing happened
                }
                else if ((Convert.ToDouble(ActualWidth) / Convert.ToDouble(800)) >= (Convert.ToDouble(ActualHeight) / Convert.ToDouble(600)))                
                {
                    //resized, leaving width @ 800
                    double percChange = (double)ActualWidth / (double)800;
                    x1 = Convert.ToInt32(Math.Round(percChange * x1, 0));
                    y1 = Convert.ToInt32(Math.Round(percChange * y1, 0));
                    w = Convert.ToInt32(Math.Round(percChange * w, 0));
                    h = Convert.ToInt32(Math.Round(percChange * h, 0));
                }
                else
                {
                    //resized, leaving height @ 600
                    double percChange = (double)ActualHeight / (double)600;
                    x1 = Convert.ToInt32(Math.Round(percChange * Convert.ToDouble(x1), 0));
                    y1 = Convert.ToInt32(Math.Round(percChange * Convert.ToDouble(y1), 0));
                    w = Convert.ToInt32(Math.Round(percChange * Convert.ToDouble(w), 0));
                    h = Convert.ToInt32(Math.Round(percChange * Convert.ToDouble(h), 0));
                }

                ImageToEdit = Zeus.Context.Persister.Get<CroppedImage>(id);
                ImageToEdit.TopLeftXVal = x1;
                ImageToEdit.TopLeftYVal = y1;
                ImageToEdit.CropWidth = w;
                ImageToEdit.CropHeight = h;
                Zeus.Context.Persister.Save(ImageToEdit);

                Response.Redirect("/admin/plugins.edit-item.default.aspx?selected=" + selected);
            }
		}

        protected bool bFixedAspectRatio { get; set; }
 
		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterJQuery();
            Page.ClientScript.RegisterJavascriptResource(typeof(Imagecrop), "Zeus.Admin.Assets.JS.jcrop_jquery.js");
			
            //Page.ClientScript.RegisterCssResource(typeof(Login), "Zeus.Admin.Assets.Css.jcrop.demos.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.reset.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.login.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.jcrop_demos.css");
            Page.ClientScript.RegisterCssResource(typeof(Imagecrop), "Zeus.Admin.Assets.Css.jcrop_jquery.css");
            
			base.OnPreRender(e);
		}
         
	}
}
