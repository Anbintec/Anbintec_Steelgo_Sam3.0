using BackEndSAM.Models.Sam3General.Cuadrante;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.CargaCarro
{
    public class MedioTransporteCarga
    {
        public int MedioTransporteCargaID { get; set; }
        public string NombreMedioTransporte { get; set; }

        public MedioTransporteCarga()
        {
            MedioTransporteCargaID = 0;
            NombreMedioTransporte = "";
        }

    }

    public class DetalleSpool
    {
        public int Accion { get; set; }
        public string SpoolJunta { get; set; }
        public int OrdenImportancia { get; set; }
        public decimal Peso { get; set; }
        public decimal Area { get; set; }
        public string SistemaPintura { get; set; }
        public int SpoolID { get; set; }
        public int SistemaPinturaID { get; set; }
        public int MedioTransporteCargaID { get; set; }
        public int CuadranteID { get; set; }
        public int ProyectoID { get; set; }
        public string NombreMedioTransporte { get; set; }
        public string CuadranteMedioTransporte { get; set; }
        public string ColorPintura { get; set; }
        public int PinturaSpoolID { get; set; }

    }

    public class Captura
    {
        public List<GuardarCarga> Detalles { get; set; }
    }

    public class GuardarCarga
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int MedioTransporteCargaID { get; set; }
        public int CuadranteID { set; get; }
       // public int SistemaPinturaID { get; set; }
    }

    public class DetalleCargaCarro
    {
        public int? PatioID { get; set; }
        public int? Accion { get; set; }
        public int? MedioTransporteCargaDetalleID { get; set; }
        public bool? EstatusCarga { get; set; }
        public int? OrdenTrabajoID { get; set; }
        public int? SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public int SistemaPinturaID { get; set; }
        public string SistemaPintura { get; set; }
        public string ColorPintura { get; set; }
        public int CuadranteID { get; set; }
        public int CuadranteAnteriorID { get; set; }
        public string Cuadrante { get; set; }
        public decimal Peso { get; set; }
        public decimal Area { get; set; }
        public int EstatusCaptura { get; set; }
        public int Prioridad { get; set; }
        public string MedioTransporte { get; set; }
        public int? CarroID { get; set; }
        public bool CarroCerrado { get; set; }
        public bool Seleccionado { get; set; }
        public int? ZonaAnteriorID { get; set; }
        public bool? NoPintable { get; set; }
    }

  
    public class CerrarMedioTransporte
    {
        public int CerrarCarro { get; set; }
        public int MedioTransporteID { get; set; }
        public int MedioTransporteCargaID { get; set; }
    }

    public class CapturaDescarga
    {
        public List<GuardarDescarga> Detalles { get; set; }
    }
    
    public class GuardarDescarga
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int MedioTransporteID { get; set; }
        public int CuadranteID { get; set; }
    }
    
    public class CamposPredeterminados
    {
        public string Vista { get; set; }
        public string Opcion { get; set; }
        public string Muestra { get; set; }
    }

 }