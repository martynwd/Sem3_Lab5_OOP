using System.Text;
using System.Data.Entity;

namespace Sem3_Lab4_OOP
{
    class ServiceDbContext : DbContext
    {
        public ServiceDbContext() : base("dbService")
        {

        }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}