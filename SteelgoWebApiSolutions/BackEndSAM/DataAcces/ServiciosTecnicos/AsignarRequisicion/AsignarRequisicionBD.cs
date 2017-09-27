using BackEndSAM.Models.Sam3General;
using BackEndSAM.Models.ServiciosTecnicos.AsignarRequisicion;
using BackEndSAM.Models.ServiciosTecnicos.ServiciosTecnicosGeneral;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.ServiciosTecnicos
{
    public class AsignarRequisicionBD
    {
        private static readonly object _mutex = new object();
        private static AsignarRequisicionBD _instance;

        public static AsignarRequisicionBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AsignarRequisicionBD();
                    }
                }
                return _instance;
            }
        }


        public object ObtenerRequisicionAsignacion(string lenguaje, int tipoVista, int tipoPruebaID, int proyectoID, int patioID, int requisicionID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_ST_Get_AsignarRequisicion_Result> result = ctx.Sam3_ST_Get_AsignarRequisicion(lenguaje, tipoVista, tipoPruebaID, proyectoID,requisicionID).ToList();
                    List<RequisicionAsignacion> ListadoRequisicionAsignacion = new List<RequisicionAsignacion>();
                    List<TurnoLaboral> listaTurnoLaboralAll = (List<TurnoLaboral>)ObtenerTurnoLaboralTotal(lenguaje, proyectoID, tipoPruebaID);
                    List<Proveedor> listaProveedores = (List<Proveedor>)ServiciosTecnicosGeneral.ServiciosTecnicosGeneralBD.Instance.ObtenerListadoProveedores(proyectoID, patioID, tipoPruebaID);
                    List<Equipo> listaEquipoAll = (List<Equipo>)ServiciosTecnicosGeneral.ServiciosTecnicosGeneralBD.Instance.ObtenerListadoEquipos(tipoPruebaID, 0, lenguaje);
                    foreach (Sam3_ST_Get_AsignarRequisicion_Result item in result)
                    {
                        
                        ListadoRequisicionAsignacion.Add(new RequisicionAsignacion
                        {
                            Accion = item.RequisicionAsignacionID == 0 ? 1 : 2,
                            RequisicionAsignacionID = item.RequisicionAsignacionID,
                            Nombre = item.TipoPrueba,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            Observacion = item.Observaciones,
                            Fecha = item.Fecha,
                            CantidadJuntas = item.NumeroElementos.GetValueOrDefault(),
                            Capacidad = item.Capacidad.GetValueOrDefault() == 0 ? "" : item.Capacidad.GetValueOrDefault().ToString(),
                            JuntasAsignadas = item.ElementosAsignados == 0 ? "" : item.ElementosAsignados.ToString(),
                            JuntasAsignadasOriginal = item.ElementosAsignados == 0 ? "" : item.ElementosAsignados.ToString(),
                            ProveedorID = item.ProveedorID.GetValueOrDefault(),
                            Proveedor = item.Proveedor == null ? "" : item.Proveedor,
                            RequisicionID = item.RequisicionID,
                            Requisicion = item.Requisicion,
                            ProyectoID = item.ProyectoID,
                            ListaProveedor = listaProveedores,
                            Equipo = item.Equipo == null ? "" : item.Equipo,
                            EquipoID = item.EquipoID.GetValueOrDefault(),
                            TurnoLaboral = item.TurnoLaboral,
                            TurnoLaboralID = item.TurnoLaboralID.GetValueOrDefault(),
                            ListaEquipos = item.ProveedorID.GetValueOrDefault() == 0 ? new List<Equipo>() : (List<Equipo>)ServiciosTecnicosGeneral.ServiciosTecnicosGeneralBD.Instance.ObtenerListadoEquipos(item.TipoPruebaID.GetValueOrDefault(), item.ProveedorID.GetValueOrDefault(), lenguaje),
                            ListaEquiposTotal = listaEquipoAll,
                            ListaTurnoLaboral = item.ProveedorID.GetValueOrDefault() == 0 ? new List<TurnoLaboral>() : (List<TurnoLaboral>)ServiciosTecnicosGeneral.ServiciosTecnicosGeneralBD.Instance.ObtenerListadoTurnos(item.TipoPruebaID.GetValueOrDefault(), item.ProveedorID.GetValueOrDefault(), item.EquipoID.GetValueOrDefault(),item.ProyectoID, lenguaje),
                            ListaTurnoLaboralTotal = listaTurnoLaboralAll,
                            RequiereEquipo = item.RequiereEquipo,
                            ListaElementosRequisicion = (List<ElementosRequisicion>)ObtenerElementosRequisicion(lenguaje, item.ProyectoID, item.TipoPruebaID.GetValueOrDefault(), item.RequisicionID),
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoEquipoOriginalID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoEquipoAnteriorID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            CapacidadTurnoProveedorAnteriorID = item.CapacidadTurnoProveedorID,
                            CapacidadTurnoProveedorOriginalID = item.CapacidadTurnoProveedorID,
                            ProveedorEquipoID = item.ProveedorEquipoID,
                            ListaElementosAsignadosTurno = new List<ElementosRequisicion>()
                            //ListaElementosAsignadosTurno = item.CapacidadTurnoEquipoID == 0 && item.CapacidadTurnoProveedorID == 0 ? new List<ElementosRequisicion>(): (List<ElementosRequisicion>)ObtenerElementosAsignadosTurno(lenguaje, item.ProyectoID, item.CapacidadTurnoEquipoID, item.CapacidadTurnoProveedorID, 0)

                        });
                    }

                    return ListadoRequisicionAsignacion;
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


        public object ObtenerElementosRequisicion(string lenguaje, int proyectoID, int tipoPruebaID, int requisicionID)
        {
            try
            {
                List<ElementosRequisicion> listaElementosRequisicion = new List<ElementosRequisicion>();
                using (SamContext ctx = new SamContext())
                {
                    
                    List<Sam3_ST_Get_ElementosPorPruebaRequisicion_Result> result = ctx.Sam3_ST_Get_ElementosPorPruebaRequisicion(lenguaje,proyectoID,tipoPruebaID,requisicionID).ToList();
                    foreach (var item in result)
                    {
                        listaElementosRequisicion.Add(new ElementosRequisicion
                        {
                            Cedula = item.Cedula,
                            Clasificacion = item.Clasificacion,
                            Cuadrante = item.Cuadrante,
                            Diametro = item.DiametroPlano.ToString(),
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Especificacion = item.Especificacion,
                            Espesor  = item.Espesor.ToString(),
                            EtiquetaJunta = item.EtiquetaJunta,
                            JuntaSpoolID = item.JuntaSpoolID.GetValueOrDefault(),
                            NombreRequisicion = item.NombreRequisicion,
                            NumeroControl = item.NumeroControl,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            RequisicionID = item.RequisicionID,
                            SpoolID = item.SpoolID.GetValueOrDefault(),
                            TipoJunta = item.TipoJunta,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            PermiteSeparar = item.PermiteSeparar,
                            Agregar = false,
                            ClasificacionPNDID = item.ClasificacionPNDID.GetValueOrDefault(),
                            ClasificacionManual = item.ClasificacionManual.GetValueOrDefault() ? 1: 0,
                            OrdenTrabajoID = item.OrdenTrabajoID

                        });
                    }
                }
                return listaElementosRequisicion;
            }
            catch (Exception ex)
            {
                List<ElementosRequisicion> listaElementosRequisicion = new List<ElementosRequisicion>();
                return listaElementosRequisicion;
            }
        }


        public object ObtenerElementosAsignadosTurno(string lenguaje, int proyectoID, int CapacidadTurnoEquipoID, int CapacidadTurnoProveedorID, int requisicionID)
        {
            try
            {
                List<ElementosRequisicion> listaElementosRequisicion = new List<ElementosRequisicion>();
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_ST_Get_ElementosTurnoAsignados_Result> result = ctx.Sam3_ST_Get_ElementosTurnoAsignados(lenguaje,proyectoID, CapacidadTurnoEquipoID, CapacidadTurnoProveedorID, requisicionID).ToList();
                    foreach (Sam3_ST_Get_ElementosTurnoAsignados_Result item in result)
                    {
                        listaElementosRequisicion.Add(new ElementosRequisicion
                        {
                            Cedula = item.Cedula,
                            Clasificacion = item.Clasificacion,
                            Cuadrante = item.Cuadrante,
                            Diametro = item.Diametro.ToString(),
                            ElementoPorClasificacionPNDID = item.ElementoPorClasificacionPNDID,
                            Especificacion = item.Especificacion,
                            Espesor = item.Espesor.ToString(),
                            EtiquetaJunta = item.EtiquetaJunta,
                            JuntaSpoolID = item.JuntaSpoolID,
                            NombreRequisicion = item.NombreRequisicion,
                            NumeroControl = item.NumeroControl,
                            OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID,
                            Prioridad = item.Prioridad.GetValueOrDefault(),
                            ProyectoID = item.ProyectoID,
                            RequisicionID = item.RequisicionID,
                            SpoolID = item.SpoolID,
                            TipoJunta = item.TipoJunta,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID.GetValueOrDefault(),
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID.GetValueOrDefault()

                        });
                    }
                }
                return listaElementosRequisicion;
            }
            catch (Exception ex)
            {
                List<ElementosRequisicion> listaElementosRequisicion = new List<ElementosRequisicion>();
                return listaElementosRequisicion;
            }
        }



        public object ObtenerTurnoLaboralTotal (string lenguaje,int proyectoID, int tipoPruebaID)
        {
            try
            {
                List<TurnoLaboral> listaTurnoLaboralTotal = new List<TurnoLaboral>();
                using (SamContext ctx = new SamContext())
                {

                    List<Sam3_ST_Get_TurnoLaboralAsignarRequisicion_Result> result = ctx.Sam3_ST_Get_TurnoLaboralAsignarRequisicion(lenguaje,tipoPruebaID).ToList();
                    foreach (var item in result)
                    {
                        listaTurnoLaboralTotal.Add(new TurnoLaboral
                        {
                            TurnoLaboralID = item.TurnoLaboralID,
                            Nombre = item.Turno,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                            Capacidad = item.Capacidad,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            JuntasAsignadas = item.ElementosAsignados.ToString(),
                            ProveedorEquipoID = item.ProveedorEquipoID,
                            ListaElementosAsignadosTurno = item.ElementosAsignados == 0 ?  new List<ElementosRequisicion>() : (List<ElementosRequisicion>) ObtenerElementosAsignadosTurno(lenguaje,proyectoID,item.CapacidadTurnoEquipoID,item.CapacidadTurnoProveedorID,0)
                        });
                    }
                }
                return listaTurnoLaboralTotal;
            }
            catch (Exception ex)
            {
                List<TurnoLaboral> listaTurnoLaboralTotal = new List<TurnoLaboral>();
                return listaTurnoLaboralTotal;
            }
        }


        public object ObtieneElementosRequisicion(string lenguaje, int requisicionID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Models.ServiciosTecnicos.AsignarRequisicion.ElementoRequisicion elemento = null;
                    Sam3_ST_Get_AR_RequisicionParametro_Result result = ctx.Sam3_ST_Get_AR_RequisicionParametro(lenguaje,requisicionID).SingleOrDefault();

                    if (result != null && result.RequisicionID != 0)
                    {
                        elemento = new Models.ServiciosTecnicos.AsignarRequisicion.ElementoRequisicion();

                        List<Models.ServiciosTecnicos.EntregaPlacasGraficas.Proyecto> listaProyecto = new List<Models.ServiciosTecnicos.EntregaPlacasGraficas.Proyecto>();
                        listaProyecto.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.Proyecto());
                        listaProyecto.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.Proyecto
                        {
                            ProyectoID = result.ProyectoID,
                            Nombre = result.Proyecto,
                            PatioID = result.PatioID
                              
                        });

                        List<Models.ServiciosTecnicos.EntregaPlacasGraficas.TipoPrueba> listaTipoPrueba = new List<Models.ServiciosTecnicos.EntregaPlacasGraficas.TipoPrueba>();
                        listaTipoPrueba.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.TipoPrueba());
                        listaTipoPrueba.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.TipoPrueba
                        {
                            TipoPruebaID = result.TipoPruebaID.GetValueOrDefault(),
                            Nombre = result.TipoPrueba
                        });


                        List<Models.ServiciosTecnicos.EntregaPlacasGraficas.Requisicion> listaRequisicion = new List<Models.ServiciosTecnicos.EntregaPlacasGraficas.Requisicion>();
                        listaRequisicion.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.Requisicion());
                        listaRequisicion.Add(new Models.ServiciosTecnicos.EntregaPlacasGraficas.Requisicion
                        {
                            RequisicionID = result.RequisicionID,
                            ProyectoID = result.ProyectoID,
                            NombreRequisicion = result.NumeroRequisicion,
                            TipoPruebaID = result.TipoPruebaID.GetValueOrDefault(),
                        });


                        elemento.RequisicionID = result.RequisicionID;
                        elemento.ProyectoID = result.ProyectoID;
                        elemento.TipoPruebaID = result.TipoPruebaID.GetValueOrDefault();
                        elemento.listaProyecto = listaProyecto;
                        elemento.listaTipoPrueba = listaTipoPrueba;
                        elemento.listaRequisicion = listaRequisicion;
                    }


                    return elemento;
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

    }
}