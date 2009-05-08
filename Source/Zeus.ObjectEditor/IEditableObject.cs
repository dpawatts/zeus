namespace Zeus.ObjectEditor
{
	public interface IEditableObject
	{
		IPropertyCollection GetPropertyCollection(string name, bool create);
		object GetPropertyValue(string name);
		void SetPropertyValue(string name, object value);
	}
}