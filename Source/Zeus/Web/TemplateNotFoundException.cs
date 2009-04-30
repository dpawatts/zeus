using System;

namespace Zeus.Web
{
	/// <summary>
	/// Exception thrown when a template associated with a content is not 
	/// found.
	/// </summary>
	public class TemplateNotFoundException : ZeusException
	{
		/// <summary>Creates a new instance of the TemplateNotFoundException.</summary>
		/// <param name="item">The item whose templates wasn't found.</param>
		public TemplateNotFoundException(ContentItem item)
			: base("Item template not found, id:{0}", item.ID)
		{
			Item = item;
		}

		/// <summary>Gets the content item associated with this exception.</summary>
		public ContentItem Item
		{
			get; private set;
		}
	}
}
