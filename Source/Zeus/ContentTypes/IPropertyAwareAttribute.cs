using System.Reflection;

namespace Zeus.ContentTypes
{
	public interface IPropertyAwareAttribute
	{
		PropertyInfo UnderlyingProperty { get; set; }
	}
}