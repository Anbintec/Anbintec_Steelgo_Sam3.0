using BackEndSAM.DataAcces.Embarque.Etiquetado;
using BackEndSAM.Models.Embarque.Etiquetado;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Embarque.Etiquetado
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class EtiquetadoController : ApiController
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
                
                return EtiquetadoBD.Instance.ObtieneElementosPorBusqueda(TipoConsulta, ZonaID, CuadranteID, NumeroControl, usuario.UsuarioID);

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

        [HttpGet]
        public object ObtieneDetalleEtiquetado(string token, int TipoConsulta, int Todos, int ZonaID, int CuadranteID, string SpoolContiene)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                if (TipoConsulta == 1) {
                    return EtiquetadoBD.Instance.ObtieneDetalleEtiquetadoPorZona(ZonaID, CuadranteID, Todos, usuario.UsuarioID);
                }else
                {
                    return EtiquetadoBD.Instance.ObtieneDetalleEtiquetadoPorSpool(SpoolContiene, Todos, usuario.UsuarioID);
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
        
        [HttpPost]
        public object GuardaCapturaEtiquetado(CapturaEtiquetado captura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            bool tokenValidado = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValidado)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalle = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(captura.listaDetalle);
                return EtiquetadoBD.Instance.GuardaCapturaEtiquetado(dtDetalle, usuario.UsuarioID, lenguaje);
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