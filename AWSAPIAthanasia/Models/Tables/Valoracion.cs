using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("VALORACION")]
    public class Valoracion
    {
        [Key]
        [Column("ID_VALORACION")]
        public int IdValoracion { get; set; }
        [Column("PUNTUACION")]
        public int? Puntuacion { get; set; }
        [Column("COMENTARIO")]
        public string? Comentario { get; set; }
        [Column("FECHA_COMENTARIO")]
        public DateTime? FechaComentario { get; set; }
        [Column("ID_USUARIO")]
        public int? IdUsuario { get; set; }

    }
}
