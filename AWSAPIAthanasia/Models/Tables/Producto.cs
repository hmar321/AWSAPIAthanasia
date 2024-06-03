using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("PRODUCTO")]
    public class Producto
    {
        [Key]
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }
        [Column("ISBN")]
        public string? Isbn { get; set; }
        [Column("PRECIO")]
        public double? Precio { get; set; }
        [Column("ID_FORMATO")]
        public int? IdFormato { get; set; }
        [Column("ID_LIBRO")]
        public int? IdLibro { get; set; }
        [Column("ID_EDITORIAL")]
        public int? IdEditorial { get; set; }

    }
}
