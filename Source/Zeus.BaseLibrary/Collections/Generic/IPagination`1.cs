using System.Collections;
using System.Collections.Generic;

namespace Zeus.BaseLibrary.Collections.Generic
{
	/// <summary>
	/// Generic form of <see cref="IPagination"/>
	/// </summary>
	/// <typeparam name="T">Type of object being paged</typeparam>
	public interface IPagination<T> : IPagination, IEnumerable<T>
	{

	}
}