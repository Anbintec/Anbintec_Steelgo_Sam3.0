using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ReporteRT
{
    public class Proyectos
    {
        public Proyectos()
        {
            ProyectoID = 0;
            Nombre = "";
            PatioID = 0;
            PrefijoOrdenTrabajo = "";
        }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int PatioID { get; set; }
        public string PrefijoOrdenTrabajo { get; set; }
    }

    public class TipoPruebaRT
    {
        public TipoPruebaRT()
        {
            TipoPruebaID = 0;
            Nombre = "";
            RequiereEquipoCaptura = false;
        }

        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public bool RequiereEquipoCaptura { get; set; }
    }

    public class Proveedor
    {
        public Proveedor()
        {
            ProveedorID = 0;
            Nombre = "";
            TipoPruebaProveedorID = 0;
            TipoPruebaID = 0;
        }

        public int ProveedorID { get; set; }
        public string Nombre { get; set; }
        public int TipoPruebaProveedorID { get; set; }
        public int TipoPruebaID { get; set; }
    }

    public class Equipo
    {
        public Equipo()
        {
            EquipoID = 0;
            NombreEquipo = "";
            ProveedorEquipoID = 0;
            TipoPruebaProveedorID = 0;
        }

        public int EquipoID { get; set; }
        public string NombreEquipo { get; set; }
        public int ProveedorEquipoID { get; set; }
        public int TipoPruebaProveedorID { get; set; }
    }

    public class Requisicion
    {
        public Requisicion()
        {
            RequisicionID = 0;
            ProyectoID = 0;
            TipoPruebaID = 0;
            NombreRequisicion = "";
            CodigoAsme = "";
            FechaRequisicion = "";
            Observacion = "";
            TurnoLaboral = "";
            TurnoLaboralID = 0;
        }

        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public string NombreRequisicion { get; set; }
        public string CodigoAsme { get; set; }
        public string FechaRequisicion { get; set; }
        public string Observacion { get; set; }
        public int TurnoLaboralID { get; set; }
        public string TurnoLaboral { get; set; }
    }

    public class TurnoLaboral
    {
        public TurnoLaboral()
        {
            TurnoLaboralID = 0;
            Turno = "";
            Capacidad = 0;
            TipoPruebaProveedorID = 0;
            CapacidadTurnoEquipoID = 0;
            CapacidadTurnoProveedorID = 0;
            ElementosAsignados = 0;
            ProveedorEquipoID = 0;
        }

        public int TurnoLaboralID { get; set; }
        public string Turno { get; set; }
        public int Capacidad { get; set; }
        public int TipoPruebaProveedorID { get; set; }
        public int CapacidadTurnoProveedorID { get; set; }
        public int CapacidadTurnoEquipoID { get; set; }
        public int ElementosAsignados { get; set; }
        public int ProveedorEquipoID { get; set; }
    }

    public class Proyecto
    {
        public Proyecto()
        {
            ProyectoID = 0;
            Nombre = "";
            PatioID = 0;
            ProveedorID = 0;
        }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int PatioID { get; set; }
        public int ProveedorID { get; set; }
    }

    public class TipoPrueba
    {
        public TipoPrueba()
        {
            TipoPruebaID = 0;
            Nombre = "";
            TipoPruebaPorSpool = 0;
        }
        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public int TipoPruebaPorSpool { get; set; }
    }

    public class Requisicion2
    {
        public Requisicion2()
        {
            RequisicionID = 0;
            ProyectoID = 0;
            NombreRequisicion = "";
            TipoPruebaID = 0;
            ProveedorID = 0;
        }

        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public string NombreRequisicion { get; set; }
        public int TipoPruebaID { get; set; }
        public int ProveedorID { get; set; }
    }


    public class ElementoRequisicion
    {
        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public int PatioID { get; set; }
        public int ProveedorID { get; set; }
        public List<Proyecto> listaProyecto { get; set; }
        public List<Proveedor> listaProveedor { get; set; }
        public List<Requisicion2> listaRequisicion { get; set; }
        public List<TipoPrueba> listaTipoPrueba { get; set; }
    }

    public class CapturaResultados
    {
        public List<CapturaResultado> Detalles { get; set; }
    }

    public class DetallePorPlacas
    {
        public int CapturaResultadoID { get; set; }
        public int CapturaResultadoPlacaID { get; set; }
        public string Ubicacion { get; set; }
        public int? ResultadoID { get; set; }
        public string Resultado { get; set; }
        public int Accion { get; set; }
        public List<DetalleResultadosDefectos> ListaDetalleDefectos { get; set; }
        public List<Resultados> ListaResultados { get; set; }
        public List<Defectos> ListaDefectos { get; set; }
        public string TemplateDetallePorPlaca { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public string NombreGrafica { get; set; }
    }


    public class Resultados
    {
        public int? ResultadosID { get; set; }
        public string Resultado { get; set; }

        public Resultados()
        {
            ResultadosID = null;
            Resultado = "";
        }
    }

    public class DetalleCaptura
    {
        public string NumeroControl { get; set; }
        public string Junta { get; set; }
        public string NumeroRequisicion { get; set; }
        public string ClasificacionPND { get; set; }
        public string TipoPrueba { get; set; }
        public string Observaciones { get; set; }
        public string ResultadoConciliacion { get; set; }
        public string RazonNoConciliacion { get; set; }
        public int RequisicionID { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public int CapturaResultadoID { get; set; }
        public int Estatus { get; set; }
        public int EquipoID { get; set; }
        public string Equipo { get; set; }
        public int TurnoLaboralID { get; set; }
        public string Turno { get; set; }
        public string CodigoAsme { get; set; }
        public int? NumeroPlacas { get; set; }
        public int Accion { get; set; }
        public int EstatusRequisicion { get; set; }

        public List<DetallePorPlacas> ListaDetallePorPlacas { get; set; }
        public List<Resultados> ListaResultados { get; set; }
        public List<Defectos> ListaDefectos { get; set; }
        public List<RangoCuadrante> listaRangoCuadrantes { get; set; }
        public string TemplateDetalleElemento { get; set; }
        public bool EsSector { get; set; }
        public int NoPlacas { get; set; }
        public List<TurnoLaboral> listadoTurno { get; set; }
        public List<Equipo> listadoEquipo { get; set; }
        public int ProveedorEquipoID { get; set; }
        public int CapacidadTurnoProveedorID { get; set; }
        public int CapacidadTurnoEquipoID { get; set; }
        public int ResultadoID { get; set; }
        public string Resultado { get; set; }

        public int MinimoPlacasCuadrante { get; set; }
        public int MaximoPlacasCuadrante { get; set; }
        public int MinimoPlacasSector { get; set; }
        public int MaximoPlacasSector { get; set; }
        public bool RowOk { get; set; }
        public string NombreGrafica { get; set; }
        public int DefectoID { get; set; }
        public string Indicacion { get; set; }
        public int TieneEntregaPlacas { get; set; }
        public bool RequiereConciliacion { get; set; }
    }



    public class DetalleResultadosDefectos
    {
        public int Accion { get; set; }
        public int CapturaResultadoPlacaDefectoID { get; set; }
        public int CapturaResultadoPlacaID { get; set; }
        public int ResultadoID { get; set; }
        public string Resultado { get; set; }
        public int DefectoID { get; set; }
        public string Defecto { get; set; }
        public decimal InicioMM { get; set; }
        public decimal FinMM { get; set; }
        public int RangoCuadranteID { get; set; }
        public string Ubicacion { get; set; }
        public int ElementoPorClasificacionPNDID { get; set; }
        public string Cuadrante { get; set; }
    }

    public class Defectos
    {
        public int? DefectoID { get; set; }
        public string Defecto { get; set; }

        public Defectos()
        {
            DefectoID = 0;
            Defecto = "";
        }
    }

    public class RangoCuadrante
    {
        public int RangoCuadranteID { get; set; }
        public string Cuadrante { get; set; }

        public RangoCuadrante()
        {
            RangoCuadranteID = 0;
            Cuadrante = "";
        }
    }

    public class CapturaResultado
    {
        public int CapturaResultadoID { get; set; }
        public int RequisicionID { get; set; }
        public int ElementoClasificacionPNDID { get; set; }
        public int ProveedorEquipoID { get; set; }
        public int CapacidadTurnoEquipoID { get; set; }
        public int CapacidadTurnoProveedorID { get; set; }
	    public int Status { get; set; }
        public int Evaluacion { get; set; }
        public int NoPlacas { get; set; }
        public int ResultadoID { get; set; }
        public int Accion { get; set; }
        public string NombreGrafica { get; set; }
        public int DefectoID { get; set; }
        public List<CapturaResultadoPlaca> ListaDetallePorPlacas { get; set; }
        public List<CapturaResultadoIndicacion> listaDetalleIndicacion { get; set; }
    }

    public class CapturaResultadoPlaca
    {
        public int CapturaResultadoPlacaID { get; set; }
        public int ElementoClasificacionPNDID { get; set; }
        public int CapturaResultadoID { get; set; }
        public string Ubicacion { get; set; }
        public int ResultadoID { get; set; }
        public int Accion { get; set; }
        
    }

    public class CapturaResultadoIndicacion
    {
        public int CapturaResultadoPlacaDefectoID { get; set; }
        public int CapturaResultadoPlacaID { get; set; }
        public int ElementoClasificacionPNDID { get; set; }
        public string Ubicacion { get; set; }
        public int ResultadoID { get; set; }
        public int DefectoID { get; set; }
        public decimal InicioMM { get; set; }
        public decimal FinMM { get; set; }
        public int RangoCuadranteID { get; set; }
        public int Accion { get; set; }
    }
}