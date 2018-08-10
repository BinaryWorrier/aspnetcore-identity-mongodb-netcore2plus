using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class IdentityRoleObjectId : IdentityRole<ObjectId>
    {
        public IdentityRoleObjectId() 
            : base(ObjectId.GenerateNewId())
        {
        }
        public IdentityRoleObjectId(string roleName)
            : base(roleName, ObjectId.GenerateNewId())
        {
        }
    }
}
