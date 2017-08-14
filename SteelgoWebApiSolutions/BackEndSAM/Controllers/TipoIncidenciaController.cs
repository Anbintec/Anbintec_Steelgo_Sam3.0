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
    public class TipoIncidenciaController : ApiController
    {
        // GET api/tipoincidencia
        public object Get(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return IncidenciaBd.Instance.TiposIncidencias(usuario);
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

        public object Get(string tipoIncidencia, string busqueda, string levantarincidencia, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                int tipoIncidenciaID = tipoIncidencia != "" ? Convert.ToInt32(tipoIncidencia) : 0;
                return ListadoBd.Instance.ObtenerEntidadComboIncidencia(tipoIncidenciaID, busqueda, usuario);
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

        public object Get(string tipoIncidencia, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                int tipoIncidenciaID = tipoIncidencia != "" ? Convert.ToInt32(tipoIncidencia) : 0;
                return ListadoBd.Instance.ListaComboIncidencia(tipoIncidenciaID, usuario);
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

        // GET api/tipoincidencia/5
        public IEnumerable<ListaCombos> Get(int tipoIncidenciaID, string token)
        {
            List<ListaCombos> lstCombo = new List<ListaCombos>();
            ListaCombos combo1 = new ListaCombos();
            ListaCombos combo2 = new ListaCombos();
            ListaCombos combo3 = new ListaCombos();

            if (tipoIncidenciaID == 1)
            {
                combo1.id = "1";
                combo1.value = "Packing List 1";
                lstCombo.Add(combo1);

                combo2.id = "2";
                combo2.value = "Packing List 2";
                lstCombo.Add(combo2);
            }

            if (tipoIncidenciaID == 2)
            {
                combo1.id = "1";
                combo1.value = "Numero Unico 1";
                lstCombo.Add(combo1);

                combo2.id = "2";
                combo2.value = "Numero Unico 2";
                lstCombo.Add(combo2);
            }

            if (tipoIncidenciaID == 3)
            {
                combo1.id = "1";
                combo1.value = "ItemCode 1";
                lstCombo.Add(combo1);

                combo2.id = "2";
                combo2.value = "ItemCode 2";
                lstCombo.Add(combo2);
            }

            return lstCombo.AsEnumerable();
        }

        // POST api/tipoincidencia
        public void Post([FromBody]string value)
        {
        }

        // PUT api/tipoincidencia/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/tipoincidencia/5
        public void Delete(int id)
        {
        }
    }
}
