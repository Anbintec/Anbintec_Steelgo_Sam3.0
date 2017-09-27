using BackEndSAM.Models.Armado;
using BackEndSAM.Models.Inspeccion.Dimensional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Inspeccion.VisualDimensional
{
    public class CapturaVisualDimensional
    {
        public int AccionNumeroUnico { get; set; }
        public int Accion { get; set; }
        public int ProyectoID { get; set; }
        public string Proyecto { get; set; }
        public string OrdenTrabajoID { get; set; }
        public string OrdenTrabajo { get; set; }
        public string OrdenTrabajoSpoolID { get; set; }
        public string OrdenTrabajoSpool { get; set; }
        public string SpoolID { get; set; }
        public int JuntaID { get; set; }
        public string Junta { get; set; }
        public string TipoJunta { get; set; }
        public string TipoJuntaID { get; set; }
        public decimal Diametro { get; set; }
        public string Resultado { get; set; }
        public string ResultadoID { get; set; }
        public string TallerID { get; set; }
        public string Taller { get; set; }
        public List<Taller> ListaTaller { get; set; }
        public string DefectosID { get; set; }
        public string Defectos { get; set; }
        public List<Defectos> ListaDefectos { get; set; }
        public string InspectorID { get; set; }
        public string Inspector { get; set; }
        public List<Inspector> ListaInspector { get; set; }
        public string FechaInspeccion { get; set; }

        public string NumeroUnico1ID { get; set; }
        public string NumeroUnico2ID { get; set; }

        public string NumeroUnico1 { get; set; }
        public string NumeroUnico2 { get; set; }
        public List<NumeroUnico> ListaNumerosUnicos1 { get; set; }
        public List<NumeroUnico> ListaNumerosUnicos2 { get; set; }
        public List<Resultado> ListaResultados { get; set; }
        
        public string DetalleJunta { get; set; }

        public string Localizacion { get; set; }
        public bool? RevisionNU  { get; set; }

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
    public class NumeroUnico
    {
        public int NumeroUnicoID { get; set; }
        public string Clave { get; set; }

        public int EtiquetaMaterial { get; set; }

        public string Etiqueta { get; set; }

        public string JuntasEncontradas { get; set; }

        public int? LongitudMaterial { get; set; }

        public int? ItemCodeID { get; set; }

        public int? TipoMaterialID { get; set; }

        public string Nombre { get; set; }

        public NumeroUnico()
        {
            NumeroUnicoID = 0;
            Clave = "";
            EtiquetaMaterial = 0;
            Etiqueta = "";
            JuntasEncontradas = "";
            LongitudMaterial = 0;
            ItemCodeID = 0;
            TipoMaterialID = 0;
            Nombre = "";
        }
    }
    public class Taller
    {
        public int TallerID { get; set; }

        public string Nombre { get; set; }
    }
    public class Resultado
    {
        public string _ResultadoID { get; set; }
        public string _Resultado { get; set; }
    }

    public class DetalleCapturaInspeccion
    {
        public string _ResultadoID { get; set; }
        public string _Resultado { get; set; }
    }


    public class DetalleJuntaDimension
    {
        //public List<JuntaXSpoolID> ListaJuntaXSpoolID { get; set; }

        public List<DetalleDimensional> ListaDetalleDimensional { get; set; }
    }

    public class JuntaXSpoolID
    {
        public string Etiqueta { get; set; }
        public int JuntaSpoolID { get; set; }
    }

    public class JuntaXSpoolIDModeloJunta
    {
        public string Junta { get; set; }
        public int JuntaID { get; set; }
    }

    public class DetalleDimensional
    {
        public int InspeccionDimensionalID { get; set; }

        public int OrdenTrabajoSpoolID { get; set; }

        public string FechaInspeccion { get; set; }

        public int ResultadoID { get; set; }

        public string Resultado { get; set; }

        public int ObreroID { get; set; }

        public string Inspector { get; set; }

        public int DefectoID { get; set; }

        public string Defecto { get; set; }

        public int IdDefectoTipo { get; set; }
        public string Tipo { get; set; }
        public List<InspeccionDimensional.JuntaXSpool> ListaJuntas { get; set; }
        public List<InspeccionDimensional.JuntaXSpool> ListaJuntasSeleccionadas { get; set; }
        public List<InspeccionDimensional.JuntaXSpool> ListaJuntasInicial { get; set; }
    }

    public class Captura
    {
        public List<DetalleGuardarJson> Detalles { get; set; }
    }



    public class DetalleGuardarJson
    {
        public int Accion { get; set; }
        public string Lenguaje { get; set; }
        public int InspeccionDimensionalID { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public string FechaInspeccion { get; set; }
        public int ResultadoID { get; set; }
        public int ObreroID { get; set; }
        public int DefectoID { get; set; }

        public List<DetalleGuardarInspeccionVisual> ListaDetalleGuardarInspeccionVisual { get; set; }

        public List<InspeccionDimensional.JuntaXSpoolGuardar> ListaJuntas { get; set; }
    }

    public class DetalleGuardarInspeccionVisual
    {
        public int Accion { get; set; }
        public string DefectosID { get; set; }
        public string ObreroID { get; set; }
        public string FechaInspeccion { get; set; }
        public string ResultadoID { get; set; }
        public string TallerID { get; set; }
        public int JuntaSpoolID { get; set; }
        public List<BackEndSAM.Models.Armado.NumerosUnicos> ListaNumeroUnicoAsignado { get; set; }

    }
    public class InspeccionCamposPredeterminados
    {
        public string FechaDimensional { get; set; }
        public string FechaVisual { get; set; }
        public string ResultadoDimensional { get; set; }
        public string ResultadoVisual { get; set; }
        public string FormatoFecha { get; set; }

    }
}