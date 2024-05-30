using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoProductosViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public PedidoProductosViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// METODO PROTEGIDO Obtiene todas las vistas de productos asociados a un pedido específico.
        /// </summary>
        /// <param name="idpedido">Id del pedido del cual se desean obtener los productos.</param>
        /// <response code="200">Devuelve una lista de todos los productos asociados al pedido especificado.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        [Authorize]
        [HttpGet]
        [Route("[action]/{idpedido}")]
        public async Task<ActionResult<List<PedidoProductoView>>> ByIdPedido(int idpedido)
        {
            List<PedidoProductoView> productos = await this.repo.GetPedidoProductoViewsByIdPedidoAsync(idpedido);
            return productos;
        }
    }
}
