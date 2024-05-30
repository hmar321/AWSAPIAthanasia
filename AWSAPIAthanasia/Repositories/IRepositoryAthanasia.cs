using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Util;
using ApiAthanasia.Models.Views;

namespace ApiAthanasia.Repositories
{
    public interface IRepositoryAthanasia
    {
        Task<Usuario> ActivarUsuarioAsync(string token);
        Task<int> DeleteInformacionCompraByIdAsync(int idinfocompra);
        Task<int> DeleteUsuarioAsync(int idusuario);
        Task<Usuario> FindUsuarioByIdAsync(int idusuario);
        Task<List<Categoria>> GetAllCategoriasAsync();
        Task<List<FormatoLibroView>> GetAllFormatoLibroViewByIdLibroAsync(int idlibro);
        Task<List<Genero>> GetAllGenerosAsync();
        Task<List<InformacionCompra>> GetAllInformacionComprabyIdUsuarioAsync(int idusuario);
        Task<List<InformacionCompraView>> GetAllInformacionCompraViewByIdUsuarioAsync(int idusuario);
        Task<List<PedidoView>> GetAllPedidoViewByIdUsuario(int idusuario);
        Task<List<ProductoSimpleView>> GetAllProductoSimpleViewAsync();
        Task<List<ProductoSimpleView>> GetAllProductoSimpleViewByIds(List<int> idsproductos);
        Task<PaginacionModel<ProductoSimpleView>> GetAllProductoSimpleViewSearchPaginacionAsync(string palabra, int posicion, int ndatos);
        Task<List<ProductoView>> GetAllProductoViewAsync();
        Task<Genero> GetGeneroByNombreAsync(string nombre);
        Task<InformacionCompra> GetInformacionCompraByIdAsync(int id);
        Task<List<MetodoPago>> GetMetodoPagosAsync();
        Task<List<PedidoProductoView>> GetPedidoProductoViewsByIdPedidoAsync(int idpedido);
        Task<ProductoView> GetProductoByIdAsync(int idproducto);
        Task<ProductoSimpleView> GetProductoSimpleByIdAsync(int idproducto);
        Task<PaginacionModel<ProductoSimpleView>> GetProductoSimpleViewsCategoriasGeneroAsync(string palabra, int posicion, int ndatos, List<int> idscategorias, List<int> idsgeneros);
        Task<List<ProductoSimpleView>> GetProductoSimpleViewTituloAutorAsync();
        Task<PaginacionModel<ProductoSimpleView>> GetProductosSimplesPaginacionAsyn(int posicion, int ndatos);
        Task<List<ProductoView>> GetProductoViewByFormatoAsync(string formato);
        Task<Usuario> GetUsuarioByTokenAsync(string token);
        Task<InformacionCompra> InsertInformacionAsync(string nombre, string direccion, string indicaciones, int idmetodopago, int idusuario);
        Task<int> InsertListPedidoProductosAsync(int idusuario, List<PedidoProducto> productos);
        Task<Pedido> InsertPedidoAsync(int idusuario);
        Task<Usuario> LogInUserAsync(string email, string password);
        Task<Usuario> RegistrarUsuarioAsync(string nombre, string apellido, string email, string password);
        Task<int> UpdatePedidoEstadoCancelarAsync(int idpedido);
        Task<Usuario> UpdateUsuarioAsync(int idusuario, string nombre, string apellido, string email, string? imagen);
        Task<Usuario> UpdateUsuarioPasswordAsync(int idusuario, string password);
        Task<Usuario> UpdateUsuarioTokenAsync(int idusuario);
        Task<List<Formato>> GetAllFormatosAsync();
        Task<Formato> GetFormatoByIdAsync(int idformato);
    }
}