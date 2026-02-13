using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Assignment1.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Assignment1.ViewModels;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    WebRootPath = "wwwroot"
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AuthDbContext>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    // Email must be unique
    options.User.RequireUniqueEmail = true;

    // Password settings
    options.Password.RequiredLength = 12;

    // Account lockout 
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    options.Lockout.AllowedForNewUsers = true;

    // Email Verification
    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.HttpOnly = true;
    config.ExpireTimeSpan = TimeSpan.FromMinutes(2);
    config.LogoutPath = "/logout";
    config.AccessDeniedPath = "/AccessDenied";
    config.SlidingExpiration = true;
    config.LoginPath = "/login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("UserType", "Admin"));
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuditService, AuditService>();

builder.Services.AddScoped<IPasswordHistoryService, PasswordHistoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}
else
{
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
