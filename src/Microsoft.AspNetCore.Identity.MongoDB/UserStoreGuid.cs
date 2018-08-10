using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class UserStoreGuid<TUser> : UserStore<TUser, Guid>
        where TUser : IdentityUser<Guid>
    {
        public UserStoreGuid(IMongoCollection<TUser> users)
            : base(users)
        {

        }
        public override bool TryParseId(string stringId, out Guid id)
        {
            return Guid.TryParse(stringId, out id);
        }
    }
}
