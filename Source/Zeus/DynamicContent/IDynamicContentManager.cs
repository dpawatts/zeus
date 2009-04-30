using System.Collections.Generic;
using Zeus.Configuration;

namespace Zeus.DynamicContent
{
	public interface IDynamicContentManager
	{
		string GetMarkup(IDynamicContent dynamicContent);
		string RenderDynamicContent(string value);
		IEnumerable<DynamicContentControl> GetAvailableControls();
		IDynamicContent InstantiateDynamicContent(string name);
	}
}