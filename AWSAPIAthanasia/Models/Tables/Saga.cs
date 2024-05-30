using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("Saga")]
    public class Saga
    {
        [Key]
        [Column("ID_SAGA")]
        public int IdSaga { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }
    }
}
