using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;

namespace BackEndSAM.DataAcces
{
    public class OrdenTrabajoSpoolBd
    {
        private static readonly object _mutex = new object();
        private static OrdenTrabajoSpoolBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private OrdenTrabajoSpoolBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static OrdenTrabajoSpoolBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoSpoolBd();
                    }
                }
                return _instance;
            }
        }

        public object ListadoNumerosDeControlPreDespachoYDespacho(string busqueda, Sam3_Usuario usuario) 
        {
            try
            {

                if (busqueda == null || (busqueda.Length == 1 && busqueda.Contains("-")))
                {
                    busqueda = "";
                }

                if ((busqueda.Length > 1 && busqueda.Contains("\\n")))
                {
                    busqueda = busqueda.Replace("\\n", "-");
                }

                if ((busqueda.Length == 1 && busqueda.Contains("-")))
                {
                    busqueda = "";
                }
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    List<int> proyectos = new List<int>();
                    List<int> patios = new List<int>();
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);
                    using (SamContext ctx = new SamContext())
                    {
                        proyectos = (from eqp in ctx.Sam3_EquivalenciaProyecto
                                     where eqp.Activo
                                     && proyectos.Contains(eqp.Sam3_ProyectoID)
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();

                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam3_PatioID
                                  join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && proyectos.Contains(eqp.Sam2_ProyectoID)
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();
                    }



                    int consecutivo = 0;

                    if (busqueda.Contains("-") && busqueda != "")
                    {
                        string[] divididos = busqueda.Split('-').ToArray();
                        int.TryParse(divididos[1], out consecutivo);

                        busqueda = divididos[0] != "" ? divididos[0].Replace("0", string.Empty).ToUpper() : "";

                    }
                    else
                    {
                        busqueda = busqueda.Replace("0", string.Empty).ToUpper();
                    }


                    List<ComboNumeroControl> listado = (from odts in ctx2.OrdenTrabajoSpool
                                                        join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                        join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                        join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                        join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                        where !(from d in ctx2.Despacho
                                                                where d.Cancelado == false
                                                                select d.OrdenTrabajoSpoolID).Contains(odts.OrdenTrabajoSpoolID)
                                                        && !(from sh in ctx2.SpoolHold
                                                             where sh.SpoolID == odts.SpoolID
                                                             && (sh.Confinado || sh.TieneHoldCalidad || sh.TieneHoldIngenieria)
                                                             select sh).Any()
                                                        && proyectos.Contains(odt.ProyectoID)
                                                        && it.TipoMaterialID == 2
                                                        select new ComboNumeroControl
                                                        {
                                                            NumeroControlID = odt.OrdenTrabajoID.ToString(),//odts.OrdenTrabajoSpoolID.ToString(),
                                                            NumeroControl = odt.NumeroOrden//odts.NumeroControl
                                                        }).Distinct().AsParallel().ToList();

                    List<ComboNumeroControl> filtrado = new List<ComboNumeroControl>();
                    listado = listado.GroupBy(x => x.NumeroControlID).Select(x => x.First()).ToList();

                    if (busqueda == "")
                    {
                        return listado.OrderBy(x => x.NumeroControl).ToList();
                    }

                    foreach (ComboNumeroControl lst in listado)
                    {
                        if (lst.NumeroControl.Contains("-") && lst.NumeroControl != "")
                        {
                            string[] elem = lst.NumeroControl.Split('-').ToArray();

                            string simplificado = elem[0].Replace("0", string.Empty);
                            int consec = Convert.ToInt32(elem[1]);

                            if (consecutivo > 0)
                            {
                                if (simplificado.Replace("0", string.Empty).ToUpper().Contains(busqueda) && consec == consecutivo)
                                {
                                    filtrado.Add(lst);
                                }
                            }
                            else
                            {
                                if (simplificado.Replace("0", string.Empty).ToUpper().Contains(busqueda))
                                {
                                    filtrado.Add(lst);
                                }
                            }
                        }
                        else
                        {
                            if (lst.NumeroControl.Replace("0", string.Empty).ToUpper().Contains(busqueda))
                            {
                                filtrado.Add(lst);
                            }
                        }

                    }
#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(filtrado);
#endif

                    return filtrado.OrderBy(x => x.NumeroControl).ToList();

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

        public object ListadoNumerosDeControl(string busqueda, Sam3_Usuario usuario)
        {
            try
            {
                
                if (busqueda == null || (busqueda.Length == 1 && busqueda.Contains("-")))
                {
                    busqueda = "";
                }

                if ((busqueda.Length > 1 && busqueda.Contains("\\n")))
                 {
                  busqueda = busqueda.Replace("\\n", "-");
                 }

                if ((busqueda.Length == 1 && busqueda.Contains("-"))) {
                    busqueda = "";
                }
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    List<int> proyectos = new List<int>();
                    List<int> patios = new List<int>();
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);
                    using (SamContext ctx = new SamContext())
                    {
                        proyectos = (from eqp in ctx.Sam3_EquivalenciaProyecto
                                     where eqp.Activo
                                     && proyectos.Contains(eqp.Sam3_ProyectoID)
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();




                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam3_PatioID
                                  join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && proyectos.Contains(eqp.Sam2_ProyectoID)
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();
                    }



                    int consecutivo = 0;

                    if (busqueda.Contains("-") && busqueda != "")
                    {
                        string[] divididos = busqueda.Split('-').ToArray();
                        int.TryParse(divididos[1], out consecutivo);

                        busqueda = divididos[0] != "" ? divididos[0].Replace("0", string.Empty).ToUpper() : "";

                    }
                    else
                    {
                        busqueda = busqueda.Replace("0", string.Empty).ToUpper();
                    }

                    
                    List<ComboNumeroControl> listado = (from odts in ctx2.OrdenTrabajoSpool
                                                        join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                        join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                        join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                        join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                        where !(from d in ctx2.Despacho
                                                                where d.Cancelado == false
                                                                select d.OrdenTrabajoSpoolID).Contains(odts.OrdenTrabajoSpoolID)
                                                        && !(from sh in ctx2.SpoolHold
                                                             where sh.SpoolID == odts.SpoolID
                                                             && (sh.Confinado || sh.TieneHoldCalidad || sh.TieneHoldIngenieria)
                                                             select sh).Any()
                                                        && proyectos.Contains(odt.ProyectoID)
                                                        && it.TipoMaterialID == 2
                                                        && !odtm.TieneDespacho
                                                        select new ComboNumeroControl
                                                        {
                                                            NumeroControlID = odts.OrdenTrabajoSpoolID.ToString(),//odts.OrdenTrabajoSpoolID.ToString(),
                                                            NumeroControl = odts.NumeroControl//odts.NumeroControl
                                                        }).Distinct().AsParallel().ToList();

                    List<ComboNumeroControl> filtrado = new List<ComboNumeroControl>();
                    listado = listado.GroupBy(x => x.NumeroControlID).Select(x => x.First()).ToList();

                    if (busqueda == "")
                    {
                        return listado.OrderBy(x => x.NumeroControl).ToList();
                    }

                    foreach (ComboNumeroControl lst in listado)
                    {
                        if (lst.NumeroControl.Contains("-") && lst.NumeroControl != "")
                        {
                            string[] elem = lst.NumeroControl.Split('-').ToArray();

                            string simplificado = elem[0].Replace("0", string.Empty);
                            int consec = Convert.ToInt32(elem[1]);

                            if (consecutivo > 0)
                            {
                                if (simplificado.Contains(busqueda) && consec == consecutivo)
                                {
                                    filtrado.Add(lst);
                                }
                            }
                            else
                            {
                                if (simplificado.Contains(busqueda))
                                {
                                    filtrado.Add(lst);
                                }
                            }
                        }
                        else
                        {
                            if(lst.NumeroControl.Contains(busqueda))
                            {
                                filtrado.Add(lst);
                            }
                        }

                    }
#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(filtrado);
#endif

                    return filtrado.OrderBy(x => x.NumeroControl).ToList();

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


        public object ListadoNumeroControlConvertirAGranel(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<int> proyectos = new List<int>();
                        List<int> patios = new List<int>();

                        UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                        List<int> proyectos_sam2 = (from eqp in ctx.Sam3_EquivalenciaProyecto
                                                    where eqp.Activo
                                                    && proyectos.Contains(eqp.Sam3_ProyectoID)
                                                    select eqp.Sam2_ProyectoID).AsParallel().ToList();

                        List<int> patios_sam2 = (from eqp in ctx.Sam3_EquivalenciaPatio
                                                 where eqp.Activo
                                                 && patios.Contains(eqp.Sam3_PatioID)
                                                 select eqp.Sam2_PatioID).AsParallel().ToList();

                        List<ListaCombos> listado = (from odts in ctx2.OrdenTrabajoSpool
                                                     join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                     where proyectos_sam2.Contains(odt.ProyectoID)
                                                     select new ListaCombos
                                                     {
                                                         id = odts.OrdenTrabajoSpoolID.ToString(),
                                                         value = odts.NumeroControl
                                                     }).AsParallel().Distinct().ToList();

                        listado = listado.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.value).ToList();

                        return listado;
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

        public object ObtenerJuntasPorODTS(int odtsID, Sam3_Usuario usuario)
        {
            try
            {
                ItemsSpoolAGranel lista = new ItemsSpoolAGranel();
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    lista.Items.AddRange((from js in ctx2.JuntaSpool
                                          join odts in ctx2.OrdenTrabajoSpool on js.SpoolID equals odts.SpoolID
                                          join tj in ctx2.TipoJunta on js.TipoJuntaID equals tj.TipoJuntaID
                                          where odts.OrdenTrabajoSpoolID == odtsID
                                          select new ListadoConvertirSpoolAGranel
                                          {
                                              Spool = odts.NumeroControl,
                                              Junta = js.Etiqueta,
                                              Status = (from jwks in ctx2.JuntaWorkstatus
                                                        join up in ctx2.UltimoProceso on jwks.UltimoProcesoID equals up.UltimoProcesoID
                                                        where jwks.JuntaSpoolID == js.JuntaSpoolID
                                                        select up.Nombre).FirstOrDefault(),
                                              TipoJunta = tj.Nombre
                                          }).AsParallel().Distinct().ToList());
                }
                return lista;
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

        public object ConvertirJuntasPorODTS(int odtsID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                //buscamos las juntas que aun no tienen ningun proceso de armado, soldadura o inspeccion visual
                                List<JuntaSpool> listaJuntas = (from js in ctx2.JuntaSpool
                                                                join odts in ctx2.OrdenTrabajoSpool on js.SpoolID equals odts.SpoolID
                                                                where odts.OrdenTrabajoSpoolID == odtsID
                                                                && !(from jwks in ctx2.JuntaWorkstatus
                                                                     where jwks.JuntaSpoolID == js.JuntaSpoolID
                                                                     && jwks.JuntaArmadoID > 0 && jwks.JuntaArmadoID != null
                                                                     && jwks.JuntaSoldaduraID > 0 && jwks.JuntaSoldaduraID != null
                                                                     && jwks.JuntaInspeccionVisualID > 0 && jwks.JuntaInspeccionVisualID != null
                                                                     && jwks.JuntaFinal
                                                                     select jwks.JuntaSpoolID).Contains(js.JuntaSpoolID)
                                                                && (from tj in ctx2.TipoJunta
                                                                    where tj.Nombre == "SHOP" || tj.Nombre == "FIELD"
                                                                    select tj.TipoJuntaID).Contains(js.TipoJuntaID)
                                                                select js).AsParallel().Distinct().ToList();

                                List<Sam3_JuntaSpool> sam3_JuntasSpool = (from eq in ctx.Sam3_EquivalenciaJuntaSpool
                                                                          join js in ctx.Sam3_JuntaSpool on eq.Sam3_JuntaSpoolID equals js.JuntaSpoolID
                                                                          where listaJuntas.Select(x => x.JuntaSpoolID).Contains(eq.Sam2_JuntaSpoolID)
                                                                          select js).AsParallel().ToList();

                                int sam2_fabAreaID = (from f in ctx2.FabArea
                                                      where f.Nombre == "FIELD"
                                                      select f.FabAreaID).AsParallel().SingleOrDefault();

                                int sam3_fabAreaID = (from f in ctx.Sam3_FabArea
                                                      where f.Nombre == "FIELD"
                                                      select f.FabAreaID).AsParallel().SingleOrDefault();

                                listaJuntas.ForEach(x => x.FabAreaID = sam2_fabAreaID);
                                sam3_JuntasSpool.ForEach(x => x.FabAreaID = sam3_fabAreaID);

                                ctx.SaveChanges();
                                ctx2.SaveChanges();

                                ctx2_tran.Commit();
                            }
                        }
                        ctx_tran.Commit();
                    }
                }

                return ObtenerJuntasPorODTS(odtsID, usuario);

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

        public object ObtenerODTSporODT(int odtID, string busqueda, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombos> listado = new List<ListaCombos>();
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    List<string> elementos = new List<string>();
                    if (busqueda != "" && busqueda != null)
                    {
                        char[] lstElementoNumeroControl = busqueda.ToCharArray();
                        foreach (char i in lstElementoNumeroControl)
                        {
                            elementos.Add(i.ToString());
                        }
                    }

                    listado = (from odts in ctx2.OrdenTrabajoSpool
                               where odts.OrdenTrabajoID == odtID
                               && elementos.Any(x => odts.NumeroControl.Contains(x))
                               select new ListaCombos
                               {
                                   id = odts.OrdenTrabajoSpoolID.ToString(),
                                   value = odts.NumeroControl
                               }).AsParallel().Distinct().OrderBy(x => x.value).ToList();
                }
                return listado;
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

        public object EtiquetasDisponibles(string numeroControl, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombos> resultado = new List<ListaCombos>();

                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        resultado = (from odtm in ctx2.OrdenTrabajoMaterial
                                     join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                     join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                     join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                     where odts.NumeroControl == numeroControl
                                     && !odtm.TieneDespacho 
                                     && it.TipoMaterialID == 1
                                     select new ListaCombos
                                     {
                                         id = ms.MaterialSpoolID.ToString(),
                                         value = ms.Etiqueta
                                     }).AsParallel().Distinct().ToList();
                    }
                }

                return resultado;

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

        public object ListadoNCImpresionDocumental(string busqueda, Sam3_Usuario usuario)
        {
            try
            {

                if (busqueda == null || (busqueda.Length == 1 && busqueda.Contains("-")))
                {
                    busqueda = "";
                }

                if ((busqueda.Length > 1 && busqueda.Contains("\\n")))
                {
                    busqueda = busqueda.Replace("\\n", "-");
                }

                if ((busqueda.Length == 1 && busqueda.Contains("-")))
                {
                    busqueda = "";
                }
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    List<int> proyectos = new List<int>();
                    List<int> patios = new List<int>();
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);
                    using (SamContext ctx = new SamContext())
                    {
                        proyectos = (from eqp in ctx.Sam3_EquivalenciaProyecto
                                     where eqp.Activo
                                     && proyectos.Contains(eqp.Sam3_ProyectoID)
                                     select eqp.Sam2_ProyectoID).Distinct().AsParallel().ToList();

                        proyectos = proyectos.Where(x => x > 0).ToList();




                        patios = (from p in ctx.Sam3_Proyecto
                                  join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                  join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam3_PatioID
                                  join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                                  where p.Activo && pa.Activo && eq.Activo
                                  && proyectos.Contains(eqp.Sam2_ProyectoID)
                                  select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                        patios = patios.Where(x => x > 0).ToList();
                    }



                    int consecutivo = 0;

                    if (busqueda.Contains("-") && busqueda != "")
                    {
                        string[] divididos = busqueda.Split('-').ToArray();
                        int.TryParse(divididos[1], out consecutivo);

                        busqueda = divididos[0] != "" ? divididos[0].Replace("0", string.Empty).ToUpper() : "";

                    }
                    else
                    {
                        busqueda = busqueda.Replace("0", string.Empty).ToUpper();
                    }


                    List<ComboNumeroControl> listado = (from odts in ctx2.OrdenTrabajoSpool
                                                        join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                        join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                        join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                        join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                        where !(from d in ctx2.Despacho
                                                                where d.Cancelado == false
                                                                select d.OrdenTrabajoSpoolID).Contains(odts.OrdenTrabajoSpoolID)
                                                        && !(from sh in ctx2.SpoolHold
                                                             where sh.SpoolID == odts.SpoolID
                                                             && (sh.Confinado || sh.TieneHoldCalidad || sh.TieneHoldIngenieria)
                                                             select sh).Any()
                                                        && proyectos.Contains(odt.ProyectoID)
                                                        select new ComboNumeroControl
                                                        {
                                                            NumeroControlID = odts.OrdenTrabajoSpoolID.ToString(),//odts.OrdenTrabajoSpoolID.ToString(),
                                                            NumeroControl = odts.NumeroControl//odts.NumeroControl
                                                        }).Distinct().AsParallel().ToList();

                    List<ComboNumeroControl> filtrado = new List<ComboNumeroControl>();
                    listado = listado.GroupBy(x => x.NumeroControlID).Select(x => x.First()).ToList();

                    if (busqueda == "")
                    {
                        return listado.OrderBy(x => x.NumeroControl).ToList();
                    }

                    foreach (ComboNumeroControl lst in listado)
                    {
                        if (lst.NumeroControl.Contains("-") && lst.NumeroControl != "")
                        {
                            string[] elem = lst.NumeroControl.Split('-').ToArray();

                            string simplificado = elem[0].Replace("0", string.Empty);
                            int consec = Convert.ToInt32(elem[1]);

                            if (consecutivo > 0)
                            {
                                if (simplificado.Contains(busqueda) && consec == consecutivo)
                                {
                                    filtrado.Add(lst);
                                }
                            }
                            else
                            {
                                if (simplificado.Contains(busqueda))
                                {
                                    filtrado.Add(lst);
                                }
                            }
                        }
                        else
                        {
                            if (lst.NumeroControl.Contains(busqueda))
                            {
                                filtrado.Add(lst);
                            }
                        }

                    }
#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(filtrado);
#endif

                    return filtrado.OrderBy(x => x.NumeroControl).ToList();

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