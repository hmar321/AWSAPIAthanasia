using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetodosPagoController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public MetodosPagoController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// METODO PROTEGIDO Obtiene todos los métodos de pago disponibles.
        /// </summary>
        /// <response code="200">Devuelve una lista de todos los métodos de pago disponibles.</response>
        /// <response code="401">Unauthorized. No se ha proporcionado un token válido.</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<MetodoPago>>> GetMetodosPago()
        {
            List<MetodoPago> metodos = await this.repo.GetMetodoPagosAsync();
            return metodos;
        }
    }
}
