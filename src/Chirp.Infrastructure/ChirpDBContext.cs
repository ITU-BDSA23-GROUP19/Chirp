using System.Net.Http.Headers;

using Chirp.Infrastructure.Models;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep>? Cheeps { get; set; }
    public DbSet<Author>? Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        string filePath = Path.GetFullPath(".");
        string fileName = Path.GetFileName(filePath);

        while (!fileName.Equals("Chirp"))
        {
            filePath = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileName(filePath);
        }

        string[] paths = new string[] { filePath, "src", "Chirp.Infrastructure", "data", "chirp.db" };

        options.UseSqlite($"Data Source={Path.Combine(paths)}");
    }
}