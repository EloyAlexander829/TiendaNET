using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CryptSharp;        //Enriptar
using BOL;               //Lógica
using ENTITIES;          //Estructura
using DESIGNER.Tools;    //Herramientas

namespace DESIGNER.Modulos
{
    public partial class frmUsuario : Form
    {
        //Objeto "Usuario" contiene las funciones/lógicas => Registrar, Listar, Eliminar, etc..
        Usuario usuario = new Usuario();

        //Objeto "entUsuario" contiene los datos => apellidos, nombres, email, clave, etc.
        EUsuario entUsuario = new EUsuario();

        string nivelAcceso = "INV";

        //Reservado un espacio de memoria para el objeto (Vista de Datos)
        DataView dv;

        public frmUsuario()
        {
            InitializeComponent();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (Aviso.Preguntar("¿Está seguro de guardar?") == DialogResult.Yes)
            {
                string claveEncriptada = Crypter.Blowfish.Crypt(txtClave.Text.Trim());

                entUsuario.apellidos = txtApellidos.Text;
                entUsuario.nombres = txtNombres.Text;
                entUsuario.email = txtEmail.Text;
                entUsuario.claveacceso = claveEncriptada;
                entUsuario.nivelAcceso = nivelAcceso;

                if (usuario.Registrar(entUsuario) > 0)
                {
                    reiniciarInterfaz();
                    actualizarDatos();   //Actualización DATAGRIDVIEW
                    Aviso.Informar("Nuevo Usuario Registrado");
                }
                else
                {
                    Aviso.Advertir("No hemos podido terminar el registro");
                }
            }
        }

        private void actualizarDatos()
        {
            dv = new DataView(usuario.Listar());
            gridUsuarios.DataSource = dv;
            gridUsuarios.Refresh();

            gridUsuarios.Columns[0].Visible = false; //ID
            gridUsuarios.Columns[4].Visible = false; //CLAVE

            gridUsuarios.Columns[1].Width = 235;    //Apellidos
            gridUsuarios.Columns[2].Width = 235;    //Nombres
            gridUsuarios.Columns[3].Width = 270;    //Email
            gridUsuarios.Columns[5].Width = 235;    //Nivel de acceso

            //Filas cebreadas (alternando)
            gridUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
        }

        private void reiniciarInterfaz()
        {
            txtApellidos.Clear();
            txtNombres.Clear();
            txtEmail.Clear();
            txtClave.Clear();
            optAdministrador.Checked = true;
            nivelAcceso = "INV";
        }

        private void optAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            nivelAcceso = "ADM";
        }

        private void optInvitado_CheckedChanged(object sender, EventArgs e)
        {
            nivelAcceso = "INV";
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            actualizarDatos();
        }

        private void gridUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void gridUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            gridUsuarios.ClearSelection();
        }

        private void txtBuscar_KeyUp(object sender, KeyEventArgs e)
        {
            dv.RowFilter = "nombres LIKE '%" + txtBuscar.Text + "%' OR apellidos LIKE '%" + txtBuscar.Text + "%'";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
