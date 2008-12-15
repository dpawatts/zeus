using System;
using System.ComponentModel;
using Isis.Drawing;
using Zeus;

namespace SoundInTheory.DynamicImage.Sources
{
	public class ZeusImageSource : ImageSource
	{
		public string PropertyName
		{
			get { return this.ViewState["PropertyName"] as string ?? string.Empty; }
			set { this.ViewState["PropertyName"] = value; }
		}

		public int ContentID
		{
			get
			{
				object value = this.ViewState["ContentID"];
				if (value != null)
					return (int) value;
				return 0;
			}
			set { this.ViewState["ContentID"] = value; }
		}

		public override FastBitmap GetBitmap(ISite site, bool designMode)
		{
			ContentItem contentItem = Context.Persister.Get(this.ContentID);
			byte[] imageBytes = contentItem[this.PropertyName] as byte[];
			return (imageBytes != null) ? new FastBitmap(imageBytes) : null;
		}
	}
}
