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
    public class FoliosCuantificacionBd
    {
        private static readonly object _mutex = new object();
        private static FoliosCuantificacionBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private FoliosCuantificacionBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static FoliosCuantificacionBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new FoliosCuantificacionBd();
                    }
                }
                return _instance;
            }
        }

        public object ObtenerProyectoFolioCuantificacion(int folioCuantificacionID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_FolioCuantificacion folio = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == folioCuantificacionID)
                        .AsParallel().SingleOrDefault();

                    return folio.ProyectoID;
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

        public object CambiarEstatus(int folioCuantificacionID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_FolioCuantificacion folio = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == folioCuantificacionID)
                        .AsParallel().SingleOrDefault();

                    folio.Estatus = "En Proceso de Recepción";
                    folio.FechaModificacion = DateTime.Now;
                    folio.UsuarioModificacion = usuario.UsuarioID;

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
        /// Obtener Folio AViso Llegada (Combo Folio Aviso Entrada)
        /// </summary>
        /// <returns>Lista de Folios de Aviso de llegada</returns>
        public object obtenerFolioLlegada(Sam3_Usuario usuario)
        {
            List<ListaCombos> listFE = new List<ListaCombos>();
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracion"]) ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false) : false;
                    List<int> proyectos;
                    List<int> patios;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    listFE = (from fe in ctx.Sam3_FolioAvisoEntrada
                              join fa in ctx.Sam3_FolioAvisoLlegada on fe.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                              join fp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fa.FolioAvisoLlegadaID equals fp.FolioAvisoLlegadaID
                              join p in ctx.Sam3_Proyecto on fp.ProyectoID equals p.ProyectoID
                              join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                              where fe.FolioDescarga > 0 
                              && fe.Activo && fa.Activo && fp.Activo && p.Activo && pa.Activo
                              && proyectos.Contains(p.ProyectoID)
                              && patios.Contains(pa.PatioID)
                              && patios.Contains(fa.PatioID)
                              && patios.Contains(fe.PatioID)
                              select new ListaCombos
                                {
                                    id = fe.FolioAvisoLlegadaID.ToString(),
                                    value = fe.FolioAvisoLlegadaID.ToString()
                                }).AsParallel().Distinct().ToList();

                    listFE = listFE.GroupBy(x => x.id).Select(x => x.First()).ToList();
                    listFE = listFE.OrderBy(x => x.value).ToList();


                    foreach (ListaCombos lst in listFE)
                    {
                        int folioavisollegadaid = Convert.ToInt32(lst.id);
                        Sam3_FolioAvisoLlegada FolioAvisoLlegada = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioavisollegadaid).FirstOrDefault();

                        lst.value = activarFolioConfiguracion ? (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                 where pc.Entidad == FolioAvisoLlegada.Entidad && pc.Proyecto == FolioAvisoLlegada.ProyectoNombrado
                                                                 select pc.PreFijoFolioAvisoLlegada + ","
                                                                  + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                  + FolioAvisoLlegada.Consecutivo.ToString() + ","
                                                                  + pc.PostFijoFolioAvisoLlegada).FirstOrDefault() : folioavisollegadaid.ToString();
                        if (activarFolioConfiguracion)
                        {
                            string[] elemntos = lst.value.Split(',').ToArray();
                            int digitos = Convert.ToInt32(elemntos[1]);
                            int consecutivo = Convert.ToInt32(elemntos[2]);
                            string formato = "D" + digitos.ToString();

                            lst.value = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                        }
                    }
                }
                return listFE;

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
        ///Obtiene la informacion de un aviso de entrada en cuantificacion
        /// </summary>
        /// <param name="folioAvisoLlegadaID">folio aviso de llegada seleccionado</param>
        /// <returns>objeto con la informacion</returns>
        public object obtenerDatosFolioEntrada(int folioAvisoLlegadaID, int folioCuantificacionID, Sam3_Usuario usuario)
        {
            List<Proyecto> proyectos = new List<Proyecto>();
            List<int> proyectosUsuario;
            List<int> patiosUsuario;
            UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectosUsuario, out patiosUsuario);
            List<FolioLlegada1> cuantificacion = new List<FolioLlegada1>();
            Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"]) ?
                (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;

            bool activaConfigFolioLlegada = ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false;

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    proyectos = (from p in ctx.Sam3_Proyecto
                                 join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                 where p.ProyectoID != 1 && pa.Activo && p.Activo
                                 && proyectosUsuario.Contains(p.ProyectoID)
                                 && patiosUsuario.Contains(pa.PatioID)
                                 && (from relp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto
                                                              where relp.Activo && relp.FolioAvisoLlegadaID == folioAvisoLlegadaID
                                                              select relp.ProyectoID).Contains(p.ProyectoID)
                                 select new Proyecto
                                 {
                                     ProyectoID = p.ProyectoID.ToString(),
                                     Nombre = p.Nombre
                                 }).AsParallel().ToList();

                    cuantificacion = (from t in ctx.Sam3_FolioCuantificacion
                                      join avll in ctx.Sam3_FolioAvisoEntrada on t.FolioAvisoEntradaID equals avll.FolioAvisoEntradaID
                                      join fa in ctx.Sam3_FolioAvisoLlegada on avll.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                                      where t.Activo && avll.Activo && fa.Activo
                                      && avll.FolioAvisoLlegadaID == folioAvisoLlegadaID
                                      select new FolioLlegada1
                                           {
                                               FolioCuantificacionID = t.FolioCuantificacionID,
                                               FolioAvisoEntradaID = t.FolioCuantificacionID,
                                               NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                          where pc.Proyecto == fa.ProyectoNombrado
                                                                          && pc.Entidad == fa.Entidad
                                                                          select pc.PreFijoFolioAvisoLlegada + ","
                                                                          + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                                          + fa.Consecutivo + ","
                                                                          + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault(),
                                               NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                            where pc.Rel_Proyecto_Entidad_Configuracion_ID == t.Rel_Proyecto_Entidad_Configuracion_ID
                                                                            select pc.PreFijoFolioPackingList + ","
                                                                            + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                                            + t.ConsecutivoConfiguracion.ToString() + ","
                                                                            + pc.PostFijoFolioPackingList).FirstOrDefault(),
                                               FolioConfiguracionCuantificacionID = "",
                                               FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID,
                                               ConsecutivoFolioCuanificacion = t.Consecutivo.Value,
                                               ConsecutivoFolioLlegada = fa.Consecutivo.Value,
                                               TipoUsoID = t.TipoUsoID,
                                               TipoUso = (from tp in ctx.Sam3_TipoUso
                                                          where tp.Activo && tp.TipoUsoID == t.TipoUsoID
                                                          select tp.Nombre).FirstOrDefault()
                                           }).AsParallel().ToList();


                    foreach (FolioLlegada1 item in cuantificacion)
                    {
                        item.NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(item.NombreFolioAvisoLlegada);
                        item.NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(item.NombreFolioCuantificacion);

                        if (activaConfigFolioLlegada && activarFolioConfiguracion)
                        {
                            item.FolioConfiguracionCuantificacionID = item.NombreFolioAvisoLlegada + "-" + item.NombreFolioCuantificacion;
                        }

                        if (activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            item.FolioConfiguracionCuantificacionID = item.NombreFolioAvisoLlegada + "-" + item.ConsecutivoFolioCuanificacion;
                        }

                        if (!activarFolioConfiguracion && activarFolioConfiguracion)
                        {
                            item.FolioConfiguracionCuantificacionID = item.FolioAvisoLlegadaID + "-" + item.NombreFolioCuantificacion;
                        }

                        if (!activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            item.FolioConfiguracionCuantificacionID = item.FolioAvisoLlegadaID + "-" + item.ConsecutivoFolioCuanificacion;
                        }

                    }

                    InfoFolioAvisoEntrada info = new InfoFolioAvisoEntrada();
                    info.Proyecto = proyectos;
                    info.FolioLlegada = cuantificacion;
                    info.OrdenDeCompra = (from ave in ctx.Sam3_FolioAvisoEntrada 
                                          where ave.FolioAvisoLlegadaID == folioAvisoLlegadaID 
                                          && ave.Activo 
                                          select ave.OrdenCompra).AsParallel().SingleOrDefault();


                    if (folioCuantificacionID > 0)
                    {
                        info.Factura = (from fc in ctx.Sam3_FolioCuantificacion
                                        where fc.FolioCuantificacionID == folioCuantificacionID && fc.Activo
                                        select fc.Factura).AsParallel().FirstOrDefault();
                    }
                    else
                    {
                        info.Factura = (from ave in ctx.Sam3_FolioAvisoEntrada
                                        where ave.FolioAvisoLlegadaID == folioAvisoLlegadaID
                                        && ave.Activo
                                        select ave.Factura).AsParallel().SingleOrDefault();
                    }
                    
                    return info;
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
        /// Actualizar proyecto cuando el folio de av llegada no tiene proyecto seleccionado
        /// </summary>
        /// <param name="avisoLlegada">folio de aviso de llegada</param>
        /// <param name="proyectoID">id del proyecto seleccionado</param>
        /// <param name="usuarioID">usuario </param>
        /// <returns></returns>
        public object actualizarProyectos(int avisoLlegada, int proyectoID, int usuarioID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    // ¿Comó sabes que el proyecto no existe?

                    Sam3_Rel_FolioAvisoLlegada_Proyecto avPR = new Sam3_Rel_FolioAvisoLlegada_Proyecto();
                    avPR.FolioAvisoLlegadaID = avisoLlegada;
                    avPR.ProyectoID = proyectoID;
                    avPR.Activo = true;
                    avPR.UsuarioModificacion = usuarioID;
                    avPR.FechaModificacion = DateTime.Now;

                    ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto.Add(avPR);
                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnMessage.Add(avPR.Rel_FolioAviso_ProyectoID.ToString());
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
        /// Obtener datos de un folio de aviso de entrada / folio cuantificacion
        /// </summary>
        /// <param name="folioAvisoLlegadaID">Folio aviso de llegada seleccionado</param>
        /// <param name="folioCuantificacion"></param>
        /// <returns></returns>
        public object obtenerDatosFolioCuantificacion(int folioAvisoLlegadaID, int folioCuantificacion, string detalleBulto)
        {
            try
            {
                InfoFolioCuantificacion info = new InfoFolioCuantificacion();
                Boolean activarFolioConfiguracion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"]) ? 
                    (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;

                bool activaConfigFolioLlegada = ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false;

                using (SamContext ctx = new SamContext())
                {
                    info = (from t in ctx.Sam3_FolioCuantificacion
                            join avll in ctx.Sam3_FolioAvisoEntrada on t.FolioAvisoEntradaID equals avll.FolioAvisoEntradaID
                            join fa in ctx.Sam3_FolioAvisoLlegada on avll.FolioAvisoLlegadaID equals fa.FolioAvisoLlegadaID
                            join tu in ctx.Sam3_TipoUso on t.TipoUsoID equals tu.TipoUsoID
                            join Bul in ctx.Sam3_Bulto on t.FolioCuantificacionID equals Bul.FolioCuantificacionID into b1
                            join py in ctx.Sam3_Proyecto on t.ProyectoID equals py.ProyectoID
                            from subBul in b1.DefaultIfEmpty()
                            where t.FolioCuantificacionID == folioCuantificacion
                            && avll.FolioAvisoLlegadaID == folioAvisoLlegadaID
                            && t.Activo && avll.Activo && tu.Activo
                            select new InfoFolioCuantificacion
                            {
                                ProyectoID = t.ProyectoID,
                                PackingList = t.PackingList,
                                Factura = t.Factura,
                                OrdenDeCompra = avll.OrdenCompra,
                                TipoUso = new TipoUso()
                                {
                                    id = t.TipoUsoID.ToString(),
                                    Nombre = tu.Nombre
                                },

                                TipoPackingList = new TipoPackingList()
                                {
                                    id = t.TipoMaterialID.ToString()
                                },

                                Estatus = detalleBulto == "-1" ? t.Estatus : subBul.Estatus,

                                FolioLlegadaHijo = 0,
                                NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                           where pc.Proyecto == fa.ProyectoNombrado
                                                           && pc.Entidad == fa.Entidad
                                                           select pc.PreFijoFolioAvisoLlegada + ","
                                                           + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                           + fa.Consecutivo + ","
                                                           + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault(),
                                NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                             where pc.Rel_Proyecto_Entidad_Configuracion_ID == t.Rel_Proyecto_Entidad_Configuracion_ID
                                                             select pc.PreFijoFolioPackingList + ","
                                                             + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                             + t.ConsecutivoConfiguracion.ToString() + ","
                                                             + pc.PostFijoFolioPackingList).FirstOrDefault(),
                                FolioConfiguracionCuantificacionID = "",
                                FolioAvisoLlegadaID = fa.FolioAvisoLlegadaID,
                                ConsecutivoFolioCuanificacion = t.Consecutivo.Value,
                                ConsecutivoFolioLlegada = fa.Consecutivo.Value,


                                MostrarComboPackingList = (from p in ctx.Sam3_ProyectoConfiguracion
                                                           where p.Activo
                                                           && p.ProyectoID == t.ProyectoID
                                                           select p.RequiereTipoPackingList).FirstOrDefault(),
                                NombreProyecto = py.Nombre

                            }).AsParallel().FirstOrDefault();




                    if (!ctx.Sam3_FolioCuantificacion.Any(x => x.Activo && x.FolioCuantificacionID == folioCuantificacion))
                    {
                        throw new Exception("El folio cuantificacion no se encuentra activo.");
                    }
                    else {
                        Sam3_FolioCuantificacion entidadcuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.Activo && x.FolioCuantificacionID == folioCuantificacion).FirstOrDefault();
                        if (!ctx.Sam3_TipoUso.Any(x => x.Activo && x.TipoUsoID == entidadcuantificacion.TipoUsoID)) 
                        {
                            throw new Exception("El Tipo de Uso no se encuentra activo.");
                        }

                        if (!ctx.Sam3_FolioAvisoEntrada.Any(x => x.Activo && x.FolioAvisoEntradaID == entidadcuantificacion.FolioAvisoEntradaID))
                        {
                            throw new Exception("La Llegada de material no se encuentra activa.");
                        }
                        else {
                            Sam3_FolioAvisoEntrada entidadfolioavisoentrada = ctx.Sam3_FolioAvisoEntrada.Where(x => x.Activo && x.FolioAvisoEntradaID == entidadcuantificacion.FolioAvisoEntradaID).FirstOrDefault();
                            if (!ctx.Sam3_FolioAvisoLlegada.Any(x => x.Activo && x.FolioAvisoLlegadaID == entidadfolioavisoentrada.FolioAvisoLlegadaID))
                            {
                                throw new Exception("El Aviso de Entrada no se encuentra activa.");
                            }
                        }
                    }

                    if (!ctx.Sam3_FolioCuantificacion.Any(x => x.Activo && x.FolioCuantificacionID == folioCuantificacion))
                    if (info != null)
                    {
                        info.NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(info.NombreFolioAvisoLlegada);
                        info.NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(info.NombreFolioCuantificacion);

                        if (activaConfigFolioLlegada && activarFolioConfiguracion)
                        {
                            info.FolioConfiguracionCuantificacionID = info.NombreFolioAvisoLlegada + "-" + info.NombreFolioCuantificacion;
                        }

                        if (activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            info.FolioConfiguracionCuantificacionID = info.NombreFolioAvisoLlegada + "-" + info.ConsecutivoFolioCuanificacion;
                        }

                        if (!activarFolioConfiguracion && activarFolioConfiguracion)
                        {
                            info.FolioConfiguracionCuantificacionID = info.FolioAvisoLlegadaID + "-" + info.NombreFolioCuantificacion;
                        }

                        if (!activaConfigFolioLlegada && !activarFolioConfiguracion)
                        {
                            info.FolioConfiguracionCuantificacionID = info.FolioAvisoLlegadaID + "-" + info.ConsecutivoFolioCuanificacion;
                        }
                    }
                    else
                    {
                        throw new Exception("Chofer existente");
                    }
                }

                return info;
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

        public object EliminarFolioCuantificacion(string folioCuantificacion, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int folioCuant = Convert.ToInt32(folioCuantificacion);

                    //Obtengo los IDS que tengan el folio Cuantificacion a eliminar
                    List<int> idBulto = ctx.Sam3_Bulto
                        .Where(x => x.FolioCuantificacionID == folioCuant && x.Activo)
                        .Select(b => b.BultoID).AsParallel().ToList();

                    List<int> idItemCodeBulto = null;

                    //Obtengo los id de las relaciones con Itemcode
                    foreach (int bulto in idBulto)
                    {
                        idItemCodeBulto.AddRange((from rbi in ctx.Sam3_Rel_Bulto_ItemCode
                                                  join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                  where rbi.BultoID == bulto
                                                  select rid.Rel_ItemCode_Diametro_ID).AsParallel().ToList());

                        //ctx.Sam3_Rel_Bulto_ItemCode
                        //.Where(x => x.BultoID == bulto && x.Activo)
                        //.Select(b => b.ItemCodeID).AsParallel().ToList());
                    }

                    //obtengo los id de las relaciones con itemcode
                    List<int> idItemCode = (from rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                            join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                            where rfi.FolioCuantificacionID == folioCuant
                                            select rid.Rel_ItemCode_Diametro_ID).AsParallel().ToList();
                    //ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                    //.Where(x => x.FolioCuantificacionID == folioCuant)
                    //.Select(i => i.ItemCodeID).AsParallel().ToList();

                    List<bool> tieneNU = null;

                    //Verifico si tiene numeros unicos
                    foreach (int bu in idBulto)
                    {
                        tieneNU.AddRange(from bulto in ctx.Sam3_Rel_Bulto_ItemCode
                                         where bulto.BultoID == bu && bulto.Activo
                                         select bulto.TieneNumerosUnicos);
                    }

                    foreach (int ic in idItemCode)
                    {
                        tieneNU.AddRange(from rfc in ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                         where rfc.Activo && rfc.FolioCuantificacionID == folioCuant
                                         select rfc.TieneNumerosUnicos);
                    }

                    //Si no tiene numeros unicos se elimina
                    if (!tieneNU.Contains(true))
                    {
                        foreach (int id in idBulto)
                        {
                            Sam3_Bulto bulto = ctx.Sam3_Bulto
                                .Where(x => x.BultoID == id && x.Activo).AsParallel().SingleOrDefault();
                            bulto.Activo = false;
                            bulto.UsuarioModificacion = usuario.UsuarioID;
                            bulto.FechaModificacion = DateTime.Now;
                            ctx.SaveChanges();

                            foreach (int icid in idItemCodeBulto)
                            {
                                Sam3_Rel_Bulto_ItemCode relBulto = ctx.Sam3_Rel_Bulto_ItemCode
                               .Where(x => x.BultoID == id && x.Rel_ItemCode_Diametro_ID == icid && x.Activo).AsParallel().SingleOrDefault();
                                relBulto.Activo = false;
                                relBulto.UsuarioModificacion = usuario.UsuarioID;
                                relBulto.FechaModificacion = DateTime.Now;
                            }
                            ctx.SaveChanges();
                        }

                        foreach (int id in idItemCode)
                        {
                            //
                            Sam3_Rel_FolioCuantificacion_ItemCode folio = ctx.Sam3_Rel_FolioCuantificacion_ItemCode
                                .Where(x => x.Rel_ItemCode_Diametro_ID == id && x.FolioCuantificacionID == folioCuant && x.Activo).AsParallel().SingleOrDefault();
                            folio.Activo = false;
                            folio.UsuarioModificacion = usuario.UsuarioID;
                            folio.FechaModificacion = DateTime.Now;

                            Sam3_Rel_ItemCode_ItemCodeSteelgo rics = ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                               .Where(x => x.Rel_ItemCode_Diametro_ID == id).AsParallel().SingleOrDefault();

                            rics.Activo = false;
                            rics.UsuarioModificacion = usuario.UsuarioID;
                            rics.FechaModificacion = DateTime.Now;

                            ctx.SaveChanges();
                        }

                        foreach (int item in idItemCodeBulto)
                        {
                            Sam3_Rel_ItemCode_ItemCodeSteelgo rics = ctx.Sam3_Rel_ItemCode_ItemCodeSteelgo
                                .Where(x => x.Rel_ItemCode_Diametro_ID == item).AsParallel().SingleOrDefault();

                            rics.Activo = false;
                            rics.UsuarioModificacion = usuario.UsuarioID;
                            rics.FechaModificacion = DateTime.Now;
                        }

                        Sam3_FolioCuantificacion folioEliminar = ctx.Sam3_FolioCuantificacion
                            .Where(x => x.FolioCuantificacionID == folioCuant && x.Activo).AsParallel().SingleOrDefault();
                        folioEliminar.Activo = false;
                        folioEliminar.UsuarioModificacion = usuario.UsuarioID;
                        folioEliminar.FechaModificacion = DateTime.Now;
                        ctx.SaveChanges();
                    }


                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
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

        public object ListadoFoliosCuantificacionProProyecto(int proyectoID, Sam3_Usuario usuario)
        {
            string errorInfo = string.Empty;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                Boolean activarFolioConfiguracionCuantificacion = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"]) ?
                    (ConfigurationManager.AppSettings["ActivarFolioConfiguracionCuantificacion"].Equals("1") ? true : false) : false;

                errorInfo += "\nActiva confgi folio c: " + activarFolioConfiguracionCuantificacion.ToString();

                bool activaConfigFolioLlegada = ConfigurationManager.AppSettings["ActivarFolioConfiguracion"].Equals("1") ? true : false;
                errorInfo += "\nActiva Config Folio Llegada: " + activaConfigFolioLlegada.ToString();

                using (SamContext ctx = new SamContext())
                {
                    List<ListaCombos> listado = (from fc in ctx.Sam3_FolioCuantificacion
                                                 join rfc in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on fc.FolioCuantificacionID equals rfc.FolioCuantificacionID
                                                 join rnufc in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rfc.Rel_FolioCuantificacion_ItemCode_ID equals rnufc.Rel_FolioCuantificacion_ItemCode_ID
                                                 where fc.Activo && rfc.Activo && rnufc.Activo //&& rnufc.Rel_NumeroUnico_RelFC_RelB_ID== 9938
                                                 && fc.ProyectoID == proyectoID
                                                 select new ListaCombos
                                                 {
                                                     id = fc.FolioCuantificacionID.ToString(),
                                                     value = fc.FolioCuantificacionID.ToString()
                                                 }).AsParallel().Distinct().ToList();

                    listado.AddRange((from fc in ctx.Sam3_FolioCuantificacion
                                      join b in ctx.Sam3_Bulto on fc.FolioCuantificacionID equals b.FolioCuantificacionID
                                      join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                      join rnufc in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rbi.Rel_Bulto_ItemCode_ID equals rnufc.Rel_Bulto_ItemCode_ID
                                      where fc.Activo && b.Activo && rbi.Activo && rnufc.Activo
                                      && fc.ProyectoID == proyectoID
                                      select new ListaCombos
                                      {
                                          id = fc.FolioCuantificacionID.ToString(),
                                          value = fc.FolioCuantificacionID.ToString()
                                      }).Distinct().ToList());

                    listado = listado.GroupBy(x => x.id).Select(x => x.First()).ToList();

                    errorInfo += "\nListado : " + serializer.Serialize(listado);

                    if (listado.Count() <= 0 || listado == null)
                    {
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Sin Datos que mostrar");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;

                        return result;
                    }

                    foreach (ListaCombos item in listado)
                    {
                        int foliocuantificacionid = Convert.ToInt32(item.id);

                        Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == foliocuantificacionid).FirstOrDefault();

                        Sam3_FolioAvisoLlegada folioLlegada = (from fa in ctx.Sam3_FolioAvisoLlegada
                                                               join fe in ctx.Sam3_FolioAvisoEntrada on fa.FolioAvisoLlegadaID equals fe.FolioAvisoLlegadaID
                                                               join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                                               where fa.Activo && fe.Activo && fc.Activo
                                                               && fc.FolioCuantificacionID == folioCuantificacion.FolioCuantificacionID
                                                               select fa).AsParallel().SingleOrDefault();

                        errorInfo += "\nFolioCuantiFicacionID :" + foliocuantificacionid.ToString();
                        errorInfo += "\nFolio Cuantificacion objeto: " + folioCuantificacion.FolioCuantificacionID.ToString();
                        errorInfo += "\nFolio Llegada : " + folioLlegada != null ? "true, Folio : " + folioLlegada.FolioAvisoLlegadaID.ToString() : "false";

                        string NombreFolioAvisoLlegada = string.Empty;

                        if (folioLlegada != null)
                        {
                            NombreFolioAvisoLlegada = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                       where pc.Proyecto == folioLlegada.ProyectoNombrado
                                                       && pc.Entidad == folioLlegada.Entidad
                                                       select pc.PreFijoFolioAvisoLlegada + ","
                                                       + pc.CantidadCerosFolioAvisoLlegada.ToString() + ","
                                                       + folioLlegada.Consecutivo + ","
                                                       + pc.PostFijoFolioAvisoLlegada.Trim()).FirstOrDefault();
                        }

                        errorInfo += "\nNombre llegada: " + NombreFolioAvisoLlegada;

                        string NombreFolioCuantificacion = string.Empty;
                        if (folioCuantificacion != null)
                        {
                            NombreFolioCuantificacion = (from pc in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion
                                                                where pc.Rel_Proyecto_Entidad_Configuracion_ID == folioCuantificacion.Rel_Proyecto_Entidad_Configuracion_ID
                                                                select pc.PreFijoFolioPackingList + ","
                                                                + pc.CantidadCerosFolioPackingList.ToString() + ","
                                                                + folioCuantificacion.ConsecutivoConfiguracion.ToString() + ","
                                                                + pc.PostFijoFolioPackingList).FirstOrDefault();
                        }

                        errorInfo += "\nNombre cuantificacion : " + NombreFolioCuantificacion;

                        int FolioAvisoLlegadaID = folioLlegada.FolioAvisoLlegadaID;
                        int ConsecutivoFolioCuanificacion = folioCuantificacion.Consecutivo.Value;
                        int ConsecutivoFolioLlegada = folioLlegada.Consecutivo.Value;

                        errorInfo += "\nFolioAvisoLlegadaID : " + FolioAvisoLlegadaID.ToString();
                        errorInfo += "\nConsecutivoCuantificacion :" + ConsecutivoFolioCuanificacion.ToString();
                        errorInfo += "\nConsecutivo llegada: " + ConsecutivoFolioLlegada.ToString();

                        if (NombreFolioAvisoLlegada.Length > 0)
                        {
                            NombreFolioAvisoLlegada = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioAvisoLlegada);
                        }

                        if (NombreFolioCuantificacion.Length > 0)
                        {
                            NombreFolioCuantificacion = Conversiones.Instance.FormatearCadenasdeElementos(NombreFolioCuantificacion);
                        }

                        if (activaConfigFolioLlegada && activarFolioConfiguracionCuantificacion)
                        {
                            item.value = NombreFolioAvisoLlegada + "-" + NombreFolioCuantificacion;
                        }

                        if (activaConfigFolioLlegada && !activarFolioConfiguracionCuantificacion)
                        {
                            item.value = NombreFolioAvisoLlegada + "-" + ConsecutivoFolioCuanificacion;
                        }

                        if (!activaConfigFolioLlegada && activarFolioConfiguracionCuantificacion)
                        {
                            item.value = FolioAvisoLlegadaID + "-" + NombreFolioCuantificacion;
                        }

                        if (!activaConfigFolioLlegada && !activarFolioConfiguracionCuantificacion)
                        {
                            item.value = FolioAvisoLlegadaID + "-" + ConsecutivoFolioCuanificacion;
                        }
                    }


                    return listado.OrderBy(x => x.value).ToList();
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
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

        public object CambiarEstatus(int folio, string estatus, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_FolioCuantificacion folioCuantificacion = ctx.Sam3_FolioCuantificacion.Where(x => x.FolioCuantificacionID == folio).SingleOrDefault();
                    folioCuantificacion.Estatus = estatus;
                    folioCuantificacion.FechaModificacion = DateTime.Now;
                    folioCuantificacion.UsuarioModificacion = usuario.UsuarioID;
                    ctx.SaveChanges();

                    if (estatus != "Recepción Terminada")
                    {
                        List<Sam3_ItemCode> Itemcodes = (from fc in ctx.Sam3_FolioCuantificacion
                                                         join rfi in ctx.Sam3_Rel_FolioCuantificacion_ItemCode on fc.FolioCuantificacionID equals rfi.FolioCuantificacionID
                                                         join rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rfi.Rel_FolioCuantificacion_ItemCode_ID equals rel.Rel_FolioCuantificacion_ItemCode_ID
                                                         join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                                         join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                                         join rid in ctx.Sam3_Rel_ItemCode_Diametro on rfi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                                         join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                                         join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                                         where fc.Activo && rfi.Activo && it.Activo && nu.Activo && rel.Activo && rid.Activo && d1.Activo && d2.Activo
                                                         && fc.FolioCuantificacionID == folio
                                                         select it).AsParallel().Distinct().ToList();


                        Itemcodes.AddRange((from fc in ctx.Sam3_FolioCuantificacion
                                            join b in ctx.Sam3_Bulto on fc.FolioCuantificacionID equals b.FolioCuantificacionID
                                            join rbi in ctx.Sam3_Rel_Bulto_ItemCode on b.BultoID equals rbi.BultoID
                                            join rel in ctx.Sam3_Rel_NumeroUnico_RelFC_RelB on rbi.Rel_Bulto_ItemCode_ID equals rel.Rel_Bulto_ItemCode_ID
                                            join nu in ctx.Sam3_NumeroUnico on rel.NumeroUnicoID equals nu.NumeroUnicoID
                                            join it in ctx.Sam3_ItemCode on nu.ItemCodeID equals it.ItemCodeID
                                            join rid in ctx.Sam3_Rel_ItemCode_Diametro on rbi.Rel_ItemCode_Diametro_ID equals rid.Rel_ItemCode_Diametro_ID
                                            join d1 in ctx.Sam3_Diametro on rid.Diametro1ID equals d1.DiametroID
                                            join d2 in ctx.Sam3_Diametro on rid.Diametro2ID equals d2.DiametroID
                                            where fc.Activo && b.Activo && rbi.Activo && it.Activo && nu.Activo && rel.Activo && rid.Activo && d1.Activo && d2.Activo
                                            && fc.FolioCuantificacionID == folio
                                            select it).AsParallel().Distinct().ToList()
                                        );

                        foreach (Sam3_ItemCode item in Itemcodes)
                        {
                            Sam3_ItemCode it = ctx.Sam3_ItemCode.Where(x => x.ItemCodeID == item.ItemCodeID).FirstOrDefault();
                            it.TieneComplementoRecepcion = false;
                            it.FechaModificacion = DateTime.Now;
                            it.UsuarioModificacion = usuario.UsuarioID;
                            ctx.SaveChanges();
                        };
                    };

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("OK");
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

        public List<ListadoIncidencias> ListadoIncidencias(int clienteID, int proyectoID, List<int> proyectos, List<int> patios,
            List<int> foliosCuantificacionIDs, DateTime fechaInicial, DateTime fechaFinal)
        {
            try
            {
                List<ListadoIncidencias> listado;
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"]) ?
                    (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_FolioCuantificacion> registros = new List<Sam3_FolioCuantificacion>();
                    if (proyectoID > 0)
                    {
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fc.Activo && fc.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && patios.Contains(fe.PatioID)
                                     && p.ProyectoID == proyectoID
                                     && foliosCuantificacionIDs.Contains(fc.FolioCuantificacionID)
                                     select fc).AsParallel().Distinct().ToList();
                    }
                    else
                    {
                        registros = (from fe in ctx.Sam3_FolioAvisoEntrada
                                     join fc in ctx.Sam3_FolioCuantificacion on fe.FolioAvisoEntradaID equals fc.FolioAvisoEntradaID
                                     join rfp in ctx.Sam3_Rel_FolioAvisoLlegada_Proyecto on fe.FolioAvisoLlegadaID equals rfp.FolioAvisoLlegadaID
                                     join p in ctx.Sam3_Proyecto on rfp.ProyectoID equals p.ProyectoID
                                     join pa in ctx.Sam3_Patio on p.PatioID equals pa.PatioID
                                     where fc.Activo && fc.Activo
                                     && proyectos.Contains(p.ProyectoID)
                                     && patios.Contains(pa.PatioID)
                                     && patios.Contains(fe.PatioID)
                                     && foliosCuantificacionIDs.Contains(fc.FolioCuantificacionID)
                                     select fc).AsParallel().Distinct().ToList();
                    }

                    if (clienteID > 0)
                    {
                        int sam3Cliente = (from c in ctx.Sam3_Cliente
                                           where c.Activo && c.Sam2ClienteID == clienteID
                                           select c.ClienteID).AsParallel().SingleOrDefault();
                        registros = (from r in registros
                                     join fe in ctx.Sam3_FolioAvisoEntrada on r.FolioAvisoEntradaID equals fe.FolioAvisoEntradaID
                                     where fe.Activo && fe.ClienteID == sam3Cliente
                                     select r).AsParallel().Distinct().ToList();
                    }

                    listado = (from r in registros
                               join rfi in ctx.Sam3_Rel_Incidencia_FolioCuantificacion on r.FolioCuantificacionID equals rfi.FolioCuantificacionID
                               join inc in ctx.Sam3_Incidencia on rfi.IncidenciaID equals inc.IncidenciaID
                               join c in ctx.Sam3_ClasificacionIncidencia on inc.ClasificacionID equals c.ClasificacionIncidenciaID
                               join tpi in ctx.Sam3_TipoIncidencia on inc.TipoIncidenciaID equals tpi.TipoIncidenciaID
                               where rfi.Activo && inc.Activo
                               select new ListadoIncidencias
                               {
                                   Clasificacion = c.Nombre,
                                   Estatus = inc.Estatus,
                                   FechaRegistro = inc.FechaCreacion.ToString(),
                                   FolioIncidenciaID = inc.IncidenciaID.ToString(),
                                   FolioOriginalID = inc.IncidenciaOriginalID.ToString(),
                                   RegistradoPor = (from us in ctx.Sam3_Usuario
                                                    where us.Activo
                                                    && us.UsuarioID == inc.UsuarioID
                                                    select us.Nombre + " " + us.ApellidoPaterno).SingleOrDefault(),
                                   TipoIncidencia = tpi.Nombre,
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

        /// <summary>
        /// Funcion para obtener los proyectos para el combo Proyecto de cuantificacion
        /// sin incluir el Proyecto Default
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerListadoProyectosCuantificacion(Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<int> patios;
                    List<int> proyectos;
                    UsuarioBd.Instance.ObtenerPatiosYProyectosDeUsuario(usuario.UsuarioID, out proyectos, out patios);

                    List<Proyecto> lstProyectos = (from r in ctx.Sam3_Proyecto
                                                   join c in ctx.Sam3_Cliente on r.ClienteID equals c.ClienteID
                                                   where r.Activo && proyectos.Contains(r.ProyectoID) && r.ProyectoID != 1
                                                   select new Proyecto
                                                   {
                                                       Nombre = r.Nombre,
                                                       ProyectoID = r.ProyectoID.ToString(),
                                                       ClienteID = c.Sam2ClienteID.ToString()
                                                   }).AsParallel().ToList();

                    lstProyectos = lstProyectos.GroupBy(x => x.ProyectoID).Select(x => x.First()).ToList();
                    return lstProyectos;
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

        public object ObtenerTipoUsoPorProyecto(int proyectoID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    TipoUso tipoUso = (from pc in ctx.Sam3_ProyectoConfiguracion
                                       join tu in ctx.Sam3_TipoUso on pc.TipoUsoID equals tu.TipoUsoID
                                       where pc.ProyectoID == proyectoID && pc.Activo && tu.Activo
                                       select new TipoUso
                                       {
                                           id = tu.TipoUsoID.ToString(),
                                           Nombre = tu.Nombre
                                       }).AsParallel().SingleOrDefault();

                    return tipoUso;
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

        public object ObtenerTipoPackingListConfiguracion(string proyectoID)
        {
            try
            {
                int proyecto = proyectoID != "" ? Convert.ToInt32(proyectoID) : 0;

                using (SamContext ctx = new SamContext())
                {
                    TipoPackingList tipoPackingList = new TipoPackingList();

                    tipoPackingList.MostrarTipoPackingList = (from p in ctx.Sam3_ProyectoConfiguracion
                                                              where p.Activo
                                                              && p.ProyectoID.ToString() == proyectoID
                                                              select p.RequiereTipoPackingList).AsParallel().SingleOrDefault();


                    return tipoPackingList;
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