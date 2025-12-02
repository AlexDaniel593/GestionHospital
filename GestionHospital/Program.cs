using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CapaDatos;
using CapaNegocio;
using DotNetEnv;

// Load environment variables from .env file
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "HospitalDB";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

var connectionString = $"Server={dbServer};Database={dbName};User ID={dbUser};Password={dbPassword};TrustServerCertificate=True;MultipleActiveResultSets=True;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddPasswordValidator<PasswordValidator<IdentityUser>>();

// Configure Argon2id password hasher
builder.Services.AddScoped<IPasswordHasher<IdentityUser>, Argon2PasswordHasher<IdentityUser>>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


builder.Services.AddControllersWithViews(); 
builder.Services.AddRazorPages();

// Register DAL and BL classes
builder.Services.AddScoped<SecurityDAL>();
builder.Services.AddScoped<PacienteDAL>();
builder.Services.AddScoped<PacienteBL>();
builder.Services.AddScoped<AccountBL>();
builder.Services.AddScoped<MedicoDAL>();
builder.Services.AddScoped<MedicoBL>();
builder.Services.AddScoped<CitaDAL>();
builder.Services.AddScoped<CitaBL>();
builder.Services.AddScoped<TratamientoDAL>();
builder.Services.AddScoped<TratamientoBL>();
builder.Services.AddScoped<FacturacionDAL>();
builder.Services.AddScoped<FacturacionBL>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Doctor", "Patient", "Staff"};
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

app.Run();
