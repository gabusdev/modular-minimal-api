using System;
using System.Collections.Generic;

namespace Modules.DataModule.Entities
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public string Name { get; set; } = null!;
        public int Id { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
