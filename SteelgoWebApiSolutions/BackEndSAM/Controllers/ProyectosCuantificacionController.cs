using BackEndSAM.DataAcces;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers
{
    public class ProyectosCuantificacionController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        //<summary>
        //Obtener los proyectos del folio de Llegada (Combo Aviso de Entrada)
        //</summary>
        //<param name="token"></param>
        //<param name="avisoLlegada"></param>
        //<returns></returns>
        //public object Get(string token, int avisoLlegada)
        //{
        //    string payload = "";
        //    string newToken = "";
        //    bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //    if (tokenValido)
        //    {
        //        return FoliosCuantificacionBd.Instance.obtenerProyectos(avisoLlegada);
        //    }
        //    else
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(payload);
        //        result.ReturnCode = 401;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = false;
        //        return result;
        //    }
        //}

        /// <summary>
        /// Actualizar la relacion av Llegada_Proyecto
        /// </summary>
        /// <param name="value"></param>
        public object Post(string token, int avisoLlegada, int proyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return FoliosCuantificacionBd.Instance.actualizarProyectos(avisoLlegada, proyectoID, usuario.UsuarioID);
                //return FoliosCuantificacionBd.Instance.getProyects(avisoLlegada);
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