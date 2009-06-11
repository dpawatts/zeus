using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Zeus.AddIns.Mailouts.ContentTypes.FormFieldAnswers;
using Zeus.AddIns.Mailouts.Services;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Mailouts.ContentTypes
{
	[ContentType("List Member")]
	[RestrictParents(typeof(List))]
	public class ListMember : ContentItem, IMailoutRecipient
	{
		#region Properties

		public override string IconUrl
		{
			get { return new Page().ClientScript.GetWebResourceUrl(typeof(ListMember), "Zeus.AddIns.Mailouts.Resources.user_go.png"); }
		}

		[TextBoxEditor("Email", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[ChildrenEditor("Answers", 20, AddNewText = "Add New Answer", TypeFilter = typeof(FormFieldAnswer))]
		public virtual IEnumerable<FormFieldAnswer> Fields
		{
			get { return GetChildren<FormFieldAnswer>(); }
		}

		public virtual PropertyCollection InterestGroups
		{
			get { return GetDetailCollection("InterestGroups", true); }
		}

		#endregion

		#region IMailoutRecipient members

		string IMailoutRecipient.Email
		{
			get { return Title; }
			set { Title = value; }
		}

		IDictionary<string, FormFieldAnswer> IMailoutRecipient.Fields
		{
			get { return Fields.ToDictionary(a => a.FormField.Name); }
		}

		IEnumerable<string> IMailoutRecipient.InterestGroups
		{
			get { return InterestGroups.Cast<string>(); }
		}

		#endregion
	}
}