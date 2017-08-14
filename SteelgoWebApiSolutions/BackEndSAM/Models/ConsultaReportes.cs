using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ConsultaReportes
    {
        public int TipoReporte { get; set; }
        public int FolioOrdenRecepcion { get; set; }
        public int FolioAvisoLlegadaID { get; set; }
        public int FolioOrdenAlmacenaje { get; set; }
        public string Cadena { get; set; }
        public int IncidenciaID { get; set; }
        public int NumeroControlID { get; set; }
        public int TipoMaterial { get; set; }
    }
}