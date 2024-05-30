using ApiAthanasia.Helpers;
using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Views;
using ApiAthanasia.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace ApiAthanasia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosViewController : ControllerBase
    {
        private IRepositoryAthanasia repo;

        public ProductosViewController(IRepositoryAthanasia repo)
        {
            this.repo = repo;
        }
        /// <summary>
        /// Método para recuperar una vista completa de los Productos.
        /// </summary>
        /// <response code="200">Devuelve una lista de productos.</response>
        [HttpGet]
        public async Task<ActionResult<List<ProductoView>>> ProductosView()
        {
            List<ProductoView> productos = await this.repo.GetAllProductoViewAsync();
            return productos;
        }

        /// <summary>
        /// Método para buscar un ProductoView por Id.
        /// </summary>
        /// <param name="id">Id del ProductoView.</param>
        /// <response code="200">Devuelve el ProductoView encontrado.</response>
        /// <response code="404">NotFound. No se ha encontrado el ProductoView.</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductoView>> ProductoView(int id)
        {
            ProductoView producto = await this.repo.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return producto;
        }

        /// <summary>
        /// Método para buscar productos por formato.
        /// </summary>
        /// <param name="formato">Formato de los productos a buscar.</param>
        /// <response code="200">Devuelve una lista de ProductoView que corresponden al formato especificado.</response>
        /// <response code="404">NotFound. No se ha encontrado el formato especificado.</response>
        [HttpGet]
        [Route("[action]/{formato}")]
        public async Task<ActionResult<List<ProductoView>>> ByFormato(string formato)
        {
            List<ProductoView> productos = await this.repo.GetProductoViewByFormatoAsync(formato);
            if (productos.Count==0)
            {
                return NotFound();
            }
            return productos;
        }
    }
}
