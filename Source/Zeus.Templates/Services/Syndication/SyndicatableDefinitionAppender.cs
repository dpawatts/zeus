using Ninject;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Web.UI;

namespace Zeus.Templates.Services.Syndication
{
	/// <summary>
	/// Examines existing item definitions and add an editable checkbox detail 
	/// to the items implementing the <see cref="ISyndicatable" />
	/// interface.
	/// </summary>
	public class SyndicatableDefinitionAppender : IInitializable
	{
		private readonly IContentTypeManager _contentTypeManager;
		private string checkBoxText = "Make available for syndication.";
		private string containerName = "Syndication";
		private int sortOrder = 30;
		public static readonly string SyndicatableDetailName = "Syndicate";

		public SyndicatableDefinitionAppender(IContentTypeManager definitions)
		{
			_contentTypeManager = definitions;
		}

		public int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}

		public string ContainerName
		{
			get { return containerName; }
			set { containerName = value; }
		}

		public string CheckBoxText
		{
			get { return checkBoxText; }
			set { checkBoxText = value; }
		}

		public void Initialize()
		{
			foreach (ContentType contentType in _contentTypeManager.GetContentTypes())
			{
				if (typeof(ISyndicatable).IsAssignableFrom(contentType.ItemType))
				{
					TabPanelAttribute seoTab = new TabPanelAttribute("Syndication", "Syndication", 30);
					contentType.Add(seoTab);

					CheckBoxEditorAttribute ecb = new CheckBoxEditorAttribute(CheckBoxText, string.Empty, 10)
					{
						Name = SyndicatableDetailName,
						ContainerName = ContainerName,
						SortOrder = SortOrder,
						DefaultValue = true
					};

					contentType.Add(ecb);
				}
			}
		}
	}
}