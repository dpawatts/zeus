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
    //the editor when this isn't hidden needs to understand that the crop values will be in the parent object

    [ContentType("User Cropped Image")]
    public class CroppedImage : Image, AcceptArgsFromChildEditor
    {
        [CroppedImageUploadEditor("CroppedImage", 100)]
        public override byte[] Data
        {
            get { return base.Data; }
            set { base.Data = value; }
        }

        public override string IconUrl
        {
            get
            {
                return Utility.GetCooliteIconUrl(Ext.Net.Icon.PictureEdit);
            }
        }

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

        public virtual string arg1
        {
            get { return (Parent as ParentWithCroppedImageValues).Width.ToString(); }
            set { }
        }

        public virtual string arg2
        {
            get { return (Parent as ParentWithCroppedImageValues).Height.ToString(); }
            set { }
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

    public interface ParentWithCroppedImageValues
    {
        int Width { get; }
        int Height { get; }
    }
}