using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tienda.Models;

namespace Tienda.BaseDeDatos
{
    public static class InitDB
    {
        public static void Initialize(TiendaContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Usuarios.Any())
            {
                return;   // DB has been seeded
            }

            var usuarios = new Usuarios[]
            {
            new Usuarios{Username="Admin",Contraseña="123",Correo="Hugo99mx@gmail.com",Activo=true},
            new Usuarios{Username="Cliente",Contraseña="123",Correo="Hugo99mx@gmail.com",Activo=true}

            };
            foreach (Usuarios u in usuarios)
            {
                context.Usuarios.Add(u);
            }
            context.SaveChanges();

            var productos = new Productos[]
            {
            new Productos{Nombre="Manzana",Precio=10},
            new Productos{Nombre="Libro",Precio=5},
            new Productos{Nombre="Pluma",Precio=2},
            new Productos{Nombre="Lapiz",Precio=2}

            };
            foreach (Productos p in productos)
            {
                context.Productos.Add(p);
            }
            context.SaveChanges();


        }

    }
}
