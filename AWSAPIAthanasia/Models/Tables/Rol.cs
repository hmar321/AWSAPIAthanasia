using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("ROL")]
    public class Rol
    {
        [Key]
        [Column("ID_ROL")]
        public int IdRol { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}
