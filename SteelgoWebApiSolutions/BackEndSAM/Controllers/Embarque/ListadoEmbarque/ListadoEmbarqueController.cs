using BackEndSAM.DataAcces.Embarque.ListadoEmbarque;
using BackEndSAM.Models.Embarque.ListadoEmbarque;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Embarque.ListadoEmbarque
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoEmbarqueController : ApiController
    {
        [HttpGet]
        public object ObtenerDetalleListadoEmbarque(string token, string Lenguaje, int EstatusEmbarque)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoEmbarqueBD.Instance.ObtenerDetalleListado(Lenguaje, EstatusEmbarque, usuario.UsuarioID);
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
        public object ObtenerDetalleListadoEmbarqueEnviado(string token, string Lenguaje, int ProyectoID, string FechaInicio, string FechaFin)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoEmbarqueBD.Instance.ObtenerDetalleListadoEmbarqueEnviado(usuario.UsuarioID, ProyectoID, Lenguaje, FechaInicio, FechaFin);
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
        public object Get(string token, string Lenguaje, int ProyectoID, string FechaInicio, string FechaFin, bool Flag)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido && Flag)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoEmbarqueBD.Instance.ObtenerDetalleListadoEmbarquesCerrados(usuario.UsuarioID, ProyectoID, Lenguaje, FechaInicio, FechaFin);
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
        public object ObtenerElementosPorEstatus(string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoEmbarqueBD.Instance.ObtenerElementosPorEstatus(lenguaje, usuario.UsuarioID);
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
        public object GuardarEnvioEmbarque(DetalleJsonEnvio DetalleJson, string token, string lenguaje, string NumeroEmbarque, string NumeroEmbarqueCliente, string FechaEnvio, int ProyectoID)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoEmbarqueBD.Instance.GuardarEnvioEmbarque(DetalleJson, usuario.UsuarioID, lenguaje, NumeroEmbarque, NumeroEmbarqueCliente, FechaEnvio);
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
        public object GuadarCaptura(CapturaListadoEmbarque Captura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DataTable dtDetalle = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(Captura.listaDetalle);

                return ListadoEmbarqueBD.Instance.GuardarCaptura(usuario.UsuarioID, lenguaje, dtDetalle);
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
