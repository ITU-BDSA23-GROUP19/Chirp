namespace Chirp.Web.Tests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            ServiceDescriptor? contextDb = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ChirpContext>));
            ServiceDescriptor? connectionDb = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));

            if (contextDb != null)
            {
                services.Remove(contextDb);
            }

            if (connectionDb != null)
            {
                services.Remove(connectionDb);
            }

            services.AddDbContext<ChirpContext>(options =>
            {
                options.UseInMemoryDatabase("DataSource=:memory:");
            });

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                ChirpContext context = serviceProvider.GetRequiredService<ChirpContext>();
                DbInitializer.SeedDatabase(context);
            }
        });

        builder.UseEnvironment("Development");
    }
}