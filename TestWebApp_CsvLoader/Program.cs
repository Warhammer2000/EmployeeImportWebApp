using EmployeeImportApp.Db;
using EmployeeImportApp.Middleware;
using EmployeeImportApp.Repository;
using EmployeeImportApp.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Net;

namespace EmployeeImportApp
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting up the application");

                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog();

                builder.Services.AddControllersWithViews();

                builder.Services.AddDbContext<EmployeeDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeDbContext")));

                builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
                builder.Services.AddScoped<EmployeeService>();

                var app = builder.Build();

                app.UseMiddleware<ErrorHandlingMiddleware>();

                app.UseStatusCodePages(context =>
                {
                    var response = context.HttpContext.Response;

                    if (response.StatusCode == (int)HttpStatusCode.NotFound)
                    {
                        context.HttpContext.Response.Redirect("/Error/Error404");
                    }
                    else if (response.StatusCode == (int)HttpStatusCode.InternalServerError)
                    {
                        context.HttpContext.Response.Redirect("/Error/Error500");
                    }
                    else if (response.StatusCode == (int)HttpStatusCode.Forbidden)
                    {
                        context.HttpContext.Response.Redirect("/Error/Error403");
                    }
                    else
                    {
                        context.HttpContext.Response.Redirect($"/Error/Error?statusCode={response.StatusCode}");
                    }

                    return Task.CompletedTask;
                });

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

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
