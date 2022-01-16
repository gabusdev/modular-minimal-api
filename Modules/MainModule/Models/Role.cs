using System;
using System.Collections.Generic;

namespace Modules.MainModule.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRols = new HashSet<UserRol>();
        }

        public string Name { get; set; } = null!;
        public int Id { get; set; }

        public virtual ICollection<UserRol> UserRols { get; set; }
    }
}
