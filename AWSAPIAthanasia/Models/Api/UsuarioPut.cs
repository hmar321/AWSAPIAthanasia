namespace ApiAthanasia.Models.Api
{
    public class UsuarioPut
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string? Imagen { get; set; }
    }
}
