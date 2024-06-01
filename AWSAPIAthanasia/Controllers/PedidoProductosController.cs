using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidoProductosController : ControllerBase
    {
        private IRepositoryAthanasia repo;
        private IMapper mapper;

        public PedidoProductosController(IRepositoryAthanasia repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        /// <summary>
        /// METODO PROTEGIDO Inserta una lista de productos en un pedido asociado a un usuario específico.
        /// </summary>
        /// <param name="idusuario">Id del usuario al cual se asociará el pedido.</param>
        /// <param name="productos">Lista de productos a agregar al pedido.</param>
        /// <response code="200">Devuelve el número de productos insertados en el pedido.</response>
        /// <response code="400">BadRequest. No se pudieron agregar los productos al pedido.</response>
        [Authorize]
        [HttpPost]
        [Route("[action]/{idusuario}")]
        public async Task<ActionResult<int>> InsertPedidoProductos(int idusuario, List<PedidoProductoPost> productos)
        {
            List<PedidoProducto> mappedProductos = this.mapper.Map<List<PedidoProducto>>(productos);
            int result = await this.repo.InsertListPedidoProductosAsync(idusuario, mappedProductos);
            if (result == 0)
            {
                return BadRequest();
            }
            return result;
        }
    }
}
