using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using Isis.Web.Configuration;

namespace Isis.Web.UI.WebControls
{
	public class CalendarMasterView : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private ExtendedCalendarSection _configSection;
		private bool _initialised = false;
		private XPathDocument _eventsXml;
		private System.Web.UI.WebControls.Xml _eventsXmlControl;
		private Calendar _calendar;

		public CalendarMasterView()
		{
			_configSection = (ExtendedCalendarSection) ConfigurationManager.GetSection("isis.web/extendedCalendar");
		}

		protected override void CreateChildControls()
		{
			if (!_initialised)
				Initialise();

			Table table = new Table();
			table.Width = this.Width;
			this.Controls.Add(table);

			TableRow row = new TableRow();
			table.Rows.Add(row);

			// add events section in left cell

			TableCell eventsCell = new TableCell();
			eventsCell.VerticalAlign = VerticalAlign.Top;
			eventsCell.Width = Unit.Pixel(400);
			row.Cells.Add(eventsCell);

			_eventsXmlControl = new System.Web.UI.WebControls.Xml();
			_eventsXmlControl.XPathNavigator = _eventsXml.CreateNavigator();
			_eventsXmlControl.TransformSource = _configSection.EventsXsltFile;
			_eventsXmlControl.TransformArgumentList = new System.Xml.Xsl.XsltArgumentList();
			_eventsXmlControl.TransformArgumentList.AddParam("Today", string.Empty, DateTime.Today.Date);
			eventsCell.Controls.Add(_eventsXmlControl);

			// add calendar in right cell

			TableCell calendarCell = new TableCell();
			calendarCell.VerticalAlign = VerticalAlign.Top;
			calendarCell.Width = Unit.Pixel(200);
			row.Cells.Add(calendarCell);

			_calendar = new ExtendedCalendar();
			_calendar.ID = "calendar";
			calendarCell.Controls.Add(_calendar);
		}

		private void Initialise()
		{
			_eventsXml = new XPathDocument(Page.Server.MapPath(_configSection.EventsXmlFile));
			_initialised = true;
		}

		protected override void OnPreRender(EventArgs e)
		{
			_eventsXmlControl.TransformArgumentList.AddParam("SelectedDate", string.Empty, _calendar.SelectedDate);
			base.OnPreRender(e);
		}
	}
}
