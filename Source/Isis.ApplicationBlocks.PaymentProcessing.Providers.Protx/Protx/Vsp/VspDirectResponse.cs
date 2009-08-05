using System;
using System.IO;

namespace Protx.Vsp
{
	[Obsolete("Replaced with DirectResponse for naming consistency")]
	public class VspDirectResponse : DirectResponse
	{
		internal VspDirectResponse(VspTransaction tx, Stream responseStream) : base(tx, responseStream)
		{
		}
	}
}