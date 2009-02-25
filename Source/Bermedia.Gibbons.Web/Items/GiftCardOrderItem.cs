using System;
using Zeus;
using Zeus.Integrity;
using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Sources;
using SoundInTheory.DynamicImage.Filters;
using Zeus.AddIns.Images;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(Order))]
	public class GiftCardOrderItem : BaseOrderItem
	{
		public string Description
		{
			get { return GetDetail<string>("Description", string.Empty); }
			set { SetDetail<string>("Description", value); }
		}

		public override string ProductTitle
		{
			get { return this.Description; }
		}

		public override string ProductSizeTitle
		{
			get { return string.Empty; }
		}

		public override string ProductColourTitle
		{
			get { return string.Empty; }
		}

		public override decimal PricePerUnit
		{
			get { return GetDetail<decimal>("PricePerUnit", 0); }
			set { SetDetail<decimal>("PricePerUnit", value); }
		}

		public Zeus.AddIns.Images.Items.Image Image
		{
			get { return GetDetail<Zeus.AddIns.Images.Items.Image>("Image", null); }
			set { SetDetail<Zeus.AddIns.Images.Items.Image>("Image", value); }
		}

		public override string FullImageUrl
		{
			get { return GetImageUrl(10000); }
		}

		public override string ThumbnailImageUrl
		{
			get { return GetImageUrl(150); }
		}

		private string GetImageUrl(int width)
		{
			return new DynamicImage
			{
				Layers = new LayerCollection
					{
						new ImageLayer
						{
							Source = new ImageSourceCollection
							{
								new ZeusImageSource { ContentID = this.Image.ID }
							},
							Filters = new FilterCollection
							{
								new ResizeFilter { Width = Unit.Pixel(width), Mode = ResizeMode.UseWidth }
							}
						}
					}
			}.ImageUrl;
		}
	}
}
