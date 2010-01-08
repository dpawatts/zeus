using Coolite.Ext.Web;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType("Member Container")]
	[RestrictParents(typeof(MessageBoard))]
	public class MemberContainer : ContentItem
	{
		public MemberContainer()
		{
			Visible = false;
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.UserComment); }
		}

		[TextBoxEditor("Title", 10)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[NameEditor("Name", 20)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		/// <summary>
		/// Here as an optimisation so we don't need to query the DB
		/// every time we display the forum list page
		/// </summary>
		public virtual int TotalMembers
		{
			get { return GetDetail("TotalMembers", 0); }
			set { SetDetail("TotalMembers", value); }
		}
	}
}