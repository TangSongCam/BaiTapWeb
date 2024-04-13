    using System.ComponentModel.DataAnnotations;

    namespace BaiTapWeb.Model
    {
        public class Students
        {
            [Key]
            public int StudentId { get; set; }
            [Required]
            [StringLength(50, MinimumLength = 5)]
            public string? Name { get; set; }
            public ICollection<StudentCourse>? StudentCourses { get; set; }

        }
    }
