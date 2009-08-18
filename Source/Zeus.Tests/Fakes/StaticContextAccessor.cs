using System.Collections;
using Zeus.Web;

namespace Zeus.Tests.Fakes
{
	public class StaticContextAccessor : IRequestContextAccessor
	{
		Hashtable context = new Hashtable();

		public object Get(object key)
		{
			return context[key];
		}

		public void Set(object key, object instance)
		{
			context[key] = instance;
		}
	}
}