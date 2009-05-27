using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.Forms
{
	[RestrictParents(typeof(Form))]
	[AllowedZones("Questions", "")]
	public abstract class Question : BaseWidget
	{
		[TextBoxEditor("Title", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}