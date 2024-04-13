using BaiTapWeb.Data;
using BaiTapWeb.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BaiTapWeb.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(string username, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;

        public AuthService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            // Thực hiện xác thực người dùng ở đây, ví dụ sử dụng một service hoặc logic riêng
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            return user;
        }
    }
}
