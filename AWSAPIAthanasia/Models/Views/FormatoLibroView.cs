using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Views
{
    [Table("V_FORMATO_LIBRO")]
    public class FormatoLibroView
    {
        [Key]
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }
        [Column("ID_LIBRO")]
        public int IdLibro { get; set; }
        [Column("FORMATO")]
        public string Formato { get; set; }
    }
}
