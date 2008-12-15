using System;
using Zeus.Integrity;
using System.Collections.Generic;
using Zeus.Linq.Filters;
using Zeus.FileSystem;

namespace Zeus.Admin.FileSystem.Items
{
	public abstract class AbstractDirectory : ContentItem
	{
		public override string Title
		{
			get { return this.Name; }
			set { this.Name = value; }
		}

		public override IList<ContentItem> GetChildren(ItemFilter filter)
		{
			IFileSystem fileSystem = Zeus.Context.Current.Resolve<IFileSystem>();
			IFolder folder = fileSystem.GetFolder(this.Name);

			List<ContentItem> items = new List<ContentItem>();
			items.AddRange(filter.Filter(GetDirectories()));
			items.AddRange(filter.Filter(GetFiles()));
			return items;
		}
	}
}
