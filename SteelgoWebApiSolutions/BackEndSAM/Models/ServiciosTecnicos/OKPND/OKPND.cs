using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.OKPND
{
    public class Elementos
    {
        public Elementos()
        {
            OKPNDID = 0;
            NumeroControl = "";
            Cuadrante = "";
            Prioridad = 0;
            ProyectoID = 0;
            SpoolID = 0;
            OrdenTrabajoSpoolID = 0;
            OkPND = false;
        }

        public int OKPNDID { get; set; }
        public string NumeroControl { get; set; }
        public string Cuadrante { get; set; }
        public int Prioridad { get; set; }

        public int SpoolID { get; set; }
        public int ProyectoID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public bool OkPND { get; set; }
        public List<Detalle> ListaDetalle { get; set; }
        public string Detalle { get; set; }
    }

    public class Detalle
    {
        public int JuntaSpoolID { get; set; }
        public string Etiqueta { get; set; }
        public string Codigo { get; set; }
        public string Cedula { get; set; }
        public decimal Diametro { get; set; }
        public decimal Espesor { get; set; }
        public string Nombre { get; set; }
        public string TipoPrueba { get; set; }
        public string NumeroRequisicion { get; set; }
    }

    public class Captura{
        public List<ListaElementos> Detalle { get; set; }
    }

    public class CapturaMasiva
    {
        public string Detalle { get; set; }
    }

    public class ListaElementos {
        public int OKPNDID { get; set; }
        public int SpoolID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public bool OkPND { get; set; }
    }

    public class ListaElementosMasivo
    {
        public List<ElementosMasivo> Elementos;
    }

    public class ElementosMasivo
    {
        public string NumeroControl { get; set; }
        public string OKPND { get; set; }
    }

    public class ElementosMasivoResult
    {
        public string NumeroControl { get; set; }
        public string OKPND { get; set; }
        public string Actualizado { get; set; }
    }
}