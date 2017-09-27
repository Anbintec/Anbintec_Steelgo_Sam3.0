using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.EditarRequisicion
{
    public class EditarRequisicion
    {
        public class ElementosPorClasificacion
        {
            public ElementosPorClasificacion()
            {
                Accion = 0;
                NumeroControl = "";
                EtiquetaJunta = "";
                TipoJunta = "";
                NombreRequisicion = "";
                Cuadrante = "";
                Prioridad = 0;
                Clasificacion = "";
                Diametro = 0;
                Espesor = 0;
                Cedula = "";
                Codigo = "";
                ElementoPorClasificacionPNDID = 0;
                Agregar = false;
                RequisicionID = 0;
                ProyectoID = 0;
                SpoolID = 0;
                JuntaSpoolID = 0;
                OrdenTrabajoSpoolID = 0;
                TipoPruebaID = 0;
                Especificacion = "";
                EstatusCaptura = 0;               
        }

            

            public int Accion { get; set; }
            public string NumeroControl { get; set; }
            public string EtiquetaJunta { get; set; }
            public string TipoJunta { get; set; }
            public string NombreRequisicion { get; set; }
            public string Cuadrante { get; set; }
            public int Prioridad { get; set; }
            public string Clasificacion { get; set; }
            public decimal Diametro { get; set; }
            public decimal Espesor { get; set; }
            public string Cedula { get; set; }
            public int? ElementoPorClasificacionPNDID { get; set; }
            public bool Agregar { get; set; }
            public int RequisicionID { get; set; }
            public int? ProyectoID { get; set; }
            public int SpoolID { get; set; }
            public int JuntaSpoolID { get; set; }
            public int? OrdenTrabajoSpoolID { get; set; }
            public int TipoPruebaID { get; set; }
            public string Especificacion { get; set; }
            public int EstatusCaptura { get; set; }
            public string Codigo { get; set; }
            
        }

        public class Captura
        {
            public Captura()
            {
                RequisicionID = 0;
                Requisicion = "";
                ProyectoID = 0;
                TipoPruebaID = 0;
                CodigoAsme = "";
                Observacion = "";
            }
            public int RequisicionID { get; set; }
            public string Requisicion { get; set; }
            public int ProyectoID { get; set; }
            public int TipoPruebaID { get; set; }
            public string FechaRequisicion { get; set; }
            public string CodigoAsme { get; set; }
            public string Observacion { get; set; }
            public List<DetalleCaptura> listaDetalle { get; set; }
        }

        public class DetalleCaptura
        {
            public DetalleCaptura()
            {
                RequisicionID = 0;
                ElementoPorClasificacionPNDID = 0;
                Accion = 0;                                
                OrdenTrabajoID = 0;
                ClasificacionPNDID = 0;
                SpoolID = 0;
                JuntaSpool = 0;
                ClasificacionManual = 0;
            }

            public int RequisicionID { get; set; }
            public int ElementoPorClasificacionPNDID { get; set; }
            public int Accion { get; set; }
            public int OrdenTrabajoID { get; set; }
            public int ClasificacionPNDID { get; set; }
            public int SpoolID { get; set; }
            public int JuntaSpool { get; set; }
            public int ClasificacionManual { get; set; }
        }

        public class Proyecto
        {
            public Proyecto()
            {
                ProyectoID = 0;
                Nombre = "";
            }
            public int ProyectoID { get; set; }
            public string Nombre { get; set; }
        }

        public class TipoPrueba
        {
            public TipoPrueba()
            {
                TipoPruebaID = 0;
                Nombre = "";
            }

            public int TipoPruebaID { get; set; }
            public string Nombre { get; set; }
        }

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

        }

        public class ElementosRequisicion
        {
            public int RequisicionID { get; set; }
            public int ProyectoID { get; set; }
            public string nombreProyecto { get; set; }
            public string nombreTipoPrueba { get; set; }
            public int TipoPruebaID { get; set; }
            public string NombreRequisicion { get; set; }
            public string CodigoAsme { get; set; }
            public string FechaRequisicion { get; set; }
            public string Observacion { get; set; }
            public string NumeroControl { get; set; }
        }

        public class JuntaSpool
        {
            public JuntaSpool()
            {
                JuntaSpoolID = 0;
                Etiqueta = "";
            }
               
            public int JuntaSpoolID { get; set; }
            public string Etiqueta { get; set; }
        }
    }
}