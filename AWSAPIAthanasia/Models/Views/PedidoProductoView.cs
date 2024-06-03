using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiAthanasia.Models.Views
{
    [Table("V_PEDIDO_PRODUCTO")]
    public class PedidoProductoView
    {
        [Key]
        [Column("ID_PEDIDO_PRODUCTO")]
        public int IdPedidoProducto { get; set; }
        [Column("ID_LIBRO")]
        public int IdLibro { get; set; }
        [Column("TITULO")]
        public string Titulo { get; set; }
        [Column("PORTADA")]
        public string? Portada { get; set; }
        [Column("AUTOR")]
        public string Autor { get; set; }
        [Column("ID_FORMATO")]
        public int IdFormato { get; set; }
        [Column("FORMATO")]
        public string Formato { get; set; }
        [Column("UNIDADES")]
        public int Unidades { get; set; }
        [Column("PRECIO")]
        public double Precio { get; set; }
        [Column("ID_PEDIDO")]
        public int IdPedido { get; set; }
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }
    }
}
