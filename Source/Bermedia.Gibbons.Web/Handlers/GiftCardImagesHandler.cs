using System;
using System.Web;
using System.IO;
using Zeus.Admin;
using Zeus;

namespace Bermedia.Gibbons.Web.Handlers
{
	public class GiftCardImagesHandler : IHttpHandler
	{
		public bool IsReusable
		{
			get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
			// Clean temp folder of all images older than one day.
			CleanTempFolder(context);

			switch (context.Request.QueryString["O"])
			{
				case "Upload" :
					UploadImage(context);
					break;
				case "Save" :
					SaveImage(context);
					break;
			}
		}

		private void CleanTempFolder(HttpContext context)
		{
			string directory = context.Server.MapPath("~/Assets/Images/Temp");
			foreach (FileInfo fileInfo in new DirectoryInfo(directory).GetFiles())
				if (fileInfo.LastAccessTime < DateTime.Today.AddDays(-1))
					fileInfo.Delete();
		}

		private void UploadImage(HttpContext context)
		{
			// Save posted file to temp folder.
			HttpPostedFile postedFile = context.Request.Files["Filedata"];
			string directory = context.Server.MapPath("~/Assets/Images/Temp");
			string filename = Guid.NewGuid().ToString() + Path.GetExtension(postedFile.FileName);
			postedFile.SaveAs(Path.Combine(directory, filename));

			context.Response.Write("/Assets/Images/Temp/" + filename);
		}

		private void SaveImage(HttpContext context)
		{
			byte[] imageData = context.Request.BinaryRead(context.Request.TotalBytes);

			Zeus.AddIns.Images.Items.Image image = new Zeus.AddIns.Images.Items.Image();
			image.ContentType = "image/jpeg";
			image.Size = imageData.Length;
			image.Data = imageData;

			ContentItem parentImageFolder = Zeus.Context.Current.Resolve<Navigator>().Navigate("~/Upload/GiftCards/Personalised");
			image.AddTo(parentImageFolder);

			Zeus.Context.Persister.Save(image);

			string url = string.Format("/shopping-cart.aspx?add=gc%26imageid={0}%26am={1}%26qty={2}",
				image.ID, context.Request.QueryString["am"], context.Request.QueryString["qty"]);

			context.Response.Write(url);
		}
	}
}
