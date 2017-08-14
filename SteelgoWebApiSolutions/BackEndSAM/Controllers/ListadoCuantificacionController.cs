using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoCuantificacionController : ApiController
    {

        // PUT api/<controller>/5
        public object Post(int ProyectoID, bool cerrar, bool incompletos, int FolioAvisollegadaId, int FolioCuantificacionID, string cuantificacion, string token, int idGuardado)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                CuantificacionListado datosItemCode = serializer.Deserialize<CuantificacionListado>(cuantificacion);

                return GuardarItemCodesBd.Instance.GuardadoInformacionItemCodes(ProyectoID, cerrar, incompletos, FolioAvisollegadaId, FolioCuantificacionID, datosItemCode, usuario, idGuardado);
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

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}