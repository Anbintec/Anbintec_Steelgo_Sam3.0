using BackEndSAM.DataAcces.Pintura.DescargaCarroBD;
using BackEndSAM.Models.Pintura.DescargaCarro;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Pintura.DecargaCarro
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DescargarCarroController : ApiController
    {
        public object Get(string token, string lenguaje, int carroID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return DescargaCarroBD.Instance.ObtieneDetalle(carroID, lenguaje);
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

        public object Post(Captura listaCapturaDescarga, string token, string lenguaje,string cargaCarroID,int carroID,int cerrarCarro)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                DataTable capturaDescarga = null;
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                if (capturaDescarga == null)
                        capturaDescarga = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaDescarga.Detalles);

               
               
                return DescargaCarroBD.Instance.Guardar(capturaDescarga, usuario.UsuarioID, lenguaje, cargaCarroID, carroID, cerrarCarro);
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
