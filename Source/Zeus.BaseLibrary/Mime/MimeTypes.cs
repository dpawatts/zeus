using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Zeus.BaseLibrary.Mime
{
    /// <summary>
	/// Helper class to identify file type by the file header, not file extension.
	/// file headers are taken from here:
	/// http://www.garykessler.net/library/file_sigs.html
	/// mime types are taken from here:
	/// http://www.webmaster-toolkit.com/mime-types.shtml
	/// </summary>
	public static class MimeTypes
    {
        // all the file types to be put into one list
        public static readonly FileType[] Types;

        public static readonly FileType[] XmlTypes;

        static MimeTypes()
        {
            Types = new FileType[] { PDF, WORD, EXCEL, JPEG, ZIP, RAR, RTF, PNG, PPT, GIF, DLL_EXE, MSDOC,
                BMP, DLL_EXE, ZIP_7z, ZIP_7z_2, GZ_TGZ, TAR_ZH, TAR_ZV, OGG, ICO, XML, DWG, LIB_COFF, PST, PSD,
                AES, SKR, SKR_2, PKR, EML_FROM, ELF, TXT_UTF8, TXT_UTF16_BE, TXT_UTF16_LE, TXT_UTF32_BE, TXT_UTF32_LE,
                Mp3, Wav, Flac, MIDI,
                Tiff, TiffLittleEndian, TiffBigEndian, TiffBig,
                Mp4ISOv1, MovQuickTime, MP4VideoFiles, Mp4QuickTime, Mp4VideoFile, ThridGPP2File, Mp4A, FLV };

            XmlTypes = new FileType[] { WORDX, EXCELX, PPTX, ODS, ODT };
        }

        #region Constants

        #region office, excel, ppt and documents, xml, pdf, rtf, msdoc

        // office and documents
        public readonly static FileType WORD = new FileType(new byte?[] { 0xEC, 0xA5, 0xC1, 0x00 }, "doc", "application/msword", 512);

        public readonly static FileType EXCEL = new FileType(new byte?[] { 0x09, 0x08, 0x10, 0x00, 0x00, 0x06, 0x05, 0x00 }, "xls", "application/excel", 512);

        //see source control for old version, def maybe wrong period
        public readonly static FileType PPT = new FileType(new byte?[] { 0xA0, 0x46, 0x1D, 0xF0 }, "ppt", "application/mspowerpoint", 512);

        //ms office and openoffice docs (they're zip files: rename and enjoy!)
        //don't add them to the list, as they will be 'subtypes' of the ZIP type
        public readonly static FileType WORDX = new FileType(new byte?[0], "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", 512);

        public readonly static FileType PPTX = new FileType(new byte?[0], "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation", 512);
        public readonly static FileType EXCELX = new FileType(new byte?[0], "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 512);
        public readonly static FileType ODT = new FileType(new byte?[0], "odt", "application/vnd.oasis.opendocument.text", 512);
        public readonly static FileType ODS = new FileType(new byte?[0], "ods", "application/vnd.oasis.opendocument.spreadsheet", 512);

        // common documents
        public readonly static FileType RTF = new FileType(new byte?[] { 0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31 }, "rtf", "application/rtf");

        public readonly static FileType PDF = new FileType(new byte?[] { 0x25, 0x50, 0x44, 0x46 }, "pdf", "application/pdf");
        public readonly static FileType MSDOC = new FileType(new byte?[] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 }, "", "application/octet-stream");

        //application/xml text/xml
        public readonly static FileType XML = new FileType(new byte?[] { 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x3D, 0x22, 0x31, 0x2E, 0x30, 0x22, 0x3F, 0x3E },
                                                            "xml,xul", "text/xml");

        //text files
        public readonly static FileType TXT = new FileType(new byte?[0], "txt", "text/plain");

        public readonly static FileType TXT_UTF8 = new FileType(new byte?[] { 0xEF, 0xBB, 0xBF }, "txt", "text/plain");
        public readonly static FileType TXT_UTF16_BE = new FileType(new byte?[] { 0xFE, 0xFF }, "txt", "text/plain");
        public readonly static FileType TXT_UTF16_LE = new FileType(new byte?[] { 0xFF, 0xFE }, "txt", "text/plain");
        public readonly static FileType TXT_UTF32_BE = new FileType(new byte?[] { 0x00, 0x00, 0xFE, 0xFF }, "txt", "text/plain");
        public readonly static FileType TXT_UTF32_LE = new FileType(new byte?[] { 0xFF, 0xFE, 0x00, 0x00 }, "txt", "text/plain");

        #endregion office, excel, ppt and documents, xml, pdf, rtf, msdoc

        // graphics

        #region Graphics jpeg, png, gif, bmp, ico, tiff

        public readonly static FileType JPEG = new FileType(new byte?[] { 0xFF, 0xD8, 0xFF }, "jpg", "image/jpeg");
        public readonly static FileType PNG = new FileType(new byte?[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, "png", "image/png");
        public readonly static FileType GIF = new FileType(new byte?[] { 0x47, 0x49, 0x46, 0x38, null, 0x61 }, "gif", "image/gif");
        public readonly static FileType BMP = new FileType(new byte?[] { 0x42, 0x4D }, "bmp", "image/bmp"); // or image/x-windows-bmp
        public readonly static FileType ICO = new FileType(new byte?[] { 0, 0, 1, 0 }, "ico", "image/x-icon");

        //tiff
        //todo review support for tiffs, values for files need verified
        public readonly static FileType Tiff = new FileType(new byte?[] { 0x49, 0x20, 0x49 }, "tiff", "image/tiff");

        public readonly static FileType TiffLittleEndian = new FileType(new byte?[] { 0x49, 0x49, 0x2A, 0 }, "tiff", "image/tiff");
        public readonly static FileType TiffBigEndian = new FileType(new byte?[] { 0x4D, 0x4D, 0, 0x2A }, "tiff", "image/tiff");
        public readonly static FileType TiffBig = new FileType(new byte?[] { 0x4D, 0x4D, 0, 0x2B }, "tiff", "image/tiff");

        #endregion Graphics jpeg, png, gif, bmp, ico, tiff

        #region Video

        //todo review these
        //mp4 iso base file format, value: ....ftypisom
        public readonly static FileType Mp4ISOv1 = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }, "mp4", "video/mp4", 4);

        public readonly static FileType Mp4QuickTime = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x6D, 0x70, 0x34, 0x32 }, "m4v", "video/x-m4v", 4);

        public readonly static FileType MovQuickTime = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x71, 0x74, 0x20, 0x20 }, "mov", "video/quicktime", 4);

        public readonly static FileType MP4VideoFiles = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70, 0x35 }, "mp4", "video/mp4", 4);

        public readonly static FileType Mp4VideoFile = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x53, 0x4E, 0x56 }, "mp4", "video/mp4", 4);

        public readonly static FileType Mp4A = new FileType(new byte?[] { 0x66, 0x74, 0x79, 0x70, 0x4D, 0x34, 0x41, 0x20 }, "mp4a", "audio/mp4", 4);

        //FLV	 	Flash video file
        public readonly static FileType FLV = new FileType(new byte?[] { 0x46, 0x4C, 0x56, 0x01 }, "flv", "application/unknown");

        public readonly static FileType ThridGPP2File = new FileType(new byte?[] { 0, 0, 0, 0x20, 0x66, 0x74, 0x79, 0x70, 0x33, 0x67, 0x70 }, "3gp", "video/3gg");

        #endregion Video

        #region Audio

        public readonly static FileType Mp3 = new FileType(new byte?[] { 0x49, 0x44, 0x33 }, "mp3", "audio/mpeg");

        //WAV	 	Resource Interchange File Format -- Audio for Windows file, where xx xx xx xx is the file size (little endian), audio/wav audio/x-wav

        public readonly static FileType Wav = new FileType(new byte?[] { 0x52, 0x49, 0x46, 0x46, null, null, null, null,
            0x57, 0x41, 0x56, 0x45, 0x66, 0x6D, 0x74, 0x20 }, "wav", "audio/wav");

        //MID, MIDI	 	Musical Instrument Digital Interface (MIDI) sound file
        public readonly static FileType MIDI = new FileType(new byte?[] { 0x4D, 0x54, 0x68, 0x64 }, "midi,mid", "audio/midi");

        public readonly static FileType Flac = new FileType(new byte?[] { 0x66, 0x4C, 0x61, 0x43, 0, 0, 0, 0x22 }, "flac", "audio/x-flac");

        #endregion Audio

        #region Zip, 7zip, rar, dll_exe, tar, bz2, gz_tgz

        public readonly static FileType GZ_TGZ = new FileType(new byte?[] { 0x1F, 0x8B, 0x08 }, "gz, tgz", "application/x-gz");

        public readonly static FileType ZIP_7z = new FileType(new byte?[] { 66, 77 }, "7z", "application/x-compressed");
        public readonly static FileType ZIP_7z_2 = new FileType(new byte?[] { 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C }, "7z", "application/x-compressed");

        public readonly static FileType ZIP = new FileType(new byte?[] { 0x50, 0x4B, 0x03, 0x04 }, "zip", "application/x-compressed");
        public readonly static FileType RAR = new FileType(new byte?[] { 0x52, 0x61, 0x72, 0x21 }, "rar", "application/x-compressed");
        public readonly static FileType DLL_EXE = new FileType(new byte?[] { 0x4D, 0x5A }, "dll, exe", "application/octet-stream");

        //Compressed tape archive file using standard (Lempel-Ziv-Welch) compression
        public readonly static FileType TAR_ZV = new FileType(new byte?[] { 0x1F, 0x9D }, "tar.z", "application/x-tar");

        //Compressed tape archive file using LZH (Lempel-Ziv-Huffman) compression
        public readonly static FileType TAR_ZH = new FileType(new byte?[] { 0x1F, 0xA0 }, "tar.z", "application/x-tar");

        //bzip2 compressed archive
        public readonly static FileType BZ2 = new FileType(new byte?[] { 0x42, 0x5A, 0x68 }, "bz2,tar,bz2,tbz2,tb2", "application/x-bzip2");

        #endregion Zip, 7zip, rar, dll_exe, tar, bz2, gz_tgz

        #region Media ogg, dwg, pst, psd

        // media
        public readonly static FileType OGG = new FileType(new byte?[] { 103, 103, 83, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0 }, "oga,ogg,ogv,ogx", "application/ogg");

        public readonly static FileType PST = new FileType(new byte?[] { 0x21, 0x42, 0x44, 0x4E }, "pst", "application/octet-stream");

        //eneric AutoCAD drawing image/vnd.dwg  image/x-dwg application/acad
        public readonly static FileType DWG = new FileType(new byte?[] { 0x41, 0x43, 0x31, 0x30 }, "dwg", "application/acad");

        //Photoshop image file
        public readonly static FileType PSD = new FileType(new byte?[] { 0x38, 0x42, 0x50, 0x53 }, "psd", "application/octet-stream");

        #endregion Media ogg, dwg, pst, psd

        public readonly static FileType LIB_COFF = new FileType(new byte?[] { 0x21, 0x3C, 0x61, 0x72, 0x63, 0x68, 0x3E, 0x0A }, "lib", "application/octet-stream");

        #region Crypto aes, skr, skr_2, pkr

        //AES Crypt file format. (The fourth byte is the version number.)
        public readonly static FileType AES = new FileType(new byte?[] { 0x41, 0x45, 0x53 }, "aes", "application/octet-stream");

        //SKR	 	PGP secret keyring file
        public readonly static FileType SKR = new FileType(new byte?[] { 0x95, 0x00 }, "skr", "application/octet-stream");

        //SKR	 	PGP secret keyring file
        public readonly static FileType SKR_2 = new FileType(new byte?[] { 0x95, 0x01 }, "skr", "application/octet-stream");

        //PKR	 	PGP public keyring file
        public readonly static FileType PKR = new FileType(new byte?[] { 0x99, 0x01 }, "pkr", "application/octet-stream");

        #endregion Crypto aes, skr, skr_2, pkr

        /*
		 * 46 72 6F 6D 20 20 20 or	 	From
		46 72 6F 6D 20 3F 3F 3F or	 	From ???
		46 72 6F 6D 3A 20	 	From:
		EML	 	A commmon file extension for e-mail files. Signatures shown here
		are for Netscape, Eudora, and a generic signature, respectively.
		EML is also used by Outlook Express and QuickMail.
		 */
        public readonly static FileType EML_FROM = new FileType(new byte?[] { 0x46, 0x72, 0x6F, 0x6D }, "eml", "message/rfc822");

        //EVTX	 	Windows Vista event log file
        public readonly static FileType ELF = new FileType(new byte?[] { 0x45, 0x6C, 0x66, 0x46, 0x69, 0x6C, 0x65, 0x00 }, "elf", "text/plain");

        // number of bytes we read from a file
        public const ushort MaxHeaderSize = 560;  // some file formats have headers offset to 512 bytes

        #endregion Constants

        #region Main Methods

        /// <summary>
        /// Read header of a file and depending on the information in the header
        /// return object FileType.
        /// Return null in case when the file type is not identified.
        /// Throws Application exception if the file can not be read or does not exist
        /// </summary>
        /// <param name="fileHeaderReadFunc">A function which returns the bytes found</param>
        /// <param name="fileFullName">If given and file typ is a zip file, a check for docx and xlsx is done</param>
        /// <returns>FileType or null not identified</returns>
        public static FileType GetFileType(Func<IReadOnlyList<byte>> fileHeaderReadFunc, Stream stream = null, byte[] data = null)
        {
            return getFileType(fileHeaderReadFunc(), stream, data);
        }

        private static FileType getFileType(IReadOnlyList<byte> fileHeader, Stream stream = null, byte[] data = null)
        {
            if (stream == null && data == null)
                throw new ArgumentNullException($"{nameof(stream)} : {nameof(data)}", "both file data arguments are null");

            // checking if it's binary (not really exact, but should do the job)
            // shouldn't work with UTF-16 OR UTF-32 files
            if (!fileHeader.Any(b => b == 0))
                return TXT;

            // compare the file header to the stored file headers
            foreach (FileType type in Types)
            {
                int matchingCount = GetFileMatchingCount(fileHeader, type);

                if (type.Header.Length == matchingCount)
                {
                    // check for docx and xlsx only if a file name is given
                    // there may be situations where the file name is not given
                    if (type.Equals(ZIP))
                    {
                        using (Stream fileData = stream != null ? stream : new MemoryStream(data))
                        {
                            if (fileData.Position > 0)
                                fileData.Seek(0, SeekOrigin.Begin);

                            using (ZipArchive zipData = new ZipArchive(fileData))
                            {
                                //check for office xml formats
                                var officeXml = CheckForDocxAndXlsxStream(zipData);

                                if (officeXml != null)
                                    return officeXml.Value;

                                //check for open office formats
                                var openOffice = CheckForOdtAndOds(zipData);

                                if (openOffice != null)
                                    return openOffice.Value;
                            }
                        }
                    }
                    else
                    {
                        return type;
                    }
                }
            }

            throw new Exception("No file type match found");
        }

        /// <summary>
        /// Gets the list of FileTypes based on list of extensions in Comma-Separated-Values string
        /// </summary>
        /// <param name="CSV">The CSV String with extensions</param>
        /// <returns>List of FileTypes</returns>
        public static List<FileType> GetFileTypesByExtensions(string CSV)
        {
            string[] extensions = CSV.ToUpper().Replace(" ", "").Split(',');

            List<FileType> result = new List<FileType>();

            foreach (FileType type in Types)
            {
                if (extensions.Contains(type.Extension.ToUpper()))
                    result.Add(type);
            }
            return result;
        }

        private static FileType? CheckForDocxAndXlsxStream(ZipArchive zipData)
        {
            if (zipData.Entries.Any(e => e.FullName.StartsWith("word/")))
                return WORDX;
            else if (zipData.Entries.Any(e => e.FullName.StartsWith("xl/")))
                return EXCELX;
            else if (zipData.Entries.Any(e => e.FullName.StartsWith("ppt/")))
                return PPTX;
            else
                return null;
        }

        /*
		private static FileType CheckForDocxAndXlsx(FileType type, FileInfo fileInfo)
		{
			FileType result = null;

			//check for docx and xlsx
			using (var zipFile = ZipFile.OpenRead(fileInfo.FullName))
			{
				if (zipFile.Entries.Any(e => e.FullName.StartsWith("word/")))
					result = WORDX;
				else if (zipFile.Entries.Any(e => e.FullName.StartsWith("xl/")))
					result = EXCELX;
				else
					result = CheckForOdtAndOds(result, zipFile);
			}
			return result;
		}
		*/

        //check for open doc formats
        private static FileType? CheckForOdtAndOds(ZipArchive zipFile)
        {
            var ooMimeType = zipFile.Entries.FirstOrDefault(e => e.FullName == "mimetype");

            if (ooMimeType == null)
                return null;

            using (var textReader = new StreamReader(ooMimeType.Open()))
            {
                var mimeType = textReader.ReadToEnd();

                if (mimeType == ODT.Mime)
                    return ODT;
                else if (mimeType == ODS.Mime)
                    return ODS;
                else
                    return null;
            }
        }

        private static int GetFileMatchingCount(IReadOnlyList<byte> fileHeader, FileType type)
        {
            int matchingCount = 0;

            for (int i = 0; i < type.Header.Length; i++)
            {
                // if file offset is not set to zero, we need to take this into account when comparing.
                // if byte in type.header is set to null, means this byte is variable, ignore it
                if (type.Header[i] != null && type.Header[i] != fileHeader[i + type.HeaderOffset])
                {
                    // if one of the bytes does not match, move on to the next type
                    matchingCount = 0;
                    break;
                }
                else
                {
                    matchingCount++;
                }
            }

            return matchingCount;
        }

        #endregion Main Methods

        #region Byte Header Get Methods

        /// <summary>
        /// Reads the file header - first (16) bytes from the file
        /// </summary>
        /// <param name="file">The file to work with</param>
        /// <returns>Array of bytes</returns>
        internal static IReadOnlyList<byte> ReadFileHeader(FileInfo file, ushort MaxHeaderSize)
        {
            byte[] header = new byte[MaxHeaderSize];

            try  // read file
            {
                using (FileStream fsSource = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    // read first symbols from file into array of bytes.
                    fsSource.Read(header, 0, MaxHeaderSize);
                }   // close the file stream
            }
            catch (Exception e) // file could not be found/read
            {
                throw new Exception($"Could not read file: {e.Message}");
            }

            return header;
        }

        /// <summary>
        /// Takes a stream does, not dispose of stream, resets read position to beginning though
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="MaxHeaderSize"></param>
        /// <returns></returns>
        internal static IReadOnlyList<byte> ReadHeaderFromStream(Stream stream, ushort MaxHeaderSize)
        {
            byte[] header = new byte[MaxHeaderSize];

            try  // read stream
            {
                if (!stream.CanRead)
                    throw new System.IO.IOException("Could not read from Stream");

                if (stream.Position > 0)
                    stream.Seek(0, SeekOrigin.Begin);

                stream.Read(header, 0, MaxHeaderSize);
            }
            catch (Exception e) // file could not be found/read
            {
                throw new Exception("Could not read Stream : " + e.Message);
            }

            return header;
        }

        internal static IReadOnlyList<byte> ReadHeaderFromByteArray(byte[] byteArray, ushort MaxHeaderSize)
        {
            if (byteArray.Length < MaxHeaderSize)
                throw new ArgumentException($"{nameof(byteArray)}:{byteArray} Is smaller than {nameof(MaxHeaderSize)}:{MaxHeaderSize}", nameof(byteArray));

            byte[] header = new byte[MaxHeaderSize];

            Array.Copy(byteArray, header, MaxHeaderSize);

            return header;
        }

        #endregion Byte Header Get Methods
    }
}
