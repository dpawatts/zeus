namespace Zeus.DynamicContent
{
	public interface IDynamicContent
	{
		string State { get; set; }
		string Render();
	}
}