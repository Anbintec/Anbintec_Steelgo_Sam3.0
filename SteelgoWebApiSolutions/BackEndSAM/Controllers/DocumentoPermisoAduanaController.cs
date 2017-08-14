using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.EntidadesPersonalizadas;
using DatabaseManager.Sam3;
using SecurityManager.TokenHandler;
using SecurityManager.Api.Models;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using BackEndSAM.Utilities;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocumentoPermisoAduanaController : ApiController
    {

        public object Get(int folio, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return DocumentosBd.Instance.ObtenerDocumentosPermisoAduana(folio, usuario);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Post(int folioAvisoLlegada, string NumeroPermiso, string token)
        {
            try
            {
                string newToken = "";
                string payload = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    HttpResponseMessage result = null;

                    var httpRequest = HttpContext.Current.Request;

                    if (httpRequest.Files.Count > 0)
                    {
                        var docfiles = new List<string>();
                        HttpPostedFile postedFile;
                        List<DocumentoPosteado> lstArchivos = new List<DocumentoPosteado>();
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            Guid docguID = Guid.NewGuid();
                            postedFile = httpRequest.Files[i];
                            string nombreArchivo = "";
                            //verificar si el nombre del archivo es una ruta completa
                            if (postedFile.FileName.Contains("\\"))
                            {
                                string[] temp = postedFile.FileName.Split('\\');
                                nombreArchivo = temp[temp.Count() - 1];
                            }
                            else
                            {
                                nombreArchivo = postedFile.FileName;
                            }

                            nombreArchivo = (string)Conversiones.Instance.EliminaCaracteresNombresDeDocumento(nombreArchivo);

                            var path = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["urlFisica"] + docguID + "_" + nombreArchivo);
                            string ruta = ConfigurationManager.AppSettings["urlBase"] + docguID + "_" + nombreArchivo;
                            string[] st = nombreArchivo.Split('.');

                            if (st.Length > 2)
                            {
                                throw new Exception("El nombre de archivo no puede contener puntos");
                            }

                            string extencion = "." + st[1];
                            lstArchivos.Add(new DocumentoPosteado
                            {
                                FileName = nombreArchivo,
                                ContentType = postedFile.ContentType,
                                Size = postedFile.ContentLength,
                                Path = ruta,
                                DocGuid = docguID,
                                FolioAvisoLlegadaID = folioAvisoLlegada,
                                UserId = usuario.UsuarioID,
                                TipoArchivoID = -1,
                                Extencion = extencion,
                                NumeroPermisoAduana = NumeroPermiso
                            });

                            postedFile.SaveAs(path);
                            docfiles.Add(ruta);
                        }

                        if ((bool)DocumentosBd.Instance.GuardarDocumentoPermisoAduana(lstArchivos))
                        {
                            return Ok();
                        }
                        else
                        {
                            foreach (string path in docfiles)
                            {
                                if (File.Exists(path))
                                {
                                    File.Delete(path);
                                }
                            }
                            result = Request.CreateResponse(HttpStatusCode.InternalServerError);
                        }
                    }
                    else
                    {
                        result = Request.CreateResponse(HttpStatusCode.BadRequest);
                    }

                    return result;
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnCode = 401;
                    result.ReturnStatus = false;
                    result.ReturnMessage.Add(payload);
                    result.IsAuthenicated = false;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(ex.Message);
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Post(string datos, string token)
        {
            string newToken = "";
            string payload = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                EstatusFolio estatus = serializer.Deserialize<EstatusFolio>(datos);
                return DocumentosBd.Instance.CambiarEstatusFolio(estatus.FolioAvisoLlegada, estatus.NumeroPermiso, usuario);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.ReturnMessage.Add(payload);
                result.IsAuthenicated = false;
                return result;
            }
        }

        public object Delete(int documentoID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return DocumentosBd.Instance.EliminarDocumentoPermisoAduana(documentoID, usuario);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payload);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
    }
}