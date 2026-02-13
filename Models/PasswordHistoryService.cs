using Microsoft.AspNetCore.Identity;
using static System.Net.WebRequestMethods;

namespace Assignment1.Models
{
    public interface IPasswordHistoryService
    {
        Task LogAsync();
    }
    public class PasswordHistoryService : IPasswordHistoryService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _db;
        private readonly IHttpContextAccessor _http;
        public PasswordHistoryService(UserManager<ApplicationUser> userManager, AuthDbContext db, IHttpContextAccessor http)
        {
            _userManager = userManager;
            _db = db;
            _http = http;
        }
        public AuthDbContext Db => _db;
        public IHttpContextAccessor Http => _http;
        public UserManager<ApplicationUser> UserManager => _userManager;
        public async Task LogAsync()
        {
            var user = UserManager.GetUserAsync(Http.HttpContext.User).Result;

            Db.PasswordHistory.Add(new UserPasswordHistory
            {
                UserId = user.Id,
                PasswordHash = user.PasswordHash,
                ChangedAt = DateTime.UtcNow
            });
            await Db.SaveChangesAsync();
        }
    }
}
