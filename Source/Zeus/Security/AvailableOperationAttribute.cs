using System;

namespace Zeus.Security
{
	public class AvailableOperationAttribute : Attribute
	{
		#region Constructor

		public AvailableOperationAttribute(string name, string title, int sortOrder)
		{
			Name = name;
			Title = title;
			SortOrder = sortOrder;
		}

		#endregion

		#region Properties

		public string Name { get; set; }
		public string Title { get; set; }
		public int SortOrder { get; set; }

		#endregion
	}
}