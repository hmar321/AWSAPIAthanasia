using ApiAthanasia.Helpers;
using ApiAthanasia.Models.Api;
using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using AWSAPIAthanasia.Models.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IRepositoryAthanasia repo;
        private string encriptKey;

        public UsuariosController(IRepositoryAthanasia repo, KeysModel keysModel)
        {
            this.repo = repo;
            this.encriptKey = keysModel.EncriptKey;
        }

        /// <summary>
        /// METODO PROTEGIDO Busca un usuario por su Id.
        /// </summary>
        /// <param name="id">Id del usuario a buscar.</param>
        /// <response code="200">Devuelve el usuario encontrado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario especificado.</response>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> FindUsuario(int id)
        {
            Usuario usuario = await this.repo.FindUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// METODO PROTEGIDO Obtiene el usuario asociado al token de autorización.
        /// </summary>
        /// <response code="200">Devuelve el usuario asociado al token de autorización.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario asociado al token.</response>
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> UsuarioToken()
        {
            string jsonData = HttpContext.User.FindFirst("UserData").Value.ToString();
            string json = HelperCryptography.DecryptString(this.encriptKey, jsonData);
            Usuario usuario = JsonConvert.DeserializeObject<Usuario>(json);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// METODO PROTEGIDO Obtiene el usuario asociado a un token de correo electrónico.
        /// </summary>
        /// <param name="token">Token de correo electrónico.</param>
        /// <response code="200">Devuelve el usuario asociado al token de correo electrónico.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario asociado al token de correo electrónico.</response>
        [Authorize]
        [HttpGet]
        [Route("[action]/{token}")]
        public async Task<ActionResult<Usuario>> UsuarioTokenMail(string token)
        {
            Usuario usuario = await this.repo.GetUsuarioByTokenAsync(token);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="usuario">Datos del usuario a registrar.</param>
        /// <response code="200">Devuelve el usuario registrado.</response>
        /// <response code="400">BadRequest. No se pudo registrar el usuario.</response>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Registro(UsuarioPost usuario)
        {
            Usuario user = await this.repo.RegistrarUsuarioAsync(usuario.Nombre, usuario.Apellido, usuario.Email, usuario.Password);
            if (user == null)
            {
                return BadRequest();
            }
            return user;
        }

        /// <summary>
        /// METODO PROTEGIDO Actualiza los datos de un usuario.
        /// </summary>
        /// <param name="usuario">Datos actualizados del usuario.</param>
        /// <response code="200">Devuelve el usuario actualizado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario especificado.</response>
        [Authorize]
        [HttpPut]
        public async Task<ActionResult<Usuario>> Update(UsuarioPut usuario)
        {
            Usuario user = await this.repo.UpdateUsuarioAsync(usuario.IdUsuario, usuario.Nombre, usuario.Apellido, usuario.Email, usuario.Imagen);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        /// <summary>
        /// METODO PROTEGIDO Actualiza la contraseña de un usuario.
        /// </summary>
        /// <param name="idusuario">Id del usuario cuya contraseña se actualizará.</param>
        /// <param name="newpassword">Nueva contraseña.</param>
        /// <response code="200">Devuelve el usuario con la contraseña actualizada.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario especificado.</response>
        [Authorize]
        [HttpPut]
        [Route("[action]/{idusuario}/{newpassword}")]
        public async Task<ActionResult<Usuario>> UpdatePassword(int idusuario, string newpassword)
        {
            Usuario usuario = await this.repo.UpdateUsuarioPasswordAsync(idusuario, newpassword);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// METODO PROTEGIDO Actualiza el token de correo electrónico de un usuario.
        /// </summary>
        /// <param name="idusuario">Id del usuario cuyo token de correo electrónico se actualizará.</param>
        /// <response code="200">Devuelve el usuario con el token actualizado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario especificado.</response>
        [Authorize]
        [HttpPut]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<Usuario>> UpdateTokenMail(int idusuario)
        {
            Usuario usuario = await this.repo.UpdateUsuarioTokenAsync(idusuario);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// Activa un usuario utilizando un token de correo electrónico.
        /// </summary>
        /// <param name="token">Token de correo electrónico.</param>
        /// <response code="200">Devuelve el usuario activado.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario asociado al token de correo electrónico.</response>
        [HttpPut]
        [Route("[action]/{token}")]
        public async Task<ActionResult<Usuario>> ActivarUsuario(string token)
        {
            Usuario usuario = await this.repo.ActivarUsuarioAsync(token);
            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        /// <summary>
        /// METODO PROTEGIDO Elimina un usuario por su Id.
        /// </summary>
        /// <param name="id">Id del usuario a eliminar.</param>
        /// <response code="200">Devuelve el número de usuarios eliminados.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado el usuario especificado.</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            int result = await this.repo.DeleteUsuarioAsync(id);
            if (result == 0)
            {
                return NotFound();
            }
            return result;
        }


    }
}
