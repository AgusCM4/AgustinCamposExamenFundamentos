using AgustinCamposExamenFundamentos.Context;
using AgustinCamposExamenFundamentos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        private PedidosContext context;
        public FormPractica()
        {
            InitializeComponent();
            this.context = new PedidosContext();
            this.CargaClientes();
        }

        public void CargaClientes()
        {
            List<String> clientes = this.context.CargaClientes();

            foreach (String cli in clientes)
            {                
                this.cmbclientes.Items.Add(cli);                
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente clien = this.context.DatosCliente(this.cmbclientes.SelectedItem.ToString());

            this.txtempresa.Text = clien.Empresa;
            this.txtcargo.Text = clien.Cargo;
            this.txtciudad.Text = clien.Ciudad;
            this.txtcontacto.Text = clien.Contacto;
            this.txttelefono.Text = clien.Telefono.ToString();

            this.CargaPedidos(clien.CodigoCliente);

        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            String codigocli = this.cmbclientes.SelectedItem.ToString().Substring(0,3).ToUpper();

            String codigoped = this.lstpedidos.SelectedItem.ToString();

            Pedido pedido=this.context.DatosPedido(codigoped);

            this.txtcodigopedido.Text = pedido.CodigoPedido;
            this.txtfechaentrega.Text = pedido.FechaPedido.ToString();
            this.txtformaenvio.Text = pedido.FormaEnvio;
            this.txtimporte.Text = pedido.Importe.ToString();

        }

        public void CargaPedidos(String CodigoCli)
        {
            this.lstpedidos.Items.Clear();
            List<Pedido> pedidos = this.context.GetPedidos(CodigoCli);

            foreach (Pedido p in pedidos)
            {
                this.lstpedidos.Items.Add(p.CodigoPedido);
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            String codigocli = this.cmbclientes.SelectedItem.ToString().Substring(0, 3).ToUpper();

            DateTime fechaentrega = new DateTime(2021, 12, 17);

            int insertados = this.context.InsertaPedido(this.txtcodigopedido.Text, codigocli,fechaentrega, this.txtformaenvio.Text, int.Parse(this.txtimporte.Text));
            this.CargaPedidos(codigocli);
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            if (this.lstpedidos.SelectedIndex!=0)
            {
                String codigocli = this.cmbclientes.SelectedItem.ToString().Substring(0, 3).ToUpper();
                String codped = this.lstpedidos.SelectedItem.ToString();
                int elim = this.context.EliminaPedido(codped);
                this.CargaPedidos(codigocli);
            }
            
        }
    }
}
