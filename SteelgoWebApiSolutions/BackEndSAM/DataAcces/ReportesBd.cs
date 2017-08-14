using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Services.Protocols;
using SecurityManager.Api.Models;
using DatabaseManager.Sam3;
using System.Net;
using System.Configuration;
using System.Security.Principal;
using BackEndSAM.Sam3Reportes;
using Microsoft.SqlServer.Server;
using Microsoft.ReportingServices;
using System.Net.Http;
using System.Net.Http.Headers;


namespace BackEndSAM.DataAcces
{
    public class ReportesBd
    {
        private static readonly object _mutex = new object();
        private static ReportesBd _instance;
        private string usuarioReportes
        {
            get
            {
                return ConfigurationManager.AppSettings["usuarioReportes"];
            }
        }
        private string passReportes
        {
            get
            {
                return ConfigurationManager.AppSettings["passReportes"];
            }
        }
        private string dominioReportes
        {
            get
            {
                return ConfigurationManager.AppSettings["dominioReportes"];
            }
        }


        /// <summary>
        /// constructor privado para implementar el patron Singleton
        /// </summary>
        private ReportesBd()
        {
        }

        /// <summary>
        /// crea una instancia de la clase
        /// </summary>
        public static ReportesBd Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ReportesBd();
                    }
                }
                return _instance;
            }
        }

        public object ReporteFormatoEtiquetasOrdenRecepcion(int ordenRecepcionID, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/Formato de Etiquetas";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                #endregion

                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[1];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "OrdenRecepcionID";
                parametros[0].Value = ordenRecepcionID.ToString();

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "EtiquetasMaterial_" + ordenRecepcionID.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReporteFormatoIncidencias(int FolioAvisoLlegadaID, string cadena, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/Formato de Incidencias";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                #endregion

                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[2];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "FolioAvisoLlegadaID";
                parametros[0].Value = FolioAvisoLlegadaID.ToString();

                parametros[1] = new ParameterValue();
                parametros[1].Name = "cadena";
                parametros[1].Value = cadena;

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "Formato_Incidencias_FolioAvisoLlegada" + FolioAvisoLlegadaID.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReportePaseSalidaTransportista(int FolioAvisoLlegadaID, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/Pase de Salida de Transportista";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                #endregion

                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[1];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "FolioAvisoLlegadaID";
                parametros[0].Value = FolioAvisoLlegadaID.ToString();

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "PaseSalidaTransportista_FolioAvisoLlegada" + FolioAvisoLlegadaID.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReporteOrdenRecepcion(int ordenRecepcionID, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/OrdenRecepcion";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                #endregion

                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[2];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "ordenRecepcionID";
                parametros[0].Value = ordenRecepcionID.ToString();
                parametros[1] = new ParameterValue();
                parametros[1].Name = "usuarioID";
                parametros[1].Value = usuario.UsuarioID.ToString();

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "OrdenRecepcion_" + ordenRecepcionID.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReporteOrdenAlmacenaje(int ordenAlmacenajeID, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/OrdenAlmacenaje";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                #endregion

                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[2];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "ordenAlmacenajeID";
                parametros[0].Value = ordenAlmacenajeID.ToString();
                parametros[1] = new ParameterValue();
                parametros[1].Name = "usuarioID";
                parametros[1].Value = usuario.UsuarioID.ToString();

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "OrdenAlmacenaje_" + ordenAlmacenajeID.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReporteIncidencias(int incidenciaID, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporte = "/Reportes/ReporteIncidencia";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                int Id = 0;
                #endregion

                using (SamContext ctx = new SamContext())
                {
                    Sam3_Incidencia incidencia = ctx.Sam3_Incidencia.Where(x => x.IncidenciaID == incidenciaID).FirstOrDefault();
                    Id = incidencia.IncidenciaOriginalID != null ? incidencia.IncidenciaOriginalID.GetValueOrDefault() : incidenciaID;
                }


                ReportExecutionServiceSoapClient cliente = new ReportExecutionServiceSoapClient();
                cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                cliente.ClientCredentials.UserName.Password = passReportes;

                ParameterValue[] parametros = new ParameterValue[1];
                parametros[0] = new ParameterValue();
                parametros[0].Name = "IncidenciaID";
                parametros[0].Value = incidenciaID.ToString();

                ExecutionInfo infoEjecucion = new ExecutionInfo();
                TrustedUserHeader encabezadoSeguro = null;
                ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporte, historyID, out encavezadoDeServidor, out infoEjecucion);
                cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                Stream iStream = new MemoryStream(reporte);

                response.Content = new StreamContent(iStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = "Formato_Incidencias" + Id.ToString() + ".pdf";

                return response;
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

        public object ReporteImpresionDocumental(int numeroControlID, int tipoMaterial, Sam3_Usuario usuario)
        {
            try
            {
                #region parametros
                byte[] reporte;
                string historyID = null;
                string deviceInfo = null;
                string extension;
                string encoding;
                string mimeType;
                Warning[] warnings;
                string[] streamIDs = null;
                string format = "PDF";
                string rutaReporteAccesorios = "/Reportes/ReporteImpresionAccesorios";
                string rutaReporteTubos = "/Reportes/ReporteImpresionTubos";
                NetworkCredential credenciales = new NetworkCredential(usuarioReportes, passReportes);
                ReportExecutionServiceSoapClient cliente;
                #endregion
                switch (tipoMaterial)
                {
                    case 1:
                        cliente = new ReportExecutionServiceSoapClient();
                        cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                        cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                        cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                        cliente.ClientCredentials.UserName.Password = passReportes;

                        ParameterValue[] parametros = new ParameterValue[1];
                        parametros[0] = new ParameterValue();
                        parametros[0].Name = "OrdenTrabajoSpoolID";
                        parametros[0].Value = numeroControlID.ToString();

                        ExecutionInfo infoEjecucion = new ExecutionInfo();
                        TrustedUserHeader encabezadoSeguro = null;
                        ExecutionHeader encabezadoDeEjecucion = new ExecutionHeader();
                        ServerInfoHeader encavezadoDeServidor = new ServerInfoHeader();

                        encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguro, rutaReporteAccesorios, historyID, out encavezadoDeServidor, out infoEjecucion);
                        cliente.SetExecutionParameters(encabezadoDeEjecucion, encabezadoSeguro, parametros, "en-US", out infoEjecucion);

                        cliente.Render(encabezadoDeEjecucion, encabezadoSeguro, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                        HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                        Stream iStream = new MemoryStream(reporte);

                        response.Content = new StreamContent(iStream);
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        response.Content.Headers.ContentDisposition.FileName = "Formato_PickingTicketAccesorios.pdf";
                        return response;
                    case 2:
                        cliente = new ReportExecutionServiceSoapClient();
                        cliente.ClientCredentials.Windows.ClientCredential = credenciales;
                        cliente.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                        cliente.ClientCredentials.UserName.UserName = usuarioReportes;
                        cliente.ClientCredentials.UserName.Password = passReportes;

                        ParameterValue[] parametrosTubos = new ParameterValue[1];
                        parametrosTubos[0] = new ParameterValue();
                        parametrosTubos[0].Name = "OrdenTrabajoSpoolID";
                        parametrosTubos[0].Value = numeroControlID.ToString();

                        ExecutionInfo infoEjecucionTubos = new ExecutionInfo();
                        TrustedUserHeader encabezadoSeguroTubos = null;
                        ExecutionHeader encabezadoDeEjecucionTubos = new ExecutionHeader();
                        ServerInfoHeader encavezadoDeServidorTubos = new ServerInfoHeader();

                        encabezadoDeEjecucion = cliente.LoadReport(encabezadoSeguroTubos, rutaReporteTubos, historyID, out encavezadoDeServidorTubos, out infoEjecucionTubos);
                        cliente.SetExecutionParameters(encabezadoDeEjecucionTubos, encabezadoSeguroTubos, parametrosTubos, "en-US", out infoEjecucionTubos);

                        cliente.Render(encabezadoDeEjecucionTubos, encabezadoSeguroTubos, format, deviceInfo, out reporte, out extension, out mimeType, out encoding, out warnings, out streamIDs);

                        HttpResponseMessage responseTubos = new HttpResponseMessage(HttpStatusCode.OK);

                        Stream iStreamTubos = new MemoryStream(reporte);

                        responseTubos.Content = new StreamContent(iStreamTubos);
                        responseTubos.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                        responseTubos.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                        responseTubos.Content.Headers.ContentDisposition.FileName = "Formato_PickingTicketTubos.pdf";
                        return responseTubos;
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                //-----------------Agregar mensaje al Log -----------------------------------------------
                LoggerBd.Instance.EscribirLog(ex);
                //-----------------Agregar mensaje al Log -----------------------------------------------

                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add("Error al cargar el reporte");
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;

                return result;
            }
        }

    }
}