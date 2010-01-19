using System;

namespace Zeus.Web.Mvc
{
	public interface IControllerMapper
	{
		string GetControllerName(Type type);
		string GetAreaName(Type type);
	}
}