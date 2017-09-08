using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.PackingList;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class PackingListBD
    {
        private static readonly object _mutex = new object();
        private static PackingListBD _instance;

        public static PackingListBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PackingListBD();
                    }
                }
                return _instance;
            }
        }        
        public object ObtenerDetallePackingList(int OrdenCompra, string PackingList)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_PackingList_Result> result = ctx.Sam3_Dynasol_GET_PackingList(OrdenCompra, PackingList).ToList();
                    List<RevisionClass> listaRevision = new List<RevisionClass>();
                    foreach (Sam3_Dynasol_GET_PackingList_Result item in result)
                    {
                        listaRevision.Add(new RevisionClass
                        {
                            Accion = item.Accion,
                            RevisionID = item.RevisionID,
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            DetallePackingListID = item.DetallePackingListID,
                            PackingListID = item.PackingListID,
                            Rev = item.Revision,
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
                            CantC = (float)item.CantidadC.GetValueOrDefault(),
                            CantG = (float)item.CantidadG.GetValueOrDefault(),
                            CantS = (float)item.CantidadS.GetValueOrDefault(),
                            ColadaID = item.ColadaID,
                            Colada = item.Colada,
                            CantPL = (float)item.CantidadPackingList.GetValueOrDefault(),
                            CantDisponible = (float)item.CantDisponible.GetValueOrDefault(),
                            ListadoPacking = item.ListadoPacking,
                            Agregar = item.Agregar == 1 ? true : false,
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

        public object GuardarDetallePackingList(DataTable Dynasol_DetallePL, int OrdenCompraID, string NombrePL, string Lenguaje, Sam3_Usuario Usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametros = { { "@OrdenCompraID", OrdenCompraID.ToString() }, { "@NombrePL", NombrePL }, { "@Lenguaje", Lenguaje }, { "@Usuario", Usuario.UsuarioID.ToString() } };
                    _SQL.Ejecuta(Stords.SAM3_DYNASIL_GUARDARDETALLEPACKINGLIST, Dynasol_DetallePL, "@Dynasol_DetallePL", parametros);
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