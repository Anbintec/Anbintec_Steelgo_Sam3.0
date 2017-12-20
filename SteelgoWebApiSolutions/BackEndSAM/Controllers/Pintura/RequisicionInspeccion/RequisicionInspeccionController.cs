using BackEndSAM.DataAcces.Pintura.RequisicionInspeccion;
using BackEndSAM.Models.Pintura.RequisicionInspeccion;
using BackEndSAM.Models.Sam3General.CapurasRapidas;
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

namespace BackEndSAM.Controllers.Pintura.RequisicionInspeccion
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RequisicionInspeccionController : ApiController
    {        

        public object GetSpoolIDRequisitado(string token, string OrdenTrabajo,  string Lenguaje)
        {
            try
            {                
                string payload = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (tokenValido)
                {
                    //IdOrdenTrabajoPintura idOrdenTrabajo = new IdOrdenTrabajoPintura();
                    IdOrdenTrabajo idOrdenTrabajo = new IdOrdenTrabajo();
                    List<Sam3_Pintura_GET_SpoolIDRequisitado_Result> lista = (List<Sam3_Pintura_GET_SpoolIDRequisitado_Result>)RequisicionInspeccionBD.Instance.ObtenerSpoolID(OrdenTrabajo, Lenguaje);
                    //List<Spool> listaAtatus = new List<Spool>();
                    List<IDS> listaAtatus = new List<IDS>();
                    if (lista.Count > 0)
                    {
                        //listaAtatus.Add(new Spool());
                        listaAtatus.Add(new IDS());
                        foreach (var item in lista)
                        {
                            listaAtatus.Add(new IDS { Status = item.status, IDValido = item.ID, Proyecto = item.NombreProyecto, Valor = item.OrdenTrabajoSpoolID, ProyectoID = item.ProyectoID });
                            //listaAtatus.Add(new Spool { SpoolID = item.SpoolID, OrdenTrabajo = item.OrdenTrabajo, ID = item.ID, OrdenTrabajoSpoolID = item.OrdenTrabajoSpoolID });
                        }
                        //idOrdenTrabajo = new IdOrdenTrabajoPintura
                        idOrdenTrabajo = new IdOrdenTrabajo
                        {
                            OrdenTrabajo = lista[0].OrdenTrabajo,
                            idStatus = listaAtatus
                        };
                    }
                    else
                    {
                        //idOrdenTrabajo = new IdOrdenTrabajoPintura
                        idOrdenTrabajo = new IdOrdenTrabajo
                        {
                            OrdenTrabajo = "",
                            idStatus = listaAtatus
                        };
                    };
                    return idOrdenTrabajo;
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

        public object GetInfoBySpool(string NumeroControl, string token)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    return RequisicionInspeccionBD.Instance.ObtenerInfoPorSpool(NumeroControl);
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
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;
            }
        }
        
        public object ObtenerSpoolsGuardados(GetSpools ListaSpools, string token, bool Param, bool OtroParam)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    DataTable tableSpools = new DataTable();
                    if(ListaSpools.Detalle != null)
                    {
                        tableSpools = ToDataTable(ListaSpools.Detalle);                    
                    }
                    return RequisicionInspeccionBD.Instance.ObtenerSpoolsGuardados(tableSpools);
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
                TransactionalInformation result = new TransactionalInformation();
                result.ReturnMessage.Add(ex.Message);
                result.ReturnCode = 500;
                result.ReturnStatus = false;
                result.IsAuthenicated = true;
                return result;                
            }
        }
        
        [HttpPost]
        public object Post(Datos Captura, string token, int Param)
        {
            try
            {
                string payLoad = "";
                string newToken = "";
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payLoad, out newToken);
                if (tokenValido)
                {
                    Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payLoad);
                    DataTable dtListaDatos = new DataTable();
                    if (Captura.Detalle != null)
                    {
                        dtListaDatos = ToDataTable(Captura.Detalle);
                    }
                    return RequisicionInspeccionBD.Instance.GuardaCapturaRequisicionInspeccion(dtListaDatos, Usuario);
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



        /*DATATABLE CONVERSION*/
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
