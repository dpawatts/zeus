using System;

namespace Zeus.BaseLibrary.Mime
{
    /// <summary>
	/// Little data structure to hold information about file types.
	/// Holds information about binary header at the start of the file
	/// these are mostly static they can be structs
	/// </summary>
	public struct FileType
    {
        public byte?[] Header { get; }
        public ushort HeaderOffset { get; }
        public string Extension { get; }
        public string Mime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileType"/> struct.
        /// Takes the details of offset for the header
        /// </summary>
        /// <param name="header">Byte array with header.</param>
        /// <param name="offset">The header offset - how far into the file we need to read the header</param>
        /// <param name="extension">String with extension.</param>
        /// <param name="mime">The description of MIME.</param>
        public FileType(byte?[] header, string extension, string mime, ushort offset = 0)
        {
            //header cannot be null, file type normal operation requires the data
            if (header == null)
                throw new ArgumentNullException(nameof(header), "cannot be null file type needs file header data");

            Header = header;
            HeaderOffset = offset;
            Extension = extension;
            Mime = mime;
        }

        public static bool operator ==(FileType a, FileType b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(FileType a, FileType b)
        {
            return !(a.Equals(b));
        }

        public override bool Equals(object other)
        {
            if (!(other is FileType))
                return false;

            FileType otherType = (FileType)other;

            if (this.Extension == otherType.Extension && this.Mime == otherType.Mime)
                return true;

            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Extension;
        }
    }
}
