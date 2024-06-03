using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Views;
using Microsoft.EntityFrameworkCore;

namespace ApiAthanasia.Data
{
    public class AthanasiaContext : DbContext
    {
        public AthanasiaContext(DbContextOptions<AthanasiaContext> options) : base(options)
        {
        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Formato> Formatos { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<GenerosLibros> GenerosLibros { get; set; }
        public DbSet<InformacionCompra> InformacionesCompra { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<MetodoPago> MetodosPago { get; set; }
        public DbSet<Opinion> Opiniones { get; set; }
        public DbSet<OpinionesLibros> OpinionesLibros { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProducto> PedidosProductos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Saga> Sagas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Valoracion> Valoraciones { get; set; }
        public DbSet<ProductoView> ProductosView { get; set; }
        public DbSet<ProductoSimpleView> ProductosSimplesView { get; set; }
        public DbSet<FormatoLibroView> FormatosLibroView { get; set; }
        public DbSet<InformacionCompraView> InformacionesCompraView { get; set; }
        public DbSet<PedidoView> PedidosView { get; set; }
        public DbSet<PedidoProductoView> PedidosProductoView { get; set; }
    }
}
