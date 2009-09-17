using Zeus.AddIns.Blogs.ContentTypes;

namespace Zeus.AddIns.Blogs.Services
{
	public interface ICategoryService
	{
		Category EnsureCategory(Blog blog, string categoryName);
	}
}