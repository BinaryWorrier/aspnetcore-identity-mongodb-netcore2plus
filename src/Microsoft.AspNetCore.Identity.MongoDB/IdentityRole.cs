namespace Microsoft.AspNetCore.Identity.MongoDB
{
	using global::MongoDB.Bson;
	using global::MongoDB.Bson.Serialization.Attributes;

    public class IdentityRole<TId>: IdentityRoleBase
    {
        public IdentityRole(TId id)
        {
            Id = id;
        }
        public IdentityRole(string roleName, TId id) : this(id)
        {
            Name = roleName;
        }
        public TId Id { get; set; }

    }
    public class IdentityRoleBase
	{
		public IdentityRoleBase()
		{
		}

		public string Name { get; set; }

		public string NormalizedName { get; set; }

		public override string ToString() => Name;
	}
}