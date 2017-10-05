using BackEndSAM.DataAcces.Pintura.PinturaGeneral;
using BackEndSAM.DataAcces.Pintura.RevisionPintura;
using BackEndSAM.Models.Pintura.PinturaGeneral;
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

namespace BackEndSAM.Controllers.Pintura.RevisionPintura
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RevisionPinturaController : ApiController
    {
        //obtenemos la spools a partir de la orden de trabajo.
        public object Get( string token, string lenguaje,int proyectoid,string dato,int tipoBusqueda)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                
                return RevisionPinturaBD.Instance.ObtenerSpoolConSP(proyectoid,dato,tipoBusqueda, lenguaje);
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

        public object Post(GuardarRevisionPintura listaCaptura, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
               
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCaptura.Detalles);
               
                return BackEndSAM.DataAcces.Pintura.RevisionPintura.RevisionPinturaBD.Instance.GuardarSpoolRevision(dtDetalleCaptura, usuario.UsuarioID);
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
        public object Put(ElementosCapturados listaCapturaActualizar, string token, string lenguaje, string SinCaptura)
        {
            string payload = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleActualizaCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturaActualizar.Detalles);
                List<PinturaRevision> listaDetalleDatos = new List<PinturaRevision>();

                return RevisionPinturaBD.Instance.ActualizaDatos(dtDetalleActualizaCaptura,lenguaje);
  
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
        //obtenemos la cantidad de  spools a partir de la orden de trabajo.
        public object Get(string token, int proyectoid, string dato, int tipoBusqueda)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return  RevisionPinturaBD.Instance.ObtenerCantidadSpoolConSP(proyectoid, dato, tipoBusqueda);
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
        
        //Obtenemos los catalogos para el planchado SP y Motivos
        public object Get(string token, string lenguaje, int proyectoid)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return RevisionPinturaBD.Instance.ObtenerCatalogosPlanchado(proyectoid, lenguaje);
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
