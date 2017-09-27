using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ValidacionRT
{
    public class DetalleCapturaActualizaRT
    {
        public int Accion { get; set; }
        public int Estatus { get; set; }
        public int ReporteRTID { get; set; }
        public int ResultadoConciliacionID { get; set; }
        public int RazonNoConciliacionID { get; set; }
        public string ComentarioValidacion { get; set; }
        public int UsuarioIDConciliacion { get; set; }
        public int ProveedorIDConciliacion { get; set; }
    }
}