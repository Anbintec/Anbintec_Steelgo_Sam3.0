using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class ListadoMaterialesPorItemCode
    {
        public string ItemCodeID { get; set; }
        public string ItemCode { get; set; }
        public string ItemCodeSteelgoID { get; set; }
        public string ItemCodeSteelgo { get; set; }
        public string Descripcion { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string TipoMaterial { get; set; }
        public string TotalIngenieria { get; set; }
        public string TotalRecibido { get; set; }
        public string OtrasEntradas { get; set; }
        public string TotalCondicionada { get; set; }
        public string TotalRechazado { get; set; }
        public string TotalDanado { get; set; }
        public string TotalRecibidoNeto { get; set; }
        public string EntradasDesdeICEquivalente { get; set; }
        public string SalidasTemporales { get; set; }
        public string TotalOtrasSalidas { get; set; }
        public string TotalMermas { get; set; }
        public string TotalOrdenTrabajo { get; set; }
        public string DespachadoPorCortarParaICEquivalente { get; set; }
        public string CortadoParaICEquivalente { get; set; }
        public string TotalDespachadoPorCortar { get; set; }
        public string TotalCortado { get; set; }
        public string DespachadoDesdeICEquivalente { get; set; }
        public string DespachadoAProduccion { get; set; }
        public string SalidaParaICEquivalente { get; set; }
        public string TotalPorDespachar { get; set; }
        public string TotalFisicoEnAlmacen { get; set; }
        public string AsignadoParaICEquivalente { get; set; }
        public string TotalCongelado { get; set; }
        public string InventarioDisponibleCruce { get; set; }
        public string DisponibleICEquivalente { get; set; }
        public string TotalDisponibleParaCruce { get; set; }
    }
}