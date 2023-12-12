using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Incluir librerías 
using CryptSharp;  //Claves encriptadas
using BOL;
using DESIGNER.Tools; //Traemos nuestras propias herramientas

namespace DESIGNER
{
    public partial class FrmLogin : Form
    {
        //Instancia de la clase
        Usuario usuario = new Usuario();
        DataTable dtRpta = new DataTable();

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }
        private void Login()
        {
            if (txtEmail.Text.Trim() == String.Empty)
            {
                errorLogin.SetError(txtEmail, "Por Favor Ingrese su email");
            }
            else
            {
                errorLogin.Clear();
                if (txtClaveAcceso.Text.Trim() == String.Empty)
                {
                    errorLogin.SetError(txtClaveAcceso, "Escriba su contraseña");
                    txtClaveAcceso.Focus();
                }
                else
                {
                    errorLogin.Clear();
                    //Los datos fueron ingreados, validamos el acceso
                    dtRpta = usuario.iniciarSesion(txtEmail.Text);

                    if (dtRpta.Rows.Count > 0)
                    {
                        string claveEncriptada = dtRpta.Rows[0][4].ToString();
                        String apellidos = dtRpta.Rows[0][1].ToString();
                        string nombres = dtRpta.Rows[0][2].ToString();

                        if (Crypter.CheckPassword(txtClaveAcceso.Text, claveEncriptada))
                        {
                            Aviso.Informar($"Bienvenido {apellidos} { nombres}");
                            frmMain formMain = new frmMain();
                            formMain.Show(); //Abre el formulario principal
                            this.Hide(); //Login se oculta
                        }
                        else
                        {
                            MessageBox.Show("Error en la contraseña");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El usuario ingresado no existe");
                    }
                }
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {

            //Sacar clave de acceso -> txtClaveAcceso.Text = Crypter.Blowfish.Crypt("SENATI123");
            //return;
            Login();
        }

        private void txtClaveAcceso_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Login();
            }
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAcercade_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Hola que hace");
        }
    }
}
