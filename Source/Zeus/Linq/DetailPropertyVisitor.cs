using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq.Visitors;
using NHibernate.Linq.Expressions;
using System.Linq.Expressions;
using NHibernate.Metadata;
using NHibernate.Type;

namespace Zeus.Linq
{
	/// <summary>
	/// Preprocesses an expression tree replacing MemberAccessExpressions and ParameterExpressions with
	/// NHibernate-specific PropertyAccessExpressions and EntityExpressions respectively.
	/// </summary>
	public class DetailPropertyVisitor : ExpressionVisitor
	{
		private EntityExpression GetParentExpression(MemberExpression expr, out string memberName, out IType nhibernateType)
		{
			memberName = null;
			nhibernateType = null;

			PropertyAccessExpression propExpr = expr.Expression as PropertyAccessExpression;
			if (propExpr != null)
			{
				memberName = propExpr.Name + "." + expr.Member.Name;
				nhibernateType = propExpr.Expression.MetaData.GetPropertyType(memberName);
				return propExpr.Expression;
			}

			return null;
		}

		protected override Expression VisitMemberAccess(MemberExpression expr)
		{
			expr = (MemberExpression) base.VisitMemberAccess(expr);

			string memberName;
			IType nhibernateType;
			EntityExpression parentExpression = GetParentExpression(expr, out memberName, out nhibernateType);

			if (parentExpression != null)
				return new PropertyAccessExpression(memberName, expr.Type, nhibernateType, parentExpression);

			return expr;
		}
	}
}
