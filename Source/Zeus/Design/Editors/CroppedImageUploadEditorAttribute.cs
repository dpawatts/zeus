using System;
using Zeus.Web.UI.WebControls;
using System.Web.UI;
using Zeus.FileSystem.Images;
using Zeus.FileSystem;
using Zeus.ContentTypes;
using System.IO;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CroppedImageUploadEditorAttribute : FileUploadEditorAttribute
	{
		public int? MinimumWidth { get; set; }
		public int? MinimumHeight { get; set; }

		/// <summary>Initializes a new instance of the ImageUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
        public CroppedImageUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		protected override FancyFileUpload CreateEditor()
		{            
			FancyImageUpload uploader = new FancyImageUpload { MinimumWidth = MinimumWidth, MinimumHeight = MinimumHeight };
            //add crop tool if item is already saved
            
            //CroppedImage image = (CroppedImage)this.UnderlyingProperty.GetValue(;
            return uploader;
            
		}

        protected override void UpdateEditorInternal(IEditableObject item, Control editor)
        {
            base.UpdateEditorInternal(item, editor);

            if (((CroppedImage)item).Data != null)
            {
                //check to see if the image is large enough...
                CroppedImage image = (CroppedImage)item;

                System.Drawing.Image imageForSize = System.Drawing.Image.FromStream(new MemoryStream(image.Data));
                int ActualWidth = imageForSize.Width;
                int ActualHeight = imageForSize.Height;
                imageForSize.Dispose();

                if (ActualWidth > image.FixedWidthValue && ActualHeight > image.FixedHeightValue)
                {
                    string selected = System.Web.HttpContext.Current.Request.QueryString["selected"];
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Preview of how the image will look on the page</p><br/><p><a href=\"/admin/ImageCrop.aspx?id=" + image.ID + "&selected=" + selected + "\">Edit Crop</a></p><br/>"));
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<img src=\"" + ((CroppedImage)image).GetUrl(image.FixedWidthValue, image.FixedHeightValue, true, SoundInTheory.DynamicImage.DynamicImageFormat.Jpeg, false) + "?rand=" + new System.Random().Next(1000) + "\" /></div><br/><br/>"));
                }
                else
                {
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Image is not large enough to be cropped - it is advised that you upload a larger image</p><br/>"));                    
                }
            }
        }


        
	}
}