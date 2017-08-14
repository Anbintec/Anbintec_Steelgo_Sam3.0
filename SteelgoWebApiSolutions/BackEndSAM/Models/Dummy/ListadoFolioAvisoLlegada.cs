using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoFolioAvisoLlegada
    {
        public int FolioAvisoLlegadaID { get; set; }
        public string Patio { get; set; }
        public string NombreProyecto { get; set; }
        public string Proveedor { get; set; }
        public string Transportista { get; set; }
        public string FechaRecepcion { get; set; }
        public string Estatus { get; set; }
    }
}