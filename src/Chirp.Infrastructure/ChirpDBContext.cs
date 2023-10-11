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

        string filePath = Path.GetFullPath(".");
        string fileName = Path.GetFileName(filePath);

        while(!fileName.Equals("Chirp")){
            filePath = Path.GetDirectoryName(filePath);
            fileName = Path.GetFileName(filePath);
        }
       
        string[] paths = new string[] { filePath, "src", "Chirp.Infrastructure", "data", "chirp.db" };

        Console.WriteLine(Path.Combine(paths));

        options.UseSqlite($"Data Source={Path.Combine(paths)}");
        //options.UseSqlite($"Data Source={Path.Combine(Path.GetTempPath(), "chirp.db")}");
    }
}