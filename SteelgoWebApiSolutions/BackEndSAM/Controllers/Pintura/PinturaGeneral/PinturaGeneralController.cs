using BackEndSAM.DataAcces.Pintura.CargaCarro;
using BackEndSAM.DataAcces.Pintura.PinturaGeneral;
using BackEndSAM.Models.Pintura.PinturaGeneral;
using CommonTools.Libraries.Strings.Security;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Pintura.PinturaGeneral
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PinturaGeneralController : ApiController
    {
        [HttpGet]
        public object obtieneMedioTransporte(string token, string lenguaje, int proyectoID)
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
                    return PinturaGeneralBD.Instance.ObtenerMedioTransporte(lenguaje, proyectoID);

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
        public object ObtieneUnidadesMedidaPintura(string token, string lenguaje,int valor,int otrovalor)
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
                    return PinturaGeneralBD.Instance.obtenerListaUnidadMedida(lenguaje);

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

        public object Post(CapturaMedioTransporte captura, string token)
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
                    captura.UsuarioID = Usuario.UsuarioID;

                    List<CapturaMedioTransporte> list = new List<CapturaMedioTransporte>();
                    list.Add(captura);

                    DataTable dt = new DataTable();
                    dt = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(list);

                    return PinturaGeneralBD.Instance.GuardarMedioTransporte(dt);

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
        public object GetColor(string token, string Lenguaje)
        {
            //Create a generic return object
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return PinturaGeneralBD.Instance.ObtenerColor(Lenguaje);
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

        //public object Post(Captura listaImagenes, string token)
        //{

        //    string payload = "";
        //    string newToken = "";

        //    bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //    if (tokenValido)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


        //        Base64Security b = new Base64Security();
        //        string imagenDecodificada= b.RegresarCadena(((ImgSerializadas)listaImagenes.Detalles[0]).imgSerializada);

        //        FileStream fs = new FileStream(((ImgSerializadas)listaImagenes.Detalles[0]).imgSerializada, FileMode.Open);
        //        byte[] pdf = new byte[fs.Length];
        //        fs.Read(pdf, 0, (int)fs.Length);


        //        fs.Close();

        //        return PinturaGeneralBD.Instance.GuardarImagenSerializa(pdf);///arreglo de la imagen
        //    }
        //    else
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(payload);
        //        result.ReturnCode = 401;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = false;

        //        return result;
        //    }
        //}

        [HttpGet]
        public object DescargaCuadranteSpool(string token,int CarroID, int SpoolID, int CuadranteID, int CuadranteSam2ID, int CuadranteAnterior,int Pantalla)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return CargaCarroBD.Instance.DescargaCarroSpool(CarroID, SpoolID, CuadranteID, CuadranteSam2ID, usuario.UsuarioID, Pantalla);
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