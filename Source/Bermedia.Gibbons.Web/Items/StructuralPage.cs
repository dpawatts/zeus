using System;
using Zeus.ContentTypes.Properties;
using Zeus;
using Zeus.Web.UI.WebControls;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Items
{
	[TabPanel(Tabs.General, "General", 0)]
	public abstract class StructuralPage : ContentItem
	{
		[TextBoxEditor("Title", 10, ContainerName = Tabs.General, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[NameEditor("Name", 20, ContainerName = Tabs.General)]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}

		public override string IconUrl
		{
			get { return "~/Assets/Images/Icons/" + this.IconName + ".png"; }
		}

		protected abstract string IconName { get; }

		public override string TemplateUrl
		{
			get { return "~/UI/Views/" + this.TemplateName + ".aspx"; }
		}

		protected virtual string TemplateName
		{
			get { return this.GetType().Name; }
		}
	}
}
