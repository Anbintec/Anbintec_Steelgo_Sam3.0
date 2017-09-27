using BackEndSAM.DataAcces.Sam3General.Cuadrante;
using BackEndSAM.Models.Embarque.Etiquetado;
using BackEndSAM.Models.Sam3General.Cuadrante;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Embarque.Etiquetado
{
    public class EtiquetadoBD
    {
        private static readonly object _mutex = new object();
        private static EtiquetadoBD _instance;

        public static EtiquetadoBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EtiquetadoBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtieneElementosPorBusqueda(int TipoConsulta, int ZonaID, int CuadranteID, string NumeroControl, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> elementos = ctx.Sam3_Embarque_Etiquetado_NumeroElementosBusqueda(TipoConsulta, ZonaID, CuadranteID, NumeroControl, UsuarioID);

                    var valor = elementos.Where(x => x.HasValue).Select(x => x.Value).ToList()[0];

                    return valor;
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

        public object ObtieneDetalleEtiquetadoPorZona(int ZonaID, int CuadranteID, int Todos, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Obtiene el catalogo de Cuadrantes
                    List<UbicacionCuadrante> listCuadrantes = (List<UbicacionCuadrante>)CuadranteBD.Instance.ObtenerCuadrante(ZonaID);

                    List<Sam3_Embarque_get_Etiquetado_Zona_Result> result = ctx.Sam3_Embarque_get_Etiquetado_Zona(ZonaID, CuadranteID, Todos, UsuarioID).ToList();
                    List<DetalleEtiquetado> listaDetalle = new List<DetalleEtiquetado>();


                    foreach (Sam3_Embarque_get_Etiquetado_Zona_Result item in result)
                    {
                        listaDetalle.Add(new DetalleEtiquetado {
                            Accion = item.EtiquetadoID != 0 ? 2 : 1,
                            SpoolID = item.SpoolID,
                            Spool = item.Spool,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,
                            CuadranteAnteriorSam2ID = item.CuadranteSam2ID,
                            CuadranteAnteriorSam3ID = item.CuadranteID,
                            CuadranteID = item.CuadranteID,
                            CuadranteSam2ID = item.CuadranteSam2ID,
                            Cuadrante = item.Cuadrante,
                            EtiquetadoID = item.EtiquetadoID,
                            Etiquetado = item.Etiquetado,
                            ModificadoPorUsuario = false,
                            OkPintura = item.OkPintura == 0 ? false : true,
                            OkPnd = item.OkPND,
                            ZonaID = item.ZonaID,
                            Zona = item.Zona,
                            RutaEtiqueta = item.RutaEtiqueta,
                            RutaDibujo = item.RutaDibujo,
                            RutaIsometrico = item.RutaIsometrico,
                            RutaPlano = item.RutaPlano,
                            ListaCuadrantes = listCuadrantes
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

        public object ObtieneDetalleEtiquetadoPorSpool(string SpoolContiene, int Todos, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Obtiene catalogo de cuadrantes
                    List<UbicacionCuadrante> listCuadrantes = (List<UbicacionCuadrante>)CuadranteBD.Instance.ObtenerCuadranteSpool(SpoolContiene, UsuarioID);

                    List<Sam3_Embarque_get_Etiquetado_NumeroControl_Result> result = ctx.Sam3_Embarque_get_Etiquetado_NumeroControl(SpoolContiene, Todos, UsuarioID).ToList();
                    List<DetalleEtiquetado> listaDetalle = new List<DetalleEtiquetado>();

                    foreach (Sam3_Embarque_get_Etiquetado_NumeroControl_Result item in result)
                    {
                        listaDetalle.Add(new DetalleEtiquetado
                        {
                            Accion = item.EtiquetadoID != 0 ? 2 : 1,
                            SpoolID = item.SpoolID,
                            Spool = item.Spool,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,
                            CuadranteAnteriorSam2ID = item.CuadranteSam2ID,
                            CuadranteAnteriorSam3ID = item.CuadranteID,
                            CuadranteID = item.CuadranteID,
                            CuadranteSam2ID = item.CuadranteSam2ID,
                            Cuadrante = item.Cuadrante,
                            EtiquetadoID = item.EtiquetadoID,
                            Etiquetado = item.Etiquetado,
                            ModificadoPorUsuario = false,
                            OkPintura = item.OkPintura == 0 ? false : true,
                            OkPnd = item.OkPND,
                            ZonaID = item.ZonaID,
                            Zona = item.Zona,
                            RutaEtiqueta = item.RutaEtiqueta,
                            RutaDibujo = item.RutaDibujo,
                            RutaIsometrico = item.RutaIsometrico,
                            RutaPlano = item.RutaPlano,
                            ListaCuadrantes = listCuadrantes
                        });
                    }

                    return listaDetalle.OrderByDescending(y => y.Etiquetado); //.Take<DetalleEtiquetado>(10000);
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

        public object GuardaCapturaEtiquetado(DataTable dtEtiquetado, int UsuarioID, string Lenguaje)
        {
            try
            {

                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", UsuarioID.ToString()}, { "@Lenguaje", Lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAETIQUETADO, dtEtiquetado, "@EmbarqueEtiquetado", parametro);

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