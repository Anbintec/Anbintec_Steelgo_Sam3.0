using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using DatabaseManager.EntidadesPersonalizadas;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Mvc;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Net.Http.Headers;
using System.Transactions;

namespace BackEndSAM.DataAcces
{
    /// <summary>
    /// Clase que contiene funciones relacionadas con los avisos de Llegada 
    /// </summary>
    public class AlmacenajeBd
    {
        private static readonly object _mutex = new object();
        private static AlmacenajeBd _instance;


        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private AlmacenajeBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static AlmacenajeBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new AlmacenajeBd();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// Obtiene las ordenes de almacenaje de un proyecto seleccionado
        /// </summary>
        /// <param name="avisoLlegadaID"></param>
        /// <returns>Regresa un JSON object con las propiedades de la orden de almacenaje</returns>
        public object ObtenerOrdenesAlmacenaje(int ProyectoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Almacenaje lstAlmacenaje = new Almacenaje();

                    //Patios y proyectos del usuario
                    List<int> proyectos = ctx.Sam3_Rel_Usuario_Proyecto.Where(x => x.UsuarioID == usuario.UsuarioID).Select(x => x.ProyectoID).AsParallel().ToList();

                    List<int> patios = (from r in ctx.Sam3_Proyecto
                                        join p in ctx.Sam3_Patio on r.PatioID equals p.PatioID
                                        where r.Activo && proyectos.Contains(r.ProyectoID)
                                        select p.PatioID).AsParallel().GroupBy(x => x).Select(x => x.First()).ToList();

                    List<ListaCombos> ordenesAlmacenaje = (from oa in ctx.Sam3_OrdenAlmacenaje
                                                           join roa in ctx.Sam3_Rel_OrdenAlmacenaje_NumeroUnico on oa.OrdenAlmacenajeID equals roa.OrdenAlmacenajeID
                                                           join nu in ctx.Sam3_NumeroUnico on roa.NumeroUnicoID equals nu.NumeroUnicoID
                                                           join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                                           join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                           where oa.Activo && roa.Activo && nu.Activo && p.Activo && pa.Activo
                                                           && nu.ProyectoID == ProyectoID
                                                           && proyectos.Contains(p.ProyectoID)
                                                           && patios.Contains(pa.PatioID)
                                                           && (nu.EstatusDocumental != null && nu.EstatusFisico != null)
                                                           select new ListaCombos
                                                           {
                                                               id = oa.Folio.ToString(),
                                                               value = oa.Folio.ToString()
                                                           }).AsParallel().GroupBy(x => x.id).Select(x => x.First()).ToList();

                    return ordenesAlmacenaje.OrderBy(x => x.value).ToList();
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

        public object ObtenerItemCodesOrdenesAlmacenaje(string Proyecto, string ordenAlmacenaje, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Almacenaje lstAlmacenaje = new Almacenaje();
                    List<ListaCombos> ordenesAlmacenaje = new List<ListaCombos>();
                    int ProyectoID = Proyecto != "" ? Convert.ToInt32(Proyecto) : 0;
                    int folio = ordenAlmacenaje != "" ? Convert.ToInt32(ordenAlmacenaje) : 0;
                    //Patios y proyectos del usuario
                    List<int> proyectos = ctx.Sam3_Rel_Usuario_Proyecto.Where(x => x.UsuarioID == usuario.UsuarioID).Select(x => x.ProyectoID).AsParallel().ToList();

                    List<int> patios = (from r in ctx.Sam3_Proyecto
                                        join p in ctx.Sam3_Patio on r.PatioID equals p.PatioID
                                        where r.Activo && proyectos.Contains(r.ProyectoID)
                                        select p.PatioID).AsParallel().GroupBy(x => x).Select(x => x.First()).ToList();


                    List<Sam3_Rel_OrdenAlmacenaje_NumeroUnico> lstRelONU = (from oa in ctx.Sam3_OrdenAlmacenaje
                                                                            join roa in ctx.Sam3_Rel_OrdenAlmacenaje_NumeroUnico on oa.OrdenAlmacenajeID equals roa.OrdenAlmacenajeID
                                                                            join nu in ctx.Sam3_NumeroUnico on roa.NumeroUnicoID equals nu.NumeroUnicoID
                                                                            join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                                                            join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                                            where oa.Activo && roa.Activo && nu.Activo && p.Activo && pa.Activo
                                                                            && oa.Folio == folio
                                                                            && proyectos.Contains(p.ProyectoID)
                                                                            && patios.Contains(pa.PatioID)
                                                                            && p.ProyectoID == ProyectoID
                                                                            && (nu.EstatusDocumental != null && nu.EstatusFisico != null)
                                                                            select roa).AsParallel().ToList();

                    List<Sam3_Rel_OrdenAlmacenaje_NumeroUnico> agruparNumerosUnicos = lstRelONU.GroupBy(x => x.NumeroUnicoID).Select(x => x.First()).OrderBy(x => x.NumeroUnicoID).AsParallel().ToList();


                    List<ListaCombos> lstItemCode = (from oa in ctx.Sam3_OrdenAlmacenaje
                                                     join roa in ctx.Sam3_Rel_OrdenAlmacenaje_NumeroUnico on oa.OrdenAlmacenajeID equals roa.OrdenAlmacenajeID
                                                     join nu in ctx.Sam3_NumeroUnico on roa.NumeroUnicoID equals nu.NumeroUnicoID
                                                     join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                     join ic in ctx.Sam3_ItemCode on nu.ItemCodeID equals ic.ItemCodeID
                                                     where oa.Activo && roa.Activo && nu.Activo && p.Activo && pa.Activo
                                                     && oa.Folio == folio
                                                     && proyectos.Contains(p.ProyectoID)
                                                     && patios.Contains(pa.PatioID)
                                                     && p.ProyectoID == ProyectoID
                                                     && (nu.EstatusDocumental != null && nu.EstatusFisico != null)
                                                     select new ListaCombos
                                                     {
                                                         id = ic.ItemCodeID.ToString(),
                                                         value = ic.Codigo
                                                     }).AsParallel().ToList();



                    List<ListaCombos> agruparItemCodes = lstItemCode.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).AsParallel().ToList();

                    List<ListadoAlmacenaje> ListadoAlmacenaje = (from oa in ctx.Sam3_OrdenAlmacenaje
                                                                 join roa in ctx.Sam3_Rel_OrdenAlmacenaje_NumeroUnico on oa.OrdenAlmacenajeID equals roa.OrdenAlmacenajeID
                                                                 join nu in ctx.Sam3_NumeroUnico on roa.NumeroUnicoID equals nu.NumeroUnicoID
                                                                 join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                                                 join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                                                 join ic in ctx.Sam3_ItemCode on nu.ItemCodeID equals ic.ItemCodeID
                                                                 join rack in ctx.Sam3_Rack on nu.RackID equals rack.RackID into gj
                                                                 from x in gj.DefaultIfEmpty()//para hacer un left join.
                                                                 where oa.Activo && roa.Activo && nu.Activo && p.Activo && pa.Activo
                                                                 && oa.Folio == folio
                                                                 && proyectos.Contains(p.ProyectoID)
                                                                 && patios.Contains(pa.PatioID)
                                                                 && p.ProyectoID == ProyectoID
                                                                 && (nu.EstatusDocumental != null && nu.EstatusFisico != null)
                                                                 select new ListadoAlmacenaje
                                                                 {
                                                                     ItemCodeID = nu.ItemCodeID.ToString(),
                                                                     NumeroUnicoID = roa.NumeroUnicoID.ToString(),
                                                                     NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                                                                     RackID = nu.RackID,
                                                                     Rack = x.Rack == null ? string.Empty : x.Rack
                                                                 }).AsParallel().ToList();

                    //List<ListadoAlmacenaje> ListadoAlmacenaje = (from roa in agruparNumerosUnicos
                    //                                             join nu in ctx.Sam3_NumeroUnico on roa.NumeroUnicoID equals nu.NumeroUnicoID
                    //                                             where nu.ProyectoID==ProyectoID
                    //                                             select new ListadoAlmacenaje
                    //                                             {
                    //                                                 ItemCodeID = nu.ItemCodeID.ToString(),
                    //                                                 NumeroUnicoID = roa.NumeroUnicoID.ToString(),
                    //                                                 NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo,
                    //                                                 Rack = nu.Rack == null ? string.Empty : nu.Rack
                    //                                             }).AsParallel().ToList();



                    foreach (var item in ListadoAlmacenaje)
                    {
                        int numeroDigitos = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == ProyectoID)
                            .Select(x => x.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                        string formato = "D" + numeroDigitos.ToString();

                        string[] elementos = item.NumeroUnico.Split('-').ToArray();

                        int temp = Convert.ToInt32(elementos[1]);

                        item.NumeroUnico = elementos[0] + "-" + temp.ToString(formato);
                    }

                    lstAlmacenaje.ListadoItemCode = agruparItemCodes;
                    lstAlmacenaje.ListadoAlmacenaje = ListadoAlmacenaje;

                    return lstAlmacenaje;
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
        /// Obtiene los numeros unicos de un item code
        /// </summary>
        /// <param name="avisoLlegadaID"></param>
        /// <returns>Regresa un JSON object con las propiedades de los numeros unicos</returns>
        public object ObtenerNumerosUnicosOrdenesAlmacenaje(string itemCode, Sam3_Usuario usuario, int ordenAlmacenajeID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int itemCodeID = itemCode != "" ? Convert.ToInt32(itemCode) : 0;
                    List<NumerosUnicos> registros = new List<NumerosUnicos>();

                    registros = (from nu in ctx.Sam3_NumeroUnico
                                 join relnu in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on nu.NumeroUnicoID equals relnu.NumeroUnicoID
                                 join oa in ctx.Sam3_OrdenAlmacenaje on relnu.OrdenAlmacenajeID equals oa.OrdenAlmacenajeID
                                 where nu.Activo && relnu.Activo && oa.Activo
                                 && nu.ItemCodeID == itemCodeID
                                 && oa.Folio == ordenAlmacenajeID
                                 && (nu.EstatusDocumental != null && nu.EstatusFisico != null)
                                 select new NumerosUnicos
                                 {
                                     NumeroUnicoID = nu.NumeroUnicoID.ToString(),
                                     NumeroUnico = nu.Prefijo + "-" + nu.Consecutivo
                                 }).AsParallel().ToList();

                    foreach (var nu in registros)
                    {
                        int numeroDigitos = (from it in ctx.Sam3_ItemCode
                                             join pc in ctx.Sam3_ProyectoConfiguracion on it.ProyectoID equals pc.ProyectoID
                                             where it.ItemCodeID == itemCodeID
                                             select pc.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                        string formato = "D" + numeroDigitos.ToString();

                        string[] codigo = nu.NumeroUnico.Split('-').ToArray();
                        int consecutivo = Convert.ToInt32(codigo[1]);
                        nu.NumeroUnico = codigo[0] + "-" + consecutivo.ToString(formato);
                    }

                    return registros;
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
        public object ObtenerDiametrosItemCodeSteelgo(string itemCode, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int itemCodeID = itemCode != "" ? Convert.ToInt32(itemCode) : 0;

                    ItemCodeSteelgoJson itemCodeSteelgo = (from ic in ctx.Sam3_ItemCode
                                                           join rid in ctx.Sam3_Rel_ItemCode_Diametro on ic.ItemCodeID equals rid.ItemCodeID
                                                           join rics in ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo on rid.Rel_ItemCode_Diametro_ID equals rics.Rel_ItemCode_Diametro_ID
                                                           join rids in ctx.Sam3_Rel_ItemCodeSteelgo_Diametro on rics.Rel_ItemCodeSteelgo_Diametro_ID equals rids.Rel_ItemCodeSteelgo_Diametro_ID
                                                           join ics in ctx.Sam3_ItemCodeSteelgo on rids.ItemCodeSteelgoID equals ics.ItemCodeSteelgoID
                                                           join d1 in ctx.Sam3_Diametro on rids.Diametro1ID equals d1.DiametroID
                                                           join d2 in ctx.Sam3_Diametro on rids.Diametro2ID equals d2.DiametroID
                                                           where ic.ItemCodeID == itemCodeID
                                                           select new ItemCodeSteelgoJson
                                                           {
                                                               Diametro1 = d1.Valor,
                                                               Diametro2 = d2.Valor
                                                           }).AsParallel().FirstOrDefault();

                    return itemCodeSteelgo;
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

        public int? getRack(List<ListaNumerosUnicos> ids, int id)
        {
            int? rack = 0;
            foreach (var i in ids)
            {
                if (i.NumeroUnicoID.Equals(id.ToString()))
                {
                    rack = i.Rack == null ? 0 : int.Parse(i.Rack);
                    break;
                }
            }

            return rack == 0 ? null : rack;
        }

        public object GenerarAlmacenaje(Items listados, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaNumerosUnicos> ids = listados.NumerosUnicos.GroupBy(x => x.NumeroUnicoID).Select(x => x.First()).OrderBy(x => x.NumeroUnicoID).AsParallel().ToList();
                List<int> nuids = new List<int>();

                foreach (var i in ids)
                {
                    nuids.Add(Convert.ToInt32(i.NumeroUnicoID));
                }

                string rack = ids.Select(x => x.Rack).FirstOrDefault();
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {

                        List<Sam3_NumeroUnico> list = (from nu in ctx.Sam3_NumeroUnico
                                                       where nu.Activo
                                                       && nuids.Contains(nu.NumeroUnicoID)
                                                       select nu).AsParallel().ToList();

                        list.ForEach(x => x.RackID = getRack(ids, x.NumeroUnicoID));

                        ctx.SaveChanges();
                        ctx_tran.Commit();
                    }
                }

                //if (!(bool)EnviarAvisosBd.Instance.EnviarNotificación(1,
                //                                    string.Format("Se guardaron los almacenajes del  orden de almacenaje Folio: {0}",
                //                                    listados.OrdenAlmacenajeID), usuario))
                //{
                //    //Agregar error a la bitacora  PENDIENTE
                //}

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Ok");
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


    }//Fin Clase
}