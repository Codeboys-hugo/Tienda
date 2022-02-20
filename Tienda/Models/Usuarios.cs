using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tienda.BaseDeDatos;

namespace Tienda.Models
{
    public class Usuarios
    {
        [Key]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
        [Required]
        public bool Activo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<Usuarios> Get(TiendaContext context)
        {
            return context.Usuarios.FromSqlRaw("SELECT * FROM dbo.Usuarios").ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public Usuarios Get(TiendaContext context, string username)
        {
            return context.Usuarios.FromSqlRaw("SELECT * FROM dbo.Usuarios where username = '" + username + "'").FirstOrDefault();
        }

        public Usuarios Create(TiendaContext context, Usuarios usuario)
        {

            string sql = "INSERT INTO [dbo].[Usuarios] (Username, Correo , Contraseña, Activo) VALUES  ('" + usuario.Username + "', '" + usuario.Correo + "' , '" + usuario.Contraseña + "' , " + 1 + ")";
            var data = context.Database.ExecuteSqlCommand(sql);
            return new Usuarios();

        }

        public Usuarios Edit(TiendaContext context, Usuarios usuario)
        {
            string sql = "update [dbo].[Usuarios] set Username = '" + usuario.Username + "', Correo= '" + usuario.Correo + "' , Contraseña= '" + usuario.Contraseña + "', Activo= " + usuario.Activo + " where username = '" + usuario.Username + "'";
            var data = context.Database.ExecuteSqlCommand(sql);
            return new Usuarios();
        }

        public Usuarios Delete(TiendaContext context, string username)
        {
            string sql = "update [dbo].[Usuarios] set Activo= " + 0 + " where username = '" + username + "'";
            var data = context.Database.ExecuteSqlCommand(sql);
            return new Usuarios();
        }

    }
}
