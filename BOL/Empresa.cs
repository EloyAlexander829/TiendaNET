using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Programas
using DAL;                    
using System.Data;            
using System.Data.SqlClient;  
using ENTITIES;

namespace BOL
{
    public class Empresa
    {
        //Conexión
        DBAccess conexion = new DBAccess();

        public DataTable iniciarSeción(string email)
        { 
            DataTable ep = new DataTable();
            conexion.abrirConexion();

            SqlCommand comando = new SqlCommand("spu_usuarios_login", conexion.GetConexion());

            comando.CommandType = CommandType.StoredProcedure;

            ep.Load(comando.ExecuteReader());

            //Cerrar
            conexion.cerrarConexion();

            //Retornar el objeto
            return ep;
        }

        public int Registrar(EEmpresa entidad)
        {
            int RegistroEmpresa = 0;
            SqlCommand comando = new SqlCommand("spu_empresas_registrar", conexion.GetConexion());
            comando.CommandType = CommandType.StoredProcedure;
            conexion.abrirConexion();

            try
            {
                comando.Parameters.AddWithValue("@razonSocial", entidad.razonSocial);
                comando.Parameters.AddWithValue("@ruc", entidad.ruc);
                comando.Parameters.AddWithValue("@direccion", entidad.direccion);
                comando.Parameters.AddWithValue("@telefono", entidad.telefono);
                comando.Parameters.AddWithValue("@email", entidad.email);
                comando.Parameters.AddWithValue("@website", entidad.website);

                RegistroEmpresa = comando.ExecuteNonQuery();
            }
            catch
            {
                RegistroEmpresa = -1;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return RegistroEmpresa;
        }

        public DataTable Listar()
        {
            DataTable ep = new DataTable();
            SqlCommand comando = new SqlCommand("spu_empresas_listar", conexion.GetConexion());
            comando.CommandType = CommandType.StoredProcedure;

            conexion.abrirConexion();
            ep.Load(comando.ExecuteReader());
            conexion.cerrarConexion();

            return ep;
        }
    }
}
