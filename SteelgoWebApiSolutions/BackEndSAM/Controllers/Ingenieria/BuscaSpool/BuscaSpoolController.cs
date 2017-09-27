using BackEndSAM.DataAcces.Ingenieria.BuscaSpool;
using BackEndSAM.Models.Ingenieria.BuscaSpool;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.Ingenieria
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class BuscaSpoolController: ApiController
    {
        [HttpGet]
        public object ObtenerListadoTipoCorte(string tkn)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(tkn, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneListadoTipoCorte();
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
        public object ObtenerListadoTipoSalida(string token)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneListadoTipoSalida();
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
        public object ObtieneListadoSpool(string token, int ProyectoID, string SpoolContiene)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneListadoSpool(ProyectoID, SpoolContiene);
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
        public object ObtieneListadoJuntaSpool(string token, int SpoolID)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneListadoJuntaSpool(SpoolID);
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
        public object ObtieneDetalleLoop(string token, int ProyectoID, int ProyectoSpoolID, int ProyectoSoporteID, string NombreLoop)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneDetalleLoop(ProyectoID, ProyectoSpoolID, ProyectoSoporteID, NombreLoop, usuario.UsuarioID);
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
        public object ObtieneDetalleMaterialSpool(string token, int Spool)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneDetalleMaterialSpool(Spool);
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
        public object ObtieneDetalleSpool(string token, int ProyectoID, string Spool)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneDetalleSpool(usuario.UsuarioID, ProyectoID, Spool);
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
        public object ObtieneDetalleJuntaSpool(string token, int JuntaSpoolID)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return BuscaSpoolBD.Instance.ObtieneDetalleJuntaSpool(JuntaSpoolID);
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

        [HttpPost]
        public object GuardarCaptura(SpoolMasterGuardado Captura, string token)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                List<DetalleSalidasGuardado> listadoSalidas = new List<DetalleSalidasGuardado>();
                List<DetalleAgrupadoSalidas> listaAgrupadoSalidas = new List<DetalleAgrupadoSalidas>();

                foreach (DetalleSalidas item in Captura.detalleSalidas)
                {

                    listadoSalidas.Add(new DetalleSalidasGuardado
                    {
                        Detalle_SalidasID = item.Detalle_SalidasID,
                        SpoolID = item.SpoolID,
                        Posicion = item.Posicion,
                        RevisionSteelgo = item.RevisionSteelgo,
                        RevisionCliente = item.RevisionCliente,
                        FamiliaAcero1ID = item.FamiliaAcero1ID,
                        FamiliaAcero2ID = item.FamiliaAcero2ID,
                        Especificacion = item.Especificacion,
                        PDI = item.PDI,
                        SistemaPintura = item.SistemaPintura,
                        ColorPintura = item.ColorPintura,
                        Cedula = item.Cedula,
                        Dibujo = item.Dibujo,
                        Peso = item.Peso,
                        Area = item.Area
                    });

                   if (item.detalleAgrupadoSalidas!=null)
                    {
                        foreach (DetalleAgrupadoSalidas x in item.detalleAgrupadoSalidas)
                        {
                            listaAgrupadoSalidas.Add(x);
                        }
                    }
                   
                }
                DataTable dtDetalleSalidas = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listadoSalidas);
                DataTable dtDetalleAgrupadoSalidas = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaAgrupadoSalidas);

                return BuscaSpoolBD.Instance.GuardarCaptura(dtDetalleSalidas, dtDetalleAgrupadoSalidas, Captura, usuario.UsuarioID);
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