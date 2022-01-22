namespace Modules.MainModule.Models
{
    public class TodoDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public bool IsDone { get; set; }
    }
}