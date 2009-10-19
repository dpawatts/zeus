using System.Collections.Generic;
using System.Linq;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.FileSystem.Images;
using Zeus.Globalization.ContentTypes;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.Globalization
{
	public class LanguageManager : ILanguageManager
	{
		private readonly IPersister _persister;
		private readonly IHost _host;
		private readonly IContentTypeManager _contentTypeManager;
		private readonly GlobalizationSection _globalizationConfig;

		#region Constructor

		public LanguageManager(IPersister persister,
			IHost host, IContentTypeManager contentTypeManager)
			: this(persister, host, contentTypeManager, null)
		{

		}

		public LanguageManager(IPersister persister, 
			IHost host, IContentTypeManager contentTypeManager,
			GlobalizationSection globalizationConfig)
		{
			_persister = persister;
			_host = host;
			_contentTypeManager = contentTypeManager;
			_globalizationConfig = globalizationConfig;
		}

		#endregion

		#region Properties

		protected int LanguageContainerParentID
		{
			get { return _host.CurrentSite.RootItemID; }
		}

		#endregion

		public IEnumerable<Language> GetAvailableLanguages(bool create)
		{
			LanguageContainer container = GetLanguageContainer(create);
			if (container != null)
				return container.GetChildren<Language>().Where(l => l.Enabled);
			return new List<Language>();
		}

		public IEnumerable<Language> GetAvailableLanguages()
		{
			return GetAvailableLanguages(true);
		}

		/// <summary>
		/// Uses fallbacks rules to get other language versions if the requested language is not available.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <param name="languageCode"></param>
		/// <returns></returns>
		public ContentItem GetTranslation(ContentItem contentItem, string languageCode)
		{
			// If item can't be translated, return it immediately.
			if (!CanBeTranslated(contentItem))
				return contentItem;

			// Get original item.
			if (contentItem.TranslationOf != null)
				contentItem = contentItem.TranslationOf;

			// 1. If the content item is available in the requested language, return that translation.
			ContentItem translatedItem = GetTranslationDirect(contentItem, languageCode);
			if (translatedItem != null)
				return translatedItem;

			// 2. If the language settings for the current item indicate that we can fallback
			// to another language, check if a translation for that fallback language exists.
			LanguageSetting languageSetting = GetLanguageSetting(contentItem, languageCode);
			if (languageSetting != null && !string.IsNullOrEmpty(languageSetting.FallbackLanguage))
			{
				translatedItem = GetTranslationDirect(contentItem, languageSetting.FallbackLanguage);
				if (translatedItem != null)
					return translatedItem;
			}

			// 3. TODO - replacements languages.

			// 4. If content item has not been localised, just return it.
			if (string.IsNullOrEmpty(contentItem.Language))
				return contentItem;

			// 5. We don't have anything to show to the user.
			return null;
		}

		private LanguageSetting GetLanguageSetting(ContentItem contentItem, string languageCode)
		{
			// Iterate upward until we find a content item with some language settings.
			while (contentItem != null)
			{
				if (contentItem.LanguageSettings != null && contentItem.LanguageSettings.Any())
					break;
				contentItem = contentItem.Parent;
			}
			if (contentItem == null)
				return null;
			return contentItem.LanguageSettings.SingleOrDefault(ls => ls.Language == languageCode);
		}

		public ContentItem GetTranslationDirect(ContentItem contentItem, string languageCode)
		{
			return GetTranslationsOf(contentItem, true).SingleOrDefault(ci => ci.Language == null || ci.Language == languageCode);
		}

		public Language GetLanguage(string languageCode)
		{
			return GetLanguageContainer(false).GetChild(languageCode) as Language;
		}

		/// <summary>Retrieves all translations of an item, optionally including the original language version.</summary>
		/// <param name="originalLanguageItem">The item whose translations to get.</param>
		/// <param name="includeOriginal">Specifies whether to include the original language version in the returned list.</param>
		/// <returns>A list of translations of the item.</returns>
		public IList<ContentItem> GetTranslationsOf(ContentItem originalLanguageItem, bool includeOriginal)
		{
			List<ContentItem> result = new List<ContentItem>();
			if (originalLanguageItem.ID == 0)
				return result;

			if (includeOriginal)
				result.Add(originalLanguageItem);

			result.AddRange(originalLanguageItem.Translations);
			return result;
		}

		/// <summary>
		/// Returns true if this content item can be translated. This method
		/// checks if the content type for this content item has been decorated
		/// with the [Translatable] attribute.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <returns></returns>
		public bool CanBeTranslated(ContentItem contentItem)
		{
			return _contentTypeManager.GetContentType(contentItem.GetType()).Translatable;
		}

		public bool TranslationExists(ContentItem contentItem, string languageCode)
		{
			return GetTranslationDirect(contentItem, languageCode) != null;
		}

		public bool Enabled
		{
			get { return (_globalizationConfig != null && _globalizationConfig.Enabled); }
		}

		public string GetDefaultLanguage()
		{
			return GetAvailableLanguages(true).First().Name;
		}

		#region Helper methods

		private LanguageContainer GetLanguageContainer(bool create)
		{
			ContentItem parent = _persister.Get(LanguageContainerParentID);
			LanguageContainer languageContainer = parent.GetChild("languages") as LanguageContainer;
			if (languageContainer == null && create)
				languageContainer = CreateLanguageContainer(parent);
			return languageContainer;
		}

		private LanguageContainer CreateLanguageContainer(ContentItem parent)
		{
			LanguageContainer languageContainer = Context.ContentTypes.CreateInstance<LanguageContainer>(parent);

			AddLanguage(languageContainer, "English", "en", "gb", true);

			_persister.Save(languageContainer);
			return languageContainer;
		}

		private void AddLanguage(LanguageContainer languageContainer, string languageTitle, string languageCode, string flagIconName, bool enabled)
		{
			Language language = Context.ContentTypes.CreateInstance<Language>(languageContainer);
			language.AddTo(languageContainer);

			language.Title = languageTitle;
			language.Name = languageCode;
			language.Enabled = enabled;

			string flagIconFileName = flagIconName + ".png";
			Image flagIcon = Image.FromStream(typeof(LanguageManager).Assembly.GetManifestResourceStream("Zeus.Web.Resources.Flags." + flagIconFileName),
																				flagIconFileName);
			language.FlagIcon = flagIcon;
		}

		#endregion
	}
}