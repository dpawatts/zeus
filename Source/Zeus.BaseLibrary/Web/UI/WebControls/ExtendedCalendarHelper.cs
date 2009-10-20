using System;
using System.Configuration;
using System.Web;
using System.Xml;
using Isis.Web.Configuration;

namespace Isis.Web.UI.WebControls
{
	public static class ExtendedCalendarHelper
	{
		public static void AddEvent(DateTime startDate, DateTime endDate, string title, string text)
		{
			string eventsXmlFilename;
			XmlDocument eventsDoc = LoadEvents(out eventsXmlFilename);
			int nextID = int.MinValue;
			foreach (XmlNode eventNode in eventsDoc.SelectNodes("//Event"))
			{
				int id = Convert.ToInt32(eventNode.Attributes["ID"].Value);
				if (id > nextID)
					nextID = id;
			}
			nextID++;
			AddEvent(eventsDoc, nextID, startDate, endDate, title, text, true);
			eventsDoc.Save(eventsXmlFilename);
		}

		public static void AddEvent(XmlDocument eventsDoc, int id, DateTime startDate, DateTime endDate, string title, string text, bool visible)
		{
			// find Events element
			XmlNode eventsNode = eventsDoc.SelectSingleNode("//Events");

			// create a new Event element and add it to Events element
			XmlElement eventElement = eventsDoc.CreateElement("Event");
			eventsNode.AppendChild(eventElement);

			// create title and text elements
			XmlElement titleElement = eventsDoc.CreateElement("Title");
			titleElement.AppendChild(eventsDoc.CreateCDataSection(title));
			XmlElement textElement = eventsDoc.CreateElement("Text");
			textElement.AppendChild(eventsDoc.CreateCDataSection(text));

			// set properties of new Event element
			eventElement.SetAttribute("ID", id.ToString());
			eventElement.SetAttribute("StartDate", XmlConvert.ToString(startDate, XmlDateTimeSerializationMode.Unspecified));
			eventElement.SetAttribute("EndDate", XmlConvert.ToString(endDate, XmlDateTimeSerializationMode.Unspecified));
			eventElement.SetAttribute("Visible", visible.ToString().ToLower());
			eventElement.AppendChild(titleElement);
			eventElement.AppendChild(textElement);
		}

		public static void EditEvent(int id, DateTime startDate, DateTime endDate, string title, string text, bool visible)
		{
			string eventsXmlFilename;
			XmlDocument eventsDoc = LoadEvents(out eventsXmlFilename);

			// remove existing event
			XmlNode eventNode = eventsDoc.SelectSingleNode("//Event[@ID = '" + id + "']");
			eventNode.ParentNode.RemoveChild(eventNode);

			// add event using existing id
			AddEvent(eventsDoc, id, startDate, endDate, title, text, visible);
			eventsDoc.Save(eventsXmlFilename);
		}

		private static XmlDocument LoadEvents(out string eventsXmlFilename)
		{
			ExtendedCalendarSection configSection = (ExtendedCalendarSection) ConfigurationManager.GetSection("isis.web/extendedCalendar");

			eventsXmlFilename = HttpContext.Current.Server.MapPath(configSection.EventsXmlFile);
			XmlDocument eventsDoc = new XmlDocument();
			eventsDoc.Load(eventsXmlFilename);

			return eventsDoc;
		}

		public static void GetEvent(int id, out DateTime startDate, out DateTime endDate, out string title, out string text, out bool visible)
		{
			string eventsXmlFilename;
			XmlDocument eventsDoc = LoadEvents(out eventsXmlFilename);

			// find event
			XmlNode eventNode = eventsDoc.SelectSingleNode("//Event[@ID = '" + id + "']");
			
			// get details
			startDate = Convert.ToDateTime(eventNode.Attributes["StartDate"].Value);
			endDate = Convert.ToDateTime(eventNode.Attributes["EndDate"].Value);
			title = eventNode.SelectSingleNode("Title").InnerText;
			text = eventNode.SelectSingleNode("Text").InnerText;
			visible = Convert.ToBoolean(eventNode.Attributes["Visible"].Value);
		}
	}
}
