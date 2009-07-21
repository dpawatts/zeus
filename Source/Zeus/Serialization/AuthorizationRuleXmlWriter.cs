using System.Xml;
using Zeus.Security;

namespace Zeus.Serialization
{
	public class AuthorizationRuleXmlWriter : IXmlWriter
	{
		public virtual void Write(ContentItem item, XmlTextWriter writer)
		{
			using (new ElementWriter("authorizationRules", writer))
			{
				if (item.AuthorizationRules != null)
					foreach (AuthorizationRule ar in item.AuthorizationRules)
						WriteRule(writer, ar);
			}
		}

		protected virtual void WriteRule(XmlTextWriter writer, AuthorizationRule ar)
		{
			using (ElementWriter role = new ElementWriter("rule", writer))
			{
				role.WriteAttribute("operation", ar.Operation);
				role.WriteAttribute("role", ar.Role);
				role.WriteAttribute("user", ar.User);
				role.WriteAttribute("allowed", ar.Allowed);
			}
		}
	}
}