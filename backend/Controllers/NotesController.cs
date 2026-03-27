using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private const string UserHeader = "X-User-Name";
        private readonly INoteRepository _repo;

        public NotesController(INoteRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!TryGetUserName(out var userName, out var error))
                return BadRequest(error);

            var notes = await _repo.GetAll(userName!);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!TryGetUserName(out var userName, out var error))
                return BadRequest(error);

            var note = await _repo.GetById(id, userName!);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
        {
            if (!TryGetUserName(out var userName, out var error))
                return BadRequest(error);

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Title is required");

            var note = await _repo.Create(dto, userName!);
            return CreatedAtAction(nameof(GetById), new { id = note.Id }, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteDto dto)
        {
            if (!TryGetUserName(out var userName, out var error))
                return BadRequest(error);

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Title is required");

            var note = await _repo.Update(id, dto, userName!);
            if (note == null) return NotFound();
            return Ok(note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetUserName(out var userName, out var error))
                return BadRequest(error);

            var deleted = await _repo.Delete(id, userName!);
            if (!deleted) return NotFound();
            return NoContent();
        }

        private bool TryGetUserName(out string? userName, out string? error)
        {
            if (!Request.Headers.TryGetValue(UserHeader, out var headerValue))
            {
                userName = null;
                error = $"Missing {UserHeader} header";
                return false;
            }

            var normalized = headerValue.ToString().Trim();
            if (string.IsNullOrWhiteSpace(normalized))
            {
                userName = null;
                error = $"{UserHeader} cannot be empty";
                return false;
            }

            userName = normalized;
            error = null;
            return true;
        }
    }
}