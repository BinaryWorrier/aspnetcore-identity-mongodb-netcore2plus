using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class UserStoreObjectId<TUser> : UserStore<TUser, ObjectId>
        where TUser: IdentityUser<ObjectId>
    {
        public UserStoreObjectId(IMongoCollection<TUser> users)
            :base(users)
        {

        }
        public override bool TryParseId(string stringId, out ObjectId id)
        {
            return ObjectId.TryParse(stringId, out id);
        }
    }
}
