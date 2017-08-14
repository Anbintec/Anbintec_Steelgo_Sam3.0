using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models
{
    public class DashBoardDespacho
    {
        public string CantidadODT { get; set; }
        public string CantidadODTActiva { get; set; }
        public string PreDespacho { get; set; }
        public string PorDespachar { get; set; }
        public string PorEntregar { get; set; }
        public string TrevelerPendiente { get; set; }
    }

    public class ListadoODTDespacho
    {
        public int ProyectoID { get; set; }
        public string SpoolID { get; set; }
        public string Spool { get; set; }
        public string NumeroControlID { get; set; }
    }

    public class ListadoTravelerPendientePorSpool
    {
        public int ProyectoID { get; set; }
        public string SpoolID { get; set; }
        public string Spool { get; set; }
        public string NumeroControlID { get; set; }
    }

    public class ListadoPorSpoolPreDespacho
    {
        public string ID { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadPredespachada { get; set; }
        public string CantidadPorPredespachar { get; set; }
    }

    public class ListadoPreDespacho
    {
        public string IC { get; set; }
        public string DescripcionIC { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Cedula { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadPredespachada { get; set; }
        public string CantidadPorPredespachar { get; set; }
    }

    public class InformacionSpool
    {
        public string SpoolID { get; set; }
        public string MaterialSpoolID { get; set; }
        public string ProyectoID { get; set; }
        public string OrdenTrabajoSpoolID { get; set; }
    }

    public class ListadoPorSpoolDespacho
    {
        public string ID { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadDespachada { get; set; }
        public string CantidadPorDespachar { get; set; }
    }

    public class ListadoDespacho
    {
        public string IC { get; set; }
        public string DescripcionIC { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Cedula { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadDespachada { get; set; }
        public string CantidadPorDespachar { get; set; }
    }

    public class ListadoPorSpoolEntrega
    {
        public string ID { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadEntregada { get; set; }
        public string CantidadPorEntregar { get; set; }
    }

    public class ListadoEntregaDash
    {
        public string IC { get; set; }
        public string DescripcionIC { get; set; }
        public string D1 { get; set; }
        public string D2 { get; set; }
        public string Cedula { get; set; }
        public string CantidadTotal { get; set; }
        public string CantidadEntregada { get; set; }
        public string CantidadPorEntregar { get; set; }
    }

    public class MedidaToneladas
    {
        public string itemCode { get; set; }
        public string itemCodeSteelgo { get; set; }
        public string material { get; set; }
    }

    public class CantidadMateriales
    {
        public int? NumeroUnicoID { get; set; }
        public int MaterialSpoolID { get; set; }
        public int ItemCodeID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
    }

    public class CantidadSpools
    {
        public int MaterialSpoolID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public int SpoolID { get; set; }
        public int ItemCodeIDSam2 { get; set; }
        public int OrdenTrabajoID { get; set; }
    }
}