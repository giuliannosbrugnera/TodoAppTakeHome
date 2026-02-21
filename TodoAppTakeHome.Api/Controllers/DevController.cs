using Microsoft.AspNetCore.Mvc;
using TodoAppTakeHome.Api.Data;

namespace TodoAppTakeHome.Api.Controllers
{
#if DEBUG
    [ApiController]
    [Route("api/dev")]
    public class DevController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        /// <summary>
        /// Clears all tasks from the database. Only available in Development builds.
        /// </summary>
        [HttpPost("reset")]
        public async Task<IActionResult> ResetDatabase()
        {
            _context.Tasks.RemoveRange(_context.Tasks);
            await _context.SaveChangesAsync();
            return Ok("Database cleared");
        }
    }
#endif
}
