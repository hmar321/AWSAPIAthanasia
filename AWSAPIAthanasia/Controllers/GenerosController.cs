using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public GenerosController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Obtiene todos los géneros activos ordenados por nombre.
        /// </summary>
        /// <response code="200">Devuelve una lista de todos los géneros activos ordenados alfabéticamente por nombre.</response>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Genero>>> ActivosOrderByNombre()
        {
            List<Genero> generos = await this.repo.GetAllGenerosAsync();
            return generos;
        }

        /// <summary>
        /// Obtiene un género por su nombre.
        /// </summary>
        /// <param name="nombre">Nombre del género a buscar.</param>
        /// <response code="200">Devuelve el género encontrado.</response>
        /// <response code="404">NotFound. No se ha encontrado el género especificado.</response>
        [HttpGet]
        [Route("[action]/{nombre}")]
        public async Task<ActionResult<Genero>> GeneroByNombre(string nombre)
        {
            Genero genero = await this.repo.GetGeneroByNombreAsync(nombre);
            return genero;
        }

    }
}
