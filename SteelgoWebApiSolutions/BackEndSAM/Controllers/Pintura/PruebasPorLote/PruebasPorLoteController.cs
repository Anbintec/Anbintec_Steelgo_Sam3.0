using BackEndSAM.DataAcces.Pintura.AdminReductores;
using BackEndSAM.DataAcces.Pintura.PruebasPorLote;
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

namespace BackEndSAM.Controllers.Pintura
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PruebasPorLoteController : ApiController
    {

        //Obtener procesos
        [HttpGet]
        public object Get(string token, string lenguaje, int proyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerProcesos(proyectoID);

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
        //obtener SP
        [HttpGet]
        public object Get(string token, int ProcesoPinturaID, int ProyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerSistemaPintura(ProyectoID, ProcesoPinturaID);

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
        //obtener pruebas
        [HttpGet]
        public object Get(string token, int ProcesoPinturaID, int SistemaPinturaProyectoID, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerPrueba(ProcesoPinturaID, SistemaPinturaProyectoID, lenguaje);

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

        //obtener Fechas
        [HttpGet]
        public object Get(string token, int ProcesoPinturaID, int SistemaPinturaProyectoID, int PruebaProcesoPinturaID, int sistemaPinturaColorID, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerFechas(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID,  sistemaPinturaColorID, lenguaje);

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

        //obtener lotes
        [HttpGet]
        public object Get(string token, int ProcesoPinturaID, int SistemaPinturaProyectoID, int PruebaProcesoPinturaID, string FechaLote, string lenguaje,int sistemaPinturaColorID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerLotes(ProcesoPinturaID, SistemaPinturaProyectoID, PruebaProcesoPinturaID, FechaLote, lenguaje, sistemaPinturaColorID);

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

        //obtener lotes
        public object Get(string token, int procesoPinturaID, int sistemaPinturaProyectoID, int sistemaPinturaColorID, int pruebaProcesoPinturaID, int loteID, string lenguaje,int todosSinCaptura)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerDetalle(procesoPinturaID, sistemaPinturaProyectoID, pruebaProcesoPinturaID, sistemaPinturaColorID, loteID, lenguaje, todosSinCaptura);

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

        //obtener pruebas por spool realizadas
        public object Get(string token, int spoolID, int proyectoProcesoPruebaID,int SistemaPinturaColorID, string lenguaje,int loteID )
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return PruebasPorLoteBD.Instance.ObtenerPruebasPorSpool(spoolID, proyectoProcesoPruebaID, SistemaPinturaColorID, lenguaje, loteID);

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

        public object Post(BackEndSAM.Models.Pintura.PruebasPorLote.Captura listaDetalle, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable TablaPruebasGeneradas = null;
                TablaPruebasGeneradas = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaDetalle.Detalles);

                return PruebasPorLoteBD.Instance.GuardarPruebas(TablaPruebasGeneradas, usuario, lenguaje);
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

        //obtener Catalogos
        [HttpGet]
        public object Get(string token, int dato, string lenguaje, int catalogo)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                if (catalogo == 1)
                    return PruebasPorLoteBD.Instance.ObtenerProcesosPintura(dato, lenguaje);
                else
                    return PruebasPorLoteBD.Instance.ObtenerPrueba(dato, lenguaje);

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
        public object Get(string token, int ordentrabajospoolid ,int sistemapinturacolorid, string lenguaje,int variable)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                
                    return PruebasPorLoteBD.Instance.ObtenerDetalleSpool(ordentrabajospoolid, sistemapinturacolorid, lenguaje);

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

        //ObtenerListadoColores
        public object Get(string token, int sistemaPinturaProyectoID, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                return PruebasPorLoteBD.Instance.ObtenerListadoColores(sistemaPinturaProyectoID, lenguaje);
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
