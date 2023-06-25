using Microsoft.EntityFrameworkCore;

namespace App.Models;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options) { }

    public DbSet<Funcionario> Funcionarios { get; set; } = null!;
}
