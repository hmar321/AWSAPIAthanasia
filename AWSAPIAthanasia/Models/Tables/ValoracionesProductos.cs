using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Tables
{
    [Table("VALORACIONES_PRODUCTOS")]
    public class ValoracionesProductos
    {
        [Key]
        [Column("ID_VALORACIO_PRODUCTO")]
        public int IdValoracionProducto { get; set; }
        [Column("ID_VALORACIO")]
        public int IdValoracion { get; set; }
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }
    }
}
