using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;

namespace Zeus.BaseLibrary.ExtensionMethods.Data.Common
{
	public static class DbCommandExtensionMethods
	{
		/// <summary>
		/// Returns a string representation of the <see cref="DbCommand.CommandText"/> along with <see cref="DbCommand.Parameters"/> if present.
		/// </summary>
		/// <param name="cmd">The <see cref="DbCommand"/> to analyze.</param>
		/// <returns>Returns a string representation of the <see cref="DbCommand.CommandText"/> along with <see cref="DbCommand.Parameters"/> if present.</returns>
		/// <remarks>This method is useful for debugging purposes or when used in other utilities such as LINQPad.</remarks>
		public static string PreviewCommandText(this DbCommand cmd)
		{
			var output = new StringBuilder();
			output.AppendLine(cmd.CommandText);
			foreach (DbParameter parameter in cmd.Parameters)
			{
				int num = 0;
				int num2 = 0;
				PropertyInfo property = parameter.GetType().GetProperty("Precision");
				if (property != null)
				{
					num = (int) Convert.ChangeType(property.GetValue(parameter, null), typeof(int), CultureInfo.InvariantCulture);
				}
				PropertyInfo info2 = parameter.GetType().GetProperty("Scale");
				if (info2 != null)
				{
					num2 = (int) Convert.ChangeType(info2.GetValue(parameter, null), typeof(int), CultureInfo.InvariantCulture);
				}
				SqlParameter parameter2 = parameter as SqlParameter;
				output.AppendFormat("-- {0}: {1} {2} (Size = {3}; Prec = {4}; Scale = {5}) [{6}]\r\n", new object[] { parameter.ParameterName, parameter.Direction, (parameter2 == null) ? parameter.DbType.ToString() : parameter2.SqlDbType.ToString(), parameter.Size.ToString(CultureInfo.CurrentCulture), num, num2, (parameter2 == null) ? parameter.Value : parameter2.SqlValue });
			}

			return output.ToString();
		}
	}
}