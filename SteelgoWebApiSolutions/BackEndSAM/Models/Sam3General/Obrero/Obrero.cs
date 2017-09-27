using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Sam3General
{
    public class Obrero
    {
        public int ObreroID { get; set; }
        public int TipoObreroID { get; set; }
        public string Codigo { get; set; }
        public string NumeroEmpleado { get; set; }
        public bool Activo { get; set; }
        public int UsuarioModificacion { get; set; }
        public string TipoObrero { get; set; }
        public string NombreCompleto { get; set; }

        public Obrero()
        {
            ObreroID = 0;
            TipoObreroID = 0;
            Codigo = "";
            NumeroEmpleado = "";
            Activo = false;
            UsuarioModificacion = 0;
            TipoObrero = "";
            NombreCompleto = "";
        }

    }
    public class TipoObreroModel
    {
        public int TipoObreroID { get; set; }
        public string TipoObrero { get; set; }
        public int TipoObreroJefe { get; set; }
        public string TipoObreroJefeNombre { get; set; }
        public bool Activo { get; set; }
        public int UsuarioModificacion { get; set; }
    }
    public class ObreroUbicacion
    {
        public int ObreroUbicacionID { get; set; }
        public int ObreroID { get; set; }
        public int PatioID { get; set; }
        public string FechaInicioLabor { get; set; }
        public string FechaFinLabor { get; set; }
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public bool Activo { get; set; }
        public int UsuarioModificacion { get; set; }
    }

    public class ObreroSteelGo
    {
        public int ObreroID { get; set; }
        public string Codigo { get; set; }
        public string TipoObrero { get; set; }
        public string NombreCompleto { get; set; }
        public ObreroSteelGo()
        {
            ObreroID = 0;
            Codigo = "";
            TipoObrero = "";
            NombreCompleto = "";
        }
    }
}