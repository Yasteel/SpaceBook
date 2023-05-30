using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Spacebook;
using Spacebook.Data;
using Spacebook.Hubs;
using Spacebook.Interfaces;
using Spacebook.Models;
using Spacebook.Services;
using Spacebook.Validation;

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

        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<IConversationService,  ConversationService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<ILikeService, LikeService>();
        builder.Services.AddScoped<ICommentService, CommentService>();

        // Add services to the container.
        builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
		builder.Services.AddSignalR();


		builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Auth/Index";
            options.AccessDeniedPath = "/Auth/AccessDenied";
        });

        builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
        builder.Services.AddScoped<IPostService, PostService>();
        builder.Services.AddScoped<IHashTagService, HashTagService>();
        builder.Services.AddScoped<IProfileService, ProfileService>();
        builder.Services.AddScoped<ISharedPostService, SharedPostService>();

        builder.Services.AddScoped<IValidator<Post>, PostValidator>();
        builder.Services.AddScoped<ISearchFunctionalityService, SearchFunctionalityService>();
        builder.Services.AddHttpContextAccessor();

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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Add another route for the default page when there's no logged-in user
            endpoints.MapControllerRoute(
                name: "defaultNoUser",
                pattern: "{controller=Auth}/{action=Index}/{id?}")
                .RequireAuthorization(); // Restrict access to this route to authenticated users
        });

		app.MapHub<ConnectionHub>("/ConnectionHub");
        
        app.Run();
    }
}