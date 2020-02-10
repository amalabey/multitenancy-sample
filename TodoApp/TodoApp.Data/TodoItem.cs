namespace TodoApp.Data
{
    public class TodoItem
    {
        public int Id { get; set; }

        public bool IsComplete { get; set; }

        public string Description { get; set; }

        public string Assignee { get; set; }
    }
}
