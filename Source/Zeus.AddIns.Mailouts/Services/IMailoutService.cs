using System.Linq;
using Zeus.AddIns.Mailouts.ContentTypes;

namespace Zeus.AddIns.Mailouts.Services
{
	public interface IMailoutService
	{
		IQueryable<IMailoutRecipient> GetRecipients(Campaign campaign);
		void Send(Campaign campaign);
	}
}
