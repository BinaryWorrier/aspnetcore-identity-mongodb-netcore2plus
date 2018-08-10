using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class RoleStoreGuid<TRole> : RoleStore<TRole, Guid>
        where TRole: IdentityRole<Guid>
    {
        public RoleStoreGuid(IMongoCollection<TRole> roles)
            : base(roles)
        {

        }
        public override bool TryParseId(string stringId, out Guid id)
        {
            return Guid.TryParse(stringId, out id);
        }
    }
}
