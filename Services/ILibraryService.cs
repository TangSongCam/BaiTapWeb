using System.Collections.Generic;
using System.Threading.Tasks;
using BaiTapWeb.Migrations;
using BaiTapWeb.Model;

namespace BaiTapWeb.Services
{
    public interface ILibraryService
    {
        // Students Services
        Task<List<Students>> GetStudentAsync(); 
        Task<Students> GetStudentsAsync(int id, bool includeBooks = false); 
        Task<Students> AddStudentsAsync(Students students);
        Task<Students> UpdateStudentAsync(Students students); 
        Task<(bool, string)> DeleteStudentAsync(Students students); 

        // Courses Services
        Task<List<Courses>> GetCoursesAsync();
        Task<Courses> GetCoursesAsync(int id); 
        Task<Courses> AddCoursesAsync(Courses courses); 
        Task<Courses> UpdateCoursesAsync(Courses courses); 
        Task<(bool, string)> DeleteCoursesAsync(Courses courses);
        object AddStudentsAsync(AddStudents addStudentRequest);
    }
}
