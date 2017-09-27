using BackEndSAM.DataAcces.Sam3General.Proveedores;
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

namespace BackEndSAM.Controllers.Sam3General.Proveedor
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    public class ProveedoresController: ApiController
    {
        [HttpGet]
        public object ObtenerProveedores(string token, int ProyectoID, int TipoProveedor)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return ProveedoresBD.Instance.ObtenerProveedores(ProyectoID, TipoProveedor);
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
        public object GuardarNuevoProveedorEmbarque(string token, string NombreProveedor, int ProyectoID, string Descripcion, string Direccion, string Telefono, int TipoProveedor)
        {
            string payload = "";
            string newToken = "";

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dt = new DataTable();
                return ProveedoresBD.Instance.GuardarNuevoProveedor(NombreProveedor, usuario.UsuarioID, ProyectoID, Descripcion, Direccion, Telefono, TipoProveedor);
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