using Microsoft.EntityFrameworkCore;
using baitap.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add cookie configuration here, before app.Build()
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Auth/Login";
    options.LogoutPath = "/Admin/Auth/Logout";
    options.AccessDeniedPath = "/Admin/Auth/Login";
    options.Cookie.Name = ".AspNetCore.Identity.Application";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options => {
    options.AddPolicy("RequireAdminRole", policy => 
        policy.RequireRole("Admin"));
});

var app = builder.Build();

// Add database seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedRolesAndAdminAsync(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Updated middleware with proper logout handling
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();
    
    // Special handling for logout path
    if (path == "/admin/auth/logout")
    {
        // Let the logout handler process the request without interference
        await next();
        return;
    }
    
    var user = context.User;

    // Always allow access to static files and auth pages
    if (path == "/admin/auth/login" || 
        path.StartsWith("/lib/") || 
        path.StartsWith("/css/") || 
        path.StartsWith("/js/") || 
        path.StartsWith("/images/"))
    {
        await next();
        return;
    }

    // Handle admin area access
    if (path.StartsWith("/admin"))
    {
        if (!user.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Admin/Auth/Login");
            return;
        }
        
        if (!user.IsInRole("Admin"))
        {
            context.Response.Redirect("/Admin/Auth/Login");
            return;
        }
        
        await next();
        return;
    }
    
    // Prevent admin users from accessing non-admin pages
    if (user.Identity.IsAuthenticated && user.IsInRole("Admin") && !path.StartsWith("/admin"))
    {
        context.Response.Redirect("/Admin/Dashboard");
        return;
    }

    await next();
});

app.MapRazorPages();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Auth}/{action=Login}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
