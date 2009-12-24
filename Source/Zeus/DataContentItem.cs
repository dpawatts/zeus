namespace Zeus
{
	public abstract class DataContentItem : ContentItem
	{
		public override bool IsPage
		{
			get { return false; }
		}
	}
}