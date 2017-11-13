using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEndSAM.Models.Pintura.SistemaPintura;
using BackEndSAM.Models.Pintura.PinturaGeneral;
using BackEndSAM.Models;
using BackEndSAM.Models.Sam3General;
using System.Data;
using DatabaseManager.Constantes;
using BackEndSAM.Utilities.ConvertirDataTable;
using BackEndSAM.DataAcces.Pintura.AdminReductores;
using BackEndSAM.Models.Pintura.AdminReductores;
using BackEndSAM.Models.Pintura.AdminComponentes;
using BackEndSAM.DataAcces.Pintura.AdminComponentes;
using System.Data.Entity.Core.Objects;

namespace BackEndSAM.DataAcces.Pintura.SistemaPintura
{
    public class SistemaPinturaBD
    {
        private static readonly object _mutex = new Object();
        private static SistemaPinturaBD _instance;

        public static SistemaPinturaBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SistemaPinturaBD();
                    }
                }
                return _instance;
            }
        }


        public object ObtenerSistemaPinturaNuevo(string lenguaje, string sistemaPintura, int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<SistemaPinturaNuevo> listaSistemaPinturaNuevo = new List<SistemaPinturaNuevo>();
                    List<Sam3_SP_Get_DetalleSistemaPintura_Result> result = ctx.Sam3_SP_Get_DetalleSistemaPintura(lenguaje, sistemaPintura, proyectoID).ToList();
                    List<UnidadMedida> listadoUnidadesMedida = (List<UnidadMedida>)ObtenerUnidadMedidaPruebasProceso(lenguaje);
                    List<Reductores> listadoReductores=(List<Reductores>)  AdminReductoresBD.Instance.ObtenerCatalogoReductoresAdministrados(lenguaje);
                    List<Componentes> listadoComponentes = (List<Componentes>)AdminComponentesBD.Instance.ObtenerCatalogoComponentesAdministrados(lenguaje);
                  
                   

                    foreach (Sam3_SP_Get_DetalleSistemaPintura_Result item in result)
                    {
                        List<ComponenteAgregado> listaDetalleComponentesAgregados = (List<ComponenteAgregado>)AdminComponentesBD.Instance.ObtenerCatalogoComponentesAgregados(item.SistemaPinturaProyectoProcesoID,lenguaje,listadoComponentes);
                        listaSistemaPinturaNuevo.Add(new SistemaPinturaNuevo
                        {
                            Accion = item.Accion,
                            Agregar = item.SistemaPinturaProyectoProcesoID == 0 ? false : true,
                            Proceso = item.NombreProceso,
                            ProcesoPinturaID = item.ProcesoPinturaID,
                            MetrosLote = item.MetrosLote,
                            NumeroPruebas = item.NumeroPruebas,
                            ProyectoID = item.ProyectoID,
                            SistemaPinturaID = item.SistemaPinturaID,
                            SistemaPinturaProyectoID = item.SistemaPinturaProyectoID,
                            SistemaPinturaProyectoProcesoID = item.SistemaPinturaProyectoProcesoID,
                            listadoUnidadesMedida = listadoUnidadesMedida,
                            NumeroComponentes = item.NumeroComponentes,
                            Reductor = item.Reductor,
                            ListadoComponentes = listadoComponentes,
                            ListaDetalleComponentesAgregados = listaDetalleComponentesAgregados,
                            ReductorID = item.ReductorID,
                            ListadoReductores = listadoReductores,
                            TemplateDetalleComponentes = lenguaje == "es-MX" ? "Detalle componentes" : "components details",
                            listadoPruebasProceso = (List<PruebasProcesos>)ObtenerPruebasProceso(lenguaje, item.ProcesoPinturaID),
                            listadoPruebasDetalle = item.SistemaPinturaProyectoProcesoID == 0 ? new List<DetallePruebas>() : (List<DetallePruebas>)ObtenerDetallePruebasProceso(lenguaje, item.SistemaPinturaProyectoProcesoID),
                            
                           
                        });
                    }

                    return listaSistemaPinturaNuevo.OrderBy(x=> x.ProcesoPinturaID).ToList<SistemaPinturaNuevo>();
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

        public object ObtenerUnidadMedidaPruebasProceso(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<UnidadMedida> listaunidadMedida = new List<UnidadMedida>();
                    List<Sam3_Steelgo_Get_UnidadProcesoPintura_Result> result = ctx.Sam3_Steelgo_Get_UnidadProcesoPintura(lenguaje).ToList();


                    foreach (Sam3_Steelgo_Get_UnidadProcesoPintura_Result item in result)
                    {
                        listaunidadMedida.Add(new UnidadMedida
                        {
                            Nombre = item.Nombre,
                            UnidadMedidaID = item.UnidadMedidaID
                        });
                    }

                    return listaunidadMedida;
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


        public object ObtenerPruebasProceso(string lenguaje, int ProcesoPinturaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<PruebasProcesos> listaPruebasProceso = new List<PruebasProcesos>();
                    List<Sam3_Pintura_Get_PruebasProcesos_Result> result = ctx.Sam3_Pintura_Get_PruebasProcesos(lenguaje, ProcesoPinturaID).ToList();

                    if (result.Count > 0)
                        listaPruebasProceso.Add(new PruebasProcesos());

                    foreach (Sam3_Pintura_Get_PruebasProcesos_Result item in result)
                    {
                        listaPruebasProceso.Add(new PruebasProcesos
                        {
                            Nombre = item.Nombre,
                            ProcesoPinturaID = item.ProcesoPinturaID,
                            PruebaProcesoPinturaID = item.PruebaProcesoPinturaID,
                            UnidadMedida = item.UnidadMedida,
                            UnidadMedidaID = item.UnidadMedidaID
                        });
                    }

                    return listaPruebasProceso;
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

        public object ObtenerDetallePruebasProceso(string lenguaje, int ProcesoPinturaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DetallePruebas> listaDetallePruebasProceso = new List<DetallePruebas>();
                    List<Sam3_SP_Get_DetallePruebasProceso_Result> result = ctx.Sam3_SP_Get_DetallePruebasProceso(lenguaje, ProcesoPinturaID).ToList();


                    foreach (Sam3_SP_Get_DetallePruebasProceso_Result item in result)
                    {
                        listaDetallePruebasProceso.Add(new DetallePruebas
                        {
                            Accion = 2,
                            ProyectoProcesoPruebaID = item.ProyectoProcesoPruebaID,
                            SistemaPinturaProyectoProcesoID = item.SistemaPinturaProyectoProcesoID,
                            UnidadMaxima = item.UnidadMaxima,
                            UnidadMedida = item.UnidadMedida,
                            UnidadMedidaID = item.UnidadMedidaID,
                            UnidadMinima = item.UnidadMinima,
                            ProyectoProcesoPrueba = item.ProyectoProcesoPrueba,
                            PruebaProcesoPinturaID = item.PruebaProcesoPinturaID
                        });
                    }

                    return listaDetallePruebasProceso;
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

        public object ObtenerSistemaPinturaEdicicion(string lenguaje, string sistemaPintura,int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<SistemaPinturaEdicion> DetalleSistemaPintura = new List<SistemaPinturaEdicion>();
                    List<Sam3_SP_GET_SistemaPintura_Result> result = ctx.Sam3_SP_GET_SistemaPintura(sistemaPintura, proyectoID).ToList();


                    foreach (Sam3_SP_GET_SistemaPintura_Result item in result)
                    {
                        DetalleSistemaPintura.Add(new SistemaPinturaEdicion
                        {
                            Nombre = item.Nombre.Split('~')[0],
                            SistemaPinturaID = item.SistemaPinturaID,
                            NoPintable = item.NoPintable,
                            listadoColor = (List<Color>)ObtenerColoresSistemaPintura(lenguaje, sistemaPintura, proyectoID),
                            listadoProyectos = (List<Proyectos>)ObtenerProyectosSistemaPintura(sistemaPintura)

                        });
                    }

                    return DetalleSistemaPintura;
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


        public object ObtenerColoresSistemaPintura(string lenguaje, string sistemaPintura,int proyectoid)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Color> DetalleSistemaPintura = new List<Color>();
                    List<Sam3_SP_GET_ColorSistemaPintura_Result> result = ctx.Sam3_SP_GET_ColorSistemaPintura(sistemaPintura, lenguaje, proyectoid).ToList();


                    foreach (Sam3_SP_GET_ColorSistemaPintura_Result item in result)
                    {
                        DetalleSistemaPintura.Add(new Color
                        {
                            Nombre = item.Color,
                            ColorID = item.ColorID
                        });
                    }

                    return DetalleSistemaPintura;
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

        public object TieneAvance(string sistemaPintura, int proyectoid)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Color> DetalleSistemaPintura = new List<Color>();
                    return  ctx.Sam3_SP_GET_TieneAvance(sistemaPintura, proyectoid).FirstOrDefault();
                   
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
        
        public object ObtenerProyectosSistemaPintura(string sistemaPintura)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Proyectos> DetalleSistemaPintura = new List<Proyectos>();

                    List<Sam3_SP_GET_Proyectos_Result> result = ctx.Sam3_SP_GET_Proyectos(sistemaPintura).ToList();

                    DetalleSistemaPintura.Add(new Proyectos());
                    foreach (Sam3_SP_GET_Proyectos_Result item in result)
                    {
                        DetalleSistemaPintura.Add(new Proyectos
                        {
                            Nombre = item.Nombre,
                            ProyectoID = item.ProyectoID,
                        });
                    }

                    return DetalleSistemaPintura;
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

        public object InsertarCaptura(DataTable dtDetalleCaptura, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAREQUISICIONASIGNACION, dtDetalleCaptura, "@AsignarRequisicion", parametro);
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

        public object CrearNuevoSistemaPintura(DataTable dtDetalleSPNuevo, DataTable dtDetalleSPColor, DataTable dtDetalleSPProyecto, DataTable dtDetalleProyectoProceso,DataTable dataTableComponentesAgregado, DataTable dtPruebasProceso,string lenguaje,int UsuarioID,bool tieneAvance)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@UsuarioID", UsuarioID.ToString() }, { "@Lenguaje", lenguaje }, { "@SPTieneAvance", (tieneAvance ? 1:0).ToString() } };

                    int valorSPID= _SQL.Ejecuta(Stords.GUARDARCAPTURASISTEMAPINTURA, dtDetalleSPNuevo, "@TTSPNuevo", dtDetalleSPProyecto, "@TTSPProyecto", dtDetalleSPColor, "@TTSPColor", dtDetalleProyectoProceso, "@TTSPProyectoProceso", dataTableComponentesAgregado, "@TTSPProyectoProcesoComponentes", dtPruebasProceso, "@TTSPProyectoProcesoPrueba", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(valorSPID.ToString());
                    result.ReturnMessage.Add(dtDetalleSPNuevo.Rows[0][0].ToString());
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

        public object RevisaSistemaCaptura(DataTable dtDetalleCaptura, string Lenguaje)
        { 
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = {  { "@Lenguaje", Lenguaje }  };

                    DataTable list = _SQL.EjecutaDataAdapter(Stords.COMPROBARSISTEMAPINTURANOMBREPROYECTO, dtDetalleCaptura, "@TablaComprobacionNombreProyecto", parametro);

                    List<DataRow> listadoDatatable = list.AsEnumerable().ToList();
                    List<NombreProyectoExisteResult> listadoResult = new List<NombreProyectoExisteResult>();

                    foreach (DataRow item in listadoDatatable)
                    {
                        listadoResult.Add(new NombreProyectoExisteResult
                        {
                            Nombre = item["Nombre"].ToString(),
                            Proyecto = item["Proyecto"].ToString(),
                            Existe = int.Parse( item["Existe"].ToString()),
                            ProyectoID = int.Parse(item["ProyectoID"].ToString())
                        });
                    }

                    return listadoResult;
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


        public static List<NombreProyectoExiste> table_to_List(DataTable table)
        {
            List<NombreProyectoExiste> result = new List<NombreProyectoExiste>();
            foreach (var row in table.Rows.Cast<DataRow>().Select((r, i) => new { Row = r, Index = i }))
            {
                result.Add(new NombreProyectoExiste
                {
                });
            }

            return result;
        }

       
    }
}