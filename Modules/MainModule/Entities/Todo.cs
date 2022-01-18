using System;
using System.Collections.Generic;

namespace Modules.MainModule.Entities
{
    public partial class Todo
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsDone { get; set; }
        public string UserId { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
