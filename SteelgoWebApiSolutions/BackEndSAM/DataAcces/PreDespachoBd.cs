using BackEndSAM.Models;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces
{
    public class PreDespachoBd
    {
        private static readonly object _mutex = new object();
        private static PreDespachoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private PreDespachoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static PreDespachoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new PreDespachoBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Funcion para obtener los datos del grid de pre despacho
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerGridPreDespacho(int odtID, Sam3_Usuario usuario)
        {
            try
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
                              join eq in ctx.Sam3_EquivalenciaPatio on pa.PatioID equals eq.Sam2_PatioID
                              where p.Activo && pa.Activo && eq.Activo
                              && proyectos.Contains(p.ProyectoID)
                              select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                    patios = patios.Where(x => x > 0).ToList();

                    List<int> numerosUnicosAprobadosSam3 = (from nu in ctx.Sam3_NumeroUnico
                                                        where nu.Activo && nu.EstatusDocumental == "Aprobado" && nu.EstatusFisico == "Aprobado"
                                                        select nu.NumeroUnicoID).AsParallel().ToList();

                    List<int> numerosUnicosAprobadosSam2 = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                            where eq.Activo && numerosUnicosAprobadosSam3.Contains(eq.Sam3_NumeroUnicoID)
                                                            select eq.Sam2_NumeroUnicoID).AsParallel().ToList();

                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        List<PreDespacho> listado = (from ots in ctx2.OrdenTrabajoSpool
                                                     join ot in ctx2.OrdenTrabajo on ots.OrdenTrabajoID equals ot.OrdenTrabajoID
                                                     join otm in ctx2.OrdenTrabajoMaterial on ots.OrdenTrabajoSpoolID equals otm.OrdenTrabajoSpoolID
                                                     join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                                     join nu in ctx2.NumeroUnico on otm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                     join ic in ctx2.ItemCode on ms.ItemCodeID equals ic.ItemCodeID
                                                     where ot.OrdenTrabajoID == odtID
                                                     && proyectos.Contains(ot.ProyectoID)
                                                     && ic.TipoMaterialID == 2
                                                     //&& !otm.TieneDespacho && numerosUnicosAprobadosSam2.Contains(nu.NumeroUnicoID)
                                                     select new PreDespacho
                                                     {
                                                         ItemCodeID = ic.ItemCodeID.ToString(),
                                                         ItemCode = ic.Codigo,
                                                         NumeroControlID = ots.OrdenTrabajoSpoolID.ToString(),
                                                         NumeroControl = ots.NumeroControl,
                                                         //NumeroUnico = nu.Codigo,
                                                         Descripcion = ic.DescripcionEspanol,
                                                         Etiqueta = ms.Etiqueta,
                                                         MaterialSpoolID = otm.MaterialSpoolID
                                                     }).AsParallel().ToList();

                        foreach (PreDespacho item in listado)
                        {
                            int sam3_ItemCodeID = (from eqn in ctx.Sam3_EquivalenciaItemCode
                                                   where eqn.Activo
                                                   && eqn.Sam2_ItemCodeID.ToString() == item.ItemCodeID
                                                   select eqn.Sam3_ItemCodeID).AsParallel().SingleOrDefault();

                            if (ctx.Sam3_PreDespacho
                                .Where(x => x.OrdenTrabajoSpoolID.ToString() == item.NumeroControlID 
                                    && x.ItemCodeID == sam3_ItemCodeID
                                    && x.MaterialSpoolID == item.MaterialSpoolID).Any())
                            {
                                Sam3_PreDespacho preDespacho = ctx.Sam3_PreDespacho
                                .Where(x => x.OrdenTrabajoSpoolID.ToString() == item.NumeroControlID
                                    && x.ItemCodeID == sam3_ItemCodeID
                                    && x.MaterialSpoolID == item.MaterialSpoolID
                                    && x.Activo).AsParallel().SingleOrDefault();

                                if (preDespacho != null)
                                {
                                    item.PreDespachoID = preDespacho.PreDespachoID.ToString();

                                    int sam3_NumeroUnicoID = preDespacho.NumeroUnicoID; //(from pre in ctx.Sam3_PreDespacho
                                    //                          where pre.Activo
                                    //                          && pre.OrdenTrabajoSpoolID.ToString() == item.NumeroControlID
                                    //                          && pre.ItemCodeID == sam3_ItemCodeID
                                    //                          && pre.MaterialSpoolID == item.MaterialSpoolID
                                    //                          select pre.NumeroUnicoID).AsParallel().SingleOrDefault();

                                    int sam2_numeroUnicoID = (from eqn in ctx.Sam3_EquivalenciaNumeroUnico
                                                              where eqn.Activo && eqn.Sam3_NumeroUnicoID == sam3_NumeroUnicoID
                                                              select eqn.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                    NumeroUnico SAM2NumUnico = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_numeroUnicoID).AsParallel().SingleOrDefault();

                                    item.NumeroUnico = SAM2NumUnico.Codigo;
                                    item.NumeroUnicoID = SAM2NumUnico.NumeroUnicoID.ToString();
                                }
                            }
                        }

                        return listado.OrderBy(x => x.NumeroControlID).ToList();
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
        /// Funcion para el boton Predespachar 
        /// Pantalla pre despacho
        /// </summary>
        /// <param name="lista"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object Predespachar(List<PreDespachoItems> lista, Sam3_Usuario usuario)
        {
            try
            {
                int preDespachoID = 0;
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            foreach (PreDespachoItems item in lista)
                            {
                                preDespachoID = item.PreDespachoID != "" ? Convert.ToInt32(item.PreDespachoID) : 0;
                                bool tieneHold = (from sh in ctx2.SpoolHold
                                                  where sh.SpoolID == (from odts in ctx2.OrdenTrabajoSpool
                                                                       where odts.OrdenTrabajoSpoolID.ToString() == item.NumeroControl
                                                                       select odts.SpoolID).FirstOrDefault()
                                                  && (sh.TieneHoldCalidad || sh.TieneHoldIngenieria || sh.Confinado)
                                                  select sh).AsParallel().Any();

                                if (!tieneHold)
                                {
                                    DatabaseManager.Sam2.ItemCode itemCode = (from it in ctx2.ItemCode
                                                                              where it.ItemCodeID.ToString() == item.ItemCode
                                                                              select it).AsParallel().SingleOrDefault();

                                    int proyectoID = itemCode.ProyectoID;
                                    int sam3_ProyectoID = (from eqp in ctx.Sam3_EquivalenciaProyecto
                                                           where eqp.Activo && eqp.Sam2_ProyectoID == proyectoID
                                                           select eqp.Sam3_ProyectoID).AsParallel().SingleOrDefault();

                                    int sam3_ItemCodeID = (from eqi in ctx.Sam3_EquivalenciaItemCode
                                                           where eqi.Activo && eqi.Sam2_ItemCodeID == itemCode.ItemCodeID
                                                           select eqi.Sam3_ItemCodeID).AsParallel().SingleOrDefault();

                                    //Dividimos el codigo del numero para buscarlo en sam3
                                    string[] elementosCodigo = item.NumeroUnico.Split('-').ToArray();
                                    int consecutivoNumeroUnico = Convert.ToInt32(elementosCodigo[1]);
                                    string prefijoNumeroUnico = elementosCodigo[0];

                                    //int sam3_ProyectoID = (from nueq in ctx.Sam3_EquivalenciaProyecto
                                    //                       where nueq.Activo && nueq.Sam2_ProyectoID == proyectoID
                                    //                       select nueq.Sam3_ProyectoID).AsParallel().SingleOrDefault();

                                    //traemos los datos de la orden de trabajo spool de Sam2
                                    OrdenTrabajoSpool odtSpool = (from odts in ctx2.OrdenTrabajoSpool
                                                                  join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                                  where odts.OrdenTrabajoSpoolID.ToString() == item.NumeroControl
                                                                  && odt.ProyectoID == proyectoID
                                                                  select odts).AsParallel().SingleOrDefault();

                                    //traemos los datos del material de Sam 2
                                    MaterialSpool materialSpool = (from ms in ctx2.MaterialSpool
                                                                   join odts in ctx2.OrdenTrabajoSpool on ms.SpoolID equals odts.SpoolID
                                                                   where odts.OrdenTrabajoSpoolID == odtSpool.OrdenTrabajoSpoolID
                                                                   && ms.Etiqueta == item.Etiqueta
                                                                   select ms).AsParallel().SingleOrDefault();

                                    //traemos los datos de la orden de trabajo material de Sam 2
                                    OrdenTrabajoMaterial odtMaterial = (from odtm in ctx2.OrdenTrabajoMaterial
                                                                        where odtm.OrdenTrabajoSpoolID == odtSpool.OrdenTrabajoSpoolID
                                                                        && odtm.MaterialSpoolID == materialSpool.MaterialSpoolID
                                                                        select odtm).AsParallel().SingleOrDefault();

                                    //buscamos el numero unico en SAM 3
                                    if (ctx.Sam3_NumeroUnico.Where(x => x.Prefijo == prefijoNumeroUnico
                                        && x.Consecutivo == consecutivoNumeroUnico && x.ProyectoID == sam3_ProyectoID).AsParallel().Any())
                                    {
                                        //int sam2_numeroUnicoID = 0;
                                        // numero unico seleccionado en el grid
                                        Sam3_NumeroUnico numeroUnico = ctx.Sam3_NumeroUnico.Where(x => x.Prefijo == prefijoNumeroUnico
                                        && x.Consecutivo == consecutivoNumeroUnico && x.ProyectoID == sam3_ProyectoID).AsParallel().SingleOrDefault();


                                        if (preDespachoID > 0) // existe el predespacho
                                        {
                                            Sam3_PreDespacho preDespacho = (from pre in ctx.Sam3_PreDespacho
                                                                            where pre.Activo
                                                                            && pre.PreDespachoID == preDespachoID
                                                                            select pre).AsParallel().SingleOrDefault();

                                            // si el numero Unico seleccionado es diferente del numero unico que se encuentra registrado en el predespacho
                                            if (preDespachoID > 0 &&  preDespacho.NumeroUnicoID != numeroUnico.NumeroUnicoID)
                                            {

                                                preDespacho.Activo = true;
                                                preDespacho.FechaModificacion = DateTime.Now;
                                                preDespacho.FechaPreDespacho = DateTime.Now;
                                                preDespacho.UsuarioModificacion = usuario.UsuarioID;
                                                preDespacho.ProyectoID = sam3_ProyectoID;
                                                preDespacho.NumeroUnicoID = numeroUnico.NumeroUnicoID;
                                                preDespacho.Cantidad = (int)odtMaterial.CantidadCongelada;
                                                ctx.SaveChanges();
                                            }
                                            
                                        }
                                        else
                                        {
                                            Sam3_PreDespacho preDespacho = new Sam3_PreDespacho();
                                            preDespacho.Activo = true;
                                            preDespacho.FechaModificacion = DateTime.Now;
                                            preDespacho.FechaPreDespacho = DateTime.Now;
                                            preDespacho.UsuarioModificacion = usuario.UsuarioID;
                                            preDespacho.ProyectoID = sam3_ProyectoID;
                                            preDespacho.OrdenTrabajoSpoolID = Convert.ToInt32(item.NumeroControl);
                                            preDespacho.NumeroUnicoID = numeroUnico.NumeroUnicoID;
                                            preDespacho.MaterialSpoolID = odtMaterial.MaterialSpoolID;
                                            preDespacho.ItemCodeID = sam3_ItemCodeID;
                                            preDespacho.Cantidad = (int)odtMaterial.CantidadCongelada;
                                            ctx.Sam3_PreDespacho.Add(preDespacho);
                                            ctx.SaveChanges();
                                        }
                                    }
                                }
                                else
                                {
                                    throw new Exception("El Spool cuenta con Hold, no se puede Pre-Despachar");
                                }

                            }

                            ctx_tran.Commit();

                            TransactionalInformation result = new TransactionalInformation();
                            //result.ReturnMessage.Add(preDespacho.PreDespachoID.ToString());
                            result.ReturnCode = 200;
                            result.ReturnStatus = true;
                            result.IsAuthenicated = true;

                            return result;
                        }
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


        public object ListadoNumerosUnicos(string numeroControl, string itemcode, string etiqueta, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombosICC> listado = new List<ListaCombosICC>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        int proyectoID = (from odts in ctx2.OrdenTrabajoSpool
                                          join s in ctx2.Spool on odts.SpoolID equals s.SpoolID
                                          where odts.NumeroControl == numeroControl
                                          select s.ProyectoID).AsParallel().Distinct().FirstOrDefault();

                        int digitos = (from p in ctx2.ProyectoConfiguracion
                                       where p.ProyectoID == proyectoID
                                       select p.DigitosNumeroUnico).AsParallel().SingleOrDefault();

                        // numero unico congelado
                        List<int> sam2_NumerosUnicosIDs = (from odts in ctx2.OrdenTrabajoSpool
                                                           join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                           join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                           join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                           join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                           join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                           join p in ctx2.Proyecto on nu.ProyectoID equals p.ProyectoID
                                                           join pa in ctx2.Patio on p.PatioID equals pa.PatioID
                                                           where odts.NumeroControl == numeroControl
                                                           && it.TipoMaterialID == 2
                                                           && it.Codigo == itemcode
                                                           && ms.Diametro1 == nu.Diametro1
                                                           && ms.Diametro2 == nu.Diametro2
                                                           && ms.Etiqueta == etiqueta
                                                           select nu.NumeroUnicoID).Distinct().AsParallel().ToList();

                        //numeros unicos del mismo itemode y con material suficiente para el despacho
                        sam2_NumerosUnicosIDs.AddRange((from it in ctx2.ItemCode
                                                        join nu in ctx2.NumeroUnico on it.ItemCodeID equals nu.ItemCodeID
                                                        join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                        join ms in ctx2.MaterialSpool on it.ItemCodeID equals ms.ItemCodeID
                                                        where it.Codigo == itemcode
                                                        && it.TipoMaterialID == 2
                                                        && ms.Diametro1 == nu.Diametro1
                                                        && ms.Diametro2 == nu.Diametro2
                                                        && (nui.InventarioDisponibleCruce >= ms.Cantidad
                                                        || nui.InventarioCongelado >= ms.Cantidad)
                                                        select nu.NumeroUnicoID).Distinct().AsParallel().ToList());

                        listado = (from nu in ctx.Sam3_NumeroUnico
                                   join nueq in ctx.Sam3_EquivalenciaNumeroUnico on nu.NumeroUnicoID equals nueq.Sam3_NumeroUnicoID
                                   join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                   where nu.Activo && nueq.Activo
                                   && sam2_NumerosUnicosIDs.Contains(nueq.Sam2_NumeroUnicoID)
                                   && nu.EstatusFisico == "Aprobado" //&& nu.EstatusDocumental == "Aprobado"
                                   select new ListaCombosICC
                                   {
                                       id = nu.NumeroUnicoID.ToString(),
                                       value = nu.Prefijo + "-" + nu.Consecutivo,
                                       tipoGrupo = it.TipoMaterialID
                                   }).Distinct().AsParallel().ToList();

                        foreach (ListaCombosICC lst in listado)
                        {
                            string[] elementos = lst.value.Split('-');

                            int temp = Convert.ToInt32(lst.id);
                            string formato = "D" + digitos.ToString();
                            int consecutivo = Convert.ToInt32(elementos[1]);

                            lst.value = elementos[0] + "-" + consecutivo.ToString(formato);

                        }
                    }
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

        public object EliminarPredespacho(string lista,Sam3_Usuario usuario) {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            int[] ids = ser.Deserialize<int[]>(lista);
            using (SamContext ctx = new SamContext()) {
                foreach (int id in ids){
                    Sam3_PreDespacho pred = ctx.Sam3_PreDespacho.Where(x => x.PreDespachoID == id).FirstOrDefault();
                    pred.Activo = false;
                    pred.FechaModificacion = DateTime.Now;
                    pred.UsuarioModificacion = usuario.UsuarioID;
                    ctx.SaveChanges();
                }
            }
            TransactionalInformation result = new TransactionalInformation();
            //result.ReturnMessage.Add(preDespacho.PreDespachoID.ToString());
            result.ReturnCode = 200;
            result.ReturnStatus = true;
            result.IsAuthenicated = true;

            return result;
        }
    }
}