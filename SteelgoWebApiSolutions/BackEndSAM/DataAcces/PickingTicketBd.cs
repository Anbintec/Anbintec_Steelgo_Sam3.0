using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using DatabaseManager.Sam2;

namespace BackEndSAM.DataAcces
{
    public class PickingTicketBd
    {
        private static readonly object _mutex = new object();
        private static PickingTicketBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private PickingTicketBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static PickingTicketBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PickingTicketBd();
                    }
                }
                return _instance;
            }
        }

        public object ListadoFoliosParaCombo(int proyectoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<ListaCombos> listado = (from pk in ctx.Sam3_FolioPickingTicket
                                                     join dpk in ctx.Sam3_DetalleFolioPickingTicket on pk.FolioPickingTicketID equals dpk.FolioPickingTicketID
                                                     join d in ctx.Sam3_Despacho on dpk.DespachoID equals d.DespachoID
                                                     where pk.Activo && dpk.Activo && d.Activo
                                                     && d.ProyectoID == proyectoID
                                                     select new ListaCombos
                                                     {
                                                         id = pk.FolioPickingTicketID.ToString(),
                                                         value = pk.OrdenTrabajoSpoolID.ToString()
                                                     }).AsParallel().Distinct().ToList();

                        listado = (from lst in listado
                                   where !(from odtm in ctx2.OrdenTrabajoMaterial
                                          where odtm.OrdenTrabajoSpoolID.ToString() == lst.value
                                          && (!odtm.TieneDespacho || odtm.TieneDespacho == null) 
                                          select odtm).Any()
                                   select lst).ToList();


                        listado = listado.GroupBy(x => x.id).Select(x => x.First()).ToList();

                        foreach (ListaCombos item in listado)
                        {
                            string valor = item.value;
                            item.value = ctx2.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID.ToString() == valor).Select(x => x.NumeroControl).AsParallel().SingleOrDefault();
                        }

                        return listado.OrderBy(x => x.value).ToList();
                    }
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

        public object DetallePickingTicket(int pickingTicketID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DetallePickingTicket resultado = new DetallePickingTicket();

                    resultado = (from en in ctx.Sam3_Entrega
                                 join pk in ctx.Sam3_FolioPickingTicket on en.FolioPickingTicketID equals pk.FolioPickingTicketID
                                 where en.Activo && pk.Activo
                                 && pk.FolioPickingTicketID == pickingTicketID
                                 select new DetallePickingTicket
                                 {
                                     EntregaID = en.EntregaID,
                                     FechaOriginal = en.FechaEntrega,
                                     EmpleadoEntrega = new ListaCombos { id = en.UsuarioEntregaID.ToString(), value = "123" },
                                     EmpleadoRecibe = new ListaCombos { id = en.UsuarioRecibeID.ToString(), value = "123" }
                                 }).AsParallel().Distinct().SingleOrDefault();

                    if (resultado != null)
                    {
                        resultado.FechaEntrega = resultado.FechaOriginal.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        resultado = new DetallePickingTicket();
                    }
                    return resultado;
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

        public object EliminarEntrega(int entregaID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Entrega entrega = ctx.Sam3_Entrega.Where(x => x.EntregaID == entregaID).AsParallel().SingleOrDefault();
                    entrega.Activo = false;
                    entrega.FechaModificacion = DateTime.Now;
                    entrega.UsuarioModificacion = usuario.UsuarioID;
                    ctx.SaveChanges();
                }

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("OK");
                result.ReturnCode = 200;
                result.ReturnStatus = true;
                result.IsAuthenicated = true;

                return result;
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