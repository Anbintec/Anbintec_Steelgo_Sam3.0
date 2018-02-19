using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Pintura.PruebasPorLote
{
    
    public class InformacionSpool
    {
        public string NumeroControl { get; set; }
        public string SistemaPintura { get; set; }
        public string Color { get; set; }
        public decimal? Area { get; set; }
        public string LoteID { get; set; }
        public string NombreCuadrante { get; set; }

    }
    public class PruebasSpool
    {
        public int ProyectoProcesoPruebaID { get; set; }
        public string Prueba { get; set; }
        public double UnidadMaxima { get; set; }
        public double UnidadMinima { get; set; }

        public string UnidadMedida { get; set; }
        public PruebasSpool()
        {
            ProyectoProcesoPruebaID = 0;
            Prueba = "";
            UnidadMaxima = 0;
            UnidadMinima = 0;
            UnidadMedida = "";
        }

    }
    public class ProcesosPinturaSpool
    {
        public int SpoolID { get; set; }
        public int? ProcesoPinturaID { get; set; }
        public string ProcesoPintura { get; set; }
        public string Status { get; set; }
        public int? SistemaPinturaProyectoProcesoID { get; set; }
        public int? SistemaPinturaProyectoID { get; set; }

        public ProcesosPinturaSpool()
        {
            SpoolID = 0;
            ProcesoPinturaID = 0;
            ProcesoPintura = "";
            Status = "";
            SistemaPinturaProyectoProcesoID = 0;
            SistemaPinturaProyectoID = 0;
        }
    }
    public class ProcesosPintura
    {
        public int ProcesoPinturaID { get; set; }
        public string ProcesoPintura { get; set; }

        public ProcesosPintura()
        {
            this.ProcesoPinturaID = 0;
            this.ProcesoPintura = "";
        }
    }
    public class SistemaPinturaLotes
    {
        public int SistemaPinturaID { get; set; }
        public string SistemaPintura { get; set; }
        public int SistemaPinturaProyectoID { get; set; }
        public SistemaPinturaLotes()
        {
            this.SistemaPinturaID = 0;
            this.SistemaPintura = "";
            this.SistemaPinturaProyectoID = 0;
        }
    }
    public class Pruebas
    {
        public int PruebaProcesoPinturaID { get; set; }
        public string Prueba { get; set; }
        public double UnidadMaxima { get; set; }
        public double UnidadMinima { get; set; }

        
        public Pruebas()
        {
            this.PruebaProcesoPinturaID = 0;
            this.Prueba = "";
            this.UnidadMaxima = 0;
            this.UnidadMinima = 0;
           
        }
    }

    public class Lotes
    {
        public int? LoteID { get; set; }
        public string Nombre { get; set; }

        public Lotes()
        {
            this.LoteID = 0;
            this.Nombre = "";
        }
    }

    public class FechasPrueba
    {
        public string Fecha { get; set; }
    }

    public class DetalleLote
    {
        public int? SpoolID { get; set; }
        public string NumeroControl { get; set; }
        public string SistemaPintura { get; set; }
        public string Color { get; set; }
        public Double Area { get; set; }
        public string Cuadrante { get; set; }
        public int? PruebaProcesoPinturaID { get; set; }
        public decimal? UnidadMinima { get; set; }
        public decimal? UnidadMaxima { get; set; }
        public int? PruebasRequeridas { get; set; }
        public int? PruebasEjecutadas { get; set; }
        public bool? CerrarLote { get; set; }
        public int? ProyectoProcesoPruebaID { get; set; }
        public string Template { get; set; }
        public List<DetallePruebasPorSpool> ListaDetallePruebas { get; set; }
        public string Medida { get; set; }
      

        public int Accion { get; set; }
    }

    public class DetallePruebasPorSpool
    {
        public int? Lote { get; set; }
        public int Accion { get; set; }
        public int? SpoolID { get; set; }
        public int? ProyectoProcesoPruebaID { get; set; }
        public double UnidadMedida { get; set; }
        public bool? ResultadoEvaluacion { get; set; }
        public string FechaPrueba { get; set; }
        public double? UnidadMaxima { get; set; }
        public double? UnidadMinima { get; set; }
        public string Medida { get; set; }
        public int PruebaLoteID { get; set; }
        public int? InspectorID { get; set; }
        public string Inspector { get; set; }
    }


    public class Captura
    {
        public List<DetalleGuardarPruebasPorSpool> Detalles { get; set; }
    }
    public class DetalleGuardarPruebasPorSpool
    {
        public int LoteID { get; set; }
        public int Accion { get; set; }
        public int SpoolID { get; set; }
        public int ProyectoProcesoPruebaID { get; set; }
        public double UnidadMedida { get; set; }
        public string FechaPrueba { get; set; }
        public bool ResultadoEvaluacion { get; set; }
        public int SistemaPinturaColorID { get; set; }
        public int PruebaLoteID { get; set; }
        public int InspectorID { get; set; }
    }
}