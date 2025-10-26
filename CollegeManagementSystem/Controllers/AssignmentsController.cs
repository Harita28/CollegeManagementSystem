using CollegeManagementSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CollegeManagementSystem.Controllers
{
    [Authorize]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _env;

        public AssignmentsController(ApplicationDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Download(long id)
        {
            var assignment = _db.Assignments.Find(id);
            if (assignment == null || string.IsNullOrEmpty(assignment.AttachmentUrl)) return NotFound();

            var path = Path.Combine(_env.WebRootPath, assignment.AttachmentUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
            if (!System.IO.File.Exists(path)) return NotFound();

            var ext = Path.GetExtension(path);
            var contentType = ext switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
            return PhysicalFile(path, contentType, Path.GetFileName(path));
        }
    }
}
