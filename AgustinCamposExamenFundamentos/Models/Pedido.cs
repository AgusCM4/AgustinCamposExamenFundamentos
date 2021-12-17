using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgustinCamposExamenFundamentos.Models
{
    public class Pedido
    {
        public String CodigoPedido { get; set; }
        public String CodigoCliente { get; set; }
        public DateTime FechaPedido { get; set; }
        public String FormaEnvio { get; set; }
        public int Importe { get; set; }
    }
}
