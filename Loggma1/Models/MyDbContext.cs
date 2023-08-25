using Loggma1.Controllers;
using Microsoft.EntityFrameworkCore;
using Loggma1.Models;
namespace Loggma1;
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public DbSet<ClientInfo> Clients { get; set; }
    // Diğer DbSet'ler buraya eklenebilir.
}
