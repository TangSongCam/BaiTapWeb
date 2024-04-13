using BaiTapWeb.Data;
using BaiTapWeb.Migrations;
using BaiTapWeb.Model;
using BaiTapWeb.Services;
using Microsoft.AspNetCore.Mvc;
using BaiTapWeb.CustomActionFilter;

namespace BaiTapWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentCourse _context;
        private readonly AppDbContext _dbContext;
        private readonly ILogger<StudentsController> _logger;

        private readonly ILibraryService _libraryService;

        public StudentsController(AppDbContext dbContext, ILibraryService libraryService, ILogger<StudentsController> logger)
        {
            _dbContext = dbContext;
            _libraryService = libraryService;
            _logger = logger;

        }

        [HttpPost("add-employee")]
        [ValidateModel]
        public IActionResult AddEmployee([FromBody] AddCourses addCourses)
        {
            if (ModelState.IsValid)
            {
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet("Get-All-Student")]
        public async Task<IActionResult> GetAll([FromQuery] string? sortby, [FromQuery] bool isacsending,
        [FromQuery] string? filteron, [FromQuery] string? filterquery,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            try
            {
                _logger.LogInformation("Get All Student Action Method Was Invoked");
                var student = await _libraryService.GetStudentAsync();
                if (student == null)
                {
                    _logger.LogWarning("No student data");
                    return StatusCode(StatusCodes.Status204NoContent, "No student in database");
                }
                _logger.LogInformation("Successfully fetched students data.");
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching students data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error fetching students data");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetStudents([FromQuery] string? sortby, [FromQuery] bool isacsending,
        [FromQuery] string? filteron, [FromQuery] string? filterquery,
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            var students = await _libraryService.GetStudentAsync();

            if (students == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, "No student in database");
            }

            // filterring
            if (string.IsNullOrWhiteSpace(filteron) == false && string.IsNullOrWhiteSpace(filterquery) == false)
            {
                if (!string.IsNullOrWhiteSpace(filteron) && filteron.Equals("name", StringComparison.OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(filterquery))
                {
                    students = students.Where(x => x.Name.Contains(filterquery, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }
            // sortby
            if (!string.IsNullOrEmpty(sortby))
            {
                switch (sortby.ToLower())
                {
                    case "name":
                        {
                            students = isacsending ? students.OrderBy(s => s.Name).ToList() : students.OrderByDescending(s => s.Name).ToList();
                            break;
                        }
                    case "id":
                        {
                            students = isacsending ? students.OrderBy(s => s.StudentId).ToList() : students.OrderByDescending(s => s.StudentId).ToList();
                            break;
                        }

                }
            }
            var totalCount = students.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagination = students.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var student = pagination.Select(s => new
            {
                student = s,

            }
            ).ToList();

            return StatusCode(StatusCodes.Status200OK, new
            {
                s = student,
                tc = totalCount,
                tp = totalPages,
                pz = pageSize,
                pn = pageNumber
            }
            );
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetStudent(int id, bool includeBooks = false)
        {
            Students students = await _libraryService.GetStudentsAsync(id, includeBooks);

            if (students == null)
            {
                return StatusCode(StatusCodes.Status204NoContent, $"No Author found for id: {id}");
            }

            return StatusCode(StatusCodes.Status200OK, students);
        }

        [HttpPost]
        public async Task<ActionResult<Students>> AddStudents(Students students)
        {
            var dbStudents = await _libraryService.AddStudentsAsync(students);

            if (dbStudents == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{students.Name} could not be added.");
            }

            return CreatedAtAction("GetStudent", new { id = students.StudentId }, students);
        }

        [HttpPost("add-student")]
        public IActionResult AddStudent([FromBody] AddStudents addStudentRequest)
        {
            if (ModelState.IsValid)
            {
                var result = _libraryService.AddStudentsAsync(addStudentRequest);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                {
                    return StatusCode(500, "Internal server error.");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut("id")]
        public async Task<IActionResult> UpdateStudents(int id, Students students)
        {
            if (id != students.StudentId)
            {
                return BadRequest();
            }

            Students dbstudents = await _libraryService.UpdateStudentAsync(students);

            if (dbstudents == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{students.Name} could not be updated");
            }

            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteStudents(int id)
        {
            var student = await _libraryService.GetStudentsAsync(id, false);
            (bool status, string message) = await _libraryService.DeleteStudentAsync(student);

            if (status == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }

            return StatusCode(StatusCodes.Status200OK, student);
        }


    }

}