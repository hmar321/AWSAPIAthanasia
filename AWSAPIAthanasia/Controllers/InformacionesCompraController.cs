using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacionesCompraController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public InformacionesCompraController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// Busca una información de compra por su Id.
        /// </summary>
        /// <param name="id">Id de la información de compra a buscar.</param>
        /// <response code="200">Devuelve la información de compra encontrada.</response>
        /// <response code="404">NotFound. No se ha encontrado la información de compra especificada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<InformacionCompra>> FindInformacion(int id)
        {
            InformacionCompra informacion = await this.repo.GetInformacionCompraByIdAsync(id);
            return informacion;
        }
        /// <summary>
        /// Obtiene todas las credenciales de compra asociadas a un usuario específico.
        /// </summary>
        /// <param name="idusuario">Id del usuario del cual se desean obtener las informaciones de compra.</param>
        /// <response code="200">Devuelve una lista de todas las informaciones de compra asociadas al usuario especificado.</response>
        [HttpGet]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<List<InformacionCompra>>> ByIdUsuario(int idusuario)
        {
            List<InformacionCompra> informaciones = await this.repo.GetAllInformacionComprabyIdUsuarioAsync(idusuario);
            return informaciones;
        }
        /// <summary>
        /// METODO PROTEGIDO Inserta una nueva información de compra.
        /// </summary>
        /// <param name="info">Información de compra a insertar.</param>
        /// <response code="200">Devuelve la información de compra insertada.</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InformacionCompra>> Insert(InformacionCompra info)
        {
            InformacionCompra informacion = await this.repo.InsertInformacionAsync(info.Nombre, info.Direccion, info.Indicaciones, info.IdMetodoPago.Value, info.IdUsuario.Value);
            return informacion;
        }
        /// <summary>
        /// METODO PROTEGIDO Elimina una información de compra por su Id.
        /// </summary>
        /// <param name="id">Id de la información de compra a eliminar.</param>
        /// <response code="200">Devuelve el número de elementos eliminados.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        /// <response code="404">NotFound. No se ha encontrado la información de compra especificada.</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            int result = await this.repo.DeleteInformacionCompraByIdAsync(id);
            if (result == 0)
            {
                return NotFound();
            }
            return result;
        }
    }
}
