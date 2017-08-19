using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using BackEndSAM.Utilities;
using System.Web.Script.Serialization;

namespace BackEndSAM.DataAcces
{
    public class GuardarFolioLlegadaCuantificacionBd
    {
        private static readonly object _mutex = new object();
        private static GuardarFolioLlegadaCuantificacionBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private GuardarFolioLlegadaCuantificacionBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static GuardarFolioLlegadaCuantificacionBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new GuardarFolioLlegadaCuantificacionBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Actualizar un folio cuantificacion existente
        /// </summary>
        /// <param name="datosCuantificacion">informacion del folio cuantificacion</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>folio de cuantificacion creado, proyectos</returns>
        public object UpdateGuardarFolio(DatosFolioLlegadaCuantificacion datosCuantificacion, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        Boolean activarFolioConfiguracionCuantificacion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"]) ?
                            (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;
                        bool activaConfigFolioLlegada = ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false;

                        int avisoEntradaID = ctx.Sam3_FolioAvisoEntrada.Where(x => x.FolioAvisoLlegadaID == datosCuantificacion.FolioAvisollegadaId && x.Activo).Select(x => x.FolioAvisoEntradaID).AsParallel().First();

                        Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion
                            .Where(x => x.FolioCuantificacionID == datosCuantificacion.FolioCuantificacionID && x.Activo).AsParallel().SingleOrDefault();

                        Sam3_FolioAvisoLlegada folioLlegada = (from fc in ctx.Sam3_FolioCuantificacion
                                                               join fe in ctx.Sam3_FolioAvisoEntrada on fc.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                                               join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                                               where fc.Activo && fe.Activo && fa.Activo
                                                               && fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                               select fa).AsParallel().FirstOrDefault();

                        if (folioCuantificacion.Estatus != "Terminado" && folioCuantificacion.Estatus != "Cerrado")
                        {
                            folioCuantificacion.Estatus = "En Proceso de Recepción";
                        }

                        folioCuantificacion.FolioAvisoEntradaID = avisoEntradaID;
                        folioCuantificacion.ProyectoID = datosCuantificacion.ProyectoID;
                        folioCuantificacion.PackingList = datosCuantificacion.PackingList;
                        folioCuantificacion.TipoUsoID = datosCuantificacion.TipoUso;
                        folioCuantificacion.FechaModificacion = DateTime.Now;
                        folioCuantificacion.UsuarioModificacion = usuario.UsuarioID;
                        folioCuantificacion.Activo = true;
                        folioCuantificacion.Factura = datosCuantificacion.Factura;
                        if (datosCuantificacion.TipoPackingList != null)
                        {
                            folioCuantificacion.TipoMaterialID = datosCuantificacion.TipoPackingList;
                        }
                        
                        List<Sam3_ItemCode> itemCodes;
                        
                        List<Sam3_NumeroUnico> numerosUnicosTemp;
                        if (folioCuantificacion.Sam3_Rel_FolioCuantificacion_ItemCode.Count > 0) // actualizar tipo de uso en itemCodes
                        {
                            itemCodes = (from rfi in folioCuantificacion.Sam3_Rel_FolioCuantificacion_ItemCode
                                                             join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                             join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                             join nu in ctx.Sam3_NumeroUnico on it.ItemCodeID equals nu.ItemCodeID.GetValueOrDefault()
                                         where rid.Activo && it.Activo && nu.Activo
                                                             select it).AsParallel().Distinct().ToList();

                            //Agrego todos los itemCodes que esten en los bultos relacionados con el folio de cuantificacion
                            itemCodes.AddRange((from b in ctx.Sam3_Bulto 
                                                join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                                join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                join it in ctx.Sam3_ItemCode on rid.ItemCodeID equals it.ItemCodeID
                                                where b.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                && b.Activo && rbi.Activo && rid.Activo && it.Activo
                                                select it).AsParallel().Distinct().ToList());

                            itemCodes.ForEach(x => x.TipoUsoID = datosCuantificacion.TipoUso);

                            ////Actualizar todos los numeros unicos que esten relacionados con los ItemCodes
                            numerosUnicosTemp = (from it in itemCodes
                                                 join nu in ctx.Sam3_NumeroUnico on it.ItemCodeID equals nu.ItemCodeID.GetValueOrDefault()
                                                 where it.Activo && nu.Activo
                                                 select nu).ToList();
                            if (numerosUnicosTemp.Count > 0)
                            {
                                List<Sam3_NumeroUnico> numerosUnicos;
                                numerosUnicos = numerosUnicosTemp;
                                numerosUnicos.ForEach(x => x.TipoUsoID = datosCuantificacion.TipoUso);
                            }
                        }

                        ctx.SaveChanges();

                        //Guardar Orden de Compra y Factura
                        Sam3_FolioAvisoEntrada folioEntrada = (from ave in ctx.Sam3_FolioAvisoEntrada
                                                               where ave.Activo && ave.FolioAvisoLlegadaID == datosCuantificacion.FolioAvisollegadaId
                                                               select ave).AsParallel().SingleOrDefault();
                        folioEntrada.OrdenCompra = datosCuantificacion.OrdenDeCompra;
                        //folioEntrada.Factura = datosCuantificacion.Factura;
                        folioEntrada.FechaModificacion = DateTime.Now;
                        folioEntrada.UsuarioModificacion = usuario.UsuarioID;

                        ctx.SaveChanges();

                        #region Proyectos

                        int cuentaProyectos = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                               join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                               join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                               join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                               where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                               select rel.ProyectoID).Count();

                        int folioAvisoLlegada = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                                 join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                                 join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                                 join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                                 where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                 select fe.FolioAvisoLlegadaID.Value).AsParallel().Distinct().SingleOrDefault();

                        if (cuentaProyectos == 1)
                        {
                            bool esDefault = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                              join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                              join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                              join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                              where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                              && (p.Nombre == "Proyecto Default" || p.Nombre == "")
                                              select rel).Count() == 1 ? true : false;


                            if (esDefault)
                            {
                                Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                nuevaRel.Activo = true;
                                nuevaRel.FechaModificacion = DateTime.Now;
                                nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegada
                                    && x.ProyectoID == folioCuantificacion.ProyectoID).Any())
                                {
                                    Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                    nuevaRel.Activo = true;
                                    nuevaRel.FechaModificacion = DateTime.Now;
                                    nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                    nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                    nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                    ctx.SaveChanges();
                                }
                            }

                        }
                        else
                        {

                            if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegada
                                    && x.ProyectoID == folioCuantificacion.ProyectoID).Any())
                            {
                                Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                nuevaRel.Activo = true;
                                nuevaRel.FechaModificacion = DateTime.Now;
                                nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                ctx.SaveChanges();
                            }
                        }

                        #endregion

                        if (datosCuantificacion.BultoID > 0)
                        {
                            Sam3_Bulto bulto = ctx.Sam3_Bulto.Where(x => x.BultoID == datosCuantificacion.BultoID && x.Activo).AsParallel().SingleOrDefault();
                            if (folioCuantificacion.Estatus != "Terminado" && folioCuantificacion.Estatus != "Cerrado")
                            {
                                bulto.Estatus = "En Proceso de Recepción";
                            }

                            bulto.FolioCuantificacionID = datosCuantificacion.FolioCuantificacionID;
                            bulto.FechaModificacion = DateTime.Now;
                            bulto.UsuarioModificacion = usuario.UsuarioID;
                            bulto.Activo = true;

                            ctx.SaveChanges();
                        }

                        string nombre = (from p in ctx.Sam3_Proyecto
                                         where p.ProyectoID == folioCuantificacion.ProyectoID && p.Activo
                                         select p.Nombre).AsParallel().SingleOrDefault();

                        ctx_tran.Commit();

                        FolioLlegadaCuantificacion foliollegadacuantificacion = new FolioLlegadaCuantificacion();
                        foliollegadacuantificacion.FolioCuantificacionID = folioCuantificacion.FolioCuantificacionID;
                        foliollegadacuantificacion.ProyectoID = folioCuantificacion.ProyectoID;
                        foliollegadacuantificacion.Nombre = nombre;

                        string NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                          where pc.Proyecto == folioLlegada.ProyectoNombrado
                                                          && pc.Entidad == folioLlegada.Entidad
                                                          select pc.PreFijoFolioAvisoLlegada + ","
                                                          + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                          + folioLlegada.Consecutivo + ","
                                                          + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault();

                        string NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                            where pc.Rel_Proyecto_Entidad_Configuracion_ID == folioCuantificacion.Rel_Proyecto_Entidad_Configuracion_ID
                                                            select pc.PreFijoFolioPackingList + ","
                                                            + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                            + folioCuantificacion.ConsecutivoConfiguracion.ToString() + ","
                                                            + pc.PostFijoFolioPackingList).FirstOrDefault();

                        int FolioAvisoLlegadaID = folioLlegada.FolioAvisoLlegadaID;
                        int ConsecutivoFolioCuanificacion = folioCuantificacion.Consecutivo.Value;
                        int ConsecutivoFolioLlegada = folioLlegada.Consecutivo.Value;
                        foliollegadacuantificacion.FolioConfiguracionCuantificacionID = "";

                        NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioAvisoLlegada);
                        NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioCuantificacion);

                        if (activaConfigFolioLlegada && activarFolioConfiguracionCuantificacion)
                        {
                            foliollegadacuantificacion.FolioConfiguracionCuantificacionID = NombreFolioAvisoLlegada + "-" + NombreFolioCuantificacion;
                        }

                        if (activaConfigFolioLlegada && !activarFolioConfiguracionCuantificacion)
                        {
                            foliollegadacuantificacion.FolioConfiguracionCuantificacionID = NombreFolioAvisoLlegada + "-" + ConsecutivoFolioCuanificacion;
                        }

                        if (!activaConfigFolioLlegada && activarFolioConfiguracionCuantificacion)
                        {
                            foliollegadacuantificacion.FolioConfiguracionCuantificacionID = FolioAvisoLlegadaID + "-" + NombreFolioCuantificacion;
                        }

                        if (!activaConfigFolioLlegada && !activarFolioConfiguracionCuantificacion)
                        {
                            foliollegadacuantificacion.FolioConfiguracionCuantificacionID = FolioAvisoLlegadaID + "-" + ConsecutivoFolioCuanificacion;
                        }

                        return foliollegadacuantificacion;
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
        /// Guardar un folio Cuantificacion Nuevo
        /// </summary>
        /// <param name="datosCuantificacion">informacion del folio cuantificacion</param>
        /// <param name="usuario">usuario actual</param>
        /// <returns>Folio de cuantificacion creados, proyectos</returns>
        public object CreateGuardarFolio(DatosFolioLlegadaCuantificacion datosCuantificacion, Sam3_Usuario usuario)
        {
            string errorInfo = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    using (var ctx_tran = ctx.Database.BeginTransaction())
                    {
                        errorInfo += "\nFolioEntrada: " + serializer.Serialize(datosCuantificacion);
                        Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"])
                            ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;
                        bool activaConfigFolioLlegada = ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false;

                        int avisoEntradaID = ctx.Sam3_FolioAvisoEntrada
                            .Where(x => x.FolioAvisoLlegadaID == datosCuantificacion.FolioAvisollegadaId && x.Activo).Select(x => x.FolioAvisoEntradaID).AsParallel().First();

                        int? consecutivofc = (from fc in ctx.Sam3_FolioCuantificacion
                                              where fc.Activo
                                              && fc.FolioAvisoEntradaID == avisoEntradaID
                                              select fc.Consecutivo).Max().HasValue ? (from fc in ctx.Sam3_FolioCuantificacion
                                                                                       where fc.Activo
                                                                                       && fc.FolioAvisoEntradaID == avisoEntradaID
                                                                                       select fc.Consecutivo).Max().Value : 0;

                        errorInfo += "\nActiva FolioConfiguracion: " + activarFolioConfiguracion.ToString() + "\nActivaFolioLlegada: " + activaConfigFolioLlegada.ToString() +
                            "\nAvisoLlegadaID: " + avisoEntradaID.ToString() + "\nConsecutivo: " + consecutivofc.ToString();


                        if (consecutivofc <= 0)
                        {
                            consecutivofc = 1;
                        }
                        else
                        {
                            consecutivofc += 1;
                        }

                        Sam3_FolioCuantificacion folioCuantificacion = new Sam3_FolioCuantificacion();

                        folioCuantificacion.FolioAvisoEntradaID = avisoEntradaID;
                        folioCuantificacion.ProyectoID = datosCuantificacion.ProyectoID;
                        folioCuantificacion.PackingList = datosCuantificacion.PackingList;
                        folioCuantificacion.TipoUsoID = datosCuantificacion.TipoUso;
                        folioCuantificacion.FechaCreacion = DateTime.Now;
                        folioCuantificacion.Estatus = "En Proceso de Recepción";
                        folioCuantificacion.FechaModificacion = DateTime.Now;
                        folioCuantificacion.UsuarioModificacion = usuario.UsuarioID;
                        folioCuantificacion.Activo = true;
                        folioCuantificacion.Factura = datosCuantificacion.Factura;
                        if (datosCuantificacion.TipoPackingList != null)
                        {
                            folioCuantificacion.TipoMaterialID = datosCuantificacion.TipoPackingList;
                        }
                        folioCuantificacion.Consecutivo = consecutivofc;

                        ctx.Sam3_FolioCuantificacion.Add(folioCuantificacion);
                        ctx.SaveChanges();

                        errorInfo += "\nFolio cuantificacion creado";
                        //Guardar Orden de Compra y Factura
                        Sam3_FolioAvisoEntrada folioEntrada = (from ave in ctx.Sam3_FolioAvisoEntrada
                                                               where ave.Activo && ave.FolioAvisoLlegadaID == datosCuantificacion.FolioAvisollegadaId
                                                               select ave).AsParallel().SingleOrDefault();

                        errorInfo += "\nFolioAvisoEntradaID: " + folioEntrada.FolioAvisoEntradaID.ToString();


                        folioEntrada.OrdenCompra = datosCuantificacion.OrdenDeCompra;
                        //folioEntrada.Factura = datosCuantificacion.Factura;
                        folioEntrada.FechaModificacion = DateTime.Now;
                        folioEntrada.UsuarioModificacion = usuario.UsuarioID;

                        ctx.SaveChanges();
                        errorInfo += "\nFolio Entrada actualizado";

                        #region Proyectos

                        int cuentaProyectos = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                               join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                               join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                               join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                               where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                               select rel.ProyectoID).Count();

                        int folioAvisoLlegada = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                                 join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                                 join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                                 join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                                 where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                 select fe.FolioAvisoLlegadaID.Value).AsParallel().Distinct().SingleOrDefault();

                        if (cuentaProyectos == 1)
                        {
                            bool esDefault = (from rel in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                              join fe in ctx.Sam3_FolioAvisoEntrada on rel.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                              join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                              join p in ctx.Sam3_Proyecto on rel.ProyectoID equals p.ProyectoID
                                              where fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                              && (p.Nombre == "Proyecto Default" || p.Nombre == "")
                                              select rel).Count() == 1 ? true : false;


                            if (esDefault)
                            {
                                Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                nuevaRel.Activo = true;
                                nuevaRel.FechaModificacion = DateTime.Now;
                                nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                ctx.SaveChanges();
                                errorInfo += "rel folio proyecto creado";
                            }
                            else
                            {
                                if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegada
                                    && x.ProyectoID == folioCuantificacion.ProyectoID).Any())
                                {
                                    Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                    nuevaRel.Activo = true;
                                    nuevaRel.FechaModificacion = DateTime.Now;
                                    nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                    nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                    nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                    ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                    ctx.SaveChanges();
                                    errorInfo += "rel folio proyecto creado";
                                }
                            }

                        }
                        else
                        {

                            if (!ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Where(x => x.FolioAvisoLlegadaID == folioAvisoLlegada
                                    && x.ProyectoID == folioCuantificacion.ProyectoID).Any())
                            {
                                Sam3_Rel_FolioAvisoLlegada_Proyecto nuevaRel = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                                nuevaRel.Activo = true;
                                nuevaRel.FechaModificacion = DateTime.Now;
                                nuevaRel.FolioAvisoLlegadaID = folioAvisoLlegada;
                                nuevaRel.ProyectoID = folioCuantificacion.ProyectoID;
                                nuevaRel.UsuarioModificacion = usuario.UsuarioID;

                                ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(nuevaRel);
                                ctx.SaveChanges();
                                errorInfo += "rel folio proyecto creado";
                            }
                        }

                        #endregion

                        if (datosCuantificacion.BultoID != null && datosCuantificacion.BultoID != 0 && datosCuantificacion.BultoID != -1)
                        {
                            Sam3_Bulto bulto = ctx.Sam3_Bulto.Where(x => x.BultoID == datosCuantificacion.BultoID && x.Activo).AsParallel().SingleOrDefault();
                            bulto.FolioCuantificacionID = datosCuantificacion.FolioCuantificacionID;
                            bulto.Estatus = "En Proceso de Recepción";
                            bulto.FechaModificacion = DateTime.Now;
                            bulto.UsuarioModificacion = usuario.UsuarioID;
                            bulto.Activo = true;

                            ctx.Sam3_Bulto.Add(bulto);
                            ctx.SaveChanges();
                        }

                        string nombre = (from p in ctx.Sam3_Proyecto
                                         where p.ProyectoID == folioCuantificacion.ProyectoID && p.Activo
                                         select p.Nombre).AsParallel().SingleOrDefault();


                        FolioLlegadaCuantificacion folioLlegadaCuantificacion = new FolioLlegadaCuantificacion();
                        folioLlegadaCuantificacion.FolioCuantificacionID = folioCuantificacion.FolioCuantificacionID;
                        folioLlegadaCuantificacion.ProyectoID = folioCuantificacion.ProyectoID;
                        folioLlegadaCuantificacion.Nombre = nombre;

                        Sam3_Rel_Proyecto_Entidad_Configuracion rel_proyecto_entidad_configuracion = ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                            .Where(x => x.Activo == 1 && x.Proyecto == folioCuantificacion.ProyectoID).FirstOrDefault();


                        int consecutivofoliopl = 0;
                        if (rel_proyecto_entidad_configuracion != null)
                        {
                            consecutivofoliopl = rel_proyecto_entidad_configuracion.ConsecutivoFolioPackingList;
                        }
                        else
                        {
                            consecutivofoliopl = 1;
                        }

                        Sam3_FolioCuantificacion folioCuantificacionConsecutivo = ctx.Sam3_FolioCuantificacion
                            .Where(x => x.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID).FirstOrDefault();
                        folioCuantificacionConsecutivo.ConsecutivoConfiguracion = consecutivofoliopl;
                        folioCuantificacionConsecutivo.Rel_Proyecto_Entidad_Configuracion_ID = rel_proyecto_entidad_configuracion.Rel_Proyecto_Entidad_Configuracion_ID;
                        ctx.SaveChanges();


                        rel_proyecto_entidad_configuracion.ConsecutivoFolioPackingList = consecutivofoliopl + 1;
                        ctx.SaveChanges();


                        Sam3_FolioAvisoLlegada foliollegada = (from fa in ctx.Sam3_FolioAvisoLlegada
                                                               where fa.Activo && fa.FolioAvisoLlegadaID == datosCuantificacion.FolioAvisollegadaId
                                                               select fa).AsParallel().SingleOrDefault();

                        string NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                          where pc.Proyecto == foliollegada.ProyectoNombrado
                                                          && pc.Entidad == foliollegada.Entidad
                                                          select pc.PreFijoFolioAvisoLlegada + ","
                                                          + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                          + foliollegada.Consecutivo + ","
                                                          + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault();

                        string NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                            where pc.Rel_Proyecto_Entidad_Configuracion_ID == folioCuantificacion.Rel_Proyecto_Entidad_Configuracion_ID
                                                            select pc.PreFijoFolioPackingList + ","
                                                            + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                            + folioCuantificacion.ConsecutivoConfiguracion.ToString() + ","
                                                            + pc.PostFijoFolioPackingList).FirstOrDefault();

                        int FolioAvisoLlegadaID = foliollegada.FolioAvisoLlegadaID;
                        int ConsecutivoFolioCuanificacion = folioCuantificacion.Consecutivo.Value;
                        int ConsecutivoFolioLlegada = foliollegada.Consecutivo.Value;
                        folioLlegadaCuantificacion.FolioConfiguracionCuantificacionID = "";

                        NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioAvisoLlegada);
                        NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioCuantificacion);

                        if (activaConfigFolioLlegada && activarFolioConfiguracion)
                        {
                            folioLlegadaCuantificacion.FolioConfiguracionCuantificacionID = NombreFolioAvisoLlegada + "-" + NombreFolioCuantificacion;
                        }

                        if (activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            folioLlegadaCuantificacion.FolioConfiguracionCuantificacionID = NombreFolioAvisoLlegada + "-" + ConsecutivoFolioCuanificacion;
                        }

                        if (!activaConfigFolioLlegada && activarFolioConfiguracion)
                        {
                            folioLlegadaCuantificacion.FolioConfiguracionCuantificacionID = FolioAvisoLlegadaID + "-" + NombreFolioCuantificacion;
                        }

                        if (!activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            folioLlegadaCuantificacion.FolioConfiguracionCuantificacionID = FolioAvisoLlegadaID + "-" + ConsecutivoFolioCuanificacion;
                        }


                        ctx_tran.Commit();

                        return folioLlegadaCuantificacion;
                    }
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                LoggerBd.Instance.EscribirLog("\n" + ex.InnerException);
                LoggerBd.Instance.EscribirLog("\n" + ex.StackTrace);
                LoggerBd.Instance.EscribirLog(errorInfo);
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