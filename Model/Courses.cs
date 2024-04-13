using System.ComponentModel.DataAnnotations;

namespace BaiTapWeb.Model
{
    public class Courses
    {
        [Key]
        public int CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public ICollection<StudentCourse>? StudentCourses { get; set; }

    }
}
