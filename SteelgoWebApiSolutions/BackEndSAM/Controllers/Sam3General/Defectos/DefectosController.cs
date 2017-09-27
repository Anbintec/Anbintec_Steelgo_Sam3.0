using BackEndSAM.DataAcces.Sam3General.Defectos;
using BackEndSAM.Models.Inspeccion.VisualDimensional;
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

namespace BackEndSAM.Controllers
{
    public class DefectosController : ApiController
    {
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public object Get(string token, string lenguaje, string TipoPrueba)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<Sam3_Steelgo_Get_Defectos_Result> listadoDefectos = (List<Sam3_Steelgo_Get_Defectos_Result>)DefectosBd.Instance.listadoDefectos(lenguaje, TipoPrueba);
                List<Defectos> listaDefectos = new List<Defectos>();
                listaDefectos.Add(new Defectos());

                foreach (Sam3_Steelgo_Get_Defectos_Result item in listadoDefectos)
                {
                    Defectos defecto = new Defectos
                    {
                        DefectoID = item.DefectoID,
                        IDDEFECTOTIPO = item.IdDefectoTipo,
                        Nombre = item.Nombre,
                        TIPO = item.Tipo
                    };
                    listaDefectos.Add(defecto);
                }
                return listaDefectos;

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
