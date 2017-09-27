using BackEndSAM.Models.ServiciosTecnicos.AsignarRequisicion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.ServiciosTecnicosGeneral
{
    public class TiposDePrueba {
        public TiposDePrueba() {
            TipoPruebaID = 0;
            Nombre = "";
            Categoria = "";
            TipoPruebaPorSpool = 0;
            RequiereEquipo = false;
            ClasificacionManual = "";
            ClasificacionManualID = 0;
        }

        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public int TipoPruebaPorSpool { get; set; }
        public bool RequiereEquipo { get; set; }
        public int ClasificacionManualID { get; set; }
        public string ClasificacionManual { get; set; }
    }

    public class Requisicion
    {
        public Requisicion()
        {
            RequisicionID = 0;
            ProyectoID = 0;
            TipoPruebaID = 0;
            NombreRequisicion = "";
            FolioCliente = "";
            FechaRequisicion = "";
            Observacion = "";
        
        }

        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public string NombreRequisicion { get; set; }
        public string FolioCliente { get; set; }
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
        public int TipoPruebaProveedorID { get; set; }
        public string Nombre { get; set; }
    }

    public class Equipo
    {
        public Equipo()
        {
            EquipoID = 0;
            Nombre = "";
            ProveedorEquipoID = 0;
            TipoPruebaProveedorID = 0;
        }
        public int EquipoID { get; set; }
        public string Nombre { get; set; }
        public int ProveedorEquipoID { get; set; }
        public int TipoPruebaProveedorID { get; set; }
    }

    public class TurnoLaboral
    {

        public TurnoLaboral()
        {
            TurnoLaboralID = 0;
            Nombre = "";
            Capacidad = 0;
            TipoPruebaProveedorID = 0;
            CapacidadTurnoProveedorID = 0;
            CapacidadTurnoEquipoID = 0;
            JuntasAsignadas = "0";
            ElementosAsignados = 0;
            ProveedorEquipoID = 0;
            ListaElementosAsignadosTurno = new List<ElementosRequisicion>();
        }
        public int TurnoLaboralID { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public int TipoPruebaProveedorID { get; set; }
        public int CapacidadTurnoProveedorID { get; set; }
        public int CapacidadTurnoEquipoID { get; set; }
        public string JuntasAsignadas {get; set;}
        public int ElementosAsignados { get; set; }
        public int ProveedorEquipoID { get; set; }
        public List<ElementosRequisicion> ListaElementosAsignadosTurno { get; set; }
    }


    public class ElementoRequisicion
    {
        public int RequisicionID { get; set; }
        public int ProyectoID { get; set; }
        public int TipoPruebaID { get; set; }
        public int PatioID { get; set; }
        public List<Proyecto> listaProyecto { get; set; }
        public List<Requisicion> listaRequisicion { get; set; }
        public List<TiposDePrueba> listaTipoPrueba { get; set; }
    }
}