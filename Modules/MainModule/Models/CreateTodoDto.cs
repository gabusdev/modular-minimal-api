namespace Modules.MainModule.Models
{
    public class CreateTodoDto
    {
        public string Name { get; set; } = null!;
        public bool IsDone { get; set; }
    }
}