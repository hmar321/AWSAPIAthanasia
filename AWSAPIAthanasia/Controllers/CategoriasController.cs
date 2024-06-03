using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public CategoriasController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Obtiene todas las categorías disponibles.
        /// </summary>
        /// <response code="200">Devuelve una lista de todas las categorías disponibles.</response>
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetCategorias()
        {
            List<Categoria> categorias = await this.repo.GetAllCategoriasAsync();
            return categorias;
        }
    }
}
