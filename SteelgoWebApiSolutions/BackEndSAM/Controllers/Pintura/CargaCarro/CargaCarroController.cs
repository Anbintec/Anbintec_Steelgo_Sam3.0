using BackEndSAM.DataAcces.Pintura.CargaCarro;
using BackEndSAM.Models.Pintura.CargaCarro;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Pintura.CargaCarro
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CargaCarroController : ApiController
    {

        [HttpPut]
        public object CerrarCarro(CerrarMedioTransporte medioTransporte, string token)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return CargaCarroBD.Instance.CierraCarro(usuario, medioTransporte.MedioTransporteID, medioTransporte.MedioTransporteCargaID, medioTransporte.CerrarCarro);
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


        public object Get(string token, int TipoConsulta, int OrdenTrabajoSpoolID, string Codigo, string lenguaje, int medioTransporteID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CargaCarroBD.Instance.ObtieneDetalleSpoolAgregar(medioTransporteID, TipoConsulta, OrdenTrabajoSpoolID, Codigo, lenguaje);
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

        [HttpGet]
        public object ObtenerDetalleCargaCarro(int medioTransporteID, int proyectoID, string token, string lenguaje, string escenario,string valorBusqueda)
        {
            string payload = "";
            string newToken = "";  
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                if (escenario == "Patio")
                    return CargaCarroBD.Instance.ObtenerDetalleCargaCarro(medioTransporteID, proyectoID, lenguaje, valorBusqueda == null?0 : (valorBusqueda.Split('~')[1]=="Spool"?1:2), valorBusqueda == null ? "" : valorBusqueda.Split('~')[0]);
                else
                    return CargaCarroBD.Instance.ObtenerDetalleCargaCarroBacklog(medioTransporteID, proyectoID, lenguaje);
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

        //Guarda la carga
        public object Post(Captura listaCaptura, string token, string lenguaje, int medioTransporteID, int medioTransporteCargaID, int cerrar)
        {
            string payload = "";
            string newToken = "";
            DataTable dtDetalleCaptura = new DataTable();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCaptura.Detalles);
                return CargaCarroBD.Instance.GuardarCargaCarro(dtDetalleCaptura, usuario, lenguaje, medioTransporteID, medioTransporteCargaID, cerrar);
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