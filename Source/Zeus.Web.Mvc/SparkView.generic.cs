using System.Web.Mvc;

namespace Zeus.Web.Mvc
{
	public abstract class SparkView<TModel> : Spark.Web.Mvc.SparkView<TModel>
		where TModel : class
	{
		private HtmlHelper<TModel> _htmlHelper;

		public new HtmlHelper<TModel> Html
		{
			get
			{
				if (_htmlHelper == null)
					_htmlHelper = new HtmlHelper<TModel>(ViewContext, this);
				return _htmlHelper;
			}
		}
	}
}