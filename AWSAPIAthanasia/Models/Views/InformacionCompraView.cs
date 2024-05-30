using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAthanasia.Models.Views
{
    [Table("V_INFORMACION_COMPRA_USUARIO")]
    public class InformacionCompraView
    {
        [Key]
        [Column("ID_INFORMACION_COMPRA")]
        public int IdInformacionCompra { get; set; }
        [Column("NOMBRE")]
        public string Nombre { get; set; }
        [Column("DIRECCION")]
        public string Direccion { get; set; }
        [Column("INDICACIONES")]
        public string Indicaciones { get; set; }
        [Column("ID_METODO_PAGO")]
        public int IdMetodoPago { get; set; }
        [Column("METODO_PAGO")]
        public string MetodoPago { get; set; }
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }   
    }
}
