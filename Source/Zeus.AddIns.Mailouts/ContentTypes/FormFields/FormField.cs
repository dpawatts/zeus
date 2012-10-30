using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes.FormFields
{
	[RestrictParents(typeof(List))]
	public abstract class FormField : ContentItem
	{
		[TextBoxEditor("Field Title", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("Field Name", 15)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[TextBoxEditor("Group Name", 18)]
		public virtual string GroupName
		{
			get { return GetDetail("GroupName", string.Empty); }
			set { SetDetail("GroupName", value); }
		}

		[CheckBoxEditor("Required", "", 20)]
		public virtual bool Required
		{
			get { return GetDetail("Required", false); }
			set { SetDetail("Required", value); }
		}
	}
}