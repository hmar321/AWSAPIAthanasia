using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("GENEROS_LIBROS")]
    public class GenerosLibros
    {
        [Key]
        [Column("ID_GENERO_LIBRO")]
        public int IdGeneroLibro { get; set; }
        [Column("ID_GENERO")]
        public int? IdGenero { get; set; }
        [Column("ID_LIBRO")]
        public int? IdLibro { get; set; }
    }
}
