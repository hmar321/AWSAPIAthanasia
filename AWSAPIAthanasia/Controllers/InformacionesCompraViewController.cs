using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformacionesCompraViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public InformacionesCompraViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// METODO PROTEGIDO Obtiene todas las vistas de credenciales de compra asociadas a un usuario específico.
        /// </summary>
        /// <param name="idusuario">Id del usuario del cual se desean obtener las informaciones de compra.</param>
        /// <response code="200">Devuelve una lista de todas las informaciones de compra asociadas al usuario especificado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        [Authorize]
        [HttpGet]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<List<InformacionCompraView>>> ByIdUsuario(int idusuario)
        {
            List<InformacionCompraView> informaciones = await this.repo.GetAllInformacionCompraViewByIdUsuarioAsync(idusuario);
            return informaciones;
        }
    }
}
