using System;

namespace Isis.ApplicationBlocks.DataMigrations.Framework
{
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class MigrationAttribute : Attribute
	{
		public int Version { get; set; }

		public MigrationAttribute(int version)
		{
			Version = version;
		}
	}
}