using BackEndSAM.Models.Embarque.RevisionEmbarque;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Embarque.RevisionEmbarque
{
    public class RevisionEmbarqueBD
    {
        private static readonly object _mutex = new object();
        private static RevisionEmbarqueBD _instance;

        public static RevisionEmbarqueBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new RevisionEmbarqueBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoProyecto(int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_ListaProyectos_Result> result = ctx.Sam3_Embarque_RE_Get_ListaProyectos(UsuarioID).ToList();
                    List<DetalleProyecto> listaDetalle = new List<DetalleProyecto>();
                    listaDetalle.Add(new DetalleProyecto());

                    foreach (Sam3_Embarque_RE_Get_ListaProyectos_Result item in result)
                    {
                        listaDetalle.Add(new DetalleProyecto
                        {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre
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

        public object ObtenerListadoPaquete(int ProyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_ListaPaquetes_Result> result = ctx.Sam3_Embarque_RE_Get_ListaPaquetes(ProyectoID).ToList();
                    List<DetallePaquete> listaDetalle = new List<DetallePaquete>();
                    listaDetalle.Add(new DetallePaquete());

                    foreach (Sam3_Embarque_RE_Get_ListaPaquetes_Result item in result)
                    {
                        listaDetalle.Add(new DetallePaquete {
                                PaqueteID = item.PaqueteID,
                                Nombre = item.Nombre
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

        public object ObtenerListadoEmbarques(int ProveedorID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_ListadoEmbarque_Result> result = ctx.Sam3_Embarque_RE_Get_ListadoEmbarque(ProveedorID).ToList();
                    List<EmbarqueEnviado> listaDetalle = new List<EmbarqueEnviado>();
                    listaDetalle.Add(new EmbarqueEnviado());

                    foreach (Sam3_Embarque_RE_Get_ListadoEmbarque_Result item in result)
                    {
                        listaDetalle.Add(new EmbarqueEnviado
                        {
                            EmbarqueID = item.EmbarqueID,
                            Nombre = item.Embarque,
                            Destino = item.Destino,
                            Estatus = item.Estatus,
                            ProyectoID = item.ProyectoID.GetValueOrDefault(),
                            RevisionEmbarqueID = item.RevisionEmbarqueID,
                            RevisionCerrada = item.RevisionCerrada
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

        public object ObtenerDetalleSpoolAgregar(int TipoConsulta, int OrdenTrabajoSpoolID, string Codigo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_DetalleSpool_Result> result = ctx.Sam3_Embarque_RE_Get_DetalleSpool(TipoConsulta,OrdenTrabajoSpoolID, Codigo).ToList();
                    List<DetalleSpoolAgregar> listaDetalle = new List<DetalleSpoolAgregar>();

                    foreach (Sam3_Embarque_RE_Get_DetalleSpool_Result item in result)
                    {
                        listaDetalle.Add(new DetalleSpoolAgregar
                        {
                            Accion = 1,                            
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            CuadranteID = item.CuadranteSam3ID.GetValueOrDefault(),
                            EmpaquetadoID = item.EmpaquetadoID.GetValueOrDefault(),
                            DetalleCargaID = item.DetalleCargaID.GetValueOrDefault(),
                            EmbarqueID = item.EmbarqueID.GetValueOrDefault(),
                            Embarque = item.Embarque,
                            EmbarqueEstatusID = item.EmbarqueEstatusID.GetValueOrDefault(),
                            Plana = item.Plana,
                            PaqueteID = item.PaqueteID.GetValueOrDefault(),
                            Paquete = item.Paquete,
                            DetalleRevisionID = item.DetalleRevisionID.GetValueOrDefault(),
                            Llego = item.Llego.GetValueOrDefault(),
                            NoLlego = item.NoLlego.GetValueOrDefault(),
                            LlegoComentario = item.LlegoComentario.GetValueOrDefault(),
                            EstatusSpool = ObtieneEstatusSpool(item.Llego.GetValueOrDefault(), item.NoLlego.GetValueOrDefault(), item.LlegoComentario.GetValueOrDefault()),
                            Comentario = item.Comentario,
                            ComentarioActual = item.Comentario,
                            CapturaManual = true,
                            ModificadoPorUsuario = true,
                            RowOk = true                            
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

        public object ObtenerDetallePaqueteAgregar(int PaquetID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_DetallePaquete_Result> result = ctx.Sam3_Embarque_RE_Get_DetallePaquete(PaquetID).ToList();
                    List<DetallePaqueteAgregar> listaDetalle = new List<DetallePaqueteAgregar>();

                    foreach (Sam3_Embarque_RE_Get_DetallePaquete_Result item in result)
                    {
                        listaDetalle.Add(new DetallePaqueteAgregar
                        {
                            Accion = 1,
                            ProyectoID = item.ProyectoID,
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            EmpaquetadoID = item.EmpaquetadoID,
                            CargaPlanaID = item.CargaPlanaID.GetValueOrDefault(),
                            Plana = item.Plana,
                            EmbarqueID = item.EmbarqueID.GetValueOrDefault(),
                            Embarque = item.Embarque,
                            EmbarqueEstatusID = item.EmbarqueEstatusID.GetValueOrDefault(),
                            PaqueteID = item.PaqueteID,
                            Paquete = item.Paquete,
                            DetalleRevisionID = item.DetalleRevisionID.GetValueOrDefault(),
                            Llego = item.Llego.GetValueOrDefault(),
                            NoLlego = item.NoLlego.GetValueOrDefault(),
                            LlegoComentario = item.LlegoComentario.GetValueOrDefault(),
                            EstatusSpool = ObtieneEstatusSpool(item.Llego.GetValueOrDefault(), item.NoLlego.GetValueOrDefault(), item.LlegoComentario.GetValueOrDefault()),
                            Comentario = item.Comentario,
                            ComentarioActual = item.Comentario,
                            CapturaManual = true,
                            ModificadoPorUsuario = true,
                            RowOk = true,
                            EstuvoEnPaquete = item.EstuvoEnPaquete.GetValueOrDefault()
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

        public object ObtenerDetalleEmbarques(int EmbarqueID, int Muestra)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Embarque_RE_Get_DetalleEmbarque_Result> result = ctx.Sam3_Embarque_RE_Get_DetalleEmbarque(EmbarqueID, Muestra).ToList();
                    List<DetalleRevisionEmbarque> listaDetalle = new List<DetalleRevisionEmbarque>();

                    foreach (Sam3_Embarque_RE_Get_DetalleEmbarque_Result item in result)
                    {
                        listaDetalle.Add(new DetalleRevisionEmbarque
                        {
                            Accion = item.Accion,
                            SpoolID = item.SpoolID,
                            NumeroControl = item.NumeroControl,
                            EmbarqueID = item.EmbarqueID,
                            EmbarqueEstatusID = item.EmbarqueEstatusID,
                            Paquete = item.Paquete,
                            PaqueteID = item.PaqueteID,
                            DetalleRevisionID = item.DetalleRevisionID,
                            Llego = item.Llego.GetValueOrDefault(),
                            NoLlego = item.NoLlego.GetValueOrDefault(),
                            LlegoComentario = item.LlegoComentario.GetValueOrDefault(),
                            EstatusSpool = ObtieneEstatusSpool(item.Llego.GetValueOrDefault(), item.NoLlego.GetValueOrDefault(), item.LlegoComentario.GetValueOrDefault()),
                            Comentario = item.Comentario,
                            ComentarioActual = item.Comentario,
                            CapturaManual = item.CapturaManual.GetValueOrDefault(),
                            ModificadoPorUsuario = false,
                            RowOk = true,
                            Empaquetado = item.EstuvoEnPaquete
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

        public object DescargarSpool(int SpoolID, int DetalleCargaID, int EmpaquetadoID, int CuadranteID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Sam3_Embarque_RE_DescargaSpool(SpoolID, DetalleCargaID, EmpaquetadoID, CuadranteID, UsuarioID);

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

        public object DescargarPaquete(int PaqueteID, int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ctx.Sam3_Embarque_RE_DescargaPaquete(PaqueteID, UsuarioID);

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

        public object GuardarCapturaRevision(DataTable dtDetalle, int EmbarqueID, int Cerrado, int UsuarioID)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametros = { { "@UsuarioID", UsuarioID.ToString() }, { "@EmbarqueID", EmbarqueID.ToString() },
                    {"@Cerrado", Cerrado.ToString() } };

                _SQL.Ejecuta(Stords.GUARDARREVISIONEMBARQUE, dtDetalle, "@DTEmbarqueRevision", parametros);

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return result;

            }
            catch(Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public string ObtieneEstatusSpool(bool Llego, bool NoLlego, bool LlegoComentario)
        {
            string estatus = "";

            if (Llego)
                estatus = "Llego";

            if(NoLlego)
                estatus = "NoLlego";

            if (LlegoComentario)
                estatus = "LlegoComentario";

            return estatus;
        }        

    }
}