using BackEndSAM.DataAcces.IncidenciasBD;
using BackEndSAM.Models.ListadoIncidenciasBilingues;
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

namespace BackEndSAM.Controllers.Incidencias
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoIncidenciasController : ApiController
    {
        [HttpGet]
        public object getProyectosIncidenciasBilingues(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ListadoIncidenciasBD.Instance.ObtenerListadoProyectos(usuario);
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
        public object getListadoIncidenciasBiilingues(string token, string lenguaje, int ProyectoID, string FechaInicial, string FechaFinal, string EstatusID, int Mostrar)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ListadoIncidenciasBD.Instance.ObtieneListadoIncidenciasBilingues(usuario,lenguaje,ProyectoID,FechaInicial,FechaFinal,EstatusID,Mostrar);

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
        public object GuardaCaptura(Captura captura, string token)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalle = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(captura.listaDetalle);

                return ListadoIncidenciasBD.Instance.InsertarCaptura(dtDetalle, usuario);
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
