namespace Zeus.Templates.ContentTypes.News
{
	public abstract class BaseNewsPage : BasePage
	{
		public abstract NewsContainer CurrentNewsContainer { get; }
	}
}