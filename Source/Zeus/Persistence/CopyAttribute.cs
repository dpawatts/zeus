using System;

namespace Zeus.Persistence
{
	/// <summary>Used to mark properties or fields that SHOULD be copied when making a version of a content item.</summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	internal class CopyAttribute : Attribute
	{
	}
}