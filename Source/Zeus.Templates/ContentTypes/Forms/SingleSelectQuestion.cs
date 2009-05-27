namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Single Select (radio buttons)")]
	public class SingleSelectQuestion : OptionSelectQuestion
	{
		[ContentProperty("Display Vertically", 110)]
		public virtual bool Vertical
		{
			get { return GetDetail("Vertical", true); }
			set { SetDetail("Vertical", value); }
		}
	}
}