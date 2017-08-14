using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoIncidencias
    {
        public string FolioConfiguracionIncidencia { get; set; }
        public string FolioIncidenciaID { get; set; }
        public string FolioOriginalID { get; set; }
        public string Clasificacion { get; set; }
        public string TipoIncidencia { get; set; }
        public string Estatus { get; set; }
        public string RegistradoPor { get; set; }
        public string FechaRegistro { get; set; }
    }
}