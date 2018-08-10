namespace CoreTests
{
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;
    using NUnit.Framework;

	[TestFixture]
	public class MongoIdentityBuilderExtensionsTests : AssertionHelper
	{
		private const string FakeConnectionStringWithDatabase = "mongodb://fakehost:27017/database";

		//[Test]
		//public void AddMongoStores_WithDefaultTypes_ResolvesStoresAndManagers()
		//{
		//	var services = new ServiceCollection();
		//	services.AddIdentityWithMongoStores(FakeConnectionStringWithDatabase);
		//	// note: UserManager and RoleManager use logging
		//	services.AddLogging();

		//	var provider = services.BuildServiceProvider();
		//	var resolvedUserStore = provider.GetService<IUserStore<IdentityUser>>();
		//	Expect(resolvedUserStore, Is.Not.Null, "User store did not resolve");

		//	var resolvedRoleStore = provider.GetService<IRoleStore<IdentityRole>>();
		//	Expect(resolvedRoleStore, Is.Not.Null, "Role store did not resolve");

		//	var resolvedUserManager = provider.GetService<UserManager<IdentityUser>>();
		//	Expect(resolvedUserManager, Is.Not.Null, "User manager did not resolve");

		//	var resolvedRoleManager = provider.GetService<RoleManager<IdentityRole>>();
		//	Expect(resolvedRoleManager, Is.Not.Null, "Role manager did not resolve");
		//}

		protected class CustomUser : IdentityUserObjectId
        {
		}

		protected class CustomRole : IdentityRoleObjectId
		{
		}

		[Test]
		public void AddMongoStores_WithCustomTypes_ThisShouldLookReasonableForUsers()
		{
			// this test is just to make sure I consider the interface for using custom types
			// so that it's not a horrible experience even though it should be rarely used
			var services = new ServiceCollection();
			services.AddIdentityWithMongoStoresUsingCustomTypesWithObjectIds<CustomUser, CustomRole>(FakeConnectionStringWithDatabase);
			services.AddLogging();

			var provider = services.BuildServiceProvider();
			var resolvedUserStore = provider.GetService<IUserStore<CustomUser>>();
			Expect(resolvedUserStore, Is.Not.Null, "User store did not resolve");

			var resolvedRoleStore = provider.GetService<IRoleStore<CustomRole>>();
			Expect(resolvedRoleStore, Is.Not.Null, "Role store did not resolve");

			var resolvedUserManager = provider.GetService<UserManager<CustomUser>>();
			Expect(resolvedUserManager, Is.Not.Null, "User manager did not resolve");

			var resolvedRoleManager = provider.GetService<RoleManager<CustomRole>>();
			Expect(resolvedRoleManager, Is.Not.Null, "Role manager did not resolve");
		}

		protected class WrongUser : IdentityUserObjectId
        {
		}

		protected class WrongRole : IdentityRoleObjectId
        {
		}
	}
}