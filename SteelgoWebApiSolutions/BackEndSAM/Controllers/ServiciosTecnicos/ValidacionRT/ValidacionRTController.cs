using BackEndSAM.DataAcces.ServiciosTecnicos.ValidacionRT;
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

namespace BackEndSAM.Controllers.ServiciosTecnicos.ValidacionRT
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValidacionRTController : ApiController
    {
        public object Get(string token) //PROYECTOS
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

                    //return ValidacionRTDB.Instance.ObtenerListadoProyectos(Usuario);
                    return ValidacionRTDB.Instance.ObtenerProyectos(Usuario.UsuarioID);
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

        public object Get(string token, int ProyectoID) //TIPOS PRUEBAS
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

                    return ValidacionRTDB.Instance.ObtenerTipoPruebas(ProyectoID);
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

        public object Get(string token, int ProyectoID, int TipoPruebaID) //PROVEEDORES
        {
            try
            {
                string payload = "";
                string newToken = "";
                bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (totokenValido)
                {
                    return ValidacionRTDB.Instance.ObtenerListadoProveedores(ProyectoID, TipoPruebaID);                    
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

        public object Get(string token, int ProyectoID, int ProveedorID, bool Flag) //REQUISICIONES
        {
            try
            {
                string payload = "";
                string newToken = "";
                bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
                if (totokenValido && Flag)
                {
                    return ValidacionRTDB.Instance.ObtenerListadoRequisiciones(ProyectoID, ProveedorID);
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
        public object GetEquipos(string token, int TipoPruebaID, int ProveedorID, string Lenguaje) //EQUIPOS
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                //return ValidacionRTDB.Instance.ObtenerEquipos(TipoPruebaID, ProveedorID, Lenguaje);
                return ValidacionRTDB.Instance.ObtenerListadoEquipos(TipoPruebaID, ProveedorID, Lenguaje);
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
        public object GetTurnos(string token, int TipoPruebaID, int ProveedorID, int EquipoID, string Lenguaje, bool Flag)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido && Flag)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                //return ValidacionRTDB.Instance.ObtenerTurnos(TipoPruebaID, ProveedorID, EquipoID, Lenguaje);
                return ValidacionRTDB.Instance.ObtenerListadoTurnos(TipoPruebaID,ProveedorID,EquipoID, Lenguaje);
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

        public object Get(string token, int ProyectoID, int TipoPruebaID, int ProveedorID, int RequisicionID, int EquipoID, int TurnoID, string Lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {                
                return ValidacionRTDB.Instance.ObtenerRequisicionesDetalle(ProyectoID, TipoPruebaID, ProveedorID, RequisicionID, EquipoID, TurnoID, Lenguaje);
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



        //public object Get(string token, int ProyectoID, int TipoPruebaID)
        //{
        //    try
        //    {
        //        string payload = "";
        //        string newToken = "";
        //        bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //        if (totokenValido)
        //        {
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payload);

        //            return ValidacionRTDB.Instance.ObtenerListadoProveedoresVR(ProyectoID, TipoPruebaID);
        //        }
        //        else
        //        {
        //            TransactionalInformation result = new TransactionalInformation();
        //            result.ReturnMessage.Add(payload);
        //            result.ReturnCode = 401;
        //            result.ReturnStatus = false;
        //            result.IsAuthenicated = false;
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;
        //        return result;
        //    }
        //}

        //public object Get(string token, int ProveedorID, int ProyectoID, string NombreProveedor, string Password)
        //{
        //    try
        //    {
        //        string payload = "";
        //        string newToken = "";
        //        bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //        if (totokenValido)
        //        {
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payload);

        //            return ValidacionRTDB.Instance.ObtenerLOGINProveedor(ProveedorID, ProyectoID, NombreProveedor, Password);
        //        }
        //        else
        //        {
        //            TransactionalInformation result = new TransactionalInformation();
        //            result.ReturnMessage.Add(payload);
        //            result.ReturnCode = 401;
        //            result.ReturnStatus = false;
        //            result.IsAuthenicated = false;
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;
        //        return result;
        //    }
        //}

        //public object Get(string token, int ProyectoID, int TipoPruebaID, int ProveedorID, int RequisicionID, string Lenguaje)
        //{
        //    try
        //    {
        //        string payload = "";
        //        string newToken = "";
        //        bool totokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //        if (totokenValido)
        //        {
        //            JavaScriptSerializer serializer = new JavaScriptSerializer();
        //            Sam3_Usuario Usuario = serializer.Deserialize<Sam3_Usuario>(payload);

        //            return ValidacionRTDB.Instance.ObtenerListadoElementosVR(ProyectoID, TipoPruebaID, ProveedorID, RequisicionID, Lenguaje);
        //        }
        //        else
        //        {
        //            TransactionalInformation result = new TransactionalInformation();
        //            result.ReturnMessage.Add(payload);
        //            result.ReturnCode = 401;
        //            result.ReturnStatus = false;
        //            result.IsAuthenicated = false;
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TransactionalInformation result = new TransactionalInformation();
        //        result.ReturnMessage.Add(ex.Message);
        //        result.ReturnCode = 500;
        //        result.ReturnStatus = false;
        //        result.IsAuthenicated = true;
        //        return result;
        //    }
        //}

        //public object GetRequisicionesDetalle(string token, int proyectoID, int tipoPruebaID, int proveedorID, int requisicionID, int equipoID, int turnoID, string lenguaje)
        //{
        //    string payload = "";
        //    string newToken = "";
        //    bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
        //    if (tokenValido)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


        //        return ValidacionRTDB.Instance.ObtenerRequisicionesDetalle(proyectoID, tipoPruebaID, proveedorID, requisicionID, equipoID, turnoID, lenguaje);
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

        //public object Post(Models.ServiciosTecnicos.ValidacionRT.DetalleCapturaReporteVRT Captura, string token, string lenguaje)
        //{
        //    string payload = "";
        //    string newToken = "";
        //    bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);

        //    if (tokenValido)
        //    {
        //        JavaScriptSerializer serializer = new JavaScriptSerializer();
        //        Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
        //        DataTable dtDetalleCaptura = ValidacionRTController.ToDataTable(Captura.Detalles);

        //        return ValidacionRTDB.Instance.ActualizaCapturaReportesRT(dtDetalleCaptura, usuario.UsuarioID, lenguaje);
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