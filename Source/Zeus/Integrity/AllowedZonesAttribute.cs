﻿using System;
using System.Collections.Generic;
using Zeus.ContentTypes;

namespace Zeus.Integrity
{
	/// <summary>
	/// Class decoration that lets N2 which zones a data item can be added to. 
	/// This is mostly a hint for the user interface. Placing an item in a zone
	/// merly means assigning the child item's ZoneName property a meaningful 
	/// string.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class AllowedZonesAttribute : AbstractContentTypeRefiner, IInheritableDefinitionRefiner
	{
		private readonly AllowedZones allowedIn = AllowedZones.SpecifiedZones;

		/// <summary>Initializes a new instance of the AllowedZonesAttribute which is used to restrict which zones item may have.</summary>
		public AllowedZonesAttribute(AllowedZones allowedIn)
		{
			this.allowedIn = allowedIn;
			if (allowedIn == AllowedZones.All)
				ZoneNames = null;
			else
				ZoneNames = new string[0];
		}

		/// <summary>Initializes a new instance of the AllowedZonesAttribute which is used to restrict which zones item may have.</summary>
		/// <param name="zoneNames">A list of allowed zone names. Null is interpreted as any/all zone.</param>
		public AllowedZonesAttribute(params string[] zoneNames)
		{
			ZoneNames = zoneNames;
		}

		/// <summary>Gets or sets zones the item can be added to.</summary>
		public string[] ZoneNames { get; set; }

		public override void Refine(ContentType currentDefinition, IList<ContentType> allDefinitions)
		{
			currentDefinition.AllowedIn = allowedIn;
			currentDefinition.AllowedZoneNames = ZoneNames;
		}
	}
}
