using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

using System.Security.Claims;

namespace Chirp.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
            builder.Services.AddDbContext<ChirpContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQLCONNSTR_AZUREDB")));
        }
        else
        {
            builder.Services.AddDbContext<ChirpContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("SQLCONNSTR_AZUREDB")));
        }

        builder.Services.AddRazorPages().AddMicrosoftIdentityUI();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddScoped<IFollowRepository, FollowRepository>();
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

        WebApplication app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        using (IServiceScope scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            ChirpContext context = services.GetRequiredService<ChirpContext>();
            DbInitializer.SeedDatabase(context);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapRazorPages();
        app.MapControllers();

        app.Run();
    }
}