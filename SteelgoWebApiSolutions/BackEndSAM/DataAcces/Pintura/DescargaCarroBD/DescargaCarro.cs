using BackEndSAM.Models.Pintura.DescargaCarro;
using DatabaseManager.Constantes;
using DatabaseManager.Sam3;
using SecurityManager.Api.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BackEndSAM.DataAcces.Pintura.DescargaCarroBD
{
    public class DescargaCarroBD
    {
        private static readonly object _mutex = new Object();
        private static DescargaCarroBD _instance;

        public static DescargaCarroBD Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {

                        _instance = new DescargaCarroBD();
                    }
                }
                return _instance;
            }
        }

        public object ObtieneDetalle(int carroID,string lenguaje)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    List<Sam3_Pintura_DescargaCarro_DetalleCarroCerrado_Result> result = ctx.Sam3_Pintura_DescargaCarro_DetalleCarroCerrado(carroID,lenguaje).ToList();

                    List<DetalleCarro> ListadoDetalleSpool = new List<DetalleCarro>();

                    foreach (Sam3_Pintura_DescargaCarro_DetalleCarroCerrado_Result item in result)
                    {
                        ListadoDetalleSpool.Add(new DetalleCarro
                        {
                            Color = item.Color,
                            M2 = item.Area,
                            CuadranteID = item.CuadranteID,
                            SpoolID=item.SpoolID,
                            NombreCuadrante = item.Cuadrante,
                            NombreSpool = item.NumeroControl,
                            SistemaPintura = item.SistemaPintura,
                            Modificado = false
                        });
                    }
                    return ListadoDetalleSpool;
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

        public object Guardar(DataTable dtDetalleCaptura, int usuario, string lenguaje,string cargaCarroID,int carroID,int cerrarCarro)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    ObjetosSQL _SQL = new ObjetosSQL();
                    string[,] parametro = { { "@Usuario", usuario.ToString() }, { "@Lenguaje", lenguaje }, { "@cargaCarroID", cargaCarroID }, { "@CarroID", carroID.ToString() }, { "@CerrarCarro", cerrarCarro.ToString() } };

                    _SQL.Ejecuta(Stords.GUARDARCAPTURADECARGACARRO, dtDetalleCaptura, "@DetalleGuardado", parametro);

                    TransactionalInformation result = new TransactionalInformation();
                    result.ReturnMessage.Add("Ok");

                    result.ReturnCode = 200;
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
                result.IsAuthenicated = true;

                return result;
            }
        }
    }
}