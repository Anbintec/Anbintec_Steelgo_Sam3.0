using BackEndSAM.Models.Embarque.CargaPlana;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Embarque.CargaPlana
{
    public class CargaPlanaBD
    {
        private static readonly object _mutex = new object();
        private static CargaPlanaBD _instance;

        public static CargaPlanaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CargaPlanaBD();
                    }
                }
                return _instance;
            }
        }
        public object ObtenerListaPaquetes(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoPaquetes_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoPaquetes(ProyectoID).ToList();
                    List<DetallePaquete> listaPaquete = new List<DetallePaquete>();
                    listaPaquete.Add(new DetallePaquete());

                    foreach (Sam3_Embarque_CG_Get_ListadoPaquetes_Result item in result)
                    {
                        listaPaquete.Add(new DetallePaquete
                        {
                            PaqueteID = item.PaqueteID,
                            Nombre = item.Nombre,
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                        });
                    }

                    return listaPaquete;
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

        public object ObtenerDetalleCargaPlana(int PlanaID, int Todos)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_Get_DetalleCargaPlana_Result> result = ctx.Sam3_Embarque_Get_DetalleCargaPlana(PlanaID, Todos).ToList();
                    List<DetalleCargaPlana> listaDetalle = new List<DetalleCargaPlana>();

                    foreach (Sam3_Embarque_Get_DetalleCargaPlana_Result item in result)
                    {
                        listaDetalle.Add(new DetalleCargaPlana
                        {
                            Accion = 2,
                            DetalleCargaID = item.DetalleCargaID,
                            SpoolID = item.SpoolID,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            Spool = item.NumeroControl,
                            PaqueteID = item.PaqueteID.GetValueOrDefault(),
                            Paquete = item.NombrePaquete,
                            Peso = item.Peso.GetValueOrDefault(),
                            CuadranteID = item.CuadranteID,
                            ZonaAnteriorID = item.ZonaAnterior.GetValueOrDefault(),
                            CuadranteAnteriorID = item.CuadranteAnteriorID.GetValueOrDefault(),
                            CuadrantePaqueteAnteriorID = item.CuadrantePaqueteAnteriorID.GetValueOrDefault(),
                            ZonaPaqueteAnteriorID = item.ZonaPaqueteAnteriorID.GetValueOrDefault(),
                            ModificadoPorUsuario = false,
                            RowOk = true,
                            EmpaquetadoInicial = item.PaqueteID == 0 ? 0 : 1,
                            Pdi = item.Pdis.GetValueOrDefault(),
                            Peq = item.Peqs.GetValueOrDefault()
                        });
                    }
                    return listaDetalle;
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

        public object ObtieneDetalleSpoolAgregar(int TipoConsulta, int OrdenTrabajoSpoolID, string NumeroControl)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_Get_DetalleSpool_Result> result = ctx.Sam3_Embarque_Get_DetalleSpool(TipoConsulta, OrdenTrabajoSpoolID, NumeroControl).ToList();
                    List<DetalleSpoolAgregar> listaDetalle = new List<DetalleSpoolAgregar>();

                    foreach (Sam3_Embarque_Get_DetalleSpool_Result item in result)
                    {
                        listaDetalle.Add(new DetalleSpoolAgregar
                        {
                            Accion = 1,
                            DetalleCargaID = item.DetalleCargaID.GetValueOrDefault(),
                            SpoolID = item.SpoolID,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            Spool = item.NumeroControl,
                            Empaquetado = item.Empaquetado,
                            Paquete = item.NombrePaquete,
                            PaqueteID = 0,
                            Peso = item.Peso.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                            CuadranteAnteriorID = item.CuadranteAnteriorID.GetValueOrDefault(),
                            ModificadoPorUsuario = true,
                            Cargado = item.Cargado,
                            PlanaCargado = item.PlanaCargado,
                            CarroCargado = item.CarroCargado,
                            RowOk = true,
                            EmpaquetadoInicial = 0,
                            Pdi = item.Pdis.GetValueOrDefault(),
                            Peq = item.Peqs.GetValueOrDefault()
                        });
                    }
                    return listaDetalle;
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

        public object ObtieneDetallePaqueteAgregar(int PaqueteID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_Get_DetallePaquete_Result> result = ctx.Sam3_Embarque_Get_DetallePaquete(PaqueteID).ToList();
                    List<DetallePaqueteAgregar> listaDetalle = new List<DetallePaqueteAgregar>();

                    foreach (Sam3_Embarque_Get_DetallePaquete_Result item in result)
                    {
                        listaDetalle.Add(new DetallePaqueteAgregar
                        {
                            Accion = 1,
                            SpoolID = item.SpoolID,
                            Spool = item.NumeroControl,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            PaqueteID = item.PaqueteID,
                            Paquete = item.NombrePaquete,
                            Peso = item.Peso.GetValueOrDefault(),
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                            CuadranteAnteriorID = item.CuadranteAnteriorID.GetValueOrDefault(),
                            DetalleCargaID = item.DetalleCargaID,
                            Cargado = item.Cargado,
                            PlanaCargado = item.PlanaCargado,
                            ModificadoPorUsuario = true,
                            RowOk = true,
                            EmpaquetadoInicial = 1,
                            Pdi = item.Pdis.GetValueOrDefault(),
                            Peq = item.Peqs.GetValueOrDefault()
                        });
                    }
                    return listaDetalle;
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

        public object GuardaCapturaCargaPlana(DataTable dtDetalle, int UsuarioID, int PlanaID, int CerrarPlana, int CuadrantePlanaSam2ID, int CuadrantePlanaSam3ID)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametros = { { "@UsuarioID", UsuarioID.ToString() }, { "@PlanaID", PlanaID.ToString() },
                    { "@CerrarPlana", CerrarPlana.ToString() }, { "@CuadrantePlanaSam2ID", CuadrantePlanaSam2ID.ToString() }, { "@CuadrantePlanaSam3ID", CuadrantePlanaSam3ID.ToString() } };

                int identityResult = _SQL.EjecutaInsertUpdate(Stords.GUARDARCAPTURACARGAPLANA, dtDetalle, "@DetalleCarga", parametros);

                TransactionalInformation result = new TransactionalInformation();

                if (identityResult > 0)
                {
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(identityResult.ToString());
                }
                else
                    result.ReturnMessage.Add("Paquete Existe");

                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return result;
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

        public object CerrarCargaPlana(DataTable dtDetalle, int UsuarioID)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametros = { { "@Usuario", UsuarioID.ToString() } };

                _SQL.EjecutaInsertUpdate("", dtDetalle, "@TablaCargaPlana", parametros);

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return result;
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

        public object DescargaSpoolPlana(int DetalleCargaID, int PaqueteID, int SpoolID, int CuadranteID, int CuadranteSamID, int CuadranteAnterior, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjectResult<int?> resultSp = ctx.Sam3_Embarque_DescargaSpool(DetalleCargaID, PaqueteID, SpoolID, CuadranteID, CuadranteSamID, CuadranteAnterior, UsuarioID);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    result.ReturnMessage.Add(valor.ToString());
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

        public object DescargaPaquetePlana(int PaqueteID, int CuadranteID, int UsuarioID, int AbrirPaquete)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ctx.Sam3_Embarque_CG_DescargaPaquete(PaqueteID, CuadranteID, UsuarioID, AbrirPaquete);

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
        public object ValidarCapturaCargaPlana(DataTable dtDetalle, int UsuarioID, int PlanaID)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametros = { { "@UsuarioID", UsuarioID.ToString() }, { "@PlanaID", PlanaID.ToString() } };

                DataTable dataTableName = _SQL.Tabla(Stords.VALIDARCAPTURACARGAPLANA, dtDetalle, "@DetalleCarga", parametros);




                List<DetalleSpoolValidar> listName = dataTableName.AsEnumerable().Select(m => new DetalleSpoolValidar()
                {
                    SpoolID = m.Field<int>("SpoolID"),
                    NumeroControl = m.Field<string>("NumeroControl"),
                    Paquete = m.Field<string>("NombrePaquete"),
                    Plana = m.Field<string>("Plana"),
                    EsCorrecto = m.Field<int>("EsCorrecto"),
                }).ToList();
                

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnMessage.Add("");
                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return listName;
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