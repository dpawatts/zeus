using System.Collections.Generic;
using Zeus.Templates.Mvc.Design.Editors;

namespace Zeus.Templates.Mvc.ContentTypes.Forms
{
	public abstract class OptionSelectQuestion : Question
	{
		[QuestionOptionsEditor("Options", 100)]
		public virtual IList<Option> Options
		{
			get
			{
				List<Option> options = new List<Option>();
				foreach (Option o in GetChildren())
					options.Add(o);
				return options;
			}
		}
	}
}