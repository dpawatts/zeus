using System;
using Zeus;
using Zeus.Web.UI;

namespace Bermedia.Gibbons.Items
{
	public abstract class BaseContentItem : ContentItem
	{
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
