using System;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// When used on a content type this attribute prevents it from being 
	/// moved to the recycle bin upon deletion.
	/// </summary>
	public class NotThrowableAttribute : Attribute
	{
	}
}