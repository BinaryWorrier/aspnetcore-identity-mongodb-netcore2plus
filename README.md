## Microsoft.AspNetCore.Identity.MongoDB

This is a MongoDB provider for the ASP.NET Core 2 Identity framework. This was ported from the v2 Identity framework that was a part of ASP.NET (AspNet.Identity.Mongo NuGet package)

I've forked this from it's source repo, because the ids were strings on the user and role entities, which didn't suit my usecase.
This library - out of the box - supports entities with ObjectIds or Guids as ids.
You can - with a couple of lines of code - write your own derived User & Role stores with what ever typed Id floats your boat.

## Usage

- Reference this package in project.json: Microsoft.AspNetCore.Identity.MongoDB
- Then, in ConfigureServices--or wherever you are registering services--include the following to register both the Identity services and MongoDB stores:

```csharp
/// Assumes you want ObjectIds as the User & Role Id
services.AddIdentityWithMongoStores("mongodb://localhost/myDB");
```

- If you want to customize what is registered, refer to the tests for further options (CoreTests/MongoIdentityBuilderExtensionsTests.cs)
- Remember with the Identity framework, the whole point is that both a `UserManager` and `RoleManager` are provided for you to use, here's how you can resolve instances manually. Of course, constructor injection is also available.

```csharp
var userManager = provider.GetService<UserManager<IdentityUserObjectId>>();
var roleManager = provider.GetService<RoleManager<IdentityRoleObjectId>>();
```

or depending on your Id type

```csharp
var userManager = provider.GetService<UserManager<IdentityUserGuid>>();
var roleManager = provider.GetService<RoleManager<IdentityRoleGuid>>();
```

- The following methods help create indexes that will boost lookups by UserName, Email and role Name. These have changed since Identity v2 to refer to Normalized fields. I dislike this aspect of Core Identity, but it is what it is. Basically these three fields are stored in uppercase format for case insensitive searches.

```csharp
	IndexChecks.EnsureUniqueIndexOnNormalizedUserName(users);
	IndexChecks.EnsureUniqueIndexOnNormalizedEmail(users);
	IndexChecks.EnsureUniqueIndexOnNormalizedRoleName(roles);
```

## Rolling with your own Id type

You want your Ids to be `DateTime`, I get it, `DateTime` is your thing

Firstly specialise IdentityUser & IdentityRole for DateTime

```csharp
public class IdentityUserDateTime : IdentityUser<DateTime>
{
    public IdentityUserGuid() : base(DateTime.Now) {}
}

public class IdentityRoleDateTime : IdentityRole<DateTime>
{
    public IdentityUserRole() : base(DateTime.Now) {}
}
```

Then specialise UserStore and RoleStore to use these types

```csharp
public class UserStoreDateTime<TUser> : UserStore<TRole, DateTime>
    where TUser: IdentityRole<DateTime>
{
    public UserStoreDateTime(IMongoCollection<TUser> users)
        : base(users){ }
    public override bool TryParseId(string stringId, out Guid id)
        => DateTime.TryParse(stringId, out id);
}
public class RoleStoreDateTime<TRole> : RoleStore<TRole, DateTime>
    where TRole: IdentityRole<DateTime>
{
    public RoleStoreDateTime(IMongoCollection<TRole> roles)
        : base(roles) { }
    public override bool TryParseId(string stringId, out Guid id)
        => DateTime.TryParse(stringId, out id);
}
```

### Extending IdentityUser and/or IdentityRole with your own properties

Simply create your entity and inherit from ours e.g.
```csharp
public class ApplicationUser: IdentityUserGuid
{
    public string FamilyName {get;set;}
}

/// and register it 
services.AddIdentityWithMongoStoresUsingCustomTypesWithGuids<ApplicationUser, IdentityRoleGuid>(connectionString);
```


## Frameworks 
What frameworks are targeted, with rationale:

- Microsoft.AspNetCore.Identity - supports net461 and netstandard2.0
- MongoDB.Driver v2.3 - supports net45 and netstandard1.5
- Thus, the lowest common denominators are net461 (of net46 and net461) and netstandard2.0 
- FYI net461 supports netstandard1.5, that's obviously too low for a single target

## Building instructions

run [build.bat](./build.bat)

## Migrating from ASP.NET Identity 2.0

- Roles names need to be normalized as follows
	- On IdentityRole documents, create a NormalizedName field = uppercase(Name). Leave Name as is.
	- On IdentityUser documents, convert the values in the Roles array to uppercase
- User names need to be normalized as follows
	- On IdentityUser documents, create a NormalizedUserName field = uppercase(UserName) and create a NormalizedEmail field = uppercase(Email). Leave UserName and Email as is.

# NuGet
https://www.nuget.org/packages/BinaryWorrier.Microsoft.AspNetCore.Identity.MongoDB/
