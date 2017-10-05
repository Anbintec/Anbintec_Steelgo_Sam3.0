using BackEndSAM.DataAcces.Pintura.OkPintura;
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
using static BackEndSAM.Models.Pintura.OK.OkPintura;

namespace BackEndSAM.Controllers.Pintura
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OkPinturaController : ApiController
    {
        public object GetNumeroElementos(string token, int ProyectoID, string NumControl, string Muestra)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                int MuestraInt = Muestra.ToLower() == "todos" ? 1 : 0;
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return OkPinturaBD.Instance.ObtenerNumeroElementosOkPintura(ProyectoID, NumControl, MuestraInt);
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
        public object GetNumeroControlValido(string token, int ProyectoID, string NumControl)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return OkPinturaBD.Instance.NumControlExisteEnProyecto(ProyectoID, NumControl);
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

        public object Get(string token, string Lenguaje, int ProyectoID, string NumControl, string Muestra)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                int muestra = Muestra.ToLower() == "todos" ? 1 : 0;
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return OkPinturaBD.Instance.ObtenerListaElementosOkPintura(Lenguaje, ProyectoID, NumControl, muestra);
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
        public object GetPruebas(string token, string Lenguaje, int SpoolID, int ProcesoID)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return OkPinturaBD.Instance.ObtenerDetallePruebas(Lenguaje, SpoolID, ProcesoID);
                }
                else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 500;
                    //result.ReturnCode = 401;
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

        [HttpPost]
        public object GuardarCaptura(Datos Captura, string token, string Lenguaje, int ProyectoID)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                if (tokenValido)
                {
                    Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payLoad);
                    DataTable dtListaDatos = new DataTable();
                    if(Captura.Detalle != null)
                    {
                        dtListaDatos = ToDataTable(Captura.Detalle);
                    }
                    return OkPinturaBD.Instance.GuardaCaptura(dtListaDatos, Lenguaje, Usuario, ProyectoID);
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
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpPost]
        public object GuardarCapturaMasiva(CapturaOKMasiva Datos, string Lenguaje, string token, int ProyectoID, bool Flag)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido && Flag)
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payLoad);
                    List<ElementosOKMasivo> Elementos = serializer.Deserialize<List<ElementosOKMasivo>>(Datos.Detalle);
                    DataTable dtDatosMasivos = new DataTable();
                    if(Elementos != null)
                    {
                        dtDatosMasivos = ToDataTable(Elementos);
                    }
                    return OkPinturaBD.Instance.GuardaCapturaMasiva(dtDatosMasivos, ProyectoID, Lenguaje, Usuario);
                }else
                {
                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add(payLoad);
                    result.ReturnCode = 401;
                    result.ReturnStatus = true;
                    result.IsAuthenicated = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = false;
                return result;
            }
        }

        /*DATATABLE CONVERSION*/
        public static System.Data.DataTable ToDataTable<T>(List<T> l_oItems)
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
