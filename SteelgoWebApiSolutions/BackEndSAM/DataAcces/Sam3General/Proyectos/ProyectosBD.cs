using BackEndSAM.Models.Sam3General;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackEndSAM.DataAcces.Sam3General.Proyectos
{
    public class ProyectosBD
    {
        private static readonly object _mutex = new object();
        private static ProyectosBD _instance;

        public static ProyectosBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectosBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoProyectos(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Models.Sam3General.Proyectos> listaProyectos = new List<Models.Sam3General.Proyectos>();
                    List<Sam3_Get_ListaProyectos_Result> listaProyectosCTX = ctx.Sam3_Get_ListaProyectos(usuario.UsuarioID).ToList();
                    listaProyectos.Add(new Models.Sam3General.Proyectos());
                    foreach (Sam3_Get_ListaProyectos_Result item in listaProyectosCTX)
                    {
                        listaProyectos.Add(new Models.Sam3General.Proyectos
                        {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre,
                            PatioID = item.PatioID,
                            PrefijoOrdenTrabajo = item.PrefijoOrdenTrabajo
                        });
                    }

                    return listaProyectos;
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

        public object ObtenerListadoProyectosIngeneria(int UsuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Steelgo_Get_ProyectoIngenieria_Result> result = ctx.Sam3_Steelgo_Get_ProyectoIngenieria(UsuarioID).ToList();
                    List<DetalleProyectoIngeneria> listaDetalle = new List<DetalleProyectoIngeneria>();
                    listaDetalle.Add(new DetalleProyectoIngeneria());

                    foreach (Sam3_Steelgo_Get_ProyectoIngenieria_Result item in result)
                    {
                        listaDetalle.Add(new DetalleProyectoIngeneria {
                            ProyectoID = item.ProyectoID,
                            Nombre = item.Nombre,
                            ProyectoSpoolID = item.ProyectoSpoolID,
                            ProyectoSoporteID = item.ProyectoSoporteID
                        });
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

        public object ObtenerIDOrdenTrabajo(Sam3_Usuario usuario, string ordentrabajo, int tipo, string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Spool> listaOrdenTrabajoSpool = null;
                        //(from ordentrabajoSpool in ctx.Sam3_Steelgo_Get_SpoolID(tipo, ordentrabajo, lenguaje)
                        // select new Spool
                        // {
                        //     HabilitadoHoldFecha = ordentrabajoSpool.HabilitadoHoldFecha,
                        //     Nombre = ordentrabajoSpool.ID,
                        //     SpoolID = ordentrabajoSpool.OrdenTrabajoSpoolID,
                        //     Proyecto = ordentrabajoSpool.NombreProyecto,
                        //     ProyectoID = ordentrabajoSpool.ProyectoID,
                        //     Status = ordentrabajoSpool.status,
                        //     OrdenTrabajo = ordentrabajoSpool.OrdenTrabajo
                        // }).AsParallel().ToList().OrderBy(x => x.CedulaTuboCalificadoDesc).ToList<Spool>();
                    listaOrdenTrabajoSpool.Insert(0, new Spool());

                    return listaOrdenTrabajoSpool;

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