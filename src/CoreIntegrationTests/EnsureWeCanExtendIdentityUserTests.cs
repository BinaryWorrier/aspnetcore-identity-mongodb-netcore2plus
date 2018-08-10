namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
    using CoreTests;
    using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
	using NUnit.Framework;

	[TestFixture]
	public class EnsureWeCanExtendIdentityUserTests : UserIntegrationTestsBase
	{
		private UserManager<ExtendedIdentityUser> _Manager;
		private ExtendedIdentityUser _User;

		public class ExtendedIdentityUser : IdentityUserObjectId
        {
			public string ExtendedField { get; set; }
		}

		[SetUp]
		public void BeforeEachTestAfterBase()
		{
			_Manager = CreateServiceProvider<ExtendedIdentityUser, IdentityRoleObjectId>()
				.GetService<UserManager<ExtendedIdentityUser>>();
			_User = new ExtendedIdentityUser
			{
				UserName = "bob"
			};
		}

		[Test]
		public async Task Create_ExtendedUserType_SavesExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = Users.FindAllAs<ExtendedIdentityUser>().Single();
			Expect(savedUser.ExtendedField, Is.EqualTo("extendedField"));
		}

		[Test]
		public async Task Create_ExtendedUserType_ReadsExtraFields()
		{
			_User.ExtendedField = "extendedField";

			await _Manager.CreateAsync(_User);

			var savedUser = await _Manager.FindByIdAsync(_User.Id.ToString());
			Expect(savedUser.ExtendedField, Is.EqualTo("extendedField"));
		}
	}
}