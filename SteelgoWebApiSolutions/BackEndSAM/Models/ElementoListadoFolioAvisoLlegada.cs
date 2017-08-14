using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ElementoListadoFolioAvisoLlegada
    {
        public string FolioAvisoLlegadaID { get; set; }
        public string FechaRecepcion { get; set; } // Fecha estimada
        public string FechaGeneracion { get; set; } // Fecha Generacion
        public string FolioConfiguracion { get; set; }
    }

    public class ElementoListadoFolioEntradaMaterial
    {
        public string FolioConfiguracion { get; set; }
        public string FolioAvisoEntradaID { get; set; }
        public string FolioAvisoLlegadaID { get; set; }
        public string Patio { get; set; }
        public string EstatusFolio { get; set; }
        public string FechaCreación { get; set; }
    }
}