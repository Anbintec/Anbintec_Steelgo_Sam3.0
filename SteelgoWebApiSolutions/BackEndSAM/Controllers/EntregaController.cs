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
    public class EntregaController : ApiController
    {
        // GET api/entrega
        public IEnumerable<ListaCombos> Get(string token)
        {
            List<ListaCombos> lstCombos = new List<ListaCombos>();
            ListaCombos combo1 = new ListaCombos();
            ListaCombos combo2 = new ListaCombos();
            ListaCombos combo3 = new ListaCombos();

            combo1.id = "123";
            combo1.value = "123";
            lstCombos.Add(combo1);

            combo2.id = "124";
            combo2.value = "124";
            lstCombos.Add(combo2);

            combo3.id = "125";
            combo3.value = "125";
            lstCombos.Add(combo3);
            
            return lstCombos.AsEnumerable();
        }

        // POST api/entrega
        public object Post(ListadoEntrega entrega, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return EntregaBd.Instance.InsertarEntrega(entrega.Entregas, usuario);
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
