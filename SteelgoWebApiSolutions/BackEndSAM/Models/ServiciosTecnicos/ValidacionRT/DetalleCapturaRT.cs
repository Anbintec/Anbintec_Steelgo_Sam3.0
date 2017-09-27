using BackEndSAM.Models.ServiciosTecnicos.ReporteRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ValidacionRT
{
    public class DetalleCapturaRT
    {
        public int RequisicionID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public decimal? Tamano { get; set; }
        public decimal? Densidad { get; set; }
        public int Accion { get; set; }
        public int EstatusRequisicion { get; set; }
        public int Junta { get; set; }
        public string ClasificacionPND { get; set; }
        public string TipoPrueba { get; set; }
        public string Observaciones { get; set; }
        public string CodigoAsme { get; set; }
        public int? NumeroPlacas { get; set; }
        public List<DetallePorPlacas> ListaDetallePorPlacas { get; set; }
        public string NumeroControl { get; set; }
        public int? ResultadoConciliacionID { get; set; }
        public string ResultadoConciliacion { get; set; }
        public List<ResultadoConciliacion> ListaResultadoConciliacion { get; set; }
        public int? RazonNoConciliacionID { get; set; }
        public string RazonNoConciliacion { get; set; }
        public List<RazonNoConciliacion> ListaRazonNoConciliacion { get; set; }
        public List<Resultados> ListaResultados { get; set; }
        public List<Defectos> ListaDefectos { get; set; }
        public string Comentarios { get; set; }
        public string TemplateDetalleElemento { get; set; }
        public int? ReporteRTID { get; set; }
    }
}