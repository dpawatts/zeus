using System;
using Zeus;
using Zeus.Integrity;
using Zeus.ContentTypes.Properties;

namespace Bermedia.Gibbons.Web.Items
{
	[ContentType(Description = "[Internal Use Only]")]
	[RestrictParents(typeof(StartPage))]
	public class RegisterPage : StructuralPage
	{
		protected override string IconName
		{
			get { return "page"; }
		}

		protected override string TemplateName
		{
			get { return "Register"; }
		}
	}
}
