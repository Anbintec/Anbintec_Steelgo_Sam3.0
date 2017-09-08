using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.PagoOC;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class PagoOCBD
    {
        private static readonly object _mutex = new object();
        private static PagoOCBD _instance;
        public static PagoOCBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PagoOCBD();
                    }
                }
                return _instance;
            }
        }


        public object ObtenerDetallePagoOC(int OrdenCompra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Pago> listaRecepcion = new List<Pago>();
                    List<Sam3_OrdenCompra_GET_PagoOC_Result> result = ctx.Sam3_OrdenCompra_GET_PagoOC(OrdenCompra).ToList();
                    foreach (Sam3_OrdenCompra_GET_PagoOC_Result item in result)
                    {
                        listaRecepcion.Add(new Pago
                        {
                            Accion = 2,
                            RevisionID = item.RevisionID,
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            ColadaID = item.ColadaID,
                            Partida = item.Partida,
                            ItemCode = item.ItemCode,
                            Descripcion = item.Descripcion,
                            MaterialNorma = item.MaterialNorma,
                            Diametro1 = (float)item.Diametro1,
                            Diametro2 = (float)item.Diametro2,
                            Colada = item.Colada,
                            CantC = item.CantC.GetValueOrDefault(),
                            CantG = item.CantG.GetValueOrDefault(),
                            CantS = item.CantS.GetValueOrDefault(),
                            Pagado = item.Pagado.GetValueOrDefault(),
                            Deficit = item.Deficit.GetValueOrDefault(),
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

        public object GuardaOC(DataTable Datos, int OrdenCompraID, int cerrada, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectParameter op = new ObjectParameter("Retorna", typeof(string));
                    op.Value = null;
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var res = ctx.Sam3_OrdenCompra_Pagar_GET_OrdenCompraCerrada(OrdenCompraID, oMyString);
                    var data = oMyString.Value.ToString();

                    if (data.Equals("ok"))
                    {
                        cerrada = 1;
                    }


                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros =
                    {
                        { "@UsuarioID", UsuarioID.ToString() },
                        { "@Cerrar", cerrada.ToString() },
                        { "@OrdenCompraID", OrdenCompraID.ToString() }
                    };
                    _SQL.Ejecuta(Stords.GUARDACAPTURA_PAGOOC, Datos, "@DATOS", Parametros);



                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnMessage.Add(cerrada + "");
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