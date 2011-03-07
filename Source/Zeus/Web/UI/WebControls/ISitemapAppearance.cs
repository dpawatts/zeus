namespace Zeus.Web.UI.WebControls
{
	/// <summary>
	/// When applied to a content item this interface helps the breadcrumbs control to 
	/// determine the item appearance in the <see cref="Breadcrumbs"/> control.
	/// </summary>
	public interface ISitemapAppearance
	{
		bool VisibleInSitemap { get; }
	}
}