using BackEndSAM.DataAcces.ServiciosTecnicos.GenerarRequisicion;
using BackEndSAM.Models.ServiciosTecnicos.RequisicionPND;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.ServiciosTecnicos.RequisicionPND
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RequisicionPNDController : ApiController
    {
        [HttpGet]
        public object ObtieneElementosPorPrueba(string token, string lenguaje, int RequisicionID, int TipoPruebaID, int ProyectoID, string Muestra)
        {
            string payload = "";
            string newToken = "";
            int all = Muestra.ToLower() == "todos" ? 1 : 0;
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return RequisicionPNDDB.Instance.ObtenerListadoElementos(lenguaje, RequisicionID, TipoPruebaID, ProyectoID, all);
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
        public object Post(Captura listaElementosPorReq, string token)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DataTable dtDetalleCaptura = new DataTable();
                if (listaElementosPorReq.listaDetalle != null)
                {
                    dtDetalleCaptura = ToDataTable(listaElementosPorReq.listaDetalle);
                }

                return RequisicionPNDDB.Instance.InsertarNuevaRequisicion(
                    dtDetalleCaptura, listaElementosPorReq.RequisicionID, listaElementosPorReq.Requisicion, 
                    listaElementosPorReq.ProyectoID, listaElementosPorReq.TipoPruebaID, listaElementosPorReq.FechaRequisicion, 
                    listaElementosPorReq.FolioCliente == null ? "" : listaElementosPorReq.FolioCliente, 
                    listaElementosPorReq.Observacion, listaElementosPorReq.Lenguaje, usuario);
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

        public static DataTable ToDataTable<T>(List<T> l_oItems)
        {
            DataTable oReturn = new DataTable(typeof(T).Name);
            object[] a_oValues;
            int i;

            //#### Collect the a_oProperties for the passed T
            PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            //#### Traverse each oProperty, .Add'ing each .Name/.BaseType into our oReturn value
            //####     NOTE: The call to .BaseType is required as DataTables/DataSets do not support nullable types, so it's non-nullable counterpart Type is required in the .Column definition
            foreach (PropertyInfo oProperty in a_oProperties)
            {
                oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
            }

            //#### Traverse the l_oItems
            foreach (T oItem in l_oItems)
            {
                //#### Collect the a_oValues for this loop
                a_oValues = new object[a_oProperties.Length];

                //#### Traverse the a_oProperties, populating each a_oValues as we go
                for (i = 0; i < a_oProperties.Length; i++)
                {
                    a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                }

                //#### .Add the .Row that represents the current a_oValues into our oReturn value
                oReturn.Rows.Add(a_oValues);
            }

            //#### Return the above determined oReturn value to the caller
            return oReturn;
        }

        public static Type BaseType(Type oType)
        {
            //#### If the passed oType is valid, .IsValueType and is logicially nullable, .Get(its)UnderlyingType
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            //#### Else the passed oType was null or was not logicially nullable, so simply return the passed oType
            else
            {
                return oType;
            }
        }

        [HttpGet]
        public object ObtieneSpools(string token, string IdOrdenTrabajo, int OrdenTrabajoSpoolID, int TipoPruebaID, int ProyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return RequisicionPNDDB.Instance.ObtieneSpools(usuario.UsuarioID, IdOrdenTrabajo, OrdenTrabajoSpoolID, TipoPruebaID, ProyectoID);
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
        public object ObtieneJuntasXSpool(string ordenTrabajo, string id, string sinCaptura, string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return RequisicionPNDDB.Instance.ObtenerJuntasXSpoolID(usuario, ordenTrabajo, id, sinCaptura == "Todos" ? 1 : 0);
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
        public object ObtieneDetalleXJunta(string token, string IdOrdenTrabajo, int OrdenTrabajoSpoolID, int TipoPruebaID, int ProyectoID,int JuntaSpoolID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return RequisicionPNDDB.Instance.ObtienedetalleJunta(usuario.UsuarioID, IdOrdenTrabajo, OrdenTrabajoSpoolID, TipoPruebaID, ProyectoID, JuntaSpoolID);
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