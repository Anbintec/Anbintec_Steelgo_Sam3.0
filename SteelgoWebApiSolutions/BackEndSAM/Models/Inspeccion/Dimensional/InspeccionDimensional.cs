using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Inspeccion.Dimensional
{
    public class InspeccionDimensional
    {
        public class CamposPred
        {
            public string Fecha { get; set; }
            public string Resultado { get; set; }
            public string Llena { get; set; }
            public string Muestra { get; set; }
        }

        public class SpoolGrid
        {
            public List<DetalleGridSpool> Detalles { get; set; }
        }

        public class DetalleGridSpool
        {
            public int OrdenTrabajoSpoool { get; set; }
        }

        public class Captura
        {
            public List<DetalleGuardarJson> Detalles { get; set; }
        }

        public class DetalleDatosJson
        {
            public int Accion { get; set; }
            public int InspeccionDimensionalID { get; set; }
            public int ProyectoID { get; set; }
            public string Proyecto { get; set; }
            public string OrdenTrabajoSpoolID { get; set; }
            public string OrdenTrabajoSpool { get; set; }
            public string ResultadoID { get; set; }
            public string Resultado { get; set; }
            public List<Resultado> ListaResultados { get; set; }
            public string DefectosID { get; set; }
            public string Defectos { get; set; }
            public string DefectoInicialID { get; set; }
            public string DefectoInicial { get; set; }
            public List<Defectos> ListaDefectos { get; set; }
            public string InspectorID { get; set; }
            public string Inspector { get; set; }
            public List<Inspector> ListaInspector { get; set; }
            public string FechaInspeccion { get; set; }
            public List<JuntaXSpool> ListaJuntas { get; set; }
            public List<JuntaXSpool> ListaJuntasSeleccionadas { get; set; }
            public List<JuntaXSpool> ListaJuntasSeleccionadasInicial { get; set; }
            public string TemplateRender { get; set; }
            public int IDDEFECTOTIPO { get; set; }
            public string TIPO { get; set; }

            public bool RowOK { get; set; }
        }

        public class DetalleGuardarJson
        {
            public int Accion { get; set; }
            public int InspeccionDimensionalID { get; set; }
            public string OrdenTrabajoSpoolID { get; set; }
            public string ResultadoID { get; set; }
            public string DefectosID { get; set; }
            public string InspectorID { get; set; }
            public string FechaInspeccion { get; set; }
            public List<JuntaXSpoolGuardar> ListaJuntas { get; set; }
        }

        public class JuntaXSpool
        {
            public int Accion { get; set; }
            public int JuntaID { get; set; }
            public string Junta { get; set; }
        }

        public class JuntaXSpoolGuardar
        {
            public int Accion { get; set; }
            public string DefectoID { get; set; }
            public int JuntaID { get; set; }
            public int OrdenTrabajoSpoolID { get; set; }
        }

        public class Resultado
        {
            public string _ResultadoID { get; set; }
            public string _Resultado { get; set; }

            public Resultado()
            {
                _ResultadoID = "";
                _Resultado = "";
            }
        }

        public class Inspector
        {
            public Inspector()
            {
                ObreroID = 0;
                Codigo = "";
                NombreCompleto = "";
            }
            public int ObreroID { get; set; }
            public string Codigo { get; set; }
            public string NombreCompleto { get; set; }
        }
        public class Defectos
        {
            public int DefectoID { get; set; }
            public string Nombre { get; set; }
            public int IDDEFECTOTIPO { get; set; }
            public string TIPO { get; set; }

            public Defectos()
            {
                DefectoID = 0;
                Nombre = "";
                IDDEFECTOTIPO = 0;
                TIPO = "NoEspecificarJunta";
            }
        }
    }
}