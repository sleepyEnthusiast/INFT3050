using The_Pag.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbContext")));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Edit_Item}/{id?}",
    constraints: new {action="Edit_Item|Edit_User|Item_Management|User_Management" }
);

app.MapControllerRoute(
    name: "catalogue",
    pattern: "{controller=Catalogue}/{action=Cart}/{id?}/{id2?}/{id3?}/{id4?}",
    constraints: new {action="Cart|Catalogue|Item"}
);

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=Login}/{id?}",
    constraints: new {action="Account|Account_Create|Login|Order_Details|Order_History|Update_Order_Details" }
);


app.MapRazorPages();

app.Run();
