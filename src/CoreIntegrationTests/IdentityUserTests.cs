namespace IntegrationTests
{
    using CoreTests;
    using Microsoft.AspNetCore.Identity.MongoDB;
	using MongoDB.Bson;
	using NUnit.Framework;
	using Tests;

	[TestFixture]
	public class IdentityUserTests : UserIntegrationTestsBase
	{
		[Test]
		public void Insert_NoId_SetsId()
		{
			var user = new IdentityUserObjectId();
			user.Id = ObjectId.Empty;

			Users.Insert(user);

			Expect(user.Id, Is.Not.Null);
			var parsed = user.Id;
			Expect(parsed, Is.Not.Null);
			Expect(parsed, Is.Not.EqualTo(ObjectId.Empty));
		}
	}
}