using BackEndSAM.DataAcces.Sam3General.CapturasRapidas;
using BackEndSAM.Models.Sam3General.CapurasRapidas;
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

namespace BackEndSAM.Controllers.Sam3General.CapturasRapidas
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CapturasRapidasController : ApiController
    {
        /// <summary>
        /// Obtiene SpoolID
        /// </summary>
        /// <param name="ordenTrabajo">Orden de trabajo Sin formato</param>
        /// <param name="tipo">Tipo de ejecución en el stord</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public object Get(string ordenTrabajo, int tipo, string token, string lenguaje)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                IdOrdenTrabajo idOrdenTrabajo = new IdOrdenTrabajo();

                List<Sam3_Steelgo_Get_SpoolID_Result> lista = (List<Sam3_Steelgo_Get_SpoolID_Result>)CapturasRapidasBd.Instance.ObtenerIDOrdenTrabajo(ordenTrabajo, tipo, lenguaje);
                List<IDS> listaAtatus = new List<IDS>();
                if (lista.Count > 0)
                {
                    listaAtatus.Add(new IDS());
                    foreach (var item in lista)
                    {
                        listaAtatus.Add(new IDS { Status = item.status, IDValido = item.ID, Proyecto = item.NombreProyecto, Valor = item.OrdenTrabajoSpoolID, ProyectoID = item.ProyectoID });
                    }

                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = lista[0].OrdenTrabajo,
                        idStatus = listaAtatus
                    };
                }
                else
                {
                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = "",
                        idStatus = listaAtatus
                    };
                };
                return idOrdenTrabajo;

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
        /// <summary>
        /// Obtiene juntas dependiendo de un spoolID
        /// </summary>
        /// <param name="id">Spool ID</param>
        /// <param name="sinCaptura">muestra lso datos con o sin capturas</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public object Get(string ordenTrabajo, string id, string sinCaptura, string token)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturasRapidasBd.Instance.ObtenerJuntasXSpoolID(usuario, ordenTrabajo, id, sinCaptura == "Todos" ? 1 : 0);
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

        public object Get(string ordenTrabajo, string id, string sinCaptura, string token, int proceso)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturasRapidasBd.Instance.ObtenerJuntasXSpoolIDPNDs(usuario, ordenTrabajo, id, sinCaptura == "Todos" ? 1 : 0, proceso);
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

        public object Get(string id, string sinCaptura, string token, int proceso)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturasRapidasBd.Instance.ObtenerJuntasXSpoolID(id, sinCaptura.ToLower() == "todos" ? 1 : 0, proceso);
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
