using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

////////////////////////////////////////
using CryptSharp;        
using BOL;               
using ENTITIES;          
using DESIGNER.Tools;
namespace DESIGNER.Modulos
{
    public partial class frmEmpresa : Form
    {
        Empresa empresa = new Empresa();
        EEmpresa entEmpresa = new EEmpresa();
        DataView ep;
        public frmEmpresa()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            FormularioE();
        }
        private void actualizarDatos()
        {
            ep = new DataView(empresa.Listar());
            gridEmpresa.DataSource = ep;
            gridEmpresa.Refresh();

            gridEmpresa.Columns[0].Visible = false;  //id

            gridEmpresa.Columns[1].Width = 120;
            gridEmpresa.Columns[2].Width = 120;
            gridEmpresa.Columns[3].Width = 158;
            gridEmpresa.Columns[4].Width = 158;
            gridEmpresa.Columns[5].Width = 120;
            gridEmpresa.Columns[6].Width = 120;

            gridEmpresa.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkOliveGreen;
        }

        private void reiniciarInterfaz()
        {
            txtRazonS.Clear();
            txtRuc.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtWebS.Clear();
        }

        private void FormularioE()
        {
            int num = 0;
            if (txtRazonS.Text.Trim() == String.Empty)
            {
                errorForm.SetError(txtRazonS, "Por Favor Llene el campo Razon Social");
                num = 1;
            }

            if(txtRuc.Text.Trim() == String.Empty)
            {
                errorForm.SetError(txtRazonS, "Por Favor Llene el campo Ruc");
                num = 1;
            }

            if (txtDireccion.Text.Trim() == String.Empty)
            {
                //txtDireccion.Text == null; //Arreglarlo convertir valor vacio a null cuando lo guarde
            }

            if (num == 0)
            {
                if (Aviso.Preguntar("¿Estás Seguro que Deseas Guardar?") == DialogResult.Yes)
                {
                    entEmpresa.razonSocial = txtRazonS.Text;
                    entEmpresa.ruc = txtRuc.Text;
                    entEmpresa.email = txtEmail.Text;
                    entEmpresa.direccion = txtDireccion.Text;
                    entEmpresa.telefono = txtTelefono.Text;
                    entEmpresa.email = txtEmail.Text;
                    entEmpresa.website = txtWebS.Text;

                    if (empresa.Registrar(entEmpresa) > 0)
                    {
                        reiniciarInterfaz();
                        actualizarDatos();
                        Aviso.Informar("Nueva Empresa Registrada");
                    }
                    else
                    {
                        Aviso.Advertir("No hemos podido terminar el registro");
                    }
                }
            }
        }

        private void frmEmpresa_Load(object sender, EventArgs e)
        {
            actualizarDatos();
        }

        
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            ep.RowFilter = "razonSocial LIKE '%" + txtBuscar.Text + "%' OR ruc LIKE '%" + txtBuscar.Text + "%'";
        }

        private void gridEmpresa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
