using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using Spark.Spool;

namespace Zeus.Web.Mvc.Pdf
{
	public class PdfViewResult : ViewResult
	{
		private readonly Stream _template;
		private readonly Assembly[] _resourceAssemblies;

		public PdfViewResult(Stream template, Assembly[] resourceAssemblies)
		{
			_template = template;
			_resourceAssemblies = resourceAssemblies;
		}

		public PdfViewResult()
		{

		}

		protected override ViewEngineResult FindView(ControllerContext context)
		{
			var result = base.FindView(context);
			if (result.View == null)
				return result;

			var pdfView = new PdfView(result, _template, _resourceAssemblies);
			return new ViewEngineResult(pdfView, pdfView);
		}

		internal class PdfView : IView, IViewEngine
		{
			private readonly ViewEngineResult _result;
			private readonly Stream _template;
			private readonly Assembly[] _resourceAssemblies;

			static PdfView()
			{
				
			}

			public PdfView(ViewEngineResult result, Stream template, Assembly[] resourceAssemblies)
			{
				_result = result;
				_template = template;
				_resourceAssemblies = resourceAssemblies;
			}

			public void Render(ViewContext viewContext, TextWriter writer)
			{
				// generate view in memory
				var spoolWriter = new SpoolWriter();
				_result.View.Render(viewContext, spoolWriter);

				// detect itext (or html) format of response
				XmlParser parser;
				using (var reader = GetXmlReader(spoolWriter))
				{
					while (reader.Read() && reader.NodeType != XmlNodeType.Element)
					{
						// no-op
					}

					if (reader.NodeType == XmlNodeType.Element && reader.Name == "itext")
						parser = new XmlParser();
					else
						parser = new HtmlParser();
				}

				// Add current assembly to resource search (for embedded fonts).
				if (_resourceAssemblies != null)
					foreach (Assembly assembly in _resourceAssemblies)
						BaseFont.AddToResourceSearch(assembly);

				// Create a document processing context
				var document = new Document();
				document.Open();

				// Associate output with stream.
				MemoryStream docStream = new MemoryStream();
				var pdfWriter = PdfWriter.GetInstance(document, docStream);
				pdfWriter.CloseStream = false;

				// this is as close as we can get to being "success" before writing output
				// so set the content type now
				viewContext.HttpContext.Response.ContentType = "application/pdf";

				// parse memory through document into output
				using (var reader = GetXmlReader(spoolWriter))
				{
					parser.Go(document, reader);
				}

				pdfWriter.Close();

				// If template has been specified, load it now.
				if (_template != null)
					docStream = AddTemplate(docStream);

				byte[] bytes = docStream.ToArray();
				viewContext.HttpContext.Response.BinaryWrite(bytes);
				docStream.Close();
			}

			private MemoryStream AddTemplate(Stream docStream)
			{
				docStream.Seek(0, SeekOrigin.Begin);

				// add template to each page
				PdfReader templatePdfReader = new PdfReader(_template);
				PdfReader newPdfReader = new PdfReader(docStream);

				MemoryStream output = new MemoryStream();
				PdfStamper pdfStamper = new PdfStamper(newPdfReader, output);
				pdfStamper.Writer.CloseStream = false;
				pdfStamper.GetUnderContent(1).AddTemplate(pdfStamper.GetImportedPage(templatePdfReader, 1), 0, 0);
				pdfStamper.Close();

				templatePdfReader.Close();
				newPdfReader.Close();

				return output;
			}

			private static XmlTextReader GetXmlReader(IEnumerable<string> source)
			{
				return new XmlTextReader(new SpoolReader(source));
			}

			public ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
			{
				throw new System.NotImplementedException();
			}

			public ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
			{
				throw new System.NotImplementedException();
			}

			public void ReleaseView(ControllerContext controllerContext, IView view)
			{
				_result.ViewEngine.ReleaseView(controllerContext, _result.View);
			}
		}
	}
}