using backend.Models;

namespace backend.Repositories
{
    public class InMemoryNoteRepository : INoteRepository
    {
        private readonly List<Note> _notes = [];
        private int _nextId = 1;

        public Task<IEnumerable<Note>> GetAll(string userName)
            => Task.FromResult<IEnumerable<Note>>(_notes
                .Where(n => n.UserName == userName)
                .OrderByDescending(n => n.CreatedAt));

        public Task<Note?> GetById(int id, string userName)
            => Task.FromResult(_notes.FirstOrDefault(n => n.Id == id && n.UserName == userName));

        public Task<Note> Create(CreateNoteDto dto, string userName)
        {
            var note = new Note
            {
                Id = _nextId++,
                UserName = userName,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _notes.Add(note);
            return Task.FromResult(note);
        }

        public Task<Note?> Update(int id, UpdateNoteDto dto, string userName)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id && n.UserName == userName);
            if (note == null)
            {
                return Task.FromResult<Note?>(null);
            }

            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;
            return Task.FromResult<Note?>(note);
        }

        public Task<bool> Delete(int id, string userName)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id && n.UserName == userName);
            if (note == null)
            {
                return Task.FromResult(false);
            }

            _notes.Remove(note);
            return Task.FromResult(true);
        }
    }
}
