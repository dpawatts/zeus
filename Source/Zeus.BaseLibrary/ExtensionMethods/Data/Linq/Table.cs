using System;
using System.Data.Common;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Ninject.Activation;
using Zeus.BaseLibrary.ExtensionMethods.Data.Common;
using Zeus.BaseLibrary.Linq;

namespace Zeus.BaseLibrary.ExtensionMethods.Data.Linq
{
	public static class TableExtensionMethods
	{
		/// <summary>
		/// Immediately deletes all entities from the collection with a single delete command.
		/// </summary>
		/// <typeparam name="TEntity">Represents the object type for rows contained in <paramref name="table"/>.</typeparam>
		/// <param name="table">Represents a table for a particular type in the underlying database containing rows are to be deleted.</param>
		/// <param name="entities">Represents the collection of items which are to be removed from <paramref name="table"/>.</param>
		/// <returns>The number of rows deleted from the database.</returns>
		/// <remarks>
		/// <para>Similiar to stored procedures, and opposite from DeleteAllOnSubmit, rows provided in <paramref name="entities"/> will be deleted immediately with no need to call <see cref="DataContext.SubmitChanges()"/>.</para>
		/// <para>Additionally, to improve performance, instead of creating a delete command for each item in <paramref name="entities"/>, a single delete command is created.</para>
		/// </remarks>
		public static int DeleteBatch<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities) where TEntity : class
		{
			DbCommand delete = table.GetDeleteBatchCommand<TEntity>(entities);

			var parameters = from p in delete.Parameters.Cast<DbParameter>()
			                 select p.Value;

			return table.Context.ExecuteCommand(delete.CommandText, parameters.ToArray());
		}

		/// <summary>
		/// Returns a string representation the LINQ <see cref="IProvider"/> command text and parameters used that would be issued to delete all entities from the collection with a single delete command.
		/// </summary>
		/// <typeparam name="TEntity">Represents the object type for rows contained in <paramref name="table"/>.</typeparam>
		/// <param name="table">Represents a table for a particular type in the underlying database containing rows are to be deleted.</param>
		/// <param name="entities">Represents the collection of items which are to be removed from <paramref name="table"/>.</param>
		/// <returns>Returns a string representation the LINQ <see cref="IProvider"/> command text and parameters used that would be issued to delete all entities from the collection with a single delete command.</returns>
		/// <remarks>This method is useful for debugging purposes or when used in other utilities such as LINQPad.</remarks>
		public static string DeleteBatchPreview<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities) where TEntity : class
		{
			DbCommand delete = table.GetDeleteBatchCommand<TEntity>(entities);
			return delete.PreviewCommandText() + table.Context.GetLog();
		}

		/// <summary>
		/// Returns a string representation the LINQ <see cref="IProvider"/> command text and parameters used that would be issued to update all entities from the collection with a single update command.
		/// </summary>
		/// <typeparam name="TEntity">Represents the object type for rows contained in <paramref name="table"/>.</typeparam>
		/// <param name="table">Represents a table for a particular type in the underlying database containing rows are to be updated.</param>
		/// <param name="entities">Represents the collection of items which are to be updated in <paramref name="table"/>.</param>
		/// <param name="evaluator">A Lambda expression returning a <typeparamref name="TEntity"/> that defines the update assignments to be performed on each item in <paramref name="entities"/>.</param>
		/// <returns>Returns a string representation the LINQ <see cref="IProvider"/> command text and parameters used that would be issued to update all entities from the collection with a single update command.</returns>
		/// <remarks>This method is useful for debugging purposes or when used in other utilities such as LINQPad.</remarks>
		public static string UpdateBatchPreview<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
		{
			DbCommand update = table.GetUpdateBatchCommand<TEntity>(entities, evaluator);
			return update.PreviewCommandText() + table.Context.GetLog();
		}

		/// <summary>
		/// Immediately updates all entities in the collection with a single update command based on a <typeparamref name="TEntity"/> created from a Lambda expression.
		/// </summary>
		/// <typeparam name="TEntity">Represents the object type for rows contained in <paramref name="table"/>.</typeparam>
		/// <param name="table">Represents a table for a particular type in the underlying database containing rows are to be updated.</param>
		/// <param name="entities">Represents the collection of items which are to be updated in <paramref name="table"/>.</param>
		/// <param name="evaluator">A Lambda expression returning a <typeparamref name="TEntity"/> that defines the update assignments to be performed on each item in <paramref name="entities"/>.</param>
		/// <returns>The number of rows updated in the database.</returns>
		/// <remarks>
		/// <para>Similiar to stored procedures, and opposite from similiar InsertAllOnSubmit, rows provided in <paramref name="entities"/> will be updated immediately with no need to call <see cref="DataContext.SubmitChanges()"/>.</para>
		/// <para>Additionally, to improve performance, instead of creating an update command for each item in <paramref name="entities"/>, a single update command is created.</para>
		/// </remarks>
		public static int UpdateBatch<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
		{
			DbCommand update = table.GetUpdateBatchCommand<TEntity>(entities, evaluator);

			var parameters = from p in update.Parameters.Cast<DbParameter>()
			                 select p.Value;
			return table.Context.ExecuteCommand(update.CommandText, parameters.ToArray());
		}

		private static DbCommand GetDeleteBatchCommand<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities) where TEntity : class
		{
			var deleteCommand = table.Context.GetCommand(entities);
			deleteCommand.CommandText = string.Format("DELETE {0}\r\n", table.GetDbName()) + GetBatchJoinQuery<TEntity>(table, entities);
			return deleteCommand;
		}

		private static DbCommand GetUpdateBatchCommand<TEntity>(this Table<TEntity> table, IQueryable<TEntity> entities, Expression<Func<TEntity, TEntity>> evaluator) where TEntity : class
		{
			var updateCommand = table.Context.GetCommand(entities);

			var setSB = new StringBuilder();
			int memberInitCount = 1;

			// Process the MemberInitExpression (there should only be one in the evaluator Lambda) to convert the expression tree
			// into a valid DbCommand.  The Visit<> method will only process expressions of type MemberInitExpression and requires
			// that a MemberInitExpression be returned - in our case we'll return the same one we are passed since we are building
			// a DbCommand and not 'really using' the evaluator Lambda.
			evaluator.Visit<MemberInitExpression>(delegate(MemberInitExpression expression)
			{
				if (memberInitCount > 1)
				{
					throw new NotImplementedException("Currently only one MemberInitExpression is allowed for the evaluator parameter.");
				}
				memberInitCount++;

				setSB.Append(GetDbSetStatement<TEntity>(expression, table, updateCommand));

				return expression; // just return passed in expression to keep 'visitor' happy.
			});

			// Complete the command text by concatenating bits together.
			updateCommand.CommandText = string.Format("UPDATE {0}\r\n{1}\r\n\r\n{2}",
				table.GetDbName(),									// Database table name
				setSB.ToString(),									// SET fld = {}, fld2 = {}, ...
				GetBatchJoinQuery<TEntity>(table, entities));	// Subquery join created from entities command text
			return updateCommand;
		}

		private static string GetDbSetStatement<TEntity>(MemberInitExpression memberInitExpression, Table<TEntity> table, DbCommand updateCommand) where TEntity : class
		{
			var entityType = typeof(TEntity);

			if (memberInitExpression.Type != entityType)
			{
				throw new NotImplementedException(string.Format("The MemberInitExpression is intializing a class of the incorrect type '{0}' and it should be '{1}'.", memberInitExpression.Type, entityType));
			}

			var setSB = new StringBuilder();

			var tableName = table.GetDbName();
			var metaTable = table.Context.Mapping.GetTable(entityType);
			var dbCols = from mdm in metaTable.RowType.DataMembers			// Used to look up actual field names when MemberAssignment is a constant
			             select new { mdm.MappedName };

			// Walk all the expression bindings and generate SQL 'commands' from them.  Each binding represents a property assignment
			// on the TEntity object initializer Lambda expression.
			foreach (var binding in memberInitExpression.Bindings)
			{
				var assignment = binding as MemberAssignment;

				if (binding == null)
				{
					throw new NotImplementedException("All bindings inside the MemberInitExpression are expected to be of type MemberAssignment.");
				}

				// TODO (Document): What is this doing?  I know it's grabbing existing parameter to pass into Expression.Call() but explain 'why'
				//					I assume it has something to do with fact we can't just access the parameters of assignment.Expression?
				//					Also, any concerns of whether or not if there are two params of type entity type?
				ParameterExpression entityParam = null;
				assignment.Expression.Visit<ParameterExpression>(delegate(ParameterExpression p) { if (p.Type == entityType) entityParam = p; return p; });

				// Get the real database field name.
				string name = binding.Member.Name;
				var dbCol = (from c in dbCols
				             where c.MappedName == name
				             select c).FirstOrDefault();

				if (dbCol == null)
				{
					throw new ArgumentOutOfRangeException(name, string.Format("The corresponding field on the {0} table could not be found.", tableName));
				}

				// If entityParam is NULL, then no references to other columns on the TEntity row and need to eval 'constant' value...
				if (entityParam == null)
				{
					// Compile and invoke the assignment expression to obtain the contant value to add as a parameter.
					var constant = Expression.Lambda(assignment.Expression, null).Compile().DynamicInvoke();

					if (constant == null)
					{
						setSB.AppendFormat("[{0}] = null, ", dbCol.MappedName);
					}
					else
					{
						// Add new parameter with massaged name to avoid clashes.
						setSB.AppendFormat("[{0}] = @p{1}, ", dbCol.MappedName, updateCommand.Parameters.Count);
						updateCommand.Parameters.Add(new SqlParameter(string.Format("@p{0}", updateCommand.Parameters.Count), constant));
					}
				}
				else
				{
					// TODO (Documentation): Explain what we are doing here again, I remember you telling me why we have to call but I can't remember now.
					// Wny are we calling Expression.Call and what are we passing it?  Below comments are just 'made up' and probably wrong.

					// Create a MethodCallExpression which represents a 'simple' select of *only* the assignment part (right hand operator) of
					// of the MemberInitExpression.MemberAssignment so that we can let the Linq Provider do all the 'sql syntax' generation for
					// us. 
					//
					// For Example: TEntity.Property1 = TEntity.Property1 + " Hello"
					// This selectExpression will be only dealing with TEntity.Property1 + " Hello"
					var selectExpression = Expression.Call(
						typeof(Queryable),
						"Select",
						new Type[] { entityType, assignment.Expression.Type },

						// TODO (Documentation): How do we know there are only 'two' parameters?  And what is Expression.Lambda
						//						 doing?  I assume it's returning a type of assignment.Expression.Type to match above?

						Expression.Constant(table),
						Expression.Lambda(assignment.Expression, entityParam));

					setSB.AppendFormat("[{0}] = {1}, ",
						dbCol.MappedName,
						GetDbSetAssignment(table, selectExpression, updateCommand, name));
				}
			}

			var setStatements = setSB.ToString();
			return "SET " + setStatements.Substring(0, setStatements.Length - 2); // remove ', '
		}

		/// <summary>
		/// Some LINQ Query syntax is invalid because SQL (or whomever the provider is) can not translate it to its native language.  
		/// DataContext.GetCommand() does not detect this, only IProvider.Execute or IProvider.Compile call the necessary code to 
		/// check this.  This function invokes the IProvider.Compile to make sure the provider can translate the expression.
		/// </summary>
		/// <remarks>
		/// An example of a LINQ query that previously 'worked' in the *Batch methods but needs to throw an exception is something
		/// like the following:
		/// 
		/// var pay = 
		///		from h in HistoryData
		///		where h.his.Groups.gName == "Ochsner" && h.hisType == "pay"
		///		select h;
		///		
		/// HistoryData.UpdateBatchPreview( pay, h => new HistoryData { hisIndex = ( int.Parse( h.hisIndex ) - 1 ).ToString() } ).Dump();
		/// 
		/// The int.Parse is not valid and needs to throw an exception like: 
		/// 
		///		Could not translate expression '(Parse(p.hisIndex) - 1).ToString()' into SQL and could not treat it as a local expression.
		///		
		///	Unfortunately, the IProvider.Compile is internal and I need to use Reflection to call it (ugh).  I've several e-mails sent into
		///	MS LINQ team members and am waiting for a response and will correct/improve code as soon as possible.
		/// </remarks>
		private static void ValidateExpression(ITable table, Expression expression)
		{
			var context = table.Context;
			PropertyInfo providerProperty = context.GetType().GetProperty("Provider", BindingFlags.Instance | BindingFlags.NonPublic);
			var provider = providerProperty.GetValue(context, null);
			var compileMI = provider.GetType().GetMethod("System.Data.Linq.Provider.IProvider.Compile", BindingFlags.Instance | BindingFlags.NonPublic);

			// Simply compile the expression to see if it will work.
			compileMI.Invoke(provider, new object[] { expression });
		}

		private static string GetDbSetAssignment(ITable table, MethodCallExpression selectExpression, DbCommand updateCommand, string bindingName)
		{
			ValidateExpression(table, selectExpression);

			// Convert the selectExpression into an IQueryable query so that I can get the CommandText
			var selectQuery = (table as IQueryable).Provider.CreateQuery(selectExpression);

			// Get the DbCommand so I can grab relavent parts of CommandText to construct a field 
			// assignment and based on the 'current TEntity row'.  Additionally need to massage parameter 
			// names from temporary command when adding to the final update command.
			var selectCmd = table.Context.GetCommand(selectQuery);
			var selectStmt = selectCmd.CommandText;
			selectStmt = selectStmt.Substring(7,									// Remove 'SELECT ' from front ( 7 )
				selectStmt.IndexOf("\r\nFROM ") - 7)		// Return only the selection field expression
				.Replace("[t0].", "")							// Remove table alias from the select
				.Replace(" AS [value]", "")					// If the select is not a direct field (constant or expression), remove the field alias
				.Replace("@p", "@p" + bindingName);			// Replace parameter name so doesn't conflict with existing ones.

			foreach (var selectParam in selectCmd.Parameters.Cast<DbParameter>())
			{
				var paramName = string.Format("@p{0}", updateCommand.Parameters.Count);

				// DataContext.ExecuteCommand ultimately just takes a object array of parameters and names them p0-N.  
				// So I need to now do replaces on the massaged value to get it in proper format.
				selectStmt = selectStmt.Replace(
					selectParam.ParameterName.Replace("@p", "@p" + bindingName),
					paramName);

				updateCommand.Parameters.Add(new SqlParameter(paramName, selectParam.Value));
			}

			return selectStmt;
		}

		private static string GetBatchJoinQuery<TEntity>(Table<TEntity> table, IQueryable<TEntity> entities) where TEntity : class
		{
			var metaTable = table.Context.Mapping.GetTable(typeof(TEntity));

			var keys = from mdm in metaTable.RowType.DataMembers
			           where mdm.IsPrimaryKey
			           select new { mdm.MappedName };

			var joinSB = new StringBuilder();
			var subSelectSB = new StringBuilder();

			foreach (var key in keys)
			{
				joinSB.AppendFormat("j0.[{0}] = j1.[{0}] AND ", key.MappedName);
				// For now, always assuming table is aliased as t0.  Should probably improve at some point.
				// Just writing a smaller sub-select so it doesn't get all the columns of data, but instead
				// only the primary key fields used for joining.
				subSelectSB.AppendFormat("[t0].[{0}], ", key.MappedName);
			}

			var selectCommand = table.Context.GetCommand(entities);
			var select = selectCommand.CommandText;

			var join = joinSB.ToString(); join = join.Substring(0, join.Length - 5);					// Remove last ' AND '
			var selectClause = select.Substring(0, select.IndexOf("[t0]"));							// Get 'SELECT ' and any TOP clause if present
			var needsTopClause = selectClause.IndexOf(" TOP ") < 0 && select.IndexOf("\r\nORDER BY ") > 0;
			var subSelect = selectClause
			                + (needsTopClause ? "TOP 100 PERCENT " : "")							// If order by in original select without TOP clause, need TOP
			                + subSelectSB.ToString();												// Apped just the primary keys.
			subSelect = subSelect.Substring(0, subSelect.Length - 2);									// Remove last ', '

			subSelect += select.Substring(select.IndexOf("\r\nFROM ")); // Create a sub SELECT that *only* includes the primary key fields

			var batchJoin = String.Format("FROM {0} AS j0 INNER JOIN (\r\n\r\n{1}\r\n\r\n) AS j1 ON ({2})\r\n", table.GetDbName(), subSelect, join);
			return batchJoin;
		}

		private static string GetDbName<TEntity>(this Table<TEntity> table) where TEntity : class
		{
			var entityType = typeof(TEntity);
			var metaTable = table.Context.Mapping.GetTable(entityType);
			var tableName = metaTable.TableName;

			if (!tableName.StartsWith("["))
			{
				string[] parts = tableName.Split('.');
				tableName = string.Format("[{0}]", string.Join("].[", parts));
			}

			return tableName;
		}

		private static string GetLog(this DataContext context)
		{
			PropertyInfo providerProperty = context.GetType().GetProperty("Provider", BindingFlags.Instance | BindingFlags.NonPublic);
			var provider = providerProperty.GetValue(context, null);
			Type providerType = provider.GetType();

			PropertyInfo modeProperty = providerType.GetProperty("Mode", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo servicesField = providerType.GetField("services", BindingFlags.Instance | BindingFlags.NonPublic);
			object services = servicesField != null ? servicesField.GetValue(provider) : null;
			PropertyInfo modelProperty = services != null ? services.GetType().GetProperty("Model", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetProperty) : null;

			return string.Format("-- Context: {0}({1}) Model: {2} Build: {3}\r\n",
				providerType.Name,
				modeProperty != null ? modeProperty.GetValue(provider, null) : "unknown",
				modelProperty != null ? modelProperty.GetValue(services, null).GetType().Name : "unknown",
				"3.5.21022.8");
		}
	}
}