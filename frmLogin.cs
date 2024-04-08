using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
namespace SharksTCGSV
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirFormularioVentas();
        }
        private void AbrirFormularioVentas()
        {
            // Verificar si las credenciales son válidas
            if (NLogin.VerificarCredenciales(txtUsuario.Text, txtContraseña.Text, out string Rol, out string Nombre))
            {
                // Cerrar el formulario actual
                this.Hide();
                string rol = Rol;
                string nombre = Nombre;
                // Abrir el formulario Ventas
                Ventas ventasForm = new Ventas(rol, nombre);
                ventasForm.Show();
            }
            else
            {
                MessageBox.Show("Credenciales inválidas. Intente nuevamente.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
