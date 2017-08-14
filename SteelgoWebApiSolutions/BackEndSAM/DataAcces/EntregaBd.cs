using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;

namespace BackEndSAM.DataAcces
{
    public class EntregaBd
    {
        private static readonly object _mutex = new object();
        private static EntregaBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private EntregaBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static EntregaBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EntregaBd();
                    }
                }
                return _instance;
            }
        }

        public object InsertarEntrega(List<Entrega> entregas, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        foreach (Entrega entrega in entregas)
                        {
                            DateTime fecha = new DateTime();

                            try
                            {
                                if (!DateTime.TryParse(entrega.FechaEntrega, out fecha))
                                {
                                    var splitDate = entrega.FechaEntrega.Split('/');
                                    var changedFormatDate = splitDate[1] + "/" + splitDate[0] + "/" + splitDate[2];
                                    if (!DateTime.TryParse(changedFormatDate, out fecha))
                                    {
                                        fecha = DateTime.Now;
                                    }
                                }
                            }
                            catch
                            {
                                fecha = DateTime.Now;
                            }
                            
                            if (entrega.EntregaID > 0)
                            {
                                Sam3_Entrega actual = ctx.Sam3_Entrega.Where(x => x.EntregaID == entrega.EntregaID && x.Activo).AsParallel().SingleOrDefault();
                                actual.Activo = true;
                                actual.FechaEntrega = fecha;
                                actual.FechaModificacion = DateTime.Now;
                                actual.UsuarioEntregaID = Convert.ToInt32(entrega.NoEmpleadoEntrega);
                                actual.UsuarioModificacion = usuario.UsuarioID;
                                actual.UsuarioRecibeID = Convert.ToInt32(entrega.NoEmpleadoRecibe);
                                actual.FolioPickingTicketID = Convert.ToInt32(entrega.NoPickingTicket);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                Sam3_Entrega nuevoRegistro = new Sam3_Entrega();
                                nuevoRegistro.Activo = true;
                                nuevoRegistro.FechaEntrega = fecha;
                                nuevoRegistro.FechaModificacion = DateTime.Now;
                                nuevoRegistro.UsuarioEntregaID = Convert.ToInt32(entrega.NoEmpleadoEntrega);
                                nuevoRegistro.UsuarioModificacion = usuario.UsuarioID;
                                nuevoRegistro.UsuarioRecibeID = Convert.ToInt32(entrega.NoEmpleadoRecibe);
                                nuevoRegistro.FolioPickingTicketID = Convert.ToInt32(entrega.NoPickingTicket);
                                ctx.Sam3_Entrega.Add(nuevoRegistro);
                                ctx.SaveChanges();
                            }
                        }
                        ctx_tran.Commit();

                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Ok");
                        result.ReturnCode = 200;
                        result.ReturnStatus = true;
                        result.IsAuthenicated = true;

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ocurrio un error al guardar los datos.");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

    }
}