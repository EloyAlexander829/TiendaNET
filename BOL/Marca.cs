using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using DAL;

namespace BOL
{
    public class Marca
    {
        DBAccess conexion = new DBAccess();

        public DataTable Listar()
        {
            return conexion.ListarDatos("spu_marcas_listar");
        }
    }
}
