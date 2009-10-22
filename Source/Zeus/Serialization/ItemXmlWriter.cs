using System.Collections.Generic;
using System.Xml;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.ContentTypes;
using Zeus.Web;

namespace Zeus.Serialization
{
	/// <summary>
	/// A content item xml serializer.
	/// </summary>
	public class ItemXmlWriter
	{
		private readonly IContentTypeManager definitions;
		private readonly IUrlParser parser;

		public ItemXmlWriter(IContentTypeManager definitions, IUrlParser parser)
		{
			this.definitions = definitions;
			this.parser = parser;
		}

		public virtual void Write(ContentItem item, ExportOptions options, XmlTextWriter writer)
		{
			WriteSingleItem(item, options, writer);

			foreach (ContentItem child in item.Children)
				Write(child, options, writer);
		}

		public virtual void WriteSingleItem(ContentItem item, ExportOptions options, XmlTextWriter writer)
		{
			using (ElementWriter itemElement = new ElementWriter("item", writer))
			{
				WriteDefaultAttributes(itemElement, item);

				foreach (IXmlWriter xmlWriter in GetWriters(options))
					xmlWriter.Write(item, writer);
			}
		}

		private IEnumerable<IXmlWriter> GetWriters(ExportOptions options)
		{
			if ((options & ExportOptions.OnlyDefinedProperties) == ExportOptions.OnlyDefinedProperties)
				yield return new DefinedPropertyXmlWriter(definitions);
			else
				yield return new PropertyXmlWriter();
			yield return new PropertyCollectionXmlWriter();
			yield return new ChildXmlWriter();
			yield return new AuthorizationRuleXmlWriter();
			yield return new LanguageSettingXmlWriter();
		}

		protected virtual void WriteDefaultAttributes(ElementWriter itemElement, ContentItem item)
		{
			itemElement.WriteAttribute("id", item.ID);
			itemElement.WriteAttribute("name", item.Name);
			itemElement.WriteAttribute("parent", item.Parent != null ? item.Parent.ID.ToString() : string.Empty);
			itemElement.WriteAttribute("title", item.Title);
			itemElement.WriteAttribute("zoneName", item.ZoneName);
			itemElement.WriteAttribute("created", item.Created);
			itemElement.WriteAttribute("updated", item.Updated);
			itemElement.WriteAttribute("published", item.Published);
			itemElement.WriteAttribute("expires", item.Expires);
			itemElement.WriteAttribute("sortOrder", item.SortOrder);
			//itemElement.WriteAttribute("url", parser.BuildUrl(item));
			itemElement.WriteAttribute("visible", item.Visible);
			itemElement.WriteAttribute("savedBy", item.SavedBy);
			itemElement.WriteAttribute("language", item.Language);
			itemElement.WriteAttribute("translationOf", (item.TranslationOf != null) ? item.TranslationOf.ID.ToString() : string.Empty);
			itemElement.WriteAttribute("typeName", item.GetType().GetTypeAndAssemblyName());
			itemElement.WriteAttribute("discriminator", definitions.GetContentType(item.GetType()).Discriminator);
		}
	}
}