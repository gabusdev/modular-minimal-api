namespace Modules.MainModule.Models
{
    public class UserViewModel
    {
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public virtual ICollection<TodoViewModel> Todos { get; set; } = null!;
        // [JsonIgnore]
        public virtual ICollection<RoleViewModel> Roles { get; set; } = null!;
    }
}