using System;

namespace Zeus.ContentTypes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class EditorContainerAttribute : Attribute
	{
	}
}
