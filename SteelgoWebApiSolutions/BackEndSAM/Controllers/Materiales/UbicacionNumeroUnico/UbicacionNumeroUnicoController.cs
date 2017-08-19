using BackEndSAM.DataAcces.Materiales.UbicacionNumeroUnico;
using BackEndSAM.Models.Materiales.UbicacionNumeroUnico;
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

namespace BackEndSAM.Controllers.Materiales.UbicacionNumeroUnico
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UbicacionNumeroUnicoController : ApiController
    {
        public object Get(int proyectoID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                return UbicacionNumeroUnicoBd.Instance.ObtenerListadoRack(proyectoID);
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

        public object Post(Captura ListaCaptura, string token, string proyectoID, int rackID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                int[] array = new int[ListaCaptura.Detalles.Count];

                for (int i = 0; i < ListaCaptura.Detalles.Count; i++)
                {
                    array[i] = ListaCaptura.Detalles[i].Consecutivo;
                }

                int[] distinctArrayNumeroUnico = array.Distinct().ToArray();

                return UbicacionNumeroUnicoBd.Instance.ActualizarRack(distinctArrayNumeroUnico, int.Parse(proyectoID), usuario.UsuarioID, rackID);
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

        public object Get(string arrayNumeroUnico, string token, int proyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                string[] array = arrayNumeroUnico.Split(',');
                int[] arrayNu = new int[array.Length];

                string[] intervalo;
                int intervaloInicial;
                int intervaloFinal;


                List<int> lista = new List<int>();

                for (int i = 0; i < array.Length; i++)
                {
                    intervalo = array[i].Split('-');
                    if(intervalo.Length==2)
                    {
                        intervaloInicial = int.Parse(intervalo[0]);
                        intervaloFinal = int.Parse(intervalo[1]);
    
                            for (int j = intervaloInicial; j <= intervaloFinal; j++)
                        {
                            lista.Add(j);
                        }
                    }
                    else if (intervalo.Length == 1)
                        lista.Add(int.Parse(array[i]));
                }

                int[] distinctArrayNumeroUnico = arrayNu.Distinct().ToArray();
                return UbicacionNumeroUnicoBd.Instance.ObtenerRack(lista.ToArray(), proyectoID, usuario.UsuarioID);
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
