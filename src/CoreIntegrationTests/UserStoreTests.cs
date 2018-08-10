namespace IntegrationTests
{
	using System.Linq;
	using System.Threading.Tasks;
    using CoreTests;
    using Microsoft.AspNetCore.Identity.MongoDB;
	using MongoDB.Bson;
	using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class UserStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task Create_NewUser_Saves()
		{
			var userName = "name";
			var user = new IdentityUserObjectId { UserName = userName};
			var manager = GetUserManager();

			await manager.CreateAsync(user);

			var savedUser = Users.FindAll().Single();
			Expect(savedUser.UserName, Is.EqualTo(user.UserName));
		}

		[Test]
		public async Task FindByName_SavedUser_ReturnsUser()
		{
			var userName = "name";
			var user = new IdentityUserObjectId { UserName = userName};
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			var foundUser = await manager.FindByNameAsync(userName);

			Expect(foundUser, Is.Not.Null);
			Expect(foundUser.UserName, Is.EqualTo(userName));
		}

		[Test]
		public async Task FindByName_NoUser_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByNameAsync("nouserbyname");

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task FindById_SavedUser_ReturnsUser()
		{
			var userId = ObjectId.GenerateNewId();
			var user = new IdentityUserObjectId { UserName = "name"};
			user.Id = userId;
			var manager = GetUserManager();
			await manager.CreateAsync(user);

			var foundUser = await manager.FindByIdAsync(userId.ToString());

			Expect(foundUser, Is.Not.Null);
			Expect(foundUser.Id, Is.EqualTo(userId));
		}

		[Test]
		public async Task FindById_NoUser_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByIdAsync(ObjectId.GenerateNewId().ToString());

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task FindById_IdIsNotAnObjectId_ReturnsNull()
		{
			var manager = GetUserManager();

			var foundUser = await manager.FindByIdAsync("notanobjectid");

			Expect(foundUser, Is.Null);
		}

		[Test]
		public async Task Delete_ExistingUser_Removes()
		{
			var user = new IdentityUserObjectId { UserName = "name"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);
			Expect(Users.FindAll(), Is.Not.Empty);

			await manager.DeleteAsync(user);

			Expect(Users.FindAll(), Is.Empty);
		}

		[Test]
		public async Task Update_ExistingUser_Updates()
		{
			var user = new IdentityUserObjectId { UserName = "name"};
			var manager = GetUserManager();
			await manager.CreateAsync(user);
			var savedUser = await manager.FindByIdAsync(user.Id.ToString());
			savedUser.UserName = "newname";

			await manager.UpdateAsync(savedUser);

			var changedUser = Users.FindAll().Single();
			Expect(changedUser, Is.Not.Null);
			Expect(changedUser.UserName, Is.EqualTo("newname"));
		}

		[Test]
		public async Task SimpleAccessorsAndGetters()
		{
			var user = new IdentityUserObjectId
            {
				UserName = "username"
			};
			var manager = GetUserManager();
			await manager.CreateAsync(user);

            Expect(await manager.GetUserIdAsync(user), Is.EqualTo(user.Id.ToString()));
			Expect(await manager.GetUserNameAsync(user), Is.EqualTo("username"));

			await manager.SetUserNameAsync(user, "newUserName");
			Expect(await manager.GetUserNameAsync(user), Is.EqualTo("newUserName"));
		}
	}
}