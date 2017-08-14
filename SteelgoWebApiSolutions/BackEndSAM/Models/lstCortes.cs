using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class LstCortes
    {
        
    }

    public class ListaComboNumeroUnicoCOrte
    {
        public string NumeroControl { get; set; }
        public string NumeroControlID { get; set; }
    }

    public class DatosBusquedaODT
    {
        public string NumerUnicoID { get; set; }
        public string OrdenTrabajo { get; set; }
        public string Consecutivo { get; set; }
        public string Etiqueta { get; set; }
        public string EsTramoCompleto { get; set; }
        public string SpoolID { get; set; }
        public decimal Cantidad { get; set; }
        public decimal CantidadIngenieria { get; set; }
        public int ProyectoID { get; set; }
        public string token { get; set; }
        public int CorteDetalleID { get; set; }
        public int MaterialSpoolID { get; set; }
        public int OrdenTrbajoSpoolID { get; set; }
        public decimal CantidadBak { get; set; }
    }

    public class ParametrosBusquedaODT
    {
        public DatosBusquedaODT DatosODT { get; set; }

        public ParametrosBusquedaODT()
        {
            DatosODT = new DatosBusquedaODT();
        }
    }

    public class DetalleNumeroUnicoCorte
    {
        public string Cantidad { get; set; }
        public string ItemCode { get; set; }
        public string D1 { get; set; }
        public string Tolerancia { get; set; }
        public List<DatosBusquedaODT> ListadoCortes { get; set; }

        public DetalleNumeroUnicoCorte()
        {
            ListadoCortes = new List<DatosBusquedaODT>();
        }
    }

    public class DetalleOdtsCorte
    {
        public string NumeroControl { get; set; }
        public string Etiqueta { get; set; }
        public string Consecutivo {get; set;}
    }
}