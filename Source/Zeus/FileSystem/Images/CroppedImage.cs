using System.IO;
using SoundInTheory.DynamicImage.Fluent;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.Design.Editors;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Filters;
using Zeus.ContentTypes;
using System;

namespace Zeus.FileSystem.Images
{
    [ContentType("User Cropped Image")]
    [AdminSiteTreeVisibility(AdminSiteTreeVisibility.Hidden)]
    [ContentTypeAuthorizedRoles("Administrators")]
    public class CroppedImage : Image, AcceptArgsFromChildEditor
    {
        [CroppedImageUploadEditor("CroppedImage", 100)]
        public override byte[] Data
        {
            get { return base.Data; }
            set { base.Data = value; }
        }

        /* These are the base settings - not sure where they should sit...possibly in the editor code? */
        /*
        [Zeus.ContentProperty("Fixed Width Value", 210, Description = "0 is off")]
        public virtual int FixedWidthValue
        {
            get { return GetDetail("FixedWidthValue", 0); }
            set { SetDetail("FixedWidthValue", value); }
        }

        [Zeus.ContentProperty("Fixed Height Value", 220, Description = "0 is off")]
        public virtual int FixedHeightValue
        {
            get { return GetDetail("FixedHeightValue", 0); }
            set { SetDetail("FixedHeightValue", value); }
        }
        */
        /* Vals from jcrop... */

        public virtual int TopLeftXVal
        {
            get { return GetDetail("TopLeftXVal", 0); }
            set { SetDetail("TopLeftXVal", value); }
        }

        public virtual int TopLeftYVal
        {
            get { return GetDetail("TopLeftYVal", 0); }
            set { SetDetail("TopLeftYVal", value); }
        }

        public virtual int CropWidth
        {
            get { return GetDetail("CropWidth", 0); }
            set { SetDetail("CropWidth", value); }
        }

        public virtual int CropHeight
        {
            get { return GetDetail("CropHeight", 0); }
            set { SetDetail("CropHeight", value); }
        }

        public string GetUrl(int width, int height, bool fill, DynamicImageFormat format)
        {
            return GetUrl(width, height, fill, format, false);
        }

        public string GetUrl(int width, int height, bool fill, DynamicImageFormat format, bool isResize)
        {
            //first construct the crop
            var imageSource = new ZeusImageSource();
            imageSource.ContentID = this.ID;

            if (this.Data == null)
                return "";

            // generate resized image url
            // set image format
            var dynamicImage = new SoundInTheory.DynamicImage.DynamicImage();
            dynamicImage.ImageFormat = format;
            
            // create image layer wit ha source
            var imageLayer = new ImageLayer();
            imageLayer.Source.SingleSource = imageSource;

            // add filters
            if (!(TopLeftXVal == 0 && TopLeftYVal == 0 && CropWidth == 0 && CropHeight == 0))
            {
                var cropFilter = new CropFilter
                {
                    Enabled = true,
                    Name = "Default Crop",
                    X = this.TopLeftXVal,
                    Y = this.TopLeftYVal,
                    Width = this.CropWidth,
                    Height = this.CropHeight
                };
                if (!isResize)
                    imageLayer.Filters.Add(cropFilter);
            }

            if (width > 0 && height > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = isResize ? ResizeMode.Uniform : ResizeMode.UniformFill,
                    Width = SoundInTheory.DynamicImage.Unit.Pixel(width),
                    Height = SoundInTheory.DynamicImage.Unit.Pixel(height)
                };
                imageLayer.Filters.Add(resizeFilter);
            }
            else if (width > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = ResizeMode.UseWidth,
                    Width = SoundInTheory.DynamicImage.Unit.Pixel(width)
                };
                imageLayer.Filters.Add(resizeFilter);
            }
            else if (height > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = ResizeMode.UseHeight,
                    Height = SoundInTheory.DynamicImage.Unit.Pixel(height)
                };
                imageLayer.Filters.Add(resizeFilter);
            }

            // add the layer
            dynamicImage.Layers.Add(imageLayer);

            // generate url
            return dynamicImage.ImageUrl;
        }

        public string GetUrl()
        {
            return GetUrl(FixedWidthValue, FixedHeightValue, true, DynamicImageFormat.Jpeg, false);
        }

        public string ImageTag
        {
            get{
                return "<img src=\"" + GetUrl() + "\" alt=\"" + this.Caption + "\" />";
            }
        }
        
        #region AcceptArgsFromChildEditor Members

        public string arg1
        {
            get { return GetDetail("arg1", string.Empty); }
            set { SetDetail("arg1", value); }
        }

        public string arg2
        {
            get { return GetDetail("arg2", string.Empty); }
            set { SetDetail("arg2", value); }
        }

        public int FixedWidthValue
        {
            get { return Convert.ToInt32(arg1); }
            set { arg1 = value.ToString(); }
        }

        public int FixedHeightValue
        {
            get { return Convert.ToInt32(arg2); }
            set { arg2 = value.ToString(); }
        }
        
        #endregion
    }
}