using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myshop.DataAccess.Data;
using myshop.DataAccess.Repository;
using myshop.DataAccess.Repository.IRepository;
using myshop.Entities;
using myshop.Utilities;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//1-Add this services to help me make refresh without debug more.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//register add pass connetion string
builder.Services.AddDbContext<ApplictionDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//AddStripe
builder.Services.Configure<StripeData>(builder.Configuration.GetSection("Stripe"));

//service Repository
builder.Services.AddScoped<IUnitofWork,UnitofWork>();
//1- ROles: to add role change to AddIdentity<IdentityUser,IdentityRole>
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
    option=>option.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromHours(4))
    .AddEntityFrameworkStores<ApplictionDbContext>()
   .AddDefaultTokenProviders().AddDefaultUI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
//Add this middleware after route (APIKey==SecretKey) 
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Secretkey").Get<string>();
app.UseAuthorization();
app.MapRazorPages(); //add this 3shan appears pages Register w login .....
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Admin}/{controller=Category}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "Customer",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
