using System;
using System.Collections.Generic;
using Zeus.Linq.Filters;
using System.Linq;

namespace Zeus.FileSystem
{
	public abstract class Folder : FileSystemNode
	{
		protected abstract IList<ContentItem> GetDirectories();
		protected abstract IList<ContentItem> GetFiles();

		public override IList<ContentItem> GetChildren(ItemFilter filter)
		{
			List<ContentItem> items = new List<ContentItem>();
			items.AddRange(filter.Filter(GetDirectories().AsQueryable()));
			items.AddRange(filter.Filter(GetFiles().AsQueryable()));
			return items;
		}
	}
}
