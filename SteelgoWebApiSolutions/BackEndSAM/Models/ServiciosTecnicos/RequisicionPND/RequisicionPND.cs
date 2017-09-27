using System.Collections.Generic;

namespace BackEndSAM.Models.ServiciosTecnicos.RequisicionPND
{
    public class ElementosPorClasificacion
    {
        public ElementosPorClasificacion()
        {
            NumeroControl = "";
            EtiquetaJunta = "";
            TipoJunta = "";
            NombreRequisicion = "";
            Cuadrante = "";
            Prioridad = 0;
            Clasificacion = "";
            DiametroPlano = 0;
            Espesor = 0;
            Cedula = "";

            ElementoPorClasificacionPNDID = 0;
            RequisicionID = 0;
            ProyectoID = 0;
            SpoolID = 0;
            JuntaSpoolID = 0;
            OrdenTrabajoSpoolID = 0;
            OrdenTrabajoID = 0;
            TipoPruebaID = 0;
            Especificacion = "";
            Agregar = false;
            ClasificacionManual = 0;
        }

        public int Accion { get; set; }
        public string NumeroControl { get; set; }
        public string EtiquetaJunta { get; set; }
        public string TipoJunta { get; set; }
        public string NombreRequisicion { get; set; }
        public string Cuadrante { get; set; }
        public int Prioridad { get; set; }
        public string Clasificacion { get; set; }
        public decimal DiametroPlano { get; set; }
        public decimal Espesor { get; set; }
        public string Cedula { get; set; }
        public string Codigo { get; set; }

        public int? ElementoPorClasificacionPNDID { get; set; }
        public int? RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int? SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public int? OrdenTrabajoSpoolID { get; set; }
        public int TipoPruebaID { get; set; }
        public string Especificacion { get; set; }
        public bool? Agregar { get; set; }
        public int? OrdenTrabajoID { get; set; }
        public int? ClasificacionPNDID { get; set; }
        public int? ClasificacionManual { get; set; }
        
    }

    public class Captura
    {
        public Captura()
        {
            RequisicionID = 0;
            Requisicion = "";
            ProyectoID = 0;
            TipoPruebaID = 0;
            FolioCliente = "";
            Observacion = "";
            Lenguaje = "";
        }
        public int RequisicionID { get; set; }
        public string Requisicion { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public string FechaRequisicion { get; set; }
        public string FolioCliente { get; set; }
        public string Observacion { get; set; }
        public string Lenguaje { get; set; }
        public List<DetalleCaptura> listaDetalle { get; set; }
    }

    public class DetalleCaptura
    {
        public DetalleCaptura()
        {
            Accion = 0;
            RequisicionID = 0;
            ElementoPorClasificacionPNDID = 0;
            OrdenTrabajoID = 0;
            ClasificacionPNDID = 0;
            SpoolID = 0;
            JuntaSpoolID = 0;
            ClasificacionManual = 0;
        }
        public int RequisicionID { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public int Accion { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int ClasificacionPNDID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public int ClasificacionManual { get; set; }

    }
}