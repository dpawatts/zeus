using System;
using System.Web.UI.WebControls;
using Zeus.Integrity;

namespace Zeus.Web.UI.WebControls
{
	public class LocallyUniqueValidator : CustomValidator
	{
		public string PropertyName
		{
			get { return ViewState["PropertyName"] as string ?? string.Empty; }
			set { ViewState["PropertyName"] = value; }
		}

		public string DisplayName
		{
			get { return ViewState["DisplayName"] as string ?? string.Empty; }
			set { ViewState["DisplayName"] = value; }
		}

		protected override bool OnServerValidate(string value)
		{
			ContentItem currentItem = this.FindCurrentItem();

			if (currentItem != null)
			{
				if (!string.IsNullOrEmpty(value))
				{
					// Ensure that the chosen name is locally unique
					if (!Zeus.Context.Current.Resolve<IIntegrityManager>().IsLocallyUnique(this.PropertyName, value, currentItem))
					{
						//Another item with the same parent and the same name was found 
						this.ErrorMessage = string.Format("Another item already has the {0} '{1}'.", this.DisplayName, value);
						return false;
					}
				}
			}

			return true;
		}
	}
}
