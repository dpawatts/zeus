using System.Configuration;

namespace Isis.Web.Configuration
{
	public class IsisWebSectionGroup : ConfigurationSectionGroup
	{
		[ConfigurationProperty("extendedCalendar")]
		public ExtendedCalendarSection ExtendedCalendar
		{
			get { return (ExtendedCalendarSection) Sections["extendedCalendar"]; }
		}

		[ConfigurationProperty("secureWebPages")]
		public SecureWebPagesSection SecureWebPages
		{
			get { return (SecureWebPagesSection) Sections["secureWebPages"]; }
		}
	}
}