using System.Net.Http.Headers;

using Chirp.Infrastructure.Models;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpDBContext : DbContext
{
    public DbSet<Cheep>? Cheeps { get; set; }
    public DbSet<Author>? Authors { get; set; }

    public string DbPath { get; }

    public ChirpDBContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        string fullPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        string[] paths = new string[] { fullPath, "src", "Chirp.Infrastructure", "data", "chirp.db" };

        Console.WriteLine("ulul");

        Console.WriteLine(Path.Combine(paths));

        options.UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), "chirp.db")}");
    }
}