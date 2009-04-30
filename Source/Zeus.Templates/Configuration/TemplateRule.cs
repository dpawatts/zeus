using System;
using System.Configuration;
using Zeus.ContentTypes;

namespace Zeus.Templates.Configuration
{
	public class TemplateRule : ConfigurationElement
	{
		public TemplateRuleAction Action { get; set; }

		[ConfigurationProperty("type", DefaultValue = "")]
		public string Type
		{
			get { return (string) base["type"]; }
			set { base["type"] = value; }
		}

		public bool AllSpecified
		{
			get { return Type == "*"; }
		}

		internal int IsContentTypeAllowed(ContentType contentType)
		{
			int num = (Action == TemplateRuleAction.Allow) ? 1 : -1;
			if (AllSpecified)
				return num;

			System.Type type = System.Type.GetType(Type);
			if (type == null)
				throw new ArgumentException("Could not load type: '" + Type + "'", "Type");
			if (type.IsAssignableFrom(contentType.ItemType))
				return num;

			return 0;
		}
	}
}