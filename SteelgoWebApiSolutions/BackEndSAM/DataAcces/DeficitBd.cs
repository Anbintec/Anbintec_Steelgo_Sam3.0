using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces
{
    public class DeficitBd
    {
        private static readonly object _mutex = new object();
        private static DeficitBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DeficitBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static DeficitBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DeficitBd();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Funcion para obtener las ordenes de trabajo para la pantalla
        /// NotificacionDeficit - combo orden de trabajo
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerOrdenesDeTrabajo(Sam3_Usuario usuario)
        {
            try
            {
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                List<ListaCombos> ordenes = new List<ListaCombos>();

                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        proyectos = (from p in ctx.Sam3_Rel_Usuario_Proyecto
                                     join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                     where p.Activo && eqp.Activo
                                     && p.UsuarioID == usuario.UsuarioID
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();

                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam2_PatioID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && proyectos.Contains(p.ProyectoID)
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();

                        ordenes = (from ot in ctx2.OrdenTrabajo
                                   where ot.EstatusOrdenID == 1 &&
                                   proyectos.Contains(ot.ProyectoID)
                                   select new ListaCombos
                                   {
                                       id = ot.OrdenTrabajoID.ToString(),
                                       value = ot.NumeroOrden
                                   }).AsParallel().ToList();
                    }
                    return ordenes;
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

        /// <summary>
        /// Funcion para obtener los item codes que pertenecen a una orden de trabajo
        /// </summary>
        /// <param name="ordenTrabajo"></param>
        /// <returns></returns>
        public object ObtenerItemCodesOT(string ordenTrabajo)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<int> itemCodes = (from ot in ctx2.OrdenTrabajo
                                               join ots in ctx2.OrdenTrabajoSpool on ot.OrdenTrabajoID equals ots.OrdenTrabajoID
                                               join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                               join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                               join ic in ctx2.ItemCode on ms.ItemCodeID equals ic.ItemCodeID
                                               where ot.OrdenTrabajoID.ToString() == ordenTrabajo
                                               select ms.ItemCodeID).AsParallel().ToList();


                        List<ListaCombos> itemCodesSam3 = (from eq in ctx.Sam3_EquivalenciaItemCode
                                                           join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on eq.Sam3_ItemCodeID equals rics.ItemCodeID
                                                           join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                           join ic in ctx.Sam3_ItemCode on eq.Sam3_ItemCodeID equals ic.ItemCodeID
                                                           where itemCodes.Contains(eq.Sam2_ItemCodeID)
                                                           && eq.Activo && rics.Activo && ics.Activo && ic.Activo
                                                           select new ListaCombos
                                                           {
                                                               id = ic.ItemCodeID.ToString(),
                                                               value = ic.Codigo
                                                           }).AsParallel().ToList();

                        return itemCodesSam3;


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

        /// <summary>
        /// Funcion para obtener los diametros de un item code
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public object ObtenerDiametros(int itemCode)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<DiametrosItemCode> diametro = (from rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                        join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                        join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                        join rid in ctx.Sam3_Rel_ItemCode_Diametro on rics.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                        join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                        join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                        join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                        where it.ItemCodeID == itemCode
                                                        && rics.Activo && ics.Activo
                                                        select new DiametrosItemCode
                                                        {
                                                            Diametro1 = d1.Valor.ToString(),
                                                            Diametro2 = d2.Valor.ToString()
                                                        }).AsParallel().ToList();

                    //List<DiametrosItemCode> diametro = (from eq in ctx.Sam3_EquivalenciaItemCode
                    //                                    join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on eq.Sam3_ItemCodeID equals rics.ItemCodeID
                    //                                    join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                    //                                    where eq.Sam2_ItemCodeID == itemCode
                    //                                    && eq.Activo && rics.Activo && ics.Activo
                    //                                    select new DiametrosItemCode
                    //                                    {
                    //                                        Diametro1 = ics.Diametro1.ToString(),
                    //                                        Diametro2 = ics.Diametro2.ToString()
                    //                                    }).AsParallel().ToList();
                    return diametro;
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

        /// <summary>
        /// Obtener la informacion del grid en Notificacion de Deficit
        /// </summary>
        /// <param name="ordenTrabajo"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public object ObtenerInformacionItemCode(string ordenTrabajo, int itemCode)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<Deficit> lista = new List<Deficit>();
                        if (ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.ItemCodeID == itemCode).Any())
                        {
                            lista = (from ic in ctx.Sam3_ItemCode
                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                     join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                     join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                     join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                     join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                     join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                     where ic.ItemCodeID == itemCode
                                     && rics.Activo && ics.Activo && ic.Activo
                                     select new Deficit
                                     {
                                         ItemCodeID = ic.ItemCodeID.ToString(),
                                         ItemCode = ic.Codigo,
                                         Diametro1 = d1.Valor.ToString(),
                                         Diametro2 = d2.Valor.ToString(),
                                         Descripcion = ics.DescripcionEspanol,
                                         DeficitTotal = (from dm in ctx.Sam3_DeficitMateriales
                                                         where dm.Activo
                                                         && dm.OrdenTrabajoID.ToString() == ordenTrabajo
                                                         && dm.ItemCodeID == ic.ItemCodeID
                                                         select dm.Deficit).FirstOrDefault().ToString()
                                     }).AsParallel().ToList();

                            //lista = (from eq in ctx.Sam3_EquivalenciaItemCode
                            //         join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on eq.Sam3_ItemCodeID equals rics.ItemCodeID
                            //         join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                            //         join ic in ctx.Sam3_ItemCode on eq.Sam3_ItemCodeID equals ic.ItemCodeID
                            //         where eq.Sam2_ItemCodeID == itemCode
                            //         && eq.Activo && rics.Activo && ics.Activo && ic.Activo
                            //         select new Deficit
                            //         {
                            //             ItemCodeID = ic.ItemCodeID.ToString(),
                            //             ItemCode = ic.Codigo,
                            //             Diametro1 = ics.Diametro1.ToString(),
                            //             Diametro2 = ics.Diametro2.ToString(),
                            //             Descripcion = ics.DescripcionEspanol,
                            //             DeficitTotal = (from dm in ctx.Sam3_DeficitMateriales
                            //                             where dm.Activo
                            //                             && dm.OrdenTrabajoID.ToString() == ordenTrabajo
                            //                             && dm.ItemCodeID == ic.ItemCodeID
                            //                             select dm.Deficit).FirstOrDefault().ToString()
                            //         }).AsParallel().ToList();
                        }
                        else
                        {
                            lista = (from ic in ctx.Sam3_ItemCode
                                     join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                     join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                     join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                     join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                     join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                     join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                     where ic.ItemCodeID == itemCode
                                     && rics.Activo && ics.Activo && ic.Activo
                                     select new Deficit
                                     {
                                         ItemCodeID = ic.ItemCodeID.ToString(),
                                         ItemCode = ic.Codigo,
                                         Diametro1 = d1.Valor.ToString(),
                                         Diametro2 = d2.Valor.ToString(),
                                         Descripcion = ics.DescripcionEspanol,
                                         DeficitTotal = "0"
                                     }).AsParallel().ToList();
                        }

                        lista.ForEach(x =>
                        {
                            int icSam2 = (from eq in ctx.Sam3_EquivalenciaItemCode
                                          where eq.Sam3_ItemCodeID.ToString() == x.ItemCodeID && eq.Activo
                                          select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault();

                            x.Cantidad = (from ot in ctx2.OrdenTrabajo
                                          join ots in ctx2.OrdenTrabajoSpool on ot.OrdenTrabajoID equals ots.OrdenTrabajoID
                                          join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                          join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                          join itmc in ctx2.ItemCode on ms.ItemCodeID equals itmc.ItemCodeID
                                          where ot.OrdenTrabajoID.ToString() == ordenTrabajo && ms.ItemCodeID == icSam2
                                          select ms.Cantidad).Sum().ToString();
                        });

                        return lista;
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

        /// <summary>
        /// Funcion para obtener la informacion del grid de spools
        /// Notificacion de deficit
        /// </summary>
        /// <param name="ordenTrabajo"></param>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public object ObtenerInformacionSpools(int ordenTrabajo, int itemCode)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<SpoolsDeficit> lista = new List<SpoolsDeficit>();
                        List<int?> spoolsGuardados = new List<int?>();

                        if (ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.ItemCodeID == itemCode).Any())
                        {
                            spoolsGuardados = (from dm in ctx.Sam3_DeficitMateriales
                                               where dm.Activo
                                               && dm.OrdenTrabajoID == ordenTrabajo
                                               && dm.ItemCodeID == itemCode
                                               select dm.SpoolID).AsParallel().ToList();
                        }

                        int icSam2 = (from eq in ctx.Sam3_EquivalenciaItemCode
                                      where eq.Sam3_ItemCodeID == itemCode && eq.Activo
                                      select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault();

                        lista = (from ots in ctx2.OrdenTrabajoSpool
                                 join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                 join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                 join s in ctx2.Spool on ms.SpoolID equals s.SpoolID
                                 where ots.OrdenTrabajoID == ordenTrabajo && ms.ItemCodeID == icSam2
                                 select new SpoolsDeficit
                                 {
                                     SpoolID = s.SpoolID.ToString(),
                                     Spool = s.Nombre,
                                     Prioridad = s.Prioridad.ToString(),
                                     Peqs = s.PeqGrupo,
                                     Peso = s.Peso.ToString(),
                                     ItemCodeID = ms.ItemCodeID.ToString(),
                                     Seleccionado = spoolsGuardados.Contains(s.SpoolID)
                                 }).AsParallel().GroupBy(x => x.SpoolID).Select(x => x.First()).ToList();


                        lista.ForEach(x =>
                        {
                            x.ItemCodes = (from ic in ctx.Sam3_ItemCode
                                           join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                           join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                           join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                           join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                           join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                           join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID 
                                           where ic.ItemCodeID == itemCode
                                           && rics.Activo && ics.Activo && ic.Activo
                                           select new Deficit
                                           {
                                               ItemCode = ic.Codigo,
                                               ItemCodeID = ic.ItemCodeID.ToString(),
                                               Diametro1 = d1.Valor.ToString(),
                                               Diametro2 = d2.Valor.ToString(),
                                               Descripcion = ics.DescripcionEspanol
                                           }).AsParallel().ToList();
                        });

                        lista.ForEach(x =>
                        {
                            x.ItemCodes.ForEach(y =>
                            {
                                y.Cantidad = (from ms in ctx2.MaterialSpool
                                              where ms.SpoolID.ToString() == x.SpoolID && ms.ItemCodeID == icSam2
                                              select ms.Cantidad).Sum().ToString();
                            });
                        });

                        return lista;
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

        public object GuardarDeficit(int ordenTrabajoID, List<string> spoolID, List<DatosDeficitItemCodes> deficit, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int cantidadDeficit = deficit[0].DeficitTotal;
                    int IDItemCode = deficit[0].ItemCodeID;

                    //int itemCodeSam3 = (from eq in ctx.Sam3_EquivalenciaItemCode
                    //                    where eq.Activo && eq.Sam2_ItemCodeID == IDItemCode
                    //                    select eq.Sam3_ItemCodeID).AsParallel().SingleOrDefault();

                    if (ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.ItemCodeID == IDItemCode).Any())
                    {
                        if (spoolID.Count > 0)
                        {
                            foreach (string item in spoolID)
                            {
                                int IDspool = Int32.Parse(item);

                                List<Sam3_DeficitMateriales> nuevoDeficit = ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.ItemCodeID == IDItemCode).AsParallel().ToList();

                                foreach (Sam3_DeficitMateriales materiales in nuevoDeficit)
                                {
                                    //Sam3_DeficitMateriales actualizarDeficit = new Sam3_DeficitMateriales();
                                    materiales.OrdenTrabajoID = ordenTrabajoID;
                                    materiales.ItemCodeID = IDItemCode;
                                    materiales.SpoolID = IDspool;
                                    materiales.Deficit = cantidadDeficit;
                                    materiales.Activo = true;
                                    materiales.UsuarioModificacion = usuario.UsuarioID;
                                    materiales.FechaModificacion = DateTime.Now;
                                    materiales.Solucionado = false;
                                    //ctx.Sam3_DeficitMateriales.Add(nuevoDeficit);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                        else
                        {
                            //elimino los que ya existian
                            List<Sam3_DeficitMateriales> nuevoDeficit = ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.ItemCodeID == IDItemCode).AsParallel().ToList();

                            foreach (Sam3_DeficitMateriales materiales in nuevoDeficit)
                            {
                                //Sam3_DeficitMateriales actualizarDeficit = new Sam3_DeficitMateriales();
                                //actualizarDeficit.OrdenTrabajoID = materiales.OrdenTrabajoID;
                                //actualizarDeficit.ItemCodeID = materiales.ItemCodeID;
                                //actualizarDeficit.SpoolID = IDspool;
                                //actualizarDeficit.Deficit = cantidadDeficit;
                                materiales.Activo = false;
                                materiales.UsuarioModificacion = usuario.UsuarioID;
                                materiales.FechaModificacion = DateTime.Now;

                                //ctx.Sam3_DeficitMateriales.Add(nuevoDeficit);
                                ctx.SaveChanges();
                            }

                            Sam3_DeficitMateriales nuevo = new Sam3_DeficitMateriales();
                            nuevo.OrdenTrabajoID = ordenTrabajoID;
                            nuevo.ItemCodeID = IDItemCode;
                            nuevo.SpoolID = null;
                            nuevo.Deficit = cantidadDeficit;
                            nuevo.Activo = true;
                            nuevo.UsuarioModificacion = usuario.UsuarioID;
                            nuevo.FechaModificacion = DateTime.Now;
                            nuevo.Solucionado = false;

                            ctx.Sam3_DeficitMateriales.Add(nuevo);
                            ctx.SaveChanges();
                        }

                        if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(10,
                               string.Format("Se ha actualizado la notificacion de deficit de la orden de trabajo {0} con fecha {1}",
                               ordenTrabajoID, DateTime.Now), usuario))
                        {
                            //Agregar error a la bitacora  PENDIENTE
                        }
                    }
                    else
                    {
                        if (spoolID.Count > 0)
                        {
                            foreach (string item in spoolID)
                            {
                                int IDspool = Int32.Parse(item);

                                Sam3_DeficitMateriales nuevoDeficit = new Sam3_DeficitMateriales();
                                nuevoDeficit.OrdenTrabajoID = ordenTrabajoID;
                                nuevoDeficit.ItemCodeID = IDItemCode;
                                nuevoDeficit.SpoolID = IDspool;
                                nuevoDeficit.Deficit = cantidadDeficit;
                                nuevoDeficit.Activo = true;
                                nuevoDeficit.UsuarioModificacion = usuario.UsuarioID;
                                nuevoDeficit.FechaModificacion = DateTime.Now;
                                nuevoDeficit.Solucionado = false;

                                ctx.Sam3_DeficitMateriales.Add(nuevoDeficit);
                                ctx.SaveChanges();
                            }
                        }
                        else
                        {
                            Sam3_DeficitMateriales nuevoDeficit = new Sam3_DeficitMateriales();
                            nuevoDeficit.OrdenTrabajoID = ordenTrabajoID;
                            nuevoDeficit.ItemCodeID = IDItemCode;
                            nuevoDeficit.SpoolID = null;
                            nuevoDeficit.Deficit = cantidadDeficit;
                            nuevoDeficit.Activo = true;
                            nuevoDeficit.UsuarioModificacion = usuario.UsuarioID;
                            nuevoDeficit.FechaModificacion = DateTime.Now;
                            nuevoDeficit.Solucionado = false;

                            ctx.Sam3_DeficitMateriales.Add(nuevoDeficit);
                            ctx.SaveChanges();
                        }

                        if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(10,
                               string.Format("La orden de trabajo {0} tiene una nueva notificacion de deficit con fecha {1}",
                               ordenTrabajoID, DateTime.Now), usuario))
                        {
                            //Agregar error a la bitacora  PENDIENTE
                        }
                    }

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
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

        ////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Funcion para obtener los detalles de un deficit
        /// pantalla revision de deficit
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public object ObtenerGridDeficit(int ordenTrabajoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<RevisionDeficitDatos> lista = (from dm in ctx.Sam3_DeficitMateriales
                                                            where dm.Activo && dm.OrdenTrabajoID == ordenTrabajoID && !dm.Solucionado
                                                            select new RevisionDeficitDatos
                                                            {
                                                                //SpoolID = dm.SpoolID.ToString(),
                                                                ItemCodeID = dm.ItemCodeID.ToString(),
                                                                ItemCode = (from ic in ctx.Sam3_ItemCode
                                                                            join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                                                            where ic.ItemCodeID == dm.ItemCodeID && ic.Activo
                                                                            select ic.Codigo).FirstOrDefault(),
                                                                Diametro1 = (from rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                                             join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                                             join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                                             join rid in ctx.Sam3_Rel_ItemCode_Diametro on rics.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                                             join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                                             where rics.Activo 
                                                                             && ics.Activo 
                                                                             && rid.ItemCodeID == dm.ItemCodeID
                                                                             select d1.Valor).FirstOrDefault().ToString(),
                                                                Diametro2 = (from rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                                             join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                                             join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                                             join rid in ctx.Sam3_Rel_ItemCode_Diametro on rics.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                                             join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                                             where rics.Activo
                                                                             && ics.Activo
                                                                             && rid.ItemCodeID == dm.ItemCodeID
                                                                             select d2.Valor).FirstOrDefault().ToString(),
                                                                Descripcion = (from rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                                                               join ics in ctx.Sam3_ItemCodeSteelgo on rics.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                                               where rics.Activo && ics.Activo && rics.ItemCodeID == dm.ItemCodeID
                                                                               select ics.DescripcionEspanol).FirstOrDefault().ToString(),
                                                                Deficit = dm.Deficit.ToString(),
                                                                DeficitID = dm.DeficitID.ToString()
                                                            }).AsParallel().ToList();

                        lista.ForEach(x =>
                        {
                            int icSam2 = (from eq in ctx.Sam3_EquivalenciaItemCode
                                          where eq.Sam3_ItemCodeID.ToString() == x.ItemCodeID && eq.Activo
                                          select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault();

                            x.Cantidad = (from ot in ctx2.OrdenTrabajo
                                          join ots in ctx2.OrdenTrabajoSpool on ot.OrdenTrabajoID equals ots.OrdenTrabajoID
                                          join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                          join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                          join itmc in ctx2.ItemCode on ms.ItemCodeID equals itmc.ItemCodeID
                                          where ot.OrdenTrabajoID == ordenTrabajoID && ms.ItemCodeID == icSam2
                                          select ms.Cantidad).Sum().ToString();
                        });

                        return lista;
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

        /// <summary>
        /// Funcion para obtener el grid de Spools
        /// Pantalla Revision de deficit
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <returns></returns>
        public object obtenerSpoolsRevision(string ordenTrabajoID)
        {
            try
            {
                List<SpoolsDeficit> totalSpools = new List<SpoolsDeficit>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<int> listaItemCodesSam3 = (from dm in ctx.Sam3_DeficitMateriales
                                                        where dm.OrdenTrabajoID.ToString() == ordenTrabajoID && dm.Activo & !dm.Solucionado
                                                        select dm.ItemCodeID).AsParallel().ToList();

                        List<int> listaItemCodesSam2 = new List<int>();

                        List<int?> spoolsGuardados = new List<int?>();

                        if (ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.OrdenTrabajoID.ToString() == ordenTrabajoID).Any())
                        {
                            spoolsGuardados = (from dm in ctx.Sam3_DeficitMateriales
                                               where dm.Activo
                                               && dm.OrdenTrabajoID.ToString() == ordenTrabajoID
                                               select dm.SpoolID).AsParallel().ToList();
                        }

                        listaItemCodesSam3.ForEach(x =>
                        {
                            listaItemCodesSam2.Add((from eq in ctx.Sam3_EquivalenciaItemCode
                                                    where eq.Sam3_ItemCodeID == x && eq.Activo
                                                    select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault());
                        });

                        foreach (int item in listaItemCodesSam2)
                        {
                            List<SpoolsDeficit> lista = (from ots in ctx2.OrdenTrabajoSpool
                                                         join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                                         join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                                         join s in ctx2.Spool on ms.SpoolID equals s.SpoolID
                                                         where ots.OrdenTrabajoID.ToString() == ordenTrabajoID && ms.ItemCodeID == item
                                                         select new SpoolsDeficit
                                                         {
                                                             SpoolID = s.SpoolID.ToString(),
                                                             Spool = s.Nombre,
                                                             Prioridad = s.Prioridad.ToString(),
                                                             Peqs = s.PeqGrupo,
                                                             Peso = s.Peso.ToString(),
                                                             ItemCodeID = ms.ItemCodeID.ToString(),
                                                             Seleccionado = spoolsGuardados.Contains(s.SpoolID)
                                                         }).AsParallel().GroupBy(x => x.SpoolID).Select(x => x.First()).ToList();

                            lista.ForEach(x =>
                            {
                                x.ItemCodes = (from eq in ctx.Sam3_EquivalenciaItemCode
                                               join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on eq.Sam3_ItemCodeID equals rics.ItemCodeID
                                               join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                               join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                               join rid in ctx.Sam3_Rel_ItemCode_Diametro on rics.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                               join ic in ctx.Sam3_ItemCode on rid.ItemCodeID equals ic.ItemCodeID
                                               join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                               join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                               //on eq.Sam3_ItemCodeID equals ic.ItemCodeID
                                               where eq.Sam2_ItemCodeID.ToString() == x.ItemCodeID
                                               && eq.Activo && rics.Activo && ics.Activo && ic.Activo
                                               select new Deficit
                                               {
                                                   ItemCode = ic.Codigo,
                                                   ItemCodeID = ic.ItemCodeID.ToString(),
                                                   Diametro1 = d1.Valor.ToString(),
                                                   Diametro2 = d2.Valor.ToString(),
                                                   Descripcion = ics.DescripcionEspanol
                                               }).AsParallel().ToList();
                            });

                            lista.ForEach(x =>
                            {
                                x.ItemCodes.ForEach(y =>
                                {
                                    y.Cantidad = (from ms in ctx2.MaterialSpool
                                                  where ms.SpoolID.ToString() == x.SpoolID && ms.ItemCodeID == item
                                                  select ms.Cantidad).Sum().ToString();
                                });
                            });

                            totalSpools.AddRange(lista);
                        }

                        return totalSpools;
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

        /// <summary>
        /// Funcion para eliminar un deficit
        /// Pantalla revision de deficit
        /// </summary>
        /// <param name="deficitID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDeficit(int deficitID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_DeficitMateriales deficit = ctx.Sam3_DeficitMateriales.Where(x => x.Activo && x.DeficitID == deficitID).AsParallel().SingleOrDefault();
                    deficit.Activo = false;
                    deficit.FechaModificacion = DateTime.Now;
                    deficit.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 200;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;

                    return result;
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

        /// <summary>
        /// Funcion para solucionar un deficit de material
        /// pantalla revision de deficit
        /// </summary>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="datos"></param>
        /// <param name="spoolsID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object SolucionarDeficit(int ordenTrabajoID, List<SolucionarRevision> datos, List<SpoolsSeleccionados> spoolsID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var sam2_tran = ctx2.Database.BeginTransaction())
                            {

                                foreach (SolucionarRevision item in datos)
                                {
                                    //Guardo en tabla de deficit
                                    Sam3_DeficitMateriales nuevoDeficit = ctx.Sam3_DeficitMateriales.Where(x => x.DeficitID == item.DeficitID).AsParallel().SingleOrDefault();
                                    nuevoDeficit.OrdenTrabajoID = ordenTrabajoID;
                                    nuevoDeficit.ItemCodeID = nuevoDeficit.ItemCodeID;
                                    nuevoDeficit.SpoolID = spoolsID.Where(x => x.ItemCodeID.Contains(nuevoDeficit.ItemCodeID)).Select(z => z.Spool).AsParallel().SingleOrDefault() == 0 ? (int?)null :
                                        spoolsID.Where(x => x.ItemCodeID.Contains(nuevoDeficit.ItemCodeID)).Select(z => z.Spool).AsParallel().SingleOrDefault();
                                    nuevoDeficit.Deficit = item.Deficit;
                                    nuevoDeficit.Activo = true;
                                    nuevoDeficit.FechaModificacion = DateTime.Now;
                                    nuevoDeficit.UsuarioModificacion = usuario.UsuarioID;
                                    nuevoDeficit.Solucionado = true;
                                    ctx.SaveChanges();
                                }

                                foreach (SpoolsSeleccionados spool in spoolsID)
                                {
                                    //Genero registro de Hold
                                    SpoolHold hold = new SpoolHold();
                                    hold.SpoolID = spool.Spool;
                                    hold.TieneHoldIngenieria = true;
                                    hold.TieneHoldCalidad = false;
                                    hold.Confinado = false;
                                    hold.FechaModificacion = DateTime.Now;

                                    ctx2.SpoolHold.Add(hold);
                                    ctx2.SaveChanges();

                                    //Genero hold historial
                                    SpoolHoldHistorial historial = new SpoolHoldHistorial();
                                    historial.SpoolID = spool.Spool;
                                    historial.TipoHold = "ING";
                                    historial.FechaHold = DateTime.Now;
                                    historial.Observaciones = "Deficit de Material";
                                    historial.FechaModificacion = DateTime.Now;

                                    ctx2.SpoolHoldHistorial.Add(historial);
                                    ctx2.SaveChanges();

                                    int ordenTrabajoSpool = (from ots in ctx2.OrdenTrabajoSpool
                                                             where ots.OrdenTrabajoID == ordenTrabajoID && ots.SpoolID == spool.Spool
                                                             select ots.OrdenTrabajoSpoolID).AsParallel().SingleOrDefault();

                                    //Obtengo datos para Orden trabajo Material
                                    List<DeficitTrabajoMaterial> ordenTrabajoMaterial = new List<DeficitTrabajoMaterial>();
                                    ordenTrabajoMaterial = (from orden in ctx2.OrdenTrabajoMaterial
                                                            where orden.OrdenTrabajoSpoolID == ordenTrabajoSpool
                                                            select new DeficitTrabajoMaterial
                                                            {
                                                                OrdenTrabajoMaterialID = orden.OrdenTrabajoMaterialID,
                                                                OrdenTrabajoSpoolID = orden.OrdenTrabajoSpoolID,
                                                                NumeroUnicoID = orden.NumeroUnicoCongeladoID,
                                                                Segmento = orden.SegmentoCongelado,
                                                                CantidadCongelada = orden.CantidadCongelada
                                                            }).AsParallel().ToList();

                                    foreach (DeficitTrabajoMaterial otm in ordenTrabajoMaterial)
                                    {
                                        //Inventario numero unico inventario
                                        NumeroUnicoInventario inventario = ctx2.NumeroUnicoInventario.Where(c => c.NumeroUnicoID == otm.NumeroUnicoID).AsParallel().SingleOrDefault();
                                        inventario.InventarioCongelado = 0;
                                        inventario.InventarioDisponibleCruce = inventario.InventarioDisponibleCruce + otm.CantidadCongelada == null ? 0 : (int)otm.CantidadCongelada;

                                        ctx2.SaveChanges();

                                        if (!String.IsNullOrEmpty(otm.Segmento))
                                        {
                                            //inventario de Numero unico Segmento
                                            NumeroUnicoSegmento segmento = ctx2.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == otm.NumeroUnicoID && x.Segmento == otm.Segmento).AsParallel().SingleOrDefault();
                                            segmento.InventarioCongelado = 0;
                                            segmento.InventarioDisponibleCruce = inventario.InventarioDisponibleCruce + otm.CantidadCongelada == null ? 0 : (int)otm.CantidadCongelada;

                                            ctx2.SaveChanges();
                                        }

                                        //Elimino orden trabajo material
                                        OrdenTrabajoMaterial OrdenTrabajoAEliminar = ctx2.OrdenTrabajoMaterial.Where(x => x.OrdenTrabajoMaterialID == otm.OrdenTrabajoMaterialID).AsParallel().SingleOrDefault();
                                        ctx2.OrdenTrabajoMaterial.Remove(OrdenTrabajoAEliminar);
                                        ctx2.SaveChanges();
                                    }

                                    //Elimino orden trabajo spool
                                    OrdenTrabajoSpool ordenSpoolAEliminar = ctx2.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpool).AsParallel().SingleOrDefault();
                                    ctx2.OrdenTrabajoSpool.Remove(ordenSpoolAEliminar);
                                    ctx2.SaveChanges();

                                    ////Eliminar congelados de orden de trabajo material
                                    //List<int> listaMaterialSpool = new List<int>();
                                    //List<int> listaItemCodesSam2 = new List<int>();

                                    //foreach (int itemcodes in spool.ItemCodeID)
                                    //{
                                    //    listaItemCodesSam2.Add((from eq in ctx.Sam3_EquivalenciaItemCode
                                    //                            where eq.Sam3_ItemCodeID == itemcodes && eq.Activo
                                    //                            select eq.Sam2_ItemCodeID).AsParallel().SingleOrDefault());
                                    //}

                                    //foreach (int ic in listaItemCodesSam2)
                                    //{
                                    //    listaMaterialSpool = (from ms in ctx2.MaterialSpool
                                    //                          where ms.SpoolID == spool.Spool && ms.ItemCodeID == ic
                                    //                          select ms.MaterialSpoolID).AsParallel().ToList();
                                    //}

                                    //int ordenTrabajoSpool = (from ots in ctx2.OrdenTrabajoSpool
                                    //                         where ots.OrdenTrabajoID == ordenTrabajoID && ots.SpoolID == spool.Spool
                                    //                         select ots.OrdenTrabajoSpoolID).AsParallel().SingleOrDefault();

                                    //foreach (int matSpool in listaMaterialSpool)
                                    //{
                                    //    //Obtengo datos para Orden trabajo Material
                                    //    DeficitTrabajoMaterial ordenTrabajoMaterial = new DeficitTrabajoMaterial();
                                    //    ordenTrabajoMaterial = (from orden in ctx2.OrdenTrabajoMaterial
                                    //                            where orden.OrdenTrabajoSpoolID == ordenTrabajoSpool && orden.MaterialSpoolID == matSpool
                                    //                            select new DeficitTrabajoMaterial
                                    //                            {
                                    //                                OrdenTrabajoMaterialID = orden.OrdenTrabajoMaterialID,
                                    //                                OrdenTrabajoSpoolID = orden.OrdenTrabajoSpoolID,
                                    //                                NumeroUnicoID = orden.NumeroUnicoCongeladoID,
                                    //                                Segmento = orden.SegmentoCongelado,
                                    //                                CantidadCongelada = orden.CantidadCongelada
                                    //                            }).AsParallel().SingleOrDefault();

                                    //    //Inventario numero unico inventario
                                    //    NumeroUnicoInventario inventario = ctx2.NumeroUnicoInventario.Where(c => c.NumeroUnicoID == ordenTrabajoMaterial.NumeroUnicoID).AsParallel().SingleOrDefault();
                                    //    inventario.InventarioCongelado = 0;
                                    //    inventario.InventarioDisponibleCruce = inventario.InventarioDisponibleCruce + ordenTrabajoMaterial.CantidadCongelada == null ? 0 : (int)ordenTrabajoMaterial.CantidadCongelada;

                                    //    ctx2.SaveChanges();

                                    //    if (!String.IsNullOrEmpty(ordenTrabajoMaterial.Segmento))
                                    //    {
                                    //        //inventario de Numero unico Segmento
                                    //        NumeroUnicoSegmento segmento = ctx2.NumeroUnicoSegmento.Where(x => x.NumeroUnicoID == ordenTrabajoMaterial.NumeroUnicoID && x.Segmento == ordenTrabajoMaterial.Segmento).AsParallel().SingleOrDefault();
                                    //        segmento.InventarioCongelado = 0;
                                    //        segmento.InventarioDisponibleCruce = inventario.InventarioDisponibleCruce + ordenTrabajoMaterial.CantidadCongelada == null ? 0 : (int)ordenTrabajoMaterial.CantidadCongelada;

                                    //        ctx2.SaveChanges();
                                    //    }

                                    //    //Elimino orden trabajo material
                                    //    OrdenTrabajoMaterial OrdenTrabajoAEliminar = ctx2.OrdenTrabajoMaterial.Where(x => x.OrdenTrabajoMaterialID == ordenTrabajoMaterial.OrdenTrabajoMaterialID).AsParallel().SingleOrDefault();
                                    //    ctx2.OrdenTrabajoMaterial.Remove(OrdenTrabajoAEliminar);
                                    //    ctx2.SaveChanges();

                                        
                                    //    //Elimino orden trabajo spool
                                    //    //DatabaseManager.Sam2.OrdenTrabajoSpool ordenSpoolAEliminar = ctx2.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoMaterial.OrdenTrabajoSpoolID).AsParallel().SingleOrDefault();
                                    //    //ctx2.OrdenTrabajoSpool.Remove(ordenSpoolAEliminar);
                                    //    //ctx2.SaveChanges();
                                    //}
                                }
                                sam2_tran.Commit();
                            } // tran sam2
                        } //using ctx2
                        sam3_tran.Commit();
                    }// tran sam3
                }// using ctx

                TransactionalInformation result = new TransactionalInformation();
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