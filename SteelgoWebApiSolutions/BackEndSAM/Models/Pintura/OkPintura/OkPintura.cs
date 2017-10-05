using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.OK
{
    public class OkPintura
    {
        public class ElementosOK
        {
            public ElementosOK()
            {
                SpoolWorkStatusID = 0;
                NumeroControl = "";
                Cuadrante = "";
                Prioridad = 0;
                SpoolID = 0;
                OrdenTrabajoSpoolID = 0;
                ProyectoID = 0;
                SHOTID = 0;
                PRIMID = 0;
                INTERID = 0;
                ACABID = 0;
                FechaProcesoSHOT = "";
                FechaProcesoPRIM = "";
                FechaProcesoINTER = "";
                FechaProcesoACAB = "";
                OkPintura = false;                
            }

            public int SpoolWorkStatusID { get; set; }
            public string NumeroControl { get; set; }
            public string Cuadrante { get; set; }
            public int Prioridad { get; set; }
            public int SpoolID { get; set; }
            public int OrdenTrabajoSpoolID { get; set; }
            public int ProyectoID { get; set; }
            public int SHOTID { get; set; }
            public int PRIMID { get; set; }
            public int INTERID { get; set; }
            public int ACABID { get; set; }
            public string FechaProcesoSHOT { get; set; }
            public string FechaProcesoPRIM { get; set; }
            public string FechaProcesoINTER { get; set; }
            public string FechaProcesoACAB { get; set; }            
            public bool OkPintura { get; set; }            
            public string Detalle { get; set; }
        }

        public class Detalle
        {
            public string Prueba { get; set; }
            public string UnidadMedida { get; set; }
            public double UnidadMinima { get; set; }
            public double UnidadMaxima { get; set; }
        }

        public class Datos
        {
            public List<ListaElementos> Detalle { get; set; }
        }

        public class CapturaOKMasiva
        {
            public string Detalle { get; set; }
        }

        public class ListaElementos
        {            
            public int SpoolID { get; set; }
            public int OrdenTrabajoSpoolID { get; set; }
            public bool OkPintura { get; set; }

        }

        public class ListaElementosMasivo
        {
            public List<ElementosOKMasivo> Elementos;
        }

        public class ElementosOKMasivo
        {
            public string NumeroControl { get; set; }            
            public string OK { get; set; }
            public string OE { get; set; }
        }

        public class ElementosMasivoResult
        {
            public string NumeroControl { get; set; }
            public string OK { get; set; }
            public string Actualizado { get; set; }
        }

        public class Existe
        {
            public int ExisteNumControl { get; set; }
        }

    }
}