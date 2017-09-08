using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.Models.Dynasol
{
    public class Dynasol
    {
        public class OrdenCompraClass
        {
            public int OrdenCompraID { get; set; }
            public string Nombre { get; set; }
            public int Cerrada { get; set; }
            public int CerradaPago { get; set; }
            

            public OrdenCompraClass()
            {
                OrdenCompraID = 0;
                Nombre = "";
                Cerrada = 0;
                CerradaPago = 0;
            }
        }

        public class InspeccionClass
        {
            public int InspeccionID { get; set; }
            public string Nombre { get; set; }

            public InspeccionClass()
            {
                InspeccionID = 0;
                Nombre = "";
            }
        }

        public class RevisionClass
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int Consecutivo { get; set; }
            public int OrdenCompraID { get; set; }
            public int EsTuberia { get; set; }
            public string Rev { get; set; }
            public string Descripcion { get; set; }
            public string MaterialNorma { get; set; }
            public float Diametro1 { get; set; }
            public float Diametro2 { get; set; }
            public string Schedule { get; set; }
            public string Rating { get; set; }
            public string PreparacionExtremos { get; set; }
            public float Cant { get; set; }
            public decimal PrecioUnidad { get; set; }
            public decimal Total { get; set; }
            public string Partida { get; set; }
            public List<ColadaClass> ListaDetalleColadas { get; set; }
            public List<InspeccionClass> ListaInspeccion { get; set; }
            public List<MedidaClass> ListaMedidas { get; set; }
            public bool RowOk { get; set; }
        }
        public class ColadaClass
        {
            public int Accion { get; set; }
            public int ColadaID { get; set; }
            public string Colada { get; set; }
            public int RevisionID { get; set; }
            public int DetalleInspeccionID { get; set; }
            public string InspeccionDetalle { get; set; }
            public string Comentario { get; set; }
            public float Cant { get; set; }
            public float CantG { get; set; }
            public string FechaRecibidoG { get; set; }
            public int Camion { get; set; }
            public string FacturaProveedor { get; set; }
            public string FechaFactura { get; set; }
            public string Acuerdo { get; set; }
            public string FechaEnvio { get; set; }
            public int Pedimento { get; set; }
            public string ShippingDate { get; set; }
            public float CantS { get; set; }
            public string FechaRecibidoS { get; set; }
            public int InspeccionSteelgo { get; set; }
            public List<DetalleInspeccionClass> ListaDetalleInspeccion { get; set; }
            public int MedidaCeciliaID { get; set; }
            public string MedidaCecilia { get; set; }
            public int MedidaGerezID { get; set; }
            public string MedidaGerez { get; set; }
            public int MedidaSteelgoID { get; set; }
            public string MedidaSteelgo { get; set; }


        }

        public class DetalleInspeccionClass
        {
            public int Accion { get; set; }
            public int DetalleInspeccionID { get; set; }
            public int InspeccionID { get; set; }
            public int ColadaID { get; set; }
            public string Inspeccion { get; set; }
            public string Comentario { get; set; }
        }

        public class Captura
        {
            public List<JsonCapturaRevision> Detalles { get; set; }
        }

        public class JsonCapturaRevision
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public string Partida { get; set; }
            public List<ListaGuardarColadas> ListaColadas { get; set; }
            public List<ListaGuardarDetalleInspeccion> ListaDetalleInspeccion { get; set; }
        }

        public class ListaGuardarColadas
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int ColadaID { get; set; }
            public string Nombre { get; set; }
            public float CantidadC { get; set; }
            public float CantidadG { get; set; }
            public string FechaRecibido { get; set; }
            public int Camion { get; set; }
            public string FacturaProveedor { get; set; }
            public string FechaFactura { get; set; }
            public string Acuerdo { get; set; }
            public string FechaEnvio { get; set; }
            public int Pedimento { get; set; }
            public string ShippingDate { get; set; }
            public float CantidadS { get; set; }
            public string FechaRecibidoSteelgo { get; set; }
            public int InspeccionSteelgo { get; set; }
            public int MedidaCeciliaID { get; set; }
            public int MedidaGerezID { get; set; }
            public int MedidaSteelgoID { get; set; }
        }

        public class ListaGuardarDetalleInspeccion
        {
            public int Accion { get; set; }
            public int RevisionID { get; set; }
            public int DetalleInspeccion { get; set; }
            
            public string NombreColada { get; set; }
            public int InspeccionID { get; set; }
            public string Comentario { get; set; }
        }
        public class MedidaClass
        {
            public int MedidaID { get; set; }
            public string Nombre { get; set; }
            public MedidaClass()
            {
                MedidaID = 0;
                Nombre = "";
            }
        }
    }
}