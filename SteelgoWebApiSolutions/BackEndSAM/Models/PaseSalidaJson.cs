using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class PaseSalidaJson
    {
        public string Fecha { get; set; }
        public string NombreOperador { get; set; }
        public int NumeroPlanas { get; set; }
        public string Proyecto { get; set; }
        public string Tracto { get; set; }
        public List<string> Planas { get; set; }
        public string FechaLlegada { get; set; }
        public string FechaInicioDescarga { get; set; }
        public string FechaFinDescarga { get; set; }
        public bool TieneIncidencia { get; set; }
        public int NumeroIncidencia { get; set; }
        public bool PackingListFirmado { get; set; }
        public bool IncidenciasFirmada { get; set; }

        public PaseSalidaJson()
        {
            Planas = new List<string>();
        }
    }
}