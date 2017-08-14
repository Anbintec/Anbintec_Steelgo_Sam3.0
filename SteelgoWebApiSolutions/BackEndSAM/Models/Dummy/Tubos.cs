using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class Tubos
    {
        public int ItemCode { get; set; }
        public List<NumerosUnicos> NumeroUnico { get; set; }
    }
}