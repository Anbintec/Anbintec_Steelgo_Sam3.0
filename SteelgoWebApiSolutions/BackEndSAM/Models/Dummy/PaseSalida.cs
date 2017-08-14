using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class PaseSalida
    {
        public int PaseEnviado { get; set; }
        public List<Files> Archivos { get; set; }
    }
}