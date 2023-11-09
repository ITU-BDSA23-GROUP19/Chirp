using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Chirp.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        var connection = String.Empty;
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
            connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        }
        else
        {
            connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
        }

        builder.Services.AddRazorPages()
            .AddMicrosoftIdentityUI();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
        builder.Services.AddDbContext<ChirpContext>(options =>
            options.UseSqlServer(connection));
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));

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
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseRouting();
        app.MapRazorPages();
        app.MapControllers();

        app.Run();
    }
}