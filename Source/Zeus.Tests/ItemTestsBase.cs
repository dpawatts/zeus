using System;
using System.Collections.Generic;
using System.Security.Principal;
using Rhino.Mocks;
using Zeus.Tests.Fakes;
using Zeus.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Zeus.Tests
{
	[TestClass]
	public abstract class ItemTestsBase
	{
		protected MockRepository mocks;

		[TestInitialize]
		public virtual void SetUp()
		{
			RequestItem.Accessor = new StaticContextAccessor();
			mocks = new MockRepository();
		}

		[TestCleanup]
		public virtual void TearDown()
		{
			if (mocks != null)
			{
				mocks.ReplayAll();
				mocks.VerifyAll();
			}
		}

		protected virtual T CreateOneItem<T>(int id, string name, ContentItem parent) where T : ContentItem
		{
			T item = (T)Activator.CreateInstance(typeof(T), true);
			item.ID = id;
			item.Name = name;
			item.Title = name;
			item.AddTo(parent);
			return item;
		}

		protected IPrincipal CreatePrincipal(string name, params string[] roles)
		{
			return SecurityUtilities.CreatePrincipal(name, roles);
		}

		private Dictionary<string, object> requestItems;
		protected IWebContext CreateWebContext(bool replay)
		{
			requestItems = new Dictionary<string, object>();
			IWebContext context = mocks.StrictMock<IWebContext>();
			Expect.On(context).Call(context.RequestItems).Return(requestItems).Repeat.Any();

			if (replay)
				mocks.Replay(context);
			return context;
		}
	}
}