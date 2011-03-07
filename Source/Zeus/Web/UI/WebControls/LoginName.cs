using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	[DefaultProperty("FormatString"), Bindable(false)]
	public class LoginName : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(UserName))
				base.Render(writer);
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(UserName))
				base.RenderBeginTag(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			string userName = UserName;
			if (!string.IsNullOrEmpty(userName))
			{
				userName = HttpUtility.HtmlEncode(userName);
				string formatString = FormatString;
				if (formatString.Length == 0)
				{
					writer.Write(userName);
				}
				else
				{
					try
					{
						writer.Write(string.Format(formatString, userName));
					}
					catch (FormatException exception)
					{
						throw new FormatException("FormatString is not a valid format string.", exception);
					}
				}
			}
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(UserName))
				base.RenderEndTag(writer);
		}

		[Localizable(true), DefaultValue("{0}")]
		public virtual string FormatString
		{
			get { return ViewState["FormatString"] as string ?? "{0}"; }
			set { ViewState["FormatString"] = value; }
		}

		internal string UserName
		{
			get { return (Page != null && Page.User != null && Page.User.Identity != null) ? Page.User.Identity.Name : null; }
		}
	}
}