using System;

namespace Zeus.Serialization
{
	/// <summary>
	/// Hints for the export service.
	/// </summary>
	[Flags]
	public enum ExportOptions
	{
		/// <summary>Default / Export everything.</summary>
		Default = 0,

		/// <summary>Don't export properties which have no definition.</summary>
		OnlyDefinedProperties = 1
	}
}