using System.Collections.Generic;

namespace Zeus.ContentTypes
{
	/// <summary>
	/// Controls whether a content type is displayed in the admin site tree.
	/// </summary>
	public class AdminSiteTreeVisibilityAttribute : AbstractContentTypeRefiner, IDefinitionRefiner
	{
		public AdminSiteTreeVisibility Visibility { get; set; }

		public AdminSiteTreeVisibilityAttribute(AdminSiteTreeVisibility visibility)
		{
			Visibility = visibility;
		}

		public override void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			currentDefinition.Visibility = Visibility;
		}
	}
}