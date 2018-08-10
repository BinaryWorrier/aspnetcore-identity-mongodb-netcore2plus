﻿namespace IntegrationTests
{
	using System;
    using CoreTests;
    using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;
    using MongoDB.Driver;
	using NUnit.Framework;

	public class UserIntegrationTestsBase : AssertionHelper
	{
		protected MongoDatabase Database;
		protected MongoCollection<IdentityUserObjectId> Users;
		protected MongoCollection<IdentityRoleObjectId> Roles;

		// note: for now we'll have interfaces to both the new and old apis for MongoDB, that way we don't have to update all the tests at once and risk introducing bugs
		protected IMongoDatabase DatabaseNewApi;
		protected IServiceProvider ServiceProvider;
        //private readonly string _TestingConnectionString = $"mongodb://10.20.30.106:27017/{IdentityTesting}";
        private readonly string _TestingConnectionString = $"mongodb://localhost:27017/{IdentityTesting}";
        private const string IdentityTesting = "identity-testing";

		[SetUp]
		public void BeforeEachTest()
		{
			var client = new MongoClient(_TestingConnectionString);

			// todo move away from GetServer which could be deprecated at some point
			Database = client.GetServer().GetDatabase(IdentityTesting);
            Users = Database.GetCollection<IdentityUserObjectId>("users");
			Roles = Database.GetCollection<IdentityRoleObjectId>("roles");

			DatabaseNewApi = client.GetDatabase(IdentityTesting);

			Database.DropCollection("users");
			Database.DropCollection("roles");

            ServiceProvider = CreateServiceProvider<IdentityUserObjectId, IdentityRoleObjectId>();
		}

		protected UserManager<IdentityUserObjectId> GetUserManager()
			=> ServiceProvider.GetService<UserManager<IdentityUserObjectId>>();

		protected RoleManager<IdentityRoleObjectId> GetRoleManager()
			=> ServiceProvider.GetService<RoleManager<IdentityRoleObjectId>>();

        protected IServiceProvider CreateServiceProvider<TUser,  TRole>(Action<IdentityOptions> optionsProvider = null)
            where TUser : IdentityUser<ObjectId>
            where TRole : IdentityRole<ObjectId>
        {
            var services = new ServiceCollection();
            optionsProvider = optionsProvider ?? (options => { });
            services.AddIdentity<TUser, TRole>(optionsProvider)
                .AddDefaultTokenProviders()
                .RegisterMongoStores<TUser, ObjectId, TRole>(_TestingConnectionString, col => new UserStoreObjectId<TUser>(col), col => new RoleStoreObjectId<TRole>(col));

            services.AddLogging();

            return services.BuildServiceProvider();
        }

    }
}