using System;
using Isis;

namespace Zeus.ContentProperties
{
	[AttributeUsage(AttributeTargets.Class)]
	public abstract class BasePropertyDataTypeAttribute : Attribute, IContextAwareAttribute
	{
		public Type ContextType { get; private set; }

		public int SortOrder { get; set; }
		public abstract bool IsDefaultPropertyDataTypeForType(Type type);

		public void SetContext(object context)
		{
			ContextType = context as Type;
		}
	}
}