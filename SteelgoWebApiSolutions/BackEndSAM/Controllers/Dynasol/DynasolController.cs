using BackEndSAM.DataAcces.Dynasol;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using SecurityManager.TokenHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;
using static BackEndSAM.Models.Dynasol.Dynasol;

namespace BackEndSAM.Controllers.Dynasol
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DynasolController : ApiController
    {
        [HttpGet]
        public object ObtenerMedidas(string token, string Lenguaje)
        {
            string payLoad = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            if (tokenValido)
            {
                return DynasolBD.Instance.ObtenerMedidas(Lenguaje);
            }
            else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
        [HttpGet]
        public object ObtenerOrdenesCompra(string token)
        {
            string payLoad = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            if (tokenValido)
            {
                return DynasolBD.Instance.ObtenerOrdenesCompra();
            }else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
        [HttpGet]
        public object ObtenerListaInspeccion(string token, bool flag)
        {
            string payLoad = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            if (tokenValido && flag)
            {                
                return DynasolBD.Instance.ObtenerListaInspeccion();                
            }else
            {                
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        [HttpGet]
        public object ObtenerRevision(string token, int OrdenCompraID, string Lenguaje)
        {
            string payLoad = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            if (tokenValido)
            {
                return DynasolBD.Instance.ObtenerRevision(OrdenCompraID, Lenguaje);
            }else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
                result.ReturnCode = 401;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }
        
        [HttpPost]               
        public object GuardarCaptura(Captura ListaCaptura, string token, string lenguaje)
        {
            string payLoad = "";
            string newToken = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
            if (tokenValido)
            {
                Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payLoad);
                DataTable DynasolRevision = null;
                DataTable DynasolColada = null;
                DataTable DynasolDetalle = null;
                foreach (JsonCapturaRevision item in ListaCaptura.Detalles)
                {
                    if (item.ListaColadas != null)
                    {
                        if (DynasolColada == null)
                            DynasolColada = ToDataTable(item.ListaColadas);
                        else
                            DynasolColada.Merge(ToDataTable(item.ListaColadas));
                    }

                    if (item.ListaDetalleInspeccion != null)
                    {
                        if (DynasolDetalle == null)
                            DynasolDetalle = ToDataTable(item.ListaDetalleInspeccion);
                        else
                            DynasolDetalle.Merge(ToDataTable(item.ListaDetalleInspeccion));

                    }
                }
                if (ListaCaptura.Detalles != null)
                {
                    DynasolRevision = new DataTable();
                    DynasolRevision = ToDataTable(ListaCaptura.Detalles);                    
                }
                DynasolRevision.Columns.Remove("ListaColadas");
                DynasolRevision.Columns.Remove("ListaDetalleInspeccion");
                return DynasolBD.Instance.GuardarCapturaDynasol(DynasolRevision, DynasolColada, DynasolDetalle, Usuario, lenguaje);
            }else
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(payLoad);
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

    }
}
