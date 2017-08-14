using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class AlmacenajeRack
    {
        public string ItemCode { get; set; }
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string Rack { get; set; }
        public string TieneLocalizacion { get; set; }
    }
}