﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DAL
{
    public class DBAccess
    {
        //Objeto conexion
        private SqlConnection conexion = new SqlConnection(ConfigurationManager.ConnectionStrings["AccesoTiendaNET"].ConnectionString);

        //Métodos
        public SqlConnection GetConexion()
        {
            return this.conexion;
        }

        public void abrirConexion()
        {
            if (this.conexion.State == ConnectionState.Closed)
            {
                this.conexion.Open();
            }
        }

        public void cerrarConexion()
        {
            if (this.conexion.State == ConnectionState.Open)
            {
                this.conexion.Close();
            }
        }

        /// <summary>
        /// Método general que retorna una colección de datos de una consulta
        /// que no tiene variables de entrada
        /// </summary>
        /// <param name="spu">Nombre del procedimiento almacenado</param>
        /// <returns>
        /// Colección de datos de tipo DataTable
        /// </returns>
 
        public DataTable ListarDatos(string spu)
        {
            DataTable dt = new DataTable();
            this.abrirConexion();
            SqlCommand comando = new SqlCommand(spu, this.GetConexion());
            comando.CommandType = CommandType.StoredProcedure;
            dt.Load(comando.ExecuteReader());
            this.cerrarConexion();
            return dt;
        }

        public DataTable listarDatosVariable(string spu, string nombreVariable, object valorVariable)
        {
            DataTable dt = new DataTable();
            this.abrirConexion();
            SqlCommand comando = new SqlCommand(spu, this.GetConexion());
            comando.CommandType = CommandType.StoredProcedure;

            //Agregar parámetro de entrada (variable)
            comando.Parameters.AddWithValue(nombreVariable, valorVariable);

            dt.Load(comando.ExecuteReader());
            this.cerrarConexion();
            return dt;
        }
    }
}
