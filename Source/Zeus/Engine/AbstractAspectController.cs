using Zeus.Web;

namespace Zeus.Engine
{
	/// <summary>
	/// Convenience base class for aspect controllers. One instance 
	/// of the inheriting class is created per request upon usage.
	/// </summary>
	public abstract class AbstractAspectController : IAspectController
	{
		#region IAspectController Members

		/// <summary>The path associated with this controller instance.</summary>
		public PathData Path { get; set; }

		/// <summary>The content engine requesting external control.</summary>
		public ContentEngine Engine { get; set; }

		#endregion
	}
}