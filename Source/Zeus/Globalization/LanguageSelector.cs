namespace Zeus.Globalization
{
	public class LanguageSelector : ILanguageSelector
	{
		// Fields
		private bool _auto;
		private bool _fallBackToMaster;
		private readonly string _languageBranch;

		// Methods
		protected LanguageSelector()
			: this(null)
		{
		}

		public LanguageSelector(string languageBranch)
		{
			_languageBranch = languageBranch;
			_auto = false;
			_fallBackToMaster = false;
		}

		public static ILanguageSelector AutoDetect()
		{
			return AutoDetect(false);
		}

		public static ILanguageSelector AutoDetect(bool enableMasterLanguageFallback)
		{
			return FallbackInternal(ContentLanguage.PreferredCulture.Name, enableMasterLanguageFallback, true);
		}

		public static ILanguageSelector Fallback(string preferredLanguageBranch, bool enableMasterLanguageFallback)
		{
			return FallbackInternal(preferredLanguageBranch, enableMasterLanguageFallback, false);
		}

		private static ILanguageSelector FallbackInternal(string preferredLanguageBranch, bool enableMasterLanguageFallback, bool autoDetect)
		{
			return new LanguageSelector(preferredLanguageBranch)
			{
				_auto = autoDetect,
				_fallBackToMaster = enableMasterLanguageFallback
			};
		}

		public virtual void LoadLanguage(LanguageSelectorContext context)
		{
			context.SelectedLanguage = _languageBranch;
			if (_languageBranch != null && _auto)
			{
				ContentItem translation = Context.Current.LanguageManager.GetTranslation(context.Page, context.SelectedLanguage);
				if (translation != null)
					context.SelectedLanguage = translation.Language;
				else
					context.SelectedLanguage = _fallBackToMaster ? context.MasterLanguageBranch : null;
			}
		}

		public static ILanguageSelector MasterLanguage()
		{
			return new LanguageSelector();
		}

		public virtual void SelectPageLanguage(LanguageSelectorContext context)
		{
			context.SelectedLanguage = _languageBranch ?? context.MasterLanguageBranch;
			if (_auto)
			{
				ContentItem translation = Context.Current.LanguageManager.GetTranslation(context.Page, context.SelectedLanguage);
				if (translation != null)
					context.SelectedLanguage = translation.Language;
				else
					context.SelectedLanguage = _fallBackToMaster ? context.MasterLanguageBranch : null;
			}
		}

		public virtual void SetInitializedLanguageBranch(LanguageSelectorContext args)
		{
			if (!_auto && !_fallBackToMaster)
				args.SelectedLanguage = _languageBranch;
		}
	}
}