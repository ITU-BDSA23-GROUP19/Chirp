namespace Chirp.Web;

public class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddScoped<ICheepRepository, CheepRepository>();
        builder.Services.AddDbContext<ChirpContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("Chirp")));

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

        app.MapRazorPages();

        app.Run();
    }
}