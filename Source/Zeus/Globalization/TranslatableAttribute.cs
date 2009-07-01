using System;
using System.Collections.Generic;
using Zeus.ContentTypes;

namespace Zeus.Globalization
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TranslatableAttribute : Attribute, IInheritableDefinitionRefiner
	{
		public void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			currentDefinition.Translatable = true;
		}
	}
}