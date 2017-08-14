using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseManager.Sam3;
using DatabaseManager.Sam2;
using DatabaseManager.EntidadesPersonalizadas;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;
using BackEndSAM.Models;
using SecurityManager.Api.Models;
using System.Transactions;
using System.Globalization;
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    public class NumeroUnicoBd
    {
        private static readonly object _mutex = new object();
        private static NumeroUnicoBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private NumeroUnicoBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static NumeroUnicoBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new NumeroUnicoBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerInfoEtiquetas(int ordenRecepcionID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int numeroDigitos = (from r in ctx.Sam3_OrdenRecepcion
                                         join rel in ctx.Sam3_Rel_OrdenRecepcion_ItemCode on r.OrdenRecepcionID equals rel.OrdenRecepcionID
                                         join i in ctx.Sam3_ItemCode on rel.ItemCodeID equals i.ItemCodeID
                                         join p in ctx.Sam3_ProyectoConfiguracion on i.ProyectoID equals p.ProyectoID
                                         where r.Activo && rel.Activo && i.Activo
                                         && r.OrdenRecepcionID == ordenRecepcionID
                                         select p.DigitosNumeroUnico).AsParallel().FirstOrDefault();

                    string formato = "D" + numeroDigitos.ToString();


                    List<InfoEtiquetaNumeroUnico> etiquetas = (from r in ctx.Sam3_NumeroUnico
                                                               join o in ctx.Sam3_Rel_OrdenRecepcion_ItemCode on r.ItemCodeID equals o.ItemCodeID
                                                               join it in ctx.Sam3_ItemCode on o.ItemCodeID equals it.ItemCodeID
                                                               join c in ctx.Sam3_Colada on r.ColadaID equals c.ColadaID
                                                               join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                                               where r.Activo && o.Activo
                                                               && o.OrdenRecepcionID == ordenRecepcionID
                                                               select new InfoEtiquetaNumeroUnico
                                                               {
                                                                   Cedula = r.Cedula,
                                                                   Certificado = c.NumeroCertificado,
                                                                   Colada = c.NumeroColada,
                                                                   Descripcion = it.DescripcionEspanol,
                                                                   Diametro1 = r.Diametro1.ToString(),
                                                                   Diametro2 = r.Diametro2.ToString(),
                                                                   ItemCode = it.Codigo,
                                                                   NumeroUnico = r.Prefijo + "-" + r.Consecutivo.ToString(),
                                                                   Proyecto = p.Nombre
                                                               }).AsParallel().ToList();

                    foreach (InfoEtiquetaNumeroUnico et in etiquetas)
                    {
                        List<string> elementos = et.NumeroUnico.Split('-').ToList();
                        int folio = Convert.ToInt32(elementos[1]);
                        et.NumeroUnico = elementos[0] + '-' + folio.ToString(formato);
                    }

#if DEBUG
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    string json = serializer.Serialize(etiquetas);
#endif

                    return etiquetas;
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

        public object ListadoNumerosUnicosCorte(int proyectoID, string busqueda, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaComboNumeroUnicoCOrte> listado = new List<ListaComboNumeroUnicoCOrte>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        int sam2_proyectoID = ctx.Sam3_EquivalenciaProyecto.Where(x => x.Sam3_ProyectoID == proyectoID)
                            .Select(x => x.Sam2_ProyectoID).AsParallel().SingleOrDefault();

                        //Busco los numeros unicos con despachos pendientes en sam 2
                        List<int> sam2_NumerosUnicos = (from nu in ctx2.NumeroUnico
                                                        join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                        join odtm in ctx2.OrdenTrabajoMaterial on nu.NumeroUnicoID equals odtm.NumeroUnicoDespachadoID
                                                        join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                        where nu.ProyectoID == sam2_proyectoID
                                                        && !(from shDespachado in ctx2.SpoolHold
                                                             where shDespachado.SpoolID == odts.SpoolID
                                                             && (shDespachado.Confinado || shDespachado.TieneHoldCalidad || shDespachado.TieneHoldIngenieria)
                                                             select shDespachado).Any()
                                                        && it.TipoMaterialID == 1
                                                        && nu.Codigo.Contains(busqueda)
                                                        && nu.Estatus == "A"
                                                        select nu.NumeroUnicoID).Distinct().AsParallel().ToList();

                        sam2_NumerosUnicos.AddRange((from nu in ctx2.NumeroUnico
                                                     join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                     join odtm in ctx2.OrdenTrabajoMaterial on nu.NumeroUnicoID equals odtm.NumeroUnicoCongeladoID
                                                     join odts in ctx2.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                                                     where nu.ProyectoID == sam2_proyectoID
                                                     && !(from shCongelado in ctx2.SpoolHold
                                                          where shCongelado.SpoolID == odts.SpoolID
                                                          && (shCongelado.Confinado || shCongelado.TieneHoldCalidad || shCongelado.TieneHoldIngenieria)
                                                          select shCongelado).Any()
                                                     && it.TipoMaterialID == 1
                                                        && nu.Codigo.Contains(busqueda)
                                                        && nu.Estatus == "A"
                                                     select nu.NumeroUnicoID).Distinct().AsParallel().ToList());


                        //ahora buscamos las equivalencias de esos numeros unicos en sam 3

                        List<int> sam3_NumerosUnicos = (from nueq in ctx.Sam3_EquivalenciaNumeroUnico
                                                        where nueq.Activo
                                                        && sam2_NumerosUnicos.Contains(nueq.Sam2_NumeroUnicoID)
                                                        select nueq.Sam3_NumeroUnicoID).Distinct().AsParallel().ToList();

                        listado = (from nu in ctx.Sam3_NumeroUnico
                                   join nus in ctx.Sam3_NumeroUnicoSegmento on nu.NumeroUnicoID equals nus.NumeroUnicoID
                                   where nu.Activo && nus.Activo && nu.Estatus == "A"
                                   && sam3_NumerosUnicos.Contains(nu.NumeroUnicoID)
                                   select new ListaComboNumeroUnicoCOrte
                                   {
                                       NumeroControlID = nu.NumeroUnicoID.ToString(),
                                       NumeroControl = nu.Prefijo + "-" + nu.Consecutivo + "-" + nus.Segmento
                                   }).Distinct().AsParallel().ToList();

                        Sam3_ProyectoConfiguracion configuracion = ctx.Sam3_ProyectoConfiguracion.Where(x => x.ProyectoID == proyectoID).AsParallel().SingleOrDefault();

                        foreach (ListaComboNumeroUnicoCOrte lst in listado)
                        {
                            string formato = "D" + configuracion.DigitosNumeroUnico.ToString();
                            string[] elementos = lst.NumeroControl.Split('-').ToArray();
                            int consecutivo = Convert.ToInt32(elementos[1]);
                            string codigo = elementos[0] + "-" + consecutivo.ToString(formato) + "-" + elementos[2];
                            lst.NumeroControl = codigo;
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

        public object DetalleNumeroUnicoCorte(string prefijo, int consecutivo, string segmento, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    DetalleNumeroUnicoCorte detalle = (from nu in ctx.Sam3_NumeroUnico
                            join nui in ctx.Sam3_NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                            join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                            join pc in ctx.Sam3_ProyectoConfiguracion on nu.ProyectoID equals pc.ProyectoID
                            where nu.Activo && nui.Activo && it.Activo && pc.Activo
                            && nu.Prefijo == prefijo
                            && nu.Consecutivo == consecutivo
                            select new DetalleNumeroUnicoCorte
                            {
                                Cantidad = nui.InventarioFisico.ToString(),
                                D1 = nu.Diametro1.ToString(),
                                ItemCode = it.Codigo,
                                Tolerancia = pc.ToleranciaCortes.Value.ToString()
                            }).Distinct().AsParallel().SingleOrDefault();

                    detalle.ListadoCortes = (List<DatosBusquedaODT>)CorteBd.Instance.ListadoGenerarCorte(prefijo, consecutivo, segmento, usuario);

                    return detalle; 
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

        public object ListadoNumeroUnicoComboGridDespacho(string numeroControl, string etiqueta, string itemcode, int proyectoID, Sam3_Usuario usuario)
        {
            try
            {
                List<ListaCombos> listado = new List<ListaCombos>();
                using (SamContext ctx = new SamContext())
                {
                    using (Sam2Context ctx2 = new Sam2Context())
                    {
                        MaterialSpool materialSpool = (from ms in ctx2.MaterialSpool
                                            join odts in ctx2.OrdenTrabajoSpool on ms.SpoolID equals odts.SpoolID
                                            join s in ctx2.Spool on odts.SpoolID equals s.SpoolID
                                            where ms.Etiqueta == etiqueta && odts.NumeroControl == numeroControl && s.ProyectoID == proyectoID
                                            select ms).AsParallel().Distinct().SingleOrDefault();

                        List<int> sam2_NumerosUnicosIDs = (from odts in ctx2.OrdenTrabajoSpool
                                                           join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                           join ms in ctx2.MaterialSpool on odts.SpoolID equals ms.SpoolID
                                                           join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                                                           join nu in ctx2.NumeroUnico on odtm.NumeroUnicoDespachadoID equals nu.NumeroUnicoID
                                                           join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                           join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                           where ms.Etiqueta == etiqueta
                                                           && odts.NumeroControl == numeroControl
                                                           && it.TipoMaterialID == 2
                                                           && it.Codigo == itemcode
                                                           && odt.ProyectoID == proyectoID
                                                           && ms.Diametro1 == nu.Diametro1
                                                           && ms.Diametro2 == nu.Diametro2
                                                           && nu.Estatus == "A"
                                                           select nu.NumeroUnicoID).Distinct().AsParallel().ToList();

                        sam2_NumerosUnicosIDs.AddRange((from odts in ctx2.OrdenTrabajoSpool
                                                        join odt in ctx2.OrdenTrabajo on odts.OrdenTrabajoID equals odt.OrdenTrabajoID
                                                        join ms in ctx2.MaterialSpool on odts.SpoolID equals ms.SpoolID
                                                        join odtm in ctx2.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                                                        join nu in ctx2.NumeroUnico on odtm.NumeroUnicoCongeladoID equals nu.NumeroUnicoID
                                                        join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                        join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                        where ms.Etiqueta == etiqueta
                                                        && odts.NumeroControl == numeroControl
                                                        && it.TipoMaterialID == 2
                                                        && it.Codigo == itemcode
                                                        && odt.ProyectoID == proyectoID
                                                        && ms.Diametro1 == nu.Diametro1
                                                        && ms.Diametro2 == nu.Diametro2
                                                        && nu.Estatus == "A"
                                                        select nu.NumeroUnicoID).Distinct().AsParallel().ToList());

                        sam2_NumerosUnicosIDs.AddRange((from nu in ctx2.NumeroUnico
                                                        join nui in ctx2.NumeroUnicoInventario on nu.NumeroUnicoID equals nui.NumeroUnicoID
                                                        join it in ctx2.ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                        where nu.Estatus == "A"
                                                        && it.Codigo == itemcode
                                                        && it.TipoMaterialID == 2
                                                        && nu.ProyectoID == proyectoID
                                                        && nu.Diametro1 == materialSpool.Diametro1
                                                        && nu.Diametro2 == materialSpool.Diametro2
                                                        && (nui.InventarioDisponibleCruce >= materialSpool.Cantidad
                                                        || nui.InventarioCongelado >= materialSpool.Cantidad)
                                                        select nu.NumeroUnicoID).AsParallel().ToList());
                                                

                        listado = (from nu in ctx.Sam3_NumeroUnico
                                   join nueq in ctx.Sam3_EquivalenciaNumeroUnico on nu.NumeroUnicoID equals nueq.Sam3_NumeroUnicoID
                                   where nu.Activo && nueq.Activo
                                   && sam2_NumerosUnicosIDs.Contains(nueq.Sam2_NumeroUnicoID)
                                   && nu.EstatusFisico == "Aprobado" && nu.EstatusDocumental == "Aprobado"
                                   select new ListaCombos
                                   {
                                       id = nu.NumeroUnicoID.ToString(),
                                       value = nu.Prefijo + "-" + nu.Consecutivo
                                   }).Distinct().AsParallel().ToList();

                        foreach (ListaCombos lst in listado)
                        {
                            string[] elementos = lst.value.Split('-');

                            int temp = Convert.ToInt32(lst.id);

                            int digitos = (from nu in ctx.Sam3_NumeroUnico
                                           join p in ctx.Sam3_ProyectoConfiguracion on nu.ProyectoID equals p.ProyectoID
                                           where nu.Activo && p.Activo
                                           && nu.NumeroUnicoID == temp
                                           select p.DigitosNumeroUnico).AsParallel().SingleOrDefault();

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

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios, List<int> IDs)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;
                List<ListadoIncidencias> listado;
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_NumeroUnico> registros = new List<Sam3_NumeroUnico>();

                    if (proyectoID > 0)
                    {
                        registros = (from nu in ctx.Sam3_NumeroUnico
                                     join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where nu.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && IDs.Contains(nu.NumeroUnicoID)
                                     select nu).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from nu in ctx.Sam3_NumeroUnico
                                     join p in ctx.Sam3_Proyecto on nu.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where nu.Activo && p.Activo && pa.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && IDs.Contains(nu.NumeroUnicoID)
                                     select nu).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join p in ctx.Sam3_Proyecto on r.ProyectoID equals p.ProyectoID
                                     where p.Activo
                                     && p.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join rin in ctx.Sam3_Rel_Incidencia_NumeroUnico on r.NumeroUnicoID equals rin.NumeroUnicoID
                               join inc in ctx.Sam3_Incidencia on rin.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rin.Activo && inc.Activo && c.Activo && tpi.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   TipoIncidencia = tpi.Nombre,
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioConfiguracionIncidencia = ActivarFolioConfiguracionIncidencias ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                                                          where  pc.Rel_Proyecto_Entidad_Configuracion_ID==inc.Rel_Proyecto_Entidad_Configuracion_ID
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
                            else {
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

        public object EliminarNumeroUnico(int numeroUnicoID, Sam3_Usuario usuario)
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
                                int sam2_NumeroUnicoID = (from eq in ctx.Sam3_EquivalenciaNumeroUnico
                                                          where eq.Activo && eq.Sam3_NumeroUnicoID == numeroUnicoID
                                                          select eq.Sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                NumeroUnico NumeroUnicoSam2 = ctx2.NumeroUnico.Where(x => x.NumeroUnicoID == sam2_NumeroUnicoID).AsParallel().SingleOrDefault();

                                //buscamos si tiene procesos en ODTM
                                bool tieneProcesos = (from odtm in ctx2.OrdenTrabajoMaterial
                                                        where odtm.NumeroUnicoCongeladoID == NumeroUnicoSam2.NumeroUnicoID
                                                        || odtm.NumeroUnicoDespachadoID == NumeroUnicoSam2.NumeroUnicoID
                                                        || odtm.NumeroUnicoSugeridoID == NumeroUnicoSam2.NumeroUnicoID
                                                        select odtm).Any();

                                bool tieneInventarioCongelado = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                                    where nui.NumeroUnicoID == NumeroUnicoSam2.NumeroUnicoID
                                                                    select nui.InventarioCongelado).AsParallel().SingleOrDefault() > 0 ? true : false;

                                bool tieneInventarioTransferenciaCorte = (from nui in ctx.Sam3_NumeroUnicoInventario
                                                                          where nui.NumeroUnicoID == NumeroUnicoSam2.NumeroUnicoID
                                                                          select nui.InventarioTransferenciaCorte).AsParallel().SingleOrDefault() > 0 ? true : false;

                                if (!tieneProcesos && !tieneInventarioCongelado && !tieneInventarioTransferenciaCorte)
                                {
                                    //Para eliminar el numero unico de sam 2 solo hay que ponerlo en estatus C
                                    NumeroUnicoSam2.Estatus = "C";
                                    NumeroUnicoSam2.FechaModificacion = DateTime.Now;
                                    ctx2.SaveChanges();

                                    Sam3_NumeroUnico numUnicoS3 = ctx.Sam3_NumeroUnico.Where(x => x.NumeroUnicoID == numeroUnicoID).AsParallel().SingleOrDefault();
                                    numUnicoS3.Activo = false;
                                    numUnicoS3.FechaModificacion = DateTime.Now;
                                    numUnicoS3.UsuarioModificacion = usuario.UsuarioID;

                                    numUnicoS3.Sam3_NumeroUnicoInventario.Activo = false;
                                    numUnicoS3.Sam3_NumeroUnicoInventario.FechaModificacion = DateTime.Now;
                                    numUnicoS3.Sam3_NumeroUnicoInventario.UsuarioModificacion = usuario.UsuarioID;

                                    foreach (Sam3_NumeroUnicoSegmento segmento in numUnicoS3.Sam3_NumeroUnicoSegmento)
                                    {
                                        segmento.Activo = false;
                                        segmento.FechaModificacion = DateTime.Now;
                                        segmento.UsuarioModificacion = usuario.UsuarioID;
                                    }

                                    Sam3_Rel_NumeroUnico_RelFC_RelB relnu = ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.NumeroUnicoID == numeroUnicoID).AsParallel().SingleOrDefault();
                                    relnu.Activo = false;
                                    relnu.FechaModificacion = DateTime.Now;
                                    relnu.UsuarioModificacion = usuario.UsuarioID;
                                    ctx.SaveChanges();

                                    if (relnu.Rel_FolioCuantificacion_ItemCode_ID != null)
                                    {
                                        Sam3_Rel_FolioCuantificacion_ItemCode relfc = ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                            .Where(x => x.Rel_FolioCuantificacion_ItemCode_ID == relnu.Rel_FolioCuantificacion_ItemCode_ID).AsParallel().SingleOrDefault();

                                        //if (ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.Rel_FolioCuantificacion_ItemCode_ID == relfc.Rel_FolioCuantificacion_ItemCode_ID
                                        //    && x.Activo).Distinct().Count() <= 0)
                                        //{
                                            relfc.TieneNumerosUnicos = false;
                                        //}
                                        ctx.SaveChanges();
                                    }

                                    if (relnu.Rel_Bulto_ItemCode_ID != null)
                                    {
                                        Sam3_Rel_Bulto_ItemCode relbu = ctx.Sam3_Rel_Bulto_ItemCode.Where(x => x.Rel_Bulto_ItemCode_ID == relnu.Rel_Bulto_ItemCode_ID).AsParallel().SingleOrDefault();

                                        //if (ctx.Sam3_Rel_NumeroUnico_RelFC_RelB.Where(x => x.Rel_Bulto_ItemCode_ID == relbu.Rel_Bulto_ItemCode_ID
                                        //    && x.Activo).Distinct().Count() <= 0)
                                        //{
                                            relbu.TieneNumerosUnicos = false;
                                        //}
                                        ctx.SaveChanges();
                                    }

                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    throw new Exception("No se puede Eliminar el Número Único pues tiene algun proceso capturado");
                                }

                                ctx_tran.Commit();
                                ctx2_tran.Commit();
                            }
                        }
                    }
 
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