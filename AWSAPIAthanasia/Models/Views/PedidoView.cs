using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Views
{
    [Table("V_PEDIDO")]
    public class PedidoView
    {
        [Key]
        [Column("ID_PEDIDO")]
        public int IdPedido { get; set; }
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        [Column("FECHA_SOLICITUD")]
        public DateTime FechaSolicitud { get; set; }
        [Column("FECHA_ESTIMADA")]
        public DateTime FechaEstimada { get; set; }
        [Column("ESTADO_PEDIDO")]
        public string EstadoPedido { get; set; }
        [Column("LIBROS")]
        public string Libros { get; set; }
    }
}
