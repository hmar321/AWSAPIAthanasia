using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiAthanasia.Models.Tables
{
    [Table("OPINIONES_LIBROS")]
    public class OpinionesLibros
    {
        [Key]
        [Column("ID_OPINION_LIBRO")]
        public int IdOpinionLibro { get; set; }

        [Column("ID_OPINION")]
        public int? IdOpinion { get; set; }

        [Column("ID_USUARIO")]
        public int? IdUsuario { get; set; }

        [Column("ID_LIBRO")]
        public int? IdLibro { get; set; }

    }
}
