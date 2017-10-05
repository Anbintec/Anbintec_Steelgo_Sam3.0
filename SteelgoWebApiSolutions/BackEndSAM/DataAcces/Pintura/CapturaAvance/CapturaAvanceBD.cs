using BackEndSAM.Models.Pintura.PinturaGeneral;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using BackEndSAM.Models.Pintura.CapturaAvance;
using System.Data;
using DatabaseManager.Constantes;
using BackEndSAM.Models.Sam3General.Cuadrante;
using BackEndSAM.DataAcces.Sam3General.Cuadrante;
using Newtonsoft.Json;

namespace BackEndSAM.DataAcces.PinturaBD.CapturaAvanceBD
{
    public class CapturaAvanceBD
    {

        private static readonly object _mutex = new object();
        private static CapturaAvanceBD _instance;
       
        public static CapturaAvanceBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CapturaAvanceBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerCamposPredeterminados(Sam3_Usuario usuario, string lenguaje, int idCampoPredeterminado)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    var oMyString = new ObjectParameter("Retorna", typeof(string));
                    var result = ctx.Sam3_Steelgo_Get_CampoPredeterminado(idCampoPredeterminado, lenguaje, oMyString);
                    var data = oMyString.Value.ToString();

                    return data;
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

        public object ObtenerCarrosCerradosPorProceso(string lenguaje, int procesoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Pintura_Avance_Get_CarrosCerrados_Result> result = ctx.Sam3_Pintura_Avance_Get_CarrosCerrados(procesoID).ToList();
                    List<CarroCerrado> ListadoMedioTransporte = new List<CarroCerrado>();

                    if (result.Count > 0)
                        ListadoMedioTransporte.Add(new CarroCerrado());

                    foreach (Sam3_Pintura_Avance_Get_CarrosCerrados_Result item in result)
                    {
                        ListadoMedioTransporte.Add(new CarroCerrado
                        {
                            MedioTransporteID = item.MedioTransporteID,
                            Nombre = item.Nombre,
                            MedioTransporteCargaID = item.MedioTransporteCargaID,
                            SistemaPinturaProyectoID = item.SistemaPinturaProyectoID,
                            CuadranteID=item.CuadranteID
                        });

                    }
                    return ListadoMedioTransporte;
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

        public object ObtenerListaMedioTransporteCargado(int cargaCarroID, string lenguaje, int sistemaPinturaProyectoID, int procesoPinturaID,int colorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    RetornaDetalles detalles = new RetornaDetalles();
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@cargaCarroID", cargaCarroID.ToString() }, { "@Lenguaje", lenguaje }, { "@sistemaPinturaProyectoID", sistemaPinturaProyectoID.ToString() }, { "@procesoPinturaID", procesoPinturaID.ToString() }, { "@ColorID", colorID.ToString() } };
                    DataTable dtDetalle = _SQL.EjecutaDataAdapter(Stords.OBTIENEDETALLEAVANCE, parametro);
                    return dtDetalle;
                   // return  JsonConvert.SerializeObject(dtDetalle);

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

        public object ObtenerSpoolNuevo(int OrdenTrabajoSpoolID, string lenguaje,int procesoPinturaID,int usuario, int CargaCarroID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    RetornaDetalles detalles = new RetornaDetalles();
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@OrdenTrabajoSpoolID", OrdenTrabajoSpoolID.ToString() }, { "@Lenguaje", lenguaje }, { "@procesoPinturaID", procesoPinturaID.ToString() }, { "@cargaCarroID", CargaCarroID.ToString() } };
                    DataTable dtDetalle = _SQL.EjecutaDataAdapter(Stords.AGREGARSPOOLSAVANCE, parametro);
                    return dtDetalle;
                   

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

        public object ObtenerObreros(int procesoPinturaID,int usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Pintura_Get_Obrero_Result> result = ctx.Sam3_Pintura_Get_Obrero(procesoPinturaID, usuario).ToList();
                    List<PintorSpool> ListadoPintores = new List<PintorSpool>();

                    foreach (Sam3_Pintura_Get_Obrero_Result item in result)
                    {
                        ListadoPintores.Add(new PintorSpool
                        {
                            Accion = 1,
                            Codigo = item.Codigo,
                            ObreroID = item.ObreroID,
                            AvanceCarroObreroId = 0
                        });
                    }




                    return ListadoPintores.OrderBy(x => x.ObreroID).ToList();
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

        public object ObtenerObrerosGuardados( int spoolID, int procesoID,int usuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Avance_Get_ObrerosProcesoPintura_Result> result = ctx.Sam3_Pintura_Avance_Get_ObrerosProcesoPintura(spoolID, procesoID).ToList();

                    List<object> listaObreros = new List<object>();

                   
                    List<PintorSpool> ListadoPintores = new List<PintorSpool>();

                    foreach (Sam3_Pintura_Avance_Get_ObrerosProcesoPintura_Result item in result)
                    {
                        ListadoPintores.Add(new PintorSpool
                        {
                            Accion = 2,
                            ObreroID = item.ObreroID.GetValueOrDefault(),
                            Codigo = item.Codigo,
                            AvanceCarroObreroId = item.AvanceCarroObreroId,
                        });
                    }
                    listaObreros.Add(ListadoPintores);
                    //listaObreros.Add(ObtenerObreros(procesoID, usuarioID));
                    return listaObreros;
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

        public object GuardarAvanceCarro(DataTable dtDetalleCaptura, DataTable dtDetalleObreros, DataTable dtDetalleComponentes, Sam3_Usuario usuario, string lenguaje, int CargaCarroID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.UsuarioID.ToString() }, { "@Lenguaje", lenguaje }, { "@CargaCarroID", CargaCarroID.ToString() } };
                    _SQL.Ejecuta(Stords.GUARDACAPTURAAVANCE, dtDetalleCaptura, "@TablaCapturaAvance", dtDetalleObreros, "@TablaObreros", dtDetalleComponentes,"@TablaComponentes", parametro);

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

        public object ObtenerLayoutProcesoPintura(int sistemaPinturaProyectoId, int procesoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_Pintura_Avance_Get_LayoutComponentes_Result> result = ctx.Sam3_Pintura_Avance_Get_LayoutComponentes(sistemaPinturaProyectoId, procesoID, lenguaje).ToList();

                    List<Sam3_Pintura_Avance_Get_LayoutReducores_Result> resultReductores = ctx.Sam3_Pintura_Avance_Get_LayoutReducores(sistemaPinturaProyectoId, procesoID, lenguaje).ToList();

                    List<object> listadoColumns = new List<object>();

                    List<Componente> ListadoLayoutComponentes = new List<Componente>();
                    List<Reductor> ListadoLayoutReductor = new List<Reductor>();
                    
                    List<ListadoLotesPorColumna> listadoLotesPorColumnaComponentes = new List<ListadoLotesPorColumna>();
                    List<ListadoLotesPorColumna> listadoLotesPorColumnaReductores = new List<ListadoLotesPorColumna>();
                    foreach (Sam3_Pintura_Avance_Get_LayoutComponentes_Result item in result)
                    {
                        ListadoLayoutComponentes.Add(new Componente
                        {
                            NombreComponente = item.NombreComponente
                        });

                        listadoLotesPorColumnaComponentes.Add(new ListadoLotesPorColumna {
                            NombreColumna= item.NombreComponente,
                            ListadoLotes = ObtenerLotesComponentes(item.NombreComponente, lenguaje)
                        });
                    }

                    foreach (Sam3_Pintura_Avance_Get_LayoutReducores_Result item in resultReductores)
                    {
                            ListadoLayoutReductor.Add(new Reductor
                            {
                                NombreReductor = item.NombreReductor
                            });
                        listadoLotesPorColumnaReductores.Add(new ListadoLotesPorColumna
                        {
                            NombreColumna = item.NombreReductor,
                            ListadoLotes = ObtenerLotesReductor(item.NombreReductor, lenguaje)
                        });
                    }

                    listadoColumns.Add(ListadoLayoutComponentes);
                    listadoColumns.Add(ListadoLayoutReductor);
                    listadoColumns.Add(listadoLotesPorColumnaComponentes);
                    listadoColumns.Add(listadoLotesPorColumnaReductores);

                    return listadoColumns;
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

        public List<Lote> ObtenerLotesComponentes(string componente, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Avance_Get_LotesComponente_Result> result = ctx.Sam3_Pintura_Avance_Get_LotesComponente(componente, lenguaje).ToList();
                    List<Lote> ListadoLotes = new List<Lote>();
                    if (result.Count > 0)
                        ListadoLotes.Add(new Lote());

                    foreach (Sam3_Pintura_Avance_Get_LotesComponente_Result item in result)
                    {
                        ListadoLotes.Add(new Lote
                        {
                            NombreLote=item.Lote,
                            Cantidad=item.Cantidad
                        });
                    }
                    return ListadoLotes;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return new List<Lote>();
            }
        }

        public List<Lote> ObtenerLotesReductor(string componente, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_Avance_Get_LotesReductor_Result> result = ctx.Sam3_Pintura_Avance_Get_LotesReductor(componente, lenguaje).ToList();
                    List<Lote> ListadoLotes = new List<Lote>();
                    if (result.Count > 0)
                        ListadoLotes.Add(new Lote());

                    foreach (Sam3_Pintura_Avance_Get_LotesReductor_Result item in result)
                    {
                        ListadoLotes.Add(new Lote
                        {
                            NombreLote = item.Lote,
                            Cantidad = item.Cantidad
                        });
                    }
                    return ListadoLotes;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return new List<Lote>();
            }
        }



    }
}