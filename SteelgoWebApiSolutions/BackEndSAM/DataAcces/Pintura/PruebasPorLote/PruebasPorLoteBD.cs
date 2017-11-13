using BackEndSAM.Models;
using BackEndSAM.Models.Pintura.PruebasPorLote;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.PruebasPorLote
{
    public class PruebasPorLoteBD
    {
        private static readonly object _mutex = new object();

        private static PruebasPorLoteBD _instance;

        public static PruebasPorLoteBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PruebasPorLoteBD();
                    }
                }
                return _instance;
            }
        }



        public object ObtenerProcesos(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_Procesos_Result> lista = ctx.Sam3_Pintura_PruebasLote_Get_Procesos(proyectoID).ToList();
                    List<ProcesosPintura> listaProceso = new List<ProcesosPintura>();

                    if (lista.Count>0)
                        listaProceso.Add(new ProcesosPintura());

                    foreach (Sam3_Pintura_PruebasLote_Get_Procesos_Result item in lista)
                    {
                        ProcesosPintura objeto = new ProcesosPintura
                        {
                            ProcesoPinturaID = item.ProcesoPinturaID,
                            ProcesoPintura = item.ProcesoPintura
                        };
                        listaProceso.Add(objeto);
                    }


                    return listaProceso;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerSistemaPintura(int proyectoID,int procesoPinturaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_SP_Result> lista = ctx.Sam3_Pintura_PruebasLote_Get_SP( procesoPinturaID, proyectoID).ToList();
                    List<SistemaPinturaLotes> listaSP = new List<SistemaPinturaLotes>();

                    if (lista.Count > 0)
                        listaSP.Add(new SistemaPinturaLotes());

                    foreach (Sam3_Pintura_PruebasLote_Get_SP_Result item in lista)
                    {
                        SistemaPinturaLotes objeto = new SistemaPinturaLotes
                        {
                            SistemaPinturaID = item.SistemaPinturaID,
                            SistemaPintura = item.SistemaPintura,
                            SistemaPinturaProyectoID=item.SistemaPinturaProyectoID
                        };
                        listaSP.Add(objeto);
                    }


                    return listaSP;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerPrueba( int ProcesoPinturaID ,int SistemaPinturaProyectoID, string lenguaje )
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_Pruebas_Result> lista = ctx.Sam3_Pintura_PruebasLote_Get_Pruebas(ProcesoPinturaID, SistemaPinturaProyectoID, lenguaje).ToList();
                    List<Pruebas> listaPrueba = new List<Pruebas>();

                    if (lista.Count > 0)
                        listaPrueba.Add(new Pruebas());

                    foreach (Sam3_Pintura_PruebasLote_Get_Pruebas_Result item in lista)
                    {
                        Pruebas objeto = new Pruebas
                        {
                            PruebaProcesoPinturaID = item.PruebaProcesoPinturaID,
                            Prueba = item.Prueba,
                            UnidadMaxima=item.UnidadMaxima,
                            UnidadMinima = item.UnidadMinima

                        };
                        listaPrueba.Add(objeto);
                    }


                    return listaPrueba;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerFechas(int ProcesoPinturaID, int SistemaPinturaProyectoID, int PruebaProcesoPinturaID,int sistemaPinturaColorID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DateTime?> lista = ctx.Sam3_Pintura_PruebasLote_Get_Fechas(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, lenguaje, sistemaPinturaColorID).ToList();
                    List<FechasPrueba> listaFechas = new List<FechasPrueba>();


                    foreach (DateTime fecha in lista)
                    {
                        FechasPrueba objeto = new FechasPrueba
                        {
                           Fecha= lenguaje=="es-MX"? fecha.ToString("dd/MM/yyyy"): fecha.ToString("MM/dd/yyyy")
                        };
                        listaFechas.Add(objeto);
                    }

                    return listaFechas;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerLotes(int ProcesoPinturaID ,int SistemaPinturaProyectoID ,int PruebaProcesoPinturaID, string FechaLote,string lenguaje,int sistemaPinturaColorID )
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_Lotes_Result> lista =  ctx.Sam3_Pintura_PruebasLote_Get_Lotes(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, FechaLote, lenguaje/*, sistemaPinturaColorID*/).ToList();
                    List<Lotes> listaLotes = new List<Lotes>();

                    if (lista.Count > 0)
                        listaLotes.Add(new Lotes());

                    foreach (Sam3_Pintura_PruebasLote_Get_Lotes_Result item in lista)
                    {
                        Lotes objeto = new Lotes
                        {
                            LoteID = item.LoteID,
                            Nombre = item.Nombre
                        };
                        listaLotes.Add(objeto);
                    }

                    return listaLotes;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }


        public object ObtenerDetalle(int? procesoPinturaID,int? sistemaPinturaProyectoID,int? pruebaProcesoPinturaID,int? sistemaPinturaColorID, int? loteID, string lenguaje,int todosSinCaptura)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_Detalle_Result> lista = ctx.Sam3_Pintura_PruebasLote_Get_Detalle(procesoPinturaID, sistemaPinturaProyectoID,sistemaPinturaColorID, pruebaProcesoPinturaID, loteID, lenguaje, todosSinCaptura).ToList();
                    List<DetalleLote> listaDetalle = new List<DetalleLote>();

                

                    foreach (Sam3_Pintura_PruebasLote_Get_Detalle_Result item in lista)
                    {
                        DetalleLote objeto = new DetalleLote
                        {
                            SpoolID = item.SpoolID,
                            Area = double.Parse(item.Area.ToString()),
                            Cuadrante = item.Cuadrante,
                            NumeroControl = item.NumeroControl,
                            UnidadMinima = item.UnidadMinima,
                            UnidadMaxima = item.UnidadMaxima,
                            SistemaPintura = item.SistemaPintura,
                            Color = item.Color,
                            PruebasRequeridas = item.PruebasRequeridas,
                            PruebasEjecutadas = item.PruebasEjecutadas,
                            PruebaProcesoPinturaID = item.PruebaProcesoPinturaID,
                            CerrarLote =  item.CerrarLote==1?true:false,
                            ProyectoProcesoPruebaID = item.ProyectoProcesoPruebaID,
                            Template = item.Template,
                            Medida = item.Medida,
                            Accion = 2
                        };
                        listaDetalle.Add(objeto);
                    }
                    return listaDetalle;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerPruebasPorSpool(int spoolID,int proyectoProcesoPruebaID,int SistemaPinturaColorID,  string lenguaje,int loteID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasLote_Get_PruebasSpool_Result> lista = ctx.Sam3_Pintura_PruebasLote_Get_PruebasSpool(spoolID, proyectoProcesoPruebaID, SistemaPinturaColorID, lenguaje,loteID).ToList();
                    List<DetallePruebasPorSpool> listaDetallePruebasSpool = new List<DetallePruebasPorSpool>();



                    foreach (Sam3_Pintura_PruebasLote_Get_PruebasSpool_Result item in lista)
                    {
                        DetallePruebasPorSpool objeto = new DetallePruebasPorSpool
                        {
                            Lote=item.LoteID,
                            SpoolID = item.SpoolID,
                            ProyectoProcesoPruebaID = item.ProyectoProcesoPruebaID,
                            FechaPrueba = item.FechaPrueba.ToString(),
                            UnidadMedida = double.Parse(item.UnidadMedida.ToString()),
                            ResultadoEvaluacion = item.ResultadoEvaluacion,
                            Medida=item.Medida,
                            UnidadMaxima=item.UnidadMaxima,
                            UnidadMinima=item.UnidadMinima,
                            Accion =2,//es dos porque si ya existe entonces se actualiza.
                            PruebaLoteID=item.PruebaLoteID
                        };
                        listaDetallePruebasSpool.Add(objeto);
                    }
                    return listaDetallePruebasSpool;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

       
        public object GuardarPruebas(DataTable TablaPruebasGeneradas, Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje } };

                    _SQL.Ejecuta(Stords.GUARDARCAPTURAPRUEBASSPOOL, TablaPruebasGeneradas, "@CapturaPruebas", parametro);

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

        public object ObtenerProcesosPintura(int OrdenTrabajoSpoolID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Pruebas_Get_ProcesosSpoolID_Result> lista = ctx.Sam3_Pintura_Pruebas_Get_ProcesosSpoolID(OrdenTrabajoSpoolID, lenguaje).ToList();
                    List<ProcesosPinturaSpool> listaLotes = new List<ProcesosPinturaSpool>();

                    if (lista.Count > 0)
                        listaLotes.Add(new ProcesosPinturaSpool());

                    foreach (Sam3_Pintura_Pruebas_Get_ProcesosSpoolID_Result item in lista)
                    {
                        ProcesosPinturaSpool objeto = new ProcesosPinturaSpool
                        {
                            SpoolID=item.spoolID,
                            ProcesoPinturaID=item.ProcesoPinturaID,
                            ProcesoPintura=item.ProcesoPintura,
                            Status=item.Status,
                            SistemaPinturaProyectoProcesoID=item.SistemaPinturaProyectoProcesoID,
                            SistemaPinturaProyectoID=item.SistemaPinturaProyectoID
                        };
                        listaLotes.Add(objeto);
                    }

                    return listaLotes;

                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerPrueba(int SistemaPinturaProyectoProcesoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_PruebasSpool_Get_Pruebas_Result> lista = ctx.Sam3_Pintura_PruebasSpool_Get_Pruebas(SistemaPinturaProyectoProcesoID, lenguaje).ToList();
                    List<PruebasSpool> listaLotes = new List<PruebasSpool>();

                    if (lista.Count > 0)
                        listaLotes.Add(new PruebasSpool());

                    foreach (Sam3_Pintura_PruebasSpool_Get_Pruebas_Result item in lista)
                    {
                        PruebasSpool objeto = new PruebasSpool
                        {
                            ProyectoProcesoPruebaID = item.ProyectoProcesoPruebaID,
                            Prueba = item.Prueba,
                            UnidadMaxima = item.UnidadMaxima,
                            UnidadMinima = item.UnidadMinima,
                            UnidadMedida=item.UnidadMedida
                        };
                        listaLotes.Add(objeto);
                    }

                    return listaLotes;

                }

                return null;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerDetalleSpool(int? ordentrabajospoolid,int? sistemapinturacolorid,string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_InformacionSpool_Result> lista = ctx.Sam3_Pintura_InformacionSpool(ordentrabajospoolid, lenguaje, sistemapinturacolorid).ToList();
                    List<BackEndSAM.Models.Pintura.PruebasPorLote.InformacionSpool> listaInformacionSpool= new List<BackEndSAM.Models.Pintura.PruebasPorLote.InformacionSpool>();

                  
                    foreach (Sam3_Pintura_InformacionSpool_Result item in lista)
                    {
                        BackEndSAM.Models.Pintura.PruebasPorLote.InformacionSpool objeto = new BackEndSAM.Models.Pintura.PruebasPorLote.InformacionSpool
                        {
                            Area=item.Area,
                            Color=item.Color,
                            LoteID=item.LoteID,
                            NombreCuadrante=item.NombreCuadrante,
                            NumeroControl=item.NumeroControl,
                            SistemaPintura=item.SistemaPintura
                        };
                        listaInformacionSpool.Add(objeto);
                    }
                    return lista;
                }
                
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ObtenerListadoColores(int SistemaPinturaProyectoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Pruebas_Get_Color_Result> result = ctx.Sam3_Pintura_Pruebas_Get_Color(SistemaPinturaProyectoID, lenguaje).ToList();

                    List<BackEndSAM.Models.Pintura.IntermedioAcabado.Color> listaColores = new List<BackEndSAM.Models.Pintura.IntermedioAcabado.Color>();

                    if (result.Count > 0)
                        listaColores.Add(new BackEndSAM.Models.Pintura.IntermedioAcabado.Color());

                    foreach (Sam3_Pintura_Pruebas_Get_Color_Result item in result)
                    {
                        listaColores.Add(new BackEndSAM.Models.Pintura.IntermedioAcabado.Color
                        {
                            SistemaPinturaColorID = item.SistemaPinturaColorID,
                            ColorID = item.ColorID,
                            Nombre = item.Nombre
                        });
                    }

                    return listaColores;

                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
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