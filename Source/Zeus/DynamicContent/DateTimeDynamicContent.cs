using System;
using Zeus.Design.Editors;

namespace Zeus.DynamicContent
{
	public class DateTimeDynamicContent : IDynamicContent
	{
		#region Fields

		private int _contentID;
		private string _detailName;

		#endregion

		#region Properties

		[CheckBoxEditor("Include Time?", "", 10)]
		public bool IncludeTime { get; set; }

		string IDynamicContent.State
		{
			get { return IncludeTime.ToString(); }
			set { Convert.ToBoolean(value); }
		}

		#endregion

		#region Methods

		public string Render()
		{
			DateTime current = DateTime.Now;
			string result = current.ToShortDateString();
			if (IncludeTime)
				result += " " + current.ToShortTimeString();
			return result;
		}

		#endregion
	}
}