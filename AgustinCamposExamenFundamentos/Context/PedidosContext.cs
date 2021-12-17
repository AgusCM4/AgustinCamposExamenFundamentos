using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using AgustinCamposExamenFundamentos.Models;

namespace AgustinCamposExamenFundamentos.Context
{
    #region STORED PROCEDURES

    //INSERTA NUEVO PEDIDO
    //
    //
    //Create procedure insertapedidos
    //(
    //@codigopedido nvarchar(50),
    //@codigocli nvarchar(50),
    //@FechaEntrega Datetime,
    //@FormaEnvio nvarchar(50),
    //@Importe int
    //)
    //as
    //	insert into pedidos values(@codigopedido, @codigocli, @FechaEntrega, @FormaEnvio, @Importe)
    //go



    //
    //INTENTO DE PROCEDURE
    //
    //Create procedure modificaclientes
    //(
    //@codigocliantig nvarchar(50),
    //@empresa nvarchar(50),
    //@contacto nvarchar(50),
    //@cargo nvarchar(50),
    //@ciudad nvarchar(50),
    //@telefono int
    //)
    //as
    //	declare @codigocli nvarchar(50)

    //    SELECT UPPER(SUBSTRING(@empresa,0,3))
    //	update clientes set CodigoCliente = @codigocli, Empresa = @empresa, Contacto = @contacto, Cargo = @cargo, Ciudad = @ciudad, Telefono = @telefono where CodigoCliente = @codigocli
    //go




    #endregion

    public class PedidosContext
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public PedidosContext()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json");
            IConfigurationRoot config = builder.Build();
            String cadenaconexion = config["UrlBase"];
            this.cn = new SqlConnection(cadenaconexion);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<String> CargaClientes()
        {
            String sql = "select empresa from clientes";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<String> clientes = new List<String>();

            while (this.reader.Read())
            {
                String empresa = this.reader["empresa"].ToString();
                clientes.Add(empresa);
            }

            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public Cliente DatosCliente(String empresa)
        {
            String sql = "select * from clientes where empresa=@EMPRESA";
            this.com.Parameters.AddWithValue("@EMPRESA", empresa);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            Cliente c = new Cliente();

            while (this.reader.Read())
            {
                c.CodigoCliente = this.reader["CodigoCliente"].ToString();
                c.Empresa = this.reader["Empresa"].ToString();
                c.Contacto = this.reader["Contacto"].ToString();
                c.Cargo = this.reader["Cargo"].ToString();
                c.Ciudad = this.reader["Ciudad"].ToString();
                c.Telefono = int.Parse(this.reader["Telefono"].ToString());
            }
            
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return c;
        }

        public List<Pedido> GetPedidos(String CodigoCliente)
        {
            //TENDRIA QUE HABER SIDO LISTA DE STRING

            String sql = "select * from pedidos where CodigoCliente=@CODCLI";
            this.com.Parameters.AddWithValue("@CODCLI", CodigoCliente);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            List<Pedido> pedidos = new List<Pedido>();

            while (this.reader.Read())
            {
                Pedido p = new Pedido();
                p.CodigoPedido = this.reader["CodigoPedido"].ToString();
                p.CodigoCliente = this.reader["CodigoCliente"].ToString();
                p.FechaPedido = (DateTime) this.reader["FechaEntrega"];
                p.FormaEnvio = this.reader["FormaEnvio"].ToString();
                p.Importe = int.Parse(this.reader["Importe"].ToString());
                pedidos.Add(p);
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public Pedido DatosPedido(String CodPed)
        {
            String sql = "select * from pedidos where CodigoPedido=@CODPED";
            this.com.Parameters.AddWithValue("@CODPED", CodPed);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            this.reader = this.com.ExecuteReader();

            Pedido p = new Pedido();

            while (this.reader.Read())
            {
                p.CodigoPedido = this.reader["CodigoPedido"].ToString();
                p.CodigoCliente = this.reader["CodigoCliente"].ToString();
                p.FechaPedido = (DateTime)this.reader["FechaEntrega"];
                p.FormaEnvio = this.reader["FormaEnvio"].ToString();
                p.Importe = int.Parse(this.reader["Importe"].ToString());
            }

            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return p;
        }

        public int InsertaPedido(String CodPed,String CodCli,DateTime FechaEntrega,String FormaEnvio,int importe)
        {
            this.com.Parameters.AddWithValue("@codigopedido", CodPed);
            this.com.Parameters.AddWithValue("@codigocli", CodCli);
            this.com.Parameters.AddWithValue("@FechaEntrega", FechaEntrega);
            this.com.Parameters.AddWithValue("@FormaEnvio", FormaEnvio);
            this.com.Parameters.AddWithValue("@Importe", importe);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "insertapedidos";
            this.cn.Open();
            int insertados=this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();

            return insertados;
        }

        public int EliminaPedido(String CodigoPed)
        {
            String sql = "delete from pedidos where CodigoPedido=@CODPED";
            this.com.Parameters.AddWithValue("@CODPED", CodigoPed);
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int elim = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return elim;
        }

    }
}
