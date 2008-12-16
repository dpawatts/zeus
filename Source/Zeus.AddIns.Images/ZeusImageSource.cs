using System;
using System.ComponentModel;
using Isis.Drawing;
using Zeus;
using Zeus.FileSystem;
using SoundInTheory.DynamicImage.Sources;
using System.Collections.Generic;
using SoundInTheory.DynamicImage.Caching;
using Zeus.AddIns.Images.Items;

namespace Zeus.AddIns.Images
{
	public class ZeusImageSource : ImageSource
	{
		#region Properties

		public int ContentID
		{
			get { return (int) (this.ViewState["ContentID"] ?? 0); }
			set { this.ViewState["ContentID"] = value; }
		}

		#endregion

		public override FastBitmap GetBitmap(ISite site, bool designMode)
		{
			Image image = Context.Persister.Get(this.ContentID) as Image;
			return (image != null && image.Data != null) ? new FastBitmap(image.Data) : null;
		}

		public override void PopulateDependencies(List<Dependency> dependencies)
		{
			base.PopulateDependencies(dependencies);
			dependencies.Add(new Dependency { Text1 = this.ContentID.ToString() });
		}
	}
}
