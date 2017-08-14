using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ReporteController : ApiController
    {
        public object Get(string data, string token)
        {

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ConsultaReportes consulta = serializer.Deserialize<ConsultaReportes>(data);

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                switch (consulta.TipoReporte)
                {
                    case 1: //Formato Etiquetas Orden de recepcion
                        return ReportesBd.Instance.ReporteFormatoEtiquetasOrdenRecepcion(consulta.FolioOrdenRecepcion, usuario);
                    case 2: //Formato Incidencia
                        return ReportesBd.Instance.ReporteFormatoIncidencias(consulta.FolioAvisoLlegadaID, consulta.Cadena, usuario);
                    case 3: //Pase Salida transportista
                        return ReportesBd.Instance.ReportePaseSalidaTransportista(consulta.FolioAvisoLlegadaID, usuario);
                    case 4:
                        return ReportesBd.Instance.ReporteOrdenRecepcion(consulta.FolioOrdenRecepcion, usuario);
                    case 5:
                        return ReportesBd.Instance.ReporteOrdenAlmacenaje(consulta.FolioOrdenAlmacenaje, usuario);
                    case 6:
                        return ReportesBd.Instance.ReporteIncidencias(consulta.IncidenciaID, usuario);
                    case 7:
                        return ReportesBd.Instance.ReporteImpresionDocumental(consulta.NumeroControlID, consulta.TipoMaterial, usuario);
                    default:
                        TransactionalInformation result = new TransactionalInformation();
                        result.ReturnMessage.Add("Tipo de reporte no encontrado");
                        result.ReturnCode = 500;
                        result.ReturnStatus = false;
                        result.IsAuthenicated = true;
                        return result;
                }
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