using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Sam2;
using DatabaseManager.Sam3;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Web.Script.Serialization;
using System.Transactions;
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    public class DespachoBd
    {
        private static readonly object _mutex = new object();
        private static DespachoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DespachoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static DespachoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DespachoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerListadoDespachos(FiltrosJson filtros, Sam3_Usuario usuario)
        {
            try
            {
                using (Sam2Context ctx2 = new Sam2Context())
                {
                    return null;
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

        public object ListadoGenerarDespacho(string id, Sam3_Usuario usuario)
        {
            try
            {
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                using (SamContext ctx = new SamContext())
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
                              join eqp in ctx.Sam3_EquivalenciaProyecto on p.ProyectoID equals eqp.Sam3_ProyectoID
                              where p.Activo && pa.Activo && eq.Activo && eqp.Activo
                              && proyectos.Contains(eqp.Sam2_ProyectoID)
                              select eq.Sam2_PatioID).Distinct().AsParallel().ToList();

                    patios = patios.Where(x => x > 0).ToList();



                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        int odtId = Convert.ToInt32(id);

                        List<int> ordenTrabajoSpools = (from odts in ctx2.OrdenTrabajoSpool
                                                        where odts.OrdenTrabajoID == odtId
                                                        select odts.OrdenTrabajoSpoolID).AsParallel().Distinct().ToList();

                        List<lstPredespachos> predespachos = (from pre in ctx.Sam3_PreDespacho
                                                              where pre.Activo
                                                              && ordenTrabajoSpools.Contains(pre.OrdenTrabajoSpoolID)
                                                              select new lstPredespachos
                                                              {
                                                                  OdtSpoolID = pre.OrdenTrabajoSpoolID,
                                                                  MaterialSpoolID = pre.MaterialSpoolID,
                                                                  NumeroUnicoID = pre.NumeroUnicoID,
                                                                  ProyectoID = pre.ProyectoID
                                                              }).AsParallel().Distinct().ToList();

                        if (predespachos != null && predespachos.Count > 0)
                        {
                            foreach (lstPredespachos lst in predespachos)
                            {
                                lst.NumeroControl = ctx2.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == lst.OdtSpoolID)
                                    .Select(x => x.NumeroControl).AsParallel().SingleOrDefault();

                                lst.ItemCode = (from ms in ctx2.MaterialSpool
                                                join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                where ms.MaterialSpoolID == lst.MaterialSpoolID
                                                select it.Codigo).AsParallel().SingleOrDefault();

                                lst.Descripcion = (from ms in ctx2.MaterialSpool
                                                   join it in ctx2.ItemCode on ms.ItemCodeID equals it.ItemCodeID
                                                   where ms.MaterialSpoolID == lst.MaterialSpoolID
                                                   select it.DescripcionEspanol).AsParallel().SingleOrDefault();
                                int sam2_numeroUnicoID = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == lst.NumeroUnicoID)
                                    .Select(x => x.Sam2_NumeroUnicoID).SingleOrDefault();

                                lst.NumeroUnico = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_numeroUnicoID)
                                    .Select(x => x.Codigo).AsParallel().SingleOrDefault();

                                lst.Etiqueta = ctx2.MaterialSpool.Where(x => x.MaterialSpoolID == lst.MaterialSpoolID)
                                    .Select(x => x.Etiqueta).AsParallel().SingleOrDefault();
                            }
                        }

                        List<int> numerosUnicosAprobadosSam2 = (from odtm in ctx2.OrdenTrabajoMaterial
                                                                join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                                join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                                join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                                where nu.Estatus == "A"
                                                                && odt.OrdenTrabajoID == odtId
                                                                //&& odtm.NumeroUnicoCongeladoID != null
                                                                select odtm.NumeroUnicoCongeladoID.Value).AsParallel().ToList();

                        numerosUnicosAprobadosSam2.AddRange((from odtm in ctx2.OrdenTrabajoMaterial
                                                             join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                             join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                             join nu in ctx2.NumeroUnico on odtm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                                             where nu.Estatus == "A"
                                                             && odt.OrdenTrabajoID == odtId
                                                             select odtm.NumeroUnicoDespachadoID.Value).AsParallel().ToList());

                        List<LstGenerarDespacho> listado = (from odts in ctx2.OrdenTrabajoSpool
                                                            join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                            join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                            join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                            join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                            join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                            where odts.OrdenTrabajoID == odtId
                                                            && proyectos.Contains(odt.ProyectoID)
                                                            && it.TipoMaterialID == 2
                                                            && numerosUnicosAprobadosSam2.Contains(nu.NumeroUnicoID)
                                                            select new LstGenerarDespacho
                                                            {
                                                                Descripcion = it.DescripcionEspanol,
                                                                ItemCode = it.Codigo,
                                                                NumeroControl = odts.NumeroControl,
                                                                //NumeroUnico = nu.Codigo,
                                                                Etiqueta = ms.Etiqueta,
                                                                Hold = (from sh in ctx2.SpoolHold
                                                                        where sh.SpoolID == odts.SpoolID
                                                                        && (sh.TieneHoldCalidad || sh.TieneHoldIngenieria || sh.Confinado)
                                                                        select sh).Any(),
                                                                ProyectoID = odt.ProyectoID.ToString(),
                                                                OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                                                                MaterialSpoolID = ms.MaterialSpoolID,
                                                                DespachoID = odtm.DespachoID.ToString()
                                                            }).Distinct().AsParallel().ToList();

                        listado.AddRange((from odts in ctx2.OrdenTrabajoSpool
                                          join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                          join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                          join nu in ctx2.NumeroUnico on odtm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                          join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                          join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                          where odts.OrdenTrabajoID == odtId
                                          && proyectos.Contains(odt.ProyectoID)
                                          && it.TipoMaterialID == 2
                                          && numerosUnicosAprobadosSam2.Contains(nu.NumeroUnicoID)
                                          select new LstGenerarDespacho
                                          {
                                              Descripcion = it.DescripcionEspanol,
                                              ItemCode = it.Codigo,
                                              NumeroControl = odts.NumeroControl,
                                              NumeroUnico = nu.Codigo,
                                              Etiqueta = ms.Etiqueta,
                                              Hold = (from sh in ctx2.SpoolHold
                                                      where sh.SpoolID == odts.SpoolID
                                                      && (sh.TieneHoldCalidad || sh.TieneHoldIngenieria || sh.Confinado)
                                                      select sh).Any(),
                                              ProyectoID = odt.ProyectoID.ToString(),
                                              DespachoID = odtm.DespachoID.ToString()
                                          }).Distinct().AsParallel().ToList());

                        //listado = listado.GroupBy(x => x.MaterialSpoolID).Select(x => x.First()).ToList();

                        if (predespachos != null && predespachos.Count > 0)
                        {
                            foreach (LstGenerarDespacho ls in listado)
                            {
                                int despachoID = ls.DespachoID != "" ? Convert.ToInt32(ls.DespachoID) : 0;
                                if (predespachos.Where(x => x.OdtSpoolID == ls.OrdenTrabajoSpoolID && x.MaterialSpoolID == ls.MaterialSpoolID).Any()
                                    && despachoID <= 0)
                                {
                                    lstPredespachos preDespacho = predespachos.Where(x => x.OdtSpoolID == ls.OrdenTrabajoSpoolID && x.MaterialSpoolID == ls.MaterialSpoolID)
                                        .SingleOrDefault();

                                    ls.NumeroUnico = preDespacho.NumeroUnico;
                                    ls.Descripcion = preDespacho.Descripcion;
                                    ls.ItemCode = preDespacho.ItemCode;
                                }
                            }
                        }

                        //eliminar numeros unicos que no se encuentren en sam3


#if DEBUG
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string json = serializer.Serialize(listado);
#endif

                        return listado.OrderBy(x => x.Etiqueta).ToList();
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


        public object GenerarDespachos(List<DespachoItems> despachos, Sam3_Usuario usuario)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Despacho nuevoDespacho = new Sam3_Despacho();
                int consecutivoNumeroUnico = 0;
                string prefijoNumeroUnico = "";
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        foreach (DespachoItems datosJson in despachos)
                        {
                            int despachoID = datosJson.DespachoID != "" ? Convert.ToInt32(datosJson.DespachoID) : 0;
                            int proyectoID = datosJson.ProyectoID != "" ? Convert.ToInt32(datosJson.ProyectoID) : 0;
                            

                            //Verificar si hay datos para seguir con el proceso
                            if (despachoID <= 0 && (datosJson.NumeroUnico == "" || datosJson.NumeroUnico == null))
                            {
                                //si no hay id de despacho o numero unico, no hay informacion con que continuar
                                continue;
                            }

                            // se borro el numero unico de este despacho
                            if (despachoID > 0 && (datosJson.NumeroUnico == "" || datosJson.NumeroUnico == null))
                            {
                                //eliminamos el despacho
                                bool resultado = EliminarDespacho(despachoID, usuario);
                                if (!resultado)
                                {
                                    throw new Exception("Error al eliminar el despacho No. " + despachoID);
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            if (despachoID > 0 && (datosJson.NumeroUnico != "" && datosJson.NumeroUnico != null))
                            {
                                string[] elementosCodigo = datosJson.NumeroUnico.Split('-').ToArray();
                                consecutivoNumeroUnico = Convert.ToInt32(elementosCodigo[1]);
                                prefijoNumeroUnico = elementosCodigo[0];
                                int sam3_ProyectoID = (from nueq in ctx.Sam3_EquivalenciaProyecto
                                                       where nueq.Activo && nueq.Sam2_ProyectoID == proyectoID
                                                       select nueq.Sam3_ProyectoID).AsParallel().SingleOrDefault();

                                Sam3_NumeroUnico tempnumeroUnico = null;
                                Sam3_Despacho despachoActual = ctx.Sam3_Despacho.Where(x => x.DespachoID == despachoID).AsParallel().SingleOrDefault();
                                if (ctx.Sam3_NumeroUnico.Where(x => x.Prefijo == prefijoNumeroUnico
                                && x.Consecutivo == consecutivoNumeroUnico && x.ProyectoID == sam3_ProyectoID).AsParallel().Any())
                                {
                                    tempnumeroUnico = ctx.Sam3_NumeroUnico.Where(x => x.Prefijo == prefijoNumeroUnico
                                    && x.Consecutivo == consecutivoNumeroUnico && x.ProyectoID == sam3_ProyectoID).AsParallel().SingleOrDefault();
                                }

                                if (tempnumeroUnico != null && despachoActual.NumeroUnicoID != tempnumeroUnico.NumeroUnicoID) // el numero unico seleccionado es diferente que el que se despacho
                                {
                                    bool resultado = EditarDespacho(despachoID, consecutivoNumeroUnico, prefijoNumeroUnico, usuario.UsuarioID);
                                    if (resultado)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        throw new Exception("Error al editar el despacho");
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }

                            //nuevo despacho
                            if (despachoID <= 0 && (datosJson.NumeroUnico != "" || datosJson.NumeroUnico != null))
                            {
                                string[] elementosCodigo = datosJson.NumeroUnico.Split('-').ToArray();
                                consecutivoNumeroUnico = Convert.ToInt32(elementosCodigo[1]);
                                prefijoNumeroUnico = elementosCodigo[0];
                                int sam3_ProyectoID = (from nueq in ctx.Sam3_EquivalenciaProyecto
                                                       where nueq.Activo && nueq.Sam2_ProyectoID == proyectoID
                                                       select nueq.Sam3_ProyectoID).AsParallel().SingleOrDefault();

                                int resultadoNuevoDespacho = NuevoDespacho(datosJson, proyectoID, sam3_ProyectoID, prefijoNumeroUnico, consecutivoNumeroUnico, usuario);
                                if (resultadoNuevoDespacho <= 0)
                                {
                                    throw new Exception("Error insertando despacho");
                                }
                            }

                        } // forech despacho
                    }
                } // using ctx

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(nuevoDespacho.DespachoID.ToString());
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

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Despacho> registros = new List<Sam3_Despacho>();

                    if (proyectoID > 0)
                    {
                        registros = (from d in ctx.Sam3_Despacho
                                     join p in ctx.Sam3_Proyecto on d.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where d.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(d.DespachoID)
                                     select d).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from d in ctx.Sam3_Despacho
                                     join p in ctx.Sam3_Proyecto on d.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where d.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && IDs.Contains(d.DespachoID)
                                     select d).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                     where p.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join rid in ctx.Sam3_Rel_Incidencia_Despacho on r.DespachoID equals rid.DespachoID
                               join inc in ctx.Sam3_Incidencia on rid.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               join d in ctx.Sam3_Despacho on rid.DespachoID equals d.DespachoID
                               where rid.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = c.Nombre,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   Estatus = inc.Estatus,
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where pc.Rel_Proyecto_Entidad_Configuracion_ID == inc.Rel_Proyecto_Entidad_Configuracion_ID
                                                                                                          select pc.PreFijoFolioIncidencias + ","
                                                                                                           + pc.CantidadCerosFolioIncidencias.ToString() + ","
                                                                                                           + inc.Consecutivo.ToString() + ","
                                                                                                           + pc.PostFijoFolioIncidencias).FirstOrDefault() : inc.IncidenciaID.ToString()
                               }).AsParallel().Distinct().ToList();

                    if (ActivarFolioConfiguracionIncidencias)
                    {
                        foreach (ListadoIncidencias item in listado)
                        {
                            if (!string.IsNullOrEmpty(item.FolioConfiguracionIncidencia))
                            {
                                string[] elemntos = item.FolioConfiguracionIncidencia.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                item.FolioConfiguracionIncidencia = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                            else
                            {
                                item.FolioConfiguracionIncidencia = item.FolioIncidenciaID.ToString();
                            }
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
                return null;
            }
        }

        public object ListadoDespachoDesdeImpresion(int materialSpoolID, Sam3_Usuario usuario)
        {
            try
            {
                List<object> resultado = new List<object>();
                List<int> proyectos = new List<int>();
                List<int> patios = new List<int>();
                using (SamContext ctx = new SamContext())
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



                    using (Sam2Context ctx2 = new Sam2Context())
                    {

                        int sam2_proyectoID = (from ms in ctx2.MaterialSpool
                                               join odts in ctx2.OrdenTrabajoSpool on ms.SpoolID equals odts.SpoolID
                                               join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                               join p in ctx2.Proyecto on odt.ProyectoID equals p.ProyectoID
                                               where ms.MaterialSpoolID == materialSpoolID
                                               select p.ProyectoID).AsParallel().SingleOrDefault();

                        int sam3_proyectoID = (from eq in ctx.Sam3_EquivalenciaProyecto
                                               where eq.Activo
                                               && eq.Sam2_ProyectoID == sam2_proyectoID
                                               select eq.Sam3_ProyectoID).AsParallel().SingleOrDefault();

                        resultado.Add((from p in ctx.Sam3_Proyecto
                                       where p.Activo && p.ProyectoID == sam3_proyectoID
                                       select new ListaCombos
                                       {
                                           id = p.ProyectoID.ToString(),
                                           value = p.Nombre
                                       }).AsParallel().Distinct().SingleOrDefault());

                        resultado.Add((from ms in ctx2.MaterialSpool
                                       join odts in ctx2.OrdenTrabajoSpool on ms.SpoolID equals odts.SpoolID
                                       join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                       join p in ctx2.Proyecto on odt.ProyectoID equals p.ProyectoID
                                       where ms.MaterialSpoolID == materialSpoolID
                                       select new ListaCombos
                                       {
                                           id = odts.OrdenTrabajoSpoolID.ToString(),
                                           value = odts.NumeroControl
                                       }).AsParallel().Distinct().SingleOrDefault());

                        LstGenerarDespacho listado = (from odts in ctx2.OrdenTrabajoSpool
                                                      join odtm in ctx2.OrdenTrabajoMaterial on odts.OrdenTrabajoSpoolID equals odtm.OrdenTrabajoSpoolID
                                                      join ms in ctx2.MaterialSpool on odtm.MaterialSpoolID equals ms.MaterialSpoolID
                                                      join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                      join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                      join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                      where ms.MaterialSpoolID == materialSpoolID
                                                      && proyectos.Contains(odt.ProyectoID)
                                                      && it.TipoMaterialID == 2
                                                      select new LstGenerarDespacho
                                                      {
                                                          Descripcion = it.DescripcionEspanol,
                                                          ItemCode = it.Codigo,
                                                          NumeroControl = odts.NumeroControl,
                                                          NumeroUnico = nu.Codigo,
                                                          Etiqueta = ms.Etiqueta,
                                                          Hold = (from sh in ctx2.SpoolHold
                                                                  where sh.SpoolID == odts.SpoolID
                                                                  && (sh.TieneHoldCalidad || sh.TieneHoldIngenieria || sh.Confinado)
                                                                  select sh).Any(),
                                                          ProyectoID = odt.ProyectoID.ToString()
                                                      }).Distinct().AsParallel().SingleOrDefault();

                        resultado.Add(listado);


#if DEBUG
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        string json = serializer.Serialize(resultado);
#endif

                        return resultado;
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

        private bool EditarDespacho(int despachoID, int consecutivoNumeroUnico, string prefijoNumeroUnico, int usuarioID)
        {
            try
            {
                LoggerBd.Instance.EscribirLog(string.Format(
                    @"Parametros de entrada para edicion de Despacho: \ndespachoID = {0},
                    \nConsecutivo = {1}, \nPrefijo= {2}",
                    despachoID, consecutivoNumeroUnico, prefijoNumeroUnico));

                //Sam3_Despacho nuevoDespacho = new Sam3_Despacho();
                using (SamContext ctx = new SamContext())
                {
                    using (var sam3_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var sam2_tran = ctx2.Database.BeginTransaction())
                            {
                                int salidaInventarioID = 0;
                                Sam3_Despacho despacho = ctx.Sam3_Despacho.Where(x => x.DespachoID == despachoID).AsParallel().SingleOrDefault();
                                //Nuevo numeroUnico
                                Sam3_NumeroUnico nuevo_NumeroUnicoSam3 = ctx.Sam3_NumeroUnico.Where(x => x.Consecutivo == consecutivoNumeroUnico
                                    && x.Prefijo == prefijoNumeroUnico).AsParallel().SingleOrDefault();

                                int sam2_NumeroUnicoNuevo = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == nuevo_NumeroUnicoSam3.NumeroUnicoID)
                                    .Select(x => x.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                if (despacho != null)
                                {
                                    int cantidadDespachada = despacho.Cantidad;
                                    Sam3_NumeroUnico numeroUnicoActual = ctx.Sam3_NumeroUnico.Where(x => x.NumeroUnicoID == despacho.NumeroUnicoID)
                                        .AsParallel().SingleOrDefault();

                                    if (nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado > 0)
                                    {
                                        // si el NU que se selecciono tiene congelados
                                        // le devolveremos el inventario al NU despachado y lo asignaremos a la odtm que tenia
                                        // congelado el NU seleccionado

                                        //buscamos la odtm que tiene congelado el NU seleccionado
                                        OrdenTrabajoMaterial odtmCongelada = (from odtm in ctx2.OrdenTrabajoMaterial
                                                                              where odtm.NumeroUnicoCongeladoID.Value == sam2_NumeroUnicoNuevo
                                                                              select odtm).AsParallel().SingleOrDefault();

                                        //a esta odtm le asiganamos como congelado el NU que se encontraba originalmente en el despahco
                                        int eqNU = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == numeroUnicoActual.NumeroUnicoID)
                                            .Select(x => x.Sam2_NumeroUnicoID).AsParallel().Distinct().SingleOrDefault();
                                        odtmCongelada.NumeroUnicoCongeladoID = eqNU;
                                        odtmCongelada.FechaModificacion = DateTime.Now;
                                        
                                        //ahora le devolvemos el inventario al NU actual
                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico =
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico + cantidadDespachada;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioBuenEstado =
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico - numeroUnicoActual.Sam3_NumeroUnicoInventario.CantidadDanada;

                                        //Agregamos el congelado
                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado = numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado + cantidadDespachada;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioBuenEstado -
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuarioID;
                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                        if (numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado < 0
                                            || numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }

                                        if ((from m in numeroUnicoActual.Sam3_NumeroUnicoMovimiento
                                             join tm in ctx.Sam3_TipoMovimiento on m.TipoMovimientoID equals tm.TipoMovimientoID
                                             
                                             select m).AsParallel().Any())
                                        {

                                            Sam3_NumeroUnicoMovimiento movimientoActual = (from m in numeroUnicoActual.Sam3_NumeroUnicoMovimiento
                                                                                           join tm in ctx.Sam3_TipoMovimiento on m.TipoMovimientoID equals tm.TipoMovimientoID
                                                                                           where tm.Nombre == "Despacho Accesorio" && m.Cantidad == cantidadDespachada
                                                                                           && m.NumeroUnicoMovimientoID == despacho.SalidaInventarioID
                                                                                           select m).AsParallel().SingleOrDefault();
                                            movimientoActual.Estatus = "C"; //cancelado
                                            movimientoActual.Activo = false;
                                            movimientoActual.Referencia += " . Se cancela por Edicion de despacho.";
                                            movimientoActual.UsuarioModificacion = usuarioID;
                                            movimientoActual.FechaModificacion = DateTime.Now;
                                            ctx.SaveChanges();
                                        }

                                        //-----------------------------------------------------------------------------------
                                        int sam2_NumeroUnicoActual = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == numeroUnicoActual.NumeroUnicoID)
                                            .Select(x => x.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();
                                        NumeroUnico NumeroUnicoActualSAM2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_NumeroUnicoActual).AsParallel().SingleOrDefault();
                                        //Actualizar numero unico de sam2 ---------------------------------------------------------------------------------------------------------------
                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico =
                                            NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico + cantidadDespachada;

                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioBuenEstado =
                                            NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico - NumeroUnicoActualSAM2.NumeroUnicoInventario.CantidadDanada;

                                        //agregamos el congelado
                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado = NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado + cantidadDespachada;

                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioDisponibleCruce = NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioBuenEstado
                                            - NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado;

                                        if (NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado < 0
                                            || NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }

                                        ctx.SaveChanges();
                                        ctx2.SaveChanges();

                                    }
                                    else
                                    {
                                        // si el nuevo NU no tiene congelados entonces devolvemos el inventario al NU que ya se habia despachado
                                        //Actualizar numero unico anterior--------------------------------------------------
                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico =
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico + cantidadDespachada;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioBuenEstado =
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioFisico - numeroUnicoActual.Sam3_NumeroUnicoInventario.CantidadDanada;

                                        //Quitamos el congelado
                                        //numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado = numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado - cantidadDespachada;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce = numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioBuenEstado -
                                            numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado;

                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuarioID;
                                        numeroUnicoActual.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                        if (numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioCongelado < 0
                                            || numeroUnicoActual.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }

                                        if ((from m in numeroUnicoActual.Sam3_NumeroUnicoMovimiento
                                             join tm in ctx.Sam3_TipoMovimiento on m.TipoMovimientoID equals tm.TipoMovimientoID
                                             where tm.Nombre == "Despacho Accesorio" && m.Cantidad == cantidadDespachada
                                             select m).AsParallel().Any())
                                        {

                                            Sam3_NumeroUnicoMovimiento movimientoActual = (from m in numeroUnicoActual.Sam3_NumeroUnicoMovimiento
                                                                                           join tm in ctx.Sam3_TipoMovimiento on m.TipoMovimientoID equals tm.TipoMovimientoID
                                                                                           where tm.Nombre == "Despacho Accesorio"
                                                                                           select m).AsParallel().SingleOrDefault();
                                            movimientoActual.Estatus = "C"; //cancelado
                                            movimientoActual.Activo = false;
                                            movimientoActual.Referencia += " . Se cancela por Edicion de despacho.";
                                            movimientoActual.UsuarioModificacion = usuarioID;
                                            movimientoActual.FechaModificacion = DateTime.Now;
                                        }

                                        //-----------------------------------------------------------------------------------
                                        int sam2_NumeroUnicoActual = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == numeroUnicoActual.NumeroUnicoID)
                                            .Select(x => x.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();
                                        NumeroUnico NumeroUnicoActualSAM2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_NumeroUnicoActual).AsParallel().SingleOrDefault();
                                        //Actualizar numero unico de sam2 ---------------------------------------------------------------------------------------------------------------
                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico =
                                            NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico + cantidadDespachada;

                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioBuenEstado =
                                            NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioFisico - NumeroUnicoActualSAM2.NumeroUnicoInventario.CantidadDanada;

                                        //Quitamos el congelado
                                        //NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado = NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado - cantidadDespachada;

                                        NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioDisponibleCruce = NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioBuenEstado
                                            - NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado;

                                        if (NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioCongelado < 0
                                            || NumeroUnicoActualSAM2.NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }

                                        ctx.SaveChanges();
                                        ctx2.SaveChanges();
                                    }
                                    ///-----------------------------------------------------------------------------------------------------------------------------------------------


                                    

                                    NumeroUnico nuevo_NumeroUnico = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_NumeroUnicoNuevo).AsParallel().SingleOrDefault();

                                    if (nuevo_NumeroUnico.NumeroUnicoInventario.InventarioFisico >= cantidadDespachada)
                                    {
                                        nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico =
                                            nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico - cantidadDespachada;

                                        nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioBuenEstado =
                                            nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioFisico - nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.CantidadDanada;

                                        if (nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado > 0)
                                        {
                                            nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado =
                                                nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado - cantidadDespachada;
                                        }

                                        nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce = nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioBuenEstado
                                            - nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado;

                                        nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuarioID;
                                        nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;

                                        if (nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioCongelado < 0 ||
                                            nuevo_NumeroUnicoSam3.Sam3_NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }

                                        nuevo_NumeroUnico.NumeroUnicoInventario.InventarioFisico =
                                            nuevo_NumeroUnico.NumeroUnicoInventario.InventarioFisico - cantidadDespachada;

                                        nuevo_NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado =
                                            nuevo_NumeroUnico.NumeroUnicoInventario.InventarioFisico - nuevo_NumeroUnico.NumeroUnicoInventario.CantidadDanada;

                                        if (nuevo_NumeroUnico.NumeroUnicoInventario.InventarioCongelado > 0)
                                        {
                                            nuevo_NumeroUnico.NumeroUnicoInventario.InventarioCongelado = nuevo_NumeroUnico.NumeroUnicoInventario.InventarioCongelado - cantidadDespachada;
                                        }

                                        nuevo_NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce = nuevo_NumeroUnico.NumeroUnicoInventario.InventarioBuenEstado
                                            - nuevo_NumeroUnico.NumeroUnicoInventario.InventarioCongelado;

                                        if (nuevo_NumeroUnico.NumeroUnicoInventario.InventarioCongelado < 0 ||
                                            nuevo_NumeroUnico.NumeroUnicoInventario.InventarioDisponibleCruce < 0)
                                        {
                                            throw new Exception("EL inventario no puede ser negativo");
                                        }


                                        Sam3_NumeroUnicoMovimiento nuevoMovimiento = new Sam3_NumeroUnicoMovimiento
                                        {
                                            Activo = true,
                                            Cantidad = cantidadDespachada,
                                            Estatus = "A",
                                            FechaModificacion = DateTime.Now,
                                            FechaMovimiento = DateTime.Now,
                                            NumeroUnicoID = nuevo_NumeroUnicoSam3.NumeroUnicoID,
                                            ProyectoID = nuevo_NumeroUnicoSam3.ProyectoID,
                                            Referencia = "Seleccionado en Edición de despacho. " + nuevo_NumeroUnico.Codigo,
                                            TipoMovimientoID = (from tm in ctx.Sam3_TipoMovimiento
                                                                where tm.Activo && tm.Nombre == "Despacho Accesorio"
                                                                select tm.TipoMovimientoID).FirstOrDefault(),
                                            UsuarioModificacion = usuarioID
                                        };
                                        ctx.Sam3_NumeroUnicoMovimiento.Add(nuevoMovimiento);
                                        ctx.SaveChanges();
                                        salidaInventarioID = nuevoMovimiento.NumeroUnicoMovimientoID;

                                    }
                                    else
                                    {
                                        throw new Exception("No esxiste suficiente material");
                                    }


                                    OrdenTrabajoMaterial odtmActual = ctx2.OrdenTrabajoMaterial.Where(x => x.OrdenTrabajoSpoolID == despacho.OrdenTrabajoSpoolID
                                        && x.MaterialSpoolID == despacho.MaterialSpoolID).AsParallel().SingleOrDefault();
                                    odtmActual.NumeroUnicoDespachadoID = nuevo_NumeroUnico.NumeroUnicoID;

                                    despacho.NumeroUnicoID = nuevo_NumeroUnicoSam3.NumeroUnicoID;
                                    despacho.UsuarioModificacion = usuarioID;
                                    despacho.FechaModificacion = DateTime.Now;
                                    if (salidaInventarioID > 0)
                                    {
                                        despacho.SalidaInventarioID = salidaInventarioID;
                                    }

                                    ctx.SaveChanges();
                                    ctx2.SaveChanges();

                                    sam3_tran.Commit();
                                    sam2_tran.Commit();
                                }// fin despacho = null
                                else
                                {
                                    throw new Exception("No se encontro el despacho relacionado");
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog("Error Edicion de Despacho");
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        private bool EliminarDespacho(int despachoID, Sam3_Usuario usuario)
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
                                Sam3_Despacho despacho = ctx.Sam3_Despacho.Where(x => x.DespachoID == despachoID && x.Activo).AsParallel().Distinct().SingleOrDefault();
                                despacho.Cancelado = true;
                                despacho.Activo = false;
                                despacho.FechaModificacion = DateTime.Now;
                                despacho.UsuarioModificacion = usuario.UsuarioID;

                                //devolver inventario a sam3
                                Sam3_NumeroUnicoInventario sam3_Inventario = ctx.Sam3_NumeroUnicoInventario.Where(x => x.NumeroUnicoID == despacho.NumeroUnicoID && x.Activo)
                                    .AsParallel().SingleOrDefault();
                                sam3_Inventario.InventarioFisico = sam3_Inventario.InventarioFisico + despacho.Cantidad;
                                sam3_Inventario.InventarioBuenEstado = sam3_Inventario.InventarioFisico - sam3_Inventario.CantidadDanada;
                                sam3_Inventario.InventarioCongelado = sam3_Inventario.InventarioCongelado + despacho.Cantidad;
                                sam3_Inventario.InventarioDisponibleCruce = sam3_Inventario.InventarioBuenEstado - sam3_Inventario.InventarioCongelado;
                                sam3_Inventario.FechaModificacion = DateTime.Now;
                                sam3_Inventario.UsuarioModificacion = usuario.UsuarioID;

                                //devolver inventario a sam2
                                int sam2_NUID = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == despacho.NumeroUnicoID && x.Activo)
                                    .Select(x => x.Sam2_NumeroUnicoID).AsParallel().Distinct().SingleOrDefault();
                                NumeroUnicoInventario sam2_Inventario = ctx2.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == sam2_NUID).AsParallel().SingleOrDefault();
                                sam2_Inventario.InventarioFisico = sam2_Inventario.InventarioFisico + despacho.Cantidad;
                                sam2_Inventario.InventarioBuenEstado = sam2_Inventario.InventarioFisico - sam2_Inventario.CantidadDanada;
                                sam2_Inventario.InventarioCongelado = sam2_Inventario.InventarioCongelado + despacho.Cantidad;
                                sam2_Inventario.InventarioDisponibleCruce = sam2_Inventario.InventarioBuenEstado - sam2_Inventario.InventarioCongelado;
                                sam2_Inventario.FechaModificacion = DateTime.Now;

                                //eliminar el movimiento de inventario
                                Sam3_NumeroUnicoMovimiento movimiento = ctx.Sam3_NumeroUnicoMovimiento.Where(x => x.NumeroUnicoMovimientoID == despacho.SalidaInventarioID && x.Activo)
                                    .AsParallel().Distinct().SingleOrDefault();
                                movimiento.Activo = false;
                                movimiento.Estatus = "C";
                                movimiento.FechaModificacion = DateTime.Now;
                                movimiento.UsuarioModificacion = usuario.UsuarioID;

                                //Actualizar la ODM
                                OrdenTrabajoMaterial odtm = ctx2.OrdenTrabajoMaterial.Where(x => x.DespachoID == despacho.DespachoID
                                    && x.MaterialSpoolID == despacho.MaterialSpoolID && x.OrdenTrabajoSpoolID == despacho.OrdenTrabajoSpoolID)
                                    .AsParallel().Distinct().SingleOrDefault();
                                odtm.NumeroUnicoCongeladoID = odtm.NumeroUnicoDespachadoID;
                                odtm.CantidadCongelada = odtm.CantidadDespachada;
                                odtm.TieneInventarioCongelado = true;
                                odtm.TieneDespacho = false;
                                odtm.CantidadDespachada = null;
                                odtm.NumeroUnicoDespachadoID = null;
                                odtm.DespachoID = null;
                                odtm.FechaModificacion = DateTime.Now;

                                //eliminar el detalle de picking ticket
                                Sam3_DetalleFolioPickingTicket detallePickingT = ctx.Sam3_DetalleFolioPickingTicket
                                    .Where(x => x.OrdenTrabajoMaterialID == odtm.OrdenTrabajoMaterialID
                                        && x.DespachoID == despacho.DespachoID
                                        && x.Activo).AsParallel().Distinct().SingleOrDefault();
                                detallePickingT.Activo = false;
                                detallePickingT.FechaModificacion = DateTime.Now;
                                detallePickingT.UsuarioModificacion = usuario.UsuarioID;

                                ctx.SaveChanges();
                                ctx2.SaveChanges();

                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog("Error al eliminar Despacho");
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        public bool TransferirCongelado(DespachoItems item, Sam3_Usuario usuario)
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
                                OrdenTrabajoMaterial odtm = (from otm in ctx2.OrdenTrabajoMaterial
                                                             join odts in ctx2.OrdenTrabajoSpool on otm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                             join ms in ctx2.MaterialSpool on otm.MaterialSpoolID equals ms.MaterialSpoolID
                                                             where odts.NumeroControl == item.NumeroControl
                                                             && ms.Etiqueta == item.Etiqueta
                                                             select otm).AsParallel().Distinct().SingleOrDefault();

                                    //Commits
                                    ctx_tran.Commit();
                                    ctx2_tran.Commit();
                            } // tran ctx2
                        } // ctx2
                    } // tran ctx
                } // ctx 

                return true;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog("Error en Transferencia de congelado");
                LoggerBd.Instance.EscribirLog(ex);
                return false;
            }
        }

        private int NuevoDespacho(DespachoItems datosJson, int proyectoID, int sam3_ProyectoID, string prefijoNumeroUnico, int consecutivoNumeroUnico, Sam3_Usuario usuario)
        {
            try
            {
                Sam3_Despacho nuevoDespacho = new Sam3_Despacho();
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        using (Sam2Context ctx2 = new Sam2Context())
                        {
                            using (var ctx2_tran = ctx2.Database.BeginTransaction())
                            {
                                //traemos los datos de la orden de trabajo spool de Sam2
                                OrdenTrabajoSpool odtSpool = (from odts in ctx2.OrdenTrabajoSpool
                                                              join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                              where odts.NumeroControl == datosJson.NumeroControl
                                                              && odt.ProyectoID == proyectoID
                                                              select odts).AsParallel().SingleOrDefault();

                                //traemos los datos del material de Sam 2
                                MaterialSpool materialSpool = (from ms in ctx2.MaterialSpool
                                                               join odts in ctx2.OrdenTrabajoSpool on ms.SpoolID equals odts.SpoolID
                                                               where odts.OrdenTrabajoSpoolID == odtSpool.OrdenTrabajoSpoolID
                                                               && ms.Etiqueta == datosJson.Etiqueta
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
                                    Sam3_NumeroUnico numeroUnico = ctx.Sam3_NumeroUnico.Where(x => x.Prefijo == prefijoNumeroUnico
                                    && x.Consecutivo == consecutivoNumeroUnico && x.ProyectoID == sam3_ProyectoID).AsParallel().SingleOrDefault();

                                    int sam2_numeroUnicoID = ctx.Sam3_EquivalenciaNumeroUnico
                                        .Where(x => x.Sam3_NumeroUnicoID == numeroUnico.NumeroUnicoID)
                                        .Select(x => x.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                    #region Transferencia congelados
                                    // el numero unico que se quiere despachar es diferente del NU que estaba congelado para él material
                                    if (odtMaterial.NumeroUnicoCongeladoID != sam2_numeroUnicoID) 
                                    {
                                        if (numeroUnico.Sam3_NumeroUnicoInventario.InventarioCongelado > 0)
                                        {
                                            // el NU seleccionado tiene un congelado en otro ODTM
                                            OrdenTrabajoMaterial odtmCongelada = ctx2.OrdenTrabajoMaterial.Where(x =>
                                                x.NumeroUnicoCongeladoID == sam2_numeroUnicoID).AsParallel().Distinct().SingleOrDefault();
                                            
                                            //intercambiamos los NU congelados entre las ODTM
                                            int nuCongeladoOriginal = odtMaterial.NumeroUnicoCongeladoID.HasValue ? odtMaterial.NumeroUnicoCongeladoID.Value : 0;
                                             
                                            //asinamos el NU seleccionado a la ODTM que se va a despachar
                                            // tratandose de accesorios la cantidad no cambia, esta sera de 1
                                            int eqNU = ctx.Sam3_EquivalenciaNumeroUnico.Where(x => x.Sam3_NumeroUnicoID == numeroUnico.NumeroUnicoID
                                                && x.Activo).Select(x => x.Sam2_NumeroUnicoID).AsParallel().Distinct().SingleOrDefault();

                                            odtMaterial.NumeroUnicoCongeladoID = eqNU;

                                            //el NU congelado original se lo asignamos a la odtm que tenia congelado el NU que se selecciono
                                            odtmCongelada.NumeroUnicoCongeladoID = nuCongeladoOriginal;

                                            ctx2.SaveChanges();

                                        }
                                    }
                                    #endregion

                                    

                                    //Actualizamos los inventarios del numero unico en sam 3
                                    Sam3_NumeroUnicoInventario numInventario = ctx.Sam3_NumeroUnicoInventario
                                        .Where(x => x.NumeroUnicoID == numeroUnico.NumeroUnicoID).AsParallel().SingleOrDefault();

                                    numInventario.InventarioFisico = numInventario.InventarioFisico - odtMaterial.CantidadCongelada.Value;
                                    numInventario.InventarioBuenEstado = numInventario.InventarioFisico - numInventario.CantidadDanada;
                                    numInventario.InventarioCongelado = numInventario.InventarioCongelado - odtMaterial.CantidadCongelada.Value;
                                    numInventario.InventarioDisponibleCruce = numInventario.InventarioBuenEstado - numInventario.InventarioCongelado;

                                    numInventario.FechaModificacion = DateTime.Now;
                                    numInventario.UsuarioModificacion = usuario.UsuarioID;

                                    if (numInventario.InventarioCongelado < 0 || numInventario.InventarioDisponibleCruce < 0)
                                    {
                                        throw new Exception("El inventario no puede ser negativo");
                                    }

                                    //generamos el nuevo movimiento de inventario
                                    Sam3_NumeroUnicoMovimiento movimientoSam3 = new Sam3_NumeroUnicoMovimiento();
                                    movimientoSam3.Activo = true;
                                    movimientoSam3.Cantidad = odtMaterial.CantidadCongelada.Value;
                                    movimientoSam3.Estatus = "A";
                                    movimientoSam3.FechaModificacion = DateTime.Now;
                                    movimientoSam3.FechaMovimiento = DateTime.Now;
                                    movimientoSam3.NumeroUnicoID = numeroUnico.NumeroUnicoID;
                                    movimientoSam3.ProyectoID = numeroUnico.ProyectoID;
                                    movimientoSam3.Referencia = odtSpool.NumeroControl;
                                    movimientoSam3.Segmento = null;
                                    movimientoSam3.TipoMovimientoID = (from tp in ctx.Sam3_TipoMovimiento
                                                                       where tp.Nombre == "Despacho Accesorio"
                                                                       select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                    movimientoSam3.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_NumeroUnicoMovimiento.Add(movimientoSam3);
                                    ctx.SaveChanges(); // guardamos los cambios para obtener el id del movimiento de inventario

                                    int salidaInventarioSam3 = movimientoSam3.NumeroUnicoMovimientoID;

                                    //generamos el despacho en sam3
                                    nuevoDespacho.Activo = true;
                                    nuevoDespacho.Cancelado = false;
                                    nuevoDespacho.Cantidad = odtMaterial.CantidadCongelada != null ? odtMaterial.CantidadCongelada.Value : 1;
                                    nuevoDespacho.EsEquivalente = odtMaterial.CongeladoEsEquivalente;
                                    nuevoDespacho.FechaDespacho = DateTime.Now;
                                    nuevoDespacho.FechaModificacion = DateTime.Now;
                                    nuevoDespacho.MaterialSpoolID = odtMaterial.MaterialSpoolID;
                                    nuevoDespacho.NumeroUnicoID = numeroUnico.NumeroUnicoID;
                                    nuevoDespacho.OrdenTrabajoSpoolID = odtSpool.OrdenTrabajoSpoolID;
                                    nuevoDespacho.ProyectoID = sam3_ProyectoID;
                                    nuevoDespacho.SalidaInventarioID = salidaInventarioSam3;
                                    nuevoDespacho.Segmento = null;
                                    nuevoDespacho.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Despacho.Add(nuevoDespacho);
                                    ctx.SaveChanges();// guardamos el nuevo despacho

                                    #region eliminar pre despacho
                                    Sam3_PreDespacho preDespacho = (from pre in ctx.Sam3_PreDespacho
                                                                    where pre.Activo
                                                                    && pre.OrdenTrabajoSpoolID == nuevoDespacho.OrdenTrabajoSpoolID
                                                                    && pre.MaterialSpoolID == nuevoDespacho.MaterialSpoolID
                                                                    && pre.ProyectoID == nuevoDespacho.ProyectoID
                                                                    select pre).AsParallel().SingleOrDefault();

                                    if (preDespacho != null)
                                    {
                                        preDespacho.Activo = false;
                                        preDespacho.FechaModificacion = DateTime.Now;
                                        preDespacho.UsuarioModificacion = usuario.UsuarioID;

                                        ctx.SaveChanges();
                                    }
                                    #endregion

                                    #region Generar Picking Ticket
                                    Sam3_FolioPickingTicket nuevoPickingTicket = new Sam3_FolioPickingTicket();

                                    //verificamos si existe el Picking ticket
                                    if (!ctx.Sam3_FolioPickingTicket.Where(x => x.OrdenTrabajoSpoolID == odtMaterial.OrdenTrabajoSpoolID && x.Activo).AsParallel().Any())
                                    {
                                        //si no existe creamos el folio

                                        nuevoPickingTicket.Activo = true;
                                        nuevoPickingTicket.FechaModificacion = DateTime.Now;
                                        nuevoPickingTicket.usuarioModificacion = usuario.UsuarioID;
                                        nuevoPickingTicket.OrdenTrabajoSpoolID = odtMaterial.OrdenTrabajoSpoolID;

                                        ctx.Sam3_FolioPickingTicket.Add(nuevoPickingTicket);
                                        ctx.SaveChanges();

                                    }
                                    else
                                    {
                                        // traemos el registro del folio
                                        nuevoPickingTicket = ctx.Sam3_FolioPickingTicket.Where(x => x.OrdenTrabajoSpoolID == odtMaterial.OrdenTrabajoSpoolID && x.Activo).AsParallel().SingleOrDefault();
                                    }
                                    LoggerBd.Instance.EscribirLog("Este es el despacho creado---------------------------------------");
                                    LoggerBd.Instance.EscribirLog("DespachoID= " + nuevoDespacho.DespachoID.ToString());
                                    LoggerBd.Instance.EscribirLog("Este es el folio picking ticket-------------------------------------");
                                    LoggerBd.Instance.EscribirLog("Folio PK=" + nuevoPickingTicket.FolioPickingTicketID.ToString());
                                    LoggerBd.Instance.EscribirLog("contexto: " + "Hay CAmbios= " +
                                        ctx.ChangeTracker.HasChanges().ToString() +
                                        "Estado= " + ctx.Database.Connection.State.ToString());
                                    //Buscamos el detalle
                                    if (!ctx.Sam3_DetalleFolioPickingTicket.Where(x => x.OrdenTrabajoMaterialID == odtMaterial.OrdenTrabajoMaterialID 
                                        && x.DespachoID == nuevoDespacho.DespachoID && x.Activo).AsParallel().Any())
                                    {
                                        // si no existe lo agregamo
                                        Sam3_DetalleFolioPickingTicket nuevoDetalleFolioPK = new Sam3_DetalleFolioPickingTicket();
                                        LoggerBd.Instance.EscribirLog("Comienza crear detalle de picking ticket");
                                        nuevoDetalleFolioPK.Activo = true;
                                        nuevoDetalleFolioPK.FechaModificacion = DateTime.Now;
                                        nuevoDetalleFolioPK.OrdenTrabajoMaterialID = odtMaterial.OrdenTrabajoMaterialID;
                                        nuevoDetalleFolioPK.FolioPickingTicketID = nuevoPickingTicket.FolioPickingTicketID;
                                        nuevoDetalleFolioPK.UsuarioModificacion = usuario.UsuarioID;
                                        nuevoDetalleFolioPK.DespachoID = nuevoDespacho.DespachoID;


                                        LoggerBd.Instance.EscribirLog(string.Format(
                                            "ODTM= {0}, FPK= {1}, DespachoID= {2}",
                                            nuevoDetalleFolioPK.OrdenTrabajoMaterialID,
                                            nuevoDetalleFolioPK.FolioPickingTicketID,
                                            nuevoDetalleFolioPK.DespachoID
                                            ));

                                        ctx.Sam3_DetalleFolioPickingTicket.Add(nuevoDetalleFolioPK);
                                        LoggerBd.Instance.EscribirLog("contexto: " + "Hay CAmbios= " +
                                        ctx.ChangeTracker.HasChanges().ToString() +
                                        "Estado= " + ctx.Database.Connection.State.ToString());

                                        ctx.SaveChanges();
                                    }
                                    else
                                    {
                                        // no hay implementación para el caso de existir el detalle
                                    }

                                    #endregion

                                    //HAY QUE HACER EL MISMO PROCESO CON LOS INVENTARIOS DE SAM 2
                                    //buscamos su inventario
                                    NumeroUnicoInventario inventarioSam2 = ctx2.NumeroUnicoInventario
                                        .Where(x => x.NumeroUnicoID == sam2_numeroUnicoID).AsParallel().SingleOrDefault();

                                    //Actualizamos
                                    inventarioSam2.FechaModificacion = DateTime.Now;
                                    inventarioSam2.InventarioFisico = inventarioSam2.InventarioFisico - odtMaterial.CantidadCongelada.Value;
                                    inventarioSam2.InventarioBuenEstado = inventarioSam2.InventarioFisico - inventarioSam2.CantidadDanada;
                                    inventarioSam2.InventarioCongelado = inventarioSam2.InventarioCongelado - odtMaterial.CantidadCongelada.Value;
                                    inventarioSam2.InventarioDisponibleCruce = inventarioSam2.InventarioBuenEstado - inventarioSam2.InventarioCongelado;


                                    if (inventarioSam2.InventarioCongelado > 0 || inventarioSam2.InventarioDisponibleCruce > 0)
                                    {
                                        throw new Exception("El inventario no puede ser negativo");
                                    }

                                    //generamos el movimiento de inventario
                                    DatabaseManager.Sam2.NumeroUnicoMovimiento nuevoMovSam2 = new DatabaseManager.Sam2.NumeroUnicoMovimiento();
                                    nuevoMovSam2.Cantidad = odtMaterial.CantidadCongelada.Value;
                                    nuevoMovSam2.Estatus = "A";
                                    nuevoMovSam2.FechaModificacion = DateTime.Now;
                                    nuevoMovSam2.FechaMovimiento = DateTime.Now;
                                    nuevoMovSam2.NumeroUnicoID = sam2_numeroUnicoID;
                                    nuevoMovSam2.ProyectoID = proyectoID;
                                    nuevoMovSam2.Referencia = odtSpool.NumeroControl;
                                    nuevoMovSam2.Segmento = null;
                                    nuevoMovSam2.TipoMovimientoID = (from tp in ctx2.TipoMovimiento
                                                                     where tp.Nombre == "Despacho Accesorio"
                                                                     select tp.TipoMovimientoID).AsParallel().SingleOrDefault();
                                    //guardamos los cambios
                                    ctx2.NumeroUnicoMovimiento.Add(nuevoMovSam2);
                                    ctx2.SaveChanges();

                                    int salidaInventarioSam2 = nuevoMovSam2.NumeroUnicoMovimientoID;

                                    //verificamos si el numero unico que se despacho es igual al que esta congelado en la orden de trabajo material
                                    if (odtMaterial.NumeroUnicoCongeladoID == sam2_numeroUnicoID)
                                    {
                                        //si el numero unico despacho es igual al que se tenia congelado se actualiza la odtm
                                        odtMaterial.CantidadDespachada = nuevoDespacho.Cantidad;
                                        odtMaterial.CantidadCongelada = nuevoDespacho.Cantidad;
                                        odtMaterial.DespachoEsEquivalente = nuevoDespacho.EsEquivalente;
                                        odtMaterial.DespachoID = nuevoDespacho.DespachoID;
                                        odtMaterial.FechaModificacion = DateTime.Now;
                                        odtMaterial.NumeroUnicoCongeladoID = null;
                                        odtMaterial.NumeroUnicoDespachadoID = sam2_numeroUnicoID;
                                        odtMaterial.TieneDespacho = true;
                                        odtMaterial.TieneInventarioCongelado = false;

                                        //guardamos los cambios en sam2
                                        ctx2.SaveChanges();

                                    }
                                    else
                                    {
                                        //si el numero unico es diferente antes de actualizar la odtm hay que regresar a inventario el 
                                        //material que estaba congelado
                                        //Buscamos el numero unico que estaba congelado
                                        DatabaseManager.Sam2.NumeroUnicoInventario numeroCongelado = ctx2.NumeroUnicoInventario
                                            .Where(x => x.NumeroUnicoID == odtMaterial.NumeroUnicoCongeladoID).AsParallel().SingleOrDefault();
                                        //actualizamos
                                        numeroCongelado.FechaModificacion = DateTime.Now;
                                        //numeroCongelado.InventarioBuenEstado = numeroCongelado.InventarioBuenEstado + odtMaterial.CantidadCongelada.Value;
                                        numeroCongelado.InventarioCongelado = numeroCongelado.InventarioCongelado - odtMaterial.CantidadCongelada.Value;
                                        numeroCongelado.InventarioDisponibleCruce = numeroCongelado.InventarioBuenEstado - numeroCongelado.InventarioCongelado;
                                        //numeroCongelado.InventarioFisico += odtMaterial.CantidadCongelada.Value;

                                        //ahora si actualizamos la odtm
                                        //si el numero unico despacho es igual al que se tenia congelado se actualiza la odtm
                                        odtMaterial.CantidadDespachada = nuevoDespacho.Cantidad;
                                        odtMaterial.CantidadCongelada = odtMaterial.CantidadCongelada - nuevoDespacho.Cantidad;
                                        odtMaterial.DespachoEsEquivalente = nuevoDespacho.EsEquivalente;
                                        odtMaterial.DespachoID = nuevoDespacho.DespachoID;
                                        odtMaterial.FechaModificacion = DateTime.Now;
                                        odtMaterial.NumeroUnicoCongeladoID = null;
                                        odtMaterial.NumeroUnicoDespachadoID = sam2_numeroUnicoID;
                                        odtMaterial.TieneDespacho = true;
                                        odtMaterial.TieneInventarioCongelado = false;

                                        //guardamos los cambios en sam2
                                        ctx2.SaveChanges();
                                    }
                                }
                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            }
                        }
                    }
                }

                return nuevoDespacho.DespachoID;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog("Error en insercion de despacho");
                LoggerBd.Instance.EscribirLog(ex);
                return 0;
            }
        }
    }
}