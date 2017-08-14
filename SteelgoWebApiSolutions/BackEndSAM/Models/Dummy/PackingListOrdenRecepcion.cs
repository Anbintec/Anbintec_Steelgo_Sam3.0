using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class PackingListOrdenRecepcion
    {
        public int PackingList { get; set; }
        public List<Tubos> Tubos { get; set; }
        public List<Accesorios> Accesorios { get; set; }
    }
}