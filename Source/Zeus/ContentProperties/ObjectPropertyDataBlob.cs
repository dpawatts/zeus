namespace Zeus.ContentProperties
{
	public class ObjectPropertyDataBlob
	{
		private object _blob;

		public virtual int ID { get; set; }

		public virtual object Blob
		{
			get { return _blob; }
			set { _blob = value; }
		}
	}
}