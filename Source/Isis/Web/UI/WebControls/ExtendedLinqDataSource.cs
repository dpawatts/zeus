using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Compilation;
using System.Data.Linq;
using System.ComponentModel;
using System.Collections;

namespace Isis.Web.UI.WebControls
{
	public class ExtendedLinqDataSource : System.Web.UI.WebControls.LinqDataSource
	{
		#region Events

		public event EventHandler<LinqDataSourceInsertUpdateEventArgs> InsertingUpdating;
		public event EventHandler<LinqDataSourceStatusEventArgs> InsertedUpdated;

		#endregion

		#region Properties

		[Category("Data"), DefaultValue("")]
		public string ConnectionStringName
		{
			get { return ViewState["ConnectionStringName"] as string; }
			set { ViewState["ConnectionStringName"] = value; }
		}

		#endregion

		#region Constructor

		public ExtendedLinqDataSource()
		{
			this.ContextCreating += new EventHandler<LinqDataSourceContextEventArgs>(ExtendedLinqDataSource_ContextCreating);
			this.Inserting += new EventHandler<LinqDataSourceInsertEventArgs>(ExtendedLinqDataSource_Inserting);
			this.Updating += new EventHandler<LinqDataSourceUpdateEventArgs>(ExtendedLinqDataSource_Updating);
			this.Inserted += new EventHandler<LinqDataSourceStatusEventArgs>(ExtendedLinqDataSource_Inserted);
			this.Updated += new EventHandler<LinqDataSourceStatusEventArgs>(ExtendedLinqDataSource_Updated);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Creates a DataContext using the constructor that takes in a single string
		/// parameter, which is the connection string
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExtendedLinqDataSource_ContextCreating(object sender, LinqDataSourceContextEventArgs e)
		{
			// create instance of type, using constructor which takes one string parameter
			Type type = BuildManager.GetType(this.ContextTypeName, true);
			string connectionString = ConfigurationManager.ConnectionStrings[this.ConnectionStringName].ConnectionString;
			object dataContext = Activator.CreateInstance(type, new object[] { connectionString });
			if (!this.EnableDelete && this.EnableInsert && !this.EnableUpdate)
			{
				DataContext context = dataContext as DataContext;
				if (context != null)
					context.ObjectTrackingEnabled = false;
			}
			e.ObjectInstance = dataContext;
		}

		private void ExtendedLinqDataSource_Inserting(object sender, LinqDataSourceInsertEventArgs e)
		{
			OnInsertingUpdating(new LinqDataSourceInsertUpdateEventArgs(e.Exception, e.ExceptionHandled, e.NewObject));
		}

		private void ExtendedLinqDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
		{
			OnInsertingUpdating(new LinqDataSourceInsertUpdateEventArgs(e.Exception, e.ExceptionHandled, e.NewObject));
		}

		private void ExtendedLinqDataSource_Inserted(object sender, LinqDataSourceStatusEventArgs e)
		{
			OnInsertedUpdated(e);
		}

		private void ExtendedLinqDataSource_Updated(object sender, LinqDataSourceStatusEventArgs e)
		{
			OnInsertedUpdated(e);
		}

		protected virtual void OnInsertingUpdating(LinqDataSourceInsertUpdateEventArgs e)
		{
			if (InsertingUpdating != null)
				InsertingUpdating(this, e);
		}

		protected virtual void OnInsertedUpdated(LinqDataSourceStatusEventArgs e)
		{
			if (InsertedUpdated != null)
				InsertedUpdated(this, e);
		}

		#endregion
	}
}
