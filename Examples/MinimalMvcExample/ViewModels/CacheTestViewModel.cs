using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
	public class CacheTestViewModel : ViewModel<CacheTest>
	{
		public CacheTestViewModel(CacheTest currentItem)
			: base(currentItem)
		{
			
		}

		public string GetTestQSNum {
			get { return System.Web.HttpContext.Current.Request.QueryString["test"]; }
		}
	}
}
