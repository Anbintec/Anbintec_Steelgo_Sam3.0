using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.CargaPlana
{
    public class DetalleCargaPlana
    {
        public int Accion { get; set; }
        public int DetalleCargaID { get; set; }
        public int SpoolID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public string Spool { get; set; }
        public int PaqueteID { get; set; }
        public string Paquete { get; set; }
        public decimal Peso { get; set; }
        public int CuadranteID { get; set; }
        public int CuadranteAnteriorID { get; set; }
        public int ZonaAnteriorID { get; set; }
        public int CuadrantePaqueteAnteriorID { get; set; }
        public int ZonaPaqueteAnteriorID { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }
        public int EmpaquetadoInicial { get; set; }
        public decimal Pdi { get; set; }
        public decimal Peq { get; set; }
    }

    public class DetalleSpoolAgregar
    {
        public int Accion { get; set; }
        public int DetalleCargaID { get; set; }
        public int SpoolID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public string Spool { get; set; }
        public int Empaquetado { get; set; }
        public string Paquete { get; set; }
        public int PaqueteID { get; set; }
        public decimal Peso { get; set; }
        public int ProyectoID { get; set; }
        public int CuadranteID { get; set; }
        public int CuadranteAnteriorID { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public int Cargado { get; set; }
        public string PlanaCargado { get; set; }
        public string CarroCargado { get; set; }
        public bool RowOk { get; set; }
        public int EmpaquetadoInicial { get; set; }
        public decimal Pdi { get; set; }
        public decimal Peq { get; set; }
    }

    public class DetallePaqueteAgregar
    {
        public int Accion { get; set; }
        public int DetalleCargaID { get; set; }
        public int SpoolID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public string Spool { get; set; }
        public int PaqueteID { get; set; }
        public string Paquete { get; set; }
        public decimal Peso { get; set; }
        public int CuadranteID { get; set; }
        public int CuadranteAnteriorID { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public int Cargado { get; set; }
        public string PlanaCargado { get; set; }
        public bool RowOk { get; set; }
        public int EmpaquetadoInicial { get; set; }
        public decimal Pdi { get; set; }
        public decimal Peq { get; set; }
    }

    public class Captura
    {
        public List<DetalleGuardadoCargaPlana> listaDetalle { get; set; }
    }

    public class DetalleGuardadoCargaPlana
    {
        public int Accion { get; set; }
        public int DetalleCargaID { get; set; }
        public int SpoolID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int CuadranteActualID { get; set; }
        public int PaqueteID { get; set; }
        public int EmpaquetadoInicial { get; set; }
    }

    public class DetallePaquete
    {
        public DetallePaquete()
        {
            PaqueteID = 0;
            Nombre = "";
            CuadranteID = 0;
        }

        public int PaqueteID { get; set; }
        public string Nombre { get; set; }
        public int CuadranteID { get; set; }
    }


    public class DetalleSpoolValidar {
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public string Paquete { get; set; }
        public string Plana { get; set; }
        public int EsCorrecto { get; set; }
    }

}