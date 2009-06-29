using System.Globalization;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Globalization
{
	public class LanguageSelectorContext
	{
		// Fields
		private readonly ContentItem _page;
		private string _selectedLanguage;
		private Language _selectedLanguageBranch;

		// Methods
		public LanguageSelectorContext(ContentItem page)
		{
			_page = page;
		}

		public bool IsLanguagePublished(string language)
		{
			if (_page == null)
				throw new ZeusException("IsLanguagePublished is only supported in SelectPageLanguage where a page must exist");

			ContentItem pageLanguage = Context.Current.LanguageManager.GetTranslationDirect(_page, language);
			if (pageLanguage == null)
				return false;

			return pageLanguage.IsPublished();
		}

		// Properties
		public string MasterLanguageBranch
		{
			get
			{
				if (_page != null && _page.TranslationOf != null && !string.IsNullOrEmpty(_page.TranslationOf.Language))
					return CultureInfo.GetCultureInfo(_page.TranslationOf.Language).Name;
				return null;
			}
		}

		public string PageLanguage
		{
			get
			{
				if (_page != null && !string.IsNullOrEmpty(_page.Language))
					return CultureInfo.GetCultureInfo(_page.Language).Name;
				return null;
			}
		}

		public ContentItem Page
		{
			get { return _page; }
		}

		public ContentItem ParentLink
		{
			get
			{
				if (_page != null)
					return _page.Parent;
				return null;
			}
		}

		public string SelectedLanguage
		{
			get
			{
				return _selectedLanguage;
			}
			set
			{
				_selectedLanguage = value;
				_selectedLanguageBranch = null;
			}
		}

		public Language SelectedLanguageBranch
		{
			get
			{
				if (_selectedLanguageBranch == null && _selectedLanguage != null)
				{
					_selectedLanguageBranch = Context.Current.LanguageManager.GetLanguage(_selectedLanguage);
					if (_selectedLanguageBranch == null)
						throw new ZeusException("Invalid language '" + _selectedLanguage + "' selected.");
				}
				return _selectedLanguageBranch;
			}
		}
	}
}