﻿namespace IntegrationTests
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
    using CoreTests;
    using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.MongoDB;
	using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Bson;
    using NUnit.Framework;

	// todo low - validate all tests work
	[TestFixture]
	public class UserLockoutStoreTests : UserIntegrationTestsBase
	{
		[Test]
		public async Task AccessFailed_IncrementsAccessFailedCount()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new IdentityUserObjectId { UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			Expect(await manager.GetAccessFailedCountAsync(user), Is.EqualTo(1));
		}

		private UserManager<IdentityUserObjectId> GetUserManagerWithThreeMaxAccessAttempts()
		{
			return CreateServiceProvider<IdentityUserObjectId, IdentityRoleObjectId>(options => options.Lockout.MaxFailedAccessAttempts = 3)
				.GetService<UserManager<IdentityUserObjectId>>();
		}

		[Test]
		public void IncrementAccessFailedCount_ReturnsNewCount()
		{
			var store = new UserStoreObjectId<IdentityUserObjectId>(null);
			var user = new IdentityUserObjectId { UserName = "bob"};

			var count = store.IncrementAccessFailedCountAsync(user, default(CancellationToken));

			Expect(count.Result, Is.EqualTo(1));
		}

		[Test]
		public async Task ResetAccessFailed_AfterAnAccessFailed_SetsToZero()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new IdentityUserObjectId { UserName = "bob"};
			await manager.CreateAsync(user);
			await manager.AccessFailedAsync(user);

			await manager.ResetAccessFailedCountAsync(user);

			Expect(await manager.GetAccessFailedCountAsync(user), Is.EqualTo(0));
		}

		[Test]
		public async Task AccessFailed_NotOverMaxFailures_NoLockoutEndDate()
		{
			var manager = GetUserManagerWithThreeMaxAccessAttempts();
			var user = new IdentityUserObjectId { UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			Expect(await manager.GetLockoutEndDateAsync(user), Is.Null);
		}

		[Test]
		public async Task AccessFailed_ExceedsMaxFailedAccessAttempts_LocksAccount()
		{
			var manager = CreateServiceProvider<IdentityUserObjectId, IdentityRoleObjectId>(options =>
				{
					options.Lockout.MaxFailedAccessAttempts = 0;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(1);
				})
				.GetService<UserManager<IdentityUserObjectId>>();

			var user = new IdentityUserObjectId { UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.AccessFailedAsync(user);

			var lockoutEndDate = await manager.GetLockoutEndDateAsync(user);
			Expect(lockoutEndDate?.Subtract(DateTime.UtcNow).TotalHours, Is.GreaterThan(0.9).And.LessThan(1.1));
		}

		[Test]
		public async Task SetLockoutEnabled()
		{
			var manager = GetUserManager();
			var user = new IdentityUserObjectId { UserName = "bob"};
			await manager.CreateAsync(user);

			await manager.SetLockoutEnabledAsync(user, true);
			Expect(await manager.GetLockoutEnabledAsync(user));

			await manager.SetLockoutEnabledAsync(user, false);
			Expect(await manager.GetLockoutEnabledAsync(user), Is.False);
		}
	}
}