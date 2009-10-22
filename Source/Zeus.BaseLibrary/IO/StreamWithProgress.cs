using System;
using System.IO;

namespace Zeus.BaseLibrary.IO
{
	public class StreamWithProgress : Stream
	{
		public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

		private Stream file;
		private long length;
		private long bytesRead;
		private string _fileName;

		public override bool CanRead
		{
			get { return true; }
		}

		public override bool CanSeek
		{
			get { return false; }
		}

		public override bool CanWrite
		{
			get { return false; }
		}

		public override long Length
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public override long Position
		{
			get { return bytesRead; }
			set { throw new Exception("The method or operation is not implemented."); }
		}

		public StreamWithProgress(Stream file, string fileName)
		{
			this.file = file;
			this.length = file.Length;
			this.bytesRead = 0;
			_fileName = fileName;
			if (ProgressChanged != null) ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length, _fileName));
		}

		public double GetProgress()
		{
			return ((double) bytesRead) / file.Length;
		}

		public override void Flush() { }

		public override int Read(byte[] buffer, int offset, int count)
		{
			int result = file.Read(buffer, offset, count);
			bytesRead += result;
			if (ProgressChanged != null)
				ProgressChanged(this, new ProgressChangedEventArgs(bytesRead, length, _fileName));
			return result;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void SetLength(long value)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}