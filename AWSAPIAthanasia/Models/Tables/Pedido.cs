using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("PEDIDO")]
    public class Pedido
    {
        [Key]
        [Column("ID_PEDIDO")]
        public int IdPedido { get; set; }
        [Column("ID_USUARIO")]
        public int? IdUsuario { get; set; }
        [Column("FECHA_SOLICITUD")]
        public DateTime? FechaSolicitud { get; set; }
        [Column("FECHA_ESTIMADA")]
        public DateTime? FechaEstimada{ get; set; }
        [Column("FECHA_ENTREGA")]
        public DateTime? FechaEntrega { get; set; }
        [Column("ID_ESTADO_PEDIDO")]
        public int IdEstadoPedido { get; set; }

    }
}
