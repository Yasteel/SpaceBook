using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Spacebook;
using Spacebook.Data;
using Spacebook.Interfaces;
using Spacebook.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        var configuration = configBuilder.Build();

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default")));
        builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AuthDbContextConnection")));

        builder.Services.AddDefaultIdentity<SpacebookUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<AuthDbContext>();

       

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Index";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });

        builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IHashTagService, HashTagService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseAuthentication(); ;

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapRazorPages();

        app.Run();
    }
}