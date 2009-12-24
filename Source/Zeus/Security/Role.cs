using Coolite.Ext.Web;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Web.Security.Items
{
	[ContentType]
	[RestrictParents(typeof(RoleContainer))]
	public class Role : DataContentItem
	{
		public override string Title
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		[TextBoxEditor("Role", 20, Required = true)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.BulletKey); }
		}
	}
}
