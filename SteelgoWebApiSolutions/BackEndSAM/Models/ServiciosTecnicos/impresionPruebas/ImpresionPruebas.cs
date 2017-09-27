using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.impresionPruebas
{
    public class Requisicion
    {
        public Requisicion()
        {
            RequisicionID = 0;
            ProyectoID = 0;
            TipoPruebaID = 0;
            NombreRequisicion = "";
            CodigoAsme = "";
            FechaRequisicion = "";
            Observacion = "";
        }

        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public string NombreRequisicion { get; set; }
        public string CodigoAsme { get; set; }
        public string FechaRequisicion { get; set; }
        public string Observacion { get; set; }
        public int Accion { get; set; }
    }

    public class ImpresionPruebasDetalle
    {
        public string Requisicion { get; set; }
        public string Spool { get; set; }
        public string Junta { get; set; }
        public string Clasificacion { get; set; }
        public decimal? Diametro { get; set; }
        public decimal? Espesor { get; set; }
        public string Cedula { get; set; }
        public string TipoJunta { get; set; }
        public string Reporte { get; set; }
        public string Version { get; set; }
        public bool Seleccionado { get; set; }

        public string Cuadrante { get; set; }
        public int Prioridad { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int SpoolID { get; set; }
        public int? JuntaSpoolID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public int TipoPruebaID { get; set; }
        public string Especificacion { get; set; }
        public int ReporteRequisicionID { get; set; }
        public string Url { get; set; }

        public int Accion { get; set; }

    }

    public class Captura
    {
        public string NombreReporte { get; set; }
        public string FechaReporte { get; set; }

        public List<GuardarReporte> Detalles { get; set; }
    }


    public class GuardarReporte
    {
        public int Accion { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public int ReporteRequisicionID { get; set; }
    }

}