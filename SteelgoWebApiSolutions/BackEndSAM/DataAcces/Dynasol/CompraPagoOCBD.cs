using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.CompraPagoOC;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class CompraPagoOCBD
    {
        private static readonly object _mutex = new object();
        private static CompraPagoOCBD _instance;
        public static CompraPagoOCBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CompraPagoOCBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerDetalleCompraPagoOC(int OrdenCompra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<CompraPago> listaRecepcion = new List<CompraPago>();
                    List<Sam3_OrdenCompra_GET_DetalleCompraPagoOC_Result> result = ctx.Sam3_OrdenCompra_GET_DetalleCompraPagoOC(OrdenCompra).ToList();
                    foreach (Sam3_OrdenCompra_GET_DetalleCompraPagoOC_Result item in result)
                    {
                        listaRecepcion.Add(new CompraPago
                        {
                            Accion = 2,
                            RevisionID = item.RevisionID,
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            ColadaID = item.ColadaID,
                            ItemCode = item.ItemCode,
                            Descripcion = item.Descripcion,
                            MaterialNorma = item.MaterialNorma,
                            Diametro1 = (float)item.Diametro1,
                            Diametro2 = (float)item.Diametro2,
                            Schedule = item.Shedule,
                            Rating = item.Rating,
                            PrepExt = item.PrepExt,
                            Partida = item.Partida,
                            Colada = item.Colada,
                            CantS = item.CantS.GetValueOrDefault(),
                            CantC = item.CantC,
                            CantP = item.CantP,
                            RowOk = true,
                            ModificadoPorUsuario = false
                        });
                    }
                    return listaRecepcion;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

        public object GuardaCapturaCompraPagoOC(DataTable Datos, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", UsuarioID.ToString() }
                    };
                    //_SQL.Ejecuta(Stords.GUARDACAPTURA_COMPRAPAGOOC, Datos, "@DATOS", Parametros);
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception e)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(e.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

    }
}