using System;
using System.Collections.Generic;

namespace Modules.MainModule.Models
{
    public partial class UserRol
    {
        public string UserId { get; set; } = null!;
        public int RolesId { get; set; }
        public int? Id { get; set; }

        public virtual Role Roles { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
