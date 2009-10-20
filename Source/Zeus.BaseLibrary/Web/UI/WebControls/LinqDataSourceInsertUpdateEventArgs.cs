using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Isis.Web.UI.WebControls
{
	public class LinqDataSourceInsertUpdateEventArgs : System.ComponentModel.CancelEventArgs
	{
		private LinqDataSourceValidationException _exception;
		private bool _exceptionHandled;
		private object _newObject;

		public LinqDataSourceValidationException Exception
		{
			get { return _exception; }
		}

		public bool ExceptionHandled
		{
			get { return _exceptionHandled; }
			set { _exceptionHandled = value; }
		}

		public object NewObject
		{
			get { return _newObject; }
		}

		public LinqDataSourceInsertUpdateEventArgs(LinqDataSourceValidationException exception, bool exceptionHandled, object newObject)
		{
			_exception = exception;
			_exceptionHandled = exceptionHandled;
			_newObject = newObject;
		}
	}
}
