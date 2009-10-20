namespace Isis
{
	/// <summary>
	/// Implemented by attributes which would like to know the type or assembly
	/// to which they have been applied.
	/// </summary>
	public interface IContextAwareAttribute
	{
		void SetContext(object context);
	}
}
