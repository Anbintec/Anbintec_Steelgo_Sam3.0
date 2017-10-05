using BackEndSAM.DataAcces.Pintura.AvanceCuadrante;
using BackEndSAM.DataAcces.PinturaBD.CapturaAvanceBD;
using BackEndSAM.Models.Pintura.AvanceCuadrante;
using BackEndSAM.Models.Pintura.CapturaAvance;
using DatabaseManager.Sam3;
using Newtonsoft.Json;
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

namespace BackEndSAM.Controllers.Pintura.AvanceCuadrante
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AvanceCuadranteController : ApiController
    {
        //ObtenerListadoZonas y ObtenerListadoCuadrantes
        public object Get(string token, int procesoPintura, int ProyectoID,int ZonaID, int tipo)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                if (tipo == 1)

                    return AvanceCuadranteBD.Instance.ObtenerListadoZonas(usuario, procesoPintura, ProyectoID);
                else
                    return AvanceCuadranteBD.Instance.ObtenerListadoCuadrantes(usuario, ZonaID, procesoPintura);
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
        //ObtenerListadoSistemaPintura
        public object Get(string token, int ZonaID, int CuadranteID, int procesoPintura, string lenguaje,int proyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                return AvanceCuadranteBD.Instance.ObtenerListadoSistemaPintura(usuario, ZonaID, CuadranteID, procesoPintura, lenguaje, proyectoID);
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
        //ObtenerListadoColores
        public object Get(string token, int sistemaPinturaProyectoID, string lenguaje,int cuadranteID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);


                return AvanceCuadranteBD.Instance.ObtenerListadoColores(sistemaPinturaProyectoID, lenguaje, cuadranteID);
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
        //Obtiene Detalle
        public object GET(string token,int proyectoID, int cuadranteID, int sistemaPinturaProyectoID, int? sistemaPinturaColorID, string lenguaje, int procesoPinturaID, int todosSinCaptura)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtdetalle = (DataTable)AvanceCuadranteBD.Instance.ObtenerDetalle(proyectoID, cuadranteID, sistemaPinturaProyectoID, sistemaPinturaColorID, lenguaje, procesoPinturaID, todosSinCaptura, usuario.UsuarioID);

                string jsonConvertido = DataTableToJSON(dtdetalle, procesoPinturaID, usuario.UsuarioID);

                return jsonConvertido;
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
        //Obtenemos los Trabajadores Guardados
        public object GET(string token, int spoolID, int procesoPinturaID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return  (List<object>)CapturaAvanceBD.Instance.ObtenerObrerosGuardados(spoolID, procesoPinturaID, usuario.UsuarioID);
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
        public static string DataTableToJSON(DataTable table, int procesopinturaID, int usuario)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();
                List<object> listaObreros = (List<object>)CapturaAvanceBD.Instance.ObtenerObrerosGuardados(int.Parse(row["SpoolID"].ToString()), procesopinturaID, usuario);

                foreach (DataColumn col in table.Columns)
                {
                    if (col.ColumnName == "ListaObreros")
                        dict[col.ColumnName] = " ";// (List<PintorSpool>)listaObreros[1];
                    else if (col.ColumnName == "ListaObrerosGuargados")
                        dict[col.ColumnName] = (List<PintorSpool>)listaObreros[0];
                    else if (col.ColumnName == "ListaObrerosSeleccionados")
                        dict[col.ColumnName] =  (List<PintorSpool>)listaObreros[0];
                    else
                        dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            //string jsonString = string.Empty;
            //jsonString = JsonConvert.SerializeObject(table);
            //return jsonString;
            return serializer.Serialize(list);
        }
       

        public object Post(Captura listaCapturasRequisicion, string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {

                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                DataTable dtDetalleSpool = new DataTable();
                DataTable dtDetalleObreros = null;
                DataTable dtDetalleComponentes = null;

                foreach (var item in listaCapturasRequisicion.Detalles)
                {
                    if (dtDetalleObreros == null)
                    {
                        dtDetalleObreros = BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaObrerosSeleccionados);
                    }
                    else
                        dtDetalleObreros.Merge(BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaObrerosSeleccionados));
                    if (dtDetalleComponentes == null)
                    {
                        dtDetalleComponentes = BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaComponentesDinamicos);
                    }
                    else
                        dtDetalleComponentes.Merge(BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaComponentesDinamicos));
                }

                dtDetalleSpool = BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturasRequisicion.Detalles);


                dtDetalleSpool.Columns.Remove("ListaObrerosSeleccionados");
                dtDetalleSpool.Columns.Remove("ListaComponentesDinamicos");

                for (int i = 0; i < dtDetalleSpool.Rows.Count; i++)
                {
                    dtDetalleSpool.Rows[i]["ID"] = i + 1;
                }

                return AvanceCuadranteBD.Instance.GuardarAvanceCuadrante(dtDetalleSpool, dtDetalleObreros, dtDetalleComponentes, usuario, lenguaje);

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
