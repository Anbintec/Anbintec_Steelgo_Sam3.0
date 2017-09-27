using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static BackEndSAM.Models.Embarque.AsignacionEnvio.AsignacionEnvio;
using SecurityManager.TokenHandler;
using DatabaseManager.Constantes;

namespace BackEndSAM.DataAcces.Embarque.AsignacionEnvio
{
    public class AsignacionEnvioBD
    {
        private static readonly object _mutex = new object();
        private static AsignacionEnvioBD _instance;

        public static AsignacionEnvioBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AsignacionEnvioBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerEmbarques(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_Embarques_Result> result = ctx.Sam3_Embarque_AE_Get_Embarques(ProyectoID).ToList();
                    List<EmbarqueClass> ListaEmbarque = new List<EmbarqueClass>();
                    foreach (Sam3_Embarque_AE_Get_Embarques_Result item in result)
                    {
                        ListaEmbarque.Add(new EmbarqueClass {
                            EmbarqueID = item.EmbarqueID,
                            Embarque = item.Nombre,
                            ListaDatosEmbarque = ObtenerDetalleEmbarques(item.EmbarqueID),
                            ListaProveedorEnvio = ObtenerListaProveedorEnvio(ProyectoID, 2)
                        });
                    }
                    return ListaEmbarque;
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

        public List<DatosEmbarque> ObtenerDetalleEmbarques(int EmbarqueID)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto_Result> result = ctx.Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto(EmbarqueID).ToList();
                    List<DatosEmbarque> ListaDatosEmbarque = new List<DatosEmbarque>();                    
                    foreach(Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto_Result item in result)
                    {
                        ListaDatosEmbarque.Add(new DatosEmbarque {
                            ProveedorID = item.ProveedorID,
                            Proveedor = item.Proveedor,
                            TractoID = item.TractoID,
                            Tracto = item.Tracto,
                            ChoferID = item.ChoferID,
                            Chofer = item.Chofer,
                            ProveedorEnvioID = item.ProveedorEnvioID,
                            ProveedorEnvio = item.ProveedorEnvio,
                            TractoEnvioID = item.TractoEnvioID,
                            TractoEnvio = item.TractoEnvio,
                            ChoferEnvioID = item.ChoferEnvioID,
                            ChoferEnvio = item.ChoferEnvio,
                            ListaTractoEnvio = ObtenerListaTractoEnvio(item.ProveedorEnvioID),
                            ListaChoferEnvio = ObtenerListaChoferEnvio(item.ProveedorEnvioID)
                        });
                    }
                    return ListaDatosEmbarque;
                }
            }
            catch (Exception ex)
            {
                List<DatosEmbarque> result = null;
                return result;
            }
        }

        public object ObtenerDatosEmbarques(int EmbarqueID)
        {
            try
            {
               using (SamContext ctx = new SamContext())
                {
                    List<DatosEmbarque> ListaDatos = new List<DatosEmbarque>();
                    List<Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto_Result> result = ctx.Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto(EmbarqueID).ToList();                    
                    foreach(Sam3_Embarque_AE_Get_Chofer_Proveedor_Tracto_Result item in result)
                    {
                        ListaDatos.Add(new DatosEmbarque {
                            ProveedorID = item.ProveedorID,
                            Proveedor = item.Proveedor,
                            TractoID = item.TractoID,
                            Tracto = item.Tracto,
                            ChoferID = item.ChoferID,
                            Chofer = item.Chofer,
                            ProveedorEnvioID = item.ProveedorEnvioID,
                            ProveedorEnvio = item.ProveedorEnvio,
                            TractoEnvioID = item.TractoEnvioID,
                            TractoEnvio = item.TractoEnvio,
                            ChoferEnvioID = item.ChoferEnvioID,
                            ChoferEnvio = item.ChoferEnvio
                        });
                    }
                    return ListaDatos;
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

        public List<ProveedorEnvioClass> ObtenerListaProveedorEnvio(int proyectoID, int tipoProveedor)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_ProveedoresEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_ProveedoresEnvio(proyectoID, tipoProveedor).ToList();
                    List<ProveedorEnvioClass> ListaProveedorEnvio = new List<ProveedorEnvioClass>();
                    foreach(Sam3_Embarque_AE_Get_ProveedoresEnvio_Result item in result)
                    {
                        ListaProveedorEnvio.Add(new ProveedorEnvioClass {
                            ProveedorEnvioID = item.ProveedorEnvioID,
                            ProveedorEnvio = item.ProveedorEnvio
                        });
                    }
                    return ListaProveedorEnvio;
                }                
            }
            catch (Exception)
            {                
                return null;
            }
        }

        public List<TractoEnvioClass> ObtenerListaTractoEnvio(int proveedorID)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_TractosEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_TractosEnvio(proveedorID).ToList();
                    List<TractoEnvioClass> ListaTractoEnvio = new List<TractoEnvioClass>();
                    foreach(Sam3_Embarque_AE_Get_TractosEnvio_Result item in result)
                    {
                        ListaTractoEnvio.Add(new TractoEnvioClass {
                            TractoEnvioID = item.TractoEnvioID,
                            TractoEnvio = item.TractoEnvio
                        });
                    }
                    return ListaTractoEnvio;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ChoferEnvioClass> ObtenerListaChoferEnvio(int proveedorID)
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_ChoferesEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_ChoferesEnvio(proveedorID).ToList();
                    List<ChoferEnvioClass> ListaChoferEnvio = new List<ChoferEnvioClass>();
                    foreach(Sam3_Embarque_AE_Get_ChoferesEnvio_Result item in result)
                    {
                        ListaChoferEnvio.Add(new ChoferEnvioClass {
                            ChoferEnvioID = item.ChoferEnvioID,
                            ChoferEnvio = item.ChoferEnvio
                        });
                    }
                    return ListaChoferEnvio;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public object ObtenerProveedorEnvio(int ProyectoID, int TipoProveedor)        
        {
            try
            {
                using(SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_ProveedoresEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_ProveedoresEnvio(ProyectoID, TipoProveedor).ToList();
                    List<ProveedorEnvioClass> ListaProveedores = new List<ProveedorEnvioClass>();
                    //ListaProveedores.Add(new ProveedorEnvioClass());
                    foreach(Sam3_Embarque_AE_Get_ProveedoresEnvio_Result item in result)
                    {
                        ListaProveedores.Add(new ProveedorEnvioClass {
                            ProveedorEnvioID = item.ProveedorEnvioID,
                            ProveedorEnvio = item.ProveedorEnvio
                        });
                    }
                    return ListaProveedores;
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

        public object ObtenerTractoEnvio(int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_TractosEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_TractosEnvio(ProveedorID).ToList();
                    List<TractoEnvioClass> listaTractoEnvio = new List<TractoEnvioClass>();
                    //listaTractoEnvio.Add(new TractoEnvioClass());

                    foreach (Sam3_Embarque_AE_Get_TractosEnvio_Result item in result)
                    {
                        listaTractoEnvio.Add(new TractoEnvioClass
                        {
                            TractoEnvioID = item.TractoEnvioID,
                            TractoEnvio = item.TractoEnvio
                        });
                    }

                    return listaTractoEnvio;
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

        public object ObtenerChoferEnvio(int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_AE_Get_ChoferesEnvio_Result> result = ctx.Sam3_Embarque_AE_Get_ChoferesEnvio(ProveedorID).ToList();
                    List<ChoferEnvioClass> listaDetalle = new List<ChoferEnvioClass>();
                    //listaDetalle.Add(new ChoferEnvioClass());

                    foreach (Sam3_Embarque_AE_Get_ChoferesEnvio_Result item in result)
                    {
                        listaDetalle.Add(new ChoferEnvioClass
                        {
                            ChoferEnvioID = item.ChoferEnvioID,
                            ChoferEnvio = item.ChoferEnvio
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


        public object GuardarCaptura(int EmbarqueID, int ProveedorEnvioID, int TractoEnvioID, int ChoferEnvioID, Sam3_Usuario Usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {                        
                        { "@EmbarqueID", EmbarqueID.ToString() },
                        { "@ProveedorEnvioID", ProveedorEnvioID.ToString() },
                        { "@TractoEnvioID", TractoEnvioID.ToString() },
                        { "@ChoferEnvioID", ChoferEnvioID.ToString() },                                                
                        { "@UsuarioID", Usuario.UsuarioID.ToString() }                        
                    };
                    bool valor = _SQL.Ejecuta(Stords.GUARDARCAPTURA_ASIGNACIONENVIO, parametro);
                    TransactionalInformation result = new TransactionalInformation();


                    if (valor)
                    {
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;
                    }
                    else
                    {
                        result.ReturnMessage.Add("error");
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