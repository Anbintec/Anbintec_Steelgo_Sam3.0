using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class EdicionCuantificacion
    {
        public Proyecto Proyecto { get; set; }
        public FolioLlegada FolioLlegada { get; set; }
        public bool TerminadoCuantificacion { get; set; }
        public List<ListadoCuantificacion> ListadoCuantificacion { get; set; }
    }
}