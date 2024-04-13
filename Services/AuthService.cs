using BaiTapWeb.Data;
using BaiTapWeb.Model;
using BaiTapWeb.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    public async Task<User> Authenticate(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        return user;
    }
}
