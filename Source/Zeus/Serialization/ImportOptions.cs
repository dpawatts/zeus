using System;

namespace Zeus.Serialization
{
	[Flags]
	public enum ImportOptions
	{
		/// <summary>All items.</summary>
		AllItems = Root | Children,

		/// <summary>The root node.</summary>
		Root = 1,

		/// <summary>All items except the root node.</summary>
		Children = 2
	}
}