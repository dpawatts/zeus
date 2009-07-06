using System;
using System.Collections.Generic;
using Zeus.ContentTypes;

namespace Zeus.Globalization
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TranslatableAttribute : Attribute, IInheritableDefinitionRefiner
	{
		public TranslatableAttribute()
			: this(true)
		{
			
		}

		public TranslatableAttribute(bool translatable)
		{
			Translatable = translatable;
		}

		public bool Translatable { get; set; }

		public void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			currentDefinition.Translatable = Translatable;
		}
	}
}