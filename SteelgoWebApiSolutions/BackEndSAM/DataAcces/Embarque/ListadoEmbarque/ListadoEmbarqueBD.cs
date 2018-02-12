using BackEndSAM.DataAcces.Sam3General.OpcionValidacion;
using BackEndSAM.Models.Embarque.ListadoEmbarque;
using BackEndSAM.Models.Sam3General.OpcionValidacion;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Embarque.ListadoEmbarque
{
    public class ListadoEmbarqueBD
    {
        private static readonly object _mutex = new object();
        private static ListadoEmbarqueBD _instance;

        public static ListadoEmbarqueBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ListadoEmbarqueBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListaDestinos(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_LE_Get_ListadoDestinos_Result> result = ctx.Sam3_Embarque_LE_Get_ListadoDestinos(ProyectoID).ToList();
                    List<ListadoDestino> listaDetalle = new List<ListadoDestino>();
                    listaDetalle.Add(new ListadoDestino());

                    foreach (Sam3_Embarque_LE_Get_ListadoDestinos_Result item in result)
                    {
                        listaDetalle.Add(new ListadoDestino {
                            DestinoID = item.DestinoID,
                            Destino = item.Nombre
                        });
                    }

                    return listaDetalle;
                }

            }catch(Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerDetalleListado(string Lenguaje, int EstatusEmbarque, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetalleOpcionValidacion> listado = (List<DetalleOpcionValidacion>)OpcionValidacionBD.Instance.ObtenerListaOpcionValidacion(Lenguaje);

                    List<Sam3_Embarque_LE_Get_ListadoEmbarque_Result> result = ctx.Sam3_Embarque_LE_Get_ListadoEmbarque(Lenguaje , EstatusEmbarque, UsuarioID).ToList();
                    List<DetalleListadoEmbarque> listaDetalle = new List<DetalleListadoEmbarque>();
                    var cont = 0;            
                    foreach (Sam3_Embarque_LE_Get_ListadoEmbarque_Result item in result)
                    {                        
                        listaDetalle.Add(new DetalleListadoEmbarque
                        {                        
                            listaDestino = (List<ListadoDestino>)ListadoEmbarqueBD.Instance.ObtenerListaDestinos(item.ProyectoID.GetValueOrDefault()),
                            EmbarqueID = item.EmbarqueID,
                            Embarque = item.Embarque,
                            EstatusEmbarqueID = item.EmbarqueEstatusID,
                            ProyectoID = item.ProyectoID.GetValueOrDefault(),
                            Proyecto = item.Proyecto,
                            Planas = item.Planas,
                            DestinoAntID = item.DestinoID,
                            DestinoID = item.DestinoID,
                            Destino = item.Destino,
                            SolicitudPermisoAnt = item.SolicitudPermiso,
                            FolioSolicitudPermiso = item.RequiereAduana ? item.SolicitudPermiso : "NA",
                            FechaSolicitudAnt = item.FechaPermiso,
                            FechaSolicitudPermiso = item.RequiereAduana ? item.FechaPermiso : "NA",
                            RequierePapCliente = item.RequierePapCliente,
                            RequierePermisoAduana = item.RequiereAduana,
                            RequiereRevisionCliente = item.RequiereRevisionCliente,
                            OkClienteAnt = item.OkCliente,
                            OkCliente = item.OkCliente,
                            OkClienteEmbarque = item.OkClienteEmbarque,
                            OkClienteCarga = item.OKClienteCarga,
                            RequiereOkDocumental = item.RequiereOkDocumental,
                            OkDocumental = item.OkDocumental,
                            AprobadoAduanaAnt = item.AprobadoAduana,
                            AprobadoAduana = item.RequiereAduana ? item.AprobadoAduana : 1,
                            AprobadoAduanaDesc = item.RequiereAduana ? item.AprobadoAduanaDesc : "NA",
                            OkEmbarqueAnt = item.OkEmbarque,
                            OkEmbarque = item.OkEmbarque,
                            Enviar =
                            !item.RequiereAduana && !item.RequierePapCliente && !item.RequiereRevisionCliente && !item.OkClienteEmbarque && !item.OkCliente && !item.OKClienteCarga && !item.RequiereOkDocumental && (item.Destino != "" && item.OkEmbarque) ? true : 
                            !item.RequiereAduana && !item.RequierePapCliente && item.RequiereRevisionCliente && !item.OkClienteEmbarque && !item.OkCliente && !item.OKClienteCarga && !item.RequiereOkDocumental && (item.Destino != "" && item.OkEmbarque) ? true :
                            !item.RequiereAduana && !item.RequierePapCliente && item.RequiereRevisionCliente && !item.OkClienteEmbarque && !item.OkCliente && !item.OKClienteCarga && (item.RequiereOkDocumental && item.OkDocumental) && (item.Destino != "" && item.OkEmbarque) ? true :
                            item.RequiereAduana ? (((item.FechaPermiso != null && item.FechaPermiso != "") && (item.SolicitudPermiso != null && item.SolicitudPermiso != "" ) && item.AprobadoAduana == 1) && item.Destino != "" && item.OkEmbarque ? true : false ) :
                            !item.RequiereAduana ? item.Destino != "" && item.OkCliente && item.OkEmbarque && (item.RequiereOkDocumental && item.OkDocumental) ? true : false : 
                            !item.RequiereAduana && item.Destino != "" && !item.OkCliente && item.OkEmbarque && (item.OkDocumental && item.RequiereOkDocumental) ? true : false,
                            
                           
                            CapturaEnvioID = item.CapturaEnvioID.GetValueOrDefault(),
                            ModificadoPorUsuario = false,
                            RowOk = true,
                            Cont = cont,
                            listaEstatus = listado
                        });
                        cont++;
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

        public object ObtenerDetalleListadoEmbarqueEnviado(int UsuarioID, int ProyectoID, string Lenguaje, string FechaInicio, string FechaFin)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_LE_Get_ListadoEmbarqueEnviado_Result> result = ctx.Sam3_Embarque_LE_Get_ListadoEmbarqueEnviado(Lenguaje, UsuarioID, ProyectoID, FechaInicio, FechaFin).ToList();
                    List<DetalleListadoEmbarqueEnviado> listaDetalle = new List<DetalleListadoEmbarqueEnviado>();

                    foreach (Sam3_Embarque_LE_Get_ListadoEmbarqueEnviado_Result item in result)
                    {
                        listaDetalle.Add(new DetalleListadoEmbarqueEnviado
                        {
                            EmbarqueID = item.EmbarqueID,
                            Embarque = item.Embarque,
                            EmbarqueEstatusID = item.EmbarqueEstatusID,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,
                            Planas = item.Planas,
                            DestinoID = item.DestinoID,
                            Destino = item.Destino,
                            FolioSolicitudPermiso = item.RequiereAduana.GetValueOrDefault() ? item.SolicitudPermiso : "NA",
                            FechaSolicitudPermiso = item.RequiereAduana.GetValueOrDefault() ? item.FechaPermiso.ToString() : "NA",
                            RequierePapCliente = item.RequierePapCliente,
                            RequierePermisoAduana = item.RequiereAduana,
                            RequiereRevisionCliente = item.RequiereRevisionCliente,
                            OkCliente = item.OkCliente,
                            AprobadoAduana = item.RequiereAduana.GetValueOrDefault() ? item.AprobadoAduana.GetValueOrDefault() : 1,
                            AprobadoAduanaDesc = item.RequiereAduana.GetValueOrDefault() ? item.AprobadoAduanaDesc : "NA",
                            OkEmbarque = item.OkEmbarque,
                            CapturaEnvioID = item.CapturaEnvioID
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

        public object ObtenerDetalleListadoEmbarquesCerrados(int UsuarioID, int ProyectoID, string Lenguaje, string FechaInicio, string FechaFin)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_LE_Get_ListadoEmbarquesCerrados_Result> result = ctx.Sam3_Embarque_LE_Get_ListadoEmbarquesCerrados(Lenguaje, UsuarioID, ProyectoID, FechaInicio, FechaFin).ToList();
                    List<DetalleListadoEmbarqueCerrado> listaDetalle = new List<DetalleListadoEmbarqueCerrado>();

                    foreach (Sam3_Embarque_LE_Get_ListadoEmbarquesCerrados_Result item in result)
                    {
                        listaDetalle.Add(new DetalleListadoEmbarqueCerrado
                        {
                            EmbarqueID = item.EmbarqueID,
                            Embarque = item.Embarque,
                            EmbarqueEstatusID = item.EmbarqueEstatusID,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,                            
                            DestinoID = item.DestinoID,
                            Destino = item.Destino,
                            FolioSolicitudPermiso = item.RequiereAduana.GetValueOrDefault() ? item.SolicitudPermiso : "NA",
                            Planas = item.Planas,                            
                            FechaSolicitudPermiso = item.RequiereAduana.GetValueOrDefault() ? item.FechaPermiso.ToString() : "NA",
                            RequierePapCliente = item.RequierePapCliente,
                            RequierePermisoAduana = item.RequiereAduana,
                            RequiereRevisionCliente = item.RequiereRevisionCliente,
                            OkEmbarque = item.OkEmbarque,
                            AprobadoAduana = item.RequiereAduana.GetValueOrDefault() ? item.AprobadoAduana.GetValueOrDefault() : 1,
                            AprobadoAduanaDesc = item.RequiereAduana.GetValueOrDefault() ? item.AprobadoAduanaDesc : "NA",
                            OkCliente = item.OkCliente,                                                        
                            CapturaEnvioID = item.CapturaEnvioID
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

        public object ObtenerElementosPorEstatus(string lenguaje, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_LE_Get_ElementosPorEstatus_Result> result = ctx.Sam3_Embarque_LE_Get_ElementosPorEstatus(lenguaje, UsuarioID).ToList();
                    List<ElementosPorStatus> listaDetalle = new List<ElementosPorStatus>();

                    foreach (Sam3_Embarque_LE_Get_ElementosPorEstatus_Result item in result)
                    {
                        listaDetalle.Add(new ElementosPorStatus
                        {
                            Pendientes = item.Pendientes.GetValueOrDefault(),
                            Transito = item.Transito.GetValueOrDefault()
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

        public object GuardarCaptura(int UsuarioID, string Lenguaje, DataTable dtDetalle)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametros = { { "@UsuarioID", UsuarioID.ToString() }, { "@Lenguaje", Lenguaje } };

               _SQL.Ejecuta(Stords.GUARDARCAPTURALISTADOEMBARQUE, dtDetalle, "@TTEmbarqueListado", parametros);

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
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

        public object GuardarEnvioEmbarque(DetalleJsonEnvio dtEnvio, int UsuarioID, string Lenguaje, string NumeroEmbarque, string NumeroEmbarqueCliente, string FechaEnvio)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var oMyString = new ObjectParameter("Retorno", typeof(string));
                    var resultSp = ctx.Sam3_Embarque_LE_GuardarEnvio(UsuarioID, Lenguaje, dtEnvio.EmbarqueID, dtEnvio.DestinoID, dtEnvio.SolicitudPermiso, dtEnvio.FechaPermiso, dtEnvio.AprobadoAduana, dtEnvio.BitacoraAduana, NumeroEmbarque, NumeroEmbarqueCliente, FechaEnvio, dtEnvio.ProyectoID, oMyString);
                    var data = oMyString.Value.ToString();

                    TransactionalInformation result = new TransactionalInformation();

                    if (data!= "AMBOS EXISTEN" && data!= "NE EXISTE" && data!= "NEC EXISTE")
                    {
                        result.ReturnMessage.Add("Ok");
                    }
                    else
                    {
                        result.ReturnMessage.Add(data);
                    }

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