using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("OPINION")]
    public class Opinion
    {
        [Key]
        [Column("ID_OPINION")]
        public int IdOpinion { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}
