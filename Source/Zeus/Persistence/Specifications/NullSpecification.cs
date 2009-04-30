namespace Zeus.Persistence.Specifications
{
	public class NullSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public NullSpecification()
			: base(ci => true)
		{

		}
	}
}
