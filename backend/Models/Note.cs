namespace backend.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    // What the client sends to CREATE a note
    public class CreateNoteDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
    }

    // What the client sends to UPDATE a note
    public class UpdateNoteDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
    }
}