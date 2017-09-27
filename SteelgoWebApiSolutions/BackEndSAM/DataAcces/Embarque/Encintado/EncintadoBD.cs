using BackEndSAM.DataAcces.Sam3General.Cuadrante;
using BackEndSAM.Models.Embarque.Encintado;
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

namespace BackEndSAM.DataAcces.Embarque.Encintado
{
    public class EncintadoBD
    {
        private static readonly object _mutex = new object();
        private static EncintadoBD _instance;

        public static EncintadoBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EncintadoBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerColoresEncintado(string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_get_Encintado_Colores_Result> resultColor = ctx.Sam3_Embarque_get_Encintado_Colores(lenguaje).ToList();
                    List<ColorEncintado> listaColores = new List<ColorEncintado>();
                    listaColores.Add(new ColorEncintado());
                    foreach (Sam3_Embarque_get_Encintado_Colores_Result item in resultColor)
                    {
                        listaColores.Add(new ColorEncintado
                        {
                            ColorID = item.ColorID,
                            Nombre = item.Color
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

        public object ObtieneElementosPorBusqueda(int TipoConsulta, int ZonaID, int CuadranteID, string NumeroControl, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjectResult<int?> elementos = ctx.Sam3_Embarque_Encintado_NumeroElementosBusqueda(TipoConsulta, ZonaID, CuadranteID, NumeroControl, UsuarioID);

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

        public object ObtieneDetalleEncitadoPorZona(int ZonaID, int CuadranteID, int Todos, string Lenguaje, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Obtiene catalogo de Cuadrantes
                    List<UbicacionCuadrante> listaCuadrante = (List<UbicacionCuadrante>)CuadranteBD.Instance.ObtenerCuadrante(ZonaID);

                    //ObtieneCatalogo de Colores
                    List<ColorEncintado> listaColores = (List<ColorEncintado>)EncintadoBD.Instance.ObtenerColoresEncintado(Lenguaje);

                    List<Sam3_Embarque_Get_Encintado_Zona_Result> result = ctx.Sam3_Embarque_Get_Encintado_Zona(ZonaID, CuadranteID, Todos, UsuarioID, Lenguaje).ToList();
                    List<DetalleEncintado> listaDetalle = new List<DetalleEncintado>();

                    foreach (Sam3_Embarque_Get_Encintado_Zona_Result item in result)
                    {
                        listaDetalle.Add(new DetalleEncintado
                        {
                            Accion = item.Accion,
                            SpoolID = item.SpoolID,
                            Spool = item.Spool,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,
                            CuadranteAnteriorSam2ID = item.CuadranteSam2ID,
                            CuadranteAnteriorSam3ID = item.CuadranteID,
                            CuadranteSam2ID = item.CuadranteSam2ID,
                            CuadranteID = item.CuadranteID,
                            Cuadrante = item.Cuadrante,
                            EncintadoID = item.EncintadoID,
                            Encintado = item.Encintado,
                            EtiquetadoID = item.EtiquetadoID,
                            Etiquetado = item.Etiquetado,
                            ColorAnteriorID = item.ColorID.GetValueOrDefault(),
                            ColorID = ((item.ColorID == null) ?(0):((int)item.ColorID)),
                            NombreColor = item.NombreColor,
                            OkPintura = ((item.OkPintura == 1) ? (true) : (false)),
                            OkPND = item.OkPND,
                            ZonaID = item.ZonaID,
                            Zona = item.Zona,
                            ModificadoPorUsuario = false,
                            RowOk = true,
                            ListaCuadrantes = listaCuadrante,
                            ListaColoresCinta = listaColores
                        });
                    }

                    return listaDetalle.OrderByDescending(y => y.Encintado).Take<DetalleEncintado>(10000);
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

        public object ObtieneDetalleEncitadoPorSpool(int ZonaID, string SpoolContiene, int Todos, string Lenguaje, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //Obtiene catalogo de cuadrantes
                    List<UbicacionCuadrante> listaCuadrante = (List<UbicacionCuadrante>)CuadranteBD.Instance.ObtenerCuadranteSpool(SpoolContiene, UsuarioID);

                    //Obtiene catalogo de colores                    
                    List<ColorEncintado> listaColores = (List<ColorEncintado>)EncintadoBD.Instance.ObtenerColoresEncintado(Lenguaje);

                    List<Sam3_Embarque_get_Encintado_NumeroControl_Result> result = ctx.Sam3_Embarque_get_Encintado_NumeroControl(SpoolContiene, Todos, UsuarioID, Lenguaje).ToList();
                    List<DetalleEncintado> listaDetalle = new List<DetalleEncintado>();

                    foreach (Sam3_Embarque_get_Encintado_NumeroControl_Result item in result)
                    {
                        listaDetalle.Add(new DetalleEncintado
                        {
                            Accion = item.Accion,
                            SpoolID = item.SpoolID,
                            Spool = item.Spool,
                            ProyectoID = item.ProyectoID,
                            Proyecto = item.Proyecto,
                            CuadranteAnteriorSam2ID = item.CuadranteSam2ID,
                            CuadranteAnteriorSam3ID = item.CuadranteID,
                            CuadranteSam2ID = item.CuadranteSam2ID,
                            CuadranteID = item.CuadranteID,
                            Cuadrante = item.Cuadrante,
                            EncintadoID = item.EncintadoID,
                            Encintado = item.Encintado,
                            EtiquetadoID = item.EtiquetadoID,
                            Etiquetado = item.Etiquetado,
                            ColorAnteriorID = item.ColorID,
                            ColorID = ((item.ColorID == null) ? (0) : ((int)item.ColorID)),
                            NombreColor = item.NombreColor,
                            OkPintura = ((item.OkPintura == 1) ? (true) : (false)),
                            OkPND = item.OkPND,
                            ZonaID = item.ZonaID,
                            Zona = item.Zona,
                            ModificadoPorUsuario = false,
                            RowOk = true,
                            ListaCuadrantes = listaCuadrante,
                            ListaColoresCinta = listaColores

                        });
                    }

                    return listaDetalle.OrderByDescending(y => y.Encintado).Take<DetalleEncintado>(10000);
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

        public object GuardarCapturaEncintado(DataTable dtDetalleCaptura, int usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.ToString() }, { "@Lenguaje", lenguaje } };
                    _SQL.Ejecuta(Stords.GUARDARCAPTURAENCINTADO, dtDetalleCaptura, "@EmbarqueEncintado", parametro);
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
    }
}