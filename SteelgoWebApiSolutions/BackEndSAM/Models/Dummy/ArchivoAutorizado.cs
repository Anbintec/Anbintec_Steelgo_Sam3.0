using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ArchivoAutorizado
    {
        public int ArchivoID { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
    }
}