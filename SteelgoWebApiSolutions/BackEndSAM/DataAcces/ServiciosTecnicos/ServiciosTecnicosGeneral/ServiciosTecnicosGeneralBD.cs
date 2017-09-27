using BackEndSAM.Models.ServiciosTecnicos.AsignarRequisicion;
using BackEndSAM.Models.ServiciosTecnicos.ServiciosTecnicosGeneral;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndSAM.DataAcces.ServiciosTecnicos.ServiciosTecnicosGeneral
{
    public class ServiciosTecnicosGeneralBD
    {
        private static readonly object _mutex = new object();
        private static ServiciosTecnicosGeneralBD _instance;

        public static ServiciosTecnicosGeneralBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ServiciosTecnicosGeneralBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoTiposDePrueba(Sam3_Usuario usuario, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TiposDePrueba> listaTiposDePrueba = new List<TiposDePrueba>();
                    List<Sam3_ST_Get_TiposDePrueba_Result> listaProyectosCTX = ctx.Sam3_ST_Get_TiposDePrueba(lenguaje).ToList();
                    listaTiposDePrueba.Add(new TiposDePrueba());
                    foreach (Sam3_ST_Get_TiposDePrueba_Result item in listaProyectosCTX)
                    {
                        listaTiposDePrueba.Add(new TiposDePrueba
                        {
                            TipoPruebaID = item.TipoPruebaID,
                            Nombre = item.Nombre,
                            Categoria = item.Categoria,
                            TipoPruebaPorSpool = item.TipoPruebaPorSpool.GetValueOrDefault() ? 1 : 0,
                            RequiereEquipo = (bool)item.RequiereEquipo,
                            ClasificacionManual = item.ClasificacionManual,
                            ClasificacionManualID = item.ClasificacionManualID
                        });
                    }

                    return listaTiposDePrueba;
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

        public object ObtenerListadoRequisiciones(Sam3_Usuario usuario, int proyectoID, int tipoPruebaID, int estatusID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    
                    List<Requisicion> listaRequisiciones = new List<Requisicion>();
                    List<Sam3_ST_Get_ListaRequisiciones_Result> listaRequisicionesCTX = ctx.Sam3_ST_Get_ListaRequisiciones(usuario.UsuarioID, proyectoID, tipoPruebaID, estatusID, lenguaje).ToList();
                    listaRequisiciones.Add(new Requisicion());
                    foreach (Sam3_ST_Get_ListaRequisiciones_Result item in listaRequisicionesCTX)
                    {
                        listaRequisiciones.Add(new Requisicion
                        {
                            RequisicionID = item.RequisicionID,
                            ProyectoID = item.ProyectoID,
                            TipoPruebaID = item.TipoPruebaID.GetValueOrDefault(),
                            NombreRequisicion = item.NombreRequisicion,
                            FechaRequisicion = item.FechaRequisicion.ToString(),
                            Observacion = item.Observaciones,
                            FolioCliente = item.FolioCliente
                        });
                    }

                    return listaRequisiciones;
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


        public object ObtenerListadoProveedores( int proyectoID,int PatioID, int tipoPruebaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Proveedor> listaProveedores = new List<Proveedor>();
                    List<Sam3_ST_Get_Proveedores_Result> listaRequisicionesCTX = ctx.Sam3_ST_Get_Proveedores( proyectoID, tipoPruebaID,PatioID).ToList();
                    listaProveedores.Add(new Proveedor());
                    foreach (Sam3_ST_Get_Proveedores_Result item in listaRequisicionesCTX)
                    {
                        listaProveedores.Add(new Proveedor
                        {
                            ProveedorID = item.ProveedorID,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                            Nombre = item.Proveedor
                        });
                    }

                    return listaProveedores;
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

        public object ObtenerListadoTurnos( int tipoPruebaID, int proveedorID, int equipoID,int proyectoID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<TurnoLaboral> listaProveedores = new List<TurnoLaboral>();
                    List<Sam3_ST_Get_TurnoLaboral_Result> listaRequisicionesCTX = ctx.Sam3_ST_Get_TurnoLaboral(tipoPruebaID, proveedorID,equipoID, lenguaje).ToList();
                    listaProveedores.Add(new TurnoLaboral());
                    foreach (Sam3_ST_Get_TurnoLaboral_Result item in listaRequisicionesCTX)
                    {
                        listaProveedores.Add(new TurnoLaboral
                        {
                            TurnoLaboralID = item.TurnoLaboralID,
                            Nombre = item.Turno,
                            Capacidad = item.Capacidad,
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID,
                            CapacidadTurnoEquipoID = item.CapacidadTurnoEquipoID,
                            CapacidadTurnoProveedorID = item.CapacidadTurnoProveedorID,
                            JuntasAsignadas = item.ElementosAsignados.ToString(),
                            ListaElementosAsignadosTurno = item.ElementosAsignados == 0 ? new List<ElementosRequisicion>() : (List<ElementosRequisicion>)AsignarRequisicionBD.Instance.ObtenerElementosAsignadosTurno(lenguaje, proyectoID, item.CapacidadTurnoEquipoID, item.CapacidadTurnoProveedorID, 0)
                        });
                    }

                    return listaProveedores;
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

        public object ObtenerListadoEquipos( int tipoPruebaID, int proveedorID, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Equipo> listaProveedores = new List<Equipo>();
                    List<Sam3_ST_Get_Equipo_Result> listaRequisicionesCTX = ctx.Sam3_ST_Get_Equipo( tipoPruebaID, proveedorID,lenguaje).ToList();
                    listaProveedores.Add(new Equipo());
                    foreach (Sam3_ST_Get_Equipo_Result item in listaRequisicionesCTX)
                    {
                        listaProveedores.Add(new Equipo
                        {
                            EquipoID = item.EquipoID,
                            Nombre = item.Equipo,
                            ProveedorEquipoID = item.ProveedorEquipoID.GetValueOrDefault(),
                            TipoPruebaProveedorID = item.TipoPruebaProveedorID
                        });
                    }

                    return listaProveedores;
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