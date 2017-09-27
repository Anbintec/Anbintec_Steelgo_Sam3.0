using BackEndSAM.DataAcces.Embarque.Encintado;
using BackEndSAM.Models.Embarque.Encintado;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Embarque.Encintado
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EncintadoController : ApiController
    {
        [HttpGet]
        public object ObtieneElementosPorBusqueda(string token, int TipoConsulta, int ZonaID, int CuadranteID, string NumeroControl)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return EncintadoBD.Instance.ObtieneElementosPorBusqueda(TipoConsulta, ZonaID, CuadranteID, NumeroControl, usuario.UsuarioID);

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

        public object Get(string token, int TipoConsulta, int Todos, int ZonaID, int CuadranteID, string SpoolContiene, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                if(TipoConsulta == 1)
                {
                    return EncintadoBD.Instance.ObtieneDetalleEncitadoPorZona(ZonaID, CuadranteID, Todos, lenguaje, usuario.UsuarioID);
                }
                else
                {
                    return EncintadoBD.Instance.ObtieneDetalleEncitadoPorSpool(ZonaID, SpoolContiene, Todos, lenguaje, usuario.UsuarioID);
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
        
        public object Post(CapturaEncintado Captura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(Captura.listaDetalle);

                return EncintadoBD.Instance.GuardarCapturaEncintado(dtDetalleCaptura, usuario.UsuarioID, lenguaje);
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

        public Object Get(string token,string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                return EncintadoBD.Instance.ObtenerColoresEncintado(lenguaje);
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
