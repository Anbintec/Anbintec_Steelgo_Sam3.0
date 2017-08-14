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
using System.Configuration;

namespace BackEndSAM.DataAcces
{
    /// <summary>
    /// Contiene operaciones acerca de los documentos cargados. Esta seccion esta planeada para migrarse al proyecto FileManager
    /// </summary>
    public class DocumentosBd
    {
        private static readonly object _mutex = new object();
        private static DocumentosBd _instance;

        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private DocumentosBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static DocumentosBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DocumentosBd();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Guarda los documentos que se cargan para los folios de aviso de llegada
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool GuardarArchivosFolioAvisoLlegada(List<DocumentoPosteado> files)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    foreach (DocumentoPosteado f in files)
                    {
                        int tipoArchivoId = ctx.Sam3_TipoArchivo.Where(x => x.Nombre == f.TipoArchivoPaseSalida && x.Activo).Select(x => x.TipoArchivoID)
                      .AsParallel().SingleOrDefault();


                        Sam3_Rel_FolioAvisoLlegada_Documento nuevoDoc = new Sam3_Rel_FolioAvisoLlegada_Documento();
                        nuevoDoc.Activo = true;
                        nuevoDoc.DocumentoID = 0;
                        nuevoDoc.DocGuid = f.DocGuid;
                        nuevoDoc.Extencion = f.Extencion;
                        nuevoDoc.FechaModificacion = DateTime.Now;
                        nuevoDoc.FolioAvisoLlegadaID = f.FolioAvisoLlegadaID.Value;
                        nuevoDoc.Nombre = f.FileName;
                        nuevoDoc.Url = f.Path;
                        nuevoDoc.UsuarioModificacion = f.UserId;
                        nuevoDoc.TipoArchivoID = tipoArchivoId;
                        nuevoDoc.ContentType = f.ContentType;

                        ctx.Sam3_Rel_FolioAvisoLlegada_Documento.Add(nuevoDoc);
                    }

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        /// <summary>
        /// Funccion para guardar un documento para un catalogo
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool GuardarDocumentoCatalogos(List<DocumentosEnCatalogos> files)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    foreach (DocumentosEnCatalogos f in files)
                    {
                        Sam3_Rel_Catalogos_Documento nuevoDoc = new Sam3_Rel_Catalogos_Documento();
                        nuevoDoc.Activo = true;
                        nuevoDoc.DocumentoID = 0;
                        nuevoDoc.DocGuid = f.DocGuid;
                        nuevoDoc.Extension = f.Extension;
                        nuevoDoc.FechaModificacion = DateTime.Now;
                        nuevoDoc.CatalogoID = Int32.Parse(f.CatalogoID);
                        nuevoDoc.ElementoCatalogoID = Int32.Parse(f.ElementoCatalogoID);
                        nuevoDoc.Nombre = f.FileName;
                        nuevoDoc.Url = f.Path;
                        nuevoDoc.UsuarioModificacion = f.UserId;
                        nuevoDoc.TipoArchivoID = f.TipoArchivoID;
                        nuevoDoc.ContentType = f.ContentType;

                        ctx.Sam3_Rel_Catalogos_Documento.Add(nuevoDoc);
                    }

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        /// <summary>
        /// Guarda registrso de los documentos cargados en para los avisos de entrada
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public bool GuardarArchivosFolioAvisoEntrada(List<DocumentoPosteado> files)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {

                    foreach (DocumentoPosteado f in files)
                    {
                        Sam3_Rel_FolioAvisoEntrada_Documento nuevoDoc = new Sam3_Rel_FolioAvisoEntrada_Documento();
                        nuevoDoc.Activo = true;
                        nuevoDoc.DocumentoID = 0;
                        nuevoDoc.DocGuid = f.DocGuid;
                        nuevoDoc.Extencion = f.Extencion;
                        nuevoDoc.FechaModificacion = DateTime.Now;
                        nuevoDoc.FolioAvisoEntradaID = f.FolioAvisoEntradaID;
                        nuevoDoc.Nombre = f.FileName;
                        nuevoDoc.Url = f.Path;
                        nuevoDoc.UsuarioModificacion = f.UserId;
                        nuevoDoc.TipoArchivoID = f.TipoArchivoID;
                        nuevoDoc.ContentType = f.ContentType;
                        nuevoDoc.Descripcion = f.Descripcion;

                        ctx.Sam3_Rel_FolioAvisoEntrada_Documento.Add(nuevoDoc);
                    }

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        /// <summary>
        /// Genera un listado de los documentos que estan relacionados con un aviso de llegada
        /// </summary>
        /// <param name="folioAvisoLlegadaId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerDocumentosFolioAvisoLlegada(int folioAvisoLlegadaId, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                   
                    List<ListaDocumentos> documentos = (from r in ctx.Sam3_FolioAvisoLlegada
                                                        join d in ctx.Sam3_Rel_FolioAvisoLlegada_Documento on r.FolioAvisoLlegadaID equals d.FolioAvisoLlegadaID
                                                        join t in ctx.Sam3_TipoArchivo on d.TipoArchivoID equals t.TipoArchivoID
                                                        where r.Activo == true && r.FolioAvisoLlegadaID == folioAvisoLlegadaId && d.Activo
                                                        select new ListaDocumentos
                                                        {
                                                            DocumentoID = d.Rel_FolioAvisoLlegada_DocumentoID.ToString(),
                                                            Nombre = d.Nombre,
                                                            Extencion = d.Extencion,
                                                            Url = d.Url,
                                                            TipoArchivo = t.Nombre
                                                        }).AsParallel().ToList();
                    return documentos;
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
        /// Genera un listado de los documentos relacionados con un aviso de entrada
        /// </summary>
        /// <param name="folioAvisoEntradaId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerDocumentosFolioAvisoEntrada(int folioAvisoEntradaId, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaDocumentos> documentos = (from r in ctx.Sam3_FolioAvisoEntrada
                                                        join d in ctx.Sam3_Rel_FolioAvisoEntrada_Documento on r.FolioAvisoEntradaID equals d.FolioAvisoEntradaID
                                                        where r.Activo && r.FolioAvisoEntradaID == folioAvisoEntradaId && d.Activo
                                                        select new ListaDocumentos
                                                        {
                                                            DocumentoID = d.Rel_FolioAvisoEntrada_DocumentoID.ToString(),
                                                            Nombre = d.Nombre,
                                                            Extencion = d.Extencion,
                                                            Url = d.Url,
                                                            TipoArchivo = string.Empty,
                                                            Descripcion= d.Descripcion
                                                        }).AsParallel().ToList();
                    return documentos;
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
        /// Genera un listado de los documentos relacionados a un permiso de aduana
        /// </summary>
        /// <param name="folioAvisoLlegadaId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerDocumentosPermisoAduana(int folioAvisoLlegadaId, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaDocumentos> documentos = (from r in ctx.Sam3_FolioAvisoLlegada
                                                        join p in ctx.Sam3_PermisoAduana on r.FolioAvisoLlegadaID equals p.FolioAvisoLlegadaID
                                                        join d in ctx.Sam3_Rel_PermisoAduana_Documento on p.PermisoAduanaID equals d.PermisoAduanaID
                                                        where r.Activo == true && r.FolioAvisoLlegadaID == folioAvisoLlegadaId && d.Activo
                                                        select new ListaDocumentos
                                                        {
                                                            DocumentoID = d.Rel_Permiso_Documento_ID.ToString(),
                                                            Nombre = d.Nombre,
                                                            Extencion = d.Extencion,
                                                            Url = d.Url
                                                        }).AsParallel().ToList();
                    return documentos;
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
        /// Genera un listado de los documentos relacionados con un pase de salida
        /// </summary>
        /// <param name="folioAvisoLlegadaId"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object ObtenerDocumentosPaseSalida(int folioAvisoLlegadaId, Sam3_Usuario usuario)
        {
            try
            {
                Boolean ActivarFolioConfiguracionIncidencias = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"])
                    ? (ConfigurationManager.AppSettings["ActivarFolioConfiguracionIncidencias"].Equals("1") ? true : false) : false;

                using (SamContext ctx = new SamContext())
                {
                    List<ListaDocumentos> documentos = (from r in ctx.Sam3_FolioAvisoLlegada
                                                        join d in ctx.Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo on r.FolioAvisoLlegadaID equals d.FolioAvisoLlegadaID
                                                        join t in ctx.Sam3_TipoArchivo on d.TipoArchivoID equals t.TipoArchivoID
                                                        where r.Activo == true && r.FolioAvisoLlegadaID == folioAvisoLlegadaId && d.Activo
                                                        select new ListaDocumentos
                                                        {
                                                            DocumentoID = d.Rel_Folio_PaseSalida_Archivo_ID.ToString(),
                                                            Nombre = d.Nombre,
                                                            Extencion = d.Extencion,
                                                            Url = d.Url,
                                                            TipoArchivo = t.Nombre,
                                                            IncidenciaID = d.IncidenciaID,
                                                            NombreIncidencia = ActivarFolioConfiguracionIncidencias ? 
                                                                (from inc in ctx.Sam3_Incidencia 
                                                                 join rpp in ctx.Sam3_Rel_Proyecto_Entidad_Configuracion on inc.Rel_Proyecto_Entidad_Configuracion_ID equals rpp.Rel_Proyecto_Entidad_Configuracion_ID
                                                                 where inc.Activo && rpp.Activo == 1
                                                                 && d.IncidenciaID.Value > 0 && d.IncidenciaID != null
                                                                 && inc.IncidenciaID == d.IncidenciaID.Value
                                                                 select rpp.PreFijoFolioIncidencias + ","
                                                                    + rpp.CantidadCerosFolioIncidencias.ToString() + ","
                                                                    + inc.Consecutivo.ToString() + ","
                                                                    + rpp.PostFijoFolioIncidencias).FirstOrDefault()
                                                                : string.Empty
                                                        }).AsParallel().ToList();

                    if (ActivarFolioConfiguracionIncidencias)
                    {
                        foreach (ListaDocumentos datos in documentos)
                        {
                            if (datos.NombreIncidencia != string.Empty && datos.NombreIncidencia != null)
                            {
                                string[] elemntos = datos.NombreIncidencia.Split(',').ToArray();
                                int digitos = Convert.ToInt32(elemntos[1]);
                                int consecutivo = Convert.ToInt32(elemntos[2]);
                                string formato = "D" + digitos.ToString();

                                datos.NombreIncidencia = elemntos[0].Trim() + consecutivo.ToString(formato).Trim() + elemntos[3].Trim();
                            }
                        }
                    }

                    return documentos;
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

        public object obtenerDocumentoCatalogos(int catalogoID, int elementoCatalogoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<ListaDocumentosCatalogos> documentos = (from c in ctx.Sam3_Catalogos
                                                                 join cd in ctx.Sam3_Rel_Catalogos_Documento on c.CatalogoID equals cd.CatalogoID
                                                                 join ta in ctx.Sam3_TipoArchivo_Catalogo on cd.TipoArchivoID equals ta.TipoArchivoID
                                                                 where c.CatalogoID == catalogoID && cd.ElementoCatalogoID == elementoCatalogoID
                                                                 && c.Activo && cd.Activo && ta.Activo
                                                                 select new ListaDocumentosCatalogos
                                                                 {
                                                                     DocumentoID = cd.Rel_Catalogos_DocumentoID.ToString(),
                                                                     Nombre = cd.Nombre,
                                                                     Extencion = cd.Extension,
                                                                     Url = cd.Url,
                                                                     TipoArchivoID = ta.TipoArchivoID.ToString(),
                                                                     TipoArchivo = ta.Nombre,
                                                                     CatalogoID = c.CatalogoID.ToString(),
                                                                     CatalogoNombre = c.CatalogoNombre,
                                                                     ElementoCatalogoID = cd.ElementoCatalogoID.ToString()
                                                                 }).AsParallel().ToList();

                    return documentos;
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
        /// Guarda registro de los documentos cargados para permiso de aduana
        /// </summary>
        /// <param name="documentos"></param>
        /// <returns></returns>
        public object GuardarDocumentoPermisoAduana(List<DocumentoPosteado> documentos)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int tipoArchivoID = ctx.Sam3_TipoArchivo.Where(x => x.Nombre == "Permiso Aduana").Select(x => x.TipoArchivoID).SingleOrDefault();
                    Sam3_PermisoAduana permisoBd;
                    int folioAviso = documentos[0].FolioAvisoLlegadaID.Value;
                    //Actualizamos el permiso de aduana
                    if (ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == folioAviso && x.Activo).Any())
                    {
                        permisoBd = ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == folioAviso && x.Activo)
                            .AsParallel().SingleOrDefault();

                        permisoBd.PermisoAutorizado = true;
                        permisoBd.PermisoTramite = false;
                        permisoBd.NumeroPermiso = documentos[0].NumeroPermisoAduana;
                        permisoBd.Estatus = "Autorizado";
                        permisoBd.FechaAutorización = DateTime.Now;
                        permisoBd.FechaModificacion = DateTime.Now;
                        permisoBd.UsuarioModificacion = documentos[0].UserId;
                        ctx.SaveChanges();
                    }
                    else 
                    {
                        permisoBd = new Sam3_PermisoAduana();

                        permisoBd.PermisoAutorizado = true;
                        permisoBd.PermisoTramite = false;
                        permisoBd.NumeroPermiso = documentos[0].NumeroPermisoAduana;
                        permisoBd.Estatus = "Autorizado";
                        permisoBd.FechaAutorización = DateTime.Now;
                        permisoBd.FechaModificacion = DateTime.Now;
                        permisoBd.UsuarioModificacion = documentos[0].UserId;

                        ctx.Sam3_PermisoAduana.Add(permisoBd);
                        ctx.SaveChanges();
 
                    }

                    //Actualizar estatus de FolioAvisoLlegada
                    //Sam3_FolioAvisoLlegada aviso = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == folioAviso)
                    //    .AsParallel().SingleOrDefault();
                    //aviso.Estatus = "Autorizado";
                    //aviso.FechaModificacion = DateTime.Now;
                    //aviso.UsuarioModificacion = documentos[0].UserId;

                    if (ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID == permisoBd.PermisoAduanaID && x.Activo).Any())
                    {
                        foreach (DocumentoPosteado d in documentos)
                        {
                            //Actualizar la informacion de los documentos
                            Sam3_Rel_PermisoAduana_Documento nuevoDoc = ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID == permisoBd.PermisoAduanaID && x.Activo).AsParallel().SingleOrDefault();
                            nuevoDoc.Activo = true;
                            nuevoDoc.ContentType = d.ContentType;
                            nuevoDoc.DocGuid = d.DocGuid;
                            nuevoDoc.DocumentoID = 0;
                            nuevoDoc.Extencion = d.Extencion;
                            nuevoDoc.FechaModificacion = DateTime.Now;
                            nuevoDoc.Nombre = d.FileName;
                            nuevoDoc.PermisoAduanaID = permisoBd.PermisoAduanaID;
                            nuevoDoc.TipoArchivoID = tipoArchivoID;
                            nuevoDoc.Url = d.Path;
                            nuevoDoc.UsuarioModificacion = d.UserId;

                            //ctx.Sam3_Rel_PermisoAduana_Documento.Add(nuevoDoc);
                        }
                    }
                    else
                    {
                        foreach (DocumentoPosteado d in documentos)
                        {
                            //Guardamos la informacion de los documentos
                            Sam3_Rel_PermisoAduana_Documento nuevoDoc = new Sam3_Rel_PermisoAduana_Documento();
                            nuevoDoc.Activo = true;
                            nuevoDoc.ContentType = d.ContentType;
                            nuevoDoc.DocGuid = d.DocGuid;
                            nuevoDoc.DocumentoID = 0;
                            nuevoDoc.Extencion = d.Extencion;
                            nuevoDoc.FechaModificacion = DateTime.Now;
                            nuevoDoc.Nombre = d.FileName;
                            nuevoDoc.PermisoAduanaID = permisoBd.PermisoAduanaID;
                            nuevoDoc.TipoArchivoID = tipoArchivoID;
                            nuevoDoc.Url = d.Path;
                            nuevoDoc.UsuarioModificacion = d.UserId;

                            ctx.Sam3_Rel_PermisoAduana_Documento.Add(nuevoDoc);
                        }
                    }

                    
                    

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        public object CambiarEstatusFolio(int FolioAviso, string NumeroPermiso, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int permisoID = (from per in ctx.Sam3_PermisoAduana
                                     where per.FolioAvisoLlegadaID == FolioAviso && per.Activo && per.Estatus == "Autorizado"
                                     select per.PermisoAduanaID).AsParallel().SingleOrDefault();

                    if (ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == FolioAviso && x.Activo).Any())
                    {
                        Sam3_PermisoAduana permisoBd = ctx.Sam3_PermisoAduana.Where(x => x.FolioAvisoLlegadaID == FolioAviso && x.Activo)
                            .AsParallel().SingleOrDefault();

                        permisoBd.PermisoAutorizado = true;
                        permisoBd.PermisoTramite = false;
                        permisoBd.NumeroPermiso = NumeroPermiso;
                        permisoBd.Estatus = "Autorizado";
                        permisoBd.FechaAutorización = DateTime.Now;
                        permisoBd.FechaModificacion = DateTime.Now;
                        permisoBd.UsuarioModificacion = usuario.UsuarioID;
                        ctx.SaveChanges();
                    }

                    if (ctx.Sam3_Rel_PermisoAduana_Documento.Where(x => x.PermisoAduanaID == permisoID && x.Activo).Any())
                    {
                        //Actualizar estatus de FolioAvisoLlegada
                        Sam3_FolioAvisoLlegada aviso = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == FolioAviso)
                            .AsParallel().SingleOrDefault();
                        aviso.Estatus = "Autorizado";
                        aviso.FechaModificacion = DateTime.Now;
                        aviso.UsuarioModificacion = usuario.UsuarioID;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                    {
                        throw new Exception("El folio no cuenta con un permiso Autorizado");
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
        /// Guarda Registro de los documentos cargados para el pase de salida
        /// </summary>
        /// <param name="documentos"></param>
        /// <returns></returns>
        public object GuardarDocumentoPaseSalida(List<DocumentoPosteado> documentos)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int Folio = documentos[0].FolioAvisoLlegadaID.GetValueOrDefault();
                    //Actualizamos el dato de Pase Salida Enviado del Folio aviso Llegada
                    Sam3_FolioAvisoLlegada folioBd = ctx.Sam3_FolioAvisoLlegada.Where(x => x.FolioAvisoLlegadaID == Folio && x.Activo)
                        .AsParallel().SingleOrDefault();

                    foreach (DocumentoPosteado d in documentos)
                    {
                        int tipoArchivoId = ctx.Sam3_TipoArchivo.Where(x => x.Nombre == d.TipoArchivoPaseSalida && x.Activo).Select(x => x.TipoArchivoID)
                        .AsParallel().SingleOrDefault();

                        Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo nuevoDoc = new Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo();
                        nuevoDoc.Activo = true;
                        nuevoDoc.ContentType = d.ContentType;
                        nuevoDoc.DocGuid = d.DocGuid;
                        nuevoDoc.DocumentoID = 0;
                        nuevoDoc.Extencion = d.Extencion;
                        nuevoDoc.FechaModificacion = DateTime.Now;
                        nuevoDoc.FolioAvisoLlegadaID = folioBd.FolioAvisoLlegadaID;
                        nuevoDoc.Nombre = d.FileName;
                        nuevoDoc.TipoArchivoID = tipoArchivoId;
                        nuevoDoc.Url = d.Path;
                        nuevoDoc.UsuarioModificacion = d.UserId;
                        nuevoDoc.IncidenciaID = d.IncidenciaID == -1 ? null : d.IncidenciaID;

                        ctx.Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo.Add(nuevoDoc);
                    }

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        /// <summary>
        /// Elimina logicamente un documento de aviso de llegada
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDocumentoAvisoLlegada(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_FolioAvisoLlegada_Documento docDb = ctx.Sam3_Rel_FolioAvisoLlegada_Documento
                        .Where(x => x.Rel_FolioAvisoLlegada_DocumentoID == documentoID).AsParallel().SingleOrDefault();

                    docDb.Activo = false;
                    docDb.FechaModificacion = DateTime.Now;
                    docDb.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Funcion para eliminar un documento de catalogos
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDocumentoCatalogos(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_Catalogos_Documento cat = ctx.Sam3_Rel_Catalogos_Documento.Where(x => x.Rel_Catalogos_DocumentoID == documentoID && x.Activo).AsParallel().SingleOrDefault();
                    cat.Activo = false;
                    cat.FechaModificacion = DateTime.Now;
                    cat.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Elimina logicamente un docuemento de un aviso de entrada
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDocumentoAvisoEntrada(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_FolioAvisoEntrada_Documento docDb = ctx.Sam3_Rel_FolioAvisoEntrada_Documento
                        .Where(x => x.Rel_FolioAvisoEntrada_DocumentoID == documentoID).AsParallel().SingleOrDefault();

                    docDb.Activo = false;
                    docDb.FechaModificacion = DateTime.Now;
                    docDb.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Elimina logicamente un documento de permiso de aduana
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDocumentoPermisoAduana(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_PermisoAduana_Documento docBd = ctx.Sam3_Rel_PermisoAduana_Documento
                        .Where(x => x.Rel_Permiso_Documento_ID == documentoID).AsParallel().SingleOrDefault();

                    docBd.Activo = false;
                    docBd.FechaModificacion = DateTime.Now;
                    docBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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
        /// Elimina logicamente un documento de pase de salida
        /// </summary>
        /// <param name="documentoID"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public object EliminarDocumentoPaseSalida(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo docBd = ctx.Sam3_Rel_FolioAvisoLlegada_PaseSalida_Archivo
                        .Where(x => x.Rel_Folio_PaseSalida_Archivo_ID == documentoID).AsParallel().SingleOrDefault();

                    docBd.Activo = false;
                    docBd.FechaModificacion = DateTime.Now;
                    docBd.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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

        public bool GuardarDocumentoIncidencia(List<DocumentoPosteado> documentos, string estatusDocumento)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int incidenciaID = documentos[0].IncidenciaID.Value;
                    int tipoDocumento = 0;

                    switch (estatusDocumento)
                    {
                        case "Incidencia" :
                            tipoDocumento = 1;
                            break;
                        case "Resolver" :
                            tipoDocumento = 2;
                            break;
                        case "Responder" :
                            tipoDocumento = 3;
                            break;
                        case "Cancelar":
                            tipoDocumento = 4;
                            break;
                    }

                    //Guardamos la informacion de los documentos
                    foreach (DocumentoPosteado d in documentos)
                    {
                        Sam3_Rel_Incidencia_Documento nuevoDoc = new Sam3_Rel_Incidencia_Documento();
                        nuevoDoc.Activo = true;
                        nuevoDoc.ContentType = d.ContentType;
                        nuevoDoc.DocGuid = d.DocGuid;
                        nuevoDoc.DocumentoID = 0;
                        nuevoDoc.Extencion = d.Extencion;
                        nuevoDoc.FechaModificacion = DateTime.Now;
                        nuevoDoc.Nombre = d.FileName;
                        nuevoDoc.IncidenciaID= incidenciaID;
                        //nuevoDoc.TipoArchivoID = tipoArchivoID;
                        nuevoDoc.Url = d.Path;
                        nuevoDoc.UsuarioModificacion = d.UserId;
                        nuevoDoc.Descripcion = d.Descripcion;
                        nuevoDoc.EstatusIncidencia = tipoDocumento;

                        ctx.Sam3_Rel_Incidencia_Documento.Add(nuevoDoc);
                    }

                    ctx.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------
                return false;
            }
        }

        public object ObtenerDocumentosIncidencia(int incidenciaID, string estatusDocumento, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    int tipoDocumento = 0;

                    switch (estatusDocumento)
                    {
                        case "Incidencia":
                            tipoDocumento = 1;
                            break;
                        case "Resolver":
                            tipoDocumento = 2;
                            break;
                        case "Responder":
                            tipoDocumento = 3;
                            break;
                        case "Cancelar":
                            tipoDocumento = 4;
                            break;
                    }

                    List<ListaDocumentos> documentos = (from rid in ctx.Sam3_Rel_Incidencia_Documento
                                                        where rid.Activo && rid.IncidenciaID == incidenciaID
                                                        && rid.EstatusIncidencia == tipoDocumento
                                                        select new ListaDocumentos
                                                        {
                                                            DocumentoID = rid.Rel_Incidencia_DocumentoID.ToString(),
                                                            Nombre = rid.Nombre,
                                                            Extencion = rid.Extencion,
                                                            Url = rid.Url,
                                                            TipoArchivo = string.Empty,
                                                            Descripcion=rid.Descripcion
                                                        }).AsParallel().ToList();
                    return documentos;
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

        public object EliminarDocumentoIncidencia(int documentoID, Sam3_Usuario usuario)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Sam3_Rel_Incidencia_Documento docDb = ctx.Sam3_Rel_Incidencia_Documento
                        .Where(x => x.Rel_Incidencia_DocumentoID == documentoID).AsParallel().SingleOrDefault();

                    docDb.Activo = false;
                    docDb.FechaModificacion = DateTime.Now;
                    docDb.UsuarioModificacion = usuario.UsuarioID;

                    ctx.SaveChanges();

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");
                    result.ReturnCode = 200;
                    result.ReturnStatus = false;
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

    }
}