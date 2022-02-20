using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda.BaseDeDatos;
using Tienda.Models;

namespace Tienda.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly TiendaContext _context;
        private readonly ILoggerManager _logger;

        public UsuariosController(TiendaContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Usuarios
        public IActionResult Index()
        {
            _logger.LogInformation("UsuariosController index");
            Usuarios usuarios = new Usuarios();
            return View(usuarios.Get(_context));
        }

        // GET: Usuarios/Details/5
        public  IActionResult Details(string id)
        {
            _logger.LogInformation("UsuariosController Details");
            if (id == null)
            {
                return NotFound();
            }

            Usuarios usuario = new Usuarios();

            var usuarios = usuario.Get(_context, id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            _logger.LogInformation("UsuariosController create");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Username,Correo,Contraseña")] Usuarios usuarios)
        {
            _logger.LogInformation("UsuariosController Create con información");
            if (ModelState.IsValid)
            {
                Usuarios usuario = new Usuarios();


                if (!UsuariosExists(usuarios.Username))
                {
                    usuario.Create(_context, usuarios);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "El usuario ya existe");
                    return View(usuarios);
                }

                
            }
            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public IActionResult Edit(string id)
        {
            _logger.LogInformation("UsuariosController edit");

            if (id == null)
            {
                return NotFound();
            }

            Usuarios usuario = new Usuarios();
            usuario = usuario.Get(_context, id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, [Bind("Username,Correo,Contraseña")] Usuarios usuarios)
        {
            _logger.LogInformation("UsuariosController edit con información");

            if (id != usuarios.Username)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Usuarios usuario = new Usuarios();
                    usuario.Edit(_context, usuarios);                                     
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuarios.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuarios);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("UsuariosController delete con información");

            if (id == null)
            {
                return NotFound();
            }
            Usuarios usuario = new Usuarios();
            
            var usuarios = usuario.Get(_context, id);
           
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            _logger.LogInformation("UsuariosController delete confirmed");
            Usuarios usuario = new Usuarios();
            var usuarios = usuario.Delete(_context, id);           
            return RedirectToAction(nameof(Index));
        }

        private bool UsuariosExists(string id)
        {
            return _context.Usuarios.Any(e => e.Username == id);
        }
    }
}
