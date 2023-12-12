using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BOL;
using DESIGNER.Tools;
using ENTITIES;

namespace DESIGNER.Modulos
{
    public partial class frmProductos : Form
    {
        Productos producto = new Productos();
        EProducto entproducto = new EProducto();
        Categoria categoria = new Categoria();
        Subcategoria subcategoria = new Subcategoria();
        Marca marca = new Marca();

        //Bandera = variable LÓGICA qu controla estados
        bool categoriasListas = false;

        public frmProductos()
        {
            InitializeComponent();
        }

        #region Método para carga de datos

        private void actualizarMarcas()
        {
            cboMarca.DataSource = marca.Listar();
            cboMarca.DisplayMember = "marca";   //Mostrar | Debe coincidir con la consulta
            cboMarca.ValueMember = "idmarca";   //pk (Guardar) | Debe coincidir con la consulta
            cboMarca.Refresh();
        }

        private void actualizarCategoria()
        {
            cboCategoria.DataSource = categoria.Listar();
            cboCategoria.DisplayMember = "categoria";   //Mostrar | Debe coincidir con la consulta
            cboCategoria.ValueMember = "idcategoria";   //pk (Guardar) | Debe coincidir con la consulta
            cboCategoria.Refresh();
            cboCategoria.Text = "";
        }

        #endregion

        private void actualizarProductos()
        {
            gridProductos.DataSource = producto.Listar();
            gridProductos.Refresh();
        }
        private void frmProductos_Load(object sender, EventArgs e)
        {
            actualizarProductos();
            actualizarMarcas();
            actualizarCategoria();

            gridProductos.Columns[0].Visible = false;
            gridProductos.Columns[1].Width = 130;
            gridProductos.Columns[2].Width = 130;
            gridProductos.Columns[3].Width = 150;
            gridProductos.Columns[4].Width = 240;
            gridProductos.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridProductos.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            gridProductos.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            categoriasListas = true;
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (categoriasListas)
            {
                //Invocar al método que carga las subcategorias
                int idcategoria = Convert.ToInt32(cboCategoria.SelectedValue.ToString());
                cboSubcategoria.DataSource = subcategoria.Listar(idcategoria);
                cboSubcategoria.DisplayMember = "subcategoria";
                cboSubcategoria.ValueMember = "idsubcategoria";
                cboSubcategoria.Refresh();
                cboSubcategoria.Text = "";
            }
            
        }
        private void reiniciarInterfaz()
        {
            cboMarca.Text = "";
            cboCategoria.Text = "";
            cboSubcategoria.Text = "";
            txtDescripcion.Clear();
            txtGarantía.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(Aviso.Preguntar("¿Quieres Registrar un nuevo producto?") == DialogResult.Yes){
                entproducto.idmarca = Convert.ToInt32(cboMarca.SelectedValue.ToString());
                entproducto.idsubcategoria = Convert.ToInt32(cboSubcategoria.SelectedValue.ToString());
                entproducto.descripcion = txtDescripcion.Text;
                entproducto.garantia = Convert.ToInt32(txtGarantía.Text);
                entproducto.precio = Convert.ToDouble(txtPrecio.Text);
                entproducto.stock = Convert.ToInt32(txtStock.Text);

                if (producto.Registrar(entproducto) > 0)
                {
                    reiniciarInterfaz();
                    actualizarProductos();
                }
            }
        }

        private void gridProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            reiniciarInterfaz();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            //1. Crear Instancia del reporte (CrystalRport)
            Reportes.rptProductos reporte = new Reportes.rptProductos();

            //2. Asignar los datos al objeto reporte (creado en el paso 1)
            reporte.SetDataSource(producto.Listar());
            reporte.Refresh();

            //3. Instanciar el formulario donde se mostrarán los reportes
            Reportes.frmVisorReportes formulario = new Reportes.frmVisorReportes();

            //4. Pasamos el reporte al visor
            formulario.visor.ReportSource = reporte;
            formulario.visor.Refresh();

            //5. Mostramos el formulario conteniendo el reporte
            formulario.ShowDialog();
        }

        private void btnExportarXLS_Click(object sender, EventArgs e)
        {
            ExportarDatos("XLSX");
        }

        /// <summary>
        /// Genera un archivo físico del reporte 
        /// </summary>
        /// <param name="extension">Utilice: XLS O PDF</param>
        
        private void ExportarDatos(string extension)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Title = "Reporte de Productos";
            sd.Filter = $"Archivos en formato {extension.ToUpper()} |*.{extension.ToLower()}";

            if (sd.ShowDialog() == DialogResult.OK)
            {
                //Creamos una versión del reporte en formato PDF
                //1. Instancia del objeto reporte (CrystalReport)
                Reportes.rptProductos reporte = new Reportes.rptProductos();

                //2. Asignar los datos al objeto reporte (creado en el paso 1)
                reporte.SetDataSource(producto.Listar());
                reporte.Refresh();

                //3. El reporte creado y en memoria se ESCRIBIRÁ en el STORAGE
                if (extension.ToUpper() == "PDF")
                {
                    reporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, sd.FileName);
                }
                else if (extension.ToUpper() == "XLSX")
                {
                    reporte.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook, sd.FileName);
                }

                //4. Notificar al usuario la creación del archivo 
                Aviso.Informar("Se ha creado el reporte correctamente");
            }
        }

        private void btnExportarPDF_Click(object sender, EventArgs e)
        {

        }

        private void btnExportarPDF_Click_1(object sender, EventArgs e)
        {
            ExportarDatos("PDF");
        }
    }
}
