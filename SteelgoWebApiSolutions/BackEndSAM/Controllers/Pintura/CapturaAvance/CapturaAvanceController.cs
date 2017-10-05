using BackEndSAM.DataAcces.Pintura;
using BackEndSAM.DataAcces.PinturaBD.CapturaAvanceBD;
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
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace BackEndSAM.Controllers.PinturaControllers.CapturaAvance
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CapturaAvanceController : ApiController
    {
        [HttpGet]
        public object ObtieneCamposPredeterminados(string token, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                string fechaShot = (string)CapturaAvanceBD.Instance.ObtenerCamposPredeterminados(usuario, lenguaje, 35);
                string fechaprim = (string)CapturaAvanceBD.Instance.ObtenerCamposPredeterminados(usuario, lenguaje, 36);
                string llenado = (string)CapturaAvanceBD.Instance.ObtenerCamposPredeterminados(usuario, lenguaje, 43);

                CamposPredeterminados CapturaAvanceCamposPredeterminados = new CamposPredeterminados();

                CapturaAvanceCamposPredeterminados = new CamposPredeterminados
                {
                    FechaShotblast = fechaShot,
                    FechaPrimario = fechaprim,
                    Llenado = llenado
                };

                return CapturaAvanceCamposPredeterminados;
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
        public object ObtenerCarrosCargados(string token, string lenguaje, int procesoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturaAvanceBD.Instance.ObtenerCarrosCerradosPorProceso(lenguaje, procesoID);
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
        public object ObtenerDetalleCarrosCargados(string token, int medioTransporteCargaID, string lenguaje, int sistemaPinturaProyectoID, int procesopinturaID,int colorID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtdetalle = (DataTable)CapturaAvanceBD.Instance.ObtenerListaMedioTransporteCargado(medioTransporteCargaID, lenguaje, sistemaPinturaProyectoID, procesopinturaID, colorID);

                string jsonConvertido = DataTableToJSON(dtdetalle, procesopinturaID, usuario.UsuarioID);// Convertir(dtdetalle, procesopinturaID, usuario.UsuarioID);

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
                        dict[col.ColumnName] = " ";//(List<PintorSpool>)listaObreros[1];
                    else if (col.ColumnName == "ListaObrerosGuargados")
                        dict[col.ColumnName] = (List<PintorSpool>)listaObreros[0];
                    else if (col.ColumnName == "ListaObrerosSeleccionados")
                        dict[col.ColumnName] = (List<PintorSpool>)listaObreros[0];
                    else
                        dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(list);
        }

        [HttpGet]
        public object getObreros(string token, int procesoPintura)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturaAvanceBD.Instance.ObtenerObreros(procesoPintura, usuario.UsuarioID);
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
        public object ObtenerLayoutPorProceso(string token, int sistemaPinturaProyectoId, int procesoID, string lenguaje)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                return CapturaAvanceBD.Instance.ObtenerLayoutProcesoPintura(sistemaPinturaProyectoId, procesoID, lenguaje);
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
        public object ObtenerDetalleSpoolAgregar(string token, int OrdenTrabajoSpoolID, string lenguaje, int procesoPinturaID,int CargaCarroID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
              
                DataTable dtdetalle = (DataTable)CapturaAvanceBD.Instance.ObtenerSpoolNuevo(OrdenTrabajoSpoolID, lenguaje, procesoPinturaID, usuario.UsuarioID, CargaCarroID);

                string jsonConvertido = DataTableToJSON(dtdetalle, procesoPinturaID, usuario.UsuarioID);// Convertir(dtdetalle, procesopinturaID, usuario.UsuarioID);

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

        [HttpGet]
        public object ObtenerLotes(string token, string componente, string lenguaje, int tipoConsulta)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                if (tipoConsulta == 0)
                    return CapturaAvanceBD.Instance.ObtenerLotesComponentes(componente, lenguaje);
                else
                    return CapturaAvanceBD.Instance.ObtenerLotesReductor(componente, lenguaje);
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

        public object Post(Captura listaCapturasRequisicion, string token, string lenguaje, int cargaCarroID)
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
                        dtDetalleObreros= BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(item.ListaObrerosSeleccionados);
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

                dtDetalleSpool=  BackEndSAM.Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(listaCapturasRequisicion.Detalles);

               
                dtDetalleSpool.Columns.Remove("ListaObrerosSeleccionados");
                dtDetalleSpool.Columns.Remove("ListaComponentesDinamicos");

                for (int i = 0; i < dtDetalleSpool.Rows.Count; i++)
                {
                    dtDetalleSpool.Rows[i]["ID"] = i + 1;
                }

                return CapturaAvanceBD.Instance.GuardarAvanceCarro(dtDetalleSpool, dtDetalleObreros, dtDetalleComponentes, usuario, lenguaje, cargaCarroID);
               
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
