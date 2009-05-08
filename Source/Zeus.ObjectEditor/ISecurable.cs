namespace Zeus.ObjectEditor
{
	/// <summary>
	/// Editable attributes implementing this interface can have their 
	/// AuthorizedRoles property set through an external attribute.
	/// </summary>
	public interface ISecurable
	{
		string[] AuthorizedRoles { get; set; }
	}
}