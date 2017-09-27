using BackEndSAM.Models.Embarque.PreparacionEmbarque;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace BackEndSAM.DataAcces.Embarque.PreparacionEmbarque
{
    public class PreparacionEmbarqueBD
    {
        private static readonly object _mutex = new object();
        private static PreparacionEmbarqueBD _instance;

        public static PreparacionEmbarqueBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PreparacionEmbarqueBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerDetalleAgregarPlana(int cargaPlanaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_PE_Get_DetalleAgregar_Result> result = ctx.Sam3_Embarque_PE_Get_DetalleAgregar(cargaPlanaID).ToList();
                    List<DetalleAgregarPlana> listaDetalle = new List<DetalleAgregarPlana>();
                    

                    foreach (Sam3_Embarque_PE_Get_DetalleAgregar_Result item in result)
                    {
                        listaDetalle.Add(new DetalleAgregarPlana
                        {
                            Accion = item.EmbarqueDetalleID == 0 ? 1 : 2,
                            EmbarqueID = item.EmbarqueID,
                            NombreEmbarque = item.NombreEmbarque,
                            EmbarqueDetalleID = item.EmbarqueDetalleID,
                            PlanaID = item.PlanaID,
                            Nombre = item.Nombre,
                            CargaPlanaID = item.CargaPlanaID,
                            StatusCarga = item.StatusCarga.GetValueOrDefault(),
                            CantidadElementos = item.CantidadElementos.GetValueOrDefault(),
                            M2 = item.M2.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            Pdi = item.Pdis.GetValueOrDefault(),
                            Peq = item.Peqs.GetValueOrDefault(),
                            ModificadoPorUsuario = true
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

        public object ObtenerDetalleEmbarque(int EmbarqueID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_PE_Get_DetalleEmbarque_Result> result = ctx.Sam3_Embarque_PE_Get_DetalleEmbarque(EmbarqueID).ToList();
                    List<DetalleAgregarPlana> listaDetalle = new List<DetalleAgregarPlana>();
                    

                    foreach (Sam3_Embarque_PE_Get_DetalleEmbarque_Result item in result)
                    {
                        listaDetalle.Add(new DetalleAgregarPlana
                        {
                            Accion = item.EmbarqueDetalleID == 0 ? 1 : 2,
                            EmbarqueID = item.EmbarqueID,
                            NombreEmbarque = item.NombreEmbarque,
                            EmbarqueDetalleID = item.EmbarqueDetalleID,
                            PlanaID = item.PlanaID,
                            Nombre = item.Nombre,
                            CargaPlanaID = item.CargaPlanaID,
                            StatusCarga = item.StatusCarga.GetValueOrDefault(),
                            CantidadElementos = item.CantidadElementos.GetValueOrDefault(),
                            M2 = item.M2.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            Pdi = item.Pdis.GetValueOrDefault(),
                            Peq = item.Peqs.GetValueOrDefault(),
                            ModificadoPorUsuario = false
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

        public object ObtenerListadoEmbarques(int ProveedorID, string Lenguaje, int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_PE_Get_ListadoEmbarque_Result> result = ctx.Sam3_Embarque_PE_Get_ListadoEmbarque(ProveedorID, Lenguaje, ProyectoID).ToList();
                    List<EmbarqueDetalle> listaDetalle = new List<EmbarqueDetalle>();
                    listaDetalle.Add(new EmbarqueDetalle());

                    foreach (Sam3_Embarque_PE_Get_ListadoEmbarque_Result item in result)
                    {
                        listaDetalle.Add(new EmbarqueDetalle
                        {
                            EmbarqueID = item.EmbarqueID,
                            Nombre = item.Nombre,
                            NombreCliente = item.NombreCliente,
                            ChoferID = item.ChoferID,
                            TractoID = item.TractoID,
                            Estatus = item.Estatus,
                            FechaCreacion = item.FechaCreacion,
                            TractoEnvioID = item.TractoEnvioID,
                            ChoferEnvioID = item.ChoferEnvioID
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


        public object EliminarEmbarque(int EmbarqueID, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                     ctx.Sam3_Embarque_PE_EliminarEmbarque(EmbarqueID, usuario.UsuarioID, lenguaje);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
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



        public object InsertarCaptura(DataTable dtDetalleCaptura, Sam3_Usuario usuario, string lenguaje, int EmbarqueID, string NombreEmbarque, string NombreEmbarqueCliente, int TractoID, int ChoferID, int TractoEnvioID, int ChoferEnvioID, int ProveedorEnvioID, string FechaCreacion)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { 
                        { "@EmbarqueID", EmbarqueID.ToString() }, 
                        { "@NombreEmbarque", NombreEmbarque },
                        { "@NombreCliente", NombreEmbarqueCliente ==  null ?"": NombreEmbarqueCliente }, 
                        { "@TractoID", TractoID.ToString() }, 
                        { "@ChoferID", ChoferID.ToString() },
                        { "@TractoEnvioID", TractoEnvioID.ToString() },
                        { "@ChoferEnvioID", ChoferEnvioID.ToString() },
                        { "@ProveedorEnvioID", ProveedorEnvioID.ToString() },
                        { "@FechaCreacion", FechaCreacion },
                        { "@Usuario", usuario.UsuarioID.ToString() }, 
                        { "@Lenguaje", lenguaje }
                    };
                    int valor = _SQL.EjecutaInsertUpdate(Stords.GUARDARPREPARACIONEMBARQUE, dtDetalleCaptura, "@TTDetalleEmbarque", parametro);
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
                        result.ReturnMessage.Add("existe");
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