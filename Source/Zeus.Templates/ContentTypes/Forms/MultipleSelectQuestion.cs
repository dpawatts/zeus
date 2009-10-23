using System.Linq;
using MvcContrib;
using MvcContrib.UI;
using MvcContrib.UI.Tags;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
{
	[ContentType("Multiple Select (checkboxes)")]
	public class MultipleSelectQuestion : OptionSelectQuestion
	{
		[ContentProperty("Display Vertically", 110)]
		public virtual bool Vertical
		{
			get { return GetDetail("Vertical", true); }
			set { SetDetail("Vertical", value); }
		}

		public override string ElementID
		{
			get { return "ms_" + ID; }
		}

		public override IElement CreateHtmlElement()
		{
			var list = new CheckBoxList
			{
				Name = ElementID
			};

			foreach (var field in base.Options)
			{
				var radioField = new CheckBoxField { Id = "ms_el_" + field.ID, Value = field.ID, Label = field.Title };

				list.Add(radioField);
			}

			return new Element("span", new Hash(@class => "alternatives")) { InnerText = list.ToString() };
		}

		public override string GetAnswerText(string value)
		{
			var values = (value ?? string.Empty).Split(',');

			var selectedOptions = Options.Where(opt => values.Contains(opt.ID.ToString())).Select(opt => opt.Title).ToArray();

			if (selectedOptions.Length == 0)
				return string.Empty;

			return string.Join(", ", selectedOptions);
		}
	}
}