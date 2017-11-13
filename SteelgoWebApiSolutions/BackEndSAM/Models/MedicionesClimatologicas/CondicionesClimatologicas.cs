using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.MedicionesClimatologicas
{
    public class EquiposDeToma
    {
        public EquiposDeToma()
        {
            Equipos = new List<ListasEquipos>();
        }
        public List<ListasEquipos> Equipos { get; set; }
    }

    public class ListasEquipos
    {
        public ListasEquipos()
        {
            EquiposHumedad = new List<DetalleEquipos>();
            EquiposTemperaturaAmbiente = new List<DetalleEquipos>();
            EquiposPuntoRocio = new List<DetalleEquipos>();
            EquiposCampoX = new List<DetalleEquipos>();
        }

        public List<DetalleEquipos> EquiposHumedad { get; set; }
        public List<DetalleEquipos> EquiposTemperaturaAmbiente { get; set; }
        public List<DetalleEquipos> EquiposPuntoRocio { get; set; }
        public List<DetalleEquipos> EquiposCampoX { get; set; }
    }

    public class DetalleEquipos
    {
        public DetalleEquipos()
        {
            EquipoTomaID = 0;
            NombreEquipo = "";
            NombreCategoria = "";
        }

        public int EquipoTomaID { get; set; }
        public string NombreEquipo { get; set; }
        public string NombreCategoria { get; set; }
    }

    public class Captura
    {
        public List<DetalleCapturaCondiciones> Detalles { get; set; }
    }

    public class DetalleCapturaCondiciones
    {
        public string FechaToma { get; set; }
        public string HoraToma { get; set; }
        public int PatioID { get; set; }
        public int ZonaID { get; set; }
        public string TempAmb { get; set; }
        public int EquipoTomaTemAmbID { get; set; }
        public int Humedad { get; set; }
        public int EquipoTomaHumedadID { get; set; }
        public int PuntoRocio { get; set; }
        public int EquipoTomaPuntoRocioID { get; set; }
        public int CampoX { get; set; }
        public int EquipoTomaCampoXID { get; set; }

    }
}