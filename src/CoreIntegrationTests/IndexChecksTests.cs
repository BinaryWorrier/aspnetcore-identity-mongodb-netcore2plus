namespace IntegrationTests
{
	using System;
	using System.Linq;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using MongoDB.Driver;
	using NUnit.Framework;

	[TestFixture]
	public class IndexChecksTests : UserIntegrationTestsBase
	{
		[Test]
		public void EnsureUniqueIndexes()
		{
			EnsureUniqueIndex<IdentityUserObjectId>(IndexChecks.OptionalIndexChecks.EnsureUniqueIndexOnUserName, "UserName");
			EnsureUniqueIndex<IdentityUserObjectId>(IndexChecks.OptionalIndexChecks.EnsureUniqueIndexOnEmail, "Email");
			EnsureUniqueIndex<IdentityRoleObjectId>(IndexChecks.OptionalIndexChecks.EnsureUniqueIndexOnRoleName, "Name");

			EnsureUniqueIndex<IdentityUserObjectId>(IndexChecks.EnsureUniqueIndexOnNormalizedUserName, "NormalizedUserName");
			EnsureUniqueIndex<IdentityUserObjectId>(IndexChecks.EnsureUniqueIndexOnNormalizedEmail, "NormalizedEmail");
			EnsureUniqueIndex<IdentityRoleObjectId>(IndexChecks.EnsureUniqueIndexOnNormalizedRoleName, "NormalizedName");
		}

		private void EnsureUniqueIndex<TCollection>(Action<IMongoCollection<TCollection>> addIndex, string indexedField)
		{
			var testCollectionName = "indextest";
			Database.DropCollection(testCollectionName);
			var testCollection = DatabaseNewApi.GetCollection<TCollection>(testCollectionName);

			addIndex(testCollection);

            System.Threading.Thread.Sleep(500);

			var legacyCollectionInterface = Database.GetCollection(testCollectionName);
			var index = legacyCollectionInterface.GetIndexes()
				.Where(i => i.IsUnique)
				.Where(i => i.Key.Count() == 1)
				.FirstOrDefault(i => i.Key.Contains(indexedField));
			var failureMessage = $"No unique index found on {indexedField}";
			Expect(index, Is.Not.Null, failureMessage);
			Expect(index.Key.Count(), Is.EqualTo(1), failureMessage);
		}
	}
}