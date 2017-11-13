using BackEndSAM.DataAcces.MedicionesClimatologicas.CondicionesClimatologicas;
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

namespace BackEndSAM.Controllers.MedicionesClimatologicas.CondicionesClimatologicas
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CondicionesClimatologicasController : ApiController
    {
        [HttpGet]
        public object ObtieneEquiposDeToma(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return CondicionesClimatologicasBD.Instance.ObtenerEquiposDeToma(usuario.UsuarioID);
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
        public object Post(BackEndSAM.Models.MedicionesClimatologicas.Captura listaCaptura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DataTable DetalleCondiciones = null;

                if (listaCaptura.Detalles.Count > 0)
                    DetalleCondiciones = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCaptura.Detalles);
              
                return CondicionesClimatologicasBD.Instance.InsertarCapturaInspeccion(DetalleCondiciones, usuario, lenguaje);
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


