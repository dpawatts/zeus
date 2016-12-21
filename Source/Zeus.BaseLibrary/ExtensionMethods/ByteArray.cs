using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeus.BaseLibrary.Mime;

namespace Zeus.BaseLibrary.ExtensionMethods
{
    public static class ByteArrayExtensions
    {
        public static FileType GetFileType(this byte[] bytes)
        {
            return MimeTypes.GetFileType(() => MimeTypes.ReadHeaderFromByteArray(bytes, MimeTypes.MaxHeaderSize), null, bytes);
        }

        public static string GetMimeType(this byte[] bytes)
        {
            FileType type = bytes.GetFileType();
            return type != null && !string.IsNullOrEmpty(type.Mime) ? type.Mime : "application/octet-stream";
        }
    }
}
