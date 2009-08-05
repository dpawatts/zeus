using System.Web.SessionState;

namespace Isis.ExtensionMethods.Web.SessionState
{
	public static class HttpSessionStateExtensionMethods
	{
		public static T Item<T>(this HttpSessionState session, string name)
		{
			object value = session[name];
			if (value == null)
				return default(T);
			else
				return (T) value;
		}
	}
}