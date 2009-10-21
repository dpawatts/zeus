using Isis.Web;
using Rhino.Mocks;
using Isis.Web.Security;
using System.Web;
using Isis.Web.Configuration;

namespace Isis.FrameworkBlocks.WebAuthentication.Tests.Security
{
	/*[TestFixture]
	public class AuthenticationServiceTests
	{
		private IWebContext _webContext;
		private AuthenticationSection _config;

		[SetUp]
		public void SetUp()
		{
			_webContext = MockRepository.GenerateStub<WebContext>();
			_config = new AuthenticationSection();
		}

		[Test]
		public void Test_CanSetAuthCookie()
		{
			// Arrange.
			var request = MockRepository.GenerateStub<HttpRequestBase>();
			_webContext.Stub(x => x.Request).Return(request);

			var response = MockRepository.GenerateStub<HttpResponseBase>();
			response.Stub(x => x.Cookies).Return(new HttpCookieCollection());
			_webContext.Stub(x => x.Response).Return(response);

			// Act.
			var authenticationService = new AuthenticationService(_webContext, _config);
			authenticationService.SetAuthCookie("myuser", true);

			// Assert.
			HttpCookie cookie = response.Cookies[0];
			Assert.IsNotNull(cookie, "Cookie should not be null");
			Assert.AreEqual(_config.Name, cookie.Name);
			Assert.AreEqual(_config.Path, cookie.Path);
			Assert.AreEqual(_config.RequireSsl, cookie.Secure);
			Assert.AreEqual(_config.Domain, cookie.Domain);
		}

		[Test]
		public void Test_CanSetAndGetAuthCookie()
		{
			// Arrange.
			var request = MockRepository.GenerateStub<HttpRequestBase>();
			_webContext.Stub(x => x.Request).Return(request);

			var response = MockRepository.GenerateStub<HttpResponseBase>();
			response.Stub(x => x.Cookies).Return(new HttpCookieCollection());
			_webContext.Stub(x => x.Response).Return(response);

			// Act.
			var authenticationService = new AuthenticationService(_webContext, _config);
			authenticationService.SetAuthCookie("myuser", true);

			// Need to copy cookie manually from response to request.

			AuthenticationTicket ticket = authenticationService.ExtractTicketFromCookie();

			// Assert.
			Assert.IsNotNull(ticket, "Ticket should not be null");
			Assert.AreEqual(_config.Name, ticket.Name);
			Assert.AreEqual(_config.Path, ticket.CookiePath);
		}
	}*/
}
