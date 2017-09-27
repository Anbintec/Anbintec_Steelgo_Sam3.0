using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ValidacionRT
{
    public class Proyectos
    {
        public Proyectos()
        {
            ProyectoID = 0;
            Nombre = "";
            PatioID = 0;
            PrefijoOrdenTrabajo = "";
            RequisicionID = 0;
            TipoPruebaID = 0;
        }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int PatioID { get; set; }
        public string PrefijoOrdenTrabajo { get; set; }
        public int RequisicionID { get; set; }
        public int TipoPruebaID { get; set; }
    }

    public class TiposDePrueba
    {
        public TiposDePrueba()
        {
            TipoPruebaID = 0;
            Nombre = "";
            Categoria = "";
            TipoPruebaPorSpool = 0;
            RequiereEquipo = false;
        }

        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int TipoPruebaPorSpool { get; set; }
        public bool RequiereEquipo { get; set; }
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
        }

        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public string NombreRequisicion { get; set; }
        public string CodigoAsme { get; set; }
        public string FechaRequisicion { get; set; }
        public string Observacion { get; set; }
    }



    public class Proveedor
    {
        public Proveedor()
        {
            ProveedorID = 0;
            Nombre = "";
            TipoPruebaProveedorID = 0;
        }

        public int ProveedorID { get; set; }
        public string Nombre { get; set; }
        public int TipoPruebaProveedorID { get; set; }        
    }

    public class LoginProveedor
    {
        public LoginProveedor()
        {
            ProveedorID = 0;
            Nombre = "";
        }

        public int ProveedorID { get; set; }
        public string Nombre { get; set; }
    }

    public class GridElement
    {
        public string NumeroControl { get; set; }
        public string Etiqueta { get; set; }
        public string NumeroRequisicion { get; set; }
        public string ClasificacionPND { get; set; }
        public string TipoPrueba { get; set; }
        public string Observaciones { get; set; }
        public string CodigoAsme { get; set; }
        public int RequisicionID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public int ReporteRTID { get; set; }
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
        public int RequisicionID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public decimal? Tamano { get; set; }
        public decimal? Densidad { get; set; }
        public int Accion { get; set; }
        public int EstatusRequisicion { get; set; }
        public int Junta { get; set; }
        public string ClasificacionPND { get; set; }
        public string TipoPrueba { get; set; }
        public string Observaciones { get; set; }
        public string CodigoAsme { get; set; }
        public int? NumeroPlacas { get; set; }
        public List<DetallePorPlacas> ListaDetallePorPlacas { get; set; }
        public string NumeroControl { get; set; }
        public string ResultadoConciliacion { get; set; }
        public string RazonNoConciliacion { get; set; }
        public List<Resultados> ListaResultados { get; set; }
        public List<Defectos> ListaDefectos { get; set; }
        public string TemplateDetalleElemento { get; set; }
        public int EquipoID { get; set; }
        public string Equipo { get; set; }
        public int TurnoLaboralID { get; set; }
        public string Turno { get; set; }
        public bool TipoRT { get; set; }
    }


    public class DetalleResultados
    {
        public int ResultadosDefectoID { get; set; }
        public int ReporteResultadosID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }

        public int DefectoID { get; set; }
        public int InicioMM { get; set; }
        public int FinMM { get; set; }
        public int Accion { get; set; }

    }

    public class DetalleResultadosDefectos
    {
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaSpoolID { get; set; }
        public int? DefectoID { get; set; }
        public string Defecto { get; set; }
        public decimal? InicioMM { get; set; }
        public decimal? FinMM { get; set; }
        public int Accion { get; set; }
        public int Posicion { get; set; }
        public string Ubicacion { get; set; }
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

    public class Equipo
    {
        public Equipo()
        {
            EquipoID = 0;
            NombreEquipo = "";
        }
        public int EquipoID { get; set; }
        public string NombreEquipo { get; set; }
    }

    public class TurnoLaboral
    {
        public TurnoLaboral()
        {
            TurnoLaboralID = 0;
            Turno = "";
        }

        public int TurnoLaboralID { get; set; }
        public string Turno { get; set; }
    }

}