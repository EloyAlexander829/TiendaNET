using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using ENTITIES;
using DAL;

namespace BOL
{
    public class Productos
    {
        DBAccess conexion = new DBAccess();

        public DataTable Listar()
        {
            return conexion.ListarDatos("spu_productos_listar");
        }

        public int Registrar(EProducto entidad)
        {
            int totalRegistros = 0;
            conexion.abrirConexion();
            
            try
            {
                SqlCommand comando = new SqlCommand("spu_productos_Registrar", conexion.GetConexion());
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@idmarca", entidad.idmarca);
                comando.Parameters.AddWithValue("@idsubcategoria", entidad.idsubcategoria);
                comando.Parameters.AddWithValue("@descripcion", entidad.descripcion);
                comando.Parameters.AddWithValue("@garantia", entidad.garantia);
                comando.Parameters.AddWithValue("@precio", entidad.precio);
                comando.Parameters.AddWithValue("@stock", entidad.stock);

                totalRegistros = comando.ExecuteNonQuery();
            }
            catch
            {
                totalRegistros = -1;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return totalRegistros;
        }
    }
}
