using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("PEDIDOS_PRODUCTOS")]
    public class PedidoProducto
    {
        [Key]
        [Column("ID_PEDIDO_PRODUCTO")]
        public int IdPedidoProducto { get; set; }
        [Column("UNIDADES")]
        public int? Unidades { get; set; }
        [Column("ID_PEDIDO")]
        public int? IdPedido { get; set; }
        [Column("ID_PRODUCTO")]
        public int? IdProducto { get; set; }
    }
}
