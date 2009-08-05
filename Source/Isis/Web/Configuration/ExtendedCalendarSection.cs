using System;
using System.Configuration;

namespace Isis.Web.Configuration
{
	/// <summary>
	/// &lt;isis.web&gt;
	///   &lt;extendedCalendar eventsXmlFile="~/App_Assets/Xml/ExtendedCalendar.xml" eventsXsltFile="~/App_Assets/Xml/ExtendedCalendar.xslt"/&gt;
	/// &lt;/isis.web&gt;
	/// </summary>
	public class ExtendedCalendarSection : System.Configuration.ConfigurationSection
	{
		private static ConfigurationPropertyCollection _properties;
		private static ConfigurationProperty _propEventsXmlFile;
		private static ConfigurationProperty _propEventsXsltFile;

		[ConfigurationProperty("eventsXmlFile", DefaultValue = "")]
		public string EventsXmlFile
		{
			get { return (string) base[_propEventsXmlFile]; }
			set { base[_propEventsXmlFile] = value; }
		}

		[ConfigurationProperty("eventsXsltFile", DefaultValue = "")]
		public string EventsXsltFile
		{
			get { return (string) base[_propEventsXsltFile]; }
			set { base[_propEventsXsltFile] = value; }
		}

		static ExtendedCalendarSection()
		{
			_propEventsXmlFile = new ConfigurationProperty("eventsXmlFile", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);
			_propEventsXsltFile = new ConfigurationProperty("eventsXsltFile", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

			_properties = new ConfigurationPropertyCollection();
			_properties.Add(_propEventsXmlFile);
			_properties.Add(_propEventsXsltFile);
		}
	}
}