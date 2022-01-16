using System;
using System.Collections.Generic;

namespace Modules.MainModule.Models
{
    public partial class User
    {
        public User()
        {
            Todos = new HashSet<Todo>();
            UserRols = new HashSet<UserRol>();
        }

        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Pass { get; set; } = null!;

        public virtual ICollection<Todo> Todos { get; set; }
        public virtual ICollection<UserRol> UserRols { get; set; }
    }
}
