using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.ServiciosTecnicos.EntregaPlacasGraficas
{
    public class DetalleGuardarJson
    {
        public int Accion { get; set; }
        public int EntregaPlacasGraficasID { get; set; }
        public int RequisicionID { get; set; }
        public int OrdenTrabajoID { get; set; }
        public int SpoolID { get; set; }
        public int JuntaID { get; set; }
        public int DocumentoRecibidoID { get; set; }
        public int DocumentoEstatusID { get; set; }
        public int DocumentoDefectoID { get; set; }
        public int Estatus { get; set; }
    }

    public class DocumentoRecibido
    {
        public int DocumentoRecibidoID { get; set; }
        public string DocumentoRecibidoNombre { get; set; }

        public DocumentoRecibido()
        {
            DocumentoRecibidoID = 0;
            DocumentoRecibidoNombre = "";
        }
    }

    public class DocumentoEstatus
    {
        public int DocumentoEstatusID { get; set; }
        public string DocumentoEstatusNombre { get; set; }

        public DocumentoEstatus()
        {
            DocumentoEstatusID = 0;
            DocumentoEstatusNombre = "";
        }
    }

    public class DocumentoDefecto {
        public int DocumentoDefectoID { get; set; }
        public string DocumentoDefectoNombre { get; set; }

        public DocumentoDefecto()
        {
            DocumentoDefectoID = 0;
            DocumentoDefectoNombre = "";
        }
    }

    public class PlacasJunta
    {
        public PlacasJunta()
        {
            CapturaResultadoID = 0;
            CapturaResultadoPlacaID = 0;
            Placa = "";
        }

        public int CapturaResultadoID { get; set; }
        public int CapturaResultadoPlacaID { get; set; }
        public string Placa { get; set; }
    }

    public class RequisicionDetalle
    {
        public int Accion { get; set; }
        public int EntregaPlacasGraficasID { get; set; }
        public int RequisicionID { get; set; }
        public int ElementoClasificacionPNDID { get; set; }
        public int CapturaResultadoID { get; set; }
        public string NumeroControl { get; set; }
        public string JuntaEtiqueta { get; set; }
        public int ClasificacionPndID { get; set; }
        public string ClasificacionPnd { get; set; }
        public int TipoPruebaID { get; set; }
        public string TipoPrueba { get; set; }
        public string Observaciones { get; set; }
        public int CodigoAsmeID { get; set; }
        public string CodigoAsme { get; set; }
        public int NumeroBuenEstado { get; set; }
        public bool BuenEstado { get; set; }
        public int DocumentoDefectoID { get; set; }
        public string DocumentoDefecto { get; set; }
        public int EstatusCaptura { get; set; }
        public int Cantplacas { get; set; }
        //public List<DocumentoRecibido> ListaRecibido { get; set; }
        //public List<DocumentoEstatus> ListaEstatusDocumento { get; set; }
        public List<DocumentoDefecto> ListaDefectoDocumento { get; set; }
        public List<PlacasJunta> ListaPlacas { get; set; }

    }

    public class CapturaPlacasGraficas
    {
        public List<DetalleGuardarJson> Detalles { get; set; }
    }

    public class Requisicion
    {
        public Requisicion()
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

    public class Proveedor
    {
        public Proveedor()
        {
            ProveedorID = 0;
            NombreProveedor = "";
        }

        public int ProveedorID { get; set; }
        public string NombreProveedor { get; set; }
    }

    public class Proyecto
    {
        public Proyecto()
        {
            ProyectoID = 0;
            Nombre = "";
            PatioID = 0;
        }
        public int ProyectoID { get; set; }
        public string Nombre { get; set; }
        public int PatioID { get; set; }
    }

    public class TipoPrueba
    {
        public TipoPrueba()
        {
            TipoPruebaID = 0;
            Nombre = "";
            TipoPruebaPorSpool = false;
        }
        public int TipoPruebaID { get; set; }
        public string Nombre { get; set; }
        public bool TipoPruebaPorSpool { get; set; }
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
        public List<Requisicion> listaRequisicion { get; set; }
        public List<TipoPrueba> listaTipoPrueba { get; set; }
    }
}