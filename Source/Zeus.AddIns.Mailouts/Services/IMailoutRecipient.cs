using System.Collections.Generic;
using Zeus.AddIns.Mailouts.ContentTypes.FormFieldAnswers;

namespace Zeus.AddIns.Mailouts.Services
{
	public interface IMailoutRecipient
	{
		string Email { get; set; }
		IDictionary<string, FormFieldAnswer> Fields { get; }
		IEnumerable<string> InterestGroups { get; }
	}
}
