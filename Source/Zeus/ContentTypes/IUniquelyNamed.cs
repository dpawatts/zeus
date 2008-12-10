using System;

namespace Zeus.ContentTypes
{
	public interface IUniquelyNamed
	{
		/// <summary>Gets or sets the name of the prpoerty referenced by this attribute.</summary>
		string Name { get; set; }
	}
}
