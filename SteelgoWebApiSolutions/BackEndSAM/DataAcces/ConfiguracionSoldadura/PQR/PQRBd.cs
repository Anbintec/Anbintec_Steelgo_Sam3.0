using BackEndSAM.DataAcces.Sam3General.TipoJunta;
using BackEndSAM.Models.ConfiguracionSoldadura.PQR;
using BackEndSAM.Models.ConfiguracionSoldadura.SoldadorCertificacion;
using BackEndSAM.Models.Sam3General.TipoJunta;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace BackEndSAM.DataAcces.ConfiguracionSoldadura
{
    public class PQRBd
    {
        private static readonly object _mutex = new object();
        private static PQRBd _instance;

        public static PQRBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PQRBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoPQRActivos(int TipoAccion, int usuarioID, string especificacion, string codigo,int pantallaEnvia)
        {
            try
            {
                List<PQR> listaPQR = new List<PQR>();

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_PQR_Result> listaPQRJson = ctx.Sam3_Soldadura_PQR(TipoAccion, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null).ToList();
                    List<TipoPrueba> listaTipoPrueba = (from lTPS in ctx.Sam3_Soldadura_Get_TipoPrueba()

                                                        select new TipoPrueba
                                                        {
                                                            TipoPruebaID = lTPS.TipoPruebaID.ToString(),
                                                            TipoDePrueba = lTPS.Nombre
                                                        }).AsParallel().ToList().Where(x => x.TipoDePrueba != "Ambos").OrderBy(x => x.TipoDePrueba).ToList<TipoPrueba>();
                    listaTipoPrueba.Insert(0, new TipoPrueba());

                    List<DetalleTipoJunta> listaTipoJunta = (List<DetalleTipoJunta>)TipoJuntaBD.Instance.ObtieneListadoTipoJunta();


                    if (listaPQRJson.Count > 0 && pantallaEnvia==2)
                        listaPQR.Add(new PQR());
                    foreach (Sam3_Soldadura_PQR_Result item in listaPQRJson)
                    {
                        listaPQR.Add(
                            new PQR
                            {
                                PQRID = item.PQRID,
                                Nombre = item.Nombre,
                                PREHEAT = Convert.ToInt32(item.PREHEAT),
                                PWHT = Convert.ToInt32(item.PWHT),
                                CVN= Convert.ToInt32(item.CVN),
                                FN = Convert.ToInt32(item.FN),
                                MacroTest = Convert.ToInt32(item.MacroTest),
                                EspesorRaiz = Decimal.ToDouble(item.EspesorRaiz.GetValueOrDefault()),
                                EspesorRelleno = Decimal.ToDouble(item.EspesorRelleno),
                                ProcesoSoldaduraRaizID = item.ProcesoSoldaduraRaizID.GetValueOrDefault(),
                                ProcesoSoldaduraRellenoID = item.ProcesoSoldaduraRellenoID,
                                CodigoRaiz = item.CodigoRaiz,
                                CodigoRelleno = item.CodigoRelleno,
                                GrupoPMaterialBase1 = item.GrupoPMaterialBase1.GetValueOrDefault(),
                                GrupoPMaterialBase1Nombre = item.GrupoPMaterialBase1Nombre,
                                GrupoPMaterialBase2 = item.GrupoPMaterialBase2.GetValueOrDefault(),
                                GrupoPMaterialBase2Nombre = item.GrupoPMaterialBase2Nombre,
                                Aporte = item.Aporte,
                                GrupoF = item.GrupoF,
                                AporteRelleno = item.AporteRelleno,
                                GrupoFRelleno = item.GrupoFRelleno,
                                Mezcla = item.Mezcla,
                                Respaldo = item.Respaldo,
                                CodigoASMEID = item.CodigoID.GetValueOrDefault(),
                                Especificacion = item.Especificacion,
                                Accion = 2,
                                RegistrosWPS = item.RegistrosWPS.GetValueOrDefault(),
                                ListaProcesosSoldadura = (List<ListaProcesoSoldadura>)obtenerListadoProcesos(TipoAccion),
                                ListaMaterialesBase = (List<ListaMaterialesBase>)obtenerListadoMaterialesBase(TipoAccion),
                                ListaCodigos = (List<ListaCodigos>)obtenerListadoCodigos(usuarioID, especificacion, codigo),
                                RowOk = true,
                                listaTipoJunta = listaTipoJunta,
                                listaTipoPrueba = listaTipoPrueba,
                                TipoJuntaID = item.TipoJuntaID.GetValueOrDefault(),
                                TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                                TipoJunta = item.TipoJunta,
                                TipoPrueba = item.TipoPrueba
                                
                            });
                    }

                    return listaPQR.OrderBy(x => x.Nombre).ToList<PQR>();
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

        public static object ObtenerListadoPQRActivos()
        {
            try
            {
                List<DetallePQR> listaPQR = new List<DetallePQR>();

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Soldadura_GET_PQR_WPS_Result> listaPQRJson = ctx.Sam3_Soldadura_GET_PQR_WPS("").ToList();
                    listaPQR.Add(new DetallePQR());
                    foreach (Sam3_Soldadura_GET_PQR_WPS_Result item in listaPQRJson)
                    {
                        listaPQR.Add(
                            new DetallePQR
                            {
                                PQRID = item.PQRID,
                                Nombre = item.Nombre,
                                PREHEAT = Convert.ToInt32(item.PREHEAT),
                                PWHT = Convert.ToInt32(item.PWHT),
                                CVN = item.CVN.GetValueOrDefault(),
                                FN = Convert.ToInt32(item.FN),
                                MacroTest = Convert.ToInt32(item.MacroTest),
                                EspesorRaiz = Decimal.ToDouble(item.EspesorRaiz.GetValueOrDefault()),
                                EspesorRelleno = Decimal.ToDouble(item.EspesorRelleno),
                                GrupoPMaterialBase1 = item.GrupoPMaterialBase1.GetValueOrDefault(),
                                GrupoPMaterialBase1Nombre = item.GrupoPMaterialBase1Nombre,
                                GrupoPMaterialBase2 = item.GrupoPMaterialBase2.GetValueOrDefault(),
                                GrupoPMaterialBase2Nombre = item.GrupoPMaterialBase2Nombre,
                                ProcesoSoldaduraRaizID = item.ProcesoSoldaduraRaizID.GetValueOrDefault(),
                                ProcesoSoldaduraRellenoID = item.ProcesoSoldaduraRellenoID,
                                CodigoRaiz = item.CodigoRaiz,
                                CodigoRelleno = item.CodigoRelleno,
                            });
                    }

                    return listaPQR.OrderBy(x => x.Nombre).ToList<DetallePQR>();
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

        internal object ObtenerListasPQR(int usuarioID)
        {
            try
            {
                List<PQR> listaPQR = new List<PQR>();

                using (SamContext ctx = new SamContext())
                {
                    List<TipoPrueba> listaTipoPrueba = (from lTPS in ctx.Sam3_Soldadura_Get_TipoPrueba()

                                                        select new TipoPrueba
                                                        {
                                                            TipoPruebaID = lTPS.TipoPruebaID.ToString(),
                                                            TipoDePrueba = lTPS.Nombre
                                                        }).AsParallel().ToList().Where(x => x.TipoDePrueba != "Ambos").OrderBy(x => x.TipoDePrueba).ToList<TipoPrueba>();
                    listaTipoPrueba.Insert(0, new TipoPrueba());

                    List<DetalleTipoJunta> listaTipoJunta = (List<DetalleTipoJunta>)TipoJuntaBD.Instance.ObtieneListadoTipoJunta();
                    
                    
                        listaPQR.Add(
                            new PQR
                            {
                                ListaProcesosSoldadura = (List<ListaProcesoSoldadura>)obtenerListadoProcesos(1),
                                ListaMaterialesBase = (List<ListaMaterialesBase>)obtenerListadoMaterialesBase(1),
                                ListaCodigos = (List<ListaCodigos>)obtenerListadoCodigos(usuarioID, null, null),
                                listaTipoJunta = listaTipoJunta,
                                listaTipoPrueba = listaTipoPrueba
                            });
                    

                    return listaPQR.OrderBy(x => x.Nombre).ToList<PQR>();
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

        public object obtenerListadoProcesos(int tipoAccion)
        {
            try
            {
                List<ListaProcesoSoldadura> listaProcesosResult = new List<ListaProcesoSoldadura>();
                listaProcesosResult.Add(new ListaProcesoSoldadura());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_PQR_ProcesoSoldadura_Result> listadoProcesosSoldadura = ctx.Sam3_Cat_PQR_ProcesoSoldadura(tipoAccion).ToList();

                    foreach (Sam3_Cat_PQR_ProcesoSoldadura_Result item in listadoProcesosSoldadura)
                    {
                        listaProcesosResult.Add(new ListaProcesoSoldadura
                        {
                            ProcesoSoldaduraID = item.ProcesoSoldaduraID,
                            Codigo = item.Codigo
                        });
                    }

                    return listaProcesosResult.OrderBy(x => x.Codigo).ToList<ListaProcesoSoldadura>();
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

        public object obtenerListadoMaterialesBase(int tipoAccion)
        {
            try
            {
                List<ListaMaterialesBase> listaMaterialesResult = new List<ListaMaterialesBase>();
                listaMaterialesResult.Add(new ListaMaterialesBase());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_PQR_ListaMateriales_Result> listadoMateriales = ctx.Sam3_Cat_PQR_ListaMateriales().ToList();
                    foreach (Sam3_Cat_PQR_ListaMateriales_Result item in listadoMateriales)
                    {
                        listaMaterialesResult.Add(new ListaMaterialesBase
                        {
                            GrupoP = item.GrupoP,
                            GrupoPID = item.GrupoPID
                        });
                    }

                    return listaMaterialesResult.OrderBy(x => x.GrupoP).ToList<ListaMaterialesBase>();
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

        public object obtenerListadoCodigos(int usuarioID, string especificacion, string codigo)
        {
            try
            {
                List<ListaCodigos> listaCodigoResult = new List<ListaCodigos>();
                listaCodigoResult.Add(new ListaCodigos());
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Cat_PQR_ListaCodigos_Result> listaCodigo = ctx.Sam3_Cat_PQR_ListaCodigos(usuarioID, especificacion, codigo).ToList();

                    foreach (Sam3_Cat_PQR_ListaCodigos_Result item in listaCodigo)
                    {
                        listaCodigoResult.Add(new ListaCodigos
                        {
                            CodigoAsmeID = item.CodigoASMEID,
                            Especificacion = item.Especificacion
                        });
                    }

                    return listaCodigoResult.OrderBy(x => x.Especificacion).ToList<ListaCodigos>();
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

        public object AgregarPQR(DataTable dtDetallePQR, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() } };
                    _SQL.Ejecuta(Stords.GUARDAPQR, dtDetallePQR, "@Tabla", parametro);

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
                TransactionalInformation lista = new TransactionalInformation();
                lista.ReturnMessage.Add(ex.Message);
                lista.ReturnCode = 500;
                lista.ReturnStatus = false;
                lista.IsAuthenicated = true;

                return lista;
            }
        }

        public object AgregarNuevoPQR(DataTable dtDetallePQR, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@PQRID", "0" } };
                    int res = _SQL.EjecutaInsertUpdate(Stords.GUARDANUEVOPQR, dtDetallePQR, "@Tabla", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
                    if (res > 0)
                    {
                        result.ReturnMessage.Add(res.ToString());
                    }
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation lista = new TransactionalInformation();
                lista.ReturnMessage.Add(ex.Message);
                lista.ReturnCode = 500;
                lista.ReturnStatus = false;
                lista.IsAuthenicated = true;

                return lista;
            }
        }



        public static object PQRExist(string nombre)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var res = ctx.Sam3_Soldadura_PQR_Existe(nombre, oMyString);
                    var data = oMyString.Value.ToString();
                    TransactionalInformation result = new TransactionalInformation();
                    if (data.Equals("ok"))
                    {
                        result.ReturnMessage.Add("OK");
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