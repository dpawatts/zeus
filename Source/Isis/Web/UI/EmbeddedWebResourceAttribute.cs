using System;

namespace Isis.Web.UI
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class EmbeddedWebResourceAttribute : Attribute
	{
		public EmbeddedWebResourceAttribute(string resourcePath, string resourceNamespace, string contentType)
		{
			ResourcePath = resourcePath;
			ResourceNamespace = resourceNamespace;
			ContentType = contentType;
		}

		public string ContentType { get; set; }
		public string ResourceNamespace { get; set; }
		public string ResourcePath { get; set; }
	}
}