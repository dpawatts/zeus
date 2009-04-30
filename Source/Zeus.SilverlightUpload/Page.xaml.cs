using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Browser;
using System.Windows.Controls;
using Zeus.SilverlightUpload.Classes;

namespace Zeus.SilverlightUpload
{
	[ScriptableType]
	public partial class Page : UserControl
	{
		#region Fields

		private int _maxFileSize = int.MaxValue;

		private string _customParams;
		private string _fileFilter;
		private string _uploadHandlerName;

		#endregion

		#region Constructor

		public Page(IDictionary<string, string> initParams)
		{
			InitializeComponent();
			LoadConfiguration(initParams);
			HtmlPage.RegisterScriptableObject("Control", this);
		}

		#endregion

		#region Scriptable events

		[ScriptableMember]
		public event EventHandler MaximumFileSizeReached;

		[ScriptableMember]
		public event EventHandler<UploadEventArgs> UploadStarted;

		[ScriptableMember]
		public event EventHandler<UploadEventArgs> UploadFinished;

		[ScriptableMember]
		public event EventHandler<PercentageEventArgs> PercentageChanged;

		#endregion

		#region Scriptable methods

		[ScriptableMember]
		public void SelectFileAndUpload()
		{
			UserFile userFile = SelectFile();
			if (userFile != null)
				UploadFile(userFile);
		}

		#endregion

		private UserFile SelectFile()
		{
			OpenFileDialog ofd = new OpenFileDialog { Multiselect = false };

			try
			{
				// Check the file filter (filter is used to filter file extensions to select, for example only .jpg files).
				if (!string.IsNullOrEmpty(_fileFilter))
					ofd.Filter = _fileFilter;
			}
			catch (ArgumentException ex)
			{
				// User supplied a wrong configuration file.
				throw new Exception("Wrong file filter configuration.", ex);
			}

			if (ofd.ShowDialog() == true)
			{
				FileInfo file = ofd.File;

				//Create a new UserFile object
				UserFile userFile = new UserFile
      	{
      		FileName = file.Name,
      		FileStream = file.OpenRead(),
      		UploadHandlerName = _uploadHandlerName
      	};

				//Check for the file size limit (configurable)
				if (userFile.FileStream.Length <= _maxFileSize)
					return userFile;
				if (MaximumFileSizeReached != null)
					MaximumFileSizeReached(this, null);
			}

			return null;
		}

		/// <summary>
		/// Start uploading files
		/// </summary>
		private void UploadFile(UserFile file)
		{
			file.PercentageChanged += file_PercentageChanged;
			file.StateChanged += file_StateChanged;
			file.Upload(_customParams);
		}

		protected void file_PercentageChanged(object sender, PercentageEventArgs e)
		{
			Dispatcher.BeginInvoke(() => { if (PercentageChanged != null) PercentageChanged(this, e); });
		}

		protected void file_StateChanged(object sender, StateEventArgs e)
		{
			UserFile file = (UserFile) sender;
			UploadEventArgs eventArgs = new UploadEventArgs { FileName = file.FileName, Identifier = file.Identifier.ToString() };
			switch (e.State)
			{
				case Constants.FileStates.Uploading :
					Dispatcher.BeginInvoke(() => { if (UploadStarted != null) UploadStarted(this, eventArgs); });
					break;
				case Constants.FileStates.Finished:
					Dispatcher.BeginInvoke(() => { if (UploadFinished != null) UploadFinished(this, eventArgs); });
					break;
			}
		}

		/// <summary>
		/// Load configuration first from initParams, then from .Config file
		/// </summary>
		/// <param name="initParams"></param>
		private void LoadConfiguration(IDictionary<string, string> initParams)
		{
			//Load Custom Config String
			if (initParams.ContainsKey("CustomParam") && !string.IsNullOrEmpty(initParams["CustomParam"]))
				_customParams = initParams["CustomParam"];

			if (initParams.ContainsKey("MaxFileSizeKB") && !string.IsNullOrEmpty(initParams["MaxFileSizeKB"]))
			{
				if (int.TryParse(initParams["MaxFileSizeKB"], out _maxFileSize))
					_maxFileSize = _maxFileSize * 1024;
			}

			if (initParams.ContainsKey("FileFilter") && !string.IsNullOrEmpty(initParams["FileFilter"]))
				_fileFilter = initParams["FileFilter"];

			if (initParams.ContainsKey("UploadHandlerName") && !string.IsNullOrEmpty(initParams["UploadHandlerName"]))
				_uploadHandlerName = initParams["UploadHandlerName"];
		}
	}
}