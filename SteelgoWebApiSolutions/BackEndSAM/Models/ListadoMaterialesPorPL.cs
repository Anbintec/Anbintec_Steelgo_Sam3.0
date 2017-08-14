using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoMaterialesPorPL
    {
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string Descripcion { get; set; }
        public string Cedula { get; set; }
        public string CedulaID { get; set; }
        public string TipoAcero { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string RangoInferior { get; set; }
        public string RangoSuperior { get; set; }
        public string Cantidad { get; set; }
        public string Colada { get; set; }
        public string EstatusFisico { get; set; }
        public string EstatusDocumental { get; set; }
        public string AlmacenVirtual { get; set; }
    }

    public class DetalleNumeroUnico
    {
        public string NumeroUnicoID { get; set; }
        public string NumeroUnico { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeID { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Cedula { get; set; }
        public string TotalRecibido { get; set; }
        public string TotalDanado { get; set; }
        public string Profile1 { get; set; }
        public string Profile2 { get; set; }
        public string TotalEntradas { get; set; }
        public string TotalSalidas { get; set; }
        public string SaldoActual { get; set; }
    }

    public class GridSegmento
    {
        public string Segmento { get; set; }
        public string Rack { get; set; }
        public string TotalRecibido { get; set; }
        public string TotalDanado { get; set; }
        public string TotalEntradas { get; set; }
        public string TotalSalidas { get; set; }
        public string SalidasTemporales { get; set; }
        public string SaldoActual { get; set; } 
    }

    public class GridMovimientos
    {
        public string Fecha { get; set; }
        public string Movimiento { get; set; }
        public string Segmento { get; set; }
        public string Entrada { get; set; }
        public string Salida { get; set; }
        public string SaldoActual { get; set; }
        public string Referencia { get; set; }
    }
}