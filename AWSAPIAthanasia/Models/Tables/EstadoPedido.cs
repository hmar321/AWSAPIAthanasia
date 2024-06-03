using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("ESTADO_PEDIDO")]
    public class EstadoPedido
    {
        [Key]
        [Column("ID_ESTADO_PEDIDO")]
        public int IdCategoria { get; set; }

        [Column("NOMBRE")]
        public string? Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}
