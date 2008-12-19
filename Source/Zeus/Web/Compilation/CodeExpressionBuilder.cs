using System;
using System.CodeDom;
using System.Web.UI;
using System.Web.Compilation;

namespace Zeus.Web.Compilation
{
	/// <summary>A quick way to set web control properties at runtime. The specified expression is evaluated at compile time.</summary>
	/// <example>
	/// &lt;asp:Label text="&lt;%$ Code: CurrentItem.Title %&gt;" runat="server" /&gt;
	/// </example>
	public class CodeExpressionBuilder : ExpressionBuilder
	{
		/// <summary>Creates code based on the given expression.</summary>
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return new CodeSnippetExpression(entry.Expression);
		}
	}
}
