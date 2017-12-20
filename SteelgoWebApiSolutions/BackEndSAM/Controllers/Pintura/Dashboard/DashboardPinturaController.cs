using BackEndSAM.DataAcces.Pintura.Dashboard;
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

namespace BackEndSAM.Controllers.Pintura.Dashboard
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashboardPinturaController : ApiController
    {

        [HttpGet]
        //public object getHeaderDashboard(string token, string lenguaje, int modulo)
        public object getHeaderDashboard(string token, string lenguaje, int ProyectoID, int ClienteID, string FechaInicial, string FechaFinal)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                
                return DashboardPinturaBD.Instance.ObtieneHeaderDashBoard(lenguaje,ProyectoID,ClienteID,FechaInicial,FechaFinal);
                
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
        //public object getHeaderDashboard(string token, string lenguaje, int modulo)
        public object getHeaderDashboard(string token, string lenguaje, int ProyectoID,int EstatusID, int ClienteID, string FechaInicial, string FechaFinal)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return DashboardPinturaBD.Instance.ObtieneContenidoDashBoard(lenguaje, ProyectoID,EstatusID, ClienteID, FechaInicial, FechaFinal);

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
