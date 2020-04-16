using Microsoft.EntityFrameworkCore;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public class DataContext :DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        // Dodajemy tabelę do bazy danych o nazwie Values (bo przechowuje wartości)
        public DbSet<Value> Values { get; set; }

    }
}