using System;
using System.Reflection;
using System.Web.UI;
using Zeus.BaseLibrary.Reflection;

namespace Zeus.Web.UI.WebControls
{
	internal class FakePropertyInfo : PropertyInfoDelegator
	{
		private readonly Type _templateContainerType;

		public FakePropertyInfo(PropertyInfo real, Type templateContainerType)
			: base(real)
		{
			_templateContainerType = templateContainerType;
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			if (attributeType == typeof(TemplateContainerAttribute))
				return new Attribute[] { new TemplateContainerAttribute(_templateContainerType) };

			return base.GetCustomAttributes(attributeType, inherit);
		}
	}
}