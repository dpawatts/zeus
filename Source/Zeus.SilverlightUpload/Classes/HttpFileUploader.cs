using System;
using System.Net;
using System.IO;

/*
 * Copyright Michiel Post
 * http://www.michielpost.nl
 * contact@michielpost.nl
 * */

namespace Zeus.SilverlightUpload.Classes
{
	public class HttpFileUploader : IFileUploader
	{
		private readonly UserFile _file;
		private readonly long _dataLength;
		private long _dataSent;
		private string _initParams;

		private const long ChunkSize = 4194304;
		private readonly string UploadUrl;

		#region Constructor

		public HttpFileUploader(UserFile file, string httpHandlerName)
		{
			_file = file;

			_dataLength = _file.FileStream.Length;
			_dataSent = 0;

			if(string.IsNullOrEmpty(httpHandlerName))
				httpHandlerName = "/FileUpload.axd";

			UploadUrl = new CustomUri(httpHandlerName).ToString();
		}

		#endregion

		#region IFileUploader Members

		/// <summary>
		/// Start the file upload
		/// </summary>
		/// <param name="initParams"></param>
		public void StartUpload(string initParams)
		{
			_initParams = initParams;
			StartUpload();
		}

		/// <summary>
		/// Cancel the file upload
		/// </summary>
		public void CancelUpload()
		{
			//Not implemented yet...
		}

		public event EventHandler UploadFinished;

		#endregion

		private void StartUpload()
		{
			long dataToSend = _dataLength - _dataSent;
			bool isLastChunk = dataToSend <= ChunkSize;
			bool isFirstChunk = _dataSent == 0;

			UriBuilder httpHandlerUrlBuilder = new UriBuilder(UploadUrl);
			httpHandlerUrlBuilder.Query = string.Format("{5}file={0}&offset={1}&last={2}&first={3}&identifier={4}", _file.FileName, _dataSent, isLastChunk, isFirstChunk, _file.Identifier, string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&");

			HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(httpHandlerUrlBuilder.Uri);
			webRequest.Method = "POST";
			webRequest.BeginGetRequestStream(WriteToStreamCallback, webRequest);
		}

		private void WriteToStreamCallback(IAsyncResult asynchronousResult)
		{
			HttpWebRequest webRequest = (HttpWebRequest) asynchronousResult.AsyncState;
			Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult);

			byte[] buffer = new byte[4096];
			int bytesRead = 0;
			int tempTotal = 0;

			//Set the start position
			_file.FileStream.Position = _dataSent;

			//Read the next chunk
			while ((bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize && !_file.IsDeleted && _file.State != Constants.FileStates.Error)
			{
				requestStream.Write(buffer, 0, bytesRead);
				requestStream.Flush();

				_dataSent += bytesRead;
				tempTotal += bytesRead;

				//Notify progress change
				OnProgressChanged();
			}

			//Leave the fileStream OPEN
			//fileStream.Close();

			requestStream.Close();

			//Get the response from the HttpHandler
			webRequest.BeginGetResponse(ReadHttpResponseCallback, webRequest);
		}

		private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
		{
			bool error = false;

			//Check if the response is OK
			try
			{
				HttpWebRequest webRequest = (HttpWebRequest) asynchronousResult.AsyncState;
				HttpWebResponse webResponse = (HttpWebResponse) webRequest.EndGetResponse(asynchronousResult);
				StreamReader reader = new StreamReader(webResponse.GetResponseStream());

				reader.ReadToEnd();
				reader.Close();
			}
			catch
			{
				error = true;
				_file.State = Constants.FileStates.Error;
			}

			if (_dataSent < _dataLength)
			{
				//Not finished yet, continue uploading
				if (_file.State != Constants.FileStates.Error && !error)
					StartUpload();
			}
			else
			{
				_file.FileStream.Close();
				_file.FileStream.Dispose();

				//Finished event
				if (UploadFinished != null)
					UploadFinished(this, null);
			}
		}

		private void OnProgressChanged()
		{
			_file.BytesUploaded = _dataSent;
		}
	}
}