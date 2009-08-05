using MbUnit.Framework;
using Rhino.Mocks;
using Isis.Web.Security;
using System.Web;
using Isis.Web.Configuration;

namespace Isis.FrameworkBlocks.WebAuthentication.Tests.Security
{
	/*[TestFixture]
	public class MembershipServiceTests
	{
		private IUserRepository _userRepository;

		[SetUp]
		public void SetUp()
		{
			_userRepository = MockRepository.GenerateStub<IUserRepository>();
		}

		[Test]
		public void Test_CanValidateUser()
		{
			// Arrange.
			var theUser = new AuthenticatedUser { Username = "myuser", Password = "mypassword" };
			_userRepository.Stub(x => x.GetUser("myuser")).Return(theUser);

			// Act.
			var membershipService = new MembershipService(_userRepository);
			bool result = membershipService.ValidateUser("myuser", "mypassword");

			// Assert.
			Assert.IsTrue(result, "User validation should succeed");
		}
	}*/
}
