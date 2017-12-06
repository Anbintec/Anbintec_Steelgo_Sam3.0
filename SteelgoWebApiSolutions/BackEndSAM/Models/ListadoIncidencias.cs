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

        public string Titulo { get; set; }
        public string Descripcion{ get; set; }
        public string Respuesta{ get; set; }
        public string DetalleResolucion{ get; set; }
        public string MotivoCancelacion{ get; set; }

        public string TituloIngles { get; set; }
        public string DescripcionIngles { get; set; }
        public string RespuestaIngles { get; set; }
        public string DetalleResolucionIngles { get; set; }
        public string MotivoCancelacionIngles { get; set; }
        public int Modificado { get; set; } 
    }
}