using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using static BackEndSAM.Models.Dynasol.Camion;

namespace BackEndSAM.DataAcces.Dynasol
{
    public class CamionDB
    {
        private static readonly object _mutex = new object();
        private static CamionDB _instance;

        public static CamionDB Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new CamionDB();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerCamiones()
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_Camiones_Result> result = ctx.Sam3_Dynasol_GET_Camiones().ToList();
                    List<CamionClass> ListaCamiones = new List<CamionClass>();
                    ListaCamiones.Add(new CamionClass());                    
                    foreach (Sam3_Dynasol_GET_Camiones_Result item in result)
                    {
                        ListaCamiones.Add(new CamionClass {
                            CamionID = item.CamionID,
                            NombreCamion = item.NombreCamion
                        });                        
                    }
                    return ListaCamiones;
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

        public object ObtenerDetalleCamion(int OrdenCompraID, string NombreCamion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Dynasol_GET_DetalleCamion_Result> result = ctx.Sam3_Dynasol_GET_DetalleCamion(OrdenCompraID, NombreCamion).ToList();
                    List<DetalleCamion> ListaDetalleCamion = new List<DetalleCamion>();
                    foreach (Sam3_Dynasol_GET_DetalleCamion_Result item in result)
                    {
                        ListaDetalleCamion.Add(new DetalleCamion
                        {
                            Accion = item.Accion,
                            RevisionID = item.RevisionID,
                            OrdenCompraID = item.OrdenCompraID.GetValueOrDefault(),
                            DetalleCamionID = item.DetalleCamionID,
                            CamionID = item.CamionID,
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
                            CantCamion = (float)item.CantidadCamion.GetValueOrDefault(),
                            CantDisponible = (float)item.CantDisponible.GetValueOrDefault(),
                            ListadoCamion = item.ListadoCamion,
                            Agregar = item.Agregar == 1 ? true : false,
                            RowOk = true
                        });
                    }
                    return ListaDetalleCamion;
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


        public object GuardaDetalleCamion(DataTable Dynasol_DetalleCamion, int OrdenCompraID, string NombreCamion, string Lenguaje, Sam3_Usuario Usuario)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] Parametros = { { "@OrdenCompraID", OrdenCompraID.ToString() }, { "@NombreCamion", NombreCamion }, { "@Lenguaje", Lenguaje }, { "@Usuario", Usuario.UsuarioID.ToString() } };
                    _SQL.Ejecuta(Stords.SAM3_DYNASOL_GUARDADETALLECAMION, Dynasol_DetalleCamion, "@Dynasol_DetalleCamion", Parametros);
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