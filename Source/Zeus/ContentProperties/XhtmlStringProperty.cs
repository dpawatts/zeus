using System;
using Zeus.DynamicContent;
using Zeus.Web;

namespace Zeus.ContentProperties
{
	public class XhtmlStringProperty : StringProperty
	{
		#region Constuctors

		public XhtmlStringProperty()
		{
		}

		public XhtmlStringProperty(ContentItem containerItem, string name, string value)
			: base(containerItem, name, value)
		{
		}

		#endregion

		#region Methods

		public override string GetXhtmlValue()
		{
			// TODO: Make this pluggable, so we don't explicitly call DynamicContent, and allow other things to hook in here.
			string result = StringValue;

			// Squirt DynamicContent in here.
			IDynamicContentManager dynamicContentManager = Context.Current.Resolve<IDynamicContentManager>();
			result = dynamicContentManager.RenderDynamicContent(result);

			// Resolve permanent links.
			IPermanentLinkManager permanentLinkManager = Context.Current.Resolve<IPermanentLinkManager>();
			result = permanentLinkManager.ResolvePermanentLinks(result);

			return result;
		}

		#endregion
	}
}