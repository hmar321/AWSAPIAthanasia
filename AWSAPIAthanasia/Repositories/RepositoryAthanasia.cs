using ApiAthanasia.Data;
using ApiAthanasia.Extension;
using ApiAthanasia.Helpers;
using ApiAthanasia.Models.Tables;
using ApiAthanasia.Models.Util;
using ApiAthanasia.Models.Views;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Metrics;

#region VIEWS
//CREATE VIEW `V_PEDIDO_PRODUCTO` AS
//SELECT
//  COALESCE(pp.ID_PEDIDO_PRODUCTO, -1) AS ID_PEDIDO_PRODUCTO,
//  l.ID_LIBRO,
//  l.TITULO,
//  l.PORTADA,
//  COALESCE(a.NOMBRE, 'Desconocido') AS AUTOR,
//  p.ID_FORMATO,
//  f.NOMBRE AS FORMATO,
//  pp.UNIDADES,
//  p.PRECIO,
//  pp.ID_PEDIDO,
//  pp.ID_PRODUCTO
//FROM PEDIDOS_PRODUCTOS pp
//INNER JOIN PRODUCTO p ON pp.ID_PRODUCTO = p.ID_PRODUCTO
//INNER JOIN FORMATO f ON p.ID_FORMATO = f.ID_FORMATO
//INNER JOIN LIBRO l ON p.ID_LIBRO = l.ID_LIBRO
//LEFT JOIN AUTOR a ON l.ID_AUTOR = a.ID_AUTOR;

//CREATE VIEW `V_PEDIDO` AS
//SELECT
//  p.ID_PEDIDO,
//  p.ID_USUARIO,
//  p.FECHA_SOLICITUD,
//  p.FECHA_ESTIMADA,
//  ep.NOMBRE AS ESTADO_PEDIDO,
//  GROUP_CONCAT(l.TITULO SEPARATOR ', ') AS LIBROS
//FROM PEDIDO p
//INNER JOIN ESTADO_PEDIDO ep ON p.ID_ESTADO_PEDIDO = ep.ID_ESTADO_PEDIDO
//INNER JOIN PEDIDOS_PRODUCTOS pp ON p.ID_PEDIDO = pp.ID_PEDIDO
//INNER JOIN PRODUCTO pr ON pp.ID_PRODUCTO = pr.ID_PRODUCTO
//INNER JOIN LIBRO l ON pr.ID_LIBRO = l.ID_LIBRO
//GROUP BY p.ID_PEDIDO, p.ID_USUARIO, p.FECHA_SOLICITUD, p.FECHA_ESTIMADA, ep.NOMBRE;

//CREATE VIEW `V_PRODUCTO` AS
//SELECT
//  COALESCE(p.ID_PRODUCTO, -1) AS ID_PRODUCTO,
//  l.ID_LIBRO,
//  l.TITULO,
//  l.SINOPSIS,
//  l.FECHA_PUBLICACION,
//  l.PORTADA,
//  c.NOMBRE AS CATEGORIA,
//  a.NOMBRE AS AUTOR,
//  GROUP_CONCAT(g.NOMBRE SEPARATOR ', ') AS GENEROS,
//  s.NOMBRE AS SAGA,
//  p.ISBN,
//  f.NOMBRE AS FORMATO,
//  p.PRECIO,
//  e.NOMBRE AS EDITORIAL,
//  e.LOGO
//FROM LIBRO l
//LEFT JOIN CATEGORIA c ON l.ID_CATEGORIA = c.ID_CATEGORIA AND l.TITULO IS NOT NULL
//LEFT JOIN AUTOR a ON l.ID_AUTOR = a.ID_AUTOR AND a.NOMBRE IS NOT NULL
//LEFT JOIN SAGA s ON s.ID_SAGA = l.ID_SAGA
//LEFT JOIN GENEROS_LIBROS gl ON gl.ID_LIBRO = l.ID_LIBRO
//LEFT JOIN GENERO g ON g.ID_GENERO = gl.ID_GENERO
//INNER JOIN PRODUCTO p ON l.ID_LIBRO = p.ID_LIBRO AND p.ISBN IS NOT NULL AND p.PRECIO IS NOT NULL
//LEFT JOIN EDITORIAL e ON e.ID_EDITORIAL = p.ID_EDITORIAL
//INNER JOIN FORMATO f ON f.ID_FORMATO = p.ID_FORMATO
//GROUP BY p.ID_PRODUCTO, l.ID_LIBRO, l.TITULO, c.NOMBRE, a.NOMBRE,
//         s.NOMBRE, l.SINOPSIS, l.FECHA_PUBLICACION, l.PORTADA,
//         p.ISBN, p.PRECIO, e.NOMBRE, e.LOGO;



//CREATE VIEW `V_PRODUCTO_SIMPLE` AS
//SELECT
//  COALESCE(p.ID_PRODUCTO, -1) AS ID_PRODUCTO,
//  l.ID_LIBRO,
//  l.TITULO,
//  l.PORTADA,
//  a.NOMBRE AS AUTOR,
//  p.PRECIO,
//  p.ID_FORMATO,
//  f.NOMBRE AS FORMATO,
//  1 AS UNIDADES  -- Cast to INT explicitly for consistency
//FROM Libro l
//LEFT JOIN AUTOR a ON l.ID_AUTOR = a.ID_AUTOR AND l.TITULO IS NOT NULL AND a.NOMBRE IS NOT NULL
//INNER JOIN PRODUCTO p ON l.ID_LIBRO = p.ID_LIBRO AND p.PRECIO IS NOT NULL
//INNER JOIN FORMATO f ON p.ID_FORMATO = f.ID_FORMATO AND f.NOMBRE IS NOT NULL;

//CREATE VIEW `V_PRODUCTO_BUSCADO` AS
//SELECT
//  COALESCE(p.ID_PRODUCTO, -1) AS ID_PRODUCTO,
//  l.TITULO,
//  l.ID_LIBRO,
//  l.PORTADA,
//  a.NOMBRE AS AUTOR,
//  p.PRECIO,
//  p.ID_FORMATO,
//  f.NOMBRE AS FORMATO,
//  G.ID_GENERO,
//  l.ID_CATEGORIA,
//  1 AS UNIDADES,
//  s.NOMBRE AS SAGA
//FROM Libro l
//LEFT JOIN AUTOR a ON l.ID_AUTOR = a.ID_AUTOR AND l.TITULO IS NOT NULL AND a.NOMBRE IS NOT NULL
//INNER JOIN PRODUCTO p ON l.ID_LIBRO = p.ID_LIBRO AND p.PRECIO IS NOT NULL
//INNER JOIN FORMATO f ON p.ID_FORMATO = f.ID_FORMATO AND f.NOMBRE IS NOT NULL
//INNER JOIN GENEROS_LIBROS gl ON l.ID_LIBRO = gl.ID_LIBRO  -- Changed to inner join for required data
//INNER JOIN GENERO g ON gl.ID_GENERO = g.ID_GENERO
//LEFT JOIN SAGA s ON l.ID_SAGA = s.ID_SAGA;

//CREATE VIEW `V_FORMATO_LIBRO` AS
//SELECT
//  COALESCE(p.ID_PRODUCTO, -1) AS ID_PRODUCTO,
//  p.ID_LIBRO,
//  f.NOMBRE AS FORMATO
//FROM PRODUCTO p
//INNER JOIN FORMATO f ON f.ID_FORMATO = p.ID_FORMATO;

//CREATE VIEW `V_INFORMACION_COMPRA_USUARIO` AS
//SELECT
//  COALESCE(ic.ID_INFORMACION_COMPRA, -1) AS ID_INFORMACION_COMPRA,
//  ic.NOMBRE,
//  ic.DIRECCION,
//  ic.INDICACIONES,
//  ic.ID_METODO_PAGO,
//  mp.NOMBRE AS METODO_PAGO,
//  ic.ID_USUARIO
//FROM INFORMACION_COMPRA ic
//INNER JOIN METODO_PAGO mp ON ic.ID_METODO_PAGO = mp.ID_METODO_PAGO;

#endregion

#region FUNCTIONS

//DELIMITER $$
//DROP FUNCTION if EXISTS LIMPIAR $$
//CREATE FUNCTION LIMPIAR(str NVARCHAR(255))
//RETURNS NVARCHAR(255)
//DETERMINISTIC
//BEGIN
//    SET str = REPLACE(str, 'á', 'a');
//SET str = REPLACE(str, 'é', 'e');
//SET str = REPLACE(str, 'í', 'i');
//SET str = REPLACE(str, 'ó', 'o');
//SET str = REPLACE(str, 'ú', 'u');
//SET str = REPLACE(str, 'Á', 'A');
//SET str = REPLACE(str, 'É', 'E');
//SET str = REPLACE(str, 'Í', 'I');
//SET str = REPLACE(str, 'Ó', 'O');
//SET str = REPLACE(str, 'Ú', 'U');
//SET str = REPLACE(str, '&', '');
//SET str = REPLACE(str, '-', '');
//SET str = REPLACE(str, '_', '');
//SET str = REPLACE(str, '+', '');
//SET str = REPLACE(str, '"', '');
//SET str = REPLACE(str, '''', '');
//SET str = REPLACE(str, ',', '');
//SET str = REPLACE(str, '.', '');
//SET str = REPLACE(str, '?', '');
//SET str = REPLACE(str, '`', '');
//SET str = REPLACE(str, '!', '');
//SET str = REPLACE(str, '¡', '');
//SET str = REPLACE(str, '¿', '');
//SET str = REPLACE(str, 'ñ', 'n');
//SET str = REPLACE(str, 'Ñ', 'N');
//SET str = REPLACE(str, 'ü', 'u');
//SET str = REPLACE(str, 'Ü', 'U');
//SET str = REPLACE(str, ' ', '');
//SET str = UPPER(str);
//RETURN str;
//END $$
//DELIMITER ;

#endregion

#region PROCEDURES

//DELIMITER $$
//DROP PROCEDURE IF EXISTS SP_GENEROS $$
//CREATE PROCEDURE SP_GENEROS()
//BEGIN
//    SELECT DISTINCT g.ID_GENERO, g.NOMBRE, g.DESCRIPCION
//    FROM GENERO g
//    INNER JOIN GENEROS_LIBROS gl ON gl.ID_GENERO = g.ID_GENERO
//    GROUP BY g.ID_GENERO, g.NOMBRE, g.DESCRIPCION
//    ORDER BY g.NOMBRE;
//END $$

//DELIMITER ;

//DELIMITER $$
//DROP PROCEDURE IF EXISTS SP_PRODUCTO_SIMPLE_PAGINACION $$
//CREATE PROCEDURE SP_PRODUCTO_SIMPLE_PAGINACION(
//    IN param_posicion INT,
//    IN param_ndatos INT,
//    OUT param_npaginas INT
//)
//BEGIN
//    SELECT CEILING(COUNT(ID_PRODUCTO) / param_ndatos) INTO param_npaginas
//    FROM (
//        SELECT ID_PRODUCTO, ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//        FROM V_PRODUCTO_SIMPLE
//    ) AS AGRUPADOS
//    WHERE REPETICION = 1;
//SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES
//    FROM (
//        SELECT
//            ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES,
//        ROW_NUMBER() OVER (ORDER BY ID_PRODUCTO) AS POSICION
//        FROM (
//            SELECT
//                ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES,
//            ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//            FROM V_PRODUCTO_SIMPLE
//        ) AS QUERY
//        WHERE REPETICION = 1
//    ) AS PRIMEROS
//    WHERE POSICION BETWEEN (param_posicion - 1) * param_ndatos + 1 AND param_posicion * param_ndatos
//    ORDER BY ID_PRODUCTO;
//END $$
//DELIMITER ;

//DELIMITER $$
//DROP PROCEDURE IF EXISTS SP_PRODUCTOS $$
//CREATE PROCEDURE SP_PRODUCTOS()
//BEGIN
//    SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES
//    FROM (
//        SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES,
//               ROW_NUMBER() OVER (PARTITION BY TITULO ORDER BY ID_PRODUCTO) AS REPETICION
//        FROM V_PRODUCTO_SIMPLE
//    ) AS PRODUCTOS
//    WHERE REPETICION = 1
//      AND TITULO IS NOT NULL
//      AND AUTOR IS NOT NULL
//    ORDER BY ID_PRODUCTO;
//END $$
//DELIMITER ;

//DELIMITER $$
//DROP PROCEDURE IF EXISTS SP_SEARCH_PRODUCTOS $$
//CREATE PROCEDURE SP_SEARCH_PRODUCTOS(
//    IN param_busqueda NVARCHAR(255),
//    IN param_posicion INT,
//    IN param_ndatos INT,
//    OUT param_npaginas INT
//)
//BEGIN
//    SELECT CEILING(COUNT(ID_PRODUCTO) / param_ndatos) INTO param_npaginas
//    FROM (
//        SELECT ID_PRODUCTO, ID_LIBRO, TITULO, AUTOR, SAGA,
//               ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//        FROM (
//            SELECT DISTINCT ID_PRODUCTO, ID_LIBRO, TITULO, AUTOR, SAGA
//            FROM V_PRODUCTO_BUSCADO
//        ) AS DISTINTOS
//    ) AS AGRUPADOS
//    WHERE REPETICION = 1
//      AND TITULO IS NOT NULL
//      AND AUTOR IS NOT NULL
//      AND (LIMPIAR(TITULO) LIKE LIMPIAR(param_busqueda)
//           OR LIMPIAR(AUTOR) LIKE LIMPIAR(param_busqueda)
//           OR LIMPIAR(SAGA) LIKE LIMPIAR(param_busqueda));
//SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES
//    FROM (
//        SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES,
//           ROW_NUMBER() OVER (ORDER BY ID_PRODUCTO) AS POSICION
//        FROM (
//            SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES, SAGA,
//               ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//            FROM (
//                SELECT DISTINCT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES, SAGA
//                FROM V_PRODUCTO_BUSCADO
//            ) AS QUERY
//        ) AS GRUPO
//        WHERE REPETICION = 1
//          AND TITULO IS NOT NULL
//          AND AUTOR IS NOT NULL
//          AND (LIMPIAR(TITULO) LIKE LIMPIAR(param_busqueda)
//               OR LIMPIAR(AUTOR) LIKE LIMPIAR(param_busqueda)
//               OR LIMPIAR(SAGA) LIKE LIMPIAR(param_busqueda))
//    ) AS QUERY
//    WHERE POSICION BETWEEN (param_posicion - 1) * param_ndatos + 1 AND param_posicion * param_ndatos
//    ORDER BY ID_PRODUCTO;
//END $$
//DELIMITER ;

//DELIMITER $$
//DROP PROCEDURE IF EXISTS SP_SEARCH_PRODUCTOS_FILTRO $$
//CREATE PROCEDURE SP_SEARCH_PRODUCTOS_FILTRO(
//    IN param_busqueda NVARCHAR(255),
//    IN param_posicion INT,
//    IN param_ndatos INT,
//    IN param_categorias VARCHAR(255),
//    IN param_generos VARCHAR(255),
//    OUT param_npaginas INT
//)
//BEGIN
//    SELECT CEILING(COUNT(ID_PRODUCTO) / param_ndatos) INTO param_npaginas
//    FROM (
//        SELECT ID_PRODUCTO, ID_LIBRO, TITULO, AUTOR, SAGA,
//               ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//        FROM (
//            SELECT DISTINCT ID_PRODUCTO, ID_LIBRO, TITULO, AUTOR, SAGA
//            FROM V_PRODUCTO_BUSCADO
//            WHERE FIND_IN_SET(ID_CATEGORIA, REPLACE(param_categorias, ' ', '')) > 0
//				AND FIND_IN_SET(ID_GENERO, REPLACE(param_generos, ' ', '')) > 0
//        ) AS DISTINTOS
//    ) AS AGRUPADOS
//    WHERE REPETICION = 1
//      AND TITULO IS NOT NULL
//      AND AUTOR IS NOT NULL
//      AND (LIMPIAR(TITULO) LIKE LIMPIAR(param_busqueda)
//           OR LIMPIAR(AUTOR) LIKE LIMPIAR(param_busqueda)
//           OR LIMPIAR(SAGA) LIKE LIMPIAR(param_busqueda));
//SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES
//    FROM (
//        SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES,
//           ROW_NUMBER() OVER (ORDER BY ID_PRODUCTO) AS POSICION
//        FROM (
//            SELECT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES, SAGA,
//               ROW_NUMBER() OVER (PARTITION BY TITULO, AUTOR ORDER BY ID_PRODUCTO) AS REPETICION
//            FROM (
//                SELECT DISTINCT ID_PRODUCTO, ID_LIBRO, TITULO, PORTADA, AUTOR, PRECIO, ID_FORMATO, FORMATO, UNIDADES, SAGA
//                FROM V_PRODUCTO_BUSCADO
//                WHERE FIND_IN_SET(ID_CATEGORIA, REPLACE(param_categorias, ' ', '')) > 0
//					 AND FIND_IN_SET(ID_GENERO, REPLACE(param_generos, ' ', '')) > 0
//            ) AS QUERY
//        ) AS GRUPO
//        WHERE REPETICION = 1
//          AND TITULO IS NOT NULL
//          AND AUTOR IS NOT NULL
//          AND (LIMPIAR(TITULO) LIKE LIMPIAR(param_busqueda)
//               OR LIMPIAR(AUTOR) LIKE LIMPIAR(param_busqueda)
//               OR LIMPIAR(SAGA) LIKE LIMPIAR(param_busqueda))
//    ) AS QUERY
//    WHERE POSICION BETWEEN (param_posicion - 1) * param_ndatos + 1 AND param_posicion * param_ndatos
//    ORDER BY ID_PRODUCTO;
//END $$
//DELIMITER ;
#endregion

namespace ApiAthanasia.Repositories
{
    public class RepositoryAthanasia : IRepositoryAthanasia
    {
        private AthanasiaContext context;

        public RepositoryAthanasia(AthanasiaContext context)
        {
            this.context = context;
        }


        #region PRODUCTO_VIEW
        public async Task<List<ProductoView>> GetAllProductoViewAsync()
        {
            List<ProductoView> productos = await this.context.ProductosView.ToListAsync();
            return productos;
        }

        public async Task<List<ProductoView>> GetProductoViewByFormatoAsync(string formato)
        {
            List<ProductoView> productos = await this.context.ProductosView.Where(o => o.Formato == formato).ToListAsync();
            return productos;
        }

        public async Task<ProductoView> GetProductoByIdAsync(int idproducto)
        {
            ProductoView producto = await this.context.ProductosView.FirstOrDefaultAsync(p => p.IdProducto == idproducto);
            return producto;
        }
        #endregion

        #region PRODUCTO_SIMPLE_VIEW

        public async Task<ProductoSimpleView> GetProductoSimpleByIdAsync(int idproducto)
        {
            ProductoSimpleView producto = await this.context.ProductosSimplesView.FirstOrDefaultAsync(p => p.IdProducto == idproducto);
            return producto;
        }

        public async Task<List<ProductoSimpleView>> GetAllProductoSimpleViewAsync()
        {
            List<ProductoSimpleView> productosSimples = await this.context.ProductosSimplesView.ToListAsync();
            return productosSimples;
        }

        public async Task<List<ProductoSimpleView>> GetProductoSimpleViewTituloAutorAsync()
        {
            string sql = "CALL SP_PRODUCTOS();";
            var consulta = this.context.ProductosSimplesView.FromSqlRaw(sql);
            return await consulta.ToListAsync();
        }
        public async Task<PaginacionModel<ProductoSimpleView>> GetProductosSimplesPaginacionAsyn(int posicion, int ndatos)
        {
            var productos = new List<ProductoSimpleView>();
            int numeroPaginas = 0;

            using (DbConnection connection = context.Database.GetDbConnection())
            {
                using (DbCommand com = connection.CreateCommand())
                {
                    string sql = "SP_PRODUCTO_SIMPLE_PAGINACION";
                    com.CommandText = sql;
                    com.CommandType = CommandType.StoredProcedure;
                    MySqlParameter paramposicion = new MySqlParameter("@param_posicion", posicion);
                    MySqlParameter paramndatos = new MySqlParameter("@param_ndatos", ndatos);
                    MySqlParameter paramnpaginas = new MySqlParameter("@param_npaginas", -1);
                    paramnpaginas.MySqlDbType = MySqlDbType.Int32;
                    paramnpaginas.Direction = ParameterDirection.Output;
                    com.Parameters.Add(paramposicion);
                    com.Parameters.Add(paramndatos);
                    com.Parameters.Add(paramnpaginas);
                    await connection.OpenAsync();
                    using (DbDataReader reader = await com.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var producto = new ProductoSimpleView
                            {
                                IdProducto = int.Parse(reader["ID_PRODUCTO"].ToString()),
                                IdLibro = int.Parse(reader["ID_LIBRO"].ToString()),
                                Titulo = reader["TITULO"].ToString(),
                                Portada = reader["PORTADA"].ToString(),
                                Autor = reader["AUTOR"].ToString(),
                                Precio = double.Parse(reader["PRECIO"].ToString()),
                                IdFormato = int.Parse(reader["ID_FORMATO"].ToString()),
                                Formato = reader["FORMATO"].ToString(),
                                Unidades = int.Parse(reader["UNIDADES"].ToString())
                            };
                            productos.Add(producto);
                        }
                        await reader.CloseAsync();
                        numeroPaginas = (int)paramnpaginas.Value;
                        com.Parameters.Clear();
                        await connection.CloseAsync();
                    }
                }
            }
            PaginacionModel<ProductoSimpleView> model = new PaginacionModel<ProductoSimpleView>
            {
                Lista = productos,
                NumeroPaginas = numeroPaginas
            };
            return model;
        }

        public async Task<PaginacionModel<ProductoSimpleView>> GetAllProductoSimpleViewSearchPaginacionAsync(string palabra, int posicion, int ndatos)
        {
            if (palabra == null)
            {
                palabra = "";
            }
            string busqueda = palabra.Limpiar();
            var productos = new List<ProductoSimpleView>();
            int numeroPaginas = 0;

            using (DbConnection connection = context.Database.GetDbConnection())
            {
                using (DbCommand com = connection.CreateCommand())
                {
                    string sql = "SP_SEARCH_PRODUCTOS";
                    com.CommandText = sql;
                    com.CommandType = CommandType.StoredProcedure;
                    MySqlParameter parambusqueda = new MySqlParameter("@param_busqueda", "%" + busqueda + "%");
                    MySqlParameter paramposicion = new MySqlParameter("@param_posicion", posicion);
                    MySqlParameter paramndatos = new MySqlParameter("@param_ndatos", ndatos);
                    MySqlParameter paramnpaginas = new MySqlParameter("@param_npaginas", -1);
                    paramnpaginas.MySqlDbType = MySqlDbType.Int32;
                    paramnpaginas.Direction = ParameterDirection.Output;
                    com.Parameters.Add(parambusqueda);
                    com.Parameters.Add(paramposicion);
                    com.Parameters.Add(paramndatos);
                    com.Parameters.Add(paramnpaginas);
                    await connection.OpenAsync();
                    using (DbDataReader reader = await com.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var producto = new ProductoSimpleView
                            {
                                IdProducto = int.Parse(reader["ID_PRODUCTO"].ToString()),
                                IdLibro = int.Parse(reader["ID_LIBRO"].ToString()),
                                Titulo = reader["TITULO"].ToString(),
                                Portada = reader["PORTADA"].ToString(),
                                Autor = reader["AUTOR"].ToString(),
                                Precio = double.Parse(reader["PRECIO"].ToString()),
                                IdFormato = int.Parse(reader["ID_FORMATO"].ToString()),
                                Formato = reader["FORMATO"].ToString(),
                                Unidades = int.Parse(reader["UNIDADES"].ToString())
                            };
                            productos.Add(producto);
                        }
                        await reader.CloseAsync();
                        numeroPaginas = (int)paramnpaginas.Value;
                        com.Parameters.Clear();
                        await connection.CloseAsync();
                    }
                }
            }
            PaginacionModel<ProductoSimpleView> model = new PaginacionModel<ProductoSimpleView>
            {
                Lista = productos,
                NumeroPaginas = numeroPaginas
            };
            return model;

        }

        public async Task<List<ProductoSimpleView>> GetAllProductoSimpleViewByIds(List<int> idsproductos)
        {
            if (idsproductos == null || idsproductos.Count == 0)
            {
                return null;
            }
            else
            {
                var consulta = from datos in this.context.ProductosSimplesView
                               where idsproductos.Contains(datos.IdProducto)
                               select datos;
                return await consulta.ToListAsync();
            }
        }

        public async Task<PaginacionModel<ProductoSimpleView>> GetProductoSimpleViewsCategoriasGeneroAsync(string palabra, int posicion, int ndatos, List<int> idscategorias, List<int> idsgeneros)
        {
            string categorias = String.Join(",", idscategorias);
            string generos = String.Join(",", idsgeneros);
            if (palabra == null)
            {
                palabra = "";
            }
            string busqueda = palabra.Limpiar();
            var productos = new List<ProductoSimpleView>();
            int numeroPaginas = 0;

            using (DbConnection connection = context.Database.GetDbConnection())
            {
                using (DbCommand com = connection.CreateCommand())
                {
                    string sql = "SP_SEARCH_PRODUCTOS_FILTRO";
                    com.CommandText = sql;
                    com.CommandType = CommandType.StoredProcedure;
                    MySqlParameter parambusqueda = new MySqlParameter("@param_busqueda", "%" + busqueda + "%");
                    MySqlParameter paramposicion = new MySqlParameter("@param_posicion", posicion);
                    MySqlParameter paramndatos = new MySqlParameter("@param_ndatos", ndatos);
                    MySqlParameter paramcategorias = new MySqlParameter("@param_categorias", categorias);
                    MySqlParameter paramgeneros = new MySqlParameter("@param_generos", generos);
                    MySqlParameter paramnpaginas = new MySqlParameter("@param_npaginas", -1);
                    paramnpaginas.MySqlDbType = MySqlDbType.Int32;
                    paramnpaginas.Direction = ParameterDirection.Output;
                    com.Parameters.Add(parambusqueda);
                    com.Parameters.Add(paramposicion);
                    com.Parameters.Add(paramndatos);
                    com.Parameters.Add(paramcategorias);
                    com.Parameters.Add(paramgeneros);
                    com.Parameters.Add(paramnpaginas);
                    await connection.OpenAsync();
                    using (DbDataReader reader = await com.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var producto = new ProductoSimpleView
                            {
                                IdProducto = int.Parse(reader["ID_PRODUCTO"].ToString()),
                                IdLibro = int.Parse(reader["ID_LIBRO"].ToString()),
                                Titulo = reader["TITULO"].ToString(),
                                Portada = reader["PORTADA"].ToString(),
                                Autor = reader["AUTOR"].ToString(),
                                Precio = double.Parse(reader["PRECIO"].ToString()),
                                IdFormato = int.Parse(reader["ID_FORMATO"].ToString()),
                                Formato = reader["FORMATO"].ToString(),
                                Unidades = int.Parse(reader["UNIDADES"].ToString())
                            };
                            productos.Add(producto);
                        }
                        await reader.CloseAsync();
                        numeroPaginas = (int)paramnpaginas.Value;
                        com.Parameters.Clear();
                        await connection.CloseAsync();
                    }
                }
            }
            PaginacionModel<ProductoSimpleView> model = new PaginacionModel<ProductoSimpleView>
            {
                Lista = productos,
                NumeroPaginas = numeroPaginas
            };
            return model;
        }


        #endregion

        #region USUARIO
        public async Task<Usuario> FindUsuarioByIdAsync(int idusuario)
        {
            return await this.context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idusuario);
        }

        public async Task<Usuario> UpdateUsuarioAsync(int idusuario, string nombre, string apellido, string email, string? imagen)
        {
            Usuario usuario = await this.FindUsuarioByIdAsync(idusuario);
            if (usuario == null)
            {
                return null;
            }
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Email = email;
            if (imagen != null)
            {
                usuario.Imagen = imagen;
            }
            await this.context.SaveChangesAsync();
            return usuario;
        }

        public async Task<int> DeleteUsuarioAsync(int idusuario)
        {
            Usuario usuario = await this.FindUsuarioByIdAsync(idusuario);
            if (usuario != null)
            {
                this.context.Usuarios.Remove(usuario);
            }
            return await this.context.SaveChangesAsync();
        }

        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(u => u.IdUsuario) + 1;
            }
        }

        public async Task<Usuario> RegistrarUsuarioAsync(string nombre, string apellido, string email, string password)
        {
            Usuario usuario = new Usuario();
            usuario.IdUsuario = await this.GetMaxIdUsuarioAsync();
            usuario.Nombre = nombre;
            usuario.Apellido = apellido;
            usuario.Email = email;
            usuario.Password = password;
            usuario.Imagen = "usuario.png";
            usuario.Salt = HelperTools.GenerateSalt();
            usuario.Pass = HelperCryptography.EncryptPassword(password, usuario.Salt);
            usuario.IdEstado = HelperTools.GetEstadoId(Estados.Pendiente);
            usuario.Token = HelperTools.GenerateTokenMail();
            usuario.IdRol = HelperTools.GetRolId(Roles.Cliente);
            this.context.Usuarios.Add(usuario);
            int result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return null;
            }
            return usuario;
        }

        public async Task<Usuario> ActivarUsuarioAsync(string token)
        {
            Usuario user = await this.context.Usuarios.FirstOrDefaultAsync(u => u.Token == token);
            if (user == null)
            {
                return null;
            }
            user.IdEstado = HelperTools.GetEstadoId(Estados.Activo);
            user.Token = "";
            await this.context.SaveChangesAsync();
            return user;
        }
        public async Task<Usuario> LogInUserAsync(string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                string salt = usuario.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                byte[] passUser = usuario.Pass;
                bool respuesta = HelperTools.CompareArrays(temp, passUser);
                if (respuesta == true)
                {
                    return usuario;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<Usuario> UpdateUsuarioTokenAsync(int idusuario)
        {
            Usuario usuario = await this.FindUsuarioByIdAsync(idusuario);
            usuario.Token = HelperTools.GenerateTokenMail();
            await this.context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> GetUsuarioByTokenAsync(string token)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(u => u.Token == token);
            if (usuario != null)
            {
                usuario.Token = "";
                await this.context.SaveChangesAsync();
            }
            return usuario;
        }


        public async Task<Usuario> UpdateUsuarioPasswordAsync(int idusuario, string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idusuario);
            if (usuario == null)
            {
                return null;
            }
            usuario.Salt = HelperTools.GenerateSalt();
            usuario.Pass = HelperCryptography.EncryptPassword(password, usuario.Salt);
            await this.context.SaveChangesAsync();
            return usuario;
        }

        #endregion

        #region PEDIDO_PRODUCTO

        public int GetPedidoProductoNextId()
        {
            if (this.context.PedidosProductos.Count() == 0)
            {
                return 1;
            }
            else
            {
                return this.context.PedidosProductos.Max(pp => pp.IdPedidoProducto) + 1;

            }

        }

        public async Task<int> InsertListPedidoProductosAsync(int idusuario, List<PedidoProducto> productos)
        {
            Pedido pedido = await this.InsertPedidoAsync(idusuario);
            int nextId = GetPedidoProductoNextId();
            foreach (PedidoProducto prod in productos)
            {
                prod.IdPedidoProducto = nextId;
                prod.IdPedido = pedido.IdPedido;
                await this.context.PedidosProductos.AddAsync(prod);
                nextId++;
            }
            return await this.context.SaveChangesAsync();
        }

        #endregion

        #region CATEGORIAS

        public async Task<List<Categoria>> GetAllCategoriasAsync()
        {
            return await this.context.Categorias.ToListAsync();
        }

        #endregion

        #region GENEROS

        public async Task<List<Genero>> GetAllGenerosAsync()
        {
            string sql = "CALL SP_GENEROS();";
            return await this.context.Generos.FromSqlRaw(sql).ToListAsync();
        }

        public async Task<Genero> GetGeneroByNombreAsync(string nombre)
        {
            return await this.context.Generos.FirstOrDefaultAsync(g => g.Nombre == nombre);
        }
        #endregion

        #region FORMATOS_LIBRO_VIEW

        public async Task<List<FormatoLibroView>> GetAllFormatoLibroViewByIdLibroAsync(int idlibro)
        {
            return await this.context.FormatosLibroView.Where(f => f.IdLibro == idlibro).ToListAsync();
        }

        #endregion

        #region INFORMACION_COMPRA

        public async Task<List<InformacionCompra>> GetAllInformacionComprabyIdUsuarioAsync(int idusuario)
        {
            List<InformacionCompra> info = await this.context.InformacionesCompra.Where(ic => ic.IdUsuario == idusuario).ToListAsync();
            if (info.Count == 0)
            {
                return null;
            }
            return info;
        }

        public async Task<int> GetNextIdInformacionCompraAsync()
        {
            if (this.context.InformacionesCompra.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.InformacionesCompra.MaxAsync(u => u.IdInformacionCompra) + 1;
            }
        }

        public async Task<InformacionCompra> InsertInformacionAsync(string nombre, string direccion, string indicaciones, int idmetodopago, int idusuario)
        {
            int nextid = await this.GetNextIdInformacionCompraAsync();
            InformacionCompra info = new InformacionCompra
            {
                IdInformacionCompra = nextid,
                Nombre = nombre,
                Direccion = direccion,
                Indicaciones = indicaciones,
                IdMetodoPago = idmetodopago,
                IdUsuario = idusuario
            };
            await this.context.InformacionesCompra.AddAsync(info);
            await this.context.SaveChangesAsync();
            return info;
        }

        public async Task<InformacionCompra> GetInformacionCompraByIdAsync(int id)
        {
            return this.context.InformacionesCompra.FirstOrDefault(ic => ic.IdInformacionCompra == id);
        }

        public async Task<int> DeleteInformacionCompraByIdAsync(int idinfocompra)
        {
            InformacionCompra info = await this.GetInformacionCompraByIdAsync(idinfocompra);
            if (info != null)
            {
                this.context.Remove(info);
            }
            return await this.context.SaveChangesAsync();
        }

        #endregion

        #region METODO_PAGO

        public async Task<List<MetodoPago>> GetMetodoPagosAsync()
        {
            return await this.context.MetodosPago.ToListAsync();
        }

        #endregion

        #region INFORMACION_COMPRA_VIEW

        public async Task<List<InformacionCompraView>> GetAllInformacionCompraViewByIdUsuarioAsync(int idusuario)
        {
            return await this.context.InformacionesCompraView.Where(dp => dp.IdUsuario == idusuario).ToListAsync();
        }
        #endregion

        #region PEDIDO
        public async Task<int> UpdatePedidoEstadoCancelarAsync(int idpedido)
        {
            Pedido pedido = await this.context.Pedidos.FirstOrDefaultAsync(p => p.IdPedido == idpedido);
            if (pedido == null)
            {
                return -1;
            }
            //ID DEL ESTADO CANCELADO
            pedido.IdEstadoPedido = 7;
            return await this.context.SaveChangesAsync();
        }

        public int GetPedidoNextId()
        {
            int nextid = -1;
            if (this.context.Pedidos.Count() == 0)
            {
                nextid = 1;
            }
            else
            {
                nextid = this.context.Pedidos.Max(o => o.IdPedido) + 1;
            }
            return nextid;
        }
        public async Task<Pedido> InsertPedidoAsync(int idusuario)
        {
            int estadopedido = 2;
            int nextid = GetPedidoNextId();
            Pedido pedido = new Pedido
            {
                IdPedido = nextid,
                FechaSolicitud = DateTime.Now,
                FechaEstimada = DateTime.Now.AddDays(3),
                FechaEntrega = null,
                IdEstadoPedido = estadopedido,
                IdUsuario = idusuario
            };
            await this.context.Pedidos.AddAsync(pedido);
            await this.context.SaveChangesAsync();
            return pedido;
        }

        #endregion

        #region PEDIDO_VIEW

        public async Task<List<PedidoView>> GetAllPedidoViewByIdUsuario(int idusuario)
        {
            return await this.context.PedidosView.Where(p => p.IdUsuario == idusuario).ToListAsync();
        }

        #endregion

        #region PEDIDO_PRODUCTOS_VIEW
        public async Task<List<PedidoProductoView>> GetPedidoProductoViewsByIdPedidoAsync(int idpedido)
        {
            return await this.context.PedidosProductoView.Where(p => p.IdPedido == idpedido).ToListAsync();
        }

        #endregion

        #region FORMATO
        public async Task<List<Formato>> GetAllFormatosAsync()
        {
            return await this.context.Formatos.ToListAsync();
        }
        public async Task<Formato> GetFormatoByIdAsync(int idformato)
        {
            return await this.context.Formatos.FirstOrDefaultAsync(f => f.IdFormato == idformato);
        }

        #endregion

    }
}
