using ApiAthanasia.Models.Api;
using ApiAthanasia.Models.Util;
using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosSimplesViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public ProductosSimplesViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// Obtiene todas las vistas de productos simples.
        /// </summary>
        /// <response code="200">Devuelve una lista de todas las vistas de productos simples.</response>
        [HttpGet]
        public async Task<ActionResult<List<ProductoSimpleView>>> ProductosSimplesView()
        {
            List<ProductoSimpleView> prodSimples = await this.repo.GetAllProductoSimpleViewAsync();
            return prodSimples;
        }

        /// <summary>
        /// Obtiene una vista de producto simple por su Id.
        /// </summary>
        /// <param name="id">Id del producto simple.</param>
        /// <response code="200">Devuelve la vista del producto simple encontrada.</response>
        /// <response code="404">NotFound. No se ha encontrado la vista del producto simple especificada.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoSimpleView>> ProductoSimple(int id)
        {
            ProductoSimpleView prodSimple = await this.repo.GetProductoSimpleByIdAsync(id);
            if (prodSimple == null)
            {
                return NotFound();
            }
            return prodSimple;
        }

        /// <summary>
        /// Obtiene vistas de productos simples con títulos y autores distintos.
        /// </summary>
        /// <response code="200">Devuelve una lista de vistas de productos simples con títulos y autores distintos.</response>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<ProductoSimpleView>>> DistinctTituloAutor()
        {
            List<ProductoSimpleView> prodSimple = await this.repo.GetProductoSimpleViewTituloAutorAsync();
            if (prodSimple == null)
            {
                return NotFound();
            }
            return prodSimple;
        }

        /// <summary>
        /// Obtiene una paginación de vistas de productos simples.
        /// </summary>
        /// <param name="posicion">Posición de la página.</param>
        /// <param name="registros">Número de registros por página.</param>
        /// <response code="200">Devuelve una paginación de vistas de productos simples.</response>
        [HttpGet]
        [Route("[action]/{posicion}/{registros}")]
        public async Task<ActionResult<PaginacionModel<ProductoSimpleView>>> Paginacion(int posicion, int registros)
        {
            PaginacionModel<ProductoSimpleView> model = await this.repo.GetProductosSimplesPaginacionAsyn(posicion, registros);
            return model;
        }

        /// <summary>
        /// Obtiene una paginación de vistas de productos simples por búsqueda.
        /// </summary>
        /// <param name="busqueda">Término de búsqueda.</param>
        /// <param name="posicion">Posición de la página.</param>
        /// <param name="registros">Número de registros por página.</param>
        /// <response code="200">Devuelve una paginación de vistas de productos simples que coinciden con la búsqueda.</response>
        [HttpGet]
        [Route("[action]/{busqueda}/{posicion}/{registros}")]
        public async Task<ActionResult<PaginacionModel<ProductoSimpleView>>> PaginacionBusqueda(string? busqueda, int posicion, int registros)
        {
            if (busqueda==null)
            {
                busqueda = "";
            }
            PaginacionModel<ProductoSimpleView> model = await this.repo.GetAllProductoSimpleViewSearchPaginacionAsync(busqueda, posicion, registros);
            return model;
        }

        /// <summary>
        /// Obtiene vistas de productos simples por lista de Ids.
        /// </summary>
        /// <param name="id">Lista de Ids de productos simples.</param>
        /// <response code="200">Devuelve una lista de vistas de productos simples que coinciden con los Ids proporcionados.</response>
        /// <response code="404">NotFound. No se encontraron vistas de productos simples para los Ids proporcionados.</response>
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<ProductoSimpleView>>> ByListId([FromQuery] List<int> id)
        {
            List<ProductoSimpleView> productos = await this.repo.GetAllProductoSimpleViewByIds(id);
            if (productos.Count == 0)
            {
                return NotFound();
            }
            return productos;
        }

        /// <summary>
        /// Obtiene una paginación de vistas de productos simples por búsqueda, categorías y géneros.
        /// </summary>
        /// <param name="busqueda">Término de búsqueda.</param>
        /// <param name="posicion">Posición de la página.</param>
        /// <param name="registros">Número de registros por página.</param>
        /// <param name="idsmodel">Modelo que contiene los Ids de categorías y géneros para filtrar.</param>
        /// <response code="200">Devuelve una paginación de vistas de productos simples que coinciden con la búsqueda, categorías y géneros.</response>
        [HttpPost]
        [Route("[action]/{busqueda}/{posicion}/{registros}")]
        public async Task<ActionResult<PaginacionModel<ProductoSimpleView>>> PaginacionBusquedaCategoriasGeneros(string? busqueda, int posicion, int registros, CategoriasGenerosModel idsmodel)
        {
            if (busqueda == null)
            {
                busqueda = "";
            }
            PaginacionModel<ProductoSimpleView> model = await this.repo.GetProductoSimpleViewsCategoriasGeneroAsync(busqueda, posicion, registros, idsmodel.IdsCategorias, idsmodel.IdsGeneros);
            return model;
        }

    }
}
