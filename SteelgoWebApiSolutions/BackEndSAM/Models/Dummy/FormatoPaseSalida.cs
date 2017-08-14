using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class FormatoPaseSalida
    {
        public string NombreProyecto { get; set; }
        public DateTime FechaHoy { get; set; }
        public string NombreOperador { get; set; }
        public int CantidadPlanas { get; set; }
        public string PlacaTracto { get; set; }
        public List<Plana> Plana { get; set; }
        public bool TienePackingListFirmado { get; set; }
        public bool TieneIncidenciasFirmadas { get; set; }
        public RegistroTiempos RegistroTiempos { get; set; }
        public Incidencia Incidencia { get; set; }
    }
}