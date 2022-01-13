
using Microsoft.EntityFrameworkCore;
using MobilePayAPI.Entities;

namespace MobilePayAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Merchant> Merchants { get; set; }
    }
}