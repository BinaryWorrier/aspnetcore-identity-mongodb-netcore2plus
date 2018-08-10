// ReSharper disable once CheckNamespace - Common convention to locate extensions in Microsoft namespaces for simplifying autocompletion as a consumer.

namespace Microsoft.Extensions.DependencyInjection
{
	using System;
	using AspNetCore.Identity;
	using AspNetCore.Identity.MongoDB;
    using MongoDB.Bson;
    using MongoDB.Driver;

	public static class MongoIdentityBuilderExtensions
	{

        /// <summary>
        ///     This method only registers mongo stores, you also need to call AddIdentity.
        ///     Assumes you want ObjectIds
        ///     Consider using AddIdentityWithMongoStores.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">Must contain the database name</param>
        public static IdentityBuilder RegisterMongoStores<TUser, TRole>(this IdentityBuilder builder, string connectionString)
            where TRole : IdentityRoleObjectId
            where TUser : IdentityUserObjectId
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            if (url.DatabaseName == null)
            {
                throw new ArgumentException("Your connection string must contain a database name", connectionString);
            }
            var database = client.GetDatabase(url.DatabaseName);
            return builder.RegisterMongoStores(
                p => database.GetCollection<TUser>("users"),
                p => database.GetCollection<TRole>("roles"),
                col => new UserStoreObjectId<TUser>(col),
                col => new RoleStoreObjectId<TRole>(col));
        }
        /// <summary>
        ///     This method only registers mongo stores, you also need to call AddIdentity.
        ///     Consider using AddIdentityWithMongoStores.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">Must contain the database name</param>
        public static IdentityBuilder RegisterMongoStoresWithGuids<TUser, TRole>(this IdentityBuilder builder, string connectionString)
            where TRole : IdentityRoleGuid
            where TUser : IdentityUserGuid
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            if (url.DatabaseName == null)
            {
                throw new ArgumentException("Your connection string must contain a database name", connectionString);
            }
            var database = client.GetDatabase(url.DatabaseName);
            return builder.RegisterMongoStores(
                p => database.GetCollection<TUser>("users"),
                p => database.GetCollection<TRole>("roles"),
                col => new UserStoreGuid<TUser>(col),
                col => new RoleStoreGuid<TRole>(col));
        }

        /// <summary>
        ///     This method only registers mongo stores, you also need to call AddIdentity.
        ///     Consider using AddIdentityWithMongoStores.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">Must contain the database name</param>
        public static IdentityBuilder RegisterMongoStores<TUser, TId, TRole>(
            this IdentityBuilder builder, 
            string connectionString, 
            Func<IMongoCollection<TUser>, UserStore<TUser, TId>> userStoreFactory,
            Func<IMongoCollection<TRole>, RoleStore<TRole, TId>> roleStoreFactory)
			where TRole : IdentityRole<TId>
			where TUser : IdentityUser<TId>
		{
			var url = new MongoUrl(connectionString);
			var client = new MongoClient(url);
			if (url.DatabaseName == null)
			{
				throw new ArgumentException("Your connection string must contain a database name", connectionString);
			}
			var database = client.GetDatabase(url.DatabaseName);
			return builder.RegisterMongoStores(
				p => database.GetCollection<TUser>("users"),
				p => database.GetCollection<TRole>("roles"),
                userStoreFactory,
                roleStoreFactory);
		}

		/// <summary>
		///     If you want control over creating the users and roles collections, use this overload.
		///     This method only registers mongo stores, you also need to call AddIdentity.
		/// </summary>
		/// <typeparam name="TUser"></typeparam>
		/// <typeparam name="TRole"></typeparam>
		/// <param name="builder"></param>
		/// <param name="usersCollectionFactory"></param>
		/// <param name="rolesCollectionFactory"></param>
		public static IdentityBuilder RegisterMongoStores<TUser, TId, TRole>(this IdentityBuilder builder,
			Func<IServiceProvider, IMongoCollection<TUser>> usersCollectionFactory,
			Func<IServiceProvider, IMongoCollection<TRole>> rolesCollectionFactory,
            Func<IMongoCollection<TUser>, UserStore<TUser, TId>> userStoreFactory,
            Func<IMongoCollection<TRole>, RoleStore<TRole, TId>> roleStoreFactory)
            where TRole : IdentityRole<TId>
			where TUser : IdentityUser<TId>
		{
			if (typeof(TUser) != builder.UserType)
			{
				var message = "User type passed to RegisterMongoStores must match user type passed to AddIdentity. "
				              + $"You passed {builder.UserType} to AddIdentity and {typeof(TUser)} to RegisterMongoStores, "
				              + "these do not match.";
				throw new ArgumentException(message);
			}
			if (typeof(TRole) != builder.RoleType)
			{
				var message = "Role type passed to RegisterMongoStores must match role type passed to AddIdentity. "
				              + $"You passed {builder.RoleType} to AddIdentity and {typeof(TRole)} to RegisterMongoStores, "
				              + "these do not match.";
				throw new ArgumentException(message);
			}
			builder.Services.AddSingleton<IUserStore<TUser>>(p => userStoreFactory(usersCollectionFactory(p)));
			builder.Services.AddSingleton<IRoleStore<TRole>>(p => roleStoreFactory(rolesCollectionFactory(p)));
			return builder;
		}

		///// <summary>
		/////     This method registers identity services and MongoDB stores using the IdentityUser and IdentityRole types.
		///// </summary>
		///// <param name="services"></param>
		///// <param name="connectionString">Connection string must contain the database name</param>
		//public static IdentityBuilder AddIdentityWithMongoStores(this IServiceCollection services, string connectionString)
		//{
		//	return services.AddIdentityWithMongoStoresUsingCustomTypes<IdentityUser, IdentityRole>(connectionString);
		//}

		/// <summary>
		///     This method allows you to customize the user and role type when registering identity services
		///     and MongoDB stores.
        ///     Where both User & Role Ids are ObjectIds
		/// </summary>
		/// <typeparam name="TUser"></typeparam>
		/// <typeparam name="TRole"></typeparam>
		/// <param name="services"></param>
		/// <param name="connectionString">Connection string must contain the database name</param>
		public static IdentityBuilder AddIdentityWithMongoStoresUsingCustomTypesWithObjectIds<TUser, TRole>(this IServiceCollection services, string connectionString)
			where TUser : IdentityUser<ObjectId>
			where TRole : IdentityRole<ObjectId>
		{
			return services.AddIdentity<TUser, TRole>()
				.RegisterMongoStores<TUser, ObjectId, TRole>(connectionString, col => new UserStoreObjectId<TUser>(col), col => new RoleStoreObjectId<TRole>(col));
		}

        /// <summary>
        ///     This method allows you to customize the user and role type when registering identity services
        ///     and MongoDB stores.
        ///     Where both User & Role Ids are Guids
        /// </summary>
        /// <typeparam name="TUser"></typeparam>
        /// <typeparam name="TRole"></typeparam>
        /// <param name="services"></param>
        /// <param name="connectionString">Connection string must contain the database name</param>
        public static IdentityBuilder AddIdentityWithMongoStoresUsingCustomTypesWithGuids<TUser, TRole>(this IServiceCollection services, string connectionString)
            where TUser : IdentityUser<Guid>
            where TRole : IdentityRole<Guid>
        {
            return services.AddIdentity<TUser, TRole>()
                .RegisterMongoStores<TUser, Guid, TRole>(connectionString, col => new UserStoreGuid<TUser>(col), col => new RoleStoreGuid<TRole>(col));
        }

        

        public static IdentityBuilder RegisterMongoStores(this IServiceCollection services, string connectionString)
        {
            return services.AddIdentity<IdentityUserObjectId, IdentityRoleObjectId>()
                .RegisterMongoStores<IdentityUserObjectId, ObjectId, IdentityRoleObjectId>(connectionString, col => new UserStoreObjectId<IdentityUserObjectId>(col), col => new RoleStoreObjectId<IdentityRoleObjectId>(col));
        }


        public static void DoReg()
        {
            IServiceCollection services = null;
            services.AddIdentity<IdentityUserGuid, IdentityRoleGuid>()
                .AddDefaultTokenProviders();

                services.RegisterMongoStores("");
        }

    }
}