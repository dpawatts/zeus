using System.Collections.Generic;
using Zeus.Globalization.ContentTypes;

namespace Zeus.Globalization
{
	public interface ILanguageManager
	{
		IEnumerable<Language> GetAvailableLanguages();
		IEnumerable<Language> GetAvailableLanguages(bool create);

		/// <summary>Retrieves all translations of an item, optionally including the original language version.</summary>
		/// <param name="originalLanguageItem">The item whose translations to get.</param>
		/// <param name="includeOriginal">Specifies whether to include the original language version in the returned list.</param>
		/// <returns>A list of translations of the item.</returns>
		IList<ContentItem> GetTranslationsOf(ContentItem originalLanguageItem, bool includeOriginal);

		/// <summary>
		/// Returns true if this content item can be translated. This method
		/// checks if the content type for this content item has been decorated
		/// with the [Translatable] attribute.
		/// </summary>
		/// <param name="contentItem"></param>
		/// <returns></returns>
		bool CanBeTranslated(ContentItem contentItem);

		string GetDefaultLanguage();
		Language GetLanguage(string languageCode);
		ContentItem GetTranslationDirect(ContentItem contentItem, string languageCode);
		ContentItem GetTranslation(ContentItem contentItem, string languageCode);
		bool TranslationExists(ContentItem contentItem, string languageCode);
		bool Enabled { get; }
	}
}