using System;

namespace Zeus.Globalization
{
	public class LanguageSetting : ICloneable
	{
		#region Constructors

		public LanguageSetting() {}

		/// <summary>Creates a new instance of the LanguageSetting class associating it with a content item and defining the language.</summary>
		/// <param name="item">The item this role is associated with.</param>
		/// <param name="language"></param>
		public LanguageSetting(ContentItem item, string language)
			: this(item, language, null)
		{
			
		}

		public LanguageSetting(ContentItem item, string language, string fallbackLanguage)
		{
			EnclosingItem = item;
			Language = language;
			FallbackLanguage = fallbackLanguage;
		}

		#endregion

		#region Public Properties

		/// <summary>Gets or sets the database identifier of this class.</summary>
		public virtual int ID { get; set; }

		/// <summary>Gets or sets the item this LanguageSetting referrs to.</summary>
		public virtual ContentItem EnclosingItem { get; set; }

		/// <summary>Gets the language code (i.e. en, en-GB, etc.) this class referrs to.</summary>
		public virtual string Language { get; set; }

		/// <summary>Gets the fallback language this class refers to.</summary>
		public virtual string FallbackLanguage { get; set; }

		#endregion

		#region ICloneable Members

		object ICloneable.Clone()
		{
			return Clone();
		}

		#endregion

		/// <summary>Copies this AuthorizedRole clearing id and enclosing item.</summary>
		/// <returns>A copy of this AuthorizedRole.</returns>
		public virtual LanguageSetting Clone()
		{
			LanguageSetting cloned = (LanguageSetting) MemberwiseClone();
			cloned.ID = 0;
			cloned.EnclosingItem = null;
			return cloned;
		}
	}
}