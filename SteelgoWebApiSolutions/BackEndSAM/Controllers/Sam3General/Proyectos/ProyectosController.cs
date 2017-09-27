using BackEndSAM.DataAcces.Sam3General.Proyectos;
using BackEndSAM.Models.Sam3General;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Sam3General.Proyectos
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProyectosController : ApiController
    {
        public object Get(string token)
        {
            try
            {
                string payload = "";
                string newToken = "";
                bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (totokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                    
                    return ProyectosBD.Instance.ObtenerListadoProyectos(Usuario);
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
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
        [HttpGet]
        public object ObtieneListadoProyectosIngeneria(string tkn)
        {
            try
            {
                string payload = "";
                string newToken = "";
                bool totokenValido = ManageTokens.Instance.ValidateToken(tkn, out payload, out newToken);
                if (totokenValido)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                    return ProyectosBD.Instance.ObtenerListadoProyectosIngeneria(Usuario.UsuarioID);
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
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }

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

                List<Spool> listaSpools = (List<Spool>)ProyectosBD.Instance.ObtenerIDOrdenTrabajo(usuario, ordenTrabajo, tipo, lenguaje);
               
                if (listaSpools.Count > 0)
                {
                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = listaSpools[0].OrdenTrabajo,
                        ListaSpools = listaSpools
                    };
                }
                else
                {
                    idOrdenTrabajo = new IdOrdenTrabajo
                    {
                        OrdenTrabajo = "",
                        ListaSpools = listaSpools
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
    }
}
