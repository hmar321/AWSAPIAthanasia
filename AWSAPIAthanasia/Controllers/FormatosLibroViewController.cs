using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatosLibroViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public FormatosLibroViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Obtiene las vistas de formatos disponibles para un libro específico.
        /// </summary>
        /// <param name="idlibro">Id del libro del cual se desean obtener los formatos disponibles.</param>
        /// <response code="200">Devuelve una lista de los formatos disponibles para el libro especificado.</response>
        [HttpGet]
        [Route("[action]/{idlibro}")]
        public async Task<ActionResult<List<FormatoLibroView>>> FormatosLibro(int idlibro)
        {
            List<FormatoLibroView> formatos = await this.repo.GetAllFormatoLibroViewByIdLibroAsync(idlibro);
            return formatos;
        }
    }
}
