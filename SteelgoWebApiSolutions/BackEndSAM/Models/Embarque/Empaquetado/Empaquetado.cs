using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Embarque.Empaquetado
{
    public class DetallePaquete
    {
        public DetallePaquete()
        {
            PaqueteID = 0;
            Nombre = "";
            Cerrado = false;
            FechaCreacion = "";
            ProyectoID = 0;
            CuadrantePaqueteSam2ID = 0;
            CuadrantePaqueteSam3ID = 0;
            CuadranteUbicacion = 0;
            ZonaID = 0;
            Elementos = 0;
            CuadranteUbicacionAnt = 0;
            ZonaUbicacionAnt = 0;
        }

        public int PaqueteID { get; set; }
        public string Nombre { get; set; }
        public bool Cerrado { get; set; }
        public string FechaCreacion { get; set; }
        public int ProyectoID { get; set; }
        public int CuadrantePaqueteSam2ID { get; set; }
        public int CuadrantePaqueteSam3ID { get; set; }
        public int CuadranteUbicacion { get; set; }
        public int ZonaID { get; set; }
        public int CuadranteUbicacionAnt { get; set; }
        public int ZonaUbicacionAnt { get; set; }
        public int Elementos { get; set; }
        public int CargaPlana { get; set; }
    }

    public class ZonaPaquete
    {
        public ZonaPaquete()
        {
            ZonaID = 0;
            Nombre = "";
        }
        public int ZonaID { get; set; }
        public string Nombre { get; set; }
    }

    public class CuadrantePaquete
    {
        public CuadrantePaquete() {
            CuadranteID = 0;
            Nombre = "";
            ZonaID = 0;
        }
        public int CuadranteID { get; set; }
        public string Nombre { get; set; }
        public int ZonaID { get; set; }
    }

    public class DetalleCargaPaquete
    {
        public int Accion { get; set; }
        public int EmpaquetadoID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public decimal Area { get; set; }
        public decimal Peso { get; set; }
        public int CuadranteSam2ID { get; set; }
        public int CuadranteSam3ID { get; set; }
        public string Cuadrante { get; set; }
        public int CuadranteAnteriorSam2ID { get; set; }
        public int CuadranteAnteriorSam3ID { get; set; }
        public int ZonaAnteriorID { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }
    }

    public class DetalleSpoolAgregar
    {
        public int Accion { get; set; }
        public int EmpaquetadoID { get; set; }
        public int SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public decimal Area { get; set; }
        public decimal Peso { get; set; }
        public int ProyectoID { get; set; }
        public int? CuadranteSam2ID { get; set; }
        public int? CuadranteSam3ID { get; set; }
        public string Cuadrante { get; set; }
        public int? CuadranteAnteriorSam2ID { get; set; }
        public int? CuadranteAnteriorSam3ID { get; set; }
        public int? ZonaAnteriorID { get; set; }
        public int Empaquetado { get; set; }
        public string Paquete { get; set; }
        public int CargaPlana { get; set; }
        public string Plana { get; set; }
        public string CarroCargado { get; set; }
        public bool ModificadoPorUsuario { get; set; }
        public bool RowOk { get; set; }
    }

    public class Captura
    {
        public List<DetalleGuardadoCargaPaquete> listaDetalle { get; set; }
    }

    public class DetalleGuardadoCargaPaquete
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int CuadranteActualSam2ID { get; set; }
        public int CuadranteActualSam3ID { get; set; }
    }
}