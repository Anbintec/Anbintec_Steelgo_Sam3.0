using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager.Sam3;

namespace BackEndSAM.Models
{
    public class FolioAvisoEntradaJson
    {
        public int FolioAvisoEntradaID { get; set; }
        public int FolioAvisollegadaId { get; set; }
        public string Factura { get; set; }
        public string OrdenCompra { get; set; }
        public string IdentificadorCliente { get; set; }
        public int ProveedorID { get; set; }
        public int ClienteId { get; set; }
        public int PatioID { get; set; }
        public string Estatus { get; set; }
        public List<int> Proyectos { get; set; }
        public string ComboEstatus { get; set; }

        public FolioAvisoEntradaJson()
        {
            Proyectos = new List<int>();
        }

    }

    public class DetalleAvisoEntradaJson
    {
        public int FolioAvisoEntradaID { get; set; }
        public int FolioAvisollegadaId { get; set; }
        public string Factura { get; set; }
        public string OrdenCompra { get; set; }
        public string IdentificadorCliente { get; set; }
        public Proveedor Proveedor { get; set; }
        public Cliente Cliente { get; set; }
        public Patio Patio { get; set; }
        public string Estatus { get; set; }
        public string EstatusAv { get; set; }
        public List<int>Proyectos { get; set; }
        public List<ListaDocumentos> Documentos { get; set; }
        public int FolioDescarga { get; set; }
        public Nullable<DateTime> FechaGeneracionDescarga { get; set; }
        public Nullable<DateTime> FechaInicioDescarga { get; set; }
        public Nullable<DateTime> FechaFinDescarga { get; set; }
        public string ComboEstatus { get; set; }
        public string FolioConfiguracion { get; set; }

        public DetalleAvisoEntradaJson()
        {
            Proveedor = new Proveedor();
            Cliente = new Cliente();
            Patio = new Patio();
            Proyectos = new List<int>();
            Documentos = new List<ListaDocumentos>();
        }
    }
}