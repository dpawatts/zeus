namespace Zeus.Templates.ContentTypes.Forms
{
	[ContentType("Multiple Select (checkboxes)")]
	public class MultipleSelectQuestion : OptionSelectQuestion
	{
		[ContentProperty("Display Vertically", 110)]
		public virtual bool Vertical
		{
			get { return GetDetail("Vertical", true); }
			set { SetDetail("Vertical", value); }
		}
	}
}