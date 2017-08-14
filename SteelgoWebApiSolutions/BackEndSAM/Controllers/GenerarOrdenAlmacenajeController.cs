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
    public class GenerarOrdenAlmacenajeController : ApiController
    {
         //GET api/<controller>/5
        public object Get(int ordenAlmacenajeID, string token)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
          
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);

                return OrdenAlmacenajeBd.Instance.ObtenerDetalleOrdenAlmacenaje(ordenAlmacenajeID, usuario);

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

        public object Get(string data, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);
                FiltrosOrdenAlmacenaje filtros = ser.Deserialize<FiltrosOrdenAlmacenaje>(data);

                return OrdenAlmacenajeBd.Instance.ObtenerListadoGenerarOrdenAlmacenaje(filtros, usuario);
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
        public object Post(ListadosFolios OrdenAlmacenaje, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);

                if (OrdenAlmacenaje.listaFoliosCuantificacion.Count > 0 || OrdenAlmacenaje.listaItemCodes.Count > 0 || OrdenAlmacenaje.listaNumerosUnicos.Count > 0)
                {
                    return OrdenAlmacenajeBd.Instance.GenerarOrdenAlmacenaje(OrdenAlmacenaje, usuario);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("No se encontro ningun ID en los datos enviados");
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

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}