using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Raiz.Data;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
=======
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
    
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
builder.Services.AddControllersWithViews();

var app = builder.Build();

<<<<<<< HEAD
// Configure the HTTP request pipeline.
=======
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
<<<<<<< HEAD
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
=======
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

<<<<<<< HEAD
=======
app.UseAuthentication();
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

<<<<<<< HEAD
app.Run();
=======
app.Run();
>>>>>>> e81c0a1 (refactor: refatoração geral dos controllers de Produto e Movimentação)
