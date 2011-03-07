using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;
using Zeus.BaseLibrary.Web;
using Zeus.Web.Security;
using CommandEventArgs=System.Web.UI.WebControls.CommandEventArgs;
using CommandEventHandler=System.Web.UI.WebControls.CommandEventHandler;
using CompositeControl=System.Web.UI.WebControls.CompositeControl;
using ImageButton=System.Web.UI.WebControls.ImageButton;
using LinkButton=System.Web.UI.WebControls.LinkButton;
using LoginCancelEventArgs=System.Web.UI.WebControls.LoginCancelEventArgs;
using WebControl=System.Web.UI.WebControls.WebControl;

namespace Zeus.Web.UI.WebControls
{
	[DefaultEvent("LoggingOut"), Bindable(false)]
	public class LoginStatus : CompositeControl
	{
		// Fields
		private ImageButton _logInImageButton;
		private LinkButton _logInLinkButton;
		private ImageButton _logOutImageButton;
		private LinkButton _logOutLinkButton;

		// Events
		public event EventHandler LoggedOut;
		public event EventHandler LoggingOut;

		// Methods
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this._logInLinkButton = new LinkButton();
			this._logInImageButton = new ImageButton();
			this._logOutLinkButton = new LinkButton();
			this._logOutImageButton = new ImageButton();
			this._logInLinkButton.EnableViewState = false;
			this._logInImageButton.EnableViewState = false;
			this._logOutLinkButton.EnableViewState = false;
			this._logOutImageButton.EnableViewState = false;
			this._logInLinkButton.EnableTheming = false;
			this._logInImageButton.EnableTheming = false;
			this._logInLinkButton.CausesValidation = false;
			this._logInImageButton.CausesValidation = false;
			this._logOutLinkButton.EnableTheming = false;
			this._logOutImageButton.EnableTheming = false;
			this._logOutLinkButton.CausesValidation = false;
			this._logOutImageButton.CausesValidation = false;
			CommandEventHandler handler = new CommandEventHandler(this.LogoutClicked);
			this._logOutLinkButton.Command += handler;
			this._logOutImageButton.Command += handler;
			handler = new CommandEventHandler(this.LoginClicked);
			this._logInLinkButton.Command += handler;
			this._logInImageButton.Command += handler;
			this.Controls.Add(this._logOutLinkButton);
			this.Controls.Add(this._logOutImageButton);
			this.Controls.Add(this._logInLinkButton);
			this.Controls.Add(this._logInImageButton);
		}

		private void LoginClicked(object Source, CommandEventArgs e)
		{
			this.Page.Response.Redirect(base.ResolveClientUrl(this.NavigateUrl), false);
		}

		private static IAuthenticationService CurrentAuthenticationService
		{
			get { return WebSecurityEngine.Get<IAuthenticationContextService>().GetCurrentService(); }
		}

		private void LogoutClicked(object Source, CommandEventArgs e)
		{
			LoginCancelEventArgs args = new LoginCancelEventArgs();
			this.OnLoggingOut(args);
			if (!args.Cancel)
			{
				CurrentAuthenticationService.SignOut();
				this.Page.Response.Clear();
				this.Page.Response.StatusCode = 200;
				this.OnLoggedOut(EventArgs.Empty);
				switch (this.LogoutAction)
				{
					case System.Web.UI.WebControls.LogoutAction.Refresh:
						if ((this.Page.Form == null) || !string.Equals(this.Page.Form.Method, "get", StringComparison.OrdinalIgnoreCase))
						{
							this.Page.Response.Redirect(new Url(Page.Request.RawUrl).PathAndQuery, false);
							return;
						}
						this.Page.Response.Redirect(this.Page.Request.Path, false);
						return;

					case System.Web.UI.WebControls.LogoutAction.Redirect:
						{
							string logoutPageUrl = this.LogoutPageUrl;
							if (string.IsNullOrEmpty(logoutPageUrl))
								logoutPageUrl = CurrentAuthenticationService.LoginUrl;
							else
								logoutPageUrl = base.ResolveClientUrl(logoutPageUrl);
							this.Page.Response.Redirect(logoutPageUrl, false);
							return;
						}
					case System.Web.UI.WebControls.LogoutAction.RedirectToLoginPage:
						this.Page.Response.Redirect(CurrentAuthenticationService.LoginUrl, false);
						return;
				}
			}
		}

		protected virtual void OnLoggedOut(EventArgs e)
		{
			if (LoggedOut != null)
				LoggedOut(this, e);
		}

		protected virtual void OnLoggingOut(LoginCancelEventArgs e)
		{
			if (LoggingOut != null)
				LoggingOut(this, e);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			LoggedIn = Page.Request.IsAuthenticated;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			RenderContents(writer);
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			if (Page != null)
			{
				Page.VerifyRenderingInServerForm(this);
			}
			SetChildProperties();
			if (!string.IsNullOrEmpty(ID))
				writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			base.RenderContents(writer);
		}

		private void SetChildProperties()
		{
			EnsureChildControls();
			this._logInLinkButton.Visible = false;
			this._logInImageButton.Visible = false;
			this._logOutLinkButton.Visible = false;
			this._logOutImageButton.Visible = false;
			WebControl control = null;
			if (LoggedIn)
			{
				string logoutImageUrl = this.LogoutImageUrl;
				if (logoutImageUrl.Length > 0)
				{
					this._logOutImageButton.AlternateText = this.LogoutText;
					this._logOutImageButton.ImageUrl = logoutImageUrl;
					control = this._logOutImageButton;
				}
				else
				{
					this._logOutLinkButton.Text = this.LogoutText;
					control = this._logOutLinkButton;
				}
			}
			else
			{
				string loginImageUrl = this.LoginImageUrl;
				if (loginImageUrl.Length > 0)
				{
					this._logInImageButton.AlternateText = this.LoginText;
					this._logInImageButton.ImageUrl = loginImageUrl;
					control = this._logInImageButton;
				}
				else
				{
					this._logInLinkButton.Text = this.LoginText;
					control = this._logInLinkButton;
				}
			}
			control.CopyBaseAttributes(this);
			control.ApplyStyle(base.ControlStyle);
			control.Visible = true;
		}

		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj2 = data["LoggedIn"];
				if (obj2 != null)
				{
					this.LoggedIn = (bool) obj2;
				}
			}
		}

		// Properties
		private bool LoggedIn { get; set; }

		[DefaultValue(""), UrlProperty, Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string LoginImageUrl
		{
			get { return ViewState["LoginImageUrl"] as string ?? string.Empty; }
			set { ViewState["LoginImageUrl"] = value; }
		}

		[Localizable(true)]
		public virtual string LoginText
		{
			get { return ViewState["LoginText"] as string ?? "Login"; }
			set { ViewState["LoginText"] = value; }
		}

		[DefaultValue(0), Themeable(false)]
		public virtual System.Web.UI.WebControls.LogoutAction LogoutAction
		{
			get { return (System.Web.UI.WebControls.LogoutAction) (ViewState["LogoutAction"] ?? System.Web.UI.WebControls.LogoutAction.Refresh); }
			set
			{
				if ((value < System.Web.UI.WebControls.LogoutAction.Refresh) || (value > System.Web.UI.WebControls.LogoutAction.RedirectToLoginPage))
					throw new ArgumentOutOfRangeException("value");
				ViewState["LoginText"] = value;
			}
		}

		[Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), UrlProperty, DefaultValue("")]
		public virtual string LogoutImageUrl
		{
			get { return ViewState["LogoutImageUrl"] as string ?? string.Empty; }
			set { ViewState["LogoutImageUrl"] = value; }
		}

		[Themeable(false), DefaultValue(""), UrlProperty, Editor("System.Web.UI.Design.UrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public virtual string LogoutPageUrl
		{
			get { return ViewState["LogoutPageUrl"] as string ?? string.Empty; }
			set { ViewState["LogoutPageUrl"] = value; }
		}

		[Localizable(true)]
		public virtual string LogoutText
		{
			get { return ViewState["LogoutText"] as string ?? "Logout"; }
			set { ViewState["LogoutText"] = value; }
		}

		private string NavigateUrl
		{
			get
			{
				if (!base.DesignMode)
					return CurrentAuthenticationService.GetLoginPage(null, true);
				return "url";
			}
		}

		protected override HtmlTextWriterTag TagKey
		{
			get { return HtmlTextWriterTag.A; }
		}
	}
}