﻿using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using System;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		private ISessionProvider _sessionProvider;

		public ContentPersister(ISessionProvider sessionProvider)
		{
			_sessionProvider = sessionProvider;
		}

		public ContentItem Load(int id)
		{
			return _sessionProvider.OpenSession.Load<ContentItem>(id);
		}

		public void Save(ContentItem contentItem)
		{
			contentItem.Updated = DateTime.Now;
			_sessionProvider.OpenSession.SaveOrUpdate(contentItem);
		}
	}
}
