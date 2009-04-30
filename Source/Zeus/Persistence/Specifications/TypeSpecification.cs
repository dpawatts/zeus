namespace Zeus.Persistence.Specifications
{
	public class TypeSpecification<T> : Specification<T>
		where T : ContentItem
	{
		public TypeSpecification()
			: base(ci => ci is T)
		{

		}
	}
}
