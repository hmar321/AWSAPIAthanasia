using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace ApiAthanasia.Models.Tables
{
    [Table("LIBRO")]
    public class Libro
    {
        [Key]
        [Column("ID_LIBRO")]
        public int IdLibro { get; set; }
        [Column("TITULO")]
        public string? Titulo { get; set; }
        [Column("SINOPSIS")]
        public string? Sinopsis { get; set; }
        [Column("FECHA_PUBLICACION")]
        public DateTime? FechaPublicacion { get; set; }
        [Column("PORTADA")]
        public string? Portada { get; set; }
        [Column("ID_AUTOR")]
        public int? IdAutor { get; set; }
        [Column("ID_CATEGORIA")]
        public int? IdCategoria { get; set; }
        [Column("ID_GENERO")]
        public int? IdSaga { get; set; }
    }
}
