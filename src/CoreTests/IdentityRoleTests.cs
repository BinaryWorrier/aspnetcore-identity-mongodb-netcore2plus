namespace Tests
{
	using Microsoft.AspNetCore.Identity.MongoDB;
	using MongoDB.Bson;
	using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class IdentityRoleTests : AssertionHelper
	{
        public IdentityRoleTests()
        {
        }

        [Test]
		public void ToBsonDocument_IdAssigned_MapsToBsonObjectId()
		{
			var role = new IdentityRoleObjectId();

			var document = role.ToBsonDocument();

			Expect(document["_id"], Is.TypeOf<BsonObjectId>());
		}

		[Test]
		public void Create_WithoutRoleName_HasIdAssigned()
		{
			var role = new IdentityRoleObjectId();

			var parsed = role.Id;
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}

		[Test]
		public void Create_WithRoleName_SetsName()
		{
			var name = "admin";

			var role = new IdentityRoleObjectId(name);

			Expect(role.Name, Is.EqualTo(name));
		}

		[Test]
		public void Create_WithRoleName_SetsId()
		{
			var role = new IdentityRoleObjectId("admin");

			var parsed = role.Id;
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}
	}
}