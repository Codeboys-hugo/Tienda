using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Tienda.Models
{
    public class PedidosProductos
    {
        [Key]
        public int IdPedido { get; set; }
        [ForeignKey("IdPedido")]
        public Pedidos Pedido { get; set; }
        [Key]
        public int IdProducto{ get; set; }
        [ForeignKey("IdProducto")]
        public Productos Producto { get; set; }

        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }

        
    }
}
