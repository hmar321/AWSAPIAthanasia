using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Views
{
    [Table("V_PRODUCTO")]
    public class ProductoView
    {
        [Key]
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }
        [Column("ID_LIBRO")]
        public int IdLibro { get; set; }
        [Column("TITULO")]
        public string Titulo { get; set; }
        [Column("SINOPSIS")]
        public string? Sinopsis { get; set; }
        [Column("FECHA_PUBLICACION")]
        public DateTime? FechaPublicacion { get; set; }
        [Column("PORTADA")]
        public string? Portada { get; set; }
        [Column("CATEGORIA")]
        public string? Categoria { get; set; }
        [Column("AUTOR")]
        public string Autor { get; set; }
        [Column("GENEROS")]
        public string? Generos { get; set; }
        [Column("SAGA")]
        public string? Saga { get; set; }
        [Column("ISBN")]
        public string Isbn { get; set; }
        [Column("FORMATO")]
        public string Formato { get; set; }
        [Column("PRECIO")]
        public double Precio { get; set; }
        [Column("EDITORIAL")]
        public string? Editorial { get; set; }
        [Column("LOGO")]
        public string? Logo { get; set; }

        
    }
}
