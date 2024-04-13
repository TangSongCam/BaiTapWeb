using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaiTapWeb.Data;
using BaiTapWeb.Migrations;
using BaiTapWeb.Model;
using Microsoft.EntityFrameworkCore;

namespace BaiTapWeb.Services
{
    public class LibraryService : ILibraryService
    {
        private readonly AppDbContext _db;

        public LibraryService(AppDbContext db)
        {
            _db = db;
        }

        #region Students

        public async Task<List<Students>> GetStudentAsync()
        {
            try
            {

                return await _db.Students.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Students> GetStudentsAsync(int id, bool includeStudents)
        {
            try
            {
                if (includeStudents) // 
                {
                    return await _db.Students.Include(b => b.StudentCourses).FirstOrDefaultAsync(i => i.StudentId == id);
                }

                return await _db.Students.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<Students> AddStudentsAsync(Students students)
        {
            try
            {
                await _db.Students.AddAsync(students);
                await _db.SaveChangesAsync();
                return await _db.Students.FindAsync(students.StudentId);
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public async Task<Students> UpdateStudentAsync(Students students)
        {
            try
            {
                _db.Entry(students).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return students;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteStudentAsync(Students students)
        {
            try
            {
                var dbAuthor = await _db.Students.FindAsync(students.StudentId);

                if (dbAuthor == null)
                {
                    return (false, "Author could not be found");
                }

                _db.Students.Remove(students);
                await _db.SaveChangesAsync();

                return (true, "Author got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        #endregion Students

        #region Courses

        public async Task<List<Courses>> GetCoursesAsync()
        {
            try
            {
                return await _db.Courses.OrderBy(c => c.CourseName).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Courses> GetCoursesAsync(int id)
        {
            try
            {
                return await _db.Courses.FindAsync(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Courses> AddCoursesAsync(Courses courses)
        {
            try
            {
                await _db.Courses.AddAsync(courses);
                await _db.SaveChangesAsync();
                /* return await _db.Courses.FindAsync(courses.CourseId);*/ // Auto ID from DB
                return courses;
            }
            catch (Exception ex)
            {
                return null; // An error occured
            }
        }

        public async Task<Courses> UpdateCoursesAsync(Courses courses)
        {
            try
            {
                _db.Entry(courses).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                return courses;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<(bool, string)> DeleteCoursesAsync(Courses courses)
        {
            try
            {


                var dbStudent = await _db.Courses.FindAsync(courses.CourseId);

                if (dbStudent == null)
                {
                    return (false, "Student could not be found.");
                }

                _db.Courses.Remove(courses);
                await _db.SaveChangesAsync();

                return (true, "Student got deleted.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occured. Error Message: {ex.Message}");
            }
        }

        public object AddStudentsAsync(AddStudents addStudentRequest)
        {
            throw new NotImplementedException();
        }

        #endregion Courses
    }
}
