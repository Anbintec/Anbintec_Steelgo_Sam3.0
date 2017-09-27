using BackEndSAM.Models.Embarque.EmbarqueGeneral;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Embarque.EmbarqueGeneral
{
    public class EmbarqueGeneralBD
    {
        private static readonly object _mutex = new object();
        private static EmbarqueGeneralBD _instance;

        public static EmbarqueGeneralBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if(_instance == null)
                    {
                        _instance = new EmbarqueGeneralBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerPlanas(int ProyectoID, int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoPlanas_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoPlanas(ProyectoID, ProveedorID).ToList();
                    List<DetallePlana> listaDetalle = new List<DetallePlana>();
                    listaDetalle.Add(new DetallePlana());

                    foreach (Sam3_Embarque_CG_Get_ListadoPlanas_Result item in result)
                    {
                        listaDetalle.Add(new DetallePlana {
                            PlanaID = item.PlanaID,
                            Nombre = item.Nombre, 
                            CargaPlanaID = item.CargaPlanaID.GetValueOrDefault(),
                            StatusCarga =       item.StatusCarga.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID.GetValueOrDefault(),
                            CuadrantePlanaSam2 = item.CuadrantePlanaSam2.GetValueOrDefault(),
                            CuadrantePlanaSam3 = item.CuadrantePlanaSam3.GetValueOrDefault()
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

        public object ObtenerPlanasCerradas(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoPlanasCerradas_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoPlanasCerradas(ProyectoID).ToList();
                    List<DetallePlana> listaDetalle = new List<DetallePlana>();
                    listaDetalle.Add(new DetallePlana());

                    foreach (Sam3_Embarque_CG_Get_ListadoPlanasCerradas_Result item in result)
                    {
                        listaDetalle.Add(new DetallePlana
                        {
                            PlanaID = item.PlanaID,
                            Nombre = item.Nombre,
                           CargaPlanaID = item.CargaPlanaID,
                           CuadrantePlanaSam2 = item.CuadrantePlanaSam2.GetValueOrDefault(),
                           CuadrantePlanaSam3 = item.CuadrantePlanaSam3.GetValueOrDefault(),
                           ProyectoID = item.ProyectoID.GetValueOrDefault(),
                           StatusCarga = item.StatusCarga.GetValueOrDefault()
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

        public object ObtenerTracto(int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoTractos_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoTractos(ProveedorID).ToList();
                    List<Tracto> listaDetalle = new List<Tracto>();
                    listaDetalle.Add(new Tracto());

                    foreach (Sam3_Embarque_CG_Get_ListadoTractos_Result item in result)
                    {
                        listaDetalle.Add(new Tracto
                        {
                            TractoID= item.TractoID,
                            Nombre = item.Nombre
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

        public object ObtenerChofer(int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_CG_Get_ListadoChofer_Result> result = ctx.Sam3_Embarque_CG_Get_ListadoChofer(ProveedorID).ToList();
                    List<Chofer> listaDetalle = new List<Chofer>();
                    listaDetalle.Add(new Chofer());

                    foreach (Sam3_Embarque_CG_Get_ListadoChofer_Result item in result)
                    {
                        listaDetalle.Add(new Chofer
                        {
                            ChoferID = item.ChoferID,
                            Nombre = item.Nombre
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

        //public object ObtenerPaquetesCerrados()
        //{
        //    try
        //    {
        //        using (SamContext ctx = new SamContext())
        //        {
        //            List<Sam3_Embarque_Get_ListadoPaquetesCerrados_Result> result = 
        //        }
        //    }catch(Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;

        //        return result;
        //    }
        //}

        public object GuardarNuevaPlana(string NombrePlana, int ProveedorID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> resultSp = ctx.Sam3_Embarque_CG_CreatePlana(NombrePlana, ProveedorID, UsuarioID);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("La plana para ese proveedor ya existe");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }

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

        public object GuardarNuevoTracto(string NombreTracto, int ProveedorID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> resultSp = ctx.Sam3_Embarque_CE_CreateTracto(NombreTracto, ProveedorID, UsuarioID);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("El tracto ya existe para ese proveedor");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }

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

        public object GuardarNuevoChofer(string NombreChofer, int ProveedorID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> resultSp = ctx.Sam3_Embarque_CE_CreateChofer(NombreChofer, ProveedorID, UsuarioID);
                    var valor = resultSp.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    TransactionalInformation result = new TransactionalInformation();

                    if (valor > 0)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("El chofer ya existe para ese proveedor");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }

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