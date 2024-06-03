using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ApiAthanasia.Models.Tables
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }
        [Column("NOMBRE")]
        public string? Nombre { get; set; }
        [Column("APELLIDO")]
        public string? Apellido { get; set; }
        [Column("EMAIL")]
        public string? Email { get; set; }
        [Column("PASSWORD")]
        public string? Password { get; set; }
        [Column("IMAGEN")]
        public string? Imagen { get; set; }
        [Column("PASS")]
        public byte[]? Pass { get; set; }
        [Column("SALT")]
        public string? Salt { get; set; }
        [Column("TOKEN")]
        public string? Token { get; set; }
        [Column("ID_ROL")]
        public int? IdRol { get; set; }
        [Column("ID_ESTADO")]
        public int? IdEstado { get; set; }

    }

}
