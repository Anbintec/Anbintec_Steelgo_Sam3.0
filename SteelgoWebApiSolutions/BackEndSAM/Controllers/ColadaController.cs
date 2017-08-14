using BackEndSAM.DataAcces;
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
using BackEndSAM.Models;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ColadaController : ApiController
    {

        /// <summary>
        /// Obtiene listado de coladas por proyecto
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="mostrarOpcion"></param>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public object Get(int proyectoID, string token, int paginaID, string texto, string idioma, int id = 0, int mostrarOpcion = 0, int numeroUnicoID = 0)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.ObtenerColadasPorProyecto(id, mostrarOpcion,texto, usuario, paginaID, idioma, proyectoID, numeroUnicoID);
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

        /// <summary>
        /// Listado de coladas por itemcode
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="mostrarOpcion"></param>
        /// <param name="itemcodeID"></param>
        /// <returns></returns>
        public object Get(string itemcode, string token, int paginaID, string idioma, int id = 0, int mostrarOpcion = 0)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.ObtenerColadasPorItemCode(id, mostrarOpcion, usuario, paginaID, idioma, itemcode);
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


        /// <summary>
        /// Listado de coladas por itemcode y texto
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="mostrarOpcion"></param>
        /// <param name="itemcodeID"></param>
        /// <returns></returns>
        public object Get(string itemcode, string texto, string token, int paginaID, string idioma, int id = 0, int mostrarOpcion = 0)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.ObtenerColadasPorItemCodeyTexto(id, mostrarOpcion, usuario, paginaID,texto, idioma, itemcode);
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
        /// <summary>
        /// Listado de coladas por Familia de acero
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <param name="mostrarOpcion"></param>
        /// <param name="itemcodeID"></param>
        /// <returns></returns>
        public object Get(string token, int familiaAcerolID, int paginaID, string idioma, int id = 0, int mostrarOpcion = 0)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.ObtenerColadasPorFamiliAcero(id, mostrarOpcion, usuario, paginaID, idioma, familiaAcerolID);
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

        public object Get(int itemcode, int proyecto, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.ObtenerColadasPorItemCodeCatalogoMTR(usuario, itemcode, proyecto);
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
        public object Post(string data, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                ColadaJson DatosColada = serializer.Deserialize<ColadaJson>(data);
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ColadaBd.Instance.GuardarColadaPopUp(DatosColada, usuario);
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