using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.Dynasol;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class DynasolBD
    {
        private static readonly object _mutex = new object();
        private static DynasolBD _instance;

        public static DynasolBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DynasolBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerOrdenesCompra()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_OrdenesCompras_Result> result = ctx.Sam3_Dynasol_GET_OrdenesCompras().ToList();
                    List<OrdenCompraClass> ListaOrdenes = new List<OrdenCompraClass>();
                    ListaOrdenes.Add(new OrdenCompraClass());
                    foreach (Sam3_Dynasol_GET_OrdenesCompras_Result item in result)
                    {
                        ListaOrdenes.Add(new OrdenCompraClass
                        {
                            OrdenCompraID = item.OrdenCompraID,
                            Nombre = item.Nombre,
                            Cerrada = item.Cerrada.GetValueOrDefault() ? 1: 0,
                            CerradaPago = item.CerradaPago.GetValueOrDefault() ? 1 : 0
                        });
                    }
                    return ListaOrdenes;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerListaInspeccion()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_ListaInspeccion_Result> result = ctx.Sam3_Dynasol_GET_ListaInspeccion().ToList();
                    List<InspeccionClass> listaInspeccion = new List<InspeccionClass>();
                    listaInspeccion.Add(new InspeccionClass());
                    foreach (Sam3_Dynasol_GET_ListaInspeccion_Result item in result)
                    {
                        listaInspeccion.Add(new InspeccionClass
                        {
                            InspeccionID = item.InspeccionID,
                            Nombre = item.Nombre
                        });
                    }
                    return listaInspeccion;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
        public object ObtenerMedidas(string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_Medidas_Result> result = ctx.Sam3_Dynasol_GET_Medidas(Lenguaje).ToList();
                    List<MedidaClass> ListaMedidas = new List<MedidaClass>();
                    ListaMedidas.Add(new MedidaClass());
                    foreach (Sam3_Dynasol_GET_Medidas_Result item in result)
                    {
                        ListaMedidas.Add(new MedidaClass
                        {
                            MedidaID = item.MedidaID,
                            Nombre = item.Nombre
                        });
                    }
                    return ListaMedidas;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object ObtenerRevision(int OrdenCompra, string Lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_Revision_Result> result = ctx.Sam3_Dynasol_GET_Revision(OrdenCompra).ToList();
                    List<InspeccionClass> listaInspeccion = (List<InspeccionClass>)ObtenerListaInspeccion();
                    List<RevisionClass> listaRevision = new List<RevisionClass>();
                    List<MedidaClass> listaMedidas = (List<MedidaClass>)ObtenerMedidas(Lenguaje);
                    foreach (Sam3_Dynasol_GET_Revision_Result item in result)
                    {
                        listaRevision.Add(new RevisionClass
                        {
                            Accion = 2,
                            RevisionID = item.RevisionID,
                            Consecutivo = item.Consecutivo.GetValueOrDefault(),
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            Rev = item.Revision,
                            EsTuberia = item.EsTuberia,
                            Descripcion = item.Descripcion,
                            MaterialNorma = item.MaterialNorma,
                            Diametro1 = (float)item.Diametro1.GetValueOrDefault(),
                            Diametro2 = (float)item.Diametro2.GetValueOrDefault(),
                            Schedule = item.Shedule,
                            Rating = item.Rating,
                            PreparacionExtremos = item.PrepExt,
                            Cant = (float)item.Cantidad.GetValueOrDefault(),
                            PrecioUnidad = item.PrecioUnidad.GetValueOrDefault(),
                            Total = item.Total.GetValueOrDefault(),
                            Partida = item.Partida,
                            ListaDetalleColadas = ObtenerColadas(item.RevisionID, Lenguaje, item.EsTuberia),
                            ListaInspeccion = listaInspeccion,
                            ListaMedidas = listaMedidas,
                            RowOk = true
                        });
                    }
                    return listaRevision;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public List<ColadaClass> ObtenerColadas(int RevisionID, string Lenguaje, int EsTuberia)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ColadaClass> ListaColadas = new List<ColadaClass>();
                    
                    List<Sam3_Dynasol_GET_Coladas_Result> result = ctx.Sam3_Dynasol_GET_Coladas(RevisionID, Lenguaje).ToList();
                    foreach (Sam3_Dynasol_GET_Coladas_Result item in result)
                    {
                        ListaColadas.Add(new ColadaClass
                        {
                            Accion = 2,
                            ColadaID = item.ColadaID,
                            Colada = item.Nombre,
                            RevisionID = item.RevisionID.GetValueOrDefault(),
                            DetalleInspeccionID = item.DetalleInspeccionID.GetValueOrDefault(),
                            InspeccionDetalle = item.NombreInspeccion,
                            Comentario = item.Comentario,
                            Cant = (float)item.CantidadC.GetValueOrDefault(),
                            CantG = (float)item.CantidadG.GetValueOrDefault(),
                            FechaRecibidoG = item.FechaRecibido,
                            Camion = item.Camion.GetValueOrDefault(),
                            FacturaProveedor = item.FacturaProveedor,
                            FechaFactura = item.FechaFactura,
                            Acuerdo = item.Acuerdo,
                            FechaEnvio = item.FechaEnvio,
                            Pedimento = item.Pedimento.GetValueOrDefault(),
                            ShippingDate = item.ShippingDate,
                            CantS = (float)item.CantidadS.GetValueOrDefault(),
                            FechaRecibidoS = item.FechaRecibidoSteelgo,
                            InspeccionSteelgo = item.InspeccionSteelgo.GetValueOrDefault(),
                            ListaDetalleInspeccion = ObtenerDetalleInspeccion(item.ColadaID),
                            MedidaCeciliaID = item.MedidaCeciliaID,
                            MedidaCecilia = (item.MedidaCecilia == "" && EsTuberia == 0 && Lenguaje == "es-MX") ? "Pza" : (item.MedidaCecilia == "" && EsTuberia == 0 && Lenguaje != "es-MX") ? "Pcs" : item.MedidaCecilia,
                            MedidaGerezID = item.MedidaGerezID,
                            MedidaGerez = (item.MedidaGerez == "" && EsTuberia == 0 && Lenguaje == "es-MX") ? "Pza" : (item.MedidaGerez == "" && EsTuberia == 0 && Lenguaje != "es-MX") ? "Pcs" : item.MedidaGerez,                            
                            MedidaSteelgoID = item.MedidaSteelgoID,
                            MedidaSteelgo = (item.MedidaSteelgo == "" && EsTuberia == 0 && Lenguaje == "es-MX") ? "Pza" : (item.MedidaSteelgo == "" && EsTuberia == 0 && Lenguaje != "es-MX") ? "Pcs" : item.MedidaSteelgo,                            
                        });
                    }
                    return ListaColadas;
                }
            }
            catch (Exception ex)
            {
                List<ColadaClass> result = null;
                return result;
            }
        }

        public List<DetalleInspeccionClass> ObtenerDetalleInspeccion(int ColadaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetalleInspeccionClass> ListaDetalles = new List<DetalleInspeccionClass>();
                    
                    List<Sam3_Dynasol_GET_DetalleInspecccion_Result> result = ctx.Sam3_Dynasol_GET_DetalleInspecccion(ColadaID).ToList();

                    foreach (Sam3_Dynasol_GET_DetalleInspecccion_Result item in result)
                    {
                        ListaDetalles.Add(new DetalleInspeccionClass
                        {
                            Accion = 2,
                            DetalleInspeccionID = item.DetalleInspeccionID,
                            ColadaID = item.ColadaID,
                            Inspeccion = item.Nombre,
                            InspeccionID = item.InspeccionID,
                            Comentario = item.Comentario,
                            
                        });
                    }
                    return ListaDetalles;
                }
            }
            catch (Exception)
            {
                List<DetalleInspeccionClass> result = null;
                return result;
            }
        }

        public object GuardarCapturaDynasol(DataTable dtDynasolRevision, DataTable dtDynasolColada, DataTable dtDynasolDetalle, Sam3_Usuario Usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametros = { { "@UsuarioID", Usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };

                    _SQL.Ejecuta(Stords.SAM3_DYNASOL_GUARDARCAPTURA, dtDynasolRevision, "@Dynasol_Revision", dtDynasolColada, "@Dynasol_Colada", dtDynasolDetalle, "@Dynasol_DetalleInspeccion", parametros);
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
    }
}
