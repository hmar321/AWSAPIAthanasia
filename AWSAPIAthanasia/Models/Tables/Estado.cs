using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiAthanasia.Models.Tables
{
    [Table("ESTADO")]
    public class Estado
    {
        [Key]
        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}
