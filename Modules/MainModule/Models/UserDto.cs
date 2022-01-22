namespace Modules.MainModule.Models
{
    public class UserDto
    {
        public string Username { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public virtual ICollection<TodoDto> Todos { get; set; } = null!;
        // [JsonIgnore]
        public virtual ICollection<string> Roles { get; set; } = null!;
    }
}