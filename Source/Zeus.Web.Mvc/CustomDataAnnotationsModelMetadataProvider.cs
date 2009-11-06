using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public class CustomDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			var defaultMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
			defaultMetadata.IsRequired = defaultMetadata.IsRequired || attributes.Where(x => x is RequiredAttribute).Any();
			defaultMetadata.HideSurroundingChrome = defaultMetadata.HideSurroundingChrome || attributes.Where(x => x is HideSurroundingChromeAttribute).Any();
			if (string.IsNullOrEmpty(defaultMetadata.Description) && attributes.Where(x => x is DescriptionAttribute).Any())
				defaultMetadata.Description = attributes.OfType<DescriptionAttribute>().First().Description;
			return defaultMetadata;
		}
	}
}