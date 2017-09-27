using BackEndSAM.DataAcces.Sam3General;
using BackEndSAM.Models.Sam3General;
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

namespace BackEndSAM.Controllers.Sam3General.Obrero
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ObreroController : ApiController
    {
        public object Get(string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return ObreroBD.Instance.ObtenerObrero();
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

        public object Get(string token, string TipoObreroID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return ObreroBD.Instance.ObtenerObrero(TipoObreroID);
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
        /// Obtiene el listado de obreros con diferentes filtros
        /// </summary>
        /// <param name="idProyecto">identificador del proyecto</param>
        /// <param name="tipo">1: Retorna todos los obreros activos en todos los Patios, 
        ///                    2: Retorna todos los obreros de un tipo en particular,
        ///                    3: Retorna todos los obreros de un patio en particular por el proyecto y por fecha,
        ///                    4: Retorna todos los obreros de un tipo y un patio en particular por el proyecto y por fechas</param>
        /// <param name="TipoObrero">Tipo de obrero en base al campo TipoObrero de la tabla Sam3_TipoObrero</param>
        ///         /// <param name="token"></param>
        /// <returns></returns>
        public object Get(int idProyecto, int tipo, string token, string TipoObrero)
        {

            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<Sam3_Steelgo_Get_Obrero_Result> listadoObrero = (List<Sam3_Steelgo_Get_Obrero_Result>)ObreroBD.Instance.ObtenerObrero(idProyecto, tipo, TipoObrero);


                List<ObreroSteelGo> listaObreros = new List<ObreroSteelGo>();
                listaObreros.Add(new ObreroSteelGo());

                foreach (Sam3_Steelgo_Get_Obrero_Result item in listadoObrero)
                {
                    ObreroSteelGo obrero = new ObreroSteelGo
                    {
                        Codigo = item.Codigo,
                        TipoObrero = item.TipoObrero,
                        ObreroID = item.ObreroID,
                        NombreCompleto = item.NombreCompleto
                    };
                    listaObreros.Add(obrero);
                }
                return listaObreros;

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
        /// Inserta Obrero
        /// </summary>
        /// <param name="Obrero">Obrero</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        public object Post(Sam3_Obrero Obrero, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ObreroBD.Instance.InsertarObrero(Obrero, usuario);
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
        /// Actualiza Obrero
        /// </summary>
        /// <param name="Obrero">Obrero</param>
        /// <param name="token"></param>
        /// <returns></returns>
        public object Put(Sam3_Obrero Obrero, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ObreroBD.Instance.ModificarObrero(Obrero, usuario);
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

        public object Delete(int ObreroID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ObreroBD.Instance.EliminarObrero(Convert.ToInt32(ObreroID), usuario);
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
