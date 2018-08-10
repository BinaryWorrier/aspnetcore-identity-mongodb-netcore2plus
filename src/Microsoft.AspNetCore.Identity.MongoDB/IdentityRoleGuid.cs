using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class IdentityRoleGuid : IdentityRole<Guid>
    {
        public IdentityRoleGuid() 
            : base(Guid.NewGuid())
        {
        }
        public IdentityRoleGuid(string roleName)
            : base(roleName, Guid.NewGuid())
        {

        }
    }
}
