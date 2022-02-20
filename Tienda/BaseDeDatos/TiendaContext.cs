using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Models;
using Microsoft.EntityFrameworkCore;

namespace Tienda.BaseDeDatos
{
    public class TiendaContext:DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) : base(options)
        {
        }

        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<PedidosProductos> PedidosProductos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PedidosProductos>()
                .HasKey(c => new { c.IdPedido, c.IdProducto });
        }
    }
}
