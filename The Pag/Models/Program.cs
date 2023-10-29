using The_Pag.Models;
using Microsoft.EntityFrameworkCore;
using The_Pag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();  // Session state memory
builder.Services.AddSession();      // Enable session state
builder.Services.AddControllersWithViews(); // Add services to container
builder.Services.AddRazorPages();   // Enables razer pages
builder.Services.AddDbContext<StoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbContext"))); // Database config
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
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Edit_Item}/{id?}",
    constraints: new {action= "Edit_Item|Edit_User|Item_Management|User_Management|Delete_Item|Add_Item" }
);

app.MapControllerRoute(
    name: "admin",
    pattern: "{controller=Admin}/{action=Edit_Item_Action}/{input}",
    constraints: new { action = "Edit_Item_Action|Add_Item_Action" }
);

app.MapControllerRoute(
    name: "catalogue",
    pattern: "{controller=Catalogue}/{action=Cart}/{productType?}/{genre?}/{sortBy?}/{order?}",
    constraints: new {action="Cart|Catalogue"}
);

app.MapControllerRoute(
    name: "catalogue",
    pattern: "{controller=Catalogue}/{action=Item}/{ID?}",
    constraints: new { action = "Item" }
);

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=Login}/{id?}",
    constraints: new {action="Account|Account_Create|Login|Order_Details|Order_History|Update_Order_Details" }
);

app.MapControllerRoute(
    name: "user",
    pattern: "{controller=User}/{action=Login_Action}/{username}/{password}",
    constraints: new { action = "Login_Action" }
);

app.MapRazorPages();

app.Run();
