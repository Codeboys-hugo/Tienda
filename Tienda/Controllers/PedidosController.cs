using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tienda.BaseDeDatos;
using Tienda.Models;

namespace Tienda.Controllers
{
    public class PedidosController : Controller
    {
        private readonly TiendaContext _context;
        private readonly ILoggerManager _logger;        


        public PedidosController(TiendaContext context, ILoggerManager logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            var tiendaContext = _context.Pedidos.Include(p => p.Usuario);
            return View(await tiendaContext.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Productos = _context.PedidosProductos.Where(x => x.IdPedido == id).Include(p => p.Producto).ToList();

            var pedidos = _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefault(m => m.Id == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            ViewBag.Productos = _context.Productos.ToList();
            ViewData["Username"] = new SelectList(_context.Usuarios, "Username", "Username");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pedidos pedido)
        {
           
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<PedidosProductos> productos = new List<PedidosProductos>();
                pedido.Username = Request.Form["Username"];
                pedido.Fecha = DateTime.UtcNow;
                List<int> idProducto = new List<int>();
                foreach (string s in Request.Form.Keys)
                {
                    if (s.Contains("-"))
                    {
                        var data = s.Split("-");
                        if (!idProducto.Any(x => x.ToString() == data[0]))
                        {
                            idProducto.Add(int.Parse(data[0]));
                            PedidosProductos producto = new PedidosProductos();
                            producto.IdProducto = int.Parse(data[0]);
                            producto.Pedido = pedido;
                            producto.Cantidad = int.Parse(Request.Form[data[0] + "-Cantidad"]);
                            producto.Precio = _context.Productos.FirstOrDefault(x => x.Id == int.Parse(data[0])).Precio;
                            productos.Add(producto);
                        }
                    }
                    pedido.Total = productos.Sum(x => x.Precio * x.Cantidad);
                }

                _context.Add(pedido);
                _context.AddRange(productos);
                _context.SaveChanges();
                transaction.Commit();

                SendMessage(pedido.Username);


            }
            catch (Exception ex)
            {
                //Se guarda error y automaticamente se hace el rollback
                _logger.LogError("Error al guardar pedido");
            }

            return RedirectToAction(nameof(Index));


        }


        /// <summary>
        /// Indica mensaje de respuesta
        /// </summary>
        /// <param name="message"></param>
        /// <param name="MAC"></param>
        /// <returns></returns>
        private void SendMessage(string usuario)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory("URL servidor con de mensajeria");
            connectionFactory.UserName = ""; //Usuario
            connectionFactory.Password = ""; // password
            IConnection _connection;
            _connection = connectionFactory.CreateConnection();
            _connection.Start();
            Apache.NMS.ISession _session;
            _session = _connection.CreateSession();
           
            IDestination dest = _session.GetQueue(usuario);  //Canal que guardara el mensaje y esperara en cola hasta su lectura         
            using (IMessageProducer producer = _session.CreateProducer(dest))
            {
                var objectMessage = producer.CreateTextMessage("Pedido realizado");
                producer.Send(objectMessage);
            }
            _session.Close();
            _connection.Close();           

            
        }

        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.Pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return NotFound();
            }
            ViewData["Username"] = new SelectList(_context.Usuarios, "Username", "Username", pedidos.Username);
            return View(pedidos);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Total,Username")] Pedidos pedidos)
        {
            if (id != pedidos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidosExists(pedidos.Id))
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
            ViewData["Username"] = new SelectList(_context.Usuarios, "Username", "Username", pedidos.Username);
            return View(pedidos);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidos = await _context.Pedidos.FindAsync(id);
            _context.Pedidos.Remove(pedidos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PedidosExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
