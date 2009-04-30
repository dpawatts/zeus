namespace Zeus.Web
{
	public interface IRequestContextAccessor
	{
		object Get(object key);
		void Set(object key, object instance);
	}
}