using ApiAlmoxarifado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiAlmoxarifado.Data.Context
{
    public class ApiContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Registros> Registros { get; set; }
    }
}
