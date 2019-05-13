using System;
using System.Net;
using System.IO;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Layers;
using SoundInTheory.DynamicImage.Sources;
using Zeus.ContentTypes;
using Zeus.Design.Editors;

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

        public new string GetUrl(int width, int height, bool fill, DynamicImageFormat format)
        {
            return GetUrl(width, height, fill, format, false);                
        }

        public string GetUrlForAdmin(int width, int height, bool fill, DynamicImageFormat format, bool isResize)
        {
            //first construct the crop
            var imageSource = new ZeusImageSource();
            imageSource.ContentID = this.ID;

            return GetUrlForAdminViaSource(imageSource, width, height, fill, format, isResize);
        }

        public string GetUrlForAdminViaSource(ImageSource source, int width, int height, bool fill, DynamicImageFormat format, bool isResize)
        {
            //see if it's the standard editor crop (from admin site most likely)
            bool isStandard = width == 800 & height == 600;

            if (this.Data == null)
                return "";

            // generate resized image url
            // set image format
            var dynamicImage = new SoundInTheory.DynamicImage.Composition();
            dynamicImage.ImageFormat = format;

            //create the background
            string optionalBackground = System.Web.HttpContext.Current.Server.MapPath("/assets/zeus/background.png");
            bool useBG = System.IO.File.Exists(optionalBackground);
            double percChangeForBG = 1;
            var bgLayer = new ImageLayer();
            if (useBG)
            {
                bgLayer.SourceFileName = optionalBackground;

                if (!isStandard)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(this.Data));
                    int ActualWidth = image.Width;
                    int ActualHeight = image.Height;
                    image.Dispose();

                    if ((Convert.ToDouble(ActualWidth) / Convert.ToDouble(800)) >= (Convert.ToDouble(ActualHeight) / Convert.ToDouble(600)))
                    {
                        percChangeForBG = (double)ActualWidth / (double)800;
                    }
                    else
                    {
                        percChangeForBG = (double)ActualHeight / (double)600;
                    }

                    if (percChangeForBG > 1)
                    {
                        var resizeBG = new ResizeFilter
                        {
                            Enabled = true,
                            Mode = ResizeMode.UniformFill,
                            EnlargeImage = true,
                            //has to change as per the original resize
                            Width = Unit.Pixel(Convert.ToInt32(Convert.ToDouble(1200) * percChangeForBG)),
                            Height = Unit.Pixel(Convert.ToInt32(Convert.ToDouble(900) * percChangeForBG)),
                        };

                        bgLayer.Filters.Add(resizeBG);
                    }
                }
            }

            // create image layer with a source
            var imageLayer = new ImageLayer();
            imageLayer.Source = source;

            // add filters
            if (!(TopLeftXVal == 0 && TopLeftYVal == 0 && CropWidth == 0 && CropHeight == 0))
            {
                var cropFilter = new CropFilter
                {
                    Enabled = true,
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

            // add the layer after resizing as it's being edited
            if (useBG)
            {
                dynamicImage.Layers.Add(bgLayer);
                if (percChangeForBG > 1)
                {
                    imageLayer.X = (Convert.ToInt32(Convert.ToDouble(200) * percChangeForBG));
                    imageLayer.Y = (Convert.ToInt32(Convert.ToDouble(150) * percChangeForBG));
                }
                else
                {
                    imageLayer.X = 200;
                    imageLayer.Y = 150;
                }
            }

            dynamicImage.Layers.Add(imageLayer);

            // generate url
            return ImageUrlGenerator.GetImageUrl(dynamicImage);
        }

        public static void Log(string what)
        {
            /*
            using (StreamWriter w = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("/log.txt")))
            {
                Log(what, w);
                // Close the writer and underlying file.
                w.Close();
            }
             */
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            // Update the underlying file.
            w.Flush();
        }

        public string GetUrl(int width, int height, bool fill, DynamicImageFormat format, bool isResize)
        {
            string appKey = "CroppedImage_" + this.ID + "_" + width + "_" + height + "_" + fill.ToString();
            string res = System.Web.HttpContext.Current.Cache[appKey] == null ? null : System.Web.HttpContext.Current.Cache[appKey].ToString();
            DateTime lastUpdated = res != null ? (DateTime)System.Web.HttpContext.Current.Cache[appKey + "_timer"] : DateTime.MinValue;

            if (res != null && lastUpdated == this.Updated)
            {
                Log("Image was retrieved from Cache - " + appKey);
                return res;
            }
            else
            {
                Log("Image originated - " + appKey + " (res was " + (res == null ? "null" : res) + " and timer was " + lastUpdated.ToShortTimeString() + ")");
            }

            if (this.TopLeftXVal == 0 && this.TopLeftYVal == 0 && this.CropWidth == 0 && this.CropHeight == 0)
            {
                string res2 = GetUrl(width, height, fill);

                System.Web.HttpContext.Current.Cache[appKey] = res2;
                System.Web.HttpContext.Current.Cache[appKey + "_timer"] = this.Updated;

                Log(res2 + " added to cache for " + appKey);

                return res2;
            }
                
            //see if it's the standard editor crop (from admin site most likely)
            bool isStandard = width == 800 & height == 600;

            //first construct the crop
            var imageSource = new ZeusImageSource();
            imageSource.ContentID = this.ID;

            if (this.Data == null)
                return "";

            // generate resized image url
            // set image format
            var dynamicImage = new SoundInTheory.DynamicImage.Composition();
            dynamicImage.ImageFormat = format;

            ImageLayer imageLayer = new ImageLayer();

            //create the background
            string optionalBackground = System.Web.HttpContext.Current.Server.MapPath("/assets/zeus/background.png");
            bool useBG = System.IO.File.Exists(optionalBackground);
            double percChangeForBG = 1;
            var bgLayer = new ImageLayer();
            if (useBG)
            {
                bgLayer.SourceFileName = optionalBackground;

                if (!isStandard)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(this.Data));
                    int ActualWidth = image.Width;
                    int ActualHeight = image.Height;
                    image.Dispose();

                    if ((Convert.ToDouble(ActualWidth) / Convert.ToDouble(800)) >= (Convert.ToDouble(ActualHeight) / Convert.ToDouble(600)))
                    {
                        percChangeForBG = (double)ActualWidth / (double)800;
                    }
                    else
                    {
                        percChangeForBG = (double)ActualHeight / (double)600;
                    }

                    if (percChangeForBG > 1)
                    {
                        var resizeBG = new ResizeFilter
                        {
                            Enabled = true,
                            Mode = ResizeMode.UniformFill,
                            EnlargeImage = true,
                            //has to change as per the original resize
                            Width = Unit.Pixel(Convert.ToInt32(Convert.ToDouble(1200) * percChangeForBG)),
                            Height = Unit.Pixel(Convert.ToInt32(Convert.ToDouble(900) * percChangeForBG)),
                        };

                        bgLayer.Filters.Add(resizeBG);
                    }
                }

                dynamicImage.Layers.Add(bgLayer);
                if (percChangeForBG > 1)
                {
                    imageLayer.X = (Convert.ToInt32(Convert.ToDouble(200) * percChangeForBG));
                    imageLayer.Y = (Convert.ToInt32(Convert.ToDouble(150) * percChangeForBG));
                }
                else
                {
                    imageLayer.X = 200;
                    imageLayer.Y = 150;
                }

            }

            // create image layer wit ha source
            imageLayer.Source = imageSource;

            //now combine the 2 layers...
            dynamicImage.Layers.Add(imageLayer);

            // generate url
            string halfWayFileName = ImageUrlGenerator.GetImageUrl(dynamicImage);

            var dynamicImage2 = new SoundInTheory.DynamicImage.Composition();

            var HalfwayImageSource = new ImageLayer();
            BytesImageSource sourceData = new BytesImageSource();
            var webClient = new WebClient();
            try
            {
                sourceData.Bytes = webClient.DownloadData("http://" + System.Web.HttpContext.Current.Request.Url.Host + halfWayFileName);
            }
            catch
            {
                return "Byte retrieval failed for " + halfWayFileName;
            }

            HalfwayImageSource.Source = sourceData;

            // add filters
            if (!(TopLeftXVal == 0 && TopLeftYVal == 0 && CropWidth == 0 && CropHeight == 0))
            {
                var cropFilter = new CropFilter
                {
                    Enabled = true,
                    X = this.TopLeftXVal,
                    Y = this.TopLeftYVal,
                    Width = this.CropWidth,
                    Height = this.CropHeight
                };
                if (!isResize)
                    HalfwayImageSource.Filters.Add(cropFilter);

                //finally resize both image and bg (if added)
                if (width > 0 && height > 0)
                {
                    var resizeFilter = new ResizeFilter
                    {
                        Mode = isResize ? ResizeMode.Uniform : ResizeMode.UniformFill,
                        Width = SoundInTheory.DynamicImage.Unit.Pixel(width),
                        Height = SoundInTheory.DynamicImage.Unit.Pixel(height)
                    };

                    HalfwayImageSource.Filters.Add(resizeFilter);
                }
                else if (width > 0)
                {
                    var resizeFilter = new ResizeFilter
                    {
                        Mode = ResizeMode.UseWidth,
                        Width = SoundInTheory.DynamicImage.Unit.Pixel(width)
                    };
                    HalfwayImageSource.Filters.Add(resizeFilter);
                }
                else if (height > 0)
                {
                    var resizeFilter = new ResizeFilter
                    {
                        Mode = ResizeMode.UseHeight,
                        Height = SoundInTheory.DynamicImage.Unit.Pixel(height)
                    };
                    HalfwayImageSource.Filters.Add(resizeFilter);
                }
            }

            dynamicImage2.Layers.Add(HalfwayImageSource);

            string imageUrl = ImageUrlGenerator.GetImageUrl(dynamicImage2);

            System.Web.HttpContext.Current.Cache[appKey] = imageUrl;
            System.Web.HttpContext.Current.Cache[appKey + "_timer"] = this.Updated;
            Log(imageUrl + " added to cache for " + appKey);

            return imageUrl;
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