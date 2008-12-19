using System;
using System.Web.Compilation;
using System.Collections;

namespace Zeus.Web.Compilation
{
	/// <summary>
	/// Gives a true/false value indicating wether a certain collections have items present.
	/// Useful in situations where we need to hide a webcontrol 
	/// when there is no value to give it.
	/// </summary>
	/// <example>
	/// &lt;asp:Image ImageUrl="&lt;%$ CurrentItem: MainImageUrl %&gt;" Visible="&lt;%$ HasValue: MainImageUrl %&gt;" runat="server" /&gt;
	/// </example>
	[ExpressionPrefix("HasItems")]
	public class HasItemsExpressionBuilder : ZeusExpressionBuilder
	{
		/// <summary>Gets wether a certain exression has a value.</summary>
		/// <param name="expression">The expression to check.</param>
		/// <returns>True if the expression would result in a non null or non empty-string value.</returns>
		public static bool HasItems(string expression)
		{
			ContentItem item = Zeus.Context.CurrentPage;
			if (item != null)
				return HasItems(item, expression);
			else
				return HasItems(Context.CurrentPage, expression);
		}

		private static bool HasItems(ContentItem item, string propertyName)
		{
			return item[propertyName] != null && ((ICollection) item[propertyName]).Count > 0;
		}

		/// <summary>Gets the expression format for this expression.</summary>
		protected override string ExpressionFormat
		{
			get { return @"Zeus.Web.Compilation.HasItemsExpressionBuilder.HasItems(""{0}"")"; }
		}
	}
}
