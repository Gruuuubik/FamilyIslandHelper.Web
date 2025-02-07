using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Compare/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Compare}/{action=Index}/{id?}");

app.Run();
