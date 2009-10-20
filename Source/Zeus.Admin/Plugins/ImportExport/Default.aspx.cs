using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using Zeus.Security;
using Zeus.Serialization;

namespace Zeus.Admin.Plugins.ImportExport
{
	[AvailableOperation(Operations.ImportExport, "Import / Export", 48)]
	public partial class Default : PreviewFrameAdminPage
	{
		public string UploadedFilePath
		{
			get { return (string) (ViewState["UploadedFilePath"] ?? ""); }
			set { ViewState["UploadedFilePath"] = value; }
		}

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
			base.OnLoad(e);
		}

		protected void btnExport_Command(object sender, CommandEventArgs e)
		{
			ExportOptions options = ExportOptions.Default;
			if (chkDefinedDetails.Checked)
				options |= ExportOptions.OnlyDefinedProperties;

			Engine.Resolve<Exporter>().Export(SelectedItem, options, Response);
		}

		protected void btnVerify_Click(object sender, EventArgs e)
		{
			UploadedFilePath = Path.GetTempFileName() + Path.GetExtension(fuImport.PostedFile.FileName);
			fuImport.PostedFile.SaveAs(UploadedFilePath);

			try
			{
				IImportRecord record = Engine.Resolve<Importer>().Read(UploadedFilePath);
				importedItems.CurrentItem = record.RootItem;
				ShowErrors(record);
			}
			catch (WrongVersionException)
			{
				using (Stream s = File.OpenRead(UploadedFilePath))
				{
					FallbackXmlReader xr = new FallbackXmlReader(Zeus.Context.Current);
					importedItems.CurrentItem = xr.Read(s);
				}
			}
			uploadFlow.ActiveViewIndex = 1;
			uploadFlow.Views[1].DataBind();
		}

		protected void btnUploadImport_Click(object sender, EventArgs e)
		{
			Importer importer = Engine.Resolve<Importer>();

			IImportRecord record;
			try
			{
				record = importer.Read(fuImport.FileContent, fuImport.FileName);
			}
			catch (WrongVersionException)
			{
				FallbackXmlReader xr = new FallbackXmlReader(Zeus.Context.Current);
				ContentItem item = xr.Read(fuImport.FileContent);
				record = CreateRecord(item);
				ShowErrors(record);
			}

			Import(importer, record);
		}

		protected void btnImportUploaded_Click(object sender, EventArgs e)
		{
			Importer importer = Engine.Resolve<Importer>();

			IImportRecord record;
			try
			{
				record = importer.Read(UploadedFilePath);
				ShowErrors(record);
			}
			catch (WrongVersionException)
			{
				FallbackXmlReader xr = new FallbackXmlReader(Zeus.Context.Current);
				ContentItem item = xr.Read(File.OpenRead(UploadedFilePath));
				record = CreateRecord(item);
			}

			Import(importer, record);
		}

		private static IImportRecord CreateRecord(ContentItem item)
		{
			ReadingJournal rj = new ReadingJournal();
			rj.Report(item);
			return rj;
		}

		private void Import(Importer importer, IImportRecord record)
		{
			try
			{
				if (chkSkipRoot.Checked)
				{
					importer.Import(record, SelectedItem, ImportOptions.Children);
					Refresh(SelectedItem, AdminFrame.Both, false);
				}
				else
				{
					importer.Import(record, SelectedItem, ImportOptions.AllItems);
					Refresh(record.RootItem, AdminFrame.Both, false);
				}

				ShowErrors(record);
			}
			catch (ZeusException ex)
			{
				cvImport.ErrorMessage = ex.Message;
				cvImport.IsValid = false;
				btnImportUploaded.Enabled = false;
			}
			finally
			{
				if (File.Exists(UploadedFilePath))
					File.Delete(UploadedFilePath);
			}
		}

		void ShowErrors(IImportRecord record)
		{
			if (record.Errors.Count > 0)
			{
				StringBuilder errorText = new StringBuilder("<ul>");
				foreach (Exception ex in record.Errors)
					errorText.Append("<li>").Append(ex.Message).Append("</li>");
				errorText.Append("</ul>");

				cvImport.IsValid = false;
				cvImport.ErrorMessage = errorText.ToString();
			}
		}

		protected void btnImport_Click(object sender, EventArgs e)
		{
			uploadFlow.ActiveViewIndex = 0;
		}

		protected override void OnPreRender(EventArgs e)
		{
			//Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");

			exportedItems.CurrentItem = SelectedItem;
			tbpExport.DataBind();

			base.OnPreRender(e);
		}

		#endregion
	}
}