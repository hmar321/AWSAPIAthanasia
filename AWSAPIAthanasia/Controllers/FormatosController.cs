using ApiAthanasia.Models.Tables;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ApiAthanasia.Helpers;
using Newtonsoft.Json;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatosController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public FormatosController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Obtiene todos los formatos disponibles.
        /// </summary>
        /// <response code="200">Devuelve una lista de todos los formatos disponibles.</response>
        [HttpGet]
        public async Task<ActionResult<List<Formato>>> Formatos()
        {
            List<Formato> formatos = await this.repo.GetAllFormatosAsync();
            return formatos;
        }
        /// <summary>
        /// Busca un formato por su Id.
        /// </summary>
        /// <param name="id">Id del formato a buscar.</param>
        /// <response code="200">Devuelve el formato encontrado.</response>
        /// <response code="404">NotFound. No se ha encontrado el formato especificado.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Formato>> FindFormato(int id)
        {
            Formato formato = await this.repo.GetFormatoByIdAsync(id);
            if (formato==null)
            {
                return NotFound();
            }
            return formato;
        }
    }
}
