using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class IdentityUserObjectId: IdentityUser<ObjectId>
    {
        public IdentityUserObjectId()
            :base(ObjectId.GenerateNewId())
        {

        }
    }
}
