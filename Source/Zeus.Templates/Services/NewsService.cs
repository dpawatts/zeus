using Castle.Core;
using Zeus.ContentTypes;
using Zeus.Persistence;
using Zeus.Templates.ContentTypes.News;

namespace Zeus.Templates.Services
{
	public class NewsService : IStartable
	{
		#region Fields

		private readonly IPersister _persister;
		private readonly IContentTypeManager _contentTypeManager;

		#endregion

		#region Constructor

		public NewsService(IPersister persister, IContentTypeManager contentTypeManager)
		{
			_persister = persister;
			_contentTypeManager = contentTypeManager;
		}

		#endregion

		#region Methods

		public void Start()
		{
			_persister.ItemSaving += OnPersisterItemSaving;
		}

		private void OnPersisterItemSaving(object sender, CancelItemEventArgs e)
		{
			if (e.AffectedItem is NewsItem && e.AffectedItem.TranslationOf == null)
			{
				// Move news item to correct year / month, creating them if necessary.
				NewsItem newsItem = (NewsItem) e.AffectedItem;

				// Get or create year item.
				NewsYear year = (NewsYear) newsItem.CurrentNewsContainer.GetChild(newsItem.Date.ToString("yyyy"))
					?? CreateItem<NewsYear>(newsItem.CurrentNewsContainer, newsItem.Date.ToString("yyyy"), newsItem.Date.ToString("yyyy"));

				// Get or create month item.
				NewsMonth month = (NewsMonth) year.GetChild(newsItem.Date.ToString("MM"))
					?? CreateItem<NewsMonth>(year, newsItem.Date.ToString("MM"), newsItem.Date.ToString("MMMM"));

				// Add news item to month.
				newsItem.AddTo(month);
			}
		}

		private T CreateItem<T>(ContentItem parent, string name, string title)
			where T : ContentItem
		{
			T item = _contentTypeManager.CreateInstance<T>(parent);
			item.Name = name;
			item.Title = title;
			item.Visible = false;
			_persister.Save(item);
			return item;
		}


		public void Stop()
		{

		}

		#endregion
	}
}