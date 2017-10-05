using BackEndSAM.DataAcces.Pintura.SistemaPinturaAplicable;
using BackEndSAM.Models.Pintura.SistemaPinturaAplicable;
using BackEndSAM.Utilities.ConvertirDataTable;
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

namespace BackEndSAM.Controllers.Pintura.SistemaPinturaAplicable
{
    [EnableCors(origins:"*", headers:"*",methods:"*")]
    public class SistemaPinturaAplicableController : ApiController
    {
        [HttpGet]
        public object ObtieneSistemaPintura(string token, int ProyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return SistemaPinturaAplicableBD.Instance.ObtieneListadoSistemaPintura(ProyectoID);
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
        public object ObtieneColorPintura(string token, int SistemaPinturaID, string Lenguaje,int proyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return SistemaPinturaAplicableBD.Instance.ObtieneListadoColorPintura(SistemaPinturaID, Lenguaje, proyectoID);
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
        public object ObtieneNumeroElementosPorBusqueda(string token, int ProyectoID, int TipoBusqueda, string Cadena)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return SistemaPinturaAplicableBD.Instance.ObtieneNumeroElementosPorBusqueda(ProyectoID, TipoBusqueda, Cadena);
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
        public object ObtieneDetalleSpool(string token, int ProyectoID, int TipoBusqueda, string Cadena, string Lenguaje) {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);

                return SistemaPinturaAplicableBD.Instance.ObtieneDetalleSpool(ProyectoID, TipoBusqueda, Cadena, Lenguaje);
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
        public object GuardaCapturaSistemaPinturaAplicable(Captura captura,string token)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(captura.detalle);

                return SistemaPinturaAplicableBD.Instance.InsertaCapturaSistemaPinturaAplicable(dtDetalleCaptura, usuario.UsuarioID);
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
        public object GuardaCapturaSistemaPinturaAplicableMasivo(CargaMasiva captura, string token, int TipoCarga, string Lenguaje, int ProyectoID)
        {
            string payload = "";
            string newToken = "";
            bool tokenValido = ManageTokens.Instance.ValidateToken(token, out payload, out newToken);
            if (tokenValido)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                Sam3_Usuario usuario = serializer.Deserialize<Sam3_Usuario>(payload);
               
                //int posicion = 0;
                //while (captura.detalle.Count>0 && posicion < captura.detalle.Count)
                //{
                //    if (captura.detalle[posicion].Color == null && captura.detalle[posicion].NombreSpool == null && captura.detalle[posicion].NumeroControl == null && captura.detalle[posicion].SistemaPintura == null)
                //    {
                //        captura.detalle.RemoveAt(posicion);
                //        posicion--;
                //    }
                //    else
                //        posicion++;
                //}

                DataTable dtDetalleCaptura = Utilities.ConvertirDataTable.ToDataTable.Instance.toDataTable(captura.detalle);
                dtDetalleCaptura.Columns.Add(new DataColumn("Resultado"));
                DataTable dtDetalleCapturaPorRegistro = null;

                if (dtDetalleCaptura.Rows.Count > 0)
                {
                    for (int i = 0; i < dtDetalleCaptura.Rows.Count; i++)
                    {
                        dtDetalleCapturaPorRegistro = new DataTable("DetallePorRegistro");
                        //agregamos columnas al dt por registro
                        for (int j = 0; j < dtDetalleCaptura.Columns.Count; j++)
                        {
                            dtDetalleCapturaPorRegistro.Columns.Add(new DataColumn( dtDetalleCaptura.Columns[j].ColumnName));
                        }
                        DataRow row = dtDetalleCaptura.NewRow();

                        dtDetalleCapturaPorRegistro.ImportRow(dtDetalleCaptura.Rows[i]);
                        dtDetalleCapturaPorRegistro.Columns.Remove("Resultado");
                        //dtDetalleCapturaPorRegistro.Rows[0][0] = dtDetalleCaptura.Rows[i][0];
                        //dtDetalleCapturaPorRegistro.Rows[0][1] = dtDetalleCaptura.Rows[i][1];
                        //dtDetalleCapturaPorRegistro.Rows[0][2] = dtDetalleCaptura.Rows[i][2];
                        //dtDetalleCapturaPorRegistro.Rows[0][3] = dtDetalleCaptura.Rows[i][3];

                        dtDetalleCaptura.Rows[i]["Resultado"]= SistemaPinturaAplicableBD.Instance.InsertaCapturaSistemaPinturaAplicableMasivo(dtDetalleCapturaPorRegistro, usuario.UsuarioID, TipoCarga, Lenguaje, ProyectoID);
                    }
                    if (TipoCarga == 1)
                        dtDetalleCaptura.Columns.RemoveAt(1);
                    else if(TipoCarga == 2)
                        dtDetalleCaptura.Columns.RemoveAt(0);
                    dtDetalleCaptura.Columns[0].ColumnName = "Elemento";
                    return ToDataTable.table_to_csv(dtDetalleCaptura);
                }
                else
                return dtDetalleCapturaPorRegistro = new DataTable("DetallePorRegistro");


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