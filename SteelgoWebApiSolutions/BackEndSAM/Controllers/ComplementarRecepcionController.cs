using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using BackEndSAM.DataAcces;
using BackEndSAM.Models;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System.Text;

namespace BackEndSAM.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ComplementarRecepcionController : ApiController
    {

        public object Get(string proyectoID, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Sam3_Usuario usuario = ser.Deserialize<Sam3_Usuario>(payload);
                return FoliosCuantificacionBd.Instance.ListadoFoliosCuantificacionProProyecto(Convert.ToInt32(proyectoID), usuario);
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

        public object Get(int folio, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return ComplementoRecepcionBd.Instance.ObtenerListado(folio, usuario);
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

        public object Put(int folio, string estatus, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return FoliosCuantificacionBd.Instance.CambiarEstatus(folio, estatus, usuario);
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

        public object Put(BackEndSAM.Models.Captura elementos, string tipoGuardado, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                StringBuilder stringbiuilderErrores = new StringBuilder();

                List<ItemCodeComplemento> list = new List<ItemCodeComplemento>();

                for (int i = 0; i < elementos.Detalles.Count; i++)
                {
                    list.Add(serializer.Deserialize<ItemCodeComplemento>(elementos.Detalles[i].cadena));
                }

                List<ItemCodeComplemento> listElementos = new List<ItemCodeComplemento>();

                for (int i = 0; i < list.Count; i++)
                {
                    ItemCodeComplemento item = (ItemCodeComplemento)ComplementoRecepcionBd.Instance.GuardarComplemento(Convert.ToInt32(tipoGuardado), list[i], usuario);
                    if (item.TieneError)
                        stringbiuilderErrores.Append("," + item.ItemCode + " " + item.DescripcionError);
                    listElementos.Add(item);
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

        public object Delete(int folio, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return null;
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

        public object Post(string datos, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                EditarItemCodeMasDetalle dat = serializer.Deserialize<EditarItemCodeMasDetalle>(datos);
                return ComplementoRecepcionBd.Instance.ReemplazarItemCode(dat, usuario);
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