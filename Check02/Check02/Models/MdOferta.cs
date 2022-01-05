using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Check02.Models
{
    public class MdOferta
    {
        [Key]
        public String IdOferta { get; set; }

        public int IdCliente { get; set; }

        public decimal ValorOfertaFinal { get; set; }

        public virtual List<MdServicos> Carrinho { get; set; }
    }
}