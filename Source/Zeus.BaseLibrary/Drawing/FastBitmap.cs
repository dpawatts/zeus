using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Zeus.BaseLibrary.Drawing
{
	public class FastBitmap : IDisposable, ICloneable
	{
		#region Variables

		private Bitmap _bitmap;
		private BitmapData _bitmapData;
		private int _bitsPerPixel;
		private Stream _stream;

		#endregion

		#region Properties / Indexer

		public Bitmap InnerBitmap
		{
			get
			{
				return _bitmap;
			}
			set
			{
				// if bitmap is an indexed format, we need to convert it so that
				// we can work with it later
				if (value != null)
				{
					Bitmap convertedBitmap = null;
					bool conversionRequired = false;
					switch (value.PixelFormat)
					{
						case PixelFormat.Format8bppIndexed:
							convertedBitmap = ConvertBitmapFromIndexedToFormat24bppRgb(value);
							conversionRequired = true;
							break;
					}

					if (conversionRequired)
					{
						value.Dispose();
						value = convertedBitmap;
					}
				}

				_bitmap = value;

				if (_bitmap != null)
				{
					switch (_bitmap.PixelFormat)
					{
						case PixelFormat.Format48bppRgb:
							_bitsPerPixel = 6;
							break;
						case PixelFormat.Format32bppArgb:
							_bitsPerPixel = 4;
							break;
						case PixelFormat.Format24bppRgb:
							_bitsPerPixel = 3;
							break;
						default:
							throw new NotSupportedException("Unrecognised pixel format: " + _bitmap.PixelFormat);
					}
				}
				else
				{
					_bitsPerPixel = 0;
				}
			}
		}

		public int Width
		{
			get { return _bitmap.Width; }
		}

		public int Height
		{
			get { return _bitmap.Height; }
		}

		unsafe public Color this[int x, int y]
		{
			get
			{
				byte* b = (byte*) _bitmapData.Scan0 + (y * _bitmapData.Stride) + (x * _bitsPerPixel);

				switch (_bitsPerPixel)
				{
					case 6 :
						return Color.FromArgb(*(b + 4), *(b + 2), *b);
					case 4:
						return Color.FromArgb(*(b + 3), *(b + 2), *(b + 1), *b);
					case 3:
						return Color.FromArgb(*(b + 2), *(b + 1), *b);
					default :
						throw new NotSupportedException();
				}
			}

			set
			{
				if (x < 0 || y < 0 || x >= this.Width || y >= this.Height)
					throw new ArgumentOutOfRangeException();

				byte* b = (byte*) _bitmapData.Scan0 + (y * _bitmapData.Stride) + (x * _bitsPerPixel);

				switch (_bitsPerPixel)
				{
					case 6:
						*b = value.B;
						*(b + 2) = value.G;
						*(b + 4) = value.R;
						break;
					case 4:
						*(b + 3) = value.A;
						goto case 3;
					case 3:
						*b = value.B;
						*(b + 1) = value.G;
						*(b + 2) = value.R;
						break;
				}
			}
		}

		#endregion

		#region Constructors / Destructor

		public FastBitmap(int width, int height, PixelFormat format)
		{
			this.InnerBitmap = new Bitmap(width, height, format);
		}

		public FastBitmap(string filename)
		{
			this.InnerBitmap = new Bitmap(filename);
		}

		public FastBitmap(byte[] bytes)
		{
			_stream = new MemoryStream(bytes);
			this.InnerBitmap = new Bitmap(_stream);
		}

		public FastBitmap(Bitmap bitmap)
		{
			this.InnerBitmap = bitmap;
		}

		public FastBitmap(Image image)
		{
			this.InnerBitmap = new Bitmap(image);
		}

		private FastBitmap() {}

		~FastBitmap()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			if (_stream != null)
				_stream.Close();
			GC.SuppressFinalize(this);
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing)
		{
			Unlock();
			if (disposing)
				_bitmap.Dispose();
		}

		#endregion

		#region Methods

		public object Clone()
		{
			FastBitmap clone = new FastBitmap();
			clone.InnerBitmap = (Bitmap) this.InnerBitmap.Clone();
			return clone;
		}

		public void Lock()
		{
			_bitmapData = _bitmap.LockBits(
				new Rectangle(0, 0, _bitmap.Width, _bitmap.Height),
				ImageLockMode.ReadWrite,
				this.InnerBitmap.PixelFormat);
		}

		public void Unlock()
		{
			if (_bitmapData != null) 
			{
				try
				{
					_bitmap.UnlockBits(_bitmapData);
				}
				catch (ArgumentException)
				{

				}
				_bitmapData = null;
			}
		}

		public void Save(string filename, ImageFormat format)
		{
			_bitmap.Save(filename, format);
		}

		public void Save(string filename)
		{
			_bitmap.Save(filename);
		}

		private unsafe Bitmap ConvertBitmapFromIndexedToFormat24bppRgb(Bitmap value)
		{
			BitmapData bitmapData = value.LockBits(
				new Rectangle(0, 0, value.Width, value.Height),
				ImageLockMode.ReadOnly,
				value.PixelFormat);

			int bytesPerPixel = 1;
			FastBitmap destination = new FastBitmap(value.Width, value.Height, PixelFormat.Format24bppRgb);
			destination.Lock();
			for (int y = 0; y < value.Height; y++)
				for (int x = 0; x < value.Width; x++)
				{
					byte* b = (byte*) bitmapData.Scan0 + (y * bitmapData.Stride) + (x * bytesPerPixel);
					destination[x, y] = value.Palette.Entries[*b];
				}
			destination.Unlock();

			value.UnlockBits(bitmapData);

			return destination.InnerBitmap;
		}

		#endregion
	}
}