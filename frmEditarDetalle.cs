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
    public partial class frmEditarDetalle : Form
    {

        // Propiedades públicas para almacenar los IDs de producto y venta
        public int IDProducto { get; set; }
        public int IDVenta { get; set; }
        public int PrecioUnitario { get; set; }
        public int Saldo { get; set; }
        public int Descuento { get; set; }
        public int Total { get; set; }
        public int Cantidad { get; set; }

        private Ventas _ventasForm;
        public frmEditarDetalle(Ventas ventasForm)
        {
            InitializeComponent();
            _ventasForm = ventasForm; 
        }

        private void frmEditarDetalle_Load(object sender, EventArgs e)
        {
            txtIDProducto.Text = IDProducto.ToString();
            txtIDVenta.Text= IDVenta.ToString();
            txtCantidad.Text = Cantidad.ToString();
            txtPrecioUnitario.Text = PrecioUnitario.ToString();
            txtSaldo.Text = Saldo.ToString();
            txtDescuento.Text = Descuento.ToString();
            txtTotal.Text = Total.ToString();
        }


        private void btnActualizar_Click(object sender, EventArgs e)
        {
            ActualizarCampos();
            _ventasForm.cargarDatagrid();
        }
        private void ActualizarCampos()
        {
            // Obtener los valores de los campos de texto
            int idProducto = int.Parse(txtIDProducto.Text);
            int idVenta = int.Parse(txtIDVenta.Text);
            decimal precioUnitario = decimal.Parse(txtPrecioUnitario.Text);
            int cantidad = int.Parse(txtCantidad.Text);
            decimal saldo = decimal.Parse(txtSaldo.Text);
            decimal descuento = decimal.Parse(txtDescuento.Text);

            // Llamar al método de NVenta para actualizar el detalle
            decimal total;
            NVentas nVenta = new NVentas();
            nVenta.ActualizarDetalle(idProducto, idVenta, precioUnitario, cantidad, saldo, descuento, out total);

            // Actualizar el campo txtTotal con el valor devuelto
            txtTotal.Text = total.ToString();
        }
      

    }
}
