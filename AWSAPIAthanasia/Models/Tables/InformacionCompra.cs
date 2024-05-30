using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiAthanasia.Models.Tables
{
    [Table("INFORMACION_COMPRA")]
    public class InformacionCompra
    {
        [Key]
        [Column("ID_INFORMACION_COMPRA")]
        public int IdInformacionCompra { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("DIRECCION")]
        public string? Direccion { get; set; }
        [Column("INDICACIONES")]
        public string? Indicaciones { get; set; }
        [Column("ID_METODO_PAGO")]
        public int? IdMetodoPago { get; set; }
        [Column("ID_USUARIO")]
        public int? IdUsuario{ get; set; }
    }
}
