using Zeus.ContentProperties;

namespace Zeus.ContentTypes
{
	public interface IEditableObject
	{
		object this[string detailName] { get; set; }
		PropertyCollection GetDetailCollection(string name, bool create);
		object GetDetail(string name);
		void SetDetail(string name, object value);
	}
}