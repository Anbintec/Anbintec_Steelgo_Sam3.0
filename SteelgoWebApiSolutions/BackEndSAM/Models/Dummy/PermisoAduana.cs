using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class PermisoAduana
    {
        public int NumeroPermiso { get; set; }
        public int PermisoTramite { get; set; }
        public int PermisoAutorizado { get; set; }
        public ArchivoAutorizadoAV ArchivoAutorizado { get; set; }
    }
}