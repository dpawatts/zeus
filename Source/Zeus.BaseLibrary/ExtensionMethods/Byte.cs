using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class ByteExtensionMethods
	{
		public static T ToDeserializedObject<T>(this byte[] array)
		{
			using (MemoryStream stream = new MemoryStream(array))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				return (T) binaryFormatter.Deserialize(stream);
			}
		}
	}
}