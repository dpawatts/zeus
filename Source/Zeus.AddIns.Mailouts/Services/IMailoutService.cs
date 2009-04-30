using System.Collections.Generic;
using System.Linq;
using Isis.ComponentModel;
using Zeus.AddIns.Mailouts.ContentTypes;

namespace Zeus.AddIns.Mailouts.Services
{
	public interface IMailoutService : IService
	{
		IQueryable<IMailoutRecipient> GetRecipients(Campaign campaign);
		void Send(Campaign campaign);
	}
}
