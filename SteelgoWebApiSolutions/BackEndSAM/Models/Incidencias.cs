using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ListadoIncidenciasBilingues
{
    public class Captura
    {
        public List<DetalleGuardadoIncidencias> listaDetalle { get; set; }
    }

    public class DetalleGuardadoIncidencias
    {
        public int IncidenciaID { get; set; }

        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Respuesta { get; set; }
        public string DetalleResolucion { get; set; }
        public string MotivoCancelacion { get; set; }

        public string TituloIngles { get; set; }
        public string DescripcionIngles { get; set; }
        public string RespuestaIngles { get; set; }
        public string DetalleResolucionIngles { get; set; }
        public string MotivoCancelacionIngles { get; set; }
    }
}