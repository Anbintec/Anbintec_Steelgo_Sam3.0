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
    public class RevisionDeficitController : ApiController
    {
        // GET api/<controller>/5
        public object Get(int ordenTrabajoID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                return DeficitBd.Instance.ObtenerGridDeficit(ordenTrabajoID);
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

         //GET api/<controller>/5
        public object Get(string ordenTrabajo, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                return DeficitBd.Instance.obtenerSpoolsRevision(ordenTrabajo);
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


        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public object Put(int ordenTrabajoID, string datosDeficit, string datosSpools, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);
                List<SolucionarRevision> datos = ser.Deserialize<List<SolucionarRevision>>(datosDeficit);
                List<SpoolsSeleccionados> spoolsID = ser.Deserialize<List<SpoolsSeleccionados>>(datosSpools);

                return DeficitBd.Instance.SolucionarDeficit(ordenTrabajoID, datos, spoolsID, usuario);
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
        public object Delete(int deficitID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);

                return DeficitBd.Instance.EliminarDeficit(deficitID, usuario);
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