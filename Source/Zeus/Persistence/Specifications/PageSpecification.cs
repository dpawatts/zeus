using Zeus.Web;
namespace Zeus.Persistence.Specifications
{
	public class PageSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public PageSpecification()
			: base(ci => ci.IsPage)
		{

		}
	}
}
