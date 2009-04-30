using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Windows.Threading;
using System.Windows.Browser;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace Zeus.SilverlightUpload.Classes
{
	public class UserFile
	{
		#region Fields

		private Stream _fileStream;
		private Constants.FileStates _state = Constants.FileStates.Pending;
		private double _bytesUploaded = 0;
		private double _fileSize = 0;
		private int _percentage = 0;
		private IFileUploader _fileUploader;

		#endregion

		#region Constructor

		public UserFile()
		{
			Identifier = Guid.NewGuid();
		}

		#endregion

		#region Events

		public event EventHandler<StateEventArgs> StateChanged;
		public event EventHandler<PercentageEventArgs> PercentageChanged;

		#endregion

		#region Properties

		public Guid Identifier { get; set; }

		public string UploadHandlerName { get; set; }

		[ScriptableMember]
		public string FileName { get; set; }

		public Constants.FileStates State
		{
			get { return _state; }
			set
			{
				_state = value;
				OnStateChanged(new StateEventArgs { State = value });
			}
		}

		[ScriptableMember]
		public string StateString
		{
			get { return _state.ToString(); }
		}

		public bool IsDeleted { get; set; }

		public Stream FileStream
		{
			get { return _fileStream; }
			set
			{
				_fileStream = value;

				if (_fileStream != null)
					_fileSize = _fileStream.Length;
			}
		}

		public double FileSize
		{
			get { return _fileSize; }
		}

		public double BytesUploaded
		{
			get { return _bytesUploaded; }
			internal set
			{
				_bytesUploaded = value;
				Percentage = (int) ((value * 100) / FileSize);
			}
		}

		[ScriptableMember]
		public int Percentage
		{
			get { return _percentage; }
			private set
			{
				_percentage = value;
				OnPercentageChanged(new PercentageEventArgs { Percentage = value });
			}
		}

		public string ErrorMessage { get; set; }

		#endregion

		#region Methods

		protected void OnPercentageChanged(PercentageEventArgs args)
		{
			if (PercentageChanged != null)
				PercentageChanged(this, args);
		}

		protected void OnStateChanged(StateEventArgs args)
		{
			if (StateChanged != null)
				StateChanged(this, args);
		}

		public void Upload(string initParams)
		{
			State = Constants.FileStates.Uploading;

			_fileUploader = new HttpFileUploader(this, UploadHandlerName);
			_fileUploader.UploadFinished += fileUploader_UploadFinished;
			_fileUploader.StartUpload(initParams);
		}

		public void CancelUpload()
		{
			if (_fileUploader != null && State == Constants.FileStates.Uploading)
			{
				_fileUploader.CancelUpload();
				//
				//_fileUploader = null;
			}

			IsDeleted = true;
		}

		private void fileUploader_UploadFinished(object sender, EventArgs e)
		{
			_fileUploader = null;
			State = Constants.FileStates.Finished;
		}

		#endregion
	}
}