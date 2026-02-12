using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Assignment1.Models
{
    public interface IAuditService
    {
        Task LogAsync(
            string action,
            string entity = null,
            string entityId = null,
            object oldValues = null,
            object newValues = null);
    }
    public class AuditService : IAuditService
    {
        private readonly AuthDbContext _db;
        private readonly IHttpContextAccessor _http;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuditService(
            AuthDbContext db,
            IHttpContextAccessor http,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _http = http;
            _userManager = userManager;
        }

            public AuthDbContext Db => _db;

            public IHttpContextAccessor Http => _http;

            public UserManager<ApplicationUser> UserManager => _userManager;

            public async Task LogAsync(
            string action,
            string entity = null,
            string entityId = null,
            object oldValues = null,
            object newValues = null)
        {
            var user = Http.HttpContext?.User;
            var userId = UserManager.GetUserId(user);
            var userName = user?.Identity?.Name;

            var log = new AuditLog
            {
                UserId = userId,
                UserName = userName,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                OldValues = oldValues != null ? JsonSerializer.Serialize(oldValues) : null,
                NewValues = newValues != null ? JsonSerializer.Serialize(newValues) : null,
                IpAddress = Http.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Http.HttpContext?.Request.Headers["User-Agent"]
            };

            Db.AuditLogs.Add(log);
            await Db.SaveChangesAsync();
        }
    }
}
