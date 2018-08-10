using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class RoleStoreObjectId<TRole> : RoleStore<TRole, ObjectId>
        where TRole : IdentityRole<ObjectId>
    {
        public RoleStoreObjectId(IMongoCollection<TRole> roles)
            : base(roles)
        {

        }
        public override bool TryParseId(string stringId, out ObjectId id)
        {
            return ObjectId.TryParse(stringId, out id);
        }
    }
}
