using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MongoDB
{
    public class IdentityUserGuid : IdentityUser<Guid>
    {
        public IdentityUserGuid() 
            : base(Guid.NewGuid())
        {
        }
    }
}
