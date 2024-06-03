using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public PedidosViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// METODO PROTEGIDO Obtiene todos los pedidos con datos extra asociados a un usuario específico.
        /// </summary>
        /// <param name="idusuario">Id del usuario del cual se desean obtener los pedidos.</param>
        /// <response code="200">Devuelve una lista de todos los pedidos asociados al usuario especificado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        [Authorize]
        [HttpGet]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<List<PedidoView>>> ByIdUsuario(int idusuario)
        {
            List<PedidoView> pedidos = await this.repo.GetAllPedidoViewByIdUsuario(idusuario);
            return pedidos;
        }
    }
}
