using System;
using Zeus.ContentTypes;
using Zeus.Globalization;
using Zeus.Web;

namespace Zeus.Integrity
{
	public class IntegrityManager : IIntegrityManager
	{
		#region Fields

		private readonly IContentTypeManager _contentTypeManager;
		private readonly IUrlParser _urlParser;
		private readonly ILanguageManager _languageManager;

		#endregion

		#region Constructor

		public IntegrityManager(IContentTypeManager contentTypeManager, IUrlParser urlParser, ILanguageManager languageManager)
		{
			_contentTypeManager = contentTypeManager;
			_urlParser = urlParser;
			_languageManager = languageManager;
		}

		#endregion

		/// <summary>Check if an item can be deleted.</summary>
		/// <exception cref="ZeusException"></exception>
		public virtual ZeusException GetDeleteException(ContentItem item)
		{
			if (_urlParser.IsRootOrStartPage(item))
				return new CannotDeleteRootException();

			return null;
		}

		/// <summary>Check if an item can be copied.</summary>
		/// <exception cref="NameOccupiedException"></exception>
		/// <exception cref="ZeusException"></exception>
		public virtual ZeusException GetCopyException(ContentItem source, ContentItem destination)
		{
			if (IsNameOccupiedOnDestination(source, destination))
				return new NameOccupiedException(source, destination);

			if (!IsTypeAllowedBelowDestination(source, destination))
				return new NotAllowedParentException(_contentTypeManager.GetContentType(source.GetType()), destination.GetType());

			return null;
		}

		/// <summary>Checks if an item can be moved to a destination.</summary>
		/// <param name="source">The item that is to be moved.</param>
		/// <param name="destination">The destination onto which the item is to be moved.</param>
		/// <returns>Null if the item can be moved or an exception if the item can't be moved.</returns>
		public virtual ZeusException GetMoveException(ContentItem source, ContentItem destination)
		{
			if (IsDestinationBelowSource(source, destination))
				return new DestinationOnOrBelowItselfException(source, destination);

			if (IsNameOccupiedOnDestination(source, destination))
				return new NameOccupiedException(source, destination);

			if (!IsTypeAllowedBelowDestination(source, destination))
				return new NotAllowedParentException(_contentTypeManager.GetContentType(source.GetType()), destination.GetType());

			return null;
		}

		/// <summary>Check if an item can be saved.</summary>
		/// <exception cref="NameOccupiedException"></exception>
		/// <exception cref="ZeusException"></exception>
		public virtual ZeusException GetSaveException(ContentItem item)
		{
			if (!IsLocallyUnique(item.Name, item))
				return new NameOccupiedException(item, item.GetParent());

			if (!IsTypeAllowedBelowDestination(item, item.Parent))
				return new NotAllowedParentException(_contentTypeManager.GetContentType(item.GetType()), item.GetParent().GetType());

			return null;
		}

		/// <summary>Find out if an item name is occupied.</summary>
		/// <param name="name">The name to check.</param>
		/// <param name="item">The item whose siblings (other items with the same parent) might be have a clashing name.</param>
		/// <returns>True if the name is unique.</returns>
		public bool IsLocallyUnique(string name, ContentItem item)
		{
			ContentItem parentItem = item.GetParent();
			if (parentItem != null)
				foreach (ContentItem potentiallyClashingItem in parentItem.Children)
					if (!potentiallyClashingItem.Equals(item) && potentiallyClashingItem != item.TranslationOf)
						foreach (ContentItem translation in _languageManager.GetTranslationsOf(potentiallyClashingItem, true))
							if (!translation.Equals(item) && translation.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
								return false;
			return true;
		}

		/// <summary>Find out if an arbitrary property on an item is already used by an item in the same scope.</summary>
		/// <param name="propertyName">The property to check.</param>
		/// <param name="value">The property value to check.</param>
		/// <param name="item">The item whose siblings (other items with the same parent) might be have a clashing property value.</param>
		/// <returns>True if the property value is unique.</returns>
		public bool IsLocallyUnique(string propertyName, object value, ContentItem item)
		{
			ContentItem parentItem = item.Parent;
			if (parentItem != null)
				foreach (ContentItem potentiallyClashingItem in parentItem.Children)
					if (!potentiallyClashingItem.Equals(item))
					{
						if (potentiallyClashingItem[propertyName] is string)
						{
							string potentiallyClashingValue = (string) potentiallyClashingItem[propertyName];
							if (potentiallyClashingValue.Equals(value as string, StringComparison.InvariantCultureIgnoreCase))
								return false;
						}
						else if (potentiallyClashingItem[propertyName].Equals(value))
							return false;
					}
			return true;
		}

		/// <summary>Checks that destination have no child item with the same name.</summary>
		private static bool IsNameOccupiedOnDestination(ContentItem source, ContentItem destination)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (destination == null) throw new ArgumentNullException("destination");

			ContentItem existingItem = destination.GetChild(source.Name);
			return existingItem != null && existingItem != source;
		}

		/// <summary>Checks that the destination isn't below the source.</summary>
		private static bool IsDestinationBelowSource(ContentItem source, ContentItem destination)
		{
			for (ContentItem parent = destination; parent != null; parent = parent.Parent)
				if (parent == source)
					return true;
			return false;
		}

		/// <summary>Check that the source item type is allowed below the destination. Throws an exception if the item isn't allowed.</summary>
		/// <param name="source">The child item</param>
		/// <param name="destination">The parent item</param>
		private bool IsTypeAllowedBelowDestination(ContentItem source, ContentItem destination)
		{
			if (destination != null)
			{
				ContentType sourceDefinition = _contentTypeManager.GetContentType(source.GetType());
				ContentType destinationDefinition = _contentTypeManager.GetContentType(destination.GetType());

				return destinationDefinition.IsChildAllowed(sourceDefinition);
			}
			return true;
		}
	}
}
