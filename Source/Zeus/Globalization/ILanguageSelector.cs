namespace Zeus.Globalization
{
	public interface ILanguageSelector
	{
		// Methods
		void LoadLanguage(LanguageSelectorContext args);
		void SelectPageLanguage(LanguageSelectorContext args);
		void SetInitializedLanguageBranch(LanguageSelectorContext args);
	}
}