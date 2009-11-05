using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public class CustomDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			var _default = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
			_default.IsRequired = _default.IsRequired || attributes.Where(x => x is RequiredAttribute).Count() > 0;
			return _default;
		}
	}
}