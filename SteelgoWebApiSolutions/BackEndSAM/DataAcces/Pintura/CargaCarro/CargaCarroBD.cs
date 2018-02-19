using BackEndSAM.Models.Pintura.CargaCarro;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.CargaCarro
{
    public class CargaCarroBD
    {
        private static readonly object _mutex = new Object();
        private static CargaCarroBD _instance;

        public static CargaCarroBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {

                        _instance = new CargaCarroBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtieneDetalleSpoolAgregar(int medioTransporteID, int TipoConsulta, int OrdenTrabajoSpoolID, string Codigo, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_DetalleSpool_Result> result = ctx.Sam3_Pintura_Get_DetalleSpool(medioTransporteID, TipoConsulta, OrdenTrabajoSpoolID, Codigo, lenguaje).ToList();

                    List<DetalleSpool> ListadoDetalleSpool = new List<DetalleSpool>();

                    foreach (Sam3_Pintura_Get_DetalleSpool_Result item in result)
                    {
                        ListadoDetalleSpool.Add(new DetalleSpool
                        {
                            Accion = item.Accion,
                            Area = item.Area.GetValueOrDefault(),
                            OrdenImportancia = item.OrdenImportancia.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            SistemaPintura = item.SistemaPintura,
                            SistemaPinturaID = item.SistemaPinturaID,
                            SpoolID = item.SpoolID,
                            SpoolJunta = item.SpoolJunta,
                            ProyectoID = item.ProyectoID,
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                            NombreMedioTransporte = item.NombreMedioTransporte,
                            CuadranteMedioTransporte = item.Cuadrante,
                            ColorPintura = item.ColorPintura,
                            PinturaSpoolID = item.PinturaSpoolID.GetValueOrDefault(),
                            PDIS=item.PDIS,
                            PEQS=item.PEQS
                        });
                    }
                    return ListadoDetalleSpool;
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

        public object ObtenerDetalleCargaCarro(int medioTransporteID, int proyectoID, string lenguaje,int tipobusqueda,string valorBusqueda)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Pintura_Get_DetalleCargaCarro_Result> result = ctx.Sam3_Pintura_Get_DetalleCargaCarro(medioTransporteID, proyectoID, lenguaje, tipobusqueda, valorBusqueda).ToList();

                    List<DetalleCargaCarro> listaDetalle = new List<DetalleCargaCarro>();
                    
                    foreach (Sam3_Pintura_Get_DetalleCargaCarro_Result item in result)
                    {
                        listaDetalle.Add(new DetalleCargaCarro
                        {
                            PatioID=item.PatioID,
                            Accion = item.Accion,
                            MedioTransporteCargaDetalleID = item.MedioTransporteCargaDetalleID,
                            EstatusCarga = item.EstatusCarga,
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            SistemaPinturaID = item.SistemaPinturaID.GetValueOrDefault(),
                            SistemaPintura = item.SistemaPintura,
                            ColorPintura = item.ColorPintura,
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                            CuadranteAnteriorID = item.CuadranteAnteriorID.GetValueOrDefault(),
                            Cuadrante = item.Cuadrante,
                            Area = item.Area.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            EstatusCaptura = 0,
                            ZonaAnteriorID=item.ZonaAnteriorID,
                            CarroID = item.CarroID,
                            MedioTransporte = item.MedioTransporte,
                            NoPintable=item.NoPintable,
                            PDIS = item.PDIS,
                            PEQS = item.PEQS
                        });
                        

                    }
                    return listaDetalle.OrderByDescending(x => x.NumeroControl);
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

        public object ObtenerDetalleCargaCarroBacklog(int medioTransporteID, int proyectoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Get_DetalleCargaCarroBackLog_Result> result = ctx.Sam3_Pintura_Get_DetalleCargaCarroBackLog(medioTransporteID, proyectoID, lenguaje).ToList();
                    List<DetalleCargaCarro> listaDetalle = new List<DetalleCargaCarro>();
                    foreach (Sam3_Pintura_Get_DetalleCargaCarroBackLog_Result item in result)
                    {
                        listaDetalle.Add( new DetalleCargaCarro
                        {
                            PatioID=item.PatioID,
                            Accion = item.Accion,
                            MedioTransporteCargaDetalleID = item.MedioTransporteCargaDetalleID.GetValueOrDefault(),
                            OrdenTrabajoID = item.OrdenTrabajoID,
                            SpoolID = item.SpoolID,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            NumeroControl = item.NumeroControl,
                            SistemaPinturaID = item.SistemaPinturaID.GetValueOrDefault(),
                            SistemaPintura = item.SistemaPintura,
                            ColorPintura = item.ColorPintura,
                            CuadranteID = item.CuadranteID.GetValueOrDefault(),
                            CuadranteAnteriorID = item.CuadranteAnteriorID.GetValueOrDefault(),
                            Cuadrante = item.Cuadrante,
                            Area = item.Area.GetValueOrDefault(),
                            Peso = item.Peso.GetValueOrDefault(),
                            MedioTransporte = item.MedioTransporte,
                            //CarroID=item.CarroID,
                            CarroCerrado = item.CarroCerrado.GetValueOrDefault(),
                            Seleccionado = item.EstatusCarga.GetValueOrDefault(),
                            EstatusCaptura = 0,
                            ZonaAnteriorID = item.ZonaAnteriorID,
                            PDIS = item.PDIS,
                            PEQS = item.PEQS
                        });
                    }

                    return listaDetalle.OrderByDescending(x => x.Seleccionado).ToList<DetalleCargaCarro>();

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

        public object GuardarCargaCarro(DataTable dtCarga, Sam3_Usuario usuario, string lenguaje, int medioTransporteID, int medioTransporteCargaID, int cerrar)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@CarroID", medioTransporteID.ToString() },
                         { "@Cerrar", cerrar.ToString() } , { "@CargaCarroID", medioTransporteCargaID.ToString() }};

                    _SQL.Ejecuta(Stords.GUARDACAPTURAPINTURASPOOLCARGA, dtCarga, "@Tabla", parametro);

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

        public object CierraCarro(Sam3_Usuario usuario, int medioTransporteID, int medioTransporteCargaID, int cerrar)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@MedioTransporteID", medioTransporteID.ToString() }, { "@MedioTransporteCargaID", medioTransporteCargaID.ToString() }, { "@Cerrar", cerrar.ToString() } };
                    _SQL.Ejecuta(Stords.SETCIERRACARRO, parametro);

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

        public object DescargaCarroSpool(int CarroID, int SpoolID, int CuadranteID, int CuadranteSamID, int UsuarioID,int Pantalla)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ctx.Sam3_Pintura_DescargaCuadranteSpool(CarroID, SpoolID, CuadranteID, CuadranteSamID, UsuarioID, Pantalla);

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