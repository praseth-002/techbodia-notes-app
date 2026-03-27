using backend.Models;

namespace backend.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAll(string userName);
        Task<Note?> GetById(int id, string userName);
        Task<Note> Create(CreateNoteDto dto, string userName);
        Task<Note?> Update(int id, UpdateNoteDto dto, string userName);
        Task<bool> Delete(int id, string userName);
    }
}