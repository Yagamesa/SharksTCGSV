using CapaNegocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharksTCGSV
{
    public partial class Ventas : Form
    {

        public string nombre { get; set; }
        public string rol { get; set; }
        public Ventas(string rol, string nombre)
        {
            InitializeComponent();

            this.rol = rol;
            this.nombre = nombre;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(rol=="ADM")
            {
                EliminarRegistrosData();
            }
            
        }

        //metodo eliminar registros 
        private void EliminarRegistrosData()
        {
            try
            {
                // Verificar si hay filas seleccionadas
                if (dgvVentas.SelectedRows.Count > 0)
                {
                    // Mostrar mensaje de confirmación
                    DialogResult result = MessageBox.Show("¿Seguro que desea eliminar el registro seleccionado?",
                        "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Crear una instancia de nVentas
                        NVentas ventasManager = new NVentas();

                        // Recorrer las filas seleccionadas y eliminarlas una por una
                        foreach (DataGridViewRow row in dgvVentas.SelectedRows)
                        {
                            // Obtener el idProducto y idVenta de la fila seleccionada
                            int idProducto = Convert.ToInt32(row.Cells["ID Producto"].Value);
                            int idVenta = Convert.ToInt32(row.Cells["ID Venta"].Value);

                            // Llamar al método para eliminar el registro
                            bool eliminado = ventasManager.EliminarRegistroProductoVenta(idProducto, idVenta);

                            if (eliminado)
                            {
                                // Eliminar la fila del DataGridView
                                dgvVentas.Rows.Remove(row);
                                EliminarVentaSinDetalles(idProducto, idVenta);
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        // Mostrar mensaje de éxito
                        MessageBox.Show("Se eliminaron los registros correctamente.", "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("No hay filas seleccionadas para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar los registros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //eliminar registros al insertar venta
        private void EliminarRegistrosDataInsertarVenta()
        {
            try
            {
                // Verificar si hay filas seleccionadas
                if (dgvDetalles.SelectedRows.Count > 0)
                {
                    // Mostrar mensaje de confirmación
                    DialogResult result = MessageBox.Show("¿Seguro que desea eliminar el registro seleccionado?",
                        "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Crear una instancia de nVentas
                        NVentas ventasManager = new NVentas();

                        // Recorrer las filas seleccionadas y eliminarlas una por una
                        foreach (DataGridViewRow row in dgvDetalles.SelectedRows)
                        {
                            // Obtener el idProducto y idVenta de la fila seleccionada
                            int idProducto = Convert.ToInt32(row.Cells["id_producto"].Value);
                            int idVenta = Convert.ToInt32(row.Cells["id_venta"].Value);

                            // Llamar al método para eliminar el registro
                            bool eliminado = ventasManager.EliminarRegistroProductoVenta(idProducto, idVenta);

                            if (eliminado)
                            {
                                // Eliminar la fila del DataGridView
                                dgvDetalles.Rows.Remove(row);
                              
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }

                        // Mostrar mensaje de éxito
                       
                        MessageBox.Show("Se eliminaron los registros correctamente.", "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                }
                else
                {
                    MessageBox.Show("No hay filas seleccionadas para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar los registros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        //ComboBox def Usuarios
        private void cbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        //Load del formuario
        private void Ventas_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'sharksTCGDataSet.users' Puede moverla o quitarla según sea necesario.
            this.usersTableAdapter.Fill(this.sharksTCGDataSet.users);
            CargarNombreUsuario();
            CargarClientes();
            CargarProductos();
            cargarDatagrid();
            CargarNombresUsuarios();
        }
        //Funciones

        private void CargarNombreUsuario()
        {
            List<string> listaUsuarios = new List<string>();
            listaUsuarios.Add(nombre);

            // Establecer la lista como origen de datos del ComboBox
            cbUsuario.DataSource = listaUsuarios;
            lblUsuario.Text = "Usuario: " + nombre + ", Rol: " + rol;
        }
        // Función para crear una venta
        private void CrearVenta(DateTime fechaVenta, int? idCliente, int? idUsuario)
        {
            try
            {
                // Crear la venta llamando al método en la capa de negocio
                int idVenta = NVentas.CrearVenta(fechaVenta, idCliente ?? 0, idUsuario ?? 0);

                // Mostrar el ID de la venta creada en el TextBox
                txtIDVenta.Text = idVenta.ToString();

                MessageBox.Show("Venta creada exitosamente. ID de venta: " + idVenta, "Venta Creada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al crear la venta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Función para validar si los campos están llenos
        private bool ValidarCamposVenta(DateTime fechaVenta, int? idCliente, int? idUsuario)
        {
            // Verificar si alguno de los campos es nulo o la fecha es la predeterminada
            if ( idCliente == null || idUsuario == null)
                return false; // Faltan rellenar campos
            else
                return true; // Todos los campos están llenos
        }


        // Función para mostrar mensajes
        private void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void CargarProductos()
        {
            try
            {
                // Limpiar el ComboBox antes de cargar los datos
                cbProducto.Items.Clear();

                // Obtener los nombres de todos los productos desde la capa de negocio
                DataTable dtProductos = NProductos.MostrarProductos();

                // Verificar si se obtuvieron datos
                if (dtProductos != null && dtProductos.Rows.Count > 0)
                {
                    // Iterar a través de cada fila y agregar el nombre del producto al ComboBox
                    foreach (DataRow row in dtProductos.Rows)
                    {
                        cbProducto.Items.Add(row["nombre"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron productos en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarClientes()
        {
            try
            {
                // Limpiar el ComboBox de clientes antes de cargar los datos
                cbCliente.Items.Clear();

                // Obtener los nombres de todos los clientes desde la capa de negocio
                DataTable dtClientes = NClientes.MostrarClientes();

                // Verificar si se obtuvieron datos
                if (dtClientes != null && dtClientes.Rows.Count > 0)
                {
                    // Iterar a través de cada fila y agregar el nombre del cliente al ComboBox
                    foreach (DataRow row in dtClientes.Rows)
                    {
                        cbCliente.Items.Add(row["nombre"].ToString());
                        cbClienteVenta.Items.Add(row["nombre"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron clientes en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int? ObtenerIdCliente(string nombreCliente)
        {
            try
            {
                // Llamar al método en la capa de negocio para obtener el ID del cliente por su nombre
                DataTable dtCliente = NClientes.MostrarClientes();

                // Verificar si se obtuvieron datos
                if (dtCliente != null && dtCliente.Rows.Count > 0)
                {
                    foreach (DataRow row in dtCliente.Rows)
                    {
                        // Verificar si el nombre coincide con el cliente seleccionado
                        if (row["nombre"].ToString() == nombreCliente)
                        {
                            // Obtener el ID del cliente desde el DataTable y convertirlo a entero
                            int idCliente = Convert.ToInt32(row["id"]);
                            return idCliente;
                        }
                    }
                    MessageBox.Show("No se encontró el cliente seleccionado en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                else
                {
                    MessageBox.Show("No se encontraron clientes en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del cliente: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private int? ObtenerIdUsuario(string nombreUsuario)
        {
            try
            {
                // Llamar al método en la capa de negocio para obtener el ID del usuario por su nombre
                DataTable dtUsuario = NUsuarios.MostrarUsuarios();

                // Verificar si se obtuvieron datos
                if (dtUsuario != null && dtUsuario.Rows.Count > 0)
                {
                    foreach (DataRow row in dtUsuario.Rows)
                    {
                        // Verificar si el nombre coincide con el usuario seleccionado
                        if (row["nombre"].ToString() == nombreUsuario)
                        {
                            // Obtener el ID del usuario desde el DataTable y convertirlo a entero
                            int idUsuario = Convert.ToInt32(row["id"]);
                            return idUsuario;
                        }
                    }
                    MessageBox.Show("No se encontró el usuario seleccionado en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
                else
                {
                    MessageBox.Show("No se encontraron usuarios en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener el ID del usuario: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


        private void CargarNombresUsuarios()
        {
            try
            {
                // Obtener los nombres de todos los usuarios desde la capa de negocio
                DataTable dtUsuarios = NUsuarios.MostrarUsuarios();

                // Verificar si se obtuvieron datos
                if (dtUsuarios != null && dtUsuarios.Rows.Count > 0)
                {
                    // Limpiar el ComboBox antes de agregar nuevos elementos
                    cbUsuarioVenta.Items.Clear();

                    // Agregar manualmente los nombres de usuario al ComboBox
                    foreach (DataRow row in dtUsuarios.Rows)
                    {
                       
                        cbUsuarioVenta.Items.Add(row["nombre"].ToString());
                    }
                }
                else
                {
                    MessageBox.Show("No se encontraron usuarios en la base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void MostrarMensajeError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        //boton crear venta
        private void btnCrearVenta_Click(object sender, EventArgs e)
        {
            // Obtener la fecha de venta seleccionada
            DateTime fechaVenta = dtFechaVenta.Value.Date;

            // Obtener el nombre del cliente seleccionado en el ComboBox
            string nombreCliente = cbCliente.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(nombreCliente))
            {
                MostrarMensajeError("Por favor, seleccione un cliente.");
                return;
            }

            // Obtener el nombre del usuario seleccionado en el ComboBox
            string nombreUsuario = cbUsuario.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                MostrarMensajeError("Por favor, seleccione un usuario.");
                return;
            }

            // Obtener el ID del cliente a partir de su nombre
            int? idCliente = NClientes.ObtenerIdClientePorNombre(nombreCliente);
            if (!idCliente.HasValue)
            {
                MostrarMensajeError("No se pudo obtener el ID del cliente.");
                return;
            }

            // Obtener el ID del usuario a partir de su nombre
            int? idUsuario = NUsuarios.ObtenerIdUsuarioPorNombre(nombreUsuario);
            if (!idUsuario.HasValue)
            {
                MostrarMensajeError("No se pudo obtener el ID del usuario.");
                return;
            }

            // Validar si todos los campos están llenos
            if (ValidarCamposVenta(fechaVenta, idCliente, idUsuario))
            {
                // Llamar a la función para crear la venta
                int idVenta = NVentas.CrearVenta(fechaVenta, idCliente.Value, idUsuario.Value);

                // Mostrar el ID de la venta creada en el TextBox
                txtIDVenta.Text = idVenta.ToString(); // Convertir el ID de venta a cadena de caracteres
            }
            else
            {
                // Mostrar mensaje de error si faltan llenar campos
                MostrarMensajeError("Faltan rellenar campos.");
            }
        }
        //Boton llenar campos nombre e id producto
        private void btnAgregar_Click(object sender, EventArgs e)
        {
           
            if (!string.IsNullOrEmpty(txtTotal.Text))
            {
                limpiarCasillasDetalle();
            }
            else
            {
                if (!string.IsNullOrEmpty(txtIDProducto.Text))
                {
                    limpiarCasillasporid();
                }
            }

            try
            {
                    // Obtener el nombre del producto seleccionado en el ComboBox
                    string nombreProducto = cbProducto.SelectedItem.ToString();

                    // Obtener el ID de la venta desde el txtIDVenta
                    int idVenta = Convert.ToInt32(txtIDVenta.Text);

                    int idProducto;
                    string nombre;
                    decimal precioVenta;

                    // Llamar al método de la capa de negocio para llenar los campos del producto
                    bool exito = NProductos.LlenarCamposProducto(nombreProducto, idVenta, out idProducto, out nombre, out precioVenta);

                    if (exito)
                    {
                        // Mostrar los datos obtenidos en los TextBox correspondientes
                        txtNombreProducto.Text = nombre;
                        txtIDProducto.Text = idProducto.ToString();
                        txtPrecioUnitario.Text = precioVenta.ToString();

                        
                }
                    else
                    {
                        MessageBox.Show("Error al llenar los campos del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            
        }
        //completar total
        private void btnAceptar_Click(object sender, EventArgs e)
        {

            try
            {
                // Verificar si el txtTotal está vacío
                if (string.IsNullOrWhiteSpace(txtTotal.Text))
                {
                    // Obtener los valores de los TextBox
                    int idVenta = int.Parse(txtIDVenta.Text);
                    int idProducto = int.Parse(txtIDProducto.Text);
                    decimal saldo = decimal.Parse(txtSaldo.Text);
                    decimal descuento = decimal.Parse(txtDescuento.Text);
                    int cantidad = int.Parse(txtCantidad.Text);

                    // Llamar al método de la capa de negocio para completar el total del producto
                    decimal total;
                    bool exito = NProductos.CompletarTotalProducto(idVenta, idProducto, saldo, descuento, cantidad, out total);

                    if (exito)
                    {
                        // Actualizar el TextBox de total
                        txtTotal.Text = total.ToString("0.00");

                       
                        
                        actGridViewIns();
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el total del producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Limpie los campos primero para realizar campos", "Acción inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Actualizar gridview de detalles
        private void actGridViewIns()
        {
            try
            {
                // Obtener el ID de la venta desde txtIDVenta
                int idVenta = Convert.ToInt32(txtIDVenta.Text);

                // Obtener los detalles de la venta desde la capa de negocio
                dgvDetalles.DataSource = NProductos.ObtenerProductosVentaPorIdVenta(idVenta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el DataGridView: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTotal.Text))
            {
                limpiarCasillasDetalle();
            }
            else
            {
                if (!string.IsNullOrEmpty(txtIDProducto.Text))
                {
                    limpiarCasillasporid();
                }
            }
        }
        //limpiar casillas por id
        private void limpiarCasillasporid()
        {
            try
            {
                // Obtener los IDs de venta y producto
                int idVenta = Convert.ToInt32(txtIDVenta.Text);
                int idProducto = Convert.ToInt32(txtIDProducto.Text);

                // Limpiar los campos relacionados a producto_venta
                txtIDProducto.Clear();
                txtNombreProducto.Clear();
                txtPrecioUnitario.Clear();
                txtCantidad.Clear();
                txtSaldo.Clear();
                txtDescuento.Clear();
                txtTotal.Clear();
               
                // Llamar al método de la capa de negocio para limpiar el registro de producto_venta
                bool exito = NProductos.LimpiarRegistroProductoVenta(idVenta, idProducto);

                if (exito)
                {
                    
                }
                else
                {
                    MessageBox.Show("Error al limpiar el registro de producto_venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar los campos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //LimpiarCasiilas Detalle
        private void limpiarCasillasDetalle()
        {
            try
            {
                // Limpiar los campos relacionados a producto_venta
                txtIDProducto.Clear();
                txtNombreProducto.Clear();
                txtPrecioUnitario.Clear();
                txtCantidad.Clear();
                txtSaldo.Clear();
                txtDescuento.Clear();
               
                txtTotal.Clear();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar las casillas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelarDetalle_Click(object sender, EventArgs e)
        {
            limpiarCasillasporid();
            actGridViewIns();
            MessageBox.Show("Se cancelo el registro", "Érror", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnCerrarVenta_Click(object sender, EventArgs e)
        {
            int idVenta = Convert.ToInt32(txtIDVenta.Text);

            if (VerificarRegistroVenta(idVenta))
            {
                limpiarTodosLosCampos();
            }
            else
            {
                EliminarRegistroVenta(idVenta);
                limpiarTodosLosCampos();
                MessageBox.Show("Ventas incompletas, se eliminara todas las ordenes", "Érror", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        private void limpiarTodosLosCampos()
        {
            txtIDProducto.Clear();
            txtNombreProducto.Clear();
            txtPrecioUnitario.Clear();
            txtCantidad.Clear();
            txtSaldo.Clear();
            txtDescuento.Clear();
            txtTotal.Clear();
            txtIDVenta.Clear();
            cbCliente.SelectedIndex = -1; // Limpiar la selección del combo box
            cbUsuario.SelectedIndex = -1; // Limpiar la selección del combo box
            dgvDetalles.DataSource = null; // Limpiar los datos del DataGridView
            dgvDetalles.Rows.Clear(); // Limpiar las filas del DataGridView
        }

        private void btnCancelar_Click(object sender, EventArgs e)

        {
            int idVenta = Convert.ToInt32(txtIDVenta.Text);
            EliminarRegistroVenta(idVenta);
            limpiarTodosLosCampos();
        }
        private void EliminarRegistroVenta(int idVenta)
        {
            if (NVentas.EliminarRegistroVenta(idVenta))
            {
                MessageBox.Show("Registro de venta eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al eliminar registro de venta.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //verificar detalles en venta
        private bool VerificarRegistroVenta(int idVenta)
        {
            return NVentas.VerificarRegistroVenta(idVenta);
        }
        public void cargarDatagrid()
        {


            try
            {
                // Obtener los valores de los controles de búsqueda
                DateTime fechaInicio = dtpInicio.Value.Date;
                DateTime fechaFin = dtpFinal.Value.Date;
                string nombreCliente = cbClienteVenta.SelectedIndex != -1 ? cbClienteVenta.SelectedItem.ToString() : null;
                string nombreUsuario = cbUsuarioVenta.SelectedIndex != -1 ? cbUsuarioVenta.SelectedItem.ToString() : null;
                int? idVenta = string.IsNullOrEmpty(txtIdVentaL.Text) ? null : (int?)int.Parse(txtIdVentaL.Text);

                // Llamar al método de búsqueda en la capa de negocio (NVentas)
                dgvVentas.DataSource = NVentas.BuscarVentas(fechaInicio, fechaFin, nombreCliente, nombreUsuario, idVenta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar ventas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {

            try
            {
                // Obtener los valores de los controles de búsqueda
                DateTime fechaInicio = dtpInicio.Value.Date;
                DateTime fechaFin = dtpFinal.Value.Date;
                string nombreCliente = cbClienteVenta.SelectedIndex != -1 ? cbClienteVenta.SelectedItem.ToString() : null;
                string nombreUsuario = cbUsuarioVenta.SelectedIndex != -1 ? cbUsuarioVenta.SelectedItem.ToString() : null;
                int? idVenta = string.IsNullOrEmpty(txtIdVentaL.Text) ? null : (int?)int.Parse(txtIdVentaL.Text);

                // Llamar al método de búsqueda en la capa de negocio (NVentas)
                dgvVentas.DataSource = NVentas.BuscarVentas(fechaInicio, fechaFin, nombreCliente, nombreUsuario, idVenta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar ventas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
        }

        private void dgvVentas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void EliminarVentaSinDetalles(int idProducto, int idVenta)
        {
            try
            {
                // Llamar al método en NVentas para verificar y eliminar la venta sin detalles
                NVentas ventasManager = new NVentas();
                ventasManager.VerificarVentaSinDetalles(idProducto, idVenta);

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar eliminar la venta sin detalles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminarDetalle_Click(object sender, EventArgs e)
        {
            EliminarRegistrosDataInsertarVenta();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (rol == "ADM")
            {
                EditarDetalle();
            }
           
        }
        private void EditarDetalle()
        {
            // Verificar si hay exactamente una fila seleccionada
            if (dgvVentas.SelectedRows.Count == 1)
            {
                // Obtener los valores de los IDs de producto y venta de la fila seleccionada
                int idProducto = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["ID Producto"].Value);
                int idVenta = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["ID Venta"].Value);
                int precioUnitario = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Precio Unitario"].Value);
                int cantidad = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Cantidad"].Value);
                int descuento = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Descuento"].Value);
                int saldo = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Saldo"].Value);
                int total = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Total"].Value);
                // Crear una instancia del formulario para editar detalle
                frmEditarDetalle editarDetalleForm = new frmEditarDetalle(this);

                // Pasar los valores de los IDs al formulario para editar detalle
                editarDetalleForm.IDProducto = idProducto;
                editarDetalleForm.IDVenta = idVenta;
                editarDetalleForm.PrecioUnitario = precioUnitario;
                editarDetalleForm.Cantidad = cantidad;
                editarDetalleForm.Descuento = descuento;
                editarDetalleForm.Saldo = saldo;
                editarDetalleForm.Total = total;

                // Mostrar el formulario para editar detalle
                editarDetalleForm.Show();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione exactamente una fila para editar el detalle.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }

}
