using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.PlaneacionYControl
{
    public class EmisionOT
    {
    }

    public class DetalleProyectoPrueba
    {
        public int TipoProductoID { get; set; }
        public string TipoProducto { get; set; }
        public int FamiliaAceroID { get; set; }
        public string FamiliaAcero { get; set; }
        public int AceroID { get; set; }
        public string Acero { get; set; }
        public string Peso { get; set; }
        public string Spools { get; set; }
        public string Juntas { get; set; }
        public string Area { get; set; }
        public string Peqs { get; set; }
        public string FabLine { get; set; }
        public List<DetalleSpoolPrueba> ListaSpools { get; set; }
        public List<DetallePatio> ListaPatio { get; set; }
    }

    public class DetalleSpoolPrueba
    {
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int Seleccionado { get; set; }
        public int Proyectado { get; set; }
        public string Proyeccion { get; set; }
        public int TipoID { get; set; }
        public string Tipo { get; set; }
        public decimal Juntas { get; set; }
        public decimal Peqs { get; set; }
        public string SpoolNombre { get; set; }
        public string Dibujo { get; set; }
        public decimal DiametroMaximo { get; set; }
        public string DiametroPromedio { get; set; }
        public decimal Peso { get; set; }
        public decimal Area { get; set; }
        public List<DetalleJuntasPrueba> ListaJuntas { get; set; }
    }

    public class DetalleJuntasPrueba
    {
        public int TipoJuntaID { get; set; }
        public int FabclasID { get; set; }
        public string Fabclas { get; set; }
        public string TipoJunta { get; set; }
        public string Junta { get; set; }
        public decimal Peqs { get; set; }
    }

    public class DetalleProyecto
    {
        public int ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public List<DetallePatio> ListaPatio { get; set; }
    }

    public class DetallePatio
    {
        public int PatioID { get; set; }
        public string NombrePatio { get; set; }
    }

    public class DetalleTaller
    {
        public int TallerID { get; set; }
        public string Taller { get; set; }
        public decimal Capacidad { get; set; }
        public string Unidad { get; set; }
        public DetalleProduccion Produccion { get; set; }
    }

    public class DetalleProduccion
    {
        public int ProyeccionID { get; set; }
        public decimal CantidadAutomatico { get; set; }
        public decimal CantidadManual { get; set; }
    }
}