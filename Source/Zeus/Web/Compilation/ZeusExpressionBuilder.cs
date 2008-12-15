using System;
using System.CodeDom;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Web.Compilation;

namespace Zeus.Web.Compilation
{
	/// <summary>The base class for Zeus expression builders. Defines methods to compile time create code expressions.</summary>
	public abstract class ZeusExpressionBuilder : ExpressionBuilder
	{
		/// <summary>The expression format base classes can override.</summary>
		protected abstract string ExpressionFormat
		{
			get;
		}

		/// <summary>Gets the code expresion evaluated at page compile time.</summary>
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return new CodeSnippetExpression(string.Format(ExpressionFormat, entry.Expression, entry.DeclaringType));
		}
	}
}