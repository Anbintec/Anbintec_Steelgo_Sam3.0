using BackEndSAM.DataAcces.Sam3General.Dashboard;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Sam3General.Dashboard
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DashboardController: ApiController
    {
        [HttpGet]
        //public object getHeaderDashboard(string token, string lenguaje, int modulo)
        public object getHeaderDashboard(string token, string lenguaje, int modulo, int ProyectoID )
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                if(ProyectoID == 0)
                {
                    return DashboardBD.Instance.ObtieneHeaderDashBoard(modulo, lenguaje, 16, 0, 0);
                }else
                {
                    return DashboardBD.Instance.ObtieneHeaderDashBoard(modulo, lenguaje, ProyectoID, 0, 0);
                }                
                //return DashboardBD.Instance.ObtieneHeaderDashBoard(modulo, lenguaje,16,0,0);
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
        public object getInformacionGrid(string token, string lenguaje,int ProyectoID, int EstatusID, int TipoPruebaID, int ProveedorID, string FechaInicial, string FechaFinal)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return DashboardBD.Instance.ObtenerInformacionGrid(lenguaje, ProyectoID, TipoPruebaID,ProveedorID,FechaInicial,FechaFinal, EstatusID);
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