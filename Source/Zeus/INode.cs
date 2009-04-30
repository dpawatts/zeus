namespace Zeus
{
	public interface INode : Web.ILink
	{
		int ID { get; }

		/// <summary>The logical path to the node from the root node.</summary>
		string Path { get; }

		/// <summary>The url used to preview the node in edit mode.</summary>
		string PreviewUrl { get; }

		/// <summary>Url to an icon image.</summary>
		string IconUrl { get; }

		/// <summary>Gets a whitespace separated list of class names used to decorate the node for editors.</summary>
		string ClassNames { get; }
	}
}
