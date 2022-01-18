using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Modules.DataModule.Entities
{
    public partial class User
    {
        public User()
        {
            Todos = new HashSet<Todo>();
            Roles = new HashSet<Role>();
        }

        public string Id { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public string Pass { get; set; } = null!;

        public virtual ICollection<Todo> Todos { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
