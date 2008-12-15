using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;

namespace Zeus.Web.Compilation
{
	/// <summary>Expression builder used for initializing web controls with the current page's details or properties with the expression syntax. This is a convenient way to bind content data to controls on a template. The expression is evaluated at compile time.</summary>
	/// <example>
	/// &lt;asp:Label text="&lt;%$ CurrentPage: Title %&gt;" runat="server" /&gt;
	/// </example>
	[ExpressionPrefix("CurrentPage")]
	public class CurrentPageExpressionBuilder : ZeusExpressionBuilder
	{
		public static object GetCurrentPageValue(string expression)
		{
			ContentItem item = Zeus.Context.CurrentPage;
			if (item != null)
				return item[expression];
			else
				return null;
		}

		protected override string ExpressionFormat
		{
			get { return @"Zeus.Web.Compilation.CurrentPageExpressionBuilder.GetCurrentPageValue(""{0}"")"; }
		}
	} 
}
