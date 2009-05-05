using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Zeus.Serialization
{
	/// <summary>
	/// Exports items to a stream.
	/// </summary>
	public class Exporter
	{
		private readonly ItemXmlWriter itemWriter;
		private Formatting xmlFormatting = Formatting.Indented;

		public Exporter(ItemXmlWriter itemWriter)
		{
			this.itemWriter = itemWriter;
		}

		public Formatting XmlFormatting
		{
			get { return xmlFormatting; }
			set { xmlFormatting = value; }
		}

		public virtual void Export(ContentItem item, ExportOptions options, HttpResponse response)
		{
			response.ContentType = GetContentType();
			response.AppendHeader("Content-Disposition", "attachment;filename=" + GetExportFilename(item));

			using (TextWriter output = GetTextWriter(response))
			{
				Export(item, options, output);
				output.Flush();
			}
			response.End();
		}

		protected virtual string GetContentType()
		{
			return "application/xml";
		}

		protected virtual TextWriter GetTextWriter(HttpResponse response)
		{
			return response.Output;
		}

		protected virtual string GetExportFilename(ContentItem item)
		{
			return Regex.Replace(item.Title.Replace(' ', '_'), "[^a-zA-Z0-9_-]", "") + ".zeus.xml";
		}

		public virtual void Export(ContentItem item, ExportOptions options, TextWriter output)
		{
			XmlTextWriter xmlOutput = new XmlTextWriter(output);
			xmlOutput.Formatting = XmlFormatting;
			xmlOutput.WriteStartDocument();

			using (ElementWriter envelope = new ElementWriter("zeus", xmlOutput))
			{
				envelope.WriteAttribute("version", GetType().Assembly.GetName().Version.ToString());
				envelope.WriteAttribute("exportVersion", 1);
				envelope.WriteAttribute("exportDate", DateTime.Now);

				itemWriter.Write(item, options, xmlOutput);
			}

			xmlOutput.WriteEndDocument();
			xmlOutput.Flush();
		}
	}
}