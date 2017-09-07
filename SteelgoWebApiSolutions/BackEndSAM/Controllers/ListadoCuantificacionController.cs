using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ListadoCuantificacionController : ApiController
    {

        // PUT api/<controller>/5
        public object Post(BackEndSAM.Models.Captura elementos,int ProyectoID, bool cerrar, bool incompletos, int FolioAvisollegadaId, int FolioCuantificacionID, string token, int idGuardado)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                StringBuilder stringbiuilderErrores = new StringBuilder();
                List<CuantificacionListado> list = new List<CuantificacionListado>();

                for (int i = 0; i < elementos.Detalles.Count; i++)
                {
                    list.Add(serializer.Deserialize<CuantificacionListado>(elementos.Detalles[i].cadena));
                }


                List<CuantificacionListado> listElementos = new List<CuantificacionListado>();

                for (int i = 0; i < list.Count; i++)
                {
                    List<CuantificacionListado> item = (List<CuantificacionListado>)GuardarItemCodesBd.Instance.GuardadoInformacionItemCodes(ProyectoID, cerrar, incompletos, FolioAvisollegadaId, FolioCuantificacionID, list[i], usuario, idGuardado);
                    if (item[0].TieneError)
                        stringbiuilderErrores.Append("," + item[0].ItemCode);
                    listElementos.Add(item[0]);
                }

                List<object> listaDetalleRegreso = new List<object>();
                listaDetalleRegreso.Add(stringbiuilderErrores.ToString());
                listaDetalleRegreso.Add(listElementos);

                return listaDetalleRegreso;
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

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}