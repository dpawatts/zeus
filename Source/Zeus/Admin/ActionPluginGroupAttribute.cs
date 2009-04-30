using System;

namespace Zeus.Admin
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly)]
	public class ActionPluginGroupAttribute : Attribute
	{
		public ActionPluginGroupAttribute(string name, int sortOrder)
		{
			Name = name;
			SortOrder = sortOrder;
		}

		public string Name { get; set; }
		public int SortOrder { get; set; }
	}
}
